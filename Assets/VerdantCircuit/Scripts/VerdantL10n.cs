using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
namespace Verdant
{
    public static class L10n
    {
        static readonly Dictionary<string,string> cache=new Dictionary<string,string>();
        static readonly Dictionary<string,string> originals=new Dictionary<string,string>();
        static LocalizedTmpFont uiFont,codeFont;static bool hooked;
        public static TMP_FontAsset UIFont,CodeFont;public static event Action Changed;
        public static string Code {get{return LocalizationSettings.SelectedLocale==null?"en":LocalizationSettings.SelectedLocale.Identifier.Code;}}
        public static string Key(string text)
        {using(var hash=SHA1.Create())return "t_"+BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-","").Substring(0,16).ToLowerInvariant();}
        public static string T(string text)
        {
            if(string.IsNullOrEmpty(text))return text;string original;if(originals.TryGetValue(text,out original))text=original;
            bool needed=false;foreach(char c in text)if(c>=0x3400&&c<=0x9fff||c=='」'){needed=true;break;}if(!needed)return text;
            string value;if(cache.TryGetValue(text,out value))return value;
            if(!LocalizationSettings.InitializationOperation.IsDone)return text;
            value=LocalizationSettings.StringDatabase.GetLocalizedString("Verdant",Key(text));
            if(string.IsNullOrEmpty(value)||value.StartsWith("No translation found"))throw new InvalidOperationException("Missing localized string: "+Key(text)+" / "+Code+" / "+value);
            cache[text]=value;originals[text]=text;originals[value]=text;return value;
        }
        public static string F(string source,params object[] args){return string.Format(CultureInfo.InvariantCulture,T(source),args);}
        public static string[] List(string[] values){var output=new string[values.Length];for(int i=0;i<output.Length;i++)output[i]=T(values[i]);return output;}
        public static void Bind(TMP_Text label,string value)
        {
            string source;if(!originals.TryGetValue(value,out source))return;
            var binding=label.gameObject.AddComponent<LocalizeStringEvent>();binding.enabled=false;
            binding.StringReference=new LocalizedString("Verdant",Key(source));binding.OnUpdateString.AddListener(text=>label.text=text);binding.enabled=true;binding.RefreshString();
        }
        public static IEnumerator Initialize()
        {
            yield return LocalizationSettings.InitializationOperation;
            if(!hooked)
            {
                hooked=true;LocalizationSettings.SelectedLocaleChanged+=locale=>{cache.Clear();PlayerPrefs.SetString("verdant.language",locale.Identifier.Code);PlayerPrefs.Save();};
                uiFont=new LocalizedTmpFont{TableReference="VerdantAssets",TableEntryReference="ui_font"};
                codeFont=new LocalizedTmpFont{TableReference="VerdantAssets",TableEntryReference="code_font"};
                uiFont.AssetChanged+=font=>{UIFont=font;if(Changed!=null)Changed();};codeFont.AssetChanged+=font=>{CodeFont=font;if(Changed!=null)Changed();};
            }
            yield return uiFont.LoadAssetAsync();yield return codeFont.LoadAssetAsync();
        }
        public static void Set(string code)
        {var locale=LocalizationSettings.AvailableLocales.GetLocale(code);if(locale==null)throw new ArgumentException("Unsupported locale: "+code);LocalizationSettings.SelectedLocale=locale;PlayerPrefs.SetString("verdant.language",code);PlayerPrefs.Save();}
        public static void ResetCache(){cache.Clear();}
    }
}
