using System;
using System.Linq;
using System.Collections.Generic;
using Verdant.Automation;

public static class VerdantBalanceChecks
{
    static List<string> names;
    [Serializable] public class Report{public int passed;public string[] checks;}
    static void Check(bool value,string name){if(!value)throw new Exception("BALANCE FAIL: "+name);names.Add(name);}
    static ClassicFarm Farm(int side=4)
    {
        var f=new ClassicFarm(17);f.Data.allowLockedSyntax=true;
        foreach(var t in ClassicFarm.Techs)f.Data.unlocks.Add(new Stock(t.id,t.max));
        f.Resize(side);foreach(string item in ClassicFarm.Items)f.Add(item,1000000);return f;
    }
    static int[] Distances(ClassicFarm f,int start)
    {
        var d=Enumerable.Repeat(-1,f.Data.cells.Count).ToArray();var q=new Queue<int>();q.Enqueue(start);d[start]=0;
        while(q.Count>0){int a=q.Dequeue();for(int dir=0;dir<4;dir++)if(f.CanMove(a,dir)){int b=f.NeighborIndex(a,dir,false);if(d[b]<0){d[b]=d[a]+1;q.Enqueue(b);}}}return d;
    }
    public static void Run()
    {
        names=new List<string>();var f=Farm();Check(ScriptValue.Number(f.Cost("speed",7)["hay"])==Math.Ceiling(20*Math.Pow(ClassicBalance.ResearchGrowth,7)),"Research follows the declared paid cost curve");
        Check(ClassicBalance.CompanionMultiplier(0)==1&&ClassicBalance.CompanionMultiplier(1)==2&&ClassicBalance.CompanionMultiplier(6)==4.5,"Companion bonuses unlock and scale gradually");
        f.Data.unlocks.RemoveAll(s=>s.id=="polyculture");var p=f.Cell(0,0);p.entity="carrot";p.growth=100;p.companion="tree";p.companionX=1;p.companionY=0;f.Cell(1,0).entity="tree";
        Check(f.Companion(0,0)==null,"Companion requests stay locked before research");double before=f.Count("carrot");f.Harvest(0,0);Check(f.Count("carrot")-before==128,"Locked companion planting grants no accidental bonus");
        Check(ClassicBalance.CactusMultiplier(4)==16&&ClassicBalance.CactusMultiplier(64)==1024&&ClassicBalance.CactusMultiplier(1024)==16384,"Cactus chains retain a bonus with bounded scaling");
        Check(ClassicBalance.BoneYield(1023,8)==180048&&ClassicBalance.BoneYield(63,1)==3969,"Tail output grows after its multiplier cap");
        foreach(bool stop in new[]{false,true}){f=Farm();var s=new ClassicSession(f);s.Start("change_hat(Hats.Dinosaur_Hat)\nwhile True:\n    pass");s.FixedTick();s.drones[0].hat="dinosaur";f.dinosaur=0;f.tail=Enumerable.Range(0,10).ToList();before=f.Count("bone");if(stop)s.Stop();else{s.Start("change_hat(Hats.Straw_Hat)");while(s.Running)s.FixedTick();}Check(f.Count("bone")-before==ClassicBalance.BoneYield(10,8),"Tail payout agrees for "+(stop?"stop":"hat change"));}
        foreach(int infected in new[]{1,4}){f=Farm(2);int index=0;foreach(var c in f.Data.cells){c.entity="pumpkin";c.growth=100;c.measure=1;c.ground="soil";c.infected=index++<infected;c.companion=null;}before=f.Count("weird_substance");f.Harvest(0,0);Check(f.Count("weird_substance")-before==128*infected,"Merged infection yield is proportional: "+infected+" / 4 plants");}
        f=Farm(1);int dead=0;bool noReroll=true;for(int i=0;i<100;i++){p=f.Cell(0,0);p.entity="pumpkin";p.growth=ClassicFarm.GrowTime("pumpkin")-1;p.measure=0;f.Use("fertilizer",0,0);if(p.entity=="dead_pumpkin")dead++;else{f.Use("fertilizer",0,0);noReroll &= p.entity=="pumpkin"&&p.measure==1;}}Check(dead>0&&dead<100,"Fertilizer cannot bypass pumpkin mortality");Check(noReroll,"Mature pumpkins do not reroll mortality");
        for(int seed=1;seed<=12;seed++){f=Farm(8);f.Data.randomState=(uint)seed;f.Cell(0,0).entity="bush";f.Use("weird_substance",0,0,1024);var d=Distances(f,0);Check(d.All(x=>x>=0)&&d[f.treasure]>=Math.Ceiling(d.Max()*.65),"Maze treasure requires a substantial connected route, seed "+seed);int old=f.treasure;f.Use("weird_substance",old%8,old/8,1024);d=Distances(f,old);Check(f.treasure!=old&&d[f.treasure]>=Math.Ceiling(d.Max()*.65),"Reused maze cannot pay repeatedly at the same tile, seed "+seed);}
        f=new ClassicFarm(41);ClassicBalance.ConfigureCompetition(f,"carrot",true);Check(f.Data.size==8&&f.MaxDrones==1&&f.Level("speed")==3&&f.Count("carrot")==0&&f.Count("hay")>0,"Single-drone competition has an isolated mid-tier preset");
        Check(new[]{"hay","wood","carrot","pumpkin","power","cactus","bone"}.Select(ClassicBalance.CompetitionTarget).Distinct().Count()==7,"Major production challenges have distinct quantity targets");
        System.IO.Directory.CreateDirectory("InternalTestReports/0.13.0");System.IO.File.WriteAllText("InternalTestReports/0.13.0/balance.json",UnityEngine.JsonUtility.ToJson(new Report{passed=names.Count,checks=names.ToArray()},true));
        UnityEngine.Debug.Log("BALANCE CHECKS PASSED: "+names.Count);
    }
}
