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
        TMP_Text farmSizeLabel,headerStatus,goalTitle,goalCost,discoveryCount,programSummaryLabel;
        RectTransform goalProgress;
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
            var image=button.GetComponent<UnityEngine.UI.Image>();image.sprite=SurfaceSprite();image.type=UnityEngine.UI.Image.Type.Sliced;image.color=background;
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
            var rail=Panel("Navigation Rail",canvas,Vector2.zero,new Vector2(.06f,1),Dark);
            var logo=Panel("Sprout Mark",rail,new Vector2(.26f,.913f),new Vector2(.74f,.966f),Color.clear);var mark=new GameObject("Sprout",typeof(RectTransform),typeof(ClassicSprout));mark.transform.SetParent(logo,false);var markRect=(RectTransform)mark.transform;markRect.anchorMin=Vector2.zero;markRect.anchorMax=Vector2.one;markRect.offsetMin=markRect.offsetMax=Vector2.zero;mark.GetComponent<ClassicSprout>().color=Lime;mark.GetComponent<ClassicSprout>().raycastTarget=false;
            Label("VC",rail,new Vector2(.1f,.873f),new Vector2(.9f,.91f),15,Lime).alignment=TextAlignmentOptions.Center;
            string[] titles={Tr("Research","科技"),Tr("Programs","程序"),Tr("Guide","手册"),Tr("Lab","模拟实验"),Tr("Records","纪录"),Tr("Settings","设置")};
            string[] modes={"research","programs","guide","lab","records","settings"};navigation.Clear();
            for(int i=0;i<titles.Length;i++)
            {
                string mode=modes[i];float y=.79f-i*.105f;
                var button=Skin(Button(titles[i],rail,new Vector2(.1f,y-.055f),new Vector2(.9f,y+.025f),()=>Open(mode)),Dark,Color.white);
                button.GetComponentInChildren<TMP_Text>().fontSize=13;navigation[mode]=button;
                Label((i+1).ToString("00"),rail,new Vector2(.15f,y+.027f),new Vector2(.85f,y+.05f),10,Muted).alignment=TextAlignmentOptions.Center;
            }
            Label(VerdantContent.Version,rail,new Vector2(.1f,.025f),new Vector2(.9f,.06f),10,Muted).alignment=TextAlignmentOptions.Center;
            Label(Tr("VERDANT CIRCUIT","禾序"),canvas,new Vector2(.075f,.951f),new Vector2(.5f,.989f),30,Ink);
            Label(Tr("An experimental garden, written by you.","用代码，培育一座会思考的花园。"),canvas,new Vector2(.076f,.918f),new Vector2(.62f,.95f),15,Accent);
            var status=Surface("Farm Status",canvas,new Vector2(.75f,.925f),new Vector2(.985f,.985f),Paper);
            headerStatus=Label("",status,new Vector2(.06f,.12f),new Vector2(.94f,.86f),16,Ink);headerStatus.alignment=TextAlignmentOptions.Center;

            resourceValues.Clear();
            for(int i=0;i<ClassicFarm.Items.Length;i++)
            {
                string item=ClassicFarm.Items[i];float x=.075f+i*.083f;
                var tile=Surface("Resource "+item,canvas,new Vector2(x,.825f),new Vector2(x+.077f,.901f),Paper,true);
                Panel("Resource Accent",tile,new Vector2(.08f,.2f),new Vector2(.105f,.8f),i%3==0?Accent:i%3==1?Clay:Sea);
                Label(ItemName(item),tile,new Vector2(.17f,.57f),new Vector2(.95f,.91f),13,Muted);
                resourceValues[item]=Label("0",tile,new Vector2(.17f,.09f),new Vector2(.95f,.59f),25,Ink);
            }
            var field=Surface("Field Observatory",canvas,new Vector2(.075f,.30f),new Vector2(.595f,.805f),Paper,true);
            field.GetComponent<UnityEngine.UI.Image>().fillCenter=false;
            Surface("Field Header",field,new Vector2(0,.875f),Vector2.one,Paper);Surface("Field Footer",field,Vector2.zero,new Vector2(1,.12f),Paper);
            Label(Tr("01 / THE LIVING GRID","01 / 生长中的田野"),field,new Vector2(.035f,.9f),new Vector2(.74f,.975f),18,Ink);
            farmSizeLabel=Label("",field,new Vector2(.74f,.9f),new Vector2(.965f,.975f),14,Accent);farmSizeLabel.alignment=TextAlignmentOptions.Right;
            Label(Tr("N ↑   /   Click to inspect · scroll to explore","北 ↑   /   点击查看地块 · 滚轮探索农田"),field,new Vector2(.035f,.025f),new Vector2(.965f,.10f),12,Muted);
            var goal=Surface("Next Discovery",canvas,new Vector2(.075f,.035f),new Vector2(.285f,.278f),Dark);
            discoveryCount=Label("",goal,new Vector2(.065f,.82f),new Vector2(.94f,.95f),12,Lime);
            goalTitle=Label("",goal,new Vector2(.065f,.60f),new Vector2(.94f,.81f),25,Color.white);
            goalCost=Label("",goal,new Vector2(.065f,.30f),new Vector2(.94f,.59f),14,ClassicView.ColorOf("CEDBC7"));
            var progress=Panel("Research Progress",goal,new Vector2(.065f,.255f),new Vector2(.935f,.272f),ClassicView.ColorOf("3C564B"));goalProgress=Panel("Fill",progress,Vector2.zero,Vector2.one,Lime);
            Skin(Button(Tr("Open research tree","打开科技树"),goal,new Vector2(.065f,.065f),new Vector2(.935f,.215f),()=>{selectedResearch=recommendedResearch;Open("research");}),Lime,Dark);
            var output=Surface("Output Journal",canvas,new Vector2(.299f,.035f),new Vector2(.595f,.278f),Paper);
            Label(Tr("FIELD JOURNAL / OUTPUT","田野日志 / 程序输出"),output,new Vector2(.045f,.84f),new Vector2(.95f,.97f),13,Accent);
            console=Label(string.Join("\n",Logs.Skip(Math.Max(0,Logs.Count-8))),output,new Vector2(.045f,.06f),new Vector2(.95f,.81f),12,Ink);console.alignment=TextAlignmentOptions.BottomLeft;console.overflowMode=TextOverflowModes.Truncate;

            var studio=Surface("Studio",canvas,new Vector2(.611f,.035f),new Vector2(.985f,.805f),Dark,true);
            Label(Tr("02 / AUTOMATION STUDIO","02 / 自动化工作台"),studio,new Vector2(.04f,.949f),new Vector2(.57f,.99f),12,Sea);
            var library=Skin(Button("",studio,new Vector2(.60f,.949f),new Vector2(.96f,.99f),()=>Open("programs")),Dark,Sea);programSummaryLabel=library.GetComponentInChildren<TMP_Text>();programSummaryLabel.fontSize=12;
            programLabel=Label("",studio,new Vector2(.04f,.884f),new Vector2(.73f,.948f),26,Color.white);
            Skin(Button(Tr("Templates","模板"),studio,new Vector2(.74f,.894f),new Vector2(.96f,.945f),()=>Open("templates")),ClassicView.ColorOf("36564A"),Color.white);
            var code=Surface("Editor Well",studio,new Vector2(.025f,.24f),new Vector2(.975f,.866f),ClassicView.ColorOf("10251F"));
            Editor=MakeInput("Code",code,new Vector2(.095f,.015f),new Vector2(.99f,.985f),true);
            var gutter=Panel("Line Number Viewport",code,new Vector2(.01f,.03f),new Vector2(.082f,.97f),Color.clear);gutter.gameObject.AddComponent<RectMask2D>();
            var numbers=Label("1",gutter,Vector2.zero,Vector2.one,18,ClassicView.ColorOf("647B6A"));numbers.enableAutoSizing=false;numbers.alignment=TextAlignmentOptions.TopRight;numbers.enableWordWrapping=false;
            var lineNumbers=code.gameObject.AddComponent<ClassicLineNumbers>();lineNumbers.field=Editor;lineNumbers.label=numbers;
            Editor.SetTextWithoutNotify(Farm.Data.programs[Farm.Data.selectedProgram].source);History().Record(Editor.text,0,0);((VerdantCodeInput)Editor).RestoreHistory=Restore;
            Editor.onValueChanged.AddListener(text=>{History().Record(text,Editor.selectionStringAnchorPosition,Editor.selectionStringFocusPosition);StoreEditor();SyncCodeWindows();});
            Skin(Button(Tr("Run","运行"),studio,new Vector2(.04f,.165f),new Vector2(.28f,.22f),Run),Lime,Dark);
            Skin(Button(Tr("Pause","暂停"),studio,new Vector2(.30f,.165f),new Vector2(.51f,.22f),()=>{Session.Paused=!Session.Paused;Refresh();}),ClassicView.ColorOf("36564A"),Color.white);
            Skin(Button(Tr("Step","单步"),studio,new Vector2(.53f,.165f),new Vector2(.74f,.22f),()=>{if(!Session.Running)Run();Session.Step();}),ClassicView.ColorOf("36564A"),Color.white);
            Skin(Button(Tr("Stop","停止"),studio,new Vector2(.76f,.165f),new Vector2(.96f,.22f),()=>{Session.Stop();if(race!=null){race.Stop();race=null;}}),ClassicView.ColorOf("685448"),Color.white);
            Skin(Button(Tr("Undo","撤销"),studio,new Vector2(.04f,.097f),new Vector2(.20f,.14f),()=>Restore(false)),Dark,Sea);
            Skin(Button(Tr("Redo","重做"),studio,new Vector2(.22f,.097f),new Vector2(.38f,.14f),()=>Restore(true)),Dark,Sea);
            stateLabel=Label("",studio,new Vector2(.40f,.087f),new Vector2(.96f,.145f),12,Lime);stateLabel.overflowMode=TextOverflowModes.Truncate;
            watchLabel=Label("",studio,new Vector2(.04f,.018f),new Vector2(.96f,.078f),12,ClassicView.ColorOf("A9C8B5"));watchLabel.overflowMode=TextOverflowModes.Truncate;
            if(View!=null)View.RefreshPalette();Refresh();
        }

        string NextResearch()
        {
            string[] route={"loops","senses","plant","expand","operators","variables","trees","carrots","watering","functions","lists","pumpkins","sunflowers","dicts","import","fertilizer","polyculture","cactus","mazes","megafarm","dinosaurs","simulation","leaderboard"};
            return route.FirstOrDefault(id=>Farm.Level(id)==0&&ResearchRouteOpen(id))??ClassicFarm.Techs.FirstOrDefault(t=>Farm.Level(t.id)<t.max&&CanReachResearch(t.id))?.id;
        }
        bool CanReachResearch(string id){return Farm.ResearchRequirementsMet(id);}
        void RefreshDashboard()
        {
            foreach(var p in resourceValues)if(p.Value!=null)p.Value.text=Farm.Count(p.Key)>=1000000?(Farm.Count(p.Key)/1000000).ToString("0.#")+"m":Farm.Count(p.Key).ToString("N0");
            if(programSummaryLabel!=null)programSummaryLabel.text=Farm.Data.programs.Count+Tr(" files · "," 个文件 · ")+Farm.Data.programs.Count(p=>Session.drones.Any(d=>!d.finished&&d.program==p.name))+Tr(" active"," 个活跃");
            if(programLabel!=null)programLabel.color=Session.drones.Any(d=>!d.finished&&d.program==Farm.Data.programs[Farm.Data.selectedProgram].name)?Lime:Color.white;
            if(farmSizeLabel!=null)farmSizeLabel.text=Farm.Data.size+" × "+Farm.Data.size;
            if(headerStatus!=null)headerStatus.text=(Session.Running?Tr("AUTOMATION LIVE","自动化运行中"):Tr("YOUR LOCAL GARDEN","你的本地花园"))+"  /  "+Speed+"×";
            int researched=ClassicFarm.Techs.Count(t=>Farm.Level(t.id)>0);
            if(discoveryCount!=null)discoveryCount.text=Tr("NEXT DISCOVERY","下一项发现")+"  /  "+researched+" : "+ClassicFarm.Techs.Length;
            recommendedResearch=NextResearch();
            if(goalTitle!=null)goalTitle.text=recommendedResearch==null?Tr("An open horizon","更广阔的天地"):TechName(recommendedResearch);
            if(goalCost!=null)
            {
                var cost=recommendedResearch==null?null:Farm.Cost(recommendedResearch);
                goalCost.text=cost==null?Tr("Refine your algorithms in local competition.","在本地竞速中继续优化你的算法。"):string.Join("\n",cost.Select(p=>ItemName((string)p.Key)+"  "+Farm.Count((string)p.Key).ToString("N0")+" / "+ScriptValue.Show(p.Value)));
                if(recommendedResearch!=null&&!CanReachResearch(recommendedResearch))goalCost.text+="\n"+ResearchRequirementsText(recommendedResearch,true);
                float ratio=cost==null?1:(float)cost.Min(p=>Math.Min(1,Farm.Count((string)p.Key)/ScriptValue.Number(p.Value)));if(recommendedResearch!=null){var requirements=Farm.Requirements(recommendedResearch);if(requirements!=null)foreach(var r in requirements.Where(r=>Convert.ToString(r.Key).StartsWith("produced:")))ratio=Math.Min(ratio,(float)Math.Min(1,Farm.Produced(Convert.ToString(r.Key).Substring(9))/ScriptValue.Number(r.Value)));}goalProgress.anchorMax=new Vector2(ratio,1);
            }
            foreach(var entry in navigation)if(entry.Value!=null)entry.Value.GetComponent<UnityEngine.UI.Image>().color=modalKind==entry.Key?Accent:Dark;
        }
    }
}
