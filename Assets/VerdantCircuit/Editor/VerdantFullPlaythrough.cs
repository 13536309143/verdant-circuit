using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using Verdant.Automation;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static partial class VerdantFullPlaythrough
{
    [Serializable] public class Phase
    {
        public int index;public string kind,title,algorithm,program,status="passed";public double gameStart,gameEnd,wallSeconds;public long frames,pulses,instructionTicks;public int spawned;public double isolatedGameSeconds;public string category;public int peakDrones;
        public Stock[] before,after;public string[] research;
    }
    [Serializable] public class Milestone {public string id;public double gameSeconds,wallSeconds;}
    [Serializable] public class Report
    {
        public string edition="Discovery Studio / "+ClassicBalance.Revision,unityVersion,sourceFingerprint,startedUtc,finishedUtc,status="running",error="";
        public uint seed;public bool freshZeroInventory=true,unrestrictedSyntax=false,resourceGrants=false,usesActualInterpreter=true;public double gameSeconds,wallSeconds;public long fixedFrames;public int researchNodes,maxedResearchNodes,achievements;public List<Phase> phases=new List<Phase>();public List<Milestone> milestones=new List<Milestone>();
    }
    static ClassicFarm farm;static ClassicController observer;static GameObject observerObject;static Report report;static Stopwatch wall;static string output;static long frames;static int resourceDepth;static MethodInfo production,challengeNotice;static readonly CultureInfo CI=CultureInfo.InvariantCulture;
    static readonly Dictionary<string,string> itemSymbols=new Dictionary<string,string>{{"hay","Hay"},{"wood","Wood"},{"carrot","Carrot"},{"pumpkin","Pumpkin"},{"solar_energy","Solar_Energy"},{"cactus","Cactus"},{"bone","Bone"},{"coins","Coins"},{"water","Water"},{"fertilizer","Fertilizer"},{"catalyst","Catalyst"}};
    static string Number(double n){return Math.Ceiling(n).ToString("0",CI);}
    static Stock[] Inventory(){return ClassicFarm.Items.Select(i=>new Stock(i,farm.Count(i))).ToArray();}
    static void Check(bool value,string text){if(!value)throw new Exception(text);}
    static VerdantFullPlaythrough(){EditorApplication.update+=BootstrapUpdate;}
    public static void Run()
    {
        SessionState.SetBool("VerdantFullPlaythrough",true);SessionState.SetFloat("VerdantFullPlaythroughStart",(float)EditorApplication.timeSinceStartup);
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);EditorApplication.isPlaying=true;
    }
    static void BootstrapUpdate()
    {
        if(!SessionState.GetBool("VerdantFullPlaythrough",false))return;
        if(EditorApplication.timeSinceStartup-SessionState.GetFloat("VerdantFullPlaythroughStart",0)>90){SessionState.SetBool("VerdantFullPlaythrough",false);UnityEngine.Debug.LogError("Full playthrough localization bootstrap timed out.");EditorApplication.Exit(1);return;}
        if(!EditorApplication.isPlaying||!UnityEngine.Localization.Settings.LocalizationSettings.InitializationOperation.IsDone)return;
        var table=UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetTableAsync("Verdant");if(!table.IsDone)return;
        SessionState.SetBool("VerdantFullPlaythrough",false);
        try{bool guide=SessionState.GetBool("VerdantPlayerGuide",false);SessionState.SetBool("VerdantPlayerGuide",false);if(SessionState.GetBool("VerdantUniverseFresh",false)){SessionState.SetBool("VerdantUniverseFresh",false);RunUniverseFreshCore();}else if(guide)RunPlayerGuideCore();else RunCore();EditorApplication.Exit(0);}catch(Exception e){UnityEngine.Debug.LogException(e);EditorApplication.Exit(1);}
    }
    static void RunCore()
    {
        output=Path.GetFullPath(Environment.GetEnvironmentVariable("VERDANT_PLAYTHROUGH_OUTPUT")??"InternalTestReports/full-playthrough");Directory.CreateDirectory(output);Directory.CreateDirectory(Path.Combine(output,"programs"));Directory.CreateDirectory(Path.Combine(output,"saves"));
        uint seed; if(!uint.TryParse(Environment.GetEnvironmentVariable("VERDANT_PLAYTHROUGH_SEED"),out seed))seed=731;
        UnityEngine.Localization.Settings.LocalizationSettings.InitializationOperation.WaitForCompletion();
        UnityEngine.Localization.Settings.LocalizationSettings.StringDatabase.GetTableAsync("Verdant").WaitForCompletion();
        wall=Stopwatch.StartNew();frames=0;resourceDepth=0;report=new Report{seed=seed,unityVersion=Application.unityVersion,startedUtc=DateTime.UtcNow.ToString("o")};
        var source=string.Join("\n",Directory.GetFiles("Assets/VerdantCircuit/Scripts/Automation","*.cs").OrderBy(p=>p).Select(File.ReadAllText));using(var hash=System.Security.Cryptography.SHA256.Create())report.sourceFingerprint=BitConverter.ToString(hash.ComputeHash(Encoding.UTF8.GetBytes(source))).Replace("-","").ToLowerInvariant();
        farm=new ClassicFarm(seed);farm.Data.programs.Add(new ClassicProgram("main","harvest()"));
        observerObject=new GameObject("Isolated Full Playthrough Observer");observer=observerObject.AddComponent<ClassicController>();observer.enabled=false;observer.Farm=farm;observer.SavePath=Path.Combine(output,"final-farm.json");
        production=typeof(ClassicController).GetMethod("UpdateProduction",BindingFlags.Instance|BindingFlags.NonPublic);challengeNotice=typeof(ClassicController).GetMethod("NotifyChallengeCompleted",BindingFlags.Instance|BindingFlags.NonPublic);farm.Message+=observer.Log;
        try
        {
            Check(farm.Data.size==1&&ClassicFarm.Items.All(i=>farm.Count(i)==0)&&!farm.Data.allowLockedSyntax&&farm.Data.unlocks.Count==0,"Start must have no gifted resources or research.");
            Boot();
            foreach(string id in new[]{"operators","variables","senses","plant","expand"})Purchase(id);UpgradeAvailable();
            Purchase("trees");UpgradeAvailable();
            foreach(string id in new[]{"functions","lists","costs","timing","hats","carrots"})Purchase(id);
            UpgradeAvailable();
            foreach(string id in new[]{"watering","dicts","import","auto_unlock","debug","debug2","utilities","polyculture"})Purchase(id);
            Purchase("pumpkins");Purchase("fertilizer");UpgradeAvailable();
            foreach(string id in new[]{"sunflowers","cactus","mazes","dinosaurs","megafarm","archipelago","leaderboard"}){Purchase(id);UpgradeAvailable();}
            Ensure("solar_energy",1000);VerifyCompanion();VerifyParallel();
            Purchase("the_farmers_remains");UpgradeAvailable();Purchase("top_hat");UpgradeAvailable();Execute("验证大师礼帽外观接口","研究奖励解锁后更换礼帽，再恢复草帽。","change_hat(Hats.Top_Hat)\nchange_hat(Hats.Straw_Hat)",120,"feature");
            VerifyMazeReuse();VerifyParallel();VerifyAllCompetitions();
            foreach(var tech in ClassicFarm.Techs)while(farm.Level(tech.id)<tech.max)Purchase(tech.id);
            Ensure("bone",100);Observe();
            Check(ClassicFarm.Techs.All(t=>farm.Level(t.id)==t.max),"Not every research node reached its maximum.");
            Check(farm.Data.achievements.Count==13,"Not every local achievement was earned.");
            Check(ClassicFarm.Items.Where(i=>!new[]{"water","fertilizer","catalyst"}.Contains(i)).All(i=>farm.Data.harvested.Any(s=>s.id==i&&s.amount>0)),"A production resource was never harvested.");
            report.status="passed";Finish();UnityEngine.Debug.Log("FULL PLAYTHROUGH PASSED / game "+report.gameSeconds.ToString("F3",CI)+"s / wall "+report.wallSeconds.ToString("F3",CI)+"s / phases "+report.phases.Count);
        }
        catch(Exception e){report.status="failed";report.error=e.ToString();Finish();throw;}
        finally{UnityEngine.Object.DestroyImmediate(observerObject);}
    }
    static void Observe()
    {
        var prior=new HashSet<string>(farm.Data.achievements);production.Invoke(observer,null);
        foreach(string id in farm.Data.achievements)if(!prior.Contains(id))report.milestones.Add(new Milestone{id="achievement:"+id,gameSeconds=farm.Data.time,wallSeconds=wall.Elapsed.TotalSeconds});
    }
    static void Finish()
    {
        report.gameSeconds=farm.Data.time;report.wallSeconds=wall.Elapsed.TotalSeconds;report.fixedFrames=frames;report.researchNodes=ClassicFarm.Techs.Length;report.maxedResearchNodes=ClassicFarm.Techs.Count(t=>farm.Level(t.id)==t.max);report.achievements=farm.Data.achievements.Count;report.finishedUtc=DateTime.UtcNow.ToString("o");observer.Save();WriteReport();
    }
    static void WriteReport(){File.WriteAllText(Path.Combine(output,"report.json"),JsonUtility.ToJson(report,true));}
    static Phase Begin(string kind,string title,string algorithm,string source=null)
    {
        var phase=new Phase{index=report.phases.Count+1,kind=kind,title=title,algorithm=algorithm,gameStart=farm.Data.time,before=Inventory()};report.phases.Add(phase);
        if(source!=null){phase.program="programs/"+phase.index.ToString("D4")+".py";File.WriteAllText(Path.Combine(output,phase.program),source+"\n");}return phase;
    }
    static void End(Phase phase,Stopwatch timer)
    {
        phase.gameEnd=farm.Data.time;phase.wallSeconds=timer.Elapsed.TotalSeconds;phase.after=Inventory();phase.research=farm.Data.unlocks.Select(s=>s.id+":"+s.amount.ToString("0",CI)).ToArray();
        WriteReport();File.WriteAllText(Path.Combine(output,"saves",phase.index.ToString("D4")+".json"),JsonUtility.ToJson(farm.Data));
        UnityEngine.Debug.Log("PLAYTHROUGH "+phase.index+" / "+phase.title+" / game "+farm.Data.time.ToString("F2",CI)+" / wall "+wall.Elapsed.TotalSeconds.ToString("F2",CI));
    }
    static void Execute(string title,string algorithm,string code,double limit=7200,string kind="production")
    {
        var phase=Begin(kind,title,algorithm,code);var timer=Stopwatch.StartNew();var session=new ClassicSession(farm);observer.Session=session;
        session.ChallengeCompleted=(category,seconds)=>{phase.category=category;phase.isolatedGameSeconds=seconds;challengeNotice.Invoke(observer,new object[]{category,seconds});report.milestones.Add(new Milestone{id="race:"+category+":"+seconds.ToString("F3",CI),gameSeconds=farm.Data.time,wallSeconds=wall.Elapsed.TotalSeconds});};
        farm.Data.programs[0].source=code;session.Start(code);long local=0;var knownDrones=new HashSet<ClassicDrone>(session.drones);
        try
        {
            while(session.Running)
            {
                foreach(var unit in session.drones)knownDrones.Add(unit);session.FixedTick();foreach(var unit in session.drones)knownDrones.Add(unit);phase.peakDrones=Math.Max(phase.peakDrones,session.drones.Count(d=>!d.finished));local++;frames++;if(frames%60==0)Observe();
                if(local>limit*60)throw new Exception("Program exceeded "+limit+" simulated seconds: "+title);
                if(wall.Elapsed.TotalSeconds>2400)throw new Exception("Full test exceeded 15 minutes wall time.");
            }
            Check(session.Error=="","Interpreter failure in "+title+": "+session.Error);
            phase.frames=local;phase.pulses=knownDrones.Sum(d=>d.task==null?0:d.task.Steps);phase.instructionTicks=knownDrones.Sum(d=>d.ticks);phase.spawned=session.TotalSpawned;End(phase,timer);
        }
        catch{phase.status="failed";phase.frames=local;End(phase,timer);throw;}
    }
    static void Boot()
    {
        var phase=Begin("bootstrap","起步：手动收获五份干草","重复运行 harvest()；等待自然成熟，不启用任何语法或成长加成。","harvest()");var timer=Stopwatch.StartNew();
        while(farm.Count("hay")<5){var session=new ClassicSession(farm);observer.Session=session;session.Start("harvest()");while(session.Running){session.FixedTick();frames++;phase.frames++;if(frames%60==0)Observe();}Check(session.Error=="",session.Error);phase.pulses+=session.drones.Sum(d=>d.task.Steps);phase.instructionTicks+=session.drones.Sum(d=>d.ticks);}
        End(phase,timer);Purchase("loops");HarvestUntil(60);
    }
    static void Purchase(string id)
    {
        var tech=ClassicFarm.Techs.First(t=>t.id==id);if(farm.Level(id)>=tech.max)return;if(tech.prerequisite!=""&&farm.Level(tech.prerequisite)==0)Purchase(tech.prerequisite);
        var requirements=farm.Requirements(id);
        foreach(var p in requirements){string key=Convert.ToString(p.Key);if(key.StartsWith("unlock:")){string parent=key.Substring(7);while(farm.Level(parent)<ScriptValue.Number(p.Value))Purchase(parent);}}
        foreach(var p in requirements){string key=Convert.ToString(p.Key);if(key.StartsWith("produced:")){string item=key.Substring(9);double missing=ScriptValue.Number(p.Value)-farm.Produced(item);if(missing>0)Ensure(item,farm.Count(item)+missing);}}
        var cost=farm.Cost(id);foreach(var p in cost)Ensure((string)p.Key,ScriptValue.Number(p.Value));int prior=farm.Level(id);
        if(farm.Level("auto_unlock")>0)
        {
            string symbol=string.Join("_",id.Split('_').Select(s=>char.ToUpperInvariant(s[0])+s.Substring(1)));if(id=="debug2")symbol="Debug_2";
            Execute("研究 "+id+" → "+(prior+1),"unlock() 按真实前置与库存扣费；无资源注入。","unlock(Unlocks."+symbol+")",120,"research");
        }
        else
        {
            var phase=Begin("research","研究 "+id+" → "+(prior+1),"点击科技树研究按钮；使用同一个付费研究方法，不计鼠标或思考时间。");var timer=Stopwatch.StartNew();Check(farm.Unlock(id),"Could not purchase "+id);End(phase,timer);
        }
        Check(farm.Level(id)==prior+1,"Research level failed: "+id);report.milestones.Add(new Milestone{id="research:"+id+":"+farm.Level(id),gameSeconds=farm.Data.time,wallSeconds=wall.Elapsed.TotalSeconds});
    }
    static void UpgradeAvailable()
    {
        bool changed;do{changed=false;foreach(string id in new[]{"expand","grass","speed","trees","carrots","pumpkins","sunflowers","watering","fertilizer","polyculture","cactus","mazes","dinosaurs","megafarm"})
        {if(!(universeFresh&&id=="expand"&&farm.Level("expand")>=3)&&(farm.Level(id)>0||id=="grass"||id=="speed")&&farm.ResearchRequirementsMet(id)){Purchase(id);changed=true;}}}while(changed);
    }
    static string Sweep(string body)
    {return "while get_pos_x()>0:\n    move(West)\nwhile get_pos_y()>0:\n    move(South)\nfor row in range(get_world_size()):\n    for col in range(get_world_size()):\n"+Indent(body,8)+"\n        if col+1<get_world_size():\n            move(East)\n    while get_pos_x()>0:\n        move(West)\n    if row+1<get_world_size():\n        move(North)\nwhile get_pos_y()>0:\n    move(South)";}
    static string Indent(string text,int count){string pad=new string(' ',count);return string.Join("\n",text.Split('\n').Select(l=>pad+l));}
    static string ClearSoil(){return "clear()\n"+Sweep("till()");}
    static string Water(){return farm.Level("watering")>0?"\nif get_water()<0.3 and num_items(Items.Water)>0:\n    use_item(Items.Water)":"";}
    static void Ensure(string item,double target)
    {
        if(farm.Count(item)>=target)return;if(++resourceDepth>12)throw new Exception("Cyclic resource planning: "+item);
        try
        {
            double goal=Math.Ceiling(target);string symbol=itemSymbols[item];int n=farm.Data.size;
            if(item=="hay"&&farm.Level("senses")==0)
            {
                // Bootstrap stops through the same Stop control used by a player; no comparison or sensor research is assumed.
                HarvestUntil(goal);return;
            }
            if(item=="hay"||item=="wood"||item=="carrot")
            {
                if(item=="carrot"){if(farm.Level("carrots")==0)Purchase("carrots");FundSeeds("carrot",goal);}
                string seed=item=="hay"?"":item=="wood"?(farm.Level("trees")>0?"if (get_pos_x()+get_pos_y())%2==0:\n    plant(Entities.Tree)\nelse:\n    plant(Entities.Bush)":"plant(Entities.Bush)"):"plant(Entities.Carrot)";
                string body="if can_harvest():\n    harvest()"+(seed==""?"":"\nif get_entity_type()==None:\n"+Indent(seed,4))+Water();
                string code=(item=="hay"?"clear()":ClearSoil())+"\nwhile num_items(Items."+symbol+")<"+Number(goal)+":\n"+Indent(Sweep(body),4);
                Execute("生产 "+item+" 至 "+Number(goal),item=="wood"?"棋盘树木与灌木；避免四邻树木减速，自动补种。":"逐行遍历、成熟判断与循环补种；根据实际库存结束。",code);
            }
            else if(item=="pumpkin"||item=="catalyst")
            {
                if(farm.Level("pumpkins")==0)Purchase("pumpkins");if(item=="catalyst"&&farm.Level("fertilizer")==0)Purchase("fertilizer");
                double yield=n*n*Math.Min(n,6)*Math.Pow(2,Math.Max(0,farm.Level("pumpkins")-1))*(item=="catalyst"?.5:1);
                double cycles=Math.Ceiling((goal-farm.Count(item))/yield);FundSeeds("pumpkin",farm.Count("pumpkin")+cycles*yield,Math.Max(2,cycles)*n*n*2.2);
                string code=PumpkinCode(Number(goal),item=="catalyst");
                Execute("生产 "+item+" 至 "+Number(goal),"反复扫描并补种枯死南瓜；整块成熟后收获。"+(item=="catalyst"?"按感染比例计算奇异物质，实际逐格消耗肥料，不再以一份肥料感染整块结算。":""),code);

            }
            else if(item=="cactus")
            {
                if(farm.Level("cactus")==0)Purchase("cactus");double yield=ClassicBalance.CactusMultiplier(n*n)*Math.Pow(2,Math.Max(0,farm.Level("cactus")-1));double cycles=Math.Ceiling((goal-farm.Count(item))/yield);
                FundSeeds("cactus",goal,(cycles+1)*n*n);
                string code="clear()\nwhile num_items(Items.Cactus)<"+Number(goal)+":\n"+Indent(Sweep("if get_ground_type()!=Grounds.Soil:\n    till()\nif get_entity_type()==None:\n    plant(Entities.Cactus)"+Water()),4)+"\n    complete=False\n    while not complete:\n        complete=True\n"+Indent(Sweep("if not can_harvest():\n    complete=False"),8)+"\n"+Indent(BoundedCactusSort(),4);
                Execute("生产 cactus 至 "+Number(goal),"行冒泡排序后列冒泡排序；二维有序后连锁收获。比较复杂度 O(n³)，输出按连锁株数与封顶倍率计算。",code);
            }
            else if(item=="solar_energy")
            {
                if(farm.Level("sunflowers")==0)Purchase("sunflowers");FundSeeds("sunflower",goal,Math.Max(2,Math.Ceiling((goal-farm.Count("solar_energy"))/(n*n*4.0)))*n*n);
                Execute("生产 solar_energy 至 "+Number(goal),"先等待整田成熟，再按 15→7 花瓣分成九档扫描；每档顺序收获，避免逐株全田查找的 O(n⁴) 扫描与能量开销。",PowerCode(Number(goal)));

            }
            else if(item=="bone")
            {
                if(farm.Level("dinosaurs")==0)Purchase("dinosaurs");while(farm.Count(item)<goal){Ensure("cactus",n*n+1);Execute("长尾生产 bone 至 "+Number(goal),"偶数网格上的哈密顿回路；只在边界受阻时结束，换帽结算尾长收益；倍率在长度 64 封顶，继续增长仍增加产量。",ClearSoil()+"\n"+ClassicExamples.Sources[8],7200);}
            }
            else if(item=="coins")
            {
                if(farm.Level("mazes")==0)Purchase("mazes");double mazeScale=Math.Pow(2,farm.Level("mazes")-1);Ensure("catalyst",Math.Ceiling((goal-farm.Count(item))/(n*n*mazeScale))*n*mazeScale);while(farm.Count(item)<goal){Execute("迷宫生产 coins 至 "+Number(goal),"迭代 DFS，显式栈回溯与坐标集合去重；到达宝藏后收获。",MazeStart()+"\n"+ClassicExamples.Sources[5],7200);}
            }
            else throw new Exception("No production planner for "+item);
            Check(farm.Count(item)>=target,"Production failed to reach "+item+" target "+target);
        }
        finally{resourceDepth--;}
    }
    static void HarvestUntil(double target)
    {
        var phase=Begin("bootstrap-loop","起步循环攒干草至 "+target,"玩家运行 while True 收获，在目标达成后点击停止。","while True:\n    harvest()");var timer=Stopwatch.StartNew();var s=new ClassicSession(farm);observer.Session=s;s.Start("while True:\n    harvest()");
        while(farm.Count("hay")<target){s.FixedTick();frames++;phase.frames++;if(frames%60==0)Observe();if(phase.frames>60*1800)throw new Exception("Bootstrap timed out.");}Check(s.Error=="",s.Error);phase.pulses=s.drones.Sum(d=>d.task.Steps);phase.instructionTicks=s.drones.Sum(d=>d.ticks);s.Stop();End(phase,timer);
    }
    static void FundSeeds(string entity,double target,double explicitCount=0)
    {
        int n=farm.Data.size;string tech=entity=="carrot"?"carrots":entity=="pumpkin"?"pumpkins":entity=="sunflower"?"sunflowers":entity;
        double yield=Math.Pow(2,Math.Max(0,farm.Level(tech)-1));double count=explicitCount>0?explicitCount:(Math.Ceiling(Math.Max(0,target-farm.Count(entity))/(n*n*yield))+1)*n*n;
        foreach(var p in farm.SeedCost(entity))Ensure((string)p.Key,Math.Ceiling(count*ScriptValue.Number(p.Value)));
    }
    static string MazeStart(){return "clear()\ntill()\nplant(Entities.Bush)\namount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)\nuse_item(Items.Catalyst,amount)";}
    static void VerifyCompanion()
    {
        Ensure("hay",200);Ensure("wood",200);string code="clear()\nentity,position=get_companion()\nx,y=position\nwhile get_pos_x()!=x:\n    move(East)\nwhile get_pos_y()!=y:\n    move(North)\ntill()\nplant(entity)\nwhile get_pos_x()!=0:\n    move(East)\nwhile get_pos_y()!=0:\n    move(North)\nwhile not can_harvest():\n    do_a_flip()\nharvest()";double prior=farm.Count("hay");Execute("验证随机伴生目标","读取伴生类型与坐标，布置邻作，回原地收获；检查实际额外收益。",code);Check(farm.Count("hay")-prior>Math.Pow(2,farm.Level("grass")),"Companion bonus was not earned.");
    }
    static void VerifyParallel()
    {
        bool stress=farm.MaxDrones==32;double before=farm.Count("hay");
        string timed="def column(seconds):\n    start=get_time()\n    while get_time()-start<seconds:\n        if can_harvest():\n            harvest()\n        move(North)\n    return True\nworkers=[]\nfor col in range(get_world_size()):\n    handle=spawn_drone(column,8)\n    if handle==None:\n        column(8)\n    else:\n        workers.append(handle)\n    move(East)\nfor worker in workers:\n    wait_for(worker)";
        string code="clear()\nwhile not can_harvest():\n    pass\n"+(stress?timed:ClassicExamples.Sources[6]);
        Execute(stress?"验证 32 台工作机同时生产":"验证独立工作机与等待","按列派生任务；槽满时主机回退执行；等待所有句柄结束。"+(stress?"工作机持续工作八游戏秒，保证同时占满 32 个槽位。":""),code);
        Check(report.phases.Last().spawned>1&&farm.Count("hay")>before,"Independent workers did not produce actual resources.");
        if(stress)Check(report.phases.Last().peakDrones==32,"The maximum-size fleet never reached 32 concurrent workers.");
    }
    static void VerifySimulation()
    {
        farm.Data.programs.Add(new ClassicProgram("experiment","for i in range(rounds):\n    if random()>0.5:\n        do_a_flip()"));
        Execute("验证固定种子模拟","两个相同种子的隔离农场执行相同实验，返回游戏时间应相等。","a=simulate('experiment', {}, {}, {'rounds':64}, 7, 64)\nb=simulate('experiment', {}, {}, {'rounds':64}, 7, 64)\nc=simulate('experiment', {}, {}, {'rounds':64}, 8, 64)\nprint(a==b)",120,"simulation");
        var values=observer.Session.drones[0].task.context.scope.values;double a=ScriptValue.Number(values["a"]),b=ScriptValue.Number(values["b"]),c=ScriptValue.Number(values["c"]);Check(a==b&&a!=c,"Simulation did not reproduce the same seed or react to a different seed.");report.phases.Last().isolatedGameSeconds=a+b+c;
        File.WriteAllText(Path.Combine(output,"simulation-audit.json"),"{\"sameSeedA\":"+a.ToString("R",CI)+",\"sameSeedB\":"+b.ToString("R",CI)+",\"differentSeed\":"+c.ToString("R",CI)+",\"globalsRounds\":64}");
    }
    static void VerifyMazeReuse()
    {
        Ensure("catalyst",farm.Data.size*Math.Pow(2,farm.Level("mazes")-1)*2);
        string search=ClassicExamples.Sources[5];int end=search.LastIndexOf("if get_entity_type() == Entities.Treasure:",StringComparison.Ordinal);search=search.Substring(0,end);
        double before=farm.Count("coins");Execute("验证迷宫复用与开墙","第一次寻宝后复用，重新 DFS 搜索随机新目标，最后收获；算法允许地图出现环。",MazeStart()+"\n"+search+"\nuse_item(Items.Catalyst,amount)\n"+ClassicExamples.Sources[5]);Check(farm.Count("coins")>before,"Maze reuse did not produce coins.");
    }
    static void VerifyAllCompetitions()
    {
        RunCompetitionMatrix();
    }
}
