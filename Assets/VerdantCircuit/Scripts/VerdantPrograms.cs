using System;
namespace Verdant
{
    public static class VerdantPrograms
    {
        public static int Add(FarmSave data)
        {Array.Resize(ref data.scripts,data.scripts.Length+1);data.slot=data.scripts.Length-1;data.scripts[data.slot]=L10n.T("# 新程序\n");return data.slot;}
        public static bool Remove(FarmSave data,int index)
        {
            if(data.scripts.Length<=1||index<0||index>=data.scripts.Length)return false;
            var list=new System.Collections.Generic.List<string>(data.scripts);list.RemoveAt(index);data.scripts=list.ToArray();if(data.slot>index)data.slot--;data.slot=Math.Min(data.slot,data.scripts.Length-1);return true;
        }
    }
}
