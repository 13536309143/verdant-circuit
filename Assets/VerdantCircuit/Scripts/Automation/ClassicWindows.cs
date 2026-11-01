using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
namespace Verdant.Automation
{
    public sealed partial class ClassicController
    {
        readonly Dictionary<ClassicProgram,TMP_InputField> codeWindows=new Dictionary<ClassicProgram,TMP_InputField>();
        void OpenCodeWindow(ClassicProgram program)
        {
            TMP_InputField old;if(codeWindows.TryGetValue(program,out old)&&old!=null){old.transform.parent.SetAsLastSibling();return;}
            float shift=(codeWindows.Count%4)*.035f;var window=Panel("Code Window",canvas,new Vector2(.12f+shift,.18f-shift),new Vector2(.58f+shift,.83f-shift),Dark);
            var title=Label(program.name+".py",window,new Vector2(.025f,.93f),new Vector2(.77f,.99f),21,Color.white);title.raycastTarget=true;title.gameObject.AddComponent<ClassicDrag>().target=window;
            Button(Tr("Close","关闭"),window,new Vector2(.8f,.935f),new Vector2(.98f,.99f),()=>{codeWindows.Remove(program);Destroy(window.gameObject);});
            var field=MakeInput("Floating Code",window,new Vector2(.02f,.1f),new Vector2(.98f,.92f),true);field.SetTextWithoutNotify(program.source);codeWindows[program]=field;
            var history=new CodeHistory();history.Record(program.source,0,0);((VerdantCodeInput)field).RestoreHistory=redo=>{var state=redo?history.Redo():history.Undo();if(state!=null)field.text=state.Text;};
            field.onValueChanged.AddListener(value=>{history.Record(value,field.selectionStringAnchorPosition,field.selectionStringFocusPosition);program.source=value;if(Farm.Data.programs[Farm.Data.selectedProgram]==program)Editor.SetTextWithoutNotify(value);});
            Button(Tr("Run separately","独立运行"),window,new Vector2(.02f,.02f),new Vector2(.45f,.085f),()=>{StoreEditor();var d=Session.Launch(program.source);if(d!=null)d.program=program.name;if(d==null)Log(Tr("Drone capacity is full.","无人机数量已达上限。"));else Log(Tr("Started program: ","已启动程序：")+program.name);});
            Button(Tr("Stop all","全部停止"),window,new Vector2(.52f,.02f),new Vector2(.98f,.085f),Session.Stop);
        }
        void SyncCodeWindows(){if(Editor==null)return;var program=Farm.Data.programs[Farm.Data.selectedProgram];TMP_InputField field;if(codeWindows.TryGetValue(program,out field)&&field!=null&&field.text!=Editor.text)field.SetTextWithoutNotify(Editor.text);}
    }
}
