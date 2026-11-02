using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Verdant.Automation
{
    public sealed class Pulse { public int line; public double seconds; public Pulse(int line,double seconds=0){this.line=line;this.seconds=seconds;} }
    public sealed class Box { public object value; }
    public interface IScriptHost
    {
        object Resolve(string name);
        IEnumerable<Pulse> Call(string name,object[] arguments,Box result,int line);
        string Module(string name);
    }
    public sealed class ScriptError:Exception {public int Line;public ScriptError(int line,string message):base("Line "+line+": "+message){Line=line;}}
    public sealed class ScriptKeys:IEqualityComparer<object>
    {
        public static readonly ScriptKeys Instance=new ScriptKeys();public new bool Equals(object a,object b){return ScriptValue.Equal(a,b);}
        public int GetHashCode(object value){if(value is System.Collections.IList&&!(value is object[])||value is System.Collections.IDictionary||value is HashSet<object>)throw new Exception("Mutable collections cannot be dictionary keys.");if(value is object[]){int h=17;foreach(var x in (object[])value)h=unchecked(h*31+GetHashCode(x));return h;}if(value is bool||value is double)return ScriptValue.Number(value).GetHashCode();return value==null?0:value.GetHashCode();}
    }
    public sealed class Scope
    {
        public readonly Dictionary<string,object> values=new Dictionary<string,object>();
        public readonly HashSet<string> globals=new HashSet<string>();public Scope parent;
        public Scope(Scope parent=null){this.parent=parent;}
        public Scope Root {get{return parent==null?this:parent.Root;}}
        public bool Try(string key,out object value){return values.TryGetValue(key,out value)||parent!=null&&parent.Try(key,out value);}
        public void Set(string key,object value){(globals.Contains(key)?Root:this).values[key]=value;}
    }
    public interface IScriptSymbols {bool Known(string name);}
    public interface IScriptFeatures {bool Allows(string feature);}
    public sealed class ScriptTrace {public Scope scope;}
    public sealed class ScriptContext
    {
        public ScriptTrace trace=new ScriptTrace();public IScriptHost host;public Scope scope;public int depth;public string flow;public object returned;
        public Dictionary<string,Scope> modules=new Dictionary<string,Scope>();
        public ScriptContext(IScriptHost host,Scope scope){this.host=host;this.scope=scope;}
        public void Require(string feature){var features=host as IScriptFeatures;if(features!=null&&!features.Allows(feature))throw new Exception("Research needed: "+feature+". Enable unrestricted syntax in Settings to practice.");}
    }
    public static class ScriptValue
    {
        public static double Number(object x){double n;if(x is bool)return (bool)x?1:0;if(x==null||!double.TryParse(Convert.ToString(x,CultureInfo.InvariantCulture),NumberStyles.Float,CultureInfo.InvariantCulture,out n)||double.IsNaN(n)||double.IsInfinity(n))throw new Exception("A finite number is required.");return n;}
        public static int Integer(object x){double n=Number(x);if(n!=Math.Truncate(n)||n<int.MinValue||n>int.MaxValue)throw new Exception("An integer is required.");return (int)n;}
        public static bool Truth(object x){if(x==null)return false;if(x is bool)return (bool)x;if(x is string)return ((string)x).Length>0;if(x is ICollection)return ((ICollection)x).Count>0;if(x is IEnumerable)return ((IEnumerable)x).GetEnumerator().MoveNext();if(x is Native)return ((Native)x).value==null||Truth(((Native)x).value);if(x is double||x is int||x is float||x is long)return Number(x)!=0;return true;}
        public static string Show(object x){return Format(x,new HashSet<object>(),0);}
        static string Format(object x,HashSet<object> active,int depth)
        {
            if(x==null)return "None";if(x is bool)return (bool)x?"True":"False";if(x is string){string text=(string)x;return text.Length>4096?text.Substring(0,4096)+"…":text;}
            if(x is Native)return ((Native)x).ToString();if(x is ScriptFunction)return "<function "+((ScriptFunction)x).name+">";if(x is ClassicDrone)return "<drone "+((ClassicDrone)x).id+">";if(x is Scope)return "<module>";
            if(depth>8)return "…";if(x is IEnumerable){if(!active.Add(x))return "<cycle>";string value;if(x is IDictionary){var d=(IDictionary)x;value="{"+string.Join(", ",d.Keys.Cast<object>().Take(100).Select(k=>Format(k,active,depth+1)+": "+Format(d[k],active,depth+1)))+(d.Count>100?", …":"")+"}";}else{var items=((IEnumerable)x).Cast<object>().Take(101).ToArray();value="["+string.Join(", ",items.Take(100).Select(v=>Format(v,active,depth+1)))+(items.Length>100?", …":"")+"]";}active.Remove(x);return value;}
            return Convert.ToString(x,CultureInfo.InvariantCulture);
        }

        public static object[] Items(object x){if(x is IDictionary)return ((IDictionary)x).Keys.Cast<object>().ToArray();if(x is string)return ((string)x).Select(c=>(object)c.ToString()).ToArray();if(x is IEnumerable)return ((IEnumerable)x).Cast<object>().ToArray();throw new Exception("The value is not iterable.");}
        public static bool Equal(object a,object b){return EqualAt(a,b,0);}
        static bool EqualAt(object a,object b,int depth){if(ReferenceEquals(a,b))return true;if(depth>32)throw new Exception("Comparison nesting limit.");if(a==null||b==null)return false;if(a is bool||a is double){if(b is bool||b is double)return Number(a)==Number(b);}if(a is object[]&&b is object[]||a is List<object>&&b is List<object>){var left=Items(a);var right=Items(b);return left.Length==right.Length&&left.Zip(right,(x,y)=>EqualAt(x,y,depth+1)).All(x=>x);}if(a is IDictionary&&b is IDictionary){var left=(IDictionary)a;var right=(IDictionary)b;return left.Count==right.Count&&left.Keys.Cast<object>().All(k=>right.Contains(k)&&EqualAt(left[k],right[k],depth+1));}if(a is HashSet<object>&&b is HashSet<object>)return ((HashSet<object>)a).SetEquals((HashSet<object>)b);return Equals(a,b);}

        public static object Binary(string op,object a,object b)
        {
            switch(op){case "==":return Equal(a,b);case "!=":return !Equal(a,b);case "is":return ReferenceEquals(a,b);case "in":return b is IDictionary?((IDictionary)b).Contains(a):b is string?((string)b).Contains(Show(a)):Items(b).Any(v=>Equal(a,v));case "not in":return !(bool)Binary("in",a,b);case "+":if(a is string&&b is string){if(((string)a).Length+(long)((string)b).Length>1000000)throw new Exception("String size limit.");return (string)a+(string)b;}if(a is IList&&b is IList){if(((IList)a).Count+(long)((IList)b).Count>100000)throw new Exception("Collection size limit.");return Items(a).Concat(Items(b)).ToList();}break;case "*":if(a is string||a is IList){int n=Integer(b);if(n<0)n=0;if(n>100000)throw new Exception("Collection size limit.");if(a is string){if(((string)a).Length*(long)n>1000000)throw new Exception("String size limit.");return string.Concat(Enumerable.Repeat((string)a,n));}if(((IList)a).Count*(long)n>100000)throw new Exception("Collection size limit.");var original=Items(a);return Enumerable.Range(0,n).SelectMany(_=>original).ToList();}break;}
            if(a is string&&b is string){int cmp=string.CompareOrdinal((string)a,(string)b);switch(op){case "<":return cmp<0;case ">":return cmp>0;case "<=":return cmp<=0;case ">=":return cmp>=0;}}
            double x=Number(a),y=Number(b);switch(op){case "+":return x+y;case "-":return x-y;case "*":return x*y;case "/":if(y==0)throw new Exception("Division by zero.");return x/y;case "//":if(y==0)throw new Exception("Division by zero.");return Math.Floor(x/y);case "%":if(y==0)throw new Exception("Division by zero.");return x-Math.Floor(x/y)*y;case "**":return Math.Pow(x,y);case "<":return x<y;case ">":return x>y;case "<=":return x<=y;case ">=":return x>=y;}throw new Exception("Unknown operator: "+op);
        }
        public static object Get(object target,object key){if(target is IDictionary){var d=(IDictionary)target;if(!d.Contains(key))throw new Exception("Missing dictionary key: "+Show(key));return d[key];}int n=Integer(key),count=target is string?((string)target).Length:((IList)target).Count;if(n<0)n+=count;if(n<0||n>=count)throw new Exception("Index out of range.");return target is string?(object)((string)target)[n].ToString():((IList)target)[n];}
        public static void Put(object target,object key,object value){if(target is IDictionary){var d=(IDictionary)target;if(d.Count>=100000&&!d.Contains(key))throw new Exception("Collection size limit.");d[key]=value;return;}if(target is object[])throw new Exception("Tuples are immutable.");var list=target as IList;if(list==null)throw new Exception("This value cannot be indexed for assignment.");int n=Integer(key);if(n<0)n+=list.Count;if(n<0||n>=list.Count)throw new Exception("Index out of range.");list[n]=value;}
        public static object Clone(object value){if(value is Dictionary<object,object>)return ((Dictionary<object,object>)value).ToDictionary(p=>Clone(p.Key),p=>Clone(p.Value));if(value is List<object>)return ((List<object>)value).Select(Clone).ToList();if(value is object[])return ((object[])value).Select(Clone).ToArray();if(value is HashSet<object>)return new HashSet<object>(((HashSet<object>)value).Select(Clone));return value;}
    }
    public abstract class Expression
    {
        public int line;protected Expression(int line){this.line=line;}
        public abstract IEnumerable<Pulse> Eval(ScriptContext c,Box result);
        public virtual IEnumerable<Pulse> Assign(ScriptContext c,object value){throw new Exception("Invalid assignment target.");}
    }
    sealed class Literal:Expression
    {object value;public Literal(int l,object v):base(l){value=v;}public override IEnumerable<Pulse> Eval(ScriptContext c,Box r){r.value=value;yield break;}}
    sealed class Name:Expression
    {
        public string name;public Name(int l,string n):base(l){name=n;}
        public override IEnumerable<Pulse> Eval(ScriptContext c,Box r){object v;if(name=="None")r.value=null;else if(name=="True"||name=="False")r.value=name=="True";else if(c.scope.Try(name,out v))r.value=v;else {object resolved=c.host.Resolve(name);if(resolved==null&&c.host is IScriptSymbols&&!((IScriptSymbols)c.host).Known(name)&&!Call.BuiltinNames.Contains(name))throw new Exception("Undefined variable: "+name);r.value=new Native(name,resolved);}yield break;}
        public override IEnumerable<Pulse> Assign(ScriptContext c,object value){c.scope.Set(name,value);yield break;}
    }
    public sealed class Native {public string name;public object value;public Native(string name,object value=null){this.name=name;this.value=value;}public override string ToString(){return value==null?name:ScriptValue.Show(value);}}
    sealed class Unary:Expression
    {string op;Expression x;public Unary(int l,string op,Expression x):base(l){this.op=op;this.x=x;}public override IEnumerable<Pulse> Eval(ScriptContext c,Box r){var v=new Box();foreach(var p in x.Eval(c,v))yield return p;r.value=op=="not"?(object)!ScriptValue.Truth(v.value):op=="-"?-ScriptValue.Number(v.value):ScriptValue.Number(v.value);yield return new Pulse(line);}}
    sealed class Binary:Expression
    {string op;Expression a,b;public Binary(int l,string op,Expression a,Expression b):base(l){this.op=op;this.a=a;this.b=b;}public override IEnumerable<Pulse> Eval(ScriptContext c,Box r){c.Require("operators");var x=new Box();var y=new Box();foreach(var p in a.Eval(c,x))yield return p;if(op=="and"&&!ScriptValue.Truth(x.value)||op=="or"&&ScriptValue.Truth(x.value)){r.value=x.value;yield break;}foreach(var p in b.Eval(c,y))yield return p;r.value=op=="and"||op=="or"?y.value:ScriptValue.Binary(op,Unwrap(x.value),Unwrap(y.value));yield return new Pulse(line);}static object Unwrap(object v){return v is Native&&((Native)v).value!=null?((Native)v).value:v;}}
    sealed class CompareChain:Expression
    {List<Expression> values;List<string> operators;public CompareChain(int line,List<Expression> values,List<string> operators):base(line){this.values=values;this.operators=operators;}public override IEnumerable<Pulse> Eval(ScriptContext c,Box r){c.Require("operators");var prior=new Box();foreach(var p in values[0].Eval(c,prior))yield return p;for(int i=0;i<operators.Count;i++){var next=new Box();foreach(var p in values[i+1].Eval(c,next))yield return p;object a=prior.value is Native?((Native)prior.value).value:prior.value,b=next.value is Native?((Native)next.value).value:next.value;bool ok=operators[i]=="is not"?!ReferenceEquals(a,b):ScriptValue.Truth(ScriptValue.Binary(operators[i],a,b));yield return new Pulse(line);if(!ok){r.value=false;yield break;}prior=next;}r.value=true;}}
    sealed class ConditionalValue:Expression
    {Expression yes,test,no;public ConditionalValue(int line,Expression yes,Expression test,Expression no):base(line){this.yes=yes;this.test=test;this.no=no;}public override IEnumerable<Pulse> Eval(ScriptContext c,Box r){var t=new Box();foreach(var p in test.Eval(c,t))yield return p;foreach(var p in (ScriptValue.Truth(t.value)?yes:no).Eval(c,r))yield return p;}}
    sealed class Collection:Expression
    {
        public List<Expression> items;string kind;public Collection(int l,string kind,List<Expression> items):base(l){this.kind=kind;this.items=items;}
        public override IEnumerable<Pulse> Eval(ScriptContext c,Box r){c.Require(kind=="dict"||kind=="set"?"dicts":"lists");var values=new List<object>();foreach(var e in items){var v=new Box();foreach(var p in e.Eval(c,v))yield return p;values.Add(v.value is Native&&((Native)v.value).value!=null?((Native)v.value).value:v.value);}if(kind=="dict"){var d=new Dictionary<object,object>(ScriptKeys.Instance);for(int i=0;i<values.Count;i+=2)d[values[i]]=values[i+1];r.value=d;}else if(kind=="set")r.value=new HashSet<object>(values,ScriptKeys.Instance);else r.value=kind=="tuple"?(object)values.ToArray():values;yield return new Pulse(line);}
        public override IEnumerable<Pulse> Assign(ScriptContext c,object value){var values=ScriptValue.Items(value);if(values.Length!=items.Count)throw new Exception("Unpacking length mismatch.");for(int i=0;i<items.Count;i++)foreach(var p in items[i].Assign(c,values[i]))yield return p;}
    }
    sealed class Index:Expression
    {Expression target,key;public Index(int l,Expression t,Expression k):base(l){target=t;key=k;}public override IEnumerable<Pulse> Eval(ScriptContext c,Box r){var t=new Box();var k=new Box();foreach(var p in target.Eval(c,t))yield return p;foreach(var p in key.Eval(c,k))yield return p;r.value=ScriptValue.Get(t.value,k.value);yield return new Pulse(line);}public override IEnumerable<Pulse> Assign(ScriptContext c,object value){var t=new Box();var k=new Box();foreach(var p in target.Eval(c,t))yield return p;foreach(var p in key.Eval(c,k))yield return p;ScriptValue.Put(t.value,k.value,value);yield return new Pulse(line);}}
    sealed class Member:Expression
    {Expression target;string name;public Member(int l,Expression t,string n):base(l){target=t;name=n;}public override IEnumerable<Pulse> Eval(ScriptContext c,Box r){var t=new Box();foreach(var p in target.Eval(c,t))yield return p;if(t.value is Scope){object v;if(!((Scope)t.value).Try(name,out v))throw new Exception("Unknown module member: "+name);r.value=v;}else if(t.value is Native)r.value=new Native(((Native)t.value).name+"."+name,c.host.Resolve(((Native)t.value).name+"."+name));else r.value=new Method(t.value,name);}}
    sealed class Method {public object target;public string name;public Method(object t,string n){target=t;name=n;}}
    sealed class Call:Expression
    {
        public static readonly string[] BuiltinNames=new[]{"len","str","int","float","bool","abs","min","max","range","list","tuple","set","dict","append","add","insert","pop","remove"}.Concat(PythonUtilities.Names).ToArray();
        Expression target;List<Expression> args;public Call(int l,Expression t,List<Expression> a):base(l){target=t;args=a;}
        public override IEnumerable<Pulse> Eval(ScriptContext c,Box r)
        {
            var f=new Box();foreach(var p in target.Eval(c,f))yield return p;var values=new List<object>();foreach(var a in args){var v=new Box();foreach(var p in a.Eval(c,v))yield return p;values.Add(v.value is Native&&((Native)v.value).value!=null?((Native)v.value).value:v.value);}
            if(f.value is ScriptFunction){foreach(var p in ((ScriptFunction)f.value).Invoke(c,values.ToArray(),r))yield return p;c.trace.scope=c.scope;}
            else if(f.value is Method){r.value=Methods((Method)f.value,values.ToArray());yield return new Pulse(line);}
            else if(f.value is Native){object builtin;if(Builtins(((Native)f.value).name,values.ToArray(),out builtin)){r.value=builtin;yield return new Pulse(line);}else foreach(var p in c.host.Call(((Native)f.value).name,values.ToArray(),r,line))yield return p;}
            else throw new Exception("The value is not callable.");
        }
        static object Methods(Method m,object[] a)
        {
            if(m.target is string)return PythonUtilities.StringMethod((string)m.target,m.name,a);
            var list=m.target as List<object>;var d=m.target as Dictionary<object,object>;var set=m.target as HashSet<object>;
            switch(m.name){case "index":Require(a,1);if(list==null)break;int index=list.IndexOf(a[0]);if(index<0)throw new Exception("Value not found.");return (double)index;case "append":Require(a,1);if(list==null)break;if(list.Count>=100000)throw new Exception("Collection size limit.");list.Add(a[0]);return null;case "insert":Require(a,2);if(list==null)break;if(list.Count>=100000)throw new Exception("Collection size limit.");list.Insert(Math.Max(0,Math.Min(list.Count,PythonUtilities.Integer(a[0]))),a[1]);return null;case "add":Require(a,1);if(set==null)break;if(set.Count>=100000&&!set.Contains(a[0]))throw new Exception("Collection size limit.");set.Add(a[0]);return null;case "remove":Require(a,1);if(list!=null){if(!list.Remove(a[0]))throw new Exception("Value not found.");return null;}if(set!=null){if(!set.Remove(a[0]))throw new Exception("Value not found.");return null;}break;case "pop":if(list!=null){if(a.Length>1)throw new Exception("pop expects zero or one argument.");int n=a.Length==0?list.Count-1:PythonUtilities.Integer(a[0]);if(n<0)n+=list.Count;object v=list[n];list.RemoveAt(n);return v;}if(d!=null){Require(a,1);object v=d[a[0]];d.Remove(a[0]);return v;}break;case "keys":Require(a,0);if(d!=null)return d.Keys.ToList();break;case "values":Require(a,0);if(d!=null)return d.Values.ToList();break;case "items":Require(a,0);if(d!=null)return d.Select(p=>(object)new object[]{p.Key,p.Value}).ToList();break;case "get":if(d!=null&&a.Length>=1&&a.Length<=2){object v;return d.TryGetValue(a[0],out v)?v:a.Length==2?a[1]:null;}break;case "clear":Require(a,0);if(list!=null)list.Clear();else if(d!=null)d.Clear();else if(set!=null)set.Clear();else break;return null;}
            throw new Exception("Unsupported collection method: "+m.name);
        }
        public static void Require(object[] a,int n){if(a.Length!=n)throw new Exception("Expected "+n+" arguments; received "+a.Length+".");}
        static bool Builtins(string n,object[] a,out object r)
        {
            r=null;if(PythonUtilities.Names.Contains(n)){r=PythonUtilities.Invoke(n,a);return true;}switch(n){case "len":Require(a,1);r=(double)ScriptValue.Items(a[0]).Length;return true;case "str":Require(a,1);r=ScriptValue.Show(a[0]);return true;case "int":Require(a,1);r=Math.Truncate(ScriptValue.Number(a[0]));return true;case "float":Require(a,1);r=ScriptValue.Number(a[0]);return true;case "bool":Require(a,1);r=ScriptValue.Truth(a[0]);return true;case "abs":Require(a,1);r=Math.Abs(ScriptValue.Number(a[0]));return true;case "min":case "max":var values=a.Length==1?ScriptValue.Items(a[0]):a;if(values.Length==0)throw new Exception("Empty collection.");r=n=="min"?values.Min(ScriptValue.Number):values.Max(ScriptValue.Number);return true;case "range":if(a.Length<1||a.Length>3)throw new Exception("range expects 1 to 3 arguments.");int start=a.Length==1?0:PythonUtilities.Integer(a[0]),end=PythonUtilities.Integer(a[a.Length==1?0:1]),step=a.Length==3?PythonUtilities.Integer(a[2]):1;if(step==0)throw new Exception("range step cannot be zero.");var v=new List<object>();for(long i=start;step>0?i<end:i>end;i+=step){if(v.Count>=100000)throw new Exception("Range size limit.");v.Add((double)i);}r=v;return true;case "list":case "tuple":case "set":if(a.Length>1)throw new Exception("Expected at most one argument.");var data=a.Length==0?new object[0]:ScriptValue.Items(a[0]);r=n=="list"?(object)data.ToList():n=="tuple"?data:new HashSet<object>(data,ScriptKeys.Instance);return true;case "dict":Require(a,0);r=new Dictionary<object,object>();return true;case "append":case "add":case "insert":case "pop":case "remove":if(a.Length<1)throw new Exception("Missing collection.");r=Methods(new Method(a[0],n),a.Skip(1).ToArray());return true;default:return false;}
        }
    }
    public abstract class Statement
    {public int line;protected Statement(int line){this.line=line;}public abstract IEnumerable<Pulse> Exec(ScriptContext c);}
    public sealed class Block:Statement
    {public List<Statement> statements=new List<Statement>();public Block():base(1){}public override IEnumerable<Pulse> Exec(ScriptContext c){foreach(var s in statements){c.trace.scope=c.scope;yield return new Pulse(s.line);if(s is FunctionStatement)c.Require("functions");if(s is Assignment)c.Require("variables");if(s is LoopStatement)c.Require("loops");if(s is Conditional)c.Require("operators");if(s is ImportStatement)c.Require("import");foreach(var p in s.Exec(c))yield return p;if(c.flow!=null)yield break;}}}
    sealed class ExprStatement:Statement
    {Expression value;public ExprStatement(int l,Expression e):base(l){value=e;}public override IEnumerable<Pulse> Exec(ScriptContext c){var r=new Box();foreach(var p in value.Eval(c,r))yield return p;yield return new Pulse(line);}}
    sealed class Assignment:Statement
    {Expression target,value;public Assignment(int l,Expression t,Expression v):base(l){target=t;value=v;}public override IEnumerable<Pulse> Exec(ScriptContext c){var r=new Box();foreach(var p in value.Eval(c,r))yield return p;foreach(var p in target.Assign(c,r.value))yield return p;yield return new Pulse(line);}}
    sealed class Conditional:Statement
    {public List<Expression> tests=new List<Expression>();public List<Block> branches=new List<Block>();public Block otherwise;public Conditional(int l):base(l){}public override IEnumerable<Pulse> Exec(ScriptContext c){for(int i=0;i<tests.Count;i++){var b=new Box();foreach(var p in tests[i].Eval(c,b))yield return p;yield return new Pulse(line);if(ScriptValue.Truth(b.value)){foreach(var p in branches[i].Exec(c))yield return p;yield break;}}if(otherwise!=null)foreach(var p in otherwise.Exec(c))yield return p;}}
    sealed class LoopStatement:Statement
    {Expression test,target,sequence;Block body;bool isFor;public LoopStatement(int l,Expression test,Block body):base(l){this.test=test;this.body=body;}public LoopStatement(int l,Expression t,Expression seq,Block b):base(l){target=t;sequence=seq;body=b;isFor=true;}public override IEnumerable<Pulse> Exec(ScriptContext c){object[] items=null;if(isFor){var r=new Box();foreach(var p in sequence.Eval(c,r))yield return p;items=ScriptValue.Items(r.value);}int i=0;while(true){if(isFor){if(i>=items.Length)yield break;foreach(var p in target.Assign(c,items[i++]))yield return p;}else{var r=new Box();foreach(var p in test.Eval(c,r))yield return p;if(!ScriptValue.Truth(r.value))yield break;}yield return new Pulse(line);foreach(var p in body.Exec(c))yield return p;if(c.flow=="return")yield break;if(c.flow=="break"){c.flow=null;yield break;}c.flow=null;}}}
    sealed class FlowStatement:Statement
    {string kind;Expression value;public FlowStatement(int l,string kind,Expression v=null):base(l){this.kind=kind;value=v;}public override IEnumerable<Pulse> Exec(ScriptContext c){if(value!=null){var r=new Box();foreach(var p in value.Eval(c,r))yield return p;c.returned=r.value;}c.flow=kind;yield return new Pulse(line);}}
    sealed class GlobalStatement:Statement
    {string[] names;public GlobalStatement(int l,string[] names):base(l){this.names=names;}public override IEnumerable<Pulse> Exec(ScriptContext c){foreach(var n in names)c.scope.globals.Add(n);yield return new Pulse(line);}}
    public sealed class ScriptFunction
    {
        public string name;public string[] parameters;public object[] defaults;public Block body;public Scope closure;
        public IEnumerable<Pulse> Invoke(ScriptContext parent,object[] args,Box result)
        {if(parent.depth>=24)throw new Exception("Function recursion exceeds 24 levels.");if(args.Length>parameters.Length||args.Length<parameters.Length-defaults.Length)throw new Exception("Incorrect argument count for "+name+".");var scope=new Scope(closure);for(int i=0;i<parameters.Length;i++)scope.values[parameters[i]]=i<args.Length?args[i]:defaults[i-(parameters.Length-defaults.Length)];var c=new ScriptContext(parent.host,scope){depth=parent.depth+1,modules=parent.modules,trace=parent.trace};foreach(var p in body.Exec(c))yield return p;result.value=c.returned;}
    }
    sealed class FunctionStatement:Statement
    {string name;string[] parameters;Expression[] defaults;Block body;public FunctionStatement(int l,string n,string[] p,Expression[] d,Block b):base(l){name=n;parameters=p;defaults=d;body=b;}public override IEnumerable<Pulse> Exec(ScriptContext c){var values=new List<object>();foreach(var d in defaults){var b=new Box();foreach(var p in d.Eval(c,b))yield return p;values.Add(b.value);}c.scope.Set(name,new ScriptFunction{name=name,parameters=parameters,defaults=values.ToArray(),body=body,closure=c.scope});yield return new Pulse(line);}}
    sealed class ImportStatement:Statement
    {string module,alias;string[] names;public ImportStatement(int l,string module,string[] names,string alias=null):base(l){this.module=ClassicProgramNames.Normalize(module);this.names=names;this.alias=alias??module;}public override IEnumerable<Pulse> Exec(ScriptContext c){if(module=="__builtins__"&&names!=null&&names.Length==1&&names[0]=="*")yield break;Scope imported;if(!c.modules.TryGetValue(module,out imported)){string source=c.host.Module(module);if(source==null)throw new Exception("Module not found: "+module);imported=new Scope();imported.values["__name__"]=module;c.modules[module]=imported;var context=new ScriptContext(c.host,imported){modules=c.modules,depth=c.depth+1};if(context.depth>=24)throw new Exception("Import depth limit.");foreach(var p in new ScriptParser().Parse(source).Exec(context))yield return p;}if(names==null)c.scope.Set(alias,imported);else foreach(var name in names){if(name=="*"){foreach(var p in imported.values)c.scope.Set(p.Key,p.Value);}else {object v;if(!imported.Try(name,out v))throw new Exception("Missing import: "+name);c.scope.Set(name,v);}}yield return new Pulse(line);}}
    public sealed class ScriptTask
    {
        public Scope CurrentScope{get{return context.trace.scope??context.scope;}}public ScriptContext context;IEnumerator<Pulse> execution;Box functionResult;public bool Finished;public object Result;public int Line=1;public long Steps;public double Delay;public string Error;
        public ScriptTask(IScriptHost host,string source){context=new ScriptContext(host,new Scope());context.scope.values["__name__"]="__main__";execution=new ScriptParser().Parse(source).Exec(context).GetEnumerator();}
        public ScriptTask(IScriptHost host,Native function,object[] args){context=new ScriptContext(host,new Scope());functionResult=new Box();execution=new Call(1,new Literal(1,function),args.Select(a=>(Expression)new Literal(1,a)).ToList()).Eval(context,functionResult).GetEnumerator();}
        public ScriptTask(IScriptHost host,ScriptFunction function,object[] args){context=new ScriptContext(host,new Scope());functionResult=new Box();execution=function.Invoke(context,args,functionResult).GetEnumerator();}
        public bool Step(){if(Finished)return false;try{Steps++;if(!execution.MoveNext()){Finished=true;Result=functionResult==null?context.returned:functionResult.value;return false;}var p=execution.Current;Line=p.line;Delay=p.seconds;return true;}catch(Exception e){Finished=true;Error=e is ScriptError?e.Message:"Line "+Line+": "+e.Message;throw new ScriptError(Line,e.Message);}}
    }
}
