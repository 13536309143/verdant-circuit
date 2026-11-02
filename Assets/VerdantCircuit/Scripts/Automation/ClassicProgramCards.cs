using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Verdant.Automation
{
    public sealed class ClassicProgramCard:MonoBehaviour {public string program;public bool editing,running;public int workers;}
    public sealed partial class ClassicController
    {
        TMP_Text programNameFeedback;
        public int ProgramWorkers(string name){return Session.drones.Count(d=>!d.finished&&d.program==name)+(Farm.Data.archipelago==null?0:Farm.Fleet.ProgramUsers(name));}
        void RemoveProgram(ClassicProgram program)
        {
            StoreEditor();int index=Farm.Data.programs.IndexOf(program);
            if(index<0||Farm.Data.programs.Count<=1||ProgramWorkers(program.name)>0)return;
            TMP_InputField floating;if(codeWindows.TryGetValue(program,out floating)&&floating!=null){Destroy(floating.transform.parent.gameObject);codeWindows.Remove(program);}
            int selected=Farm.Data.selectedProgram;Farm.Data.programs.RemoveAt(index);if(selected>index)selected--;Farm.Data.selectedProgram=Math.Min(selected,Farm.Data.programs.Count-1);histories.Clear();Editor.SetTextWithoutNotify(Farm.Data.programs[Farm.Data.selectedProgram].source);Save();Open("programs");
        }
        void BuildPrograms()
        {
            StoreEditor();var originalBody=modalBody;var panes=Panel("Program Center Columns",modalBody,Vector2.zero,Vector2.one,Color.clear);panes.gameObject.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight=690;
            modalBody=ProgramPane(panes,"Execution Targets",0,.40f);BuildProgramTargets();modalBody=ProgramPane(panes,"Program Files",.415f,1);
            var summary=Surface("Program Library Summary",modalBody,Vector2.zero,Vector2.one,Dark);summary.gameObject.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight=105;
            var totals=Label("",summary,new Vector2(.025f,.52f),new Vector2(.975f,.95f),26,Color.white);
            Label(Tr("GREEN = running · AMBER BORDER = editing · GRAY = idle. Browsing preserves existing work.","绿色＝运行中 · 琥珀色边框＝正在编辑 · 灰色＝空闲。切换查看会保留已有运行。"),summary,new Vector2(.025f,.08f),new Vector2(.975f,.48f),15,Lime);
            modalRefreshers.Add(()=>{if(totals!=null)totals.text=Farm.Data.programs.Count+Tr(" programs  /  "," 个程序  /  ")+Farm.Data.programs.Count(p=>ProgramWorkers(p.name)>0)+Tr(" active  /  "," 个活跃  /  ")+Farm.Fleet.Data.units.Count+" / "+Farm.Fleet.SlotLimit+Tr(" drone slots"," 无人机槽位")+(Farm.Data.archipelago==null?"":"  /  "+Tr("native programs: ","本地载具程序：")+Farm.Fleet.Data.units.Count(u=>Farm.Fleet.ProgramRunning(u.id)));});
            var entry=Surface("New Program Entry",modalBody,Vector2.zero,Vector2.one,Paper);entry.gameObject.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight=140;
            Label(Tr("New program","新建程序"),entry,new Vector2(.02f,.73f),new Vector2(.96f,.97f),21,Ink);
            Label(Tr("Chinese or digits may come first, for example 123, 1_farm or 浇水程序. At least one program is kept.","支持数字或汉字开头，例如 123、1号农场、浇水程序；至少保留一个程序。"),entry,new Vector2(.02f,.52f),new Vector2(.96f,.74f),15,Muted);
            newName=MakeInput("ProgramName",entry,new Vector2(.02f,.22f),new Vector2(.75f,.50f),false);newName.characterLimit=ClassicProgramNames.MaxLength;
            programNameFeedback=Label("",entry,new Vector2(.02f,.01f),new Vector2(.98f,.21f),14,Clay);
            Skin(Button(Tr("Create","创建"),entry,new Vector2(.79f,.22f),new Vector2(.98f,.50f),()=>{if(NewProgram(newName.text))Open("programs");}),Accent,Color.white);
            int suggestion=Farm.Data.programs.Count+1;while(Farm.Data.programs.Any(p=>ClassicProgramNames.Same(p.name,"program_"+suggestion,true)))suggestion++;newName.SetTextWithoutNotify("program_"+suggestion);
            for(int i=0;i<Farm.Data.programs.Count;i+=2)
            {
                var row=Panel("Program Card Row",modalBody,Vector2.zero,Vector2.one,Color.clear);row.gameObject.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight=225;
                for(int column=0;column<2&&i+column<Farm.Data.programs.Count;column++)BuildProgramCard(Farm.Data.programs[i+column],row,column);
            }
            modalBody=originalBody;Refresh();
        }
        void BuildProgramCard(ClassicProgram program,Transform parent,int column)
        {
            var card=Surface("Program Card "+program.name,parent,new Vector2(column*.505f,0),new Vector2(column*.505f+.495f,1),Paper,true);
            var state=card.gameObject.AddComponent<ClassicProgramCard>();state.program=program.name;
            var outline=card.gameObject.AddComponent<UnityEngine.UI.Outline>();outline.effectDistance=new Vector2(2,-2);outline.useGraphicAlpha=true;
            var stripe=Panel("Activity Stripe",card,Vector2.zero,new Vector2(.012f,1),Muted).GetComponent<UnityEngine.UI.Image>();
            var title=Label(program.name+".py",card,new Vector2(.04f,.74f),new Vector2(.96f,.95f),25,Ink);title.overflowMode=TextOverflowModes.Ellipsis;
            var badge=Label("",card,new Vector2(.04f,.55f),new Vector2(.96f,.74f),15,Accent);
            var info=Label("",card,new Vector2(.04f,.40f),new Vector2(.96f,.55f),13,Muted);
            var preview=Label(program.source.Split('\n').FirstOrDefault(l=>!string.IsNullOrWhiteSpace(l))??Tr("Empty program","空程序"),card,new Vector2(.04f,.32f),new Vector2(.96f,.40f),13,Muted);preview.richText=false;preview.overflowMode=TextOverflowModes.Ellipsis;
            var bindings=Label(string.Join(" · ",Farm.Fleet.ProgramBindings(program.name)),card,new Vector2(.04f,.21f),new Vector2(.96f,.31f),12,Accent);bindings.overflowMode=TextOverflowModes.Ellipsis;
            Skin(Button(Tr("Edit","编辑"),card,new Vector2(.04f,.055f),new Vector2(.25f,.20f),()=>{Switch(Farm.Data.programs.IndexOf(program));Close();}),Dark,Color.white);
            Skin(Button(Tr("Window","窗口"),card,new Vector2(.27f,.055f),new Vector2(.49f,.20f),()=>{Close();OpenCodeWindow(program);}),Accent,Color.white);
            Skin(Button(Tr("Bind","绑定"),card,new Vector2(.51f,.055f),new Vector2(.73f,.20f),()=>{RunTarget(program,false);Open("programs");}),Accent,Color.white);
            var remove=Skin(Button(Tr("Remove","删除"),card,new Vector2(.75f,.055f),new Vector2(.96f,.20f),()=>RemoveProgram(program)),ClassicView.ColorOf("78604E"),Color.white);
            modalRefreshers.Add(()=>
            {
                if(card==null)return;state.editing=Farm.Data.programs[Farm.Data.selectedProgram]==program;state.workers=ProgramWorkers(program.name);state.running=state.workers>0;
                card.GetComponent<UnityEngine.UI.Image>().color=state.running?ClassicView.ColorOf("234B3E"):Paper;
                stripe.color=state.running?Accent:Muted;outline.effectColor=state.editing?Clay:Color.clear;
                badge.text=(state.running?(Session.Paused?Tr("PAUSED","已暂停"):Tr("RUNNING","运行中")):Tr("IDLE","空闲"))+(state.editing?Tr("  ·  EDITING","  ·  正在编辑"):"");badge.color=state.running?Lime:Muted;
                info.text=program.source.Length+Tr(" characters  ·  "," 字符  ·  ")+state.workers+Tr(" workers"," 台工作机")+(codeWindows.ContainsKey(program)?Tr("  ·  window open","  ·  窗口已打开"):"");
                remove.interactable=Farm.Data.programs.Count>1&&!state.running;
            });
        }
    }
}
