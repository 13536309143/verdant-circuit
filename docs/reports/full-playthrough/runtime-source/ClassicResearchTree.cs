using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Verdant.Automation
{
    public static class ClassicResearchLayout
    {
        public static readonly Dictionary<string,Vector2> Positions=new Dictionary<string,Vector2>{
            {"plant",new Vector2(60,360)},{"grass",new Vector2(370,170)},{"expand",new Vector2(370,360)},{"trees",new Vector2(370,550)},
            {"speed",new Vector2(680,170)},{"carrots",new Vector2(680,360)},{"polyculture",new Vector2(680,550)},
            {"watering",new Vector2(990,170)},{"pumpkins",new Vector2(990,450)},{"sunflowers",new Vector2(1300,170)},
            {"fertilizer",new Vector2(1300,360)},{"cactus",new Vector2(1300,550)},{"mazes",new Vector2(1610,360)},{"dinosaurs",new Vector2(1610,550)},
            {"megafarm",new Vector2(1920,360)},{"simulation",new Vector2(1920,550)},{"leaderboard",new Vector2(2230,550)},
            {"the_farmers_remains",new Vector2(2540,550)},{"top_hat",new Vector2(2850,550)},
            {"loops",new Vector2(60,950)},{"operators",new Vector2(370,950)},{"variables",new Vector2(680,950)},
            {"functions",new Vector2(990,950)},{"lists",new Vector2(1300,850)},{"dicts",new Vector2(1610,850)},
            {"import",new Vector2(1300,1050)},{"utilities",new Vector2(680,1150)},
            {"senses",new Vector2(60,1480)},{"costs",new Vector2(370,1480)},{"auto_unlock",new Vector2(680,1480)},{"timing",new Vector2(990,1480)},
            {"debug",new Vector2(60,1690)},{"debug2",new Vector2(370,1690)},{"hats",new Vector2(680,1690)}};
        public static int Lane(string id){return Positions[id].y<750?0:Positions[id].y<1400?1:2;}
    }
    public sealed class ClassicResearchNode:MonoBehaviour {public string id;}

    public sealed partial class ClassicController
    {
        string selectedResearch;
        ClassicTreeScroll researchMap;
        TMP_Text researchTitle,researchDescription,researchCost,researchRequirement,researchCode,researchLevel,researchMessage;
        UnityEngine.UI.Button researchAction;
        RectTransform researchLevelFill;
        const float NodeWidth=240,NodeHeight=132;

        RectTransform GraphRect(string name,Transform parent,Vector2 position,Vector2 size,Color color,bool round=false)
        {
            var rect=round?Surface(name,parent,new Vector2(0,1),new Vector2(0,1),color):Panel(name,parent,new Vector2(0,1),new Vector2(0,1),color);
            rect.pivot=new Vector2(0,1);rect.anchoredPosition=new Vector2(position.x,-position.y);rect.sizeDelta=size;return rect;
        }
        void FocusResearchLane(int lane)
        {
            if(lane==0){researchMap.Fit();return;}
            if(selectedResearch==null||ClassicResearchLayout.Lane(selectedResearch)!=lane-1)selectedResearch=lane==1?"plant":lane==2?"loops":"senses";
            researchMap.Focus(new Vector2(0,lane==1?90:lane==2?780:1410),.78f);Refresh();
        }
        void TickResearchNavigation()
        {
            if(modalKind!="research"||researchMap==null)return;
            var focused=UnityEngine.EventSystems.EventSystem.current?.currentSelectedGameObject;if(focused!=null&&focused.GetComponent<TMP_InputField>()!=null)return;
            if(Input.GetKeyDown(KeyCode.Alpha1))FocusResearchLane(0);
            if(Input.GetKeyDown(KeyCode.Alpha2))FocusResearchLane(1);
            if(Input.GetKeyDown(KeyCode.Alpha3))FocusResearchLane(2);
            if(Input.GetKeyDown(KeyCode.Alpha4))FocusResearchLane(3);
        }
        void BuildResearchTree()
        {
            modal=Panel("Research Overlay",canvas,Vector2.zero,Vector2.one,new Color(.04f,.12f,.10f,.72f));modal.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;
            var window=Surface("Research Tree",modal,new Vector2(.072f,.035f),new Vector2(.985f,.985f),Paper,true);
            Label(Tr("THE DISCOVERY TREE","发现之树"),window,new Vector2(.025f,.925f),new Vector2(.76f,.985f),31,Ink);
            Label(Tr("Grow the field. Grow the language. Connect the possibilities.","让农田生长，也让语言生长。每一条连线，都是新的可能。"),window,new Vector2(.026f,.875f),new Vector2(.84f,.925f),15,Muted);
            Skin(Button(Tr("Close","关闭"),window,new Vector2(.9f,.934f),new Vector2(.978f,.98f),Close),Dark,Color.white);
            string[] labels={Tr("Overview","全览"),Tr("Cultivation","种植与产能"),Tr("Programming","编程语言"),Tr("Automation","自动化工具")};
            for(int i=0;i<labels.Length;i++)
            {
                int lane=i;Skin(Button(labels[i],window,new Vector2(.025f+i*.155f,.812f),new Vector2(.17f+i*.155f,.858f),()=>
                {FocusResearchLane(lane);}),i==0?Accent:Dark,Color.white);
            }
            var search=MakeInput("Research Search",window,new Vector2(.66f,.812f),new Vector2(.973f,.858f),false);search.characterLimit=64;
            var placeholder=Label(Tr("Search a name, then Enter…","输入节点名称后按回车…"),search.transform,new Vector2(.03f,.1f),new Vector2(.97f,.9f),14,Muted);search.placeholder=placeholder;
            search.onEndEdit.AddListener(query=>
            {if(string.IsNullOrWhiteSpace(query))return;var found=ClassicFarm.Techs.FirstOrDefault(t=>TechName(t.id).IndexOf(query,StringComparison.OrdinalIgnoreCase)>=0||t.id.IndexOf(query,StringComparison.OrdinalIgnoreCase)>=0);if(found!=null)SelectResearch(found.id,true);else researchMessage.text=Tr("No matching discovery.","没有找到对应节点。");});

            var scrollRoot=Surface("Research Map",window,new Vector2(.025f,.075f),new Vector2(.735f,.79f),Farm.Data.theme=="night"?ClassicView.ColorOf("19372F"):ClassicView.ColorOf("E5EBDD"));
            researchMap=scrollRoot.gameObject.AddComponent<ClassicTreeScroll>();
            var viewport=Panel("Viewport",scrollRoot,Vector2.zero,Vector2.one,Color.clear);viewport.gameObject.AddComponent<RectMask2D>();viewport.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;
            var content=GraphRect("Research Graph",viewport,Vector2.zero,new Vector2(3160,1900),Color.clear);
            researchMap.viewport=viewport;researchMap.content=content;researchMap.horizontal=researchMap.vertical=true;researchMap.movementType=ScrollRect.MovementType.Clamped;researchMap.decelerationRate=.08f;researchMap.scrollSensitivity=20;
            for(int x=0;x<3160;x+=155)GraphRect("Grid",content,new Vector2(x,0),new Vector2(1,1900),new Color(.25f,.45f,.32f,.08f));
            for(int y=0;y<1900;y+=95)GraphRect("Grid",content,new Vector2(0,y),new Vector2(3160,1),new Color(.25f,.45f,.32f,.08f));
            string[] laneTitles={Tr("01 / CULTIVATION & YIELD","01 / 种植与产能"),Tr("02 / THE LANGUAGE OF AUTOMATION","02 / 自动化的语言"),Tr("03 / SENSE, MEASURE, REFINE","03 / 感知、测量与优化")};
            int[] laneTops={45,760,1390};
            for(int i=0;i<3;i++){var heading=GraphRect("Lane Heading",content,new Vector2(60,laneTops[i]),new Vector2(2950,60),Color.clear);Label(laneTitles[i],heading,Vector2.zero,Vector2.one,28,i==1?ClassicView.ColorOf("B47E4E"):Accent);}
            foreach(var tech in ClassicFarm.Techs)if(tech.prerequisite!="")BuildResearchLink(content,tech.prerequisite,tech.id);
            foreach(var tech in ClassicFarm.Techs)BuildResearchNode(content,tech);

            var detail=Surface("Research Inspector",window,new Vector2(.752f,.075f),new Vector2(.975f,.79f),Farm.Data.theme=="night"?ClassicView.ColorOf("1D3A32"):ClassicView.ColorOf("E6E9DE"));
            Label(Tr("SELECTED DISCOVERY","当前发现"),detail,new Vector2(.065f,.91f),new Vector2(.935f,.97f),12,Accent);
            researchTitle=Label("",detail,new Vector2(.065f,.78f),new Vector2(.935f,.91f),27,Ink);
            researchLevel=Label("",detail,new Vector2(.065f,.72f),new Vector2(.935f,.78f),14,Muted);
            var track=Panel("Level Track",detail,new Vector2(.065f,.696f),new Vector2(.935f,.703f),Muted);researchLevelFill=Panel("Level",track,Vector2.zero,Vector2.one,Accent);
            researchDescription=Label("",detail,new Vector2(.065f,.53f),new Vector2(.935f,.68f),17,Ink);researchDescription.enableWordWrapping=true;
            var example=Surface("Syntax Example",detail,new Vector2(.065f,.39f),new Vector2(.935f,.52f),Dark);
            researchCode=Label("",example,new Vector2(.07f,.08f),new Vector2(.94f,.92f),14,Lime);researchCode.richText=false;
            researchRequirement=Label("",detail,new Vector2(.065f,.275f),new Vector2(.935f,.38f),15,Ink);
            researchCost=Label("",detail,new Vector2(.065f,.16f),new Vector2(.935f,.275f),16,Ink);
            researchAction=Skin(Button(Tr("Research","研究"),detail,new Vector2(.065f,.065f),new Vector2(.935f,.145f),()=>
            {
                if(selectedResearch!=null&&Farm.Unlock(selectedResearch)){Save();researchMessage.text=Tr("Discovery unlocked: ","已解锁：")+TechName(selectedResearch);Refresh();}
            }),Accent,Color.white);
            researchMessage=Label("",window,new Vector2(.025f,.017f),new Vector2(.735f,.06f),13,Accent);
            var zoomLabel=Label("",window,new Vector2(.84f,.017f),new Vector2(.9f,.06f),13,Muted);zoomLabel.alignment=TextAlignmentOptions.Center;
            Skin(Button("−",window,new Vector2(.758f,.017f),new Vector2(.795f,.06f),()=>researchMap.SetZoom(researchMap.Zoom/1.2f)),Dark,Color.white);
            Skin(Button("+",window,new Vector2(.80f,.017f),new Vector2(.837f,.06f),()=>researchMap.SetZoom(researchMap.Zoom*1.2f)),Dark,Color.white);
            Skin(Button(Tr("Fit","适应"),window,new Vector2(.91f,.017f),new Vector2(.975f,.06f),()=>researchMap.Fit()),Dark,Color.white);
            modalRefreshers.Add(()=>{if(zoomLabel!=null)zoomLabel.text=Mathf.RoundToInt(researchMap.Zoom*100)+"%";RefreshResearchInspector();});
            researchMessage.text=Tr("Drag to pan · Scroll to zoom · 1–4: switch areas · Click nodes for details.","拖动平移 · 滚轮缩放 · 数字 1–4 切换区域 · 点击节点查看详情。");
            SelectResearch(selectedResearch!=null&&ClassicResearchLayout.Positions.ContainsKey(selectedResearch)?selectedResearch:"plant",true);
        }
        void BuildResearchLink(Transform content,string parent,string child)
        {
            var a=ClassicResearchLayout.Positions[parent]+new Vector2(NodeWidth,NodeHeight*.5f);var b=ClassicResearchLayout.Positions[child]+new Vector2(0,NodeHeight*.5f);float mid=(a.x+b.x)*.5f;
            var pieces=new List<UnityEngine.UI.Image>();
            pieces.Add(GraphRect("Dependency "+parent+" → "+child,content,a,new Vector2(mid-a.x,3),Muted).GetComponent<UnityEngine.UI.Image>());
            if(Math.Abs(a.y-b.y)>1)pieces.Add(GraphRect("Dependency Bend",content,new Vector2(mid,Math.Min(a.y,b.y)),new Vector2(3,Math.Abs(a.y-b.y)+3),Muted).GetComponent<UnityEngine.UI.Image>());
            pieces.Add(GraphRect("Dependency End",content,new Vector2(mid,b.y),new Vector2(b.x-mid,3),Muted).GetComponent<UnityEngine.UI.Image>());
            var tip=GraphRect("Dependency Tip",content,b-new Vector2(4,3),new Vector2(7,7),Muted);tip.localRotation=Quaternion.Euler(0,0,45);pieces.Add(tip.GetComponent<UnityEngine.UI.Image>());
            modalRefreshers.Add(()=>{foreach(var line in pieces)if(line!=null)line.color=selectedResearch==child?Clay:Farm.Level(parent)>0?Accent:Muted;});
        }
        void BuildResearchNode(Transform content,ClassicTech tech)
        {
            var node=GraphRect("Research Node "+tech.id,content,ClassicResearchLayout.Positions[tech.id],new Vector2(NodeWidth,NodeHeight),Paper,true);
            node.gameObject.AddComponent<ClassicResearchNode>().id=tech.id;
            var image=node.GetComponent<UnityEngine.UI.Image>();image.raycastTarget=true;
            var button=node.gameObject.AddComponent<UnityEngine.UI.Button>();button.targetGraphic=image;button.onClick.AddListener(()=>SelectResearch(tech.id,false));
            var border=node.gameObject.AddComponent<UnityEngine.UI.Outline>();border.effectDistance=new Vector2(2,-2);border.useGraphicAlpha=true;
            var status=Label("",node,new Vector2(.065f,.72f),new Vector2(.935f,.94f),11,Muted);
            var title=Label(TechName(tech.id),node,new Vector2(.065f,.36f),new Vector2(.935f,.72f),21,Ink);
            var price=Label("",node,new Vector2(.065f,.07f),new Vector2(.935f,.35f),13,Muted);
            modalRefreshers.Add(()=>
            {
                if(node==null)return;var cost=Farm.Cost(tech.id);bool reachable=CanReachResearch(tech.id),affordable=cost!=null&&reachable&&cost.All(p=>Farm.Count((string)p.Key)>=ScriptValue.Number(p.Value));int level=Farm.Level(tech.id);
                image.color=affordable?Accent:level>0?ClassicView.ColorOf("D5E2C6"):Paper;
                title.color=affordable?Color.white:level>0?ClassicView.ColorOf("243C33"):Ink;
                status.color=price.color=affordable?Lime:level>0?Accent:Muted;
                status.text=cost==null?Tr("COMPLETE","已完成"):affordable?Tr("READY TO RESEARCH","可以研究"):!reachable?Tr("PREREQUISITE LOCKED","前置未解锁"):Tr("GATHER RESOURCES","收集资源");
                price.text=level+" / "+tech.max+(cost==null?"":"  ·  "+string.Join(" + ",cost.Select(p=>ItemName((string)p.Key)+" "+ScriptValue.Show(p.Value))));
                border.effectColor=tech.id==selectedResearch?Clay:new Color(0,0,0,0);
            });
        }
        void SelectResearch(string id,bool focus)
        {
            selectedResearch=id;
            if(focus){Canvas.ForceUpdateCanvases();var p=ClassicResearchLayout.Positions[id];researchMap.Focus(new Vector2(Math.Max(0,p.x-250),ClassicResearchLayout.Lane(id)==0?90:ClassicResearchLayout.Lane(id)==1?780:1410),.78f);}
            Refresh();
        }
        void RefreshResearchInspector()
        {
            if(researchTitle==null||selectedResearch==null)return;
            var tech=ClassicFarm.Techs.First(t=>t.id==selectedResearch);var cost=Farm.Cost(tech.id);
            researchTitle.text=TechName(tech.id);researchDescription.text=ResearchEffect(tech.id);researchCode.text=ResearchExample(tech.id);
            researchLevel.text=Tr("Level ","等级 ")+Farm.Level(tech.id)+" / "+tech.max;
            researchLevelFill.anchorMax=new Vector2((float)Farm.Level(tech.id)/tech.max,1);
            researchRequirement.text=Tr("Prerequisite: ","前置：")+(tech.prerequisite==""?Tr("Starting discovery","起始节点"):TechName(tech.prerequisite))+"\n"+(CanReachResearch(tech.id)?Tr("Route unlocked","研究路线已开放"):Tr("Unlock the connected parent first.","请先研究连线指向的前置节点。"));
            researchCost.text=cost==null?Tr("Fully researched","已完成全部研究"):string.Join("\n",cost.Select(p=>ItemName((string)p.Key)+"  "+Farm.Count((string)p.Key).ToString("N0")+" / "+ScriptValue.Show(p.Value)));
            researchAction.interactable=cost!=null&&CanReachResearch(tech.id)&&cost.All(p=>Farm.Count((string)p.Key)>=ScriptValue.Number(p.Value));
            researchAction.GetComponentInChildren<TMP_Text>().text=cost==null?Tr("Complete","已完成"):researchAction.interactable?Tr("Research","研究"):CanReachResearch(tech.id)?Tr("More resources needed","物资不足"):Tr("Prerequisite required","需要前置研究");
        }

        static string ResearchEffect(string id)
        {
            switch(id)
            {
                case "plant":return Tr("Plant free bushes and begin shaping the field.","开放灌木种植，开始安排田间布局。");
                case "expand":return Tr("Double the field edge per level, up to 32×32.","每级扩大农田边长，最大达到 32×32。");
                case "speed":return Tr("Double instruction and action speed per level.","每级加倍指令与动作执行速度。");
                case "grass":return Tr("Increase the hay yield from each grass harvest.","提高每次收获草的干草产量。");
                case "trees":return Tr("Unlock trees and improve wood production. Leave space between trees.","开放树木并提高木材产量；合理留出间距可改善生长。");
                case "carrots":return Tr("Grow carrots on tilled soil using hay and wood.","在耕地上消耗干草与木材种植胡萝卜。");
                case "watering":return Tr("Generate water supplies and accelerate watered crops.","持续产生水资源，灌溉可以加快作物生长。");
                case "pumpkins":return Tr("Repair dead plants and grow complete squares for giant harvests.","补种枯死地块，培育完整南瓜方阵获得巨型丰收。");
                case "sunflowers":return Tr("Measure petals and prioritize the largest flowers to produce power.","测量花瓣，优先收获最大向日葵来生产能量。");
                case "fertilizer":return Tr("Produce fertilizer, accelerate growth and explore infection.","产生肥料，加快生长，并开放感染产物的玩法。");
                case "polyculture":return Tr("Improve the yield bonus for satisfying companion requests.","提高满足伴生偏好时的额外收获倍率。");
                case "cactus":return Tr("Measure sizes, swap neighbors and sort a field for chain harvests.","测量尺寸、交换邻格，将仙人掌排序后连锁收获。");
                case "mazes":return Tr("Turn bushes into hedge mazes and search for treasure.","将灌木变成树篱迷宫，编写算法寻找宝藏。");
                case "dinosaurs":return Tr("Explore apple targets with a growing tail to produce bone.","追踪苹果目标，避开增长的尾巴，生产骨头。");
                case "megafarm":return Tr("Double independent drone capacity per level, up to 32 workers.","每级加倍独立无人机容量，最多 32 台工作机。");
                case "simulation":return Tr("Evaluate programs on separate farms with repeatable seeds.","在独立农场中用固定种子验证程序和策略。");
                case "leaderboard":return Tr("Measure strategies in local fixed-start competitions.","在固定开局的本地竞速中比较策略耗时。");
                case "loops":return Tr("Unlock repeated execution with while, for and repeat.","解锁 while、for 与 repeat，让重复工作自动执行。");
                case "operators":return Tr("Unlock arithmetic, comparisons and conditional decisions.","解锁运算、比较和条件判断，按状态决定动作。");
                case "variables":return Tr("Name values, assign results and keep state in your programs.","为数值命名、保存结果，并在程序中记录状态。");
                case "functions":return Tr("Define reusable functions with parameters and returned values.","定义带参数和返回值的函数，复用你的算法。");
                case "lists":return Tr("Organize sequences with lists, tuples, indices and unpacking.","使用列表、元组、索引与解包组织一组数据。");
                case "dicts":return Tr("Map keys to values and track unique coordinates with sets.","使用字典映射数据，用集合记录不重复的坐标。");
                case "import":return Tr("Import other programs as modules to organize larger systems.","把其他程序作为模块导入，组织更复杂的系统。");
                case "senses":return Tr("Read position, crop state, water and companion requests.","读取位置、作物状态、水分和伴生偏好。");
                case "costs":return Tr("Query the exact costs of seeds and future research levels.","查询种植物资与各级研究的准确成本。");
                case "auto_unlock":return Tr("Let code purchase research when its requirements are met.","条件满足时，让程序自动购买研究。");
                case "timing":return Tr("Measure elapsed game time and instruction counts.","测量游戏时间和指令次数，用数据改进算法。");
                case "debug":return Tr("Print values to the field journal while testing code.","测试代码时，在田野日志中输出变量和值。");
                case "debug2":return Tr("Temporarily change field size and limit execution speed.","临时调整农田大小或限制执行速度，方便调试。");
                case "utilities":return Tr("Use random values and numeric conversion utilities.","使用随机数与数值转换工具。");
                case "hats":return Tr("Choose a new appearance for your drone.","为无人机选择不同的外观。");
                case "the_farmers_remains":return Tr("Build an autonomy beacon as a late-game research milestone.","建造自治信标，完成后期研究里程碑。");
                case "top_hat":return Tr("Unlock the master hat after completing the autonomy beacon.","完成自治信标后，解锁大师礼帽。");
                default:return id;
            }
        }
        static string ResearchExample(string id)
        {
            switch(id)
            {
                case "loops":return "while True:\n    harvest()";
                case "operators":return "if can_harvest():\n    harvest()";
                case "variables":return "size = get_world_size()";
                case "functions":return "def collect():\n    return harvest()";
                case "lists":return "route = [North, East]\nfirst = route[0]";
                case "dicts":return "visited = set()\nvisited.add((0, 0))";
                case "import":return "import helpers\nhelpers.collect()";
                case "utilities":return "value = int(random()*10)";
                case "megafarm":return "handle = spawn_drone(work)\nwait_for(handle)";
                case "costs":return "get_cost(Unlocks.Speed)";
                case "auto_unlock":return "unlock(Unlocks.Speed)";
                case "simulation":return "simulate('experiment',\n         Unlocks, {}, {}, 7)";
                case "debug":return "print(get_pos_x())";
                case "debug2":return "set_world_size(4)\nset_execution_speed(1)";
                case "timing":return "start = get_time()";
                default:return "# "+id+"\nnum_unlocked(Unlocks."+(id=="debug2"?"Debug_2":string.Join("_",id.Split('_').Select(s=>char.ToUpperInvariant(s[0])+s.Substring(1))))+")";
            }
        }
    }
}
