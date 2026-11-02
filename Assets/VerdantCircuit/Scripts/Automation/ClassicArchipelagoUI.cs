using System;
using System.Linq;
using TMPro;
using UnityEngine;
namespace Verdant.Automation
{
    public sealed partial class ClassicController
    {
        int fleetSize,ufoSelected=-1,ufoSource,ufoDestination=1,ufoCargo;string fleetFeedback="";
        static string FleetRoleName(string id){return id=="drone"?Tr("Drone","无人机"):id=="bee"?Tr("Bee","小蜜蜂"):id=="boat"?Tr("Fishing boat","渔船"):Tr("Mining drone","采矿无人机");}
        static string CargoName(string id){return UnifiedProductNames.Name(id);}
        static string IslandName(int id){if(id==-1)return Tr("Mothership","轨道母舰");return L10n.Code=="en"?ArchipelagoWorld.Definitions[id].en:ArchipelagoWorld.Definitions[id].zh;}
        static string FleetStatus(string status){return status=="working"?Tr("Working","作业中"):status=="waiting_inputs"?Tr("Waiting for materials","等待原料"):status=="moving"?Tr("Moving","移动中"):status=="traffic_queue"?Tr("Traffic queue","等待通行"):status=="running"?Tr("Code running","代码运行中"):Tr("Idle","空闲");}
        void FleetAction(Func<bool> action){UniverseAction("archipelago",action);}
        void UniverseAction(string page,Func<bool> action){fleetFeedback=action()?Tr("Accepted.","已接受。"):Tr("Check unlocks, supplies, footprint and capacity.","请检查科技、原料、占地和数量上限。");Save();Open(page);}
        void ConsoleWindow(string name)
        {
            modal=Panel(name,canvas,new Vector2(.035f,.045f),new Vector2(.965f,.915f),Paper);modal.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;Label(name,modal,new Vector2(.025f,.91f),new Vector2(.82f,.985f),30,Ink);Button(Tr("Close","关闭"),modal,new Vector2(.88f,.925f),new Vector2(.975f,.975f),Close);
            modalBody=ProgramPane(modal,name+" Scroll",.025f,.975f);var rect=modalBody.parent.parent as RectTransform;rect.anchorMin=new Vector2(.025f,.035f);rect.anchorMax=new Vector2(.975f,.89f);
        }
        void BuildArchipelago()
        {
            var world=Farm.Fleet;ConsoleWindow(Tr("COLONY / ","殖民地／")+IslandName(activePlanet));
            var live=Row(Tr("Surface and local workers","球面与本地载具"),"",Tr("Expand surface","扩展表层"),()=>FleetAction(()=>world.ExpandPlanet(activePlanet)));
            var detail=live.GetComponentsInChildren<TMP_Text>()[1];modalRefreshers.Add(()=>detail.text=world.SurfaceWidth(activePlanet)+" × "+world.SurfaceHeight(activePlanet)+" / 360 × 128   ·   "+FleetRoleName(ArchipelagoWorld.Roles[activePlanet])+" "+world.Data.units.Count(u=>u.island==activePlanet)+" / "+world.WorkerLimit(activePlanet)+"   ·   "+Tr("Hull tier ","尺寸等级 ")+world.HullTier(activePlanet));
            Paragraph(Tr("The usable belt is 128° wide, between latitude −64° and +64°. Longitude closes only when 360 columns are unlocked. The poles and locked edges cannot be crossed.","可用纬度带共128°，位于南纬64°至北纬64°之间。横向解锁360列后才形成完整闭环；两极和未开放边界不可穿越。"),100);
            int w=ArchipelagoWorld.Sizes[fleetSize,0],h=ArchipelagoWorld.Sizes[fleetSize,1];Row(Tr("Build local worker","建造本地载具"),FleetRoleName(ArchipelagoWorld.Roles[activePlanet])+" / "+w+"×"+h+" / "+world.WorkerPrice(w,h),Tr("Build","建造"),()=>FleetAction(()=>world.Build(ArchipelagoWorld.Roles[activePlanet],w,h,activePlanet)>0));
            Row(Tr("Hull selection","选择尺寸"),w+" × "+h,Tr("Next size","下一尺寸"),()=>{fleetSize=(fleetSize+1)%5;Open("archipelago");});
            Row(Tr("Worker count","集群数量"),Tr("Independent per planet. Spend local products; Earth also retains its parallel research.","各星球分别研究，消耗当地物资；地球还保留原有并行科技。"),Tr("Research slots","研究数量"),()=>FleetAction(()=>world.ResearchWorker(activePlanet,"slots")));
            Row(Tr("Hull scale","机体规模"),Tr("1×1 → 2×1 → 2×2 → 3×2 → 3×3. Both inputs and outputs scale with area.","1×1 → 2×1 → 2×2 → 3×2 → 3×3；原料和产量均按面积放大。"),Tr("Research hull","研究尺寸"),()=>FleetAction(()=>world.ResearchWorker(activePlanet,"size")));
            foreach(var worker in world.Data.units.Where(u=>u.island==activePlanet))
            {
                var u=worker;var row=Row("#"+u.id+" / "+FleetRoleName(u.role)+" / "+u.width+"×"+u.height,"",Tr("Bind program","绑定程序"),()=>{programIsland=activePlanet;programTarget="drone:"+u.id;Open("programs");});var text=row.GetComponentsInChildren<TMP_Text>()[1];modalRefreshers.Add(()=>text.text=FleetStatus(u.status)+" / ("+u.x+","+u.y+") / "+u.program+".py"+(u.codeError==""?"":" / "+u.codeError));
            }
            Paragraph(Tr("Local production functions","本地生产函数"),50);
            foreach(var recipe in ArchipelagoWorld.Recipes.Where(r=>r.island==activePlanet)){var r=recipe;Row(CargoName(r.item),"→ "+r.amount+" / "+r.seconds+"s / "+string.Join(" + ",r.inputs.Select(v=>CargoName(v.Key)+"×"+v.Value)),Tr("Reference","函数说明"),()=>{manualQuery=r.item;manualChapter="world_products";Open("guide");});}
            var current=world.Current;if(current!=null){Paragraph(Tr("UNIVERSE CONTRACT / ","宇宙订单／")+(world.Data.order+1)+" / "+ArchipelagoWorld.Contracts.Length+" / "+(L10n.Code=="en"?current.en:current.zh),65);var goal=Row(Tr("Simultaneous delivery","联合交付"),"",Tr("Deliver","交付"),()=>FleetAction(world.Submit));var label=goal.GetComponentsInChildren<TMP_Text>()[1];modalRefreshers.Add(()=>label.text=string.Join(" · ",current.goals.Select(g=>IslandName(g.island)+" "+CargoName(g.item)+" "+world.Stock(g.island,g.item).ToString("F0")+"/"+g.amount)));}
            Paragraph(fleetFeedback,50);
        }
        void BuildUfoConsole()
        {
            var world=Farm.Fleet;ConsoleWindow(Tr("UFO / INTERPLANETARY FREIGHT","UFO／星际货运"));
            if(!world.Data.started){Row(Tr("Spaceflight research required","需要星际航行科技"),Tr("Local workers cannot travel between planets. Research Spaceflight and the target planet first.","本地载具不能跨星球；先研究星际航行及目标星球。"),Tr("Research","科技"),()=>{selectedResearch="spaceflight";Open("research");});return;}
            Row(Tr("Build UFO","建造UFO"),Tr("50 credits + 20 wood + 10 solar energy on Earth. UFO slots and payload are researched separately.","50资金＋20地球木材＋10太阳能，UFO数量和载荷分别研究。"),Tr("Build","建造"),()=>UniverseAction("ufos",()=>{int id=world.BuildUfo();ufoSelected=id;return id>0;}));
            Paragraph(Tr("Every interplanetary route docks at the mothership. Use depot −1 to store and retrieve goods there.","每条跨星球航线都停靠母舰。使用仓库编号−1，可将货物存入母舰或从母舰提货。"),80);
            foreach(var stock in world.Data.mothership.Where(v=>v.amount>0))Paragraph(IslandName(-1)+" / "+CargoName(stock.id)+" / "+stock.amount,45);
            Row(Tr("Payload","载荷"),world.UfoCapacity+Tr(" per trip","／每航次"),Tr("Upgrade","升级"),()=>UniverseAction("ufos",()=>world.ResearchUfo("cargo")));
            Row(Tr("UFO slots","UFO数量"),world.Data.ufos.Count+" / "+(world.Data.ufoSlots+1),Tr("Upgrade","升级"),()=>UniverseAction("ufos",()=>world.ResearchUfo("slots")));
            foreach(var ship in world.Data.ufos){var vessel=ship;var row=Row("UFO #"+vessel.id,"",Tr("Select","选择"),()=>{ufoSelected=vessel.id;Open("ufos");});var text=row.GetComponentsInChildren<TMP_Text>()[1];modalRefreshers.Add(()=>text.text=vessel.status+" / "+CargoName(vessel.item)+" / "+vessel.cargo+" / "+vessel.reserved+" / ETA "+vessel.timer.ToString("F1")+"s");}
            var opened=new[]{-1,0,1,2,3}.Where(world.DepotUnlocked).ToArray();if(!world.DepotUnlocked(ufoDestination))ufoDestination=opened.Last();
            Row(Tr("Source planet","出发星球"),IslandName(ufoSource),Tr("Change","切换"),()=>{ufoSource=opened[(Array.IndexOf(opened,ufoSource)+1)%opened.Length];Open("ufos");});Row(Tr("Destination planet","到达星球"),IslandName(ufoDestination),Tr("Change","切换"),()=>{ufoDestination=opened[(Array.IndexOf(opened,ufoDestination)+1)%opened.Length];Open("ufos");});
            var goods=ArchipelagoWorld.Goods.Where(item=>world.Stock(ufoSource,item)>0).ToArray();if(goods.Length>0){ufoCargo%=goods.Length;string item=goods[ufoCargo];Row(Tr("Cargo","物资"),CargoName(item)+" / "+world.Stock(ufoSource,item),Tr("Change","切换"),()=>{ufoCargo=(ufoCargo+1)%goods.Length;Open("ufos");});Row(Tr("Send one payload","发送一舱"),Tr("Stock is reserved immediately; delivery occurs only after the actual flight.","接单即预留库存，实际航行结束后才送达。"),Tr("Launch","发射"),()=>UniverseAction("ufos",()=>world.LaunchUfo(ufoSelected>0?ufoSelected:world.Data.ufos.FirstOrDefault()?.id??-1,ufoSource,ufoDestination,item,(int)Math.Min(world.UfoCapacity,world.Stock(ufoSource,item)))));}
            if(ufoSelected>0)Row(Tr("Refuel at current dock","在当前泊位补能"),Tr("Consume 20 local solar energy for 100 battery charge.","消耗当前泊位20太阳能，补充100电量。"),Tr("Refuel","补能"),()=>UniverseAction("ufos",()=>world.RefuelUfo(ufoSelected,20)));
            if(ufoSelected>0)Row(Tr("Cancel transfer","取消运输"),Tr("Unused reservations return immediately. Airborne cargo finishes its current leg safely.","未装船预留立即退回；在途货物安全完成当前航段。"),Tr("Cancel","取消"),()=>UniverseAction("ufos",()=>world.CancelUfo(ufoSelected)));
            Paragraph(fleetFeedback,50);
        }
    }
    // Retained type name for older editor-only diagnostic scripts.
    public sealed class ClassicIslandMap:UnityEngine.UI.Graphic {public ArchipelagoWorld world;public int island,selected;public Action<int> onSelect;protected override void OnPopulateMesh(UnityEngine.UI.VertexHelper vh){vh.Clear();}}
}
