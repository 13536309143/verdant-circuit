using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verdant.Automation;
public static partial class VerdantFullPlaythrough
{
    static bool universeFresh;static ArchipelagoWorld freshWorld;static int freshShip;static readonly Dictionary<int,int> freshWorkers=new Dictionary<int,int>();static readonly List<Stock> freshOrders=new List<Stock>();
    static string BoundedCactusSort()
    {
        return "for pass_id in range(get_world_size()):\n"+Indent(Sweep("if get_pos_x()+1<get_world_size() and measure()>measure(East):\n    swap(East)"),4)+"\nfor pass_id in range(get_world_size()):\n"+Indent(Sweep("if get_pos_y()+1<get_world_size() and measure()>measure(North):\n    swap(North)"),4)+"\nharvest()";
    }
    public static void RunUniverseFresh(){UnityEditor.SessionState.SetBool("VerdantUniverseFresh",true);Run();}
    static void RunUniverseFreshCore()
    {
        UnityEngine.Localization.Settings.LocalizationSettings.InitializationOperation.WaitForCompletion();UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetTableAsync("Verdant").WaitForCompletion();
        output=Path.GetFullPath(Environment.GetEnvironmentVariable("VERDANT_FRESH_OUTPUT")??"InternalTestReports/0.13.2/fresh");Directory.CreateDirectory(output);Directory.CreateDirectory(Path.Combine(output,"programs"));Directory.CreateDirectory(Path.Combine(output,"saves"));
        uint seed=731;uint.TryParse(Environment.GetEnvironmentVariable("VERDANT_FRESH_SEED")??"731",out seed);
        wall=Stopwatch.StartNew();frames=0;resourceDepth=0;universeFresh=true;freshWorkers.Clear();freshOrders.Clear();
        report=new Report{seed=seed,edition="0.13.2 / fresh zero-inventory Earth + mothership + 24 universe projects",unityVersion=Application.unityVersion,startedUtc=DateTime.UtcNow.ToString("o")};
        string source=string.Join("\n",Directory.GetFiles("Assets/VerdantCircuit/Scripts/Automation","*.cs").OrderBy(p=>p).Select(File.ReadAllText));using(var hash=System.Security.Cryptography.SHA256.Create())report.sourceFingerprint=BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(source))).Replace("-","").ToLowerInvariant();
        farm=new ClassicFarm(seed);farm.Data.programs.Add(new ClassicProgram("main","harvest()"));observerObject=new GameObject("Fresh Universe Reference Observer");observer=observerObject.AddComponent<ClassicController>();observer.enabled=false;observer.Farm=farm;observer.SavePath=Path.Combine(output,"final-farm.json");production=typeof(ClassicController).GetMethod("UpdateProduction",BindingFlags.Instance|BindingFlags.NonPublic);challengeNotice=typeof(ClassicController).GetMethod("NotifyChallengeCompleted",BindingFlags.Instance|BindingFlags.NonPublic);farm.Message+=observer.Log;
        try
        {
            Check(farm.Data.size==1&&farm.Data.height==1&&farm.Data.unlocks.Count==0&&ClassicFarm.Items.All(i=>farm.Count(i)==0),"Fresh start must have zero inventory and research.");Boot();
            foreach(string id in new[]{"operators","variables","senses","plant","expand","trees","functions","lists","carrots","watering","dicts","import","costs","timing","debug","debug2","auto_unlock","pumpkins","fertilizer","sunflowers","cactus","mazes","dinosaurs","megafarm","spaceflight","flower_planet","mineral_planet","ocean_planet"}){Purchase(id);UpgradeAvailable();UnityEngine.Debug.Log("FRESH RESEARCH / "+id+" / "+farm.Data.time);}
            freshWorld=farm.Fleet;Ensure("wood",2000);freshWorld.Sell(0,"wood",1000);Ensure("solar_energy",2000);while(farm.Count("water")<40)FreshTick();
            for(int p=0;p<4;p++)
            {
                if(p>0){int starter=freshWorld.EnsurePlanetWorker(p);freshWorkers[p]=starter;string initial=p==1?"rose":p==2?"carp":"stone";if(p==1){FreshAcquire(0,"water",80);FreshTransfer(0,p,"water",80);FreshAcquire(0,"compost",40);FreshTransfer(0,p,"compost",40);}if(p==2){FreshAcquire(0,"fish_feed",60);FreshTransfer(0,p,"fish_feed",60);}
                    while(freshWorld.SurfaceWidth(p)<8){FreshAcquire(p,initial,20*(freshWorld.Data.islands[p].expansion+1)*(freshWorld.Data.islands[p].expansion+1));Check(freshWorld.ExpandPlanet(p),"Paid surface expansion");}
                }
                else freshWorkers[p]=freshWorld.EnsurePlanetWorker(p);
                string material=p==0?"hay":p==1?"rose":p==2?"carp":"stone";
                for(int level=0;level<2;level++){FreshAcquire(p,material,40*(level+1));Check(freshWorld.ResearchWorker(p,"size"),"Paid hull research");}
                int old=freshWorkers[p];freshWorld.Remove(old);int unit=freshWorld.Build(ArchipelagoWorld.Roles[p],2,2,p);Check(unit>0,"Paid native 2x2 worker");freshWorkers[p]=unit;
                string program="reference_observer_"+p;farm.Data.programs.Add(new ClassicProgram(program,"while True:\n    worker_wait(10)"));Check(freshWorld.RunProgram(unit,program),"Independent worker interpreter");
            }
            freshShip=FreshShip();
            for(int p=1;p<4;p++)FreshTransfer(0,p,"solar_energy",500);
            while(!freshWorld.Complete)
            {
                int order=freshWorld.Data.order;var phase=Begin("universe-project",freshWorld.Current.zh,freshWorld.Current.lessonZh+" Reference policy: one 2x2 worker per planet, recursive input planning, real routed UFOs. No idle timing gates.");var timer=Stopwatch.StartNew();
                int reconcile=0;while(!freshWorld.Ready&&reconcile++<20){foreach(var g in freshWorld.Current.goals)FreshAcquire(g.island,g.item,g.amount);}
                Check(freshWorld.Submit(),"Reference project was not deliverable: "+order);freshOrders.Add(new Stock("project_"+(order+1),farm.Data.time-phase.gameStart));End(phase,timer);Check(freshWorld.Audit()=="",freshWorld.Audit());
                UnityEngine.Debug.Log("FRESH PROJECT / "+(order+1)+" / "+farm.Data.time);
                if(order==2){FreshAcquire(0,"bronze",10);Check(freshWorld.ResearchUfo("cargo"),"Paid payload upgrade");}
                if(order==7){FreshAcquire(0,"silver",20);Check(freshWorld.ResearchUfo("cargo"),"Paid second payload upgrade");}
                if(order==11){FreshAcquire(0,"platinum",30);Check(freshWorld.ResearchUfo("cargo"),"Paid third payload upgrade");}
                if(order==8||order==15)
                foreach(var t in ArchipelagoWorld.PlanetTechnologies){if(t.parent!=""&&freshWorld.PlanetLevel(t.planet,t.parent)==0)continue;FreshAcquire(t.planet==-1?0:t.planet,t.item,freshWorld.TechnologyCost(t));Check(freshWorld.ResearchPlanet(t.planet,t.id),"Paid specialist research");}
            }
            Check(freshWorld.Data.order==24,"All twenty-four projects must complete.");Check(freshWorld.Data.hubVisits>0,"Routes must use the physical mothership.");
            File.WriteAllText(Path.Combine(output,"universe-orders.json"),JsonUtility.ToJson(new FreshOrders{status="passed",orders=freshOrders.ToArray(),shipments=freshWorld.Data.shipments,hubVisits=freshWorld.Data.hubVisits,workerPolicy="One paid 2x2 native worker per planet. Autonomous production and transport planning; not an optimized eight-worker speedrun.",zeroInventory=true,resourceGrants=false},true));
            report.status="passed";Finish();UnityEngine.Debug.Log("FRESH UNIVERSE PASSED / "+report.gameSeconds+"s / "+wall.Elapsed.TotalSeconds+"s wall");
        }
        catch(Exception e){report.status="failed";report.error=e.ToString();Finish();throw;}
        finally{universeFresh=false;UnityEngine.Object.DestroyImmediate(observerObject);}
    }
    [Serializable] public sealed class FreshOrders {public string status,workerPolicy;public bool zeroInventory,resourceGrants;public int shipments,hubVisits;public Stock[] orders;}
    static void FreshTick(){farm.Advance(.25);if(wall.Elapsed.TotalSeconds>2400)throw new Exception("Reference wall-time budget exceeded");}
    static int FreshShip(){if(freshShip>0)return freshShip;Ensure("wood",20);Ensure("solar_energy",10);freshShip=freshWorld.BuildUfo();Check(freshShip>0,"Paid UFO build");return freshShip;}
    static void FreshWaitShip(){int id=FreshShip();for(int i=0;i<10000000;i++){if(freshWorld.Data.ufos.First(u=>u.id==id).phase==0)return;FreshTick();}throw new Exception("UFO did not dock");}
    static void FreshTransfer(int from,int to,string item,int amount)
    {
        int id=FreshShip();var u=freshWorld.Data.ufos.First(v=>v.id==id);int remaining=amount;
        while(remaining>0)
        {
            if(u.battery<60){if(freshWorld.Stock(u.planet,"solar_energy")<100){Check(freshWorld.RepositionUfo(id,0),"Return for local fuel");FreshWaitShip();Ensure("solar_energy",100);}Check(freshWorld.RefuelUfo(id,100),"Pay local refueling");}
            int packet=Math.Min(remaining,freshWorld.UfoCapacity*8);Check(freshWorld.LaunchUfo(id,from,to,item,packet),"Launch physical transfer "+item+" "+from+"→"+to);FreshWaitShip();remaining-=packet;
        }
    }
    static void FreshAcquire(int planet,string item,int amount,int depth=0)
    {
        if(depth>20)throw new Exception("Recursive production loop: "+item);int missing=amount-(int)freshWorld.Stock(planet,item);if(missing<=0)return;
        var recipe=ArchipelagoWorld.Recipes.FirstOrDefault(r=>r.island==planet&&r.item==item);
        if(recipe==null)
        {
            var origin=ArchipelagoWorld.Recipes.FirstOrDefault(r=>r.item==item);int from=origin?.island??0;
            if(from==planet){if(item=="water"||item=="fertilizer"){while(farm.Count(item)<amount)FreshTick();return;}Ensure(item,amount);return;}
            FreshAcquire(from,item,missing,depth+1);FreshTransfer(from,planet,item,missing);return;
        }
        if(!freshWorkers.ContainsKey(planet))freshWorkers[planet]=freshWorld.EnsurePlanetWorker(planet);
        int id=freshWorkers[planet];var worker=freshWorld.Unit(id);int area=worker.width*worker.height,cycles=(int)Math.Ceiling(missing/(double)freshWorld.ProductionOutput(recipe,area));
        int guard=0;while(recipe.inputs.Any(i=>freshWorld.Stock(planet,i.Key)<i.Value*area*cycles))
        {if(++guard>30)throw new Exception("Recipe inputs did not converge: "+item);foreach(var input in recipe.inputs)FreshAcquire(planet,input.Key,input.Value*area*cycles,depth+1);}
        Check(freshWorld.Work(id,planet,item,cycles),"Enqueue "+item);int ticks=0;
        while(worker.jobs.Count>0){FreshTick();if(++ticks>20000000)throw new Exception("Production stalled: "+item+" / "+worker.status);}
        Check(freshWorld.Stock(planet,item)>=amount,"Produced quantity mismatch: "+item);
    }
}
