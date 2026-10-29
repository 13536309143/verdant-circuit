using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Verdant
{
    public sealed class CodeHistory
    {
        public sealed class Entry {public string Text;public int Anchor,Focus;public Entry(string t,int a,int f){Text=t;Anchor=a;Focus=f;}}
        readonly List<Entry> states=new List<Entry>();int cursor=-1;
        public void Record(string text,int anchor,int focus)
        {
            if(cursor>=0&&states[cursor].Text==text)return;
            if(cursor+1<states.Count)states.RemoveRange(cursor+1,states.Count-cursor-1);
            states.Add(new Entry(text,anchor,focus));cursor++;
            if(states.Count>101){states.RemoveAt(0);cursor--;}
        }
        public Entry Undo(){return cursor>0?states[--cursor]:null;}
        public Entry Redo(){return cursor+1<states.Count?states[++cursor]:null;}
    }
    public sealed class VerdantCodeInput : TMP_InputField
    {
        public Action<bool> RestoreHistory;
        public override void OnUpdateSelected(BaseEventData data)
        {
            bool command=Input.GetKey(KeyCode.LeftCommand)||Input.GetKey(KeyCode.RightCommand)||Input.GetKey(KeyCode.LeftControl)||Input.GetKey(KeyCode.RightControl);
            if(isFocused&&command&&(Input.GetKeyDown(KeyCode.Z)||Input.GetKeyDown(KeyCode.Y)))
            {if(RestoreHistory!=null)RestoreHistory(Input.GetKeyDown(KeyCode.Y)||Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift));data.Use();return;}
            base.OnUpdateSelected(data);
        }
    }
}
