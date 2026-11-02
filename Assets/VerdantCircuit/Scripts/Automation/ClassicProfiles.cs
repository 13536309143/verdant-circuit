using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
namespace Verdant.Automation
{
    public sealed partial class ClassicController
    {
        string FarmFolder {get{return Path.Combine(Application.persistentDataPath,"Automation/Universe0131");}}
        static bool SafeFarmName(string name){return name!=null&&Regex.IsMatch(name,"^[A-Za-z0-9][A-Za-z0-9_.-]{0,63}$")&&!name.Contains("..");}
        void SelectFarmPath(string name){if(!SafeFarmName(name))throw new Exception("Invalid farm name.");SavePath=Path.Combine(FarmFolder,name+".json");PlayerPrefs.SetString("universe0131.activeFarm",name);PlayerPrefs.Save();}
        void ResetPresentation()
        {
            programTarget="earth";programIsland=0;Close();foreach(var f in codeWindows.Values)if(f!=null)Destroy(f.transform.parent.gameObject);codeWindows.Clear();histories.Clear();if(canvas!=null)Destroy(canvas.gameObject);hoverPanel=null;hoverText=null;Volume=Farm.Data.volume;Audio.SetVolume(Volume);BuildUI();
        }
        void CreateFarm(string name)
        {
            if(!SafeFarmName(name)||File.Exists(Path.Combine(FarmFolder,name+".json"))){Log(Tr("Choose a unique farm name using letters, digits, underscores or hyphens.","请使用不重复的农场名称，仅限英文字母、数字、下划线和短横线。"));return;}
            Save();if(race!=null){race.Stop();race=null;}bool watched=Watching;Watch(false);Session.Stop();Farm.Message-=Log;SelectFarmPath(name);Farm=new ClassicFarm();SeedPrograms();Session=new ClassicSession(Farm){ChallengeCompleted=NotifyChallengeCompleted};Farm.Message+=Log;ResetPresentation();Save();if(watched)Watch(true);
        }
        void LoadFarm(string name)
        {
            if(!SafeFarmName(name))return;var path=Path.Combine(FarmFolder,name+".json");var data=JsonUtility.FromJson<ClassicSave>(File.ReadAllText(path));Validate(data);Save();if(race!=null){race.Stop();race=null;}bool watched=Watching;Watch(false);Session.Stop();Farm.Message-=Log;SelectFarmPath(name);Farm=new ClassicFarm{Data=data};Session=new ClassicSession(Farm){ChallengeCompleted=NotifyChallengeCompleted};Farm.Message+=Log;ResetPresentation();if(watched)Watch(true);
        }
        void BuildFarms()
        {
            Paragraph(Tr("Each farm has its own progress, program folder, records and backup. Loading closes the current execution after saving it.","每个农场独立保存进度、程序目录、纪录和备份。载入前会保存当前农场并结束执行。"),85);
            var entry=Row(Tr("Create farm","创建农场"),Tr("Use letters, digits, underscores and hyphens.","名称可用英文字母、数字、下划线和短横线。"),Tr("Create","创建"),()=>CreateFarm(newName.text));newName=MakeInput("FarmName",entry,new Vector2(.02f,.08f),new Vector2(.6f,.38f),false);newName.text="farm_"+DateTime.Now.ToString("yyyyMMdd_HHmmss");Directory.CreateDirectory(FarmFolder);
            foreach(string path in Directory.GetFiles(FarmFolder,"*.json").Where(p=>!Path.GetFileName(p).StartsWith("qa-"))){string name=Path.GetFileNameWithoutExtension(path);if(SafeFarmName(name))Row(name,File.GetLastWriteTime(path).ToString("yyyy-MM-dd HH:mm"),Tr("Load","载入"),()=>LoadFarm(name));}
        }
    }
}
