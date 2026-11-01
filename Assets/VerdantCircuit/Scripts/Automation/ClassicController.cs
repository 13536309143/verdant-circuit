using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
namespace Verdant.Automation
{
    public sealed partial class ClassicController:MonoBehaviour
    {
        public ClassicFarm Farm;public ClassicSession Session;public ClassicView View;public TMP_InputField Editor;public int Speed=1;public int RunningProgram=-1;public string SavePath;public bool Watching;public float Volume=.25f;
        public readonly List<string> Logs=new List<string>();public TMP_FontAsset Font;public VerdantAudio Audio;
        readonly Dictionary<int,CodeHistory> histories=new Dictionary<int,CodeHistory>();FileSystemWatcher watcher;volatile bool filesChanged;float saveTimer,refreshTimer;double lastWrite;bool languageDirty;
        public void Initialize()
        {
            Font=L10n.UIFont;string active=PlayerPrefs.GetString("automation.activeFarm","farm-0.11.1");if(!SafeFarmName(active))active="farm-0.11.1";SavePath=Path.Combine(FarmFolder,active+".json");
#if UNITY_EDITOR
            if(Environment.GetEnvironmentVariable("VERDANT_AUTOMATION_QA")=="1"){SavePath=Path.Combine(Application.persistentDataPath,"Automation","qa-0.11.1.json");if(File.Exists(SavePath))File.Delete(SavePath);}
#endif
            Load();Volume=Farm.Data.volume;Session=new ClassicSession(Farm);Farm.Message+=Log;
            Audio=new GameObject("Sound Garden").AddComponent<VerdantAudio>();Audio.transform.SetParent(transform);Audio.Initialize(Volume);
            View=new GameObject("Farm View").AddComponent<ClassicView>();View.transform.SetParent(transform);View.Initialize(this);BuildUI();L10n.Changed+=LanguageChanged;
            Log(Tr("Welcome. Harvest grass to earn hay, then unlock planting and expansion.","欢迎。先收获草获得干草，再解锁种植和扩田。"));
        }
        void LanguageChanged(){languageDirty=true;}
        void Update()
        {
            if(Farm==null)return;if(race==null)Session.Tick(Time.unscaledDeltaTime,Speed);TickRace();if(!Session.Running&&RunningProgram>=0){RunningProgram=-1;Save();}
            if(Editor!=null&&Editor.isFocused&&Input.GetKeyDown(KeyCode.Tab)){int n=Editor.stringPosition;Editor.text=Editor.text.Insert(n,"    ");Editor.stringPosition=n+4;}
            if(modal==null&&Editor!=null&&!Editor.isFocused&&View.Camera.pixelRect.Contains(UnityEngine.Input.mousePosition))
            {
                if(UnityEngine.Input.GetMouseButtonDown(0)){var ray=View.Camera.ScreenPointToRay(UnityEngine.Input.mousePosition);float distance;if(new Plane(Vector3.up,Vector3.zero).Raycast(ray,out distance)){var point=ray.GetPoint(distance);int x=Mathf.RoundToInt(point.x+(Farm.Data.size-1)*.5f),y=Mathf.RoundToInt(point.z+(Farm.Data.size-1)*.5f);if(x>=0&&y>=0&&x<Farm.Data.size&&y<Farm.Data.size){var cell=Farm.Cell(x,y);Log("("+x+", "+y+") / "+cell.entity+" / "+Tr("Water ","水分 ")+(cell.water*100).ToString("F0")+"% / "+Tr("Measure ","测量值 ")+cell.measure+" / "+(Farm.Ready(cell)?Tr("Ready to harvest","可以收获"):Tr("Growing","生长中")));}}}
                if(Math.Abs(UnityEngine.Input.mouseScrollDelta.y)>.01f)View.Camera.orthographicSize=Mathf.Clamp(View.Camera.orthographicSize-UnityEngine.Input.mouseScrollDelta.y*.5f,1.2f,30);
            }
            if(Editor!=null&&Editor.isFocused&&UnityEngine.Input.GetKeyDown(KeyCode.F9)){int position=Math.Max(0,Math.Min(Editor.stringPosition,Editor.text.Length));int line=1+Editor.text.Substring(0,position).Count(c=>c=='\n');if(!Session.breakpoints.Add(line))Session.breakpoints.Remove(line);Log(Tr("Breakpoint line: ","断点行：")+line);}
            bool command=Input.GetKey(KeyCode.LeftControl)||Input.GetKey(KeyCode.RightControl)||Input.GetKey(KeyCode.LeftCommand)||Input.GetKey(KeyCode.RightCommand);
            if(command&&Input.GetKeyDown(KeyCode.Return))Run();if(Input.GetKeyDown(KeyCode.F8))L10n.Set(L10n.Code=="en"?"zh-Hans":"en");if(Input.GetKeyDown(KeyCode.Escape)){if(modal!=null)Close();else Open("settings");}
            if(languageDirty){languageDirty=false;Font=L10n.UIFont;foreach(var t in canvas.GetComponentsInChildren<TMP_Text>(true))t.font=Font;string kind=modalKind;if(kind!=null)Open(kind);Refresh();}
            saveTimer+=Time.unscaledDeltaTime;if(saveTimer>=30){saveTimer=0;Save();}refreshTimer+=Time.unscaledDeltaTime;if(refreshTimer>=.15f){refreshTimer=0;UpdateProduction();Refresh();}
            if(filesChanged&&Watching&&Time.realtimeSinceStartupAsDouble-lastWrite>1){filesChanged=false;ImportFiles();}
        }
        public void Run(){try{StoreEditor();Session.Start(Editor.text).program=Farm.Data.programs[Farm.Data.selectedProgram].name;RunningProgram=Farm.Data.selectedProgram;Close();Log(Tr("Program started.","程序已启动。"));}catch(Exception e){Log(e.Message);}Refresh();}
        public void Log(string message){if(message.StartsWith("Line ")||message.StartsWith("Save: "))message=ClassicDiagnostics.Localize(message);Logs.Add(message);if(Logs.Count>120)Logs.RemoveAt(0);if(console!=null)console.text=string.Join("\n",Logs.Skip(Math.Max(0,Logs.Count-12)));}
        void StoreEditor(){if(Editor!=null&&Farm.Data.selectedProgram<Farm.Data.programs.Count)Farm.Data.programs[Farm.Data.selectedProgram].source=Editor.text;}
        void Switch(int index){StoreEditor();Farm.Data.selectedProgram=index;Editor.SetTextWithoutNotify(Farm.Data.programs[index].source);History().Record(Editor.text,0,0);Refresh();}
        CodeHistory History(){int slot=Farm.Data.selectedProgram;CodeHistory h;if(!histories.TryGetValue(slot,out h)){h=new CodeHistory();histories[slot]=h;}return h;}
        void Restore(bool redo){var state=redo?History().Redo():History().Undo();if(state==null)return;Editor.SetTextWithoutNotify(state.Text);Editor.selectionStringAnchorPosition=state.Anchor;Editor.selectionStringFocusPosition=state.Focus;StoreEditor();}
        void NewProgram(string name,string source="")
        {if(!Regex.IsMatch(name,"^[A-Za-z_]\\w*$")||Farm.Data.programs.Any(p=>p.name==name)){Log(Tr("Choose a unique name using letters, digits and underscores.","请使用不重复的字母、数字、下划线名称。"));return;}StoreEditor();Farm.Data.programs.Add(new ClassicProgram(name,source));Switch(Farm.Data.programs.Count-1);Save();}
        void NewFarm(){string name="farm_"+DateTime.Now.ToString("yyyyMMdd_HHmmss");int suffix=1;string candidate=name;while(File.Exists(Path.Combine(FarmFolder,candidate+".json")))candidate=name+"_"+suffix++;CreateFarm(candidate);}
        void Load()
        {
            Farm=new ClassicFarm();try{if(File.Exists(SavePath)){var data=JsonUtility.FromJson<ClassicSave>(File.ReadAllText(SavePath));Validate(data);Farm.Data=data;}}catch(Exception e){Log("Save: "+e.Message);try{var data=JsonUtility.FromJson<ClassicSave>(File.ReadAllText(SavePath+".bak"));Validate(data);Farm.Data=data;}catch{Farm=new ClassicFarm();}}
            if(Farm.Data.programs.Count==0)SeedPrograms();
        }
        static void Validate(ClassicSave d){if(d==null||d.schema!=1||d.size<1||d.size>32||d.cells==null||d.cells.Count!=d.size*d.size||d.items==null||d.unlocks==null||d.programs==null||d.selectedProgram<0||d.programs.Count>0&&d.selectedProgram>=d.programs.Count||d.cells.Any(c=>c==null)||d.programs.Any(p=>p==null||p.source==null||p.name==null))throw new Exception("Invalid automation save.");if(d.playerX<0||d.playerY<0||d.playerX>=d.size||d.playerY>=d.size||d.maze&&(d.walls==null||d.walls.Length!=d.size*d.size||d.treasure<0||d.treasure>=d.cells.Count)||d.programs.Any(p=>!Regex.IsMatch(p.name,"^[A-Za-z_]\\w*$")||p.source.Length>200000)||d.items.Any(p=>p==null||double.IsNaN(p.amount)||double.IsInfinity(p.amount)||p.amount<0))throw new Exception("Invalid automation state.");if(d.projectId==null||!Regex.IsMatch(d.projectId,"^[a-f0-9]{32}$"))d.projectId=Guid.NewGuid().ToString("N");if(d.records==null)d.records=new List<RaceRecord>();if(d.personalBest==null)d.personalBest=new List<Stock>();if(d.achievements==null)d.achievements=new List<string>();if(d.tail==null)d.tail=new List<int>();}
        void SeedPrograms(){Farm.Data.programs.Add(new ClassicProgram("main","harvest()"));Farm.Data.selectedProgram=0;}
        public void Save()
        {
            if(Farm==null)return;try{StoreEditor();Directory.CreateDirectory(Path.GetDirectoryName(SavePath));string temp=SavePath+".tmp";File.WriteAllText(temp,JsonUtility.ToJson(Farm.Data,true));if(File.Exists(SavePath))File.Copy(SavePath,SavePath+".bak",true);File.Copy(temp,SavePath,true);File.Delete(temp);File.WriteAllText(Path.Combine(Path.GetDirectoryName(SavePath),"output.txt"),string.Join("\n",Logs));if(Watching)ExportFiles();}catch(Exception e){Log("Save: "+e.Message);}
        }
        string CodeFolder {get{return Path.Combine(Path.GetDirectoryName(SavePath),"Programs",Farm.Data.projectId);}}
        void ExportFiles(){StoreEditor();Directory.CreateDirectory(CodeFolder);lastWrite=Time.realtimeSinceStartupAsDouble;foreach(var p in Farm.Data.programs)File.WriteAllText(Path.Combine(CodeFolder,p.name+".py"),p.source);File.WriteAllText(Path.Combine(CodeFolder,"__builtins__.py"),ClassicExamples.EditorStubs);}
        void ImportFiles(){try{foreach(string path in Directory.GetFiles(CodeFolder,"*.py")){string n=Path.GetFileNameWithoutExtension(path);if(n=="__builtins__"||!Regex.IsMatch(n,"^[A-Za-z_]\\w*$"))continue;string text=File.ReadAllText(path);if(text.Length>200000)continue;var p=Farm.Data.programs.Find(x=>x.name==n);if(p==null)Farm.Data.programs.Add(new ClassicProgram(n,text));else p.source=text;}if(Editor!=null&&!Editor.isFocused)Editor.SetTextWithoutNotify(Farm.Data.programs[Farm.Data.selectedProgram].source);Log(Tr("External code reloaded.","已重新载入外部代码。"));}catch(Exception e){Log(e.Message);}}
        void Watch(bool enabled){Watching=enabled;if(watcher!=null){watcher.Dispose();watcher=null;}if(enabled){ExportFiles();watcher=new FileSystemWatcher(CodeFolder,"*.py");watcher.NotifyFilter=NotifyFilters.LastWrite|NotifyFilters.FileName;watcher.Changed+=(s,e)=>filesChanged=true;watcher.Created+=(s,e)=>filesChanged=true;watcher.EnableRaisingEvents=true;}}
        void OnApplicationQuit(){Save();}void OnApplicationPause(bool pause){if(pause)Save();}void OnDestroy(){L10n.Changed-=LanguageChanged;if(watcher!=null)watcher.Dispose();}
        static string Tr(string english,string chinese){return L10n.T(chinese);}
        static string ItemName(string id){switch(id){case "hay":return Tr("Hay","干草");case "wood":return Tr("Wood","木材");case "carrot":return Tr("Carrot","胡萝卜");case "pumpkin":return Tr("Pumpkin","南瓜");case "power":return Tr("Power","能量");case "cactus":return Tr("Cactus","仙人掌");case "bone":return Tr("Bone","骨头");case "gold":return Tr("Gold","宝藏金币");case "water":return Tr("Water","水");case "fertilizer":return Tr("Fertilizer","肥料");case "weird_substance":return Tr("Catalyst","奇异物质");default:return id;}}
    }
}
