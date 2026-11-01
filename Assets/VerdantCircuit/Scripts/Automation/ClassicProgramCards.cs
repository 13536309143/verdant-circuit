using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Verdant.Automation
{
    public sealed class ClassicProgramCard:MonoBehaviour {public string program;public bool editing,running;public int workers;}
    public sealed partial class ClassicController
    {
        void RemoveProgram(ClassicProgram program)
        {
            StoreEditor();int index=Farm.Data.programs.IndexOf(program);
            if(index<0||Farm.Data.programs.Count<=1||Session.drones.Any(d=>!d.finished&&d.program==program.name))return;
            TMP_InputField floating;if(codeWindows.TryGetValue(program,out floating)&&floating!=null){Destroy(floating.transform.parent.gameObject);codeWindows.Remove(program);}
            int selected=Farm.Data.selectedProgram;Farm.Data.programs.RemoveAt(index);if(selected>index)selected--;Farm.Data.selectedProgram=Math.Min(selected,Farm.Data.programs.Count-1);histories.Clear();Editor.SetTextWithoutNotify(Farm.Data.programs[Farm.Data.selectedProgram].source);Save();Open("programs");
        }
        void BuildPrograms()
        {
            StoreEditor();
            var summary=Surface("Program Library Summary",modalBody,Vector2.zero,Vector2.one,Dark);summary.gameObject.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight=105;
            var totals=Label("",summary,new Vector2(.025f,.52f),new Vector2(.975f,.95f),26,Color.white);
            Label(Tr("GREEN = running · AMBER BORDER = editing · GRAY = idle. Browsing preserves existing work.","绿色＝运行中 · 琥珀色边框＝正在编辑 · 灰色＝空闲。切换查看会保留已有运行。"),summary,new Vector2(.025f,.08f),new Vector2(.975f,.48f),15,Lime);
            modalRefreshers.Add(()=>{if(totals!=null)totals.text=Farm.Data.programs.Count+Tr(" programs  /  "," 个程序  /  ")+Farm.Data.programs.Count(p=>Session.drones.Any(d=>!d.finished&&d.program==p.name))+Tr(" active  /  "," 个活跃  /  ")+Session.drones.Count(d=>!d.finished)+" / "+Farm.MaxDrones+Tr(" drone slots"," 无人机槽位");});
            var entry=Row(Tr("New program","新建程序"),Tr("Unique letters, digits and underscores. At least one program is kept.","名称用字母、数字和下划线；至少保留一个程序。"),Tr("Create","创建"),()=>{NewProgram(newName.text);Open("programs");});newName=MakeInput("ProgramName",entry,new Vector2(.02f,.08f),new Vector2(.60f,.38f),false);newName.text="program_"+(Farm.Data.programs.Count+1);
            for(int i=0;i<Farm.Data.programs.Count;i+=2)
            {
                var row=Panel("Program Card Row",modalBody,Vector2.zero,Vector2.one,Color.clear);row.gameObject.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight=225;
                for(int column=0;column<2&&i+column<Farm.Data.programs.Count;column++)BuildProgramCard(Farm.Data.programs[i+column],row,column);
            }
            Refresh();
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
            var preview=Label(program.source.Split('\n').FirstOrDefault(l=>!string.IsNullOrWhiteSpace(l))??Tr("Empty program","空程序"),card,new Vector2(.04f,.25f),new Vector2(.96f,.40f),13,Muted);preview.richText=false;preview.overflowMode=TextOverflowModes.Ellipsis;
            Skin(Button(Tr("Edit","编辑"),card,new Vector2(.04f,.055f),new Vector2(.34f,.23f),()=>{Switch(Farm.Data.programs.IndexOf(program));Close();}),Dark,Color.white);
            Skin(Button(Tr("Window","窗口"),card,new Vector2(.36f,.055f),new Vector2(.66f,.23f),()=>{Close();OpenCodeWindow(program);}),Accent,Color.white);
            var remove=Skin(Button(Tr("Remove","删除"),card,new Vector2(.68f,.055f),new Vector2(.96f,.23f),()=>RemoveProgram(program)),ClassicView.ColorOf("78604E"),Color.white);
            modalRefreshers.Add(()=>
            {
                if(card==null)return;state.editing=Farm.Data.programs[Farm.Data.selectedProgram]==program;state.workers=Session.drones.Count(d=>!d.finished&&d.program==program.name);state.running=state.workers>0;
                card.GetComponent<UnityEngine.UI.Image>().color=state.running?(Farm.Data.theme=="night"?ClassicView.ColorOf("234B3E"):ClassicView.ColorOf("DCEBD9")):Paper;
                stripe.color=state.running?Accent:Muted;outline.effectColor=state.editing?Clay:Color.clear;
                badge.text=(state.running?(Session.Paused?Tr("PAUSED","已暂停"):Tr("RUNNING","运行中")):Tr("IDLE","空闲"))+(state.editing?Tr("  ·  EDITING","  ·  正在编辑"):"");badge.color=state.running?(Farm.Data.theme=="night"?Lime:Accent):Muted;
                info.text=program.source.Length+Tr(" characters  ·  "," 字符  ·  ")+state.workers+Tr(" workers"," 台工作机")+(codeWindows.ContainsKey(program)?Tr("  ·  window open","  ·  窗口已打开"):"");
                remove.interactable=Farm.Data.programs.Count>1&&!state.running;
            });
        }
    }
}
