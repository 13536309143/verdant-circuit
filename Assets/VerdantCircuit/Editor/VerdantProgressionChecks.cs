using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Verdant.Automation;

public static class VerdantProgressionChecks
{
    static List<string> names;
    [Serializable] public class Report{public int passed;public string[] checks;}
    static void Check(bool value,string name){if(!value)throw new Exception("PROGRESSION FAIL: "+name);names.Add(name);}
    static void Visit(string id,int level,HashSet<string> path,HashSet<string> done)
    {
        if(level<1)return;string key=id+":"+level;if(done.Contains(key))return;if(!path.Add(key))throw new Exception("Cyclic staged research: "+key);
        Visit(id,level-1,path,done);foreach(var p in ClassicProgression.Requirements(id,level))if(Convert.ToString(p.Key).StartsWith("unlock:"))Visit(Convert.ToString(p.Key).Substring(7),(int)ScriptValue.Number(p.Value),path,done);path.Remove(key);done.Add(key);
    }
    public static void Run()
    {
        names=new List<string>();var visited=new HashSet<string>();foreach(var tech in ClassicFarm.Techs)Visit(tech.id,tech.max,new HashSet<string>(),visited);Check(visited.Count==120,"All 120 research levels have an acyclic dependency route");
        var f=new ClassicFarm();f.Add("hay",1e9);f.Data.unlocks.Add(new Stock("plant",1));int expectedSide=1;
        foreach(string crop in new[]{"plant","trees","pumpkins","cactus","dinosaurs"})
        {
            if(crop!="plant"){double before=f.Count("hay");Check(!f.Unlock("expand")&&f.Count("hay")==before,"Expansion is blocked without "+crop+" and preserves funds");f.Data.unlocks.Add(new Stock(crop,1));}
            expectedSide*=2;Check(f.Unlock("expand")&&f.Data.size==expectedSide,"Crop discovery opens field side "+expectedSide);
        }
        foreach(var pair in ClassicProgression.TierDiscoveries)
        {
            for(int level=1;level<=pair.Value.Length;level++)
            {
                string needed=pair.Value[level-1];if(needed=="")continue;
                var trial=new ClassicFarm();trial.Data.unlocks.Add(new Stock(pair.Key,level-1));var r=ClassicProgression.Requirements(pair.Key,level);
                foreach(var req in r.Where(p=>Convert.ToString(p.Key).StartsWith("unlock:"))){string id=Convert.ToString(req.Key).Substring(7);if(id!=needed)trial.Data.unlocks.Add(new Stock(id,ScriptValue.Number(req.Value)));}
                foreach(var resource in ClassicFarm.Items)trial.Add(resource,1e12);
                foreach(var goal in ClassicProgression.ProductionGoals.Values)trial.Data.harvested.Add(new Stock(goal.id,1e12));
                double[] before=ClassicFarm.Items.Select(trial.Count).ToArray();Check(!trial.Unlock(pair.Key)&&before.SequenceEqual(ClassicFarm.Items.Select(trial.Count)),pair.Key+" level "+level+" cannot bypass "+needed);
            }
        }
        foreach(var pair in ClassicProgression.ProductionGoals)
        {
            f=new ClassicFarm();foreach(var p in ClassicProgression.Requirements(pair.Key,1).Where(p=>Convert.ToString(p.Key).StartsWith("unlock:")))f.Data.unlocks.Add(new Stock(Convert.ToString(p.Key).Substring(7),ScriptValue.Number(p.Value)));
            foreach(string item in ClassicFarm.Items)f.Add(item,1e9);
            Check(!f.CanUnlock(pair.Key)&&!f.Unlock(pair.Key),"Inventory alone cannot satisfy "+pair.Key+" production milestone");
            f.Produce(pair.Value.id,pair.Value.amount);f.Add(pair.Value.id,-pair.Value.amount);
            Check(f.Produced(pair.Value.id)>=pair.Value.amount&&f.Unlock(pair.Key),"Spent production still counts toward "+pair.Key+" discovery");
        }
        f=new ClassicFarm();f.Data.allowLockedSyntax=true;f.Data.unlocks.Add(new Stock("expand",1));f.Resize(2);var session=new ClassicSession(f);session.Start("set_world_size(32)");for(int i=0;i<100&&session.Running;i++)session.FixedTick();Check(session.Error!=""&&f.Data.size==2,"Debug world resizing cannot bypass unpurchased expansion tiers");
        f=new ClassicFarm();f.Data.allowLockedSyntax=true;f.Add("hay",1000000);f.Data.unlocks.Add(new Stock("plant",1));session=new ClassicSession(f);session.Start("a=can_unlock(Unlocks.Expand)\nr=get_unlock_requirements(Unlocks.Expand)\nn=num_produced(Items.Hay)\nb=unlock(Unlocks.Expand)\nc=can_unlock(Unlocks.Expand)");for(int i=0;i<1000&&session.Running;i++)session.FixedTick();var scope=session.drones[0].task.context.scope.values;Check(session.Error==""&&!session.Running&&ScriptValue.Truth(scope["a"])&&ScriptValue.Truth(scope["b"])&&!ScriptValue.Truth(scope["c"])&&ScriptValue.Number(scope["n"])==0,"New public research APIs distinguish funds, production and staged gates");
        foreach(var chapter in ClassicManual.Chapters)if(chapter.code!="")new ScriptParser().Parse(chapter.code);Check(ClassicManual.Chapters.Length==16&&ClassicManual.Apis.Length>=30,"All 16 bilingual manual chapters and their code fragments are present and parse");
        Check(ClassicBalance.CompetitionTimeLimit("reset")==14400&&ClassicBalance.CompetitionTimeLimit("hay")==3600,"Staged reset challenge permits a longer run than resource competitions");
        Directory.CreateDirectory("InternalTestReports/0.11.3");File.WriteAllText("InternalTestReports/0.11.3/progression.json",JsonUtility.ToJson(new Report{passed=names.Count,checks=names.ToArray()},true));Debug.Log("PROGRESSION CHECKS PASSED: "+names.Count);
    }
}
