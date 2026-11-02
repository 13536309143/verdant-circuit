using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Verdant.Automation
{
    public sealed partial class ClassicController
    {
        readonly List<Action> modalRefreshers=new List<Action>();RectTransform canvas,modal,modalBody;string modalKind;TMP_Text stateLabel,console,programLabel,watchLabel;TMP_InputField newName,breakpointInput;int lastPauseSerial;
        Color Ink {get{return ClassicView.ColorOf("E3F0F2");}}
        Color Paper {get{return ClassicView.ColorOf(activePlanet==1?"19392D":activePlanet==2?"112D4B":activePlanet==3?"293443":"112F3A");}}
        Color Accent {get{return ClassicView.ColorOf(activePlanet==1?"C86C87":activePlanet==2?"368EC5":activePlanet==3?"9CAFC3":"258E83");}}
        Color Dark {get{return ClassicView.ColorOf(activePlanet==1?"10261F":activePlanet==2?"08182E":activePlanet==3?"151C27":"081E29");}}
        Color Muted {get{return ClassicView.ColorOf("9BB2BD");}}
        void BuildUI(){BuildDashboard();}
        void Refresh()
        {
            if(canvas==null)return;foreach(var refresh in modalRefreshers)refresh();RefreshDashboard();
            if(studioTarget!=null)studioTarget.text=Tr("TARGET / ","绑定目标／")+TargetName()+Tr(" · change"," · 更改");
            programLabel.text=Farm.Data.programs[Farm.Data.selectedProgram].name+".py";var running=Session.FocusedDrone;
            if(lastPauseSerial!=Session.PauseSerial){lastPauseSerial=Session.PauseSerial;if(running!=null&&running.task!=null){object module;running.task.CurrentScope.Root.Try("__name__",out module);string name=module as string;if(name=="__main__")name=running.program;int index=Farm.Data.programs.FindIndex(p=>ClassicProgramNames.Same(p.name,name));if(index>=0&&index!=Farm.Data.selectedProgram)Switch(index);int start=0,line=1;while(start<Editor.text.Length&&line<running.line)if(Editor.text[start++]=='\n')line++;int end=Editor.text.IndexOf('\n',start);Editor.selectionStringAnchorPosition=start;Editor.selectionStringFocusPosition=end<0?Editor.text.Length:end;}}
            var active=SelectedRuntime();stateLabel.text=(Farm.Fleet.ProgramsPaused?Tr("Paused","已暂停"):active!=null?Tr("Running","运行中"):Tr("Ready","就绪"))+" / "+TargetName()+" / "+Tr("Line ","行 ")+(active?.Line??0)+" / "+Farm.Fleet.Data.units.Count(u=>u.island==activePlanet)+" / "+Farm.Fleet.WorkerLimit(activePlanet);

            watchLabel.text=running==null?Tr("Ctrl/Cmd+Enter: run  /  F8: language  /  Esc: settings","Ctrl/Cmd+Enter 运行  /  F8 语言  /  Esc 设置"):string.Join("  ",running.task.CurrentScope.values.Where(p=>!p.Key.StartsWith("_")).Take(7).Select(p=>p.Key+"="+ScriptValue.Show(p.Value)));
        }
        void Close(){RestoreUniverseUi();modalRefreshers.Clear();if(modal!=null){modal.gameObject.SetActive(false);Destroy(modal.gameObject);}modal=null;modalBody=null;modalKind=null;}
        void Open(string kind)
        {
            if(Editor!=null)Editor.DeactivateInputField();if(EventSystem.current!=null)EventSystem.current.SetSelectedGameObject(null);Close();modalKind=kind;if(kind=="world"){BuildWorldSelection();return;}if(kind=="research"){BuildResearchTree();return;}if(kind=="ufos"){BuildUfoConsole();return;}if(kind=="archipelago"){BuildArchipelago();return;}modal=Panel("Modal",canvas,new Vector2(.075f,.075f),new Vector2(.925f,.925f),Paper);modal.gameObject.AddComponent<CanvasGroup>().blocksRaycasts=true;modal.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;
            var drag=Label(Tr("VERDANT CIRCUIT","禾序"),modal,new Vector2(.03f,.925f),new Vector2(.78f,.985f),27,Ink);drag.raycastTarget=true;drag.gameObject.AddComponent<ClassicDrag>().target=modal;Button(Tr("Close","关闭"),modal,new Vector2(.87f,.935f),new Vector2(.975f,.982f),Close);
            var scroll=Panel("Scroll",modal,new Vector2(.025f,.025f),new Vector2(.975f,.91f),Paper);var sr=scroll.gameObject.AddComponent<ScrollRect>();var viewport=Panel("Viewport",scroll,Vector2.zero,Vector2.one,Paper);viewport.gameObject.AddComponent<Mask>().showMaskGraphic=false;viewport.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;
            modalBody=Panel("Content",viewport,new Vector2(0,1),new Vector2(1,1),new Color(0,0,0,0));modalBody.pivot=new Vector2(.5f,1);modalBody.anchoredPosition=Vector2.zero;var layout=modalBody.gameObject.AddComponent<VerticalLayoutGroup>();layout.spacing=12;layout.childControlHeight=true;layout.childControlWidth=true;layout.childForceExpandHeight=false;layout.padding=new RectOffset(8,8,8,8);var fit=modalBody.gameObject.AddComponent<ContentSizeFitter>();fit.verticalFit=ContentSizeFitter.FitMode.PreferredSize;sr.viewport=viewport;sr.content=modalBody;sr.horizontal=false;sr.movementType=ScrollRect.MovementType.Clamped;
            if(kind=="inventory"){Paragraph(Tr("EARTH INVENTORY / original crops and returned island products","地球库存／原有作物与运回的群岛特产"),70);foreach(string item in ArchipelagoWorld.Goods)if(Farm.Fleet.Stock(activePlanet,item)>0){string cargo=item;var label=Row(CargoName(item),"",Tr("Programs","程序"),()=>Open("programs"));var value=label.GetComponentsInChildren<TMP_Text>()[1];modalRefreshers.Add(()=>value.text=Farm.Fleet.Stock(activePlanet,cargo).ToString("N0"));}}
            else if(kind=="programs")BuildPrograms();
            else if(kind=="templates")
            {Paragraph(Tr("Templates are examples, not automatic solutions. Unlock the required crops first. Each template opens as a new program.","模板用于学习，不会替你完成研究。先解锁所需作物；每个模板会创建为新程序。"),75);for(int i=0;i<ClassicExamples.Sources.Length;i++){int index=i;Row(ExampleName(i),ClassicExamples.Requirements[i],Tr("Create","创建"),()=>{string name="example_"+index;int suffix=1;while(Farm.Data.programs.Any(p=>ClassicProgramNames.Same(p.name,name,true)))name="example_"+index+"_"+suffix++;NewProgram(name,ClassicExamples.Sources[index]);Close();});}}
            else if(kind=="guide")BuildManual();
            else if(kind=="farms")BuildFarms();
            else if(kind=="settings")
            {
                Row(Tr("Language","语言"),Tr("English or Simplified Chinese. F8 also switches languages.","英文或简体中文，也可以按 F8 切换。"),Tr("Switch","切换"),()=>L10n.Set(L10n.Code=="en"?"zh-Hans":"en"));
                Row(Tr("Simulation speed","运行速度"),Speed+"×",Tr("Change","调整"),()=>{Speed=Speed==1?2:Speed==2?4:Speed==4?8:Speed==8?16:1;Open("settings");});
                BuildVolumeSetting();
                Row(Tr("Opening film","开场动画"),Tr("Replay the prologue. Your farm and running code wait until it ends.","重播序章。播放期间农场与正在运行的程序暂停推进。"),Tr("Replay","重播"),PlayOpening);
                Row(Tr("Unrestricted syntax","允许未解锁语法"),Farm.Data.allowLockedSyntax?Tr("Enabled","已启用"):Tr("Disabled","已禁用"),Tr("Switch","切换"),()=>{Farm.Data.allowLockedSyntax=!Farm.Data.allowLockedSyntax;Open("settings");});
                Row(Tr("External editor","外部编辑器"),CodeFolder,Watching?Tr("Disable watcher","停止监视"):Tr("Export and watch","导出并监视"),()=>{Watch(!Watching);Open("settings");});
                Row(Tr("Farm library","农场列表"),Tr("Independent saves and programs.","独立存档和程序。"),Tr("Manage","管理"),()=>Open("farms"));
                Row(Tr("Save","保存"),SavePath,Tr("Save now","立即保存"),Save);Row(Tr("New farm","新建农场"),Tr("Start a fresh automation farm. Your current save is backed up.","创建全新的自动化农场，当前存档会保留备份。"),Tr("Restart","重新开始"),()=>Open("confirm"));
                Row(Tr("Breakpoints","断点"),Tr("Comma-separated line numbers. A matching line pauses every worker.","用逗号分隔行号。到达断点时暂停所有工作机。"),Tr("Apply","应用"),()=>{Session.breakpoints.Clear();foreach(string n in breakpointInput.text.Split(',')){int value;if(int.TryParse(n.Trim(),out value)&&value>0)Session.breakpoints.Add(value);}Log(Tr("Breakpoints updated.","断点已更新。"));});breakpointInput=MakeInput("Breakpoints",modalBody.GetChild(modalBody.childCount-1),new Vector2(.02f,.1f),new Vector2(.6f,.35f),false);breakpointInput.text=string.Join(",",Session.breakpoints);
                Row(Tr("Quit","退出"),Tr("Save before closing.","退出前保存。"),Tr("Save and quit","保存并退出"),()=>{Save();Application.Quit();});
            }
            else if(kind=="confirm")Row(Tr("Start again?","重新开始？"),Tr("This creates a fresh one-plot farm with no resources.","这将创建一格农田、零物资的新开局。"),Tr("Start","开始"),NewFarm);
            else if(kind=="records")BuildRecords();else if(kind=="challenges")BuildChallenges();
        }
        void Paragraph(string text,int height){var row=Panel("Paragraph",modalBody,Vector2.zero,Vector2.one,Paper);row.gameObject.AddComponent<LayoutElement>().preferredHeight=height;var t=Label(text,row,new Vector2(.015f,.02f),new Vector2(.985f,.98f),18,Ink);t.enableWordWrapping=true;}
        RectTransform Row(string title,string detail,string button,Action action,bool enabled=true)
        {var row=Panel("Entry",modalBody,Vector2.zero,Vector2.one,new Color(Muted.r,Muted.g,Muted.b,.15f));row.gameObject.AddComponent<LayoutElement>().preferredHeight=105;Label(title,row,new Vector2(.02f,.62f),new Vector2(.79f,.95f),21,Ink);var d=Label(detail,row,new Vector2(.02f,.06f),new Vector2(.79f,.6f),15,Ink);d.enableWordWrapping=true;Button(button,row,new Vector2(.82f,.28f),new Vector2(.98f,.72f),action).interactable=enabled;return row;}
        RectTransform Panel(string name,Transform parent,Vector2 min,Vector2 max,Color color)
        {var go=new GameObject(name,typeof(RectTransform),typeof(UnityEngine.UI.Image));go.transform.SetParent(parent,false);var r=(RectTransform)go.transform;r.anchorMin=min;r.anchorMax=max;r.offsetMin=r.offsetMax=Vector2.zero;var image=go.GetComponent<UnityEngine.UI.Image>();image.color=color;image.raycastTarget=false;return r;}
        TMP_Text Label(string value,Transform parent,Vector2 min,Vector2 max,int size,Color color)
        {var go=new GameObject("Label",typeof(RectTransform),typeof(TextMeshProUGUI));go.transform.SetParent(parent,false);var r=(RectTransform)go.transform;r.anchorMin=min;r.anchorMax=max;r.offsetMin=r.offsetMax=Vector2.zero;var t=go.GetComponent<TextMeshProUGUI>();t.font=Font;t.fontSize=size;t.enableAutoSizing=true;t.fontSizeMax=size;t.fontSizeMin=Math.Max(11,size*.8f);t.color=color;t.text=value;t.enableWordWrapping=true;t.raycastTarget=false;L10n.Bind(t,value);return t;}
        UnityEngine.UI.Button Button(string value,Transform parent,Vector2 min,Vector2 max,Action action)
        {var r=Panel(value,parent,min,max,Accent);r.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;var button=r.gameObject.AddComponent<UnityEngine.UI.Button>();button.targetGraphic=r.GetComponent<UnityEngine.UI.Image>();button.onClick.AddListener(()=>{Audio.PlayCue(440);try{action();}catch(Exception e){Log(e.Message);}});var t=Label(value,r,new Vector2(.03f,.06f),new Vector2(.97f,.94f),16,activePlanet==3?Dark:Color.white);t.alignment=TextAlignmentOptions.Center;return button;}
        TMP_InputField MakeInput(string name,Transform parent,Vector2 min,Vector2 max,bool multiline)
        {var r=Panel(name,parent,min,max,multiline?Dark:Paper);r.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;var field=r.gameObject.AddComponent<VerdantCodeInput>();field.enabled=false;var viewport=Panel("Viewport",r,new Vector2(.025f,.015f),new Vector2(.975f,.985f),new Color(0,0,0,0));viewport.gameObject.AddComponent<RectMask2D>();var text=Label("",viewport,Vector2.zero,Vector2.one,multiline?18:17,multiline?ClassicView.ColorOf("E2EAD6"):Ink);text.enableAutoSizing=false;text.enableWordWrapping=false;field.textViewport=viewport;field.textComponent=text;field.lineType=multiline?TMP_InputField.LineType.MultiLineNewline:TMP_InputField.LineType.SingleLine;field.richText=false;field.restoreOriginalTextOnEscape=false;field.customCaretColor=true;field.caretColor=multiline?Color.white:Ink;field.selectionColor=new Color(.28f,.65f,.55f,.55f);field.caretWidth=2;field.characterLimit=200000;field.enabled=true;if(multiline){var hover=r.gameObject.AddComponent<ClassicCodeHover>();hover.field=field;hover.controller=this;}return field;}
        static string TechName(string id){switch(id){case "plant":return Tr("Planting","种植");case "expand":return Tr("Field expansion","农田扩张");case "speed":return Tr("Execution speed","执行速度");case "grass":return Tr("Grass yield","干草增产");case "trees":return Tr("Woodland","林木");case "carrots":return Tr("Carrots","胡萝卜种植");case "watering":return Tr("Irrigation","灌溉");case "pumpkins":return Tr("Giant pumpkins","巨型南瓜");case "sunflowers":return Tr("Solar flowers","太阳花");case "fertilizer":return Tr("Fertilizer and infection","肥料与感染");case "polyculture":return Tr("Companion planting","伴生种植");case "cactus":return Tr("Ordered cacti","仙人掌排序");case "mazes":return Tr("Hedge mazes","树篱迷宫");case "dinosaurs":return Tr("Tail exploration","长尾探索");case "megafarm":return Tr("Parallel drones","并行无人机");case "spaceflight":return Tr("Spaceflight","星际航行");case "flower_planet":return Tr("Flower planet","花卉星球");case "mineral_planet":return Tr("Mineral planet","矿物星球");case "ocean_planet":return Tr("Ocean planet","海洋星球");case "leaderboard":return Tr("Local competition","本地竞速");case "loops":return Tr("Loops","循环");case "operators":return Tr("Operators","运算符");case "variables":return Tr("Variables","变量");case "functions":return Tr("Functions","函数");case "lists":return Tr("Lists and tuples","列表与元组");case "dicts":return Tr("Dictionaries and sets","字典与集合");case "import":return Tr("Modules","模块");case "senses":return Tr("Sensors","传感器");case "costs":return Tr("Cost queries","成本查询");case "auto_unlock":return Tr("Automatic research","自动科研");case "timing":return Tr("Timing tools","计时工具");case "debug":return Tr("Debugging","调试");case "hats":return Tr("Hats","帽子");case "debug2":return Tr("World testing tools","世界调试工具");case "utilities":return Tr("Numeric utilities","数值工具");case "the_farmers_remains":return Tr("Autonomy beacon","自治信标");case "top_hat":return Tr("The master hat","大师礼帽");default:return id;}}
        static string ExampleName(int i){switch(i){case 0:return Tr("First harvest","第一份收获");case 1:return Tr("A full-field patrol","整田巡查");case 2:return Tr("Checkerboard woodland","棋盘林地");case 3:return Tr("Growing giant pumpkins","巨型南瓜");case 4:return Tr("Sorting cacti","仙人掌排序");case 5:return Tr("Searching a maze","探索迷宫");case 6:return Tr("Parallel columns","并行分列");case 7:return Tr("Research controller","科研控制器");default:return Tr("Tail coverage","长尾覆盖");}}
    }
}
