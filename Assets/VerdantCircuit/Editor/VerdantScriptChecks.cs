using System;
using System.IO;
using System.Collections.Generic;
using Verdant.Automation;
public static class VerdantScriptChecks
{
    sealed class Host:IScriptHost
    {
        public List<string> output=new List<string>();public Dictionary<string,string> modules=new Dictionary<string,string>();
        public object Resolve(string name){return null;}
        public string Module(string name){string s;return modules.TryGetValue(name,out s)?s:null;}
        public IEnumerable<Pulse> Call(string name,object[] args,Box r,int line){if(name!="print")throw new Exception("Unknown function: "+name);output.Add(string.Join(" ",Array.ConvertAll(args,ScriptValue.Show)));yield return new Pulse(line,.01);}
    }
    public static void Run()
    {
        int checks=0;Action<bool,string> check=(ok,n)=>{if(!ok)throw new Exception("FAIL: "+n);checks++;};
        Func<Host,string,ScriptTask> run=(h,s)=>{var t=new ScriptTask(h,s);int budget=100000;while(!t.Finished&&budget-->0)t.Step();if(!t.Finished)throw new Exception("Unfinished test.");return t;};
        var host=new Host();run(host,"def add(a, b=2):\n    return a+b\nprint(add(5))\nprint(add(1,4))");check(string.Join(",",host.output)=="7,5","Parameters, defaults and returned values");
        host=new Host();run(host,"def fact(n):\n    if n<=1:\n        return 1\n    return n*fact(n-1)\nprint(fact(6))");check(host.output[0]=="720","Recursive expression calls");
        host=new Host();run(host,"a=[3,1,2]\na[0]=4\na.append(5)\ns=0\nfor x in a:\n    s+=x\nprint(s)\nprint(a.pop())");check(string.Join(",",host.output)=="12,5","List mutations and iteration");
        host=new Host();run(host,"d={'a':3}\nd['b']=4\ns=0\nfor k in d:\n    s+=d[k]\nx,y=(s,9)\nprint(x,y)\nprint(len({1,1,2}))");check(string.Join(",",host.output)=="7 9,2","Dictionary, set, tuples and unpacking");
        host=new Host();run(host,"x=1\ndef outer(n):\n    def inner(v):\n        return v+n\n    return inner\nf=outer(5)\nprint(f(3))\ndef inc():\n    global x\n    x+=1\ninc()\nprint(x)");check(string.Join(",",host.output)=="8,2","Closures and explicit global scope");
        host=new Host();host.modules["maths"]="def double(x):\n    return x*2";run(host,"import maths\nprint(maths.double(7))\nfrom maths import double\nprint(double(8))");check(string.Join(",",host.output)=="14,16","Module cache and imported functions");
        host=new Host();run(host,"s=0\nfor i in range(10,0,-2):\n    if i==6:\n        continue\n    s+=i\n    if i==4:\n        break\nprint(s)\nprint(-2**2, 7//2, -1%4)");check(string.Join(",",host.output)=="22,-4 3 3","Loop control and arithmetic semantics");
        var task=new ScriptTask(new Host(),"while True:\n    pass");for(int i=0;i<160;i++)task.Step();check(!task.Finished&&task.Steps==160,"Infinite loops yield to frame budgets");
        bool error=false;try{run(new Host(),"def f():\n    return f()\nf()");}catch(ScriptError){error=true;}check(error,"Recursion is bounded");
        Directory.CreateDirectory("InternalTestReports/0.11.1");File.WriteAllText("InternalTestReports/0.11.1/language.json","{\"passed\":"+checks+"}");UnityEngine.Debug.Log("SCRIPT CHECKS PASSED: "+checks);
    }
}
