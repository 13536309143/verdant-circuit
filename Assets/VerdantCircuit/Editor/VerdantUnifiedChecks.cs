using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Verdant.Automation;

public static class VerdantUnifiedChecks
{
    static int checks;static List<string> names;static double campaignSeconds;
    static void Check(bool ok,string name){if(!ok)throw new Exception("UNIFIED FAIL: "+name);checks++;names.Add(name);}
    static ClassicFarm Fixture(bool mature=false)
    {
        var f=new ClassicFarm();foreach(var t in ClassicFarm.Techs)f.Data.unlocks.Add(new Stock(t.id,t.max));f.Resize(4);var w=f.Fleet;w.Start();f.Add("wood",10000);w.Sell(0,"wood",10000);
        if(mature){w.Data.order=18;w.Data.orderTimes=Enumerable.Repeat(0.0,18).ToList();w.Data.credits+=ArchipelagoWorld.Contracts.Take(18).Sum(c=>c.reward);for(int i=0;i<4;i++)Check(w.Research("size"),"Paid hull research "+i);}
        return f;
    }
    static void Step(ArchipelagoWorld w,double seconds){int ticks=(int)Math.Ceiling(seconds*4);while(ticks-->0)w.Advance(.25);}
    static void Wait(ArchipelagoWorld w,int id,double limit=20000)
    {
        for(int i=0;i<limit*4;i++){var u=w.Unit(id);if(u.jobs.Count==0&&u.flightTarget<0)return;w.Advance(.25);}throw new Exception("Timed out #"+id+" "+w.Unit(id).status+" / "+w.Unit(id).jobs[0].item);
    }
    static void Program(ClassicFarm f,string name,string code){f.Data.programs.Add(new ClassicProgram(name,code));}
    public static void Run()
    {
        checks=0;names=new List<string>();
        var fresh=new ClassicFarm();var fw=fresh.Fleet;Check(!fw.Start()&&!fw.IsUnlocked(6),"A fresh Earth garden cannot bypass later island research");fresh.Advance(4);int earth=fw.EnsureEarthDrone();Program(fresh,"初次收获","harvest()");Check(fw.RunProgram(earth,"初次收获"),"Bind a Chinese-named original crop program before island research");fresh.Advance(2);Check(fresh.Count("hay")==1&&!fw.ProgramRunning(earth),"Bound Earth farmer harvests the original field and inventory");
        fresh.Add("wood",50);Check(fw.Stock(0,"wood")==50,"Earth inventory and home depot are the same storage");
        var one=new ClassicFarm();foreach(string id in new[]{"archipelago","flower_island"})one.Data.unlocks.Add(new Stock(id,1));Check(one.Fleet.IsUnlocked(6)&&!one.Fleet.IsUnlocked(7)&&!one.Fleet.IsUnlocked(8),"Flower branch opens independently without falsely opening earlier island IDs");Check(!one.Fleet.Work(999,7,"stone",1)&&!one.Fleet.Ship(999,0,7,"wood",2),"Locked specialist islands reject production and shipping");
        var f=Fixture();var w=f.Fleet;Check(w.Data.started&&f.Level("simulation")==0&&!ClassicHost.Api.Contains("simulate"),"Network replaces the retired laboratory");
        Check(ClassicProgression.Requirements("archipelago",1).ContainsKey("unlock:import")&&ClassicProgression.Requirements("archipelago",1).ContainsKey("unlock:dicts"),"Network research requires programming discoveries");
        Check(w.Research("engineer")&&w.Research("harvester"),"Mineral and fish discoveries independently open appropriate specialist roles");
        int hauler=w.Build("hauler",1,1,0),grower=w.Build("grower",1,1,6),engineer=w.Build("engineer",1,1,7),fisher=w.Build("harvester",1,1,8);Check(new[]{hauler,grower,engineer,fisher}.All(id=>id>0),"Unified slot capacity supports multi-island specialists");
        f.Add("water",40);Check(w.Ship(hauler,0,6,"water",24)&&f.Count("water")==16,"Shipping reserves real Earth inventory immediately");Check(!w.Ship(hauler,0,6,"water",24),"Reserved cargo cannot be spent twice");Wait(w,hauler);Check(w.Stock(6,"water")==24&&w.Data.shipments==2,"Multi-hold flight unloads each cargo once");Check(w.Audit()=="","Unified Earth transfer and physical cargo ledgers balance");
        f.Add("hay",10);Check(w.Ship(hauler,0,8,"hay",6),"Earth hay becomes a fisheries input");Wait(w,hauler);f.Add("water",10);w.Ship(hauler,0,8,"water",3);Wait(w,hauler);Check((bool)w.Invoke("catch_carp",new object[]{(double)fisher,1.0}),"Named fisheries function queues the correct recipe");Wait(w,fisher);Check(w.Stock(8,"carp")==3&&w.Stock(8,"hay")==4,"Fish production consumes the actual delivered Earth inputs");
        Check(w.Ship(hauler,8,0,"carp",3),"Fish products can return to Earth");Wait(w,hauler);Check(f.Count("carp")==3&&w.Audit()=="","Returned fish lives in the original Earth inventory with conservation intact");
        Check(!(bool)w.Invoke("mine_stone",new object[]{(double)grower,1.0}),"Product functions reject the wrong specialist role");Check(!w.Rebase(w.EnsureEarthDrone(),6),"Earth-only farmers cannot be stranded in island logistics queues");
        Program(f,"单机A","result = 11\nfleet_wait(5)");Program(f,"单机B","result = 22\nfleet_wait(5)");Check(w.BindProgram(grower,"单机A")&&!w.ProgramRunning(grower),"Binding alone persists the file without execution");Check(w.RunProgram(grower,"单机A")&&w.RunProgram(grower,"单机B"),"A second binding replaces only the same drone interpreter");Step(w,6);Check(ScriptValue.Number(w.Result(grower))==22,"Single drone has one final independent result");
        Program(f,"worker_a","fleet_wait(3)\nresult = 7");Program(f,"worker_b","fleet_wait(4)\nresult = 9");
        Program(f,"manager_a","fleet_run("+grower+", 'worker_a')");Program(f,"manager_b","fleet_run("+engineer+", 'worker_b')");Program(f,"master","island_run(6, 'manager_a')\nisland_run(7, 'manager_b')");Check(w.RunControl("network","master"),"Network controller launches a real hierarchical program");Step(w,1);Check(!w.ControlRunning("network")&&w.ProgramRunning(grower)&&w.ProgramRunning(engineer),"Children remain alive after their network master exits");Check(w.ProgramUsers("worker_a")>0,"Program activity includes bound island workers");Step(w,6);Check(ScriptValue.Number(w.Result(grower))==7&&ScriptValue.Number(w.Result(engineer))==9,"Different islands finish distinct independent programs");
        Program(f,"group_worker","fleet_send(fleet_self(), {'value': fleet_self()})\nfleet_wait(1)\nresult = fleet_receive()");Check(w.CreateGroup("双岛机群",new List<int>{grower,engineer}),"Named groups persist machines on multiple islands");Check(!(bool)w.Invoke("group_create",new object[]{"bad",new List<object>{(double)grower,(double)grower}}),"Group duplicate IDs are rejected");Check((bool)w.Invoke("group_run",new object[]{"双岛机群","group_worker"}),"A group dispatches independent copies of its file");Step(w,3);var gr=(Dictionary<object,object>)w.Result(grower);var er=(Dictionary<object,object>)w.Result(engineer);Check(ScriptValue.Number(gr["value"])==grower&&ScriptValue.Number(er["value"])==engineer,"Group workers keep separate variables, inboxes and returned objects");
        Program(f,"bad_worker","value = 1 / 0");Program(f,"good_worker","fleet_wait(2)\nresult = 42");w.RunProgram(grower,"bad_worker");w.RunProgram(engineer,"good_worker");Step(w,4);Check(w.Unit(grower).codeError!=""&&ScriptValue.Number(w.Result(engineer))==42,"One worker failure does not kill a different island worker");
        Program(f,"cpu_hog","while True:\n    pass");Program(f,"patient","fleet_wait(4)\nresult = 8");w.RunControl("island:6","cpu_hog");w.RunProgram(engineer,"patient");Step(w,2);Check(w.ProgramRunning(engineer),"Busy island manager cannot erase another program's wait");Step(w,4);Check(ScriptValue.Number(w.Result(engineer))==8,"Instruction budgets preserve fairness across control levels");w.HaltAll();
        var reloaded=new ClassicFarm{Data=JsonUtility.FromJson<ClassicSave>(JsonUtility.ToJson(f.Data))};ArchipelagoWorld.Validate(reloaded.Fleet.Data);Check(reloaded.Fleet.Data.controls.Any(c=>c.scope=="network"&&c.program=="master")&&reloaded.Fleet.Data.groups.Any(g=>g.name=="双岛机群"),"Save roundtrip retains hierarchy and group membership");Check(!reloaded.Fleet.AnyCodeRunning&&reloaded.Fleet.Unit(engineer).program=="patient","Load preserves bindings without pretending to restore interpreter stacks");
        var fork=Fixture();int farmer=fork.Fleet.EnsureEarthDrone();Program(fork,"fork","def work(n):\n    return n * 2\na = spawn_drone(work, 7)\nresult = wait_for(a)");fork.Fleet.RunProgram(farmer,"fork");fork.Advance(6);Check(ScriptValue.Number(fork.Fleet.Result(farmer))==14&&fork.Fleet.Data.units.Count==1,"Original function spawning works inside bound Earth programs and releases temporary slots");
        Program(fork,"debug_size","set_world_size(2)\nharvest()");fork.Fleet.RunProgram(farmer,"debug_size");fork.Advance(6);Check(fork.Data.size==4&&fork.Data.defaultSize==0,"Bound program cleanup restores temporary field size");
        foreach(string file in UnifiedExamples.Names)new ScriptParser().Parse(UnifiedExamples.Code(file));Check(true,"Every shipped hierarchical example parses");
        foreach(var entry in UnifiedManual.Functions){new ScriptParser().Parse(entry.example);Check(!string.IsNullOrWhiteSpace(entry.parametersEn)&&!string.IsNullOrWhiteSpace(entry.zh)&&!string.IsNullOrWhiteSpace(entry.returns),"Complete function reference: "+entry.name);}
        Check(ClassicHost.Api.Concat(ArchipelagoWorld.Api).Concat((string[])typeof(ScriptTask).Assembly.GetType("Verdant.Automation.Call").GetField("BuiltinNames").GetValue(null)).All(api=>UnifiedManual.Functions.Any(entry=>entry.name==api)),"Every public game function and builtin has an individual documented entry");
        Check(ArchipelagoWorld.NatureFunctions.Length==20&&ArchipelagoWorld.NatureItems.Distinct().Count()==20,"Every specialist product has a unique identifier and function");
        foreach(var recipe in ArchipelagoWorld.Recipes.Where(r=>ArchipelagoWorld.NatureItems.Contains(r.item)))Check(recipe.inputs.Keys.All(item=>ArchipelagoWorld.Goods.Contains(item))&&ArchipelagoWorld.Goods.Contains(recipe.item),"Valid recipe material graph: "+recipe.item);
        var old=new ClassicSave();old.items.Add(new Stock("water",7));old.archipelago=new ArchipelagoSave{started=true};for(int i=0;i<6;i++)old.archipelago.islands.Add(new IslandStock{island=i});old.archipelago.islands[0].items.Add(new Stock("water",13));old.archipelago.islands[0].made.Add(new Stock("water",13));
        string priorJson=JsonUtility.ToJson(old);ArchipelagoWorld.Validate(old.archipelago);Check(JsonUtility.ToJson(old)==priorJson,"Save validation is read-only and never consumes a legacy home depot");
        typeof(ClassicController).GetMethod("Validate",System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.NonPublic).Invoke(null,new object[]{old});var migrated=new ClassicFarm{Data=old};Check(migrated.Fleet.Stock(0,"water")==20&&old.archipelago.islands[0].items.Count==0,"Old home and island-zero depots merge into the real farm exactly once");var second=new ClassicFarm{Data=JsonUtility.FromJson<ClassicSave>(JsonUtility.ToJson(old))};Check(second.Fleet.Stock(0,"water")==20&&second.Fleet.Audit()=="","Reloading a migrated farm never duplicates Earth cargo");
        var economy=new ClassicFarm();var ew=economy.Fleet;double credits=ew.Data.credits;for(int i=0;i<20;i++){int id=ew.EnsureEarthDrone();Check(ew.Remove(id),"Free starter can be dismantled and replaced "+i);}Check(ew.Data.credits==credits&&ew.Data.refunds==0,"Free starter replacement never creates saleable credits");
        var interrupted=Fixture();int root=interrupted.Fleet.EnsureEarthDrone();Program(interrupted,"children","def child():\n    fleet_wait(100)\na = spawn_drone(child)\nb = spawn_drone(child)\nfleet_wait(100)");interrupted.Fleet.RunProgram(root,"children");Step(interrupted.Fleet,1);var temporary=interrupted.Fleet.Data.units.Where(u=>u.transient).Select(u=>u.id).ToList();Check(temporary.Count==2&&interrupted.Fleet.CreateGroup("temporary",temporary),"Temporary workers may join a group");interrupted.Fleet.HaltAll();Check(interrupted.Fleet.Data.units.Count==1&&interrupted.Fleet.Data.groups[0].members.Count==0&&!interrupted.Fleet.AnyCodeRunning,"Stop all atomically releases temporary slots and group memberships");
        ProductCases();Directory.CreateDirectory("InternalTestReports/0.13.0");NewCampaign();
        Directory.CreateDirectory("InternalTestReports/0.13.0");File.WriteAllText("InternalTestReports/0.13.0/unified.json",JsonUtility.ToJson(new Report{status="passed",assertions=checks,referenceEntries=UnifiedManual.Functions.Length,newCampaignGameSeconds=campaignSeconds,checks=names.ToArray()},true));Debug.Log("UNIFIED CHECKS PASSED / "+checks);
    }
    [Serializable] public sealed class Report {public string status;public int assertions,referenceEntries;public double newCampaignGameSeconds;public string[] checks;}
    static void ProductCases()
    {
        for(int i=0;i<ArchipelagoWorld.NatureFunctions.Length;i++)
        {
            var f=Fixture(true);var w=f.Fleet;string item=ArchipelagoWorld.NatureItems[i];var recipe=ArchipelagoWorld.Recipes.First(r=>r.item==item);string role=recipe.roles[0];if(!w.Data.roles.Contains(role))w.Research(role);
            int id=w.Build(role,3,3,recipe.island);Check(id>0,"Product fixture creates matching specialist: "+item);
            foreach(var input in recipe.inputs){int amount=input.Value*9;if(recipe.island==0)f.Add(input.Key,amount);else{w.Add(recipe.island,input.Key,amount);ArchipelagoWorld.Put(w.Data.islands[recipe.island].made,input.Key,amount);}}
            Program(f,"product_case","result = "+ArchipelagoWorld.NatureFunctions[i]+"(fleet_self(), 1)");w.RunProgram(id,"product_case");
            for(int tick=0;tick<4000&&!(w.Unit(id).jobs.Count>0&&w.Unit(id).jobs[0].phase==1);tick++)w.Advance(.25);
            Check(w.Unit(id).jobs.Count==1&&w.Unit(id).jobs[0].phase==1,"Named function reaches real processing: "+item);
            string saved=JsonUtility.ToJson(f.Data);var loaded=new ClassicFarm{Data=JsonUtility.FromJson<ClassicSave>(saved)};var world=loaded.Fleet;ArchipelagoWorld.Validate(world.Data);Wait(world,id);
            Check(world.Stock(recipe.island,item)==recipe.amount*9,"Mid-recipe save restores area-scaled output exactly: "+item);
            Check(world.Audit()=="","Saved product input and output ledger balances: "+item);
        }
    }
    static ClassicFarm campaign;static Dictionary<string,int> producers;static int transport;
    static int Producer(FleetRecipe recipe)
    {
        string role=recipe.roles[0];string key=recipe.island+":"+role;int id;if(producers.TryGetValue(key,out id))return id;
        if(!campaign.Fleet.Data.roles.Contains(role))Check(campaign.Fleet.Research(role),"Campaign purchases specialist "+role);
        id=campaign.Fleet.Build(role,3,3,recipe.island);if(id<0)throw new Exception("Cannot build producer "+key);producers[key]=id;
        if(producers.Count<=3){string file="campaign_worker_"+producers.Count;Program(campaign,file,"while True:\n    fleet_wait(10)");campaign.Fleet.RunProgram(id,file);}return id;
    }
    static void Acquire(int island,string item,int amount,int depth=0)
    {
        if(depth>20)throw new Exception("Cyclic production dependency: "+item);var w=campaign.Fleet;int shortage=amount-(int)w.Stock(island,item);if(shortage<=0)return;
        var recipe=ArchipelagoWorld.Recipes.FirstOrDefault(r=>r.island==island&&r.item==item);
        if(recipe==null)
        {
            var source=ArchipelagoWorld.Recipes.FirstOrDefault(r=>r.item==item);int from=source?.island??0;
            if(from==island)throw new Exception("Missing Earth input "+item);Acquire(from,item,shortage,depth+1);Check(w.Ship(transport,from,island,item,shortage),"Campaign reserves shipment "+item+" "+from+"→"+island);Wait(w,transport);return;
        }
        int id=Producer(recipe);int batches=(int)Math.Ceiling(shortage/(double)(recipe.amount*9));
        int refill=0;while(recipe.inputs.Any(input=>w.Stock(island,input.Key)<input.Value*9*batches)){if(++refill>100)throw new Exception("Recipe input refill did not converge: "+item);foreach(var input in recipe.inputs)Acquire(island,input.Key,input.Value*9*batches,depth+1);}
        Check(w.Work(id,island,item,batches),"Campaign queues "+item);Wait(w,id);Check(w.Audit()=="","Campaign cargo conservation after "+item);
    }
    static void NewCampaign()
    {
        campaign=Fixture(true);producers=new Dictionary<string,int>();var w=campaign.Fleet;
        // Integration fixture starts at the existing campaign boundary; only original Earth supplies are seeded.
        foreach(string item in ClassicFarm.Items)campaign.Add(item,1000000);
        transport=w.Build("hauler",3,3,0);Check(transport>0,"Mature campaign builds a paid heavy transporter");
        double start=w.Data.time;
        while(!w.Complete)
        {
            int index=w.Data.order;int retries=0;
            while(!w.Ready&&retries++<12){foreach(var goal in w.Current.goals)Acquire(goal.island,goal.item,goal.amount);}
            Check(w.Ready&&w.Submit(),"All multi-product contract goals complete: "+(index+1));Check(w.Audit()=="","Order submission conserves all unified stock: "+(index+1));
        }
        campaignSeconds=w.Data.time-start;Check(w.Data.order==24&&campaign.Count("diamond")>=0,"All six new specialist contracts complete through real production and flights");
        File.WriteAllText("InternalTestReports/0.13.0/campaign-final.json",JsonUtility.ToJson(campaign.Data,true));
    }
}
