using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Localization.Settings;
namespace Verdant.Automation
{
    public static class ClassicDiagnostics
    {
        [Serializable] sealed class Keys {public string[] keys;}
        static string[] prefixes;
        public static string Localize(string text)
        {
            if(string.IsNullOrEmpty(text))return text;if(!LocalizationSettings.InitializationOperation.IsDone)return text;
            var numbered=Regex.Match(text,"^Line ([0-9]+): ([\\s\\S]*)$");if(numbered.Success)return L10n.F("第 {0} 行：{1}",numbered.Groups[1].Value,Localize(numbered.Groups[2].Value));
            var counts=Regex.Match(text,"^Expected ([0-9]+) arguments; received ([0-9]+)\\.$");if(counts.Success)return L10n.F("需要 {0} 个参数，收到 {1} 个。",counts.Groups[1].Value,counts.Groups[2].Value);
            if(text.StartsWith("Save: "))return L10n.F("保存：{0}",Localize(text.Substring(6)));
            const string practice=". Enable unrestricted syntax in Settings to practice.";if(text.EndsWith(practice))return Localize(text.Substring(0,text.Length-practice.Length))+"。"+L10n.T("可在设置中允许未解锁语法以练习。");
            if(prefixes==null){var asset=Resources.Load<TextAsset>("DiagnosticKeys");prefixes=asset==null?new string[0]:JsonUtility.FromJson<Keys>(asset.text).keys;}
            foreach(string prefix in prefixes)if(text.StartsWith(prefix,StringComparison.Ordinal)){string value=LocalizationSettings.StringDatabase.GetLocalizedString("Verdant",L10n.Key(prefix));return value+text.Substring(prefix.Length);}
            if(text.EndsWith(" outside a loop."))return text.Substring(0,text.Length-16)+LocalizationSettings.StringDatabase.GetLocalizedString("Verdant",L10n.Key(" outside a loop."));return text;
        }
    }
}
