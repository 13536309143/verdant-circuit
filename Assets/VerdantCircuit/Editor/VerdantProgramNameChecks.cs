using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Verdant.Automation;

public static class VerdantProgramNameChecks
{
    static List<string> names;
    [Serializable] public class Report{public int passed;public string[] checks;}
    static void Check(bool value,string name){if(!value)throw new Exception("PROGRAM NAME FAIL: "+name);names.Add(name);}
    static ClassicSession Run(ClassicFarm farm,string source){var s=new ClassicSession(farm);s.Start(source);int frames=10000;while(s.Running&&frames-->0)s.FixedTick();if(s.Error!=""||s.Running)throw new Exception("Name program failed: "+s.Error);return s;}
    public static void Run()
    {
        names=new List<string>();
        foreach(string name in new[]{"123","1号农场","浇水程序","中文_2","_helpers","main","école","Программа"})Check(ClassicProgramNames.IsCreatable(name),"Create accepts "+name);
        foreach(string name in new[]{""," ","../main","a/b","a\\b","a:b","a.py","a b","x\ny","<b>name</b>","CON","nul","COM1","LPT9","__builtins__","__main__",new string('中',81)})Check(!ClassicProgramNames.IsCreatable(ClassicProgramNames.Normalize(name)),"Reject unsafe, reserved or overlong name: "+name.Replace("\n","\\n"));
        Check(ClassicProgramNames.Normalize("  1号农场  ")=="1号农场","Creation trims outer whitespace");
        Check(ClassicProgramNames.Same("e\u0301cole","école")&&ClassicProgramNames.Same("Helpers","helpers",true),"Unicode-equivalent and case-only names cannot create conflicting files");
        Check(!ClassicProgramNames.Same("Helpers","helpers"),"Script imports keep case-sensitive lookup");
        var f=new ClassicFarm();f.Data.allowLockedSyntax=true;f.Data.programs.Add(new ClassicProgram("浇水程序","def 工作(次数=3):\n    return 次数*2"));f.Data.programs.Add(new ClassicProgram("123","def work():\n    return 7"));f.Data.programs.Add(new ClassicProgram("1号农场","value=11"));f.Data.programs.Add(new ClassicProgram("école","value=13"));
        var session=Run(f,"import 浇水程序\n结果=浇水程序.工作(4)\nassertion=结果==8");Check(ScriptValue.Truth(session.drones[0].task.context.scope.values["assertion"]),"Chinese-named modules, functions, parameters and values execute");
        session=Run(f,"from 123 import work\nimport 123 as farm\nimport 1号农场 as 农场\nassertion=work()==7 and farm.work()==7 and 农场.value==11");Check(ScriptValue.Truth(session.drones[0].task.context.scope.values["assertion"]),"Digit-leading module names support from-import and explicit aliases");
        session=Run(f,"import e\u0301cole as school\nassertion=school.value==13");Check(ScriptValue.Truth(session.drones[0].task.context.scope.values["assertion"]),"Decomposed Unicode file lookup resolves the canonical module");
        string saved=JsonUtility.ToJson(f.Data);var restored=JsonUtility.FromJson<ClassicSave>(saved);typeof(ClassicController).GetMethod("Validate",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,new object[]{restored});Check(restored.programs.Select(p=>p.name).SequenceEqual(f.Data.programs.Select(p=>p.name)),"Save validation and JSON roundtrip preserve numeric and Chinese program names");
        Check(!ClassicHost.Api.Contains("simulate"),"Retired experiment API is no longer callable");
        bool rejected=false;try{new ScriptParser().Parse("1abc=5");}catch(ScriptError){rejected=true;}Check(rejected,"Numeric program names do not make digit-leading variables legal");
        rejected=false;try{new ScriptParser().Parse("import ../secret");}catch(ScriptError){rejected=true;}Check(rejected,"Module imports cannot escape the game program library");
        Directory.CreateDirectory("InternalTestReports/0.13.0");File.WriteAllText("InternalTestReports/0.13.0/program-names.json",JsonUtility.ToJson(new Report{passed=names.Count,checks=names.ToArray()},true));Debug.Log("PROGRAM NAME CHECKS PASSED: "+names.Count);
    }
}
