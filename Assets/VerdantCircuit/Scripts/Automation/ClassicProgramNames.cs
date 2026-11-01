using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Verdant.Automation
{
    public static class ClassicProgramNames
    {
        public const int MaxLength=80;
        static readonly Regex StoredPattern=new Regex(@"\A[\p{L}\p{Nd}_][\p{L}\p{Nd}\p{M}\p{Pc}]*\z",RegexOptions.CultureInvariant);
        static readonly Regex ReservedPattern=new Regex(@"\A(?:CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9]|__builtins__|__main__)\z",RegexOptions.IgnoreCase|RegexOptions.CultureInvariant);
        public static string Normalize(string name){if(name==null)return null;try{return name.Trim().Normalize(NormalizationForm.FormC);}catch(ArgumentException){return null;}}
        // Stored names are checked for path safety without rejecting older long/reserved names.
        public static bool IsStoredNameSafe(string name){return name!=null&&StoredPattern.IsMatch(name);}
        public static bool IsReserved(string name){return name!=null&&ReservedPattern.IsMatch(name);}
        public static bool IsCreatable(string name){return IsStoredNameSafe(name)&&name.Length<=MaxLength&&Encoding.UTF8.GetByteCount(name)<=240&&!IsReserved(name);}
        public static bool Same(string a,string b,bool ignoreCase=false){return string.Equals(Normalize(a),Normalize(b),ignoreCase?StringComparison.OrdinalIgnoreCase:StringComparison.Ordinal);}
    }
    public static class ScriptIdentifier
    {
        public const string Pattern=@"[\p{L}_][\p{L}\p{Nd}\p{M}\p{Pc}]*";
        public static bool IsValid(string name){return name!=null&&Regex.IsMatch(name,@"\A"+Pattern+@"\z",RegexOptions.CultureInvariant);}
        public static bool IsStart(char value){return char.IsLetter(value)||value=='_';}
        public static bool IsContinue(char value){var c=char.GetUnicodeCategory(value);return char.IsLetterOrDigit(value)||c==UnicodeCategory.NonSpacingMark||c==UnicodeCategory.SpacingCombiningMark||c==UnicodeCategory.EnclosingMark||c==UnicodeCategory.ConnectorPunctuation;}
    }
}
