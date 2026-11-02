using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Verdant.Automation;

public static partial class VerdantFullPlaythrough
{
    static string guidePrograms;
    public static void RunPlayerGuide(){SessionState.SetBool("VerdantPlayerGuide",true);Run();}
    static string GuideCode(string name){return File.ReadAllText(Path.Combine(guidePrograms,name+".py"));}
    static void GuidePay(params string[] ids)
    {
        foreach(string id in ids)
        {
            var phase=Begin("research","手动研究 "+id+" → "+(farm.Level(id)+1),"玩家在科技树点击付费；不代为生产缺少的资源。");var timer=Stopwatch.StartNew();
            Check(farm.Unlock(id),"Guide omitted a prerequisite, production goal or payable resource for "+id);End(phase,timer);
            report.milestones.Add(new Milestone{id="research:"+id+":"+farm.Level(id),gameSeconds=farm.Data.time,wallSeconds=wall.Elapsed.TotalSeconds});
        }
    }
    static void GuideRun(string name,double target=-1)
    {
        string code=GuideCode(name);
        if(target>=0){code=code.Replace("target = 500","target = "+Number(target)).Replace("target = 900","target = "+Number(target));}
        Execute("运行玩家程序 "+name+(target>=0?"，target="+Number(target):""),"实际执行交付文件；保持普通语法、资源成本、随机数和自然生长。",code,28800,"guide-program");
        if(target>=0)Check(farm.Count(name)>=target,"Starter "+name+" stopped before target "+target);
    }
    static void RunPlayerGuideCore()
    {
        guidePrograms=Path.GetFullPath(Environment.GetEnvironmentVariable("VERDANT_GUIDE_PROGRAMS")??"docs/test-kits/full-flow-0.11.3/programs");
        output=Path.GetFullPath(Environment.GetEnvironmentVariable("VERDANT_PLAYTHROUGH_OUTPUT")??"InternalTestReports/player-guide");Directory.CreateDirectory(output);Directory.CreateDirectory(Path.Combine(output,"programs"));Directory.CreateDirectory(Path.Combine(output,"saves"));
        uint seed;if(!uint.TryParse(Environment.GetEnvironmentVariable("VERDANT_PLAYTHROUGH_SEED"),out seed))seed=731;
        wall=Stopwatch.StartNew();frames=0;resourceDepth=0;report=new Report{seed=seed,edition="Player-copyable full-flow kit / 0.11.3",unityVersion=Application.unityVersion,startedUtc=DateTime.UtcNow.ToString("o")};
        string source=string.Join("\n",Directory.GetFiles("Assets/VerdantCircuit/Scripts/Automation","*.cs").OrderBy(p=>p).Select(File.ReadAllText));using(var hash=System.Security.Cryptography.SHA256.Create())report.sourceFingerprint=BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(source))).Replace("-","").ToLowerInvariant();
        farm=new ClassicFarm(seed);farm.Data.programs.Add(new ClassicProgram("main","harvest()"));
        foreach(string path in Directory.GetFiles(guidePrograms,"*.py")){string code=File.ReadAllText(path);new ScriptParser().Parse(code);farm.Data.programs.Add(new ClassicProgram(Path.GetFileNameWithoutExtension(path),code));}
        observerObject=new GameObject("Isolated Player Guide Observer");observer=observerObject.AddComponent<ClassicController>();observer.enabled=false;observer.Farm=farm;observer.SavePath=Path.Combine(output,"final-farm.json");
        production=typeof(ClassicController).GetMethod("UpdateProduction",BindingFlags.Instance|BindingFlags.NonPublic);challengeNotice=typeof(ClassicController).GetMethod("NotifyChallengeCompleted",BindingFlags.Instance|BindingFlags.NonPublic);farm.Message+=observer.Log;
        try
        {
            Check(farm.Data.size==1&&ClassicFarm.Items.All(i=>farm.Count(i)==0)&&farm.Data.unlocks.Count==0&&!farm.Data.allowLockedSyntax,"Guide must start from a legitimate empty farm.");
            var first=Begin("bootstrap","重复运行 boot，收获5份干草","只执行 harvest()，不赠物资，不提前开放语法。",GuideCode("boot"));var watch=Stopwatch.StartNew();
            while(farm.Count("hay")<5){var session=new ClassicSession(farm);observer.Session=session;session.Start(GuideCode("boot"));while(session.Running){session.FixedTick();frames++;first.frames++;if(frames%60==0)Observe();}Check(session.Error=="",session.Error);}End(first,watch);
            GuidePay("loops");
            var phase=Begin("bootstrap-loop","运行 start，干草库存60时停止","玩家只需要循环研究；在指定库存点击停止。",GuideCode("start"));watch=Stopwatch.StartNew();var starter=new ClassicSession(farm);observer.Session=starter;starter.Start(GuideCode("start"));while(farm.Count("hay")<60){starter.FixedTick();frames++;phase.frames++;Check(phase.frames<1800*60,"Starter loop exceeded budget.");if(frames%60==0)Observe();}Check(starter.Error=="",starter.Error);starter.Stop();End(phase,watch);
            GuidePay("operators","variables","senses","plant","expand");
            GuideRun("hay",50);GuidePay("grass");
            GuideRun("hay",200);GuideRun("wood",30);GuidePay("trees","expand","grass","speed");
            GuideRun("wood",900);GuidePay("functions","lists","costs","timing","hats","carrots","watering");
            GuideRun("hay",500);GuideRun("carrot");Check(farm.Count("carrot")>=160,"Starter carrot seed budget is insufficient.");
            GuidePay("dicts","import","auto_unlock","debug","debug2","utilities");
            GuideRun("campaign");Check(ClassicFarm.Techs.All(t=>farm.Level(t.id)==t.max),"Delivered campaign failed to max every research node.");
            report.milestones.Add(new Milestone{id="guide:all-research-maxed",gameSeconds=farm.Data.time,wallSeconds=wall.Elapsed.TotalSeconds});
            if(Environment.GetEnvironmentVariable("VERDANT_FULL_0121")=="1")
            {
                report.edition="Verdant Circuit 0.12.1 / zero-inventory farm + archipelago campaign";
                foreach(string module in ClassicFleetExample.Names)farm.Data.programs.Add(new ClassicProgram(module,ClassicFleetExample.Code(module)));
                Execute("群岛开拓材料","正常生产100木材和100胡萝卜，准备真实开拓成本。","import farm_lib\nfarm_lib.ensure(Items.Wood,100)\nfarm_lib.ensure(Items.Carrot,100)",7200,"fleet-materials");
                farm.Fleet.OrderCompleted+=(order,seconds)=>report.milestones.Add(new Milestone{id="island-order:"+order,gameSeconds=farm.Data.time,wallSeconds=wall.Elapsed.TotalSeconds});
                Execute("群岛完整18单／多文件多机控制","真实解释器运行总控、规划器、七类单机程序与共享模块；所有机体、研究、配方与运输正常付费。",ClassicFleetExample.Source,72000,"archipelago-campaign");
                Check(farm.Fleet.Audit()=="",farm.Fleet.Audit());ArchipelagoWorld.Validate(farm.Fleet.Data);
                Check(farm.Fleet.Data.usedRoles.Count==6,"Reference campaign did not exercise all six specialisms.");
                Check(farm.Fleet.Complete&&farm.Fleet.Data.programFaults==0,"Archipelago modules did not complete cleanly.");
                Check(farm.Fleet.Data.peakPrograms>=3&&farm.Fleet.Data.nearMisses==0,"Independent programs or collision-free queues failed.");
                report.milestones.Add(new Milestone{id="complete:0.12.1-campaign",gameSeconds=farm.Data.time,wallSeconds=wall.Elapsed.TotalSeconds});
            }
            foreach(string name in new[]{"verify_companion","verify_maze","verify_drones","verify_simulation","verify_tail","verify_hat","verify_debug"})
            {
                GuideRun(name);var scope=observer.Session.drones[0].task.context.scope.values;
                string result=name=="verify_companion"?"companion_ok":name=="verify_maze"?"maze_reuse_ok":name=="verify_simulation"?"simulation_ok":name=="verify_tail"?"tail_ok":name=="verify_hat"?"hat_ok":null;
                if(result!=null)Check(ScriptValue.Truth(scope[result]),"Verification flag failed: "+result);
                if(name=="verify_drones")Check(report.phases.Last().peakDrones==32,"Player code did not reach 32 simultaneous workers.");
                if(name=="verify_debug")Check(farm.Data.size==32&&farm.Data.defaultSize==0,"Temporary debug size did not restore.");
            }
            GuideRun("races");Observe();Check(farm.Data.records.Count==16,"Player race driver did not finish all 16 categories.");Check(farm.Data.achievements.Count==13,"Player kit did not earn all 13 achievements.");
            report.status="passed";Finish();UnityEngine.Debug.Log("PLAYER GUIDE PASSED / "+report.gameSeconds+" game seconds / "+wall.Elapsed.TotalSeconds+" wall seconds");
        }
        catch(Exception e){report.status="failed";report.error=e.ToString();Finish();throw;}
        finally{UnityEngine.Object.DestroyImmediate(observerObject);}
    }
}
