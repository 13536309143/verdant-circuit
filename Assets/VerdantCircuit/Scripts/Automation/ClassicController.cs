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
            Font=L10n.UIFont;string active=PlayerPrefs.GetString("universe0131.activeFarm","earth-0.13.1");if(!SafeFarmName(active))active="earth-0.13.1";SavePath=Path.Combine(FarmFolder,active+".json");
#if UNITY_EDITOR
            if(Environment.GetEnvironmentVariable("VERDANT_AUTOMATION_QA")=="1"){SavePath=Path.Combine(Application.persistentDataPath,"Automation/Universe0131","qa-0.11.2.json");if(File.Exists(SavePath))File.Delete(SavePath);}
#endif
            Load();activePlanet=Farm.Data.selectedPlanet;Volume=Farm.Data.volume;Session=new ClassicSession(Farm){ChallengeCompleted=NotifyChallengeCompleted};Farm.Message+=Log;Farm.Fleet.Breakpoints=Session.breakpoints;
            Audio=new GameObject("Sound Garden").AddComponent<VerdantAudio>();Audio.transform.SetParent(transform);Audio.Initialize(Volume);
            View=new GameObject("Farm View").AddComponent<ClassicView>();View.transform.SetParent(transform);View.Initialize(this);BuildUI();L10n.Changed+=LanguageChanged;
            Log(Tr("Welcome. Harvest grass to earn hay, then unlock planting and expansion.","欢迎。先收获草获得干草，再解锁种植和扩田。"));PlayOpening();
        }
        ClassicOpening opening;int openingClosedFrame=-1;
        public bool OpeningActive {get{return opening!=null&&opening.Playing;}}
        public void PlayOpening()
        {
            if(OpeningActive)return;StoreEditor();Close();if(Editor!=null)Editor.DeactivateInputField();
            foreach(var field in codeWindows.Values)if(field!=null)field.DeactivateInputField();
            if(UnityEngine.EventSystems.EventSystem.current!=null)UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            var go=new GameObject("Verdant Opening");go.transform.SetParent(transform,false);opening=go.AddComponent<ClassicOpening>();opening.Initialize(Font);canvas.gameObject.SetActive(false);
            opening.Finished+=()=>{openingClosedFrame=Time.frameCount;if(canvas!=null){canvas.gameObject.SetActive(true);Open("world");}};
        }
        public void SkipOpening(){if(opening!=null)opening.Skip();}
        void LanguageChanged(){languageDirty=true;}
        void Update()
        {
            if(Farm==null||OpeningActive||Time.frameCount==openingClosedFrame)return;
            if(WorldSelectionActive&&!hasEnteredPlanet){if(languageDirty){languageDirty=false;Font=L10n.UIFont;Open("world");}return;}if(race==null)Session.Tick(Time.unscaledDeltaTime,Speed);TickRace();if(!Session.Running&&RunningProgram>=0){RunningProgram=-1;Save();}
            if(Editor!=null&&Editor.isFocused&&Input.GetKeyDown(KeyCode.Tab)){int n=Editor.stringPosition;Editor.text=Editor.text.Insert(n,"    ");Editor.stringPosition=n+4;}
            if(Editor!=null&&Editor.isFocused&&UnityEngine.Input.GetKeyDown(KeyCode.F9)){int position=Math.Max(0,Math.Min(Editor.stringPosition,Editor.text.Length));int line=1+Editor.text.Substring(0,position).Count(c=>c=='\n');if(!Session.breakpoints.Add(line))Session.breakpoints.Remove(line);Log(Tr("Breakpoint line: ","断点行：")+line);}
            bool command=Input.GetKey(KeyCode.LeftControl)||Input.GetKey(KeyCode.RightControl)||Input.GetKey(KeyCode.LeftCommand)||Input.GetKey(KeyCode.RightCommand);
            if(command&&Input.GetKeyDown(KeyCode.Return))Run();if(Input.GetKeyDown(KeyCode.F2)&&!CompletionOpen)Open("research");if(Input.GetKeyDown(KeyCode.F8))L10n.Set(L10n.Code=="en"?"zh-Hans":"en");if(Input.GetKeyDown(KeyCode.Escape)){if(CompletionOpen)DismissCompletion();else if(modal!=null)Close();else Open("settings");}
            if(languageDirty){languageDirty=false;RebuildLocaleUi();}

            saveTimer+=Time.unscaledDeltaTime;if(saveTimer>=30){saveTimer=0;Save();}refreshTimer+=Time.unscaledDeltaTime;if(refreshTimer>=.15f){refreshTimer=0;UpdateProduction();Refresh();}
            TickResearchNavigation();TickCompletionNotice();
            if(filesChanged&&Watching&&Time.realtimeSinceStartupAsDouble-lastWrite>1){filesChanged=false;ImportFiles();}
        }
        public void Run(){if(CompletionOpen||OpeningActive||Time.frameCount==openingClosedFrame)return;try{StoreEditor();RunTarget(Farm.Data.programs[Farm.Data.selectedProgram]);Close();}catch(Exception e){Log(e.Message);}Refresh();}
        public void Log(string message){if(message.StartsWith("Line ")||message.StartsWith("Save: "))message=ClassicDiagnostics.Localize(message);Logs.Add(message);if(Logs.Count>120)Logs.RemoveAt(0);if(console!=null)console.text=string.Join("\n",Logs.Skip(Math.Max(0,Logs.Count-8)));}
        void StoreEditor(){if(Editor!=null&&Farm.Data.selectedProgram<Farm.Data.programs.Count)Farm.Data.programs[Farm.Data.selectedProgram].source=Editor.text;}
        void Switch(int index){StoreEditor();Farm.Data.selectedProgram=index;Editor.SetTextWithoutNotify(Farm.Data.programs[index].source);History().Record(Editor.text,0,0);Refresh();}
        CodeHistory History(){int slot=Farm.Data.selectedProgram;CodeHistory h;if(!histories.TryGetValue(slot,out h)){h=new CodeHistory();histories[slot]=h;}return h;}
        void Restore(bool redo){var state=redo?History().Redo():History().Undo();if(state==null)return;Editor.SetTextWithoutNotify(state.Text);Editor.selectionStringAnchorPosition=state.Anchor;Editor.selectionStringFocusPosition=state.Focus;StoreEditor();}
        bool ProgramNameError(string message){Log(message);if(programNameFeedback!=null)programNameFeedback.text=message;return false;}
        bool NewProgram(string name,string source="")
        {
            name=ClassicProgramNames.Normalize(name);
            if(string.IsNullOrEmpty(name))return ProgramNameError(Tr("Enter a program name.","请输入程序名称。"));
            if(ClassicProgramNames.IsReserved(name))return ProgramNameError(Tr("This name is reserved. Choose another name.","这个名称已被系统保留，请换一个名称。"));
            if(!ClassicProgramNames.IsCreatable(name))return ProgramNameError(Tr("Use Chinese characters, letters, digits or underscores, up to 80 characters. Do not add .py.","请使用汉字、字母、数字或下划线，最多80个字符，不需要填写 .py。"));
            if(Farm.Data.programs.Any(p=>ClassicProgramNames.Same(p.name,name,true)))return ProgramNameError(Tr("This program name already exists. Names cannot differ only by letter case.","程序名称已存在；仅修改英文字母大小写也会重名。"));
            StoreEditor();Farm.Data.programs.Add(new ClassicProgram(name,source));Switch(Farm.Data.programs.Count-1);Save();return true;
        }
        void NewFarm(){string name="farm_"+DateTime.Now.ToString("yyyyMMdd_HHmmss");int suffix=1;string candidate=name;while(File.Exists(Path.Combine(FarmFolder,candidate+".json")))candidate=name+"_"+suffix++;CreateFarm(candidate);}
        void Load()
        {
            Farm=new ClassicFarm();try{if(File.Exists(SavePath)){var data=JsonUtility.FromJson<ClassicSave>(File.ReadAllText(SavePath));Validate(data);Farm.Data=data;}}catch(Exception e){Log("Save: "+e.Message);try{var data=JsonUtility.FromJson<ClassicSave>(File.ReadAllText(SavePath+".bak"));Validate(data);Farm.Data=data;}catch{Farm=new ClassicFarm();}}
            if(Farm.Data.programs.Count==0)SeedPrograms();
        }
        static void Validate(ClassicSave d){if(d!=null&&ArchipelagoWorld.EmptyPlaceholder(d.archipelago))d.archipelago=null;if(d!=null&&d.archipelago!=null){var normalizer=new ClassicFarm{Data=d};var normalized=normalizer.Fleet;}ArchipelagoWorld.Validate(d==null?null:d.archipelago);if(d==null||d.schema!=2||d.size<1||d.size>360||d.cells==null||d.height<1||d.height>128||d.selectedPlanet<0||d.selectedPlanet>3||d.cells.Count!=d.size*d.height||d.items==null||d.unlocks==null||d.programs==null||d.selectedProgram<0||d.programs.Count>0&&d.selectedProgram>=d.programs.Count||d.cells.Any(c=>c==null)||d.programs.Any(p=>p==null||p.source==null||p.name==null))throw new Exception("Invalid automation save.");if(d.completionNotices!=null&&d.completionNotices.Any(n=>n==null||n.kind=="archipelago"&&(!int.TryParse(n.id,out int orderIndex)||orderIndex<0||orderIndex>=ArchipelagoWorld.Contracts.Length)))throw new Exception("Invalid saved island completion notice.");if(d.playerX<0||d.playerY<0||d.playerX>=d.size||d.playerY>=d.height||d.maze&&(d.walls==null||d.walls.Length!=d.size*d.height||d.treasure<0||d.treasure>=d.cells.Count)||d.programs.Any(p=>!ClassicProgramNames.IsStoredNameSafe(p.name)||p.source.Length>200000)||d.items.Any(p=>p==null||double.IsNaN(p.amount)||double.IsInfinity(p.amount)||p.amount<0))throw new Exception("Invalid automation state.");if(d.archipelago!=null){var actual=new ClassicFarm{Data=d};if(actual.Fleet.Data.units.Any(u=>!actual.Fleet.IsUnlocked(u.island)||!actual.Fleet.Fits(u,u.x,u.y)))throw new Exception("Saved worker exceeds its real spherical surface.");}if(d.projectId==null||!Regex.IsMatch(d.projectId,"^[a-f0-9]{32}$"))d.projectId=Guid.NewGuid().ToString("N");if(d.records==null)d.records=new List<RaceRecord>();if(d.personalBest==null)d.personalBest=new List<Stock>();if(d.achievements==null)d.achievements=new List<string>();if(d.tail==null)d.tail=new List<int>();}
        void SeedPrograms(){Farm.Data.programs.Add(new ClassicProgram("main","harvest()"));Farm.Data.selectedProgram=0;}
        public void Save()
        {
            if(Farm==null)return;try{StoreEditor();Directory.CreateDirectory(Path.GetDirectoryName(SavePath));string temp=SavePath+".tmp";File.WriteAllText(temp,JsonUtility.ToJson(Farm.Data,true));if(File.Exists(SavePath))File.Copy(SavePath,SavePath+".bak",true);File.Copy(temp,SavePath,true);File.Delete(temp);File.WriteAllText(Path.Combine(Path.GetDirectoryName(SavePath),"output.txt"),string.Join("\n",Logs));if(Watching)ExportFiles();}catch(Exception e){Log("Save: "+e.Message);}
        }
        string CodeFolder {get{return Path.Combine(Path.GetDirectoryName(SavePath),"Programs",Farm.Data.projectId);}}
        void ExportFiles(){StoreEditor();Directory.CreateDirectory(CodeFolder);lastWrite=Time.realtimeSinceStartupAsDouble;foreach(var p in Farm.Data.programs)File.WriteAllText(Path.Combine(CodeFolder,p.name+".py"),p.source);File.WriteAllText(Path.Combine(CodeFolder,"__builtins__.py"),ClassicExamples.EditorStubs);}
        void ImportFiles(){try{foreach(string path in Directory.GetFiles(CodeFolder,"*.py")){string n=ClassicProgramNames.Normalize(Path.GetFileNameWithoutExtension(path));if(!ClassicProgramNames.IsCreatable(n))continue;string text=File.ReadAllText(path);if(text.Length>200000)continue;var p=Farm.Data.programs.Find(x=>ClassicProgramNames.Same(x.name,n,true));if(p==null)Farm.Data.programs.Add(new ClassicProgram(n,text));else p.source=text;}if(Editor!=null&&!Editor.isFocused)Editor.SetTextWithoutNotify(Farm.Data.programs[Farm.Data.selectedProgram].source);Log(Tr("External code reloaded.","已重新载入外部代码。"));}catch(Exception e){Log(e.Message);}}
        void Watch(bool enabled){Watching=enabled;if(watcher!=null){watcher.Dispose();watcher=null;}if(enabled){ExportFiles();watcher=new FileSystemWatcher(CodeFolder,"*.py");watcher.NotifyFilter=NotifyFilters.LastWrite|NotifyFilters.FileName;watcher.Changed+=(s,e)=>filesChanged=true;watcher.Created+=(s,e)=>filesChanged=true;watcher.EnableRaisingEvents=true;}}
        void OnApplicationQuit(){Save();}void OnApplicationPause(bool pause){if(pause)Save();}void OnDestroy(){L10n.Changed-=LanguageChanged;if(watcher!=null)watcher.Dispose();}
        static string Tr(string english,string chinese){return L10n.T(chinese);}
        static string ItemName(string id){return UnifiedProductNames.Name(id);}

    }
}
