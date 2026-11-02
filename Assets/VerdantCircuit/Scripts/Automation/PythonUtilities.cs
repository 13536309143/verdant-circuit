using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
namespace Verdant.Automation
{
    public static class PythonUtilities
    {
        public static readonly string[] Names={"sum","enumerate","zip","sorted","reversed","round","all","any","pow","divmod","type","ord","chr","bin","hex","oct"};
        public static object Invoke(string name,object[] a)
        {
            switch(name)
            {
                case "sum":if(a.Length<1||a.Length>2)throw new Exception("sum expects iterable and optional start.");return ScriptValue.Items(a[0]).Sum(ScriptValue.Number)+(a.Length==2?ScriptValue.Number(a[1]):0);
                case "enumerate":if(a.Length<1||a.Length>2)throw new Exception("enumerate expects iterable and optional start.");double start=a.Length==2?Integer(a[1]):0;return ScriptValue.Items(a[0]).Select((v,i)=>(object)new object[]{start+i,v}).ToList();
                case "zip":if(a.Length==0)return new List<object>();var lists=a.Select(ScriptValue.Items).ToArray();int count=lists.Min(v=>v.Length);return Enumerable.Range(0,count).Select(i=>(object)lists.Select(v=>v[i]).ToArray()).ToList();
                case "sorted":if(a.Length<1||a.Length>2)throw new Exception("sorted expects iterable and optional reverse bool.");var values=ScriptValue.Items(a[0]).ToList();if(values.All(v=>v is string))values=values.OrderBy(v=>(string)v,StringComparer.Ordinal).ToList();else if(values.All(v=>v is double||v is int))values=values.OrderBy(ScriptValue.Number).ToList();else throw new Exception("sorted supports uniform numbers or strings.");if(a.Length==2&&ScriptValue.Truth(a[1]))values.Reverse();return values;
                case "reversed":Call.Require(a,1);return ScriptValue.Items(a[0]).Reverse().ToList();
                case "round":if(a.Length<1||a.Length>2)throw new Exception("round expects number and optional digits.");int digits=a.Length==2?Integer(a[1]):0;if(digits< -12||digits>12)throw new Exception("round digits must be -12 to 12.");double factor=Math.Pow(10,digits);return Math.Round(ScriptValue.Number(a[0])*factor,MidpointRounding.ToEven)/factor;
                case "all":Call.Require(a,1);return ScriptValue.Items(a[0]).All(ScriptValue.Truth);
                case "any":Call.Require(a,1);return ScriptValue.Items(a[0]).Any(ScriptValue.Truth);
                case "pow":Call.Require(a,2);double result=Math.Pow(ScriptValue.Number(a[0]),ScriptValue.Number(a[1]));if(double.IsInfinity(result)||double.IsNaN(result))throw new Exception("pow result is not finite.");return result;
                case "divmod":Call.Require(a,2);double divisor=ScriptValue.Number(a[1]);if(divisor==0)throw new Exception("division by zero");double q=Math.Floor(ScriptValue.Number(a[0])/divisor);return new object[]{q,ScriptValue.Number(a[0])-q*divisor};
                case "type":Call.Require(a,1);return a[0]==null?"NoneType":a[0] is string?"str":a[0] is bool?"bool":a[0] is List<object>?"list":a[0] is object[]?"tuple":a[0] is Dictionary<object,object>?"dict":a[0] is HashSet<object>?"set":a[0] is double?"number":"function";
                case "ord":Call.Require(a,1);string s=Convert.ToString(a[0]);if(s.Length!=1&&!(s.Length==2&&char.IsSurrogatePair(s,0)))throw new Exception("ord expects one Unicode character.");return (double)char.ConvertToUtf32(s,0);
                case "chr":Call.Require(a,1);int code=Integer(a[0]);if(code<0||code>0x10ffff||code>=0xd800&&code<=0xdfff)throw new Exception("Invalid Unicode scalar.");return char.ConvertFromUtf32(code);
                case "bin":case "hex":case "oct":Call.Require(a,1);int n=Integer(a[0]),radix=name=="bin"?2:name=="oct"?8:16;long positive=Math.Abs((long)n);string encoded="";do{int rem=(int)(positive%radix);encoded="0123456789abcdef"[rem]+encoded;positive/=radix;}while(positive>0);return (n<0?"-":"")+(radix==2?"0b":radix==8?"0o":"0x")+encoded;
                default:throw new Exception("Unknown numeric helper.");
            }
        }
        public static int Integer(object value){if(!(value is double)&&!(value is int)&&!(value is long))throw new Exception("Expected an integer number, not a string or boolean.");double n=ScriptValue.Number(value);if(double.IsNaN(n)||double.IsInfinity(n)||n!=Math.Truncate(n)||n<int.MinValue||n>int.MaxValue)throw new Exception("Expected a finite integer.");return (int)n;}
        public static object StringMethod(string s,string name,object[] a)
        {
            switch(name)
            {
                case "lower":Call.Require(a,0);return s.ToLowerInvariant();case "upper":Call.Require(a,0);return s.ToUpperInvariant();case "strip":Call.Require(a,0);return s.Trim();
                case "split":if(a.Length>1)throw new Exception("split expects optional separator.");if(a.Length==0)return s.Split((char[])null,StringSplitOptions.RemoveEmptyEntries).Select(v=>(object)v).ToList();string separator=Convert.ToString(a[0]);if(separator=="")throw new Exception("empty separator");return s.Split(new[]{separator},StringSplitOptions.None).Select(v=>(object)v).ToList();
                case "join":Call.Require(a,1);var parts=ScriptValue.Items(a[0]);if(parts.Any(v=>!(v is string)))throw new Exception("join expects strings.");return string.Join(s,parts.Cast<string>());
                case "replace":Call.Require(a,2);return s.Replace(Convert.ToString(a[0]),Convert.ToString(a[1]));
                case "startswith":Call.Require(a,1);return s.StartsWith(Convert.ToString(a[0]),StringComparison.Ordinal);case "endswith":Call.Require(a,1);return s.EndsWith(Convert.ToString(a[0]),StringComparison.Ordinal);
                case "find":Call.Require(a,1);return (double)s.IndexOf(Convert.ToString(a[0]),StringComparison.Ordinal);
                case "count":Call.Require(a,1);string needle=Convert.ToString(a[0]);if(needle=="")return (double)s.Length+1;int count=0,index=0;while((index=s.IndexOf(needle,index,StringComparison.Ordinal))>=0){count++;index+=needle.Length;}return (double)count;
                default:throw new Exception("Unsupported string method: "+name);
            }
        }
    }
}
