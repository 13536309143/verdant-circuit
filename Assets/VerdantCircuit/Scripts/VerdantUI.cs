using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Verdant
{
    public sealed class VerdantUI : MonoBehaviour
    {
        public VerdantApp App;public TMP_InputField Editor;public TMP_Text ConsoleText;
        TMP_Text resources,season,chapter,taskTitle,taskStory,taskProgress,taskHint,cell,soil,cropTitle,saveLabel,programState,watch,cropInfo,rate,slotLabel,speedLabel,runLabel,pauseLabel;
        UnityEngine.UI.Image taskBar;UnityEngine.UI.Button claim;RectTransform canvas,modal;TMP_Text modalTitle;
        RectTransform modalBody;ScrollRect modalScroll;string modalKind;float refresh;List<string> log=new List<string>();
        List<UnityEngine.UI.Button> seedButtons=new List<UnityEngine.UI.Button>();TMP_FontAsset font,codeFont;
        readonly Dictionary<int,CodeHistory> histories=new Dictionary<int,CodeHistory>();
        public bool ModalOpen {get{return modal!=null;}}
        public bool Typing {get{return Editor!=null&&Editor.isFocused;}}
        static readonly Color Ink=VerdantWorld.Hex("233D36"),Muted=VerdantWorld.Hex("718275"),Paper=VerdantWorld.Hex("F8F7EF"),Accent=VerdantWorld.Hex("3C7C64"),Gold=VerdantWorld.Hex("E7C57D"),Dark=VerdantWorld.Hex("20372F");
        public void Initialize(VerdantApp app,TMP_FontAsset uiFont,TMP_FontAsset monoFont)
        {
            App=app;font=uiFont;codeFont=monoFont;
            var c=New("禾序 · 界面",transform,typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));canvas=(RectTransform)c.transform;
            c.GetComponent<Canvas>().renderMode=RenderMode.ScreenSpaceOverlay;
            var scaler=c.GetComponent<CanvasScaler>();scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1600,1000);scaler.screenMatchMode=CanvasScaler.ScreenMatchMode.Expand;
            if(EventSystem.current==null)new GameObject("EventSystem",typeof(EventSystem),typeof(StandaloneInputModule));
            // Space is reserved around the world camera; only panels cover it.
            var header=Panel("Header",canvas,new Vector2(0,1),new Vector2(1,1),new Vector2(0,-112),new Vector2(0,0),Paper);
            Text("Logo","禾序",header,new Vector2(0,1),new Vector2(28,-17),new Vector2(145,60),44,Ink,FontStyles.Bold);
            Text("English Name","VERDANT CIRCUIT",header,new Vector2(0,1),new Vector2(31,-77),new Vector2(260,23),12,Muted);
            resources=Text("Resources","",header,new Vector2(.5f,1),new Vector2(-140,-26),new Vector2(680,38),22,Ink,FontStyles.Bold);
            season=Text("Season","",header,new Vector2(.5f,1),new Vector2(-140,-72),new Vector2(700,25),15,Muted);
            Button("订单",header,new Vector2(1,1),new Vector2(-292,-25),new Vector2(72,34),()=>Open("orders"),Accent);
            Button("工坊",header,new Vector2(1,1),new Vector2(-292,-67),new Vector2(72,28),()=>Open("workshop"),Muted);
            Button("科技",header,new Vector2(1,1),new Vector2(-212,-25),new Vector2(72,34),()=>Open("research"),Accent);
            Button("仓库",header,new Vector2(1,1),new Vector2(-132,-25),new Vector2(72,34),()=>Open("crops"),Accent);
            Button("手册",header,new Vector2(1,1),new Vector2(-52,-25),new Vector2(72,34),()=>Open("manual"),Accent);
            Button("试炼",header,new Vector2(1,1),new Vector2(-212,-67),new Vector2(72,28),()=>Open("challenges"),Muted);
            Button("成就",header,new Vector2(1,1),new Vector2(-132,-67),new Vector2(72,28),()=>Open("achievements"),Muted);
            Button("设置",header,new Vector2(1,1),new Vector2(-52,-67),new Vector2(72,28),()=>Open("settings"),Muted);
            BuildLeft();BuildEditor();
            var footer=Panel("Footer",canvas,new Vector2(0,0),new Vector2(.66f,0),new Vector2(0,0),new Vector2(-12,86),Paper);
            saveLabel=Text("Saved","",footer,new Vector2(0,0),new Vector2(26,18),new Vector2(290,25),13,Muted);
            rate=Text("Income","",footer,new Vector2(1,0),new Vector2(-24,18),new Vector2(560,25),14,Muted,FontStyles.Normal,TextAlignmentOptions.Right);
            Text("WorldHint","点击地块导航  ·  方向键 / WASD 移动  ·  鼠标滚轮缩放",footer,new Vector2(.5f,1),new Vector2(0,-12),new Vector2(900,27),14,Muted,FontStyles.Normal,TextAlignmentOptions.Center);
            App.Farm.Message+=AddLog;App.Farm.Changed+=Refresh;App.Runner.Changed+=Refresh;
            Refresh();AddLog("欢迎来到禾湾。先完成左侧委托，再让程序接手农田。");
        }
        void BuildLeft()
        {
            var left=Panel("Mission Rail",canvas,new Vector2(0,.1f),new Vector2(.22f,.875f),new Vector2(18,0),new Vector2(-4,0),Paper);
            chapter=Text("Chapter","",left,new Vector2(0,1),new Vector2(20,-20),new Vector2(300,26),14,Accent,FontStyles.Bold);
            taskTitle=Text("Mission Title","",left,new Vector2(0,1),new Vector2(20,-58),new Vector2(295,44),28,Ink,FontStyles.Bold);
            taskStory=Text("Mission Story","",left,new Vector2(0,1),new Vector2(20,-111),new Vector2(292,80),16,Muted);taskStory.enableWordWrapping=true;
            var bar=PanelFixed("Progress Track",left,new Vector2(0,1),new Vector2(20,-205),new Vector2(290,6),VerdantWorld.Hex("E0E6D8"));
            var fill=Panel("Progress",bar,Vector2.zero,Vector2.one,Vector2.zero,Vector2.zero,Accent);taskBar=fill.GetComponent<UnityEngine.UI.Image>();
            taskProgress=Text("Progress Text","",left,new Vector2(0,1),new Vector2(20,-225),new Vector2(292,50),13,Ink);
            claim=Button("领取委托奖励",left,new Vector2(0,1),new Vector2(20,-281),new Vector2(290,40),()=>{if(App.Farm.Claim()){App.Audio.PlayCue(720);Refresh();}},Accent);
            taskHint=Text("Mission Hint","",left,new Vector2(0,1),new Vector2(20,-333),new Vector2(292,94),14,Muted);taskHint.enableWordWrapping=true;
            Text("Seed Label","播种目录",left,new Vector2(0,1),new Vector2(20,-427),new Vector2(290,28),16,Ink,FontStyles.Bold);
            for(int i=0;i<8;i++)
            {
                int index=i;var def=VerdantContent.Crops[i];
                var b=Button(def.name,left,new Vector2(0,1),new Vector2(20+(i%4)*74,-469-(i/4)*38),new Vector2(68,31),()=>{App.Farm.Data.selectedCrop=index;Refresh();},Accent);
                seedButtons.Add(b);
            }
            cropInfo=Text("Seed Rules","",left,new Vector2(0,1),new Vector2(20,-552),new Vector2(292,78),13,Muted);cropInfo.enableWordWrapping=true;
            Button("播种",left,new Vector2(0,0),new Vector2(20,72),new Vector2(68,37),()=>App.Manual("plant"),Accent);
            Button("收获",left,new Vector2(0,0),new Vector2(94,72),new Vector2(68,37),()=>App.Manual("harvest"),Accent);
            Button("浇水",left,new Vector2(0,0),new Vector2(168,72),new Vector2(68,37),()=>App.Manual("water"),Accent);
            Button("翻土",left,new Vector2(0,0),new Vector2(242,72),new Vector2(68,37),()=>App.Manual("till"),Accent);
            Text("Manual Hint","空格 播种  /  E 收获  /  Q 浇水  /  T 翻土",left,new Vector2(0,0),new Vector2(20,28),new Vector2(300,24),12,Muted);
            var inspect=Panel("Plot Inspector",canvas,new Vector2(.232f,.105f),new Vector2(.65f,.23f),Vector2.zero,Vector2.zero,Paper);
            cell=Text("Position","",inspect,new Vector2(0,1),new Vector2(20,-13),new Vector2(260,23),14,Muted);
            cropTitle=Text("Current Crop","",inspect,new Vector2(0,1),new Vector2(20,-40),new Vector2(290,31),24,Ink,FontStyles.Bold);
            soil=Text("Soil","",inspect,new Vector2(0,0),new Vector2(20,14),new Vector2(620,24),14,Muted);
        }
        void BuildEditor()
        {
            var right=Panel("Program Studio",canvas,new Vector2(.67f,0),new Vector2(1,.875f),new Vector2(0,12),new Vector2(-18,0),Dark);
            Text("Program Title","自动化工作台",right,new Vector2(0,1),new Vector2(20,-20),new Vector2(330,32),23,Paper,FontStyles.Bold);
            slotLabel=Text("Slot Label","",right,new Vector2(0,1),new Vector2(20,-61),new Vector2(315,24),12,VerdantWorld.Hex("A0C3AF"));
            Button("程序列表",right,new Vector2(1,1),new Vector2(-110,-56),new Vector2(90,29),()=>Open("programs"),Accent);
            Button("撤销",right,new Vector2(0,1),new Vector2(20,-91),new Vector2(66,25),()=>Restore(false),Accent);
            Button("重做",right,new Vector2(0,1),new Vector2(95,-91),new Vector2(66,25),()=>Restore(true),Accent);
            Button("模板",right,new Vector2(1,1),new Vector2(-20,-56),new Vector2(76,29),()=>Open("templates"),Accent);
            var field=New("Code Editor",right,typeof(UnityEngine.UI.Image),typeof(VerdantCodeInput));var rect=(RectTransform)field.transform;
            Stretch(rect,new Vector2(0,.30f),new Vector2(1,1),new Vector2(16,0),new Vector2(-16,-126));
            field.GetComponent<UnityEngine.UI.Image>().color=VerdantWorld.Hex("172B24");
            Editor=field.GetComponent<TMP_InputField>();
            var viewport=New("Viewport",rect,typeof(RectMask2D));Stretch((RectTransform)viewport.transform,Vector2.zero,Vector2.one,new Vector2(16,10),new Vector2(-12,-12));
            var text=Text("Code","",viewport.transform,new Vector2(0,1),Vector2.zero,new Vector2(440,650),17,VerdantWorld.Hex("DAE8CF"));
            var textRect=(RectTransform)text.transform;textRect.anchorMin=new Vector2(0,1);textRect.anchorMax=Vector2.one;textRect.pivot=new Vector2(.5f,1);textRect.offsetMin=new Vector2(0,-650);textRect.offsetMax=Vector2.zero;
            text.font=codeFont;text.richText=false;text.enableWordWrapping=false;text.alignment=TextAlignmentOptions.TopLeft;
            Editor.textViewport=(RectTransform)viewport.transform;Editor.textComponent=text;Editor.lineType=TMP_InputField.LineType.MultiLineNewline;Editor.contentType=TMP_InputField.ContentType.Standard;
            Editor.caretColor=Gold;Editor.customCaretColor=true;Editor.caretWidth=3;Editor.caretBlinkRate=0;Editor.selectionColor=new Color(.35f,.65f,.95f,.65f);Editor.targetGraphic=field.GetComponent<UnityEngine.UI.Image>();Editor.onFocusSelectAll=false;Editor.richText=false;Editor.characterLimit=48000;Editor.scrollSensitivity=16;
            Editor.text=App.Farm.Data.scripts[App.Farm.Data.slot]??"";
            ((VerdantCodeInput)Editor).RestoreHistory=Restore;
            History().Record(Editor.text,0,0);
            Editor.onValueChanged.AddListener(value=>{if(App.Farm.InChallenge)return;App.Farm.Data.scripts[App.Farm.Data.slot]=value;History().Record(value,Editor.selectionStringAnchorPosition,Editor.stringPosition);});
            // OnEnable ran before textComponent was assigned on this runtime-created field.
            // Re-enable after wiring references so TMP creates the caret/selection renderer.
            Editor.enabled=false;Editor.enabled=true;
            var run=Button("运行",right,new Vector2(0,.30f),new Vector2(16,-14),new Vector2(100,37),()=>App.RunCode(),Accent);runLabel=run.GetComponentInChildren<TMP_Text>();
            var pause=Button("暂停",right,new Vector2(0,.30f),new Vector2(124,-14),new Vector2(77,37),()=>App.Runner.TogglePause(),VerdantWorld.Hex("53694F"));pauseLabel=pause.GetComponentInChildren<TMP_Text>();
            Button("单步",right,new Vector2(0,.30f),new Vector2(209,-14),new Vector2(77,37),()=>App.Runner.Step(Editor.text),VerdantWorld.Hex("53694F"));
            Button("停止",right,new Vector2(0,.30f),new Vector2(294,-14),new Vector2(77,37),()=>App.Runner.Stop(),VerdantWorld.Hex("806852"));
            var speed=Button("1×",right,new Vector2(1,.30f),new Vector2(-16,-14),new Vector2(68,37),()=>App.CycleSpeed(),VerdantWorld.Hex("53694F"));speedLabel=speed.GetComponentInChildren<TMP_Text>();
            programState=Text("Program State","就绪",right,new Vector2(0,.30f),new Vector2(18,-67),new Vector2(470,24),14,Gold);
            watch=Text("Variables","",right,new Vector2(0,.30f),new Vector2(18,-94),new Vector2(470,21),12,VerdantWorld.Hex("A0C3AF"));
            ConsoleText=Text("Console","",right,new Vector2(0,0),new Vector2(18,17),new Vector2(465,124),12,VerdantWorld.Hex("C2D0BD"));ConsoleText.enableWordWrapping=true;ConsoleText.alignment=TextAlignmentOptions.BottomLeft;
        }
        public void SelectSlot(int slot)
        {
            if(App.Farm.InChallenge){AddLog("请先退出试炼再切换主线程序槽");return;}
            if(slot<0||slot>=App.Farm.Data.scripts.Length)return;App.Farm.Data.scripts[App.Farm.Data.slot]=Editor.text;App.Farm.Data.slot=slot;Editor.SetTextWithoutNotify(App.Farm.Data.scripts[slot]??"");History().Record(Editor.text,0,0);Refresh();
        }
        public void ResetPrograms(){histories.Clear();App.RunningSlot=-1;History().Record(Editor.text,0,0);}
        CodeHistory History(){int slot=App.Farm.Data.slot;CodeHistory h;if(!histories.TryGetValue(slot,out h)){h=new CodeHistory();histories[slot]=h;}return h;}
        public void Restore(bool redo)
        {
            if(App.Farm.InChallenge)return;var value=redo?History().Redo():History().Undo();if(value==null)return;
            Editor.SetTextWithoutNotify(value.Text);App.Farm.Data.scripts[App.Farm.Data.slot]=value.Text;
            Editor.ActivateInputField();Editor.stringPosition=Mathf.Clamp(value.Focus,0,value.Text.Length);Editor.selectionStringAnchorPosition=Mathf.Clamp(value.Anchor,0,value.Text.Length);Editor.ForceLabelUpdate();
        }
        public void AddProgram(){if(App.Farm.InChallenge)return;App.Farm.Data.scripts[App.Farm.Data.slot]=Editor.text;VerdantPrograms.Add(App.Farm.Data);Editor.SetTextWithoutNotify(App.Farm.Data.scripts[App.Farm.Data.slot]);History().Record(Editor.text,0,0);App.Farm.Save();Open("programs");}
        public void DeleteProgram(int slot)
        {
            if(App.Farm.InChallenge||slot==App.RunningSlot&&App.Runner.Running){AddLog("请先停止正在运行的程序再删除");return;}
            if(!VerdantPrograms.Remove(App.Farm.Data,slot)){AddLog("至少保留一个程序");return;}
            var moved=new Dictionary<int,CodeHistory>();foreach(var pair in histories)if(pair.Key!=slot)moved[pair.Key>slot?pair.Key-1:pair.Key]=pair.Value;histories.Clear();foreach(var pair in moved)histories[pair.Key]=pair.Value;
            if(App.RunningSlot>slot)App.RunningSlot--;else if(App.RunningSlot==slot)App.RunningSlot=-1;
            Editor.SetTextWithoutNotify(App.Farm.Data.scripts[App.Farm.Data.slot]);History().Record(Editor.text,0,0);App.Farm.Save();Open("programs");
        }
        public void LoadTemplate(int i)
        {Editor.text=VerdantContent.Templates[i];if(!App.Farm.InChallenge)App.Farm.Data.scripts[App.Farm.Data.slot]=Editor.text;Close();AddLog("已载入「"+VerdantContent.TemplateNames[i]+"」。编辑后点击运行。");}
        void Update(){refresh-=Time.unscaledDeltaTime;if(refresh>0)return;refresh=.25f;Refresh();}
        public void Refresh()
        {
            if(App==null||App.Farm==null||resources==null)return;var f=App.Farm;var d=f.Data;
            resources.text="金币  "+d.coins.ToString("N0")+"     研究  "+d.knowledge.ToString("N0")+"     光能  "+d.energy+"     水  "+Mathf.FloorToInt(d.water);
            season.text="第 "+((int)(d.clock/180)+1)+" 日    "+f.SeasonName+"    "+(f.Rain?"细雨 · 自动润田":"晴朗 · 正常生长")+"    "+d.size+"×"+d.size+" 农田";
            chapter.text=f.InChallenge?"限时试炼  /  "+VerdantContent.ChallengeNames[f.challenge]:"第 "+(f.Chapter+1)+" 章  /  "+VerdantContent.Chapters[f.Chapter];
            var mission=f.CurrentContract;
            if(f.InChallenge)
            {
                int i=f.challenge;int value=f.Count(VerdantContent.ChallengeCrops[i])-f.challengeStart;float remaining=Mathf.Max(0,VerdantContent.ChallengeTimes[i]-f.challengeTime);
                taskTitle.text=VerdantContent.ChallengeNames[i];taskStory.text=VerdantContent.ChallengeNotes[i];taskProgress.text=value+" / "+VerdantContent.ChallengeTargets[i]+"     剩余 "+Mathf.CeilToInt(remaining)+" 秒";
                taskHint.text="试炼为独立农场，研究全部解锁。停止脚本可以重新编辑；设置页可退出试炼。";SetProgress((float)value/VerdantContent.ChallengeTargets[i]);claim.interactable=false;
            }
            else if(mission!=null)
            {
                int value=f.ContractProgress;taskTitle.text=mission.title;taskStory.text="目标："+MissionGoal(mission)+"\n"+mission.story;taskProgress.text=Mathf.Min(value,mission.amount)+" / "+mission.amount+"    奖励 "+mission.reward+" 金币";
                taskHint.text="操作："+mission.hint+"\n达标后点击领取委托奖励。";
                if(f.ContractCropIndex>=0)taskHint.text="本任务需新增收获 "+mission.amount+" 单位"+VerdantContent.Crop(mission.metric).name+"。历史收获不计入本任务。\n操作："+mission.hint;
                SetProgress((float)value/mission.amount);claim.interactable=f.ContractReady;
            }
            else{taskTitle.text="禾湾长明";taskStory.text="主线已完成。试炼、成就和自由农场仍在等待你的下一次优化。";taskProgress.text="30 / 30 委托已完成";taskHint.text="感谢你让这片土地重新有了秩序。";SetProgress(1);claim.interactable=false;}
            cell.text="PIP-01   /   地块 "+(d.x+1).ToString("00")+" : "+(d.y+1).ToString("00");var p=f.Here;var current=VerdantContent.Crop(p.crop);
            cropTitle.text=current==null?"等待播种的土地":current.name+"  "+(f.Ready(p)?"可收获":Mathf.FloorToInt(p.growth/current.seconds*100)+"%");
            soil.text=(p.tilled?"耕地":"原土")+"     水分 "+Mathf.RoundToInt(p.moisture*100)+"%     肥力 "+Mathf.RoundToInt(p.fertility*100)+"%";
            var selected=VerdantContent.Crops[d.selectedCrop];cropInfo.text=selected.name+" · 播种 "+selected.cost+" 金币\n"+selected.note;
            for(int i=0;i<seedButtons.Count;i++){var image=seedButtons[i].GetComponent<UnityEngine.UI.Image>();image.color=i==d.selectedCrop?Accent:f.Unlocked(VerdantContent.Crops[i].id)?VerdantWorld.Hex("94AE95"):VerdantWorld.Hex("B8BCAF");}
            saveLabel.text=f.InChallenge?"试炼中 · 主线进度已保留":f.LastSaveMessage;
            rate.text="累计收获 "+f.Count("harvested").ToString("N0")+"     自动化动作 "+f.Count("programActions").ToString("N0")+"     近一分钟收入 "+App.IncomeRate+" 金币";
            slotLabel.text="查看 "+(d.slot+1)+" / "+d.scripts.Length+"   运行 "+(App.Runner.Running?(App.RunningSlot+1).ToString():"无");speedLabel.text=App.Speed+"×";
            var runner=App.Runner;programState.text=runner.Error!=""?runner.Error:runner.Status+(runner.VM!=null?"    第 "+runner.VM.CurrentLine+" 行  ·  "+runner.VM.Executed+" 指令":"    支持循环、条件与函数");
            programState.color=runner.Error!=""?VerdantWorld.Hex("F1A792"):Gold;
            watch.text=runner.VM!=null?runner.VM.Watch():"Ctrl / Cmd + Enter 运行    ·    Esc 打开设置";
            runLabel.text=runner.Running?"重新运行":"运行";pauseLabel.text=runner.Paused?"继续":"暂停";
        }
        void BuildTechTree()
        {
            var root=PanelFixed("Tech Tree",modalBody,new Vector2(0,1),Vector2.zero,new Vector2(970,1160),Paper);root.gameObject.AddComponent<LayoutElement>().preferredHeight=1160;
            var positions=new Dictionary<string,Vector2>();int[] columns=new int[6];
            foreach(var t in VerdantContent.Techs)positions[t.id]=new Vector2(8+columns[t.chapter]++*238,-25-t.chapter*185);
            foreach(var t in VerdantContent.Techs)if(t.prerequisite!="")
            {Vector2 start=positions[t.prerequisite]+new Vector2(110,-138),end=positions[t.id]+new Vector2(110,0),delta=end-start;
             var line=PanelFixed("Dependency",root,new Vector2(0,1),start,new Vector2(delta.magnitude,3),App.Farm.Has(t.prerequisite)?Accent:Muted);line.pivot=new Vector2(0,.5f);line.localRotation=Quaternion.Euler(0,0,Mathf.Atan2(delta.y,delta.x)*Mathf.Rad2Deg);}
            foreach(var t in VerdantContent.Techs)
            {var tech=t;bool done=App.Farm.Has(t.id);bool available=App.Farm.Chapter>=t.chapter&&(t.prerequisite==""||App.Farm.Has(t.prerequisite));
             var node=PanelFixed(t.id,root,new Vector2(0,1),positions[t.id],new Vector2(224,140),done?VerdantWorld.Hex("D6E6D2"):VerdantWorld.Hex("ECEFE4"));
             Text("Name",t.name,node,new Vector2(0,1),new Vector2(10,-8),new Vector2(204,26),17,Ink,FontStyles.Bold);
             var details=Text("Requirements","第"+(t.chapter+1)+"章 · "+t.price+"金币 / "+t.knowledge+"研究\n前置："+(t.prerequisite==""?"无":VerdantContent.Tech(t.prerequisite).name),node,new Vector2(0,1),new Vector2(10,-39),new Vector2(204,65),12,Muted);details.enableWordWrapping=true;
             Button(done?"已完成":available?"研究":"条件未满足",node,new Vector2(0,1),new Vector2(10,-107),new Vector2(200,26),()=>{if(App.Farm.Research(tech.id)){float scroll=modalScroll.verticalNormalizedPosition;Open("research");Canvas.ForceUpdateCanvases();modalScroll.verticalNormalizedPosition=scroll;}},done?Muted:Accent).interactable=!done&&available;
            }
        }
        string MissionGoal(ContractDef c)
        {var crop=VerdantContent.Crop(c.metric);if(crop!=null)return "本任务新增收获 "+c.amount+" 单位"+crop.name;
         if(c.metric.StartsWith("tech:"))return "研究「"+VerdantContent.Tech(c.metric.Substring(5)).name+"」";
         string name=c.metric=="moves"?"移动格数":c.metric=="planted"?"成功播种次数":c.metric=="watered"?"主动浇水次数":c.metric=="fertilized"?"成功施肥次数":c.metric=="energyEarned"?"累计生产光能":c.metric=="bonusHarvests"?"触发增产的收获次数":c.metric=="functionCalls"?"函数调用次数":c.metric=="harvested"?"累计收获单位":c.metric=="size"?"农田边长":"自动化动作次数";
         return name+"达到 "+c.amount;}
        void SetProgress(float amount){var rect=taskBar.rectTransform;rect.anchorMax=new Vector2(Mathf.Clamp01(amount),1);}
        public void AddLog(string text)
        {
            text=text.Replace("<","‹").Replace(">","›");log.Add("· "+text);if(log.Count>6)log.RemoveAt(0);if(ConsoleText!=null)ConsoleText.text=string.Join("\n",log.ToArray());
        }
        public void Close(){if(modal!=null)Destroy(modal.gameObject);modal=null;modalKind=null;App.PausedWorld=false;}
        public void Open(string kind)
        {
            if(modal!=null)Close();modalKind=kind;App.PausedWorld=kind!="programs";
            modal=Panel("Modal",canvas,Vector2.zero,Vector2.one,Vector2.zero,Vector2.zero,new Color(.08f,.16f,.12f,.72f));modal.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;
            var card=PanelFixed("Card",modal,new Vector2(.5f,.5f),Vector2.zero,new Vector2(1060,780),Paper);
            modalTitle=Text("Title","",card,new Vector2(0,1),new Vector2(32,-24),new Vector2(850,45),30,Ink,FontStyles.Bold);
            Button("关闭",card,new Vector2(1,1),new Vector2(-28,-24),new Vector2(90,36),Close,Accent);
            modalScroll=New("Scroll",card,typeof(ScrollRect)).GetComponent<ScrollRect>();var scrollRect=(RectTransform)modalScroll.transform;
            Stretch(scrollRect,Vector2.zero,Vector2.one,new Vector2(28,26),new Vector2(-28,-90));
            var viewport=New("Viewport",scrollRect,typeof(UnityEngine.UI.Image),typeof(UnityEngine.UI.Mask));Stretch((RectTransform)viewport.transform,Vector2.zero,Vector2.one,Vector2.zero,Vector2.zero);viewport.GetComponent<UnityEngine.UI.Image>().color=Paper;viewport.GetComponent<UnityEngine.UI.Mask>().showMaskGraphic=false;
            modalBody=(RectTransform)New("Content",viewport.transform,typeof(VerticalLayoutGroup),typeof(ContentSizeFitter)).transform;
            modalBody.anchorMin=new Vector2(0,1);modalBody.anchorMax=Vector2.one;modalBody.pivot=new Vector2(.5f,1);modalBody.sizeDelta=Vector2.zero;
            var layout=modalBody.GetComponent<VerticalLayoutGroup>();layout.spacing=12;layout.childControlWidth=true;layout.childControlHeight=true;layout.childForceExpandWidth=true;layout.childForceExpandHeight=false;layout.padding=new RectOffset(4,4,4,4);
            modalBody.GetComponent<ContentSizeFitter>().verticalFit=ContentSizeFitter.FitMode.PreferredSize;
            modalScroll.viewport=(RectTransform)viewport.transform;modalScroll.content=modalBody;modalScroll.horizontal=false;modalScroll.scrollSensitivity=35;modalScroll.movementType=ScrollRect.MovementType.Clamped;
            Populate(kind);Canvas.ForceUpdateCanvases();modalScroll.verticalNormalizedPosition=1;
        }
        void Populate(string kind)
        {
            var f=App.Farm;
            if(kind=="intro")
            {
                modalTitle.text="欢迎来到禾湾";
                Paragraph("禾序  /  VERDANT CIRCUIT",26,Accent);
                Paragraph("一座无人照看的农场，一台巡田机器人，一份等待你写下的程序。\n\n从手动种下第一粒稻穗，到让循环与函数照看整片农田。你将在六章委托中学习规律、建设水利、研究共生，最终让星辉温室持续运转。",20,Ink,200);
                Row("先认识土地","方向键移动；空格播种；E 收获。左侧委托会告诉你下一步。","开始农耕",()=>{f.Add("intro");Close();},110);
                Row("让程序接手","右侧已准备好稻穗巡田程序。随时可以停止、编辑和单步。","运行示例",()=>{f.Add("intro");Close();App.RunCode();},110);
                Paragraph("内容：8 种作物 · 30 份委托 · 18 项研究 · 6 项试炼 · 5 种加工配方\n农场与程序列表会自动保存。",16,Muted,75);
            }
            else if(kind=="manual")
            {modalTitle.text="农场语言手册";Paragraph(VerdantContent.Manual,18,Ink,2100);}
            else if(kind=="templates")
            {
                modalTitle.text="程序范例";Paragraph("范例会替换当前编辑区。程序列表可以保存不同方案。代码可以自由修改。",17,Muted,55);
                for(int i=0;i<VerdantContent.TemplateNames.Length;i++){int n=i;Row((i+1).ToString("00")+"  "+VerdantContent.TemplateNames[i],i<2?"从基础动作到整田循环":"结合传感器和作物规则设计自动化方案","载入",()=>LoadTemplate(n),82);}
            }
            else if(kind=="programs")
            {modalTitle.text="程序管理";Paragraph("查看其他程序不会停止当前执行。点击运行将替换当前程序，始终仅执行一份。删除运行中的程序前请先停止。",17,Muted,65);
             Row("新建程序","默认一个，可增加和删除，至少保留一个。","增加",AddProgram,80,!f.InChallenge);
             for(int i=0;i<f.Data.scripts.Length;i++){int n=i;Row("程序 "+(i+1)+(App.Runner.Running&&App.RunningSlot==i?" · 运行中":""),f.Data.scripts[i].Length+" 字符","查看",()=>{SelectSlot(n);Close();},80);Row("删除程序 "+(i+1),"删除后不可从本列表恢复，请先复制需要的代码。","删除",()=>DeleteProgram(n),75,f.Data.scripts.Length>1&&!(App.Runner.Running&&App.RunningSlot==i));}}
            else if(kind=="research")
            {
                modalTitle.text="科技路线";Paragraph("研究需要金币与研究点。章节随主线委托解锁；完成前置研究后即可购买。",17,Muted,55);
                BuildTechTree();
            }
            else if(kind=="crops")
            {
                modalTitle.text="作物图鉴与仓库";
                foreach(var c in VerdantContent.Crops){var crop=c;Row(c.name+(f.Unlocked(c.id)?"":" · 未解锁"),c.note+"\n生长 "+c.seconds+" 秒 · 播种 "+c.cost+" 金币 · 单位价值 "+c.value+" 金币\n库存 / 累计收获："+f.Count("stock:"+c.id),"选择",()=>{f.Data.selectedCrop=Array.IndexOf(VerdantContent.Crops,crop);Close();Refresh();},122);}
            }
            else if(kind=="challenges")
            {
                modalTitle.text="独立试炼";Paragraph("每项试炼都使用独立的 6×6 农场，临时解锁全部研究。完成后恢复主线农场；首次通过奖励 400 金币与 50 研究点。",17,Muted,80);
                for(int i=0;i<6;i++){int n=i;string stars=new string('★',f.Data.stars[i])+new string('☆',3-f.Data.stars[i]);Row((i+1)+". "+VerdantContent.ChallengeNames[i]+"   "+stars,VerdantContent.ChallengeNotes[i],i>f.Chapter?"章节锁定":"开始",()=>{App.Runner.Stop();f.StartChallenge(n);if(f.InChallenge){Close();int template=n==0?0:n==1?1:n==2?2:n==3?4:n==4?5:6;Editor.SetTextWithoutNotify(VerdantContent.Templates[template]);AddLog("点击运行开始耕作。退出试炼请打开设置。");}},110,i<=f.Chapter&&!f.InChallenge);}
            }
            else if(kind=="achievements")
            {
                modalTitle.text="田野档案  /  "+f.Data.achievements.Count+" / 12";
                for(int i=0;i<12;i++){bool done=f.Data.achievements.Contains(i.ToString());Row(VerdantContent.AchievementNames[i],VerdantContent.AchievementNotes[i]+" · 首次解锁奖励 10 研究",done?"已解锁":"进行中",null,86,false);}
            }
            else if(kind=="orders")
            {
                modalTitle.text="禾湾供给订单";
                Paragraph("第二章起开放。交付会消耗仓库库存，并获得供给补贴与研究点。每次完成后都会出现新订单；已完成 "+f.Count("orders")+" 单。",17,Muted,70);
                for(int i=0;i<3;i++){int slot=i;var o=VerdantEconomy.Order(f,i);Row(o.title,VerdantEconomy.Name(o.a)+" "+f.Count("stock:"+o.a)+" / "+o.amountA+"   +   "+VerdantEconomy.Name(o.b)+" "+f.Count("stock:"+o.b)+" / "+o.amountB+"\n补贴 "+o.coins+" 金币 / "+o.knowledge+" 研究",f.Chapter<1?"第二章解锁":"交付",()=>{if(VerdantEconomy.Deliver(f,slot)){App.Audio.PlayCue(740);Open("orders");}},115,f.Chapter>=1&&!f.InChallenge);}
                Paragraph("加工品订单会在第三章及对应作物解锁后加入。合理保留库存，可以让工坊和农田相互支持。",17,Muted,70);
            }
            else if(kind=="workshop")
            {
                modalTitle.text="加工工坊";Paragraph("第三章开放。将库存作物加工为高价值补给品，再完成供给订单。配方会消耗材料，并额外提供研究点。",17,Muted,70);
                foreach(var r in VerdantEconomy.Recipes){var recipe=r;Row(r.name+"   库存 "+f.Count("stock:"+r.id),r.note+"\n"+VerdantEconomy.Name(r.a)+" "+r.amountA+" + "+VerdantEconomy.Name(r.b)+" "+r.amountB+(r.energy>0?" + 光能 "+r.energy:"")+"   / 研究 +"+r.knowledge,VerdantEconomy.Available(f,r)?"加工":"尚未解锁",()=>{if(VerdantEconomy.Craft(f,recipe)){App.Audio.PlayCue(650);float position=modalScroll.verticalNormalizedPosition;Open("workshop");Canvas.ForceUpdateCanvases();modalScroll.verticalNormalizedPosition=position;}},110,VerdantEconomy.Available(f,r)&&!f.InChallenge);}
            }
            else if(kind=="settings")
            {
                modalTitle.text="设置与存档";
                Row("农场时速","当前 "+App.Speed+"×。只影响生长和程序动作，界面始终正常响应。","切换速度",()=>{App.CycleSpeed();Open("settings");},90);
                Row("声音","轻柔合成音乐与操作音效；当前 "+Mathf.RoundToInt(f.Data.volume*100)+"%。","切换音量",()=>{f.Data.volume=f.Data.volume>=.5f?0:f.Data.volume+.25f;App.Audio.SetVolume(f.Data.volume);Open("settings");},90);
                Row("保存农场","保存农田、资源、研究与程序列表。上一份存档会保留为备份。","立即保存",()=>{f.Save();AddLog(f.LastSaveMessage);},90);
                if(f.InChallenge)Row("返回主线农场","放弃当前临时试炼，恢复进入前的主线进度。","退出试炼",()=>{App.Runner.Stop();f.FinishChallenge(false);Editor.SetTextWithoutNotify(f.Data.scripts[f.Data.slot]);Close();},90);
                Row("重新开始","会先保存一份带时间戳的备份，再建立新的农场。","新建农场",()=>Open("confirm"),90);
                Paragraph("禾序 "+VerdantContent.Version+"\n程序、场景与农艺系统为本项目原创。字体使用 Noto Sans CJK（SIL OFL）。\n存档位置：\n"+f.SavePath,14,Muted,110);
                Row("退出游戏","退出前会自动保存。","保存并退出",()=>App.Quit(),80);
            }
            else if(kind=="confirm")
            {modalTitle.text="重新开始农场？";Paragraph("当前进度会先保存为带日期的备份。新的主线将从 4×4 农田开始。",21,Ink,120);Row("创建新农场","此操作会停止当前程序。","备份并开始",()=>{Close();App.NewCampaign();},100);}
        }
        void Paragraph(string value,int size,Color color,int height=60)
        {var t=Text("Paragraph",value,modalBody,new Vector2(0,1),Vector2.zero,new Vector2(960,height),size,color);t.enableWordWrapping=true;var e=t.gameObject.AddComponent<LayoutElement>();e.preferredHeight=height;e.flexibleWidth=1;}
        void Row(string title,string detail,string button,Action action,int height,bool enabled=true)
        {
            var row=PanelFixed("Entry",modalBody,new Vector2(0,1),Vector2.zero,new Vector2(970,height),VerdantWorld.Hex("ECEFE4"));row.gameObject.AddComponent<LayoutElement>().preferredHeight=height;
            Text("Title",title,row,new Vector2(0,1),new Vector2(18,-12),new Vector2(770,30),20,Ink,FontStyles.Bold);
            var description=Text("Detail",detail,row,new Vector2(0,1),new Vector2(18,-47),new Vector2(770,height-50),15,Muted);description.enableWordWrapping=true;
            var b=Button(button,row,new Vector2(1,.5f),new Vector2(-16,0),new Vector2(142,38),action,Accent);b.interactable=enabled;
        }
        GameObject New(string name,Transform parent,params Type[] components)
        {var go=new GameObject(name,typeof(RectTransform));go.transform.SetParent(parent,false);foreach(var type in components)go.AddComponent(type);return go;}
        static void Stretch(RectTransform r,Vector2 min,Vector2 max,Vector2 low,Vector2 high){r.anchorMin=min;r.anchorMax=max;r.pivot=new Vector2(.5f,.5f);r.offsetMin=low;r.offsetMax=high;}
        RectTransform Panel(string name,Transform parent,Vector2 min,Vector2 max,Vector2 low,Vector2 high,Color color)
        {var go=New(name,parent,typeof(UnityEngine.UI.Image));var rect=(RectTransform)go.transform;Stretch(rect,min,max,low,high);var image=go.GetComponent<UnityEngine.UI.Image>();image.color=color;image.raycastTarget=false;return rect;}
        RectTransform PanelFixed(string name,Transform parent,Vector2 anchor,Vector2 position,Vector2 size,Color color)
        {var rect=Panel(name,parent,anchor,anchor,Vector2.zero,Vector2.zero,color);rect.pivot=anchor;rect.anchoredPosition=position;rect.sizeDelta=size;return rect;}
        TMP_Text Text(string name,string value,Transform parent,Vector2 anchor,Vector2 position,Vector2 size,int fontSize,Color color,FontStyles style=FontStyles.Normal,TextAlignmentOptions align=TextAlignmentOptions.TopLeft)
        {
            var go=New(name,parent,typeof(TextMeshProUGUI));var rect=(RectTransform)go.transform;rect.anchorMin=rect.anchorMax=anchor;rect.pivot=anchor;rect.anchoredPosition=position;rect.sizeDelta=size;
            var t=go.GetComponent<TextMeshProUGUI>();t.font=font;t.text=value;t.fontSize=fontSize;t.fontStyle=style;t.color=color;t.alignment=align;t.raycastTarget=false;t.enableAutoSizing=false;t.enableWordWrapping=false;return t;
        }
        UnityEngine.UI.Button Button(string value,Transform parent,Vector2 anchor,Vector2 position,Vector2 size,Action action,Color color)
        {
            var r=PanelFixed(value,parent,anchor,position,size,color);if(Mathf.Abs(anchor.y-.30f)<.001f)r.pivot=new Vector2(anchor.x,1);var image=r.GetComponent<UnityEngine.UI.Image>();image.raycastTarget=true;
            var button=r.gameObject.AddComponent<UnityEngine.UI.Button>();button.targetGraphic=image;var colors=button.colors;colors.highlightedColor=new Color(1.13f,1.13f,1.13f);colors.pressedColor=new Color(.8f,.8f,.8f);colors.disabledColor=new Color(.7f,.7f,.7f,.6f);button.colors=colors;
            if(action!=null)button.onClick.AddListener(()=>{App.Audio.PlayCue(430);action();});
            var text=Text("Label",value,r,new Vector2(.5f,.5f),Vector2.zero,new Vector2(size.x-8,size.y-4),15,Paper,FontStyles.Normal,TextAlignmentOptions.Center);return button;
        }
    }
}
