using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Verdant.Automation
{
    public sealed partial class ClassicController
    {
        static Sprite roundedSurface;
        readonly Dictionary<string,TMP_Text> resourceValues=new Dictionary<string,TMP_Text>();
        readonly Dictionary<string,UnityEngine.UI.Button> navigation=new Dictionary<string,UnityEngine.UI.Button>();
        TMP_Text cropStatus;RectTransform growthStatusFill;
        TMP_Text farmSizeLabel,headerStatus,goalTitle,goalCost,discoveryCount,programSummaryLabel;
        RectTransform goalProgress;
        UnityEngine.UI.Button goalAction;
        string recommendedResearch;
        static readonly Color Lime=ClassicView.ColorOf("D2E58F"),Sea=ClassicView.ColorOf("6DB5A3"),Clay=ClassicView.ColorOf("D49369");

        static Sprite SurfaceSprite()
        {
            if(roundedSurface!=null)return roundedSurface;
            const int size=64;const float radius=14;
            var texture=new Texture2D(size,size,TextureFormat.RGBA32,false){name="Verdant Rounded Surface",filterMode=FilterMode.Bilinear,wrapMode=TextureWrapMode.Clamp};
            var pixels=new Color32[size*size];
            for(int y=0;y<size;y++)for(int x=0;x<size;x++)
            {
                float dx=Mathf.Max(radius-(x+.5f),x+.5f-(size-radius));float dy=Mathf.Max(radius-(y+.5f),y+.5f-(size-radius));
                float distance=Mathf.Sqrt(Mathf.Max(0,dx)*Mathf.Max(0,dx)+Mathf.Max(0,dy)*Mathf.Max(0,dy));
                pixels[y*size+x]=new Color32(255,255,255,(byte)(Mathf.Clamp01(radius+.5f-distance)*255));
            }
            texture.SetPixels32(pixels);texture.Apply(false,true);
            roundedSurface=Sprite.Create(texture,new Rect(0,0,size,size),new Vector2(.5f,.5f),100,0,SpriteMeshType.FullRect,new Vector4(16,16,16,16));
            return roundedSurface;
        }
        RectTransform Surface(string name,Transform parent,Vector2 min,Vector2 max,Color color,bool shadow=false)
        {
            var rect=Panel(name,parent,min,max,color);var graphic=rect.GetComponent<UnityEngine.UI.Image>();graphic.sprite=SurfaceSprite();graphic.type=UnityEngine.UI.Image.Type.Sliced;
            if(shadow){var effect=rect.gameObject.AddComponent<UnityEngine.UI.Shadow>();effect.effectColor=new Color(0,.08f,.04f,.10f);effect.effectDistance=new Vector2(0,-3);}
            return rect;
        }
        UnityEngine.UI.Button Skin(UnityEngine.UI.Button button,Color background,Color foreground)
        {
            var image=button.GetComponent<UnityEngine.UI.Image>();image.sprite=SurfaceSprite();image.type=UnityEngine.UI.Image.Type.Sliced;image.color=background;if(activePlanet==3&&background==Accent)foreground=Dark;
            button.GetComponentInChildren<TMP_Text>().color=foreground;
            var colors=button.colors;colors.highlightedColor=new Color(1.12f,1.12f,1.12f);colors.pressedColor=new Color(.85f,.9f,.85f);colors.disabledColor=new Color(.7f,.7f,.7f,.65f);button.colors=colors;
            button.gameObject.AddComponent<ClassicButtonMotion>();return button;
        }
        void BuildDashboard()
        {
            var go=new GameObject("Automation UI",typeof(RectTransform),typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));go.transform.SetParent(transform,false);canvas=(RectTransform)go.transform;
            go.GetComponent<Canvas>().renderMode=RenderMode.ScreenSpaceOverlay;
            var scaler=go.GetComponent<CanvasScaler>();scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1600,1000);scaler.screenMatchMode=CanvasScaler.ScreenMatchMode.Expand;
            if(EventSystem.current==null)new GameObject("EventSystem",typeof(EventSystem),typeof(StandaloneInputModule));
            Panel("Workspace",canvas,Vector2.zero,Vector2.one,Color.clear);
            var bar=Surface("Top Navigation",canvas,new Vector2(.015f,.925f),new Vector2(.985f,.988f),Dark,true);
            Label("VERDANT / "+VerdantContent.Version,bar,new Vector2(.015f,.13f),new Vector2(.20f,.90f),20,Ink);
            string[] titles={Tr("Universe","宇宙"),Tr("Colony","殖民地"),Tr("Research","科技"),Tr("Programs","程序"),Tr("Manual","手册"),"UFO",Tr("Records","纪录"),Tr("Settings","设置")};
            string[] modes={"world","archipelago","research","programs","guide","ufos","records","settings"};navigation.Clear();
            for(int i=0;i<modes.Length;i++){string mode=modes[i];float x=.22f+i*.096f;var button=Skin(Button(titles[i],bar,new Vector2(x,.12f),new Vector2(x+.087f,.88f),()=>{if(mode=="research")researchScope=WorldSelectionActive?-1:activePlanet;Open(mode);}),Dark,Ink);navigation[mode]=button;}
            Label(IslandName(activePlanet),canvas,new Vector2(.025f,.857f),new Vector2(.49f,.92f),36,Ink);
            var status=Surface("Farm Status",canvas,new Vector2(.64f,.865f),new Vector2(.98f,.915f),Paper);headerStatus=Label("",status,new Vector2(.04f,.12f),new Vector2(.96f,.88f),16,Ink);
            Button(Tr("Inventory","库存"),canvas,new Vector2(.51f,.866f),new Vector2(.62f,.913f),()=>Open("inventory"));
            resourceValues.Clear();var visible=activePlanet==0?ClassicFarm.Items.Take(8).ToArray():ArchipelagoWorld.Recipes.Where(v=>v.island==activePlanet).Select(v=>v.item).Take(8).ToArray();
            for(int i=0;i<visible.Length;i++){string item=visible[i];float x=.025f+i*.075f;var tile=Surface("Resource "+item,canvas,new Vector2(x,.785f),new Vector2(x+.07f,.85f),Paper,true);Label(CargoName(item),tile,new Vector2(.09f,.57f),new Vector2(.94f,.94f),12,Muted);resourceValues[item]=Label("0",tile,new Vector2(.09f,.04f),new Vector2(.94f,.60f),23,Ink);}
            var field=Surface("Spherical Surface",canvas,new Vector2(.025f,.265f),new Vector2(.62f,.765f),Paper,true);field.GetComponent<UnityEngine.UI.Image>().fillCenter=false;
            Surface("Planet Caption",field,new Vector2(0,.88f),Vector2.one,Paper);Label(Tr("SPHERICAL SURFACE / 360° × 128°","球面表层 / 360° × 128°"),field,new Vector2(.025f,.91f),new Vector2(.70f,.98f),15,Ink);farmSizeLabel=Label("",field,new Vector2(.72f,.91f),new Vector2(.975f,.98f),15,Ink);
            Button(Tr("Orbit view","全景"),field,new Vector2(.025f,.025f),new Vector2(.22f,.105f),()=>View.FocusPlot=false);Button(Tr("Focus worker","聚焦载具"),field,new Vector2(.24f,.025f),new Vector2(.49f,.105f),()=>View.FocusPlot=true);Button(Tr("Surface controls","表层管理"),field,new Vector2(.68f,.025f),new Vector2(.975f,.105f),()=>Open("archipelago"));
            cropStatus=Label("",field,new Vector2(.025f,.80f),new Vector2(.975f,.88f),14,Ink);
            var growthTrack=Panel("Growth Track",field,new Vector2(.025f,.77f),new Vector2(.975f,.778f),Dark);growthStatusFill=Panel("Growth",growthTrack,Vector2.zero,Vector2.one,Accent);
            var goal=Surface("Next Discovery",canvas,new Vector2(.025f,.035f),new Vector2(.31f,.245f),Paper,true);discoveryCount=Label("",goal,new Vector2(.05f,.82f),new Vector2(.95f,.97f),12,Muted);goalTitle=Label("",goal,new Vector2(.05f,.61f),new Vector2(.95f,.82f),22,Ink);goalCost=Label("",goal,new Vector2(.05f,.31f),new Vector2(.95f,.60f),13,Muted);var progress=Panel("Progress",goal,new Vector2(.05f,.26f),new Vector2(.95f,.28f),Dark);goalProgress=Panel("Fill",progress,Vector2.zero,Vector2.one,Accent);goalAction=Skin(Button(Tr("Research & progression","科技与发展"),goal,new Vector2(.05f,.055f),new Vector2(.95f,.22f),()=>Open("research")),Accent,Ink);
            var output=Surface("Flight Journal",canvas,new Vector2(.325f,.035f),new Vector2(.62f,.245f),Paper,true);Label(Tr("TRANSMISSIONS / PROGRAM OUTPUT","通讯 / 程序输出"),output,new Vector2(.04f,.83f),new Vector2(.96f,.98f),12,Muted);console=Label(string.Join("\n",Logs.Skip(Math.Max(0,Logs.Count-8))),output,new Vector2(.04f,.05f),new Vector2(.96f,.81f),12,Ink);console.overflowMode=TextOverflowModes.Truncate;
            var studio=Surface("Studio",canvas,new Vector2(.64f,.035f),new Vector2(.98f,.85f),Dark,true);
            Label(Tr("02 / AUTOMATION STUDIO","02 / 自动化工作台"),studio,new Vector2(.04f,.949f),new Vector2(.57f,.99f),12,Sea);
            BuildStudioTarget(studio);
            var library=Skin(Button("",studio,new Vector2(.60f,.949f),new Vector2(.96f,.99f),()=>Open("programs")),Dark,Sea);programSummaryLabel=library.GetComponentInChildren<TMP_Text>();programSummaryLabel.fontSize=12;
            programLabel=Label("",studio,new Vector2(.04f,.884f),new Vector2(.73f,.948f),26,Color.white);
            Skin(Button(Tr("Templates","模板"),studio,new Vector2(.74f,.894f),new Vector2(.96f,.945f),()=>Open("templates")),Paper,Color.white);
            var code=Surface("Editor Well",studio,new Vector2(.025f,.24f),new Vector2(.975f,.81f),Dark);
            Editor=MakeInput("Code",code,new Vector2(.095f,.015f),new Vector2(.99f,.985f),true);
            var gutter=Panel("Line Number Viewport",code,new Vector2(.01f,.03f),new Vector2(.082f,.97f),Color.clear);gutter.gameObject.AddComponent<RectMask2D>();
            var numbers=Label("1",gutter,Vector2.zero,Vector2.one,18,ClassicView.ColorOf("647B6A"));numbers.enableAutoSizing=false;numbers.alignment=TextAlignmentOptions.TopRight;numbers.enableWordWrapping=false;
            var lineNumbers=code.gameObject.AddComponent<ClassicLineNumbers>();lineNumbers.field=Editor;lineNumbers.label=numbers;
            Editor.SetTextWithoutNotify(Farm.Data.programs[Farm.Data.selectedProgram].source);History().Record(Editor.text,0,0);((VerdantCodeInput)Editor).RestoreHistory=Restore;
            Editor.onValueChanged.AddListener(text=>{History().Record(text,Editor.selectionStringAnchorPosition,Editor.selectionStringFocusPosition);StoreEditor();SyncCodeWindows();});
            Skin(Button(Tr("Run","运行"),studio,new Vector2(.04f,.165f),new Vector2(.28f,.22f),Run),Accent,Ink);
            Skin(Button(Tr("Pause","暂停"),studio,new Vector2(.30f,.165f),new Vector2(.51f,.22f),()=>{Session.Paused=!Session.Paused;Farm.Fleet.ProgramsPaused=Session.Paused;Refresh();}),Paper,Color.white);
            Skin(Button(Tr("Step","单步"),studio,new Vector2(.53f,.165f),new Vector2(.74f,.22f),StepSelectedTarget),Paper,Color.white);
            Skin(Button(Tr("Stop all","全部停止"),studio,new Vector2(.76f,.165f),new Vector2(.96f,.22f),()=>{Session.Stop();if(Farm.Data.archipelago!=null)Farm.Fleet.HaltAll();if(race!=null){race.Stop();race=null;}}),ClassicView.ColorOf("685448"),Color.white);
            Skin(Button(Tr("Undo","撤销"),studio,new Vector2(.04f,.097f),new Vector2(.20f,.14f),()=>Restore(false)),Dark,Sea);
            Skin(Button(Tr("Redo","重做"),studio,new Vector2(.22f,.097f),new Vector2(.38f,.14f),()=>Restore(true)),Dark,Sea);
            stateLabel=Label("",studio,new Vector2(.40f,.087f),new Vector2(.96f,.145f),12,Lime);stateLabel.overflowMode=TextOverflowModes.Truncate;
            watchLabel=Label("",studio,new Vector2(.04f,.018f),new Vector2(.96f,.078f),12,ClassicView.ColorOf("A9C8B5"));watchLabel.overflowMode=TextOverflowModes.Truncate;
            if(View!=null)View.RefreshPalette();Refresh();
        }

        string NextResearch()
        {
            string[] route={"loops","senses","plant","expand","operators","variables","trees","carrots","watering","functions","lists","pumpkins","sunflowers","dicts","import","fertilizer","polyculture","cactus","mazes","megafarm","dinosaurs","spaceflight","flower_planet","mineral_planet","ocean_planet","leaderboard"};
            return route.FirstOrDefault(id=>Farm.Level(id)==0&&ResearchRouteOpen(id))??ClassicFarm.Techs.FirstOrDefault(t=>Farm.Level(t.id)<t.max&&CanReachResearch(t.id))?.id;
        }
        bool CanReachResearch(string id){return Farm.ResearchRequirementsMet(id);}
        void RefreshDashboard()
        {
            if(cropStatus!=null)
            {
                var unit=Farm.Fleet.Data.units.FirstOrDefault(u=>u.island==activePlanet);float progress=0;
                if(activePlanet==0){int x=unit?.x??0,y=unit?.y??0;var cell=Farm.Cell(x,y);progress=ClassicView.GrowthFraction(cell);cropStatus.text=(cell.entity=="grass"?Tr("Grass","草"):cell.entity=="tree"?Tr("Tree","树木"):cell.entity=="bush"?Tr("Bush","灌木"):ItemName(cell.entity))+"  ·  "+Mathf.RoundToInt(progress*100)+"%  ·  "+(Farm.Ready(cell)?Tr("Ready to harvest","可以收获"):Tr("Growing","生长中"))+"  ·  ("+x+","+y+")";}
                else if(unit!=null&&unit.jobs.Count>0){var job=unit.jobs[0];var recipe=ArchipelagoWorld.Recipes.FirstOrDefault(r=>r.item==job.item);progress=recipe==null?0:1-(float)(job.timer/(recipe.seconds*(.8+.2*Math.Sqrt(unit.width*unit.height))/Farm.Fleet.ProductionSpeed(activePlanet,job.item)));cropStatus.text=CargoName(job.item)+"  ·  "+FleetStatus(unit.status)+"  ·  "+Math.Max(0,job.timer).ToString("F1")+"s";}
                else cropStatus.text=Tr("Assign a production program to your local worker.","为本地载具绑定生产程序。");
                growthStatusFill.anchorMax=new Vector2(Mathf.Clamp01(progress),1);
            }
            foreach(var p in resourceValues)if(p.Value!=null)p.Value.text=Farm.Fleet.Stock(activePlanet,p.Key)>=1000000?(Farm.Fleet.Stock(activePlanet,p.Key)/1000000).ToString("0.#")+"m":Farm.Fleet.Stock(activePlanet,p.Key).ToString("N0");
            if(programSummaryLabel!=null)programSummaryLabel.text=Farm.Data.programs.Count+Tr(" files · "," 个文件 · ")+Farm.Data.programs.Count(p=>ProgramWorkers(p.name)>0)+Tr(" active"," 个活跃");
            if(programLabel!=null)programLabel.color=ProgramWorkers(Farm.Data.programs[Farm.Data.selectedProgram].name)>0?Lime:Color.white;
            if(farmSizeLabel!=null)farmSizeLabel.text=Farm.Fleet.SurfaceWidth(activePlanet)+" × "+Farm.Fleet.SurfaceHeight(activePlanet);
            if(headerStatus!=null)headerStatus.text=(Session.Running||Farm.Fleet.AnyCodeRunning?Tr("AUTOMATION LIVE","自动化运行中"):Tr("COLONY ONLINE","殖民地在线"))+"  /  "+Speed+"×";
            int researched=ClassicFarm.Techs.Count(t=>Farm.Level(t.id)>0);
            if(discoveryCount!=null)discoveryCount.text=Tr("NEXT DISCOVERY","下一项发现")+"  /  "+researched+" : "+ClassicFarm.Techs.Length;
            recommendedResearch=NextResearch();
            if(goalTitle!=null)goalTitle.text=recommendedResearch==null?Tr("An open horizon","更广阔的天地"):TechName(recommendedResearch);
            bool islandsNext=recommendedResearch==null;
            if(goalAction!=null)goalAction.GetComponentInChildren<TMP_Text>().text=islandsNext?Tr("Open colony","打开殖民地"):Tr("Open research tree","打开科技树");
            if(goalCost!=null)
            {
                var cost=recommendedResearch==null?null:Farm.Cost(recommendedResearch);
                goalCost.text=cost==null?Tr("Refine your algorithms in local competition.","在本地竞速中继续优化你的算法。"):string.Join("\n",cost.Select(p=>ItemName((string)p.Key)+"  "+Farm.Count((string)p.Key).ToString("N0")+" / "+ScriptValue.Show(p.Value)));
                if(recommendedResearch!=null&&!CanReachResearch(recommendedResearch))goalCost.text+="\n"+ResearchRequirementsText(recommendedResearch,true);
                float ratio=cost==null?1:(float)cost.Min(p=>Math.Min(1,Farm.Count((string)p.Key)/ScriptValue.Number(p.Value)));if(recommendedResearch!=null){var requirements=Farm.Requirements(recommendedResearch);if(requirements!=null)foreach(var r in requirements.Where(r=>Convert.ToString(r.Key).StartsWith("produced:")))ratio=Math.Min(ratio,(float)Math.Min(1,Farm.Produced(Convert.ToString(r.Key).Substring(9))/ScriptValue.Number(r.Value)));}goalProgress.anchorMax=new Vector2(ratio,1);
                if(islandsNext)
                {
                    var world=Farm.Fleet;goalTitle.text=world.Complete?Tr("Verdant concord","禾序协约"):world.Data.started?L10n.T(world.Current.zh):Tr("Build an island network","建设群岛网络");
                    goalCost.text=world.Complete?Tr("Research and all universe projects complete.","研究与全部宇宙项目均已完成。"):world.Data.started?Tr("Universe contract ","宇宙订单 ")+(world.Data.order+1)+" / "+ArchipelagoWorld.Contracts.Length+"\n"+L10n.T(world.Current.lessonZh):Tr("Research the network and connect Earth to specialist islands.","研究群岛网络，连接地球与特色岛生产链。");
                    goalProgress.anchorMax=new Vector2(world.Data.order/(float)ArchipelagoWorld.Contracts.Length,1);
                }
            }
            foreach(var entry in navigation)if(entry.Value!=null)entry.Value.GetComponent<UnityEngine.UI.Image>().color=modalKind==entry.Key?Accent:Dark;
        }
    }
}
