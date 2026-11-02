using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Verdant.Automation;

public static class VerdantFleetScenarioChecks
{
    static int checks;
    static void Check(bool ok,string name){if(!ok)throw new Exception("FLEET SCENARIO FAIL: "+name);checks++;Debug.Log("FLEET SCENARIO / "+name);}
    static ClassicFarm Fixture()
    {
        var farm=new ClassicFarm();foreach(var tech in ClassicFarm.Techs)farm.Data.unlocks.Add(new Stock(tech.id,tech.max));farm.Add("wood",100);farm.Add("carrot",100);Check(farm.Fleet.Start(),"Fixture charter pays ordinary starting cost");return farm;
    }
    static void Advance(ArchipelagoWorld world,double seconds)
    {for(int i=0;i<seconds*4;i++){world.Advance(.25);if(i%40==0){Check(world.Audit()=="",world.Audit());}}}
    static void AddProgram(ClassicFarm farm,string name,string code){farm.Data.programs.Add(new ClassicProgram(name,code));}
    public static void Run()
    {
        var farm=Fixture();var world=farm.Fleet;int pump=world.Build("irrigator",1,1,0),grower=world.Build("grower",1,1,0);
        world.Work(pump,0,"water",24);world.Work(grower,0,"grain",6);
        for(int i=0;i<5000&&!(world.Unit(grower).jobs.Count>0&&world.Unit(grower).jobs[0].phase==1);i++)world.Advance(.25);
        Check(world.Unit(grower).jobs.Count>0&&world.Unit(grower).jobs[0].phase==1,"Save checkpoint is inside a paid recipe");
        double consumed=ArchipelagoWorld.Get(world.Data.consumed,"water");
        string json=JsonUtility.ToJson(farm.Data);var resumed=new ClassicFarm{Data=JsonUtility.FromJson<ClassicSave>(json)};ArchipelagoWorld.Validate(resumed.Data.archipelago);
        Advance(world,400);Advance(resumed.Fleet,400);
        Check(JsonUtility.ToJson(world.Data)==JsonUtility.ToJson(resumed.Fleet.Data),"Recipe timers, input reservations and physical positions resume exactly");
        Check(consumed>0&&world.Stock(0,"grain")>=12,"Saved recipe consumes inputs only once");Check(world.Submit(),"First multi-objective order can be delivered");
        world.Work(pump,0,"water",24);world.Work(grower,0,"grain",16);Advance(world,500);Check(world.Submit(),"Second order consumes all required goods atomically");
        Check(!world.Submit(),"Cannot claim the next order without its island cargo");
        int hauler=world.Build("hauler",1,1,0);double water=world.Stock(0,"water");Check(world.Ship(hauler,0,1,"water",12),"Real depot stock can be reserved for a shipment");
        Check(world.Stock(0,"water")==water-12&&!world.Ship(hauler,0,1,"water",100000),"Accepted cargo reserves once; absent cargo is rejected rather than waiting forever");
        for(int i=0;i<1000&&world.Unit(hauler).flightTarget<0;i++)world.Advance(.25);
        Check(world.Unit(hauler).flightTarget>=0,"Save checkpoint is in an actual cross-island flight");Check(!world.Cancel(hauler)&&!world.Remove(hauler),"In-flight cancellation cannot duplicate or drop cargo");
        resumed=new ClassicFarm{Data=JsonUtility.FromJson<ClassicSave>(JsonUtility.ToJson(farm.Data))};ArchipelagoWorld.Validate(resumed.Data.archipelago);
        var halted=new ClassicFarm{Data=JsonUtility.FromJson<ClassicSave>(JsonUtility.ToJson(farm.Data))};halted.Fleet.HaltAll();Advance(halted.Fleet,180);
        Check(halted.Fleet.Data.units.All(u=>u.jobs.Count==0&&u.flightTarget<0)&&halted.Fleet.Audit()=="","Stop all lands in-flight cargo, refunds future manifests and halts queues without loss");
        Advance(world,180);Advance(resumed.Fleet,180);Check(JsonUtility.ToJson(world.Data)==JsonUtility.ToJson(resumed.Fleet.Data),"Flight, landing queue and manifest resume exactly");
        Check(world.Stock(1,"water")==12&&world.Audit()=="","Arrival credits local stock once and all seven cargo ledgers balance");
        Check(world.Work(grower,0,"grain",1),"New recipe can be queued for refund test");for(int i=0;i<2000&&world.Unit(grower).jobs[0].phase==0;i++)world.Advance(.25);
        water=world.Stock(0,"water");Check(world.Cancel(grower)&&world.Stock(0,"water")==water+1&&world.Audit()=="","Cancellation refunds paid recipe inputs without minting goods");
        world.Unit(grower).delay=0;world.Unit(grower).manual=true;world.Unit(grower).x=0;world.Unit(grower).y=0;double credits=world.Data.credits;
        Check(!world.ManualStep(grower,-1,0)&&world.Data.credits==credits-2,"Unsafe manual boundary move brakes and charges a bounded fee");
        Check(!world.ManualStep(grower,-1,0)&&world.Data.credits==credits-2,"Emergency cooldown prevents repeated per-frame fees");
        Check(world.Audit()=="","Fee, purchase, refund and order credit ledger reconciles");
        AddProgram(farm,"shared_math","def increment(value):\n    return value+1");
        AddProgram(farm,"数字123","import shared_math\nvalue=fleet_receive()\nvalue.append(999)\nresult=shared_math.increment(value[0])");
        var payload=new List<object>{3.0};Check(world.Send(pump,payload)&&world.RunProgram(pump,"数字123"),"A named Chinese/numeric subprogram imports a shared function module");payload[0]=99.0;
        Advance(world,8);Check(Convert.ToDouble(world.Result(pump))==4&&payload.Count==1,"Worker return values and message data are isolated copies");
        AddProgram(farm,"faulty","missing_name");AddProgram(farm,"healthy","result=41+1");world.RunProgram(pump,"faulty");world.RunProgram(grower,"healthy");Advance(world,2);
        Check(world.Unit(pump).codeError!=""&&Convert.ToDouble(world.Result(grower))==42,"A worker error cannot stop another island program");
        AddProgram(farm,"busy","while True:\n    pass");world.RunProgram(grower,"busy");double clock=world.Data.time;Advance(world,2);Check(world.Data.time==clock+2&&world.ProgramRunning(grower),"Infinite user loops have bounded execution and do not freeze simulation");world.StopProgram(grower);
        AddProgram(farm,"compute_then_wait","for i in range(2000):\n    pass\nstart=fleet_state()['time']\nfleet_wait(2)\nresult=fleet_state()['time']-start");world.RunProgram(grower,"compute_then_wait");Advance(world,40);
        Check(Convert.ToDouble(world.Result(grower))>=1.75,"Heavy computation cannot cancel a later requested wait through accumulated CPU debt");
        AddProgram(farm,"next_program","result=7");AddProgram(farm,"switch_program","fleet_run(fleet_self(),'next_program')\nresult=-1");world.RunProgram(grower,"switch_program");Advance(world,2);
        Check(Convert.ToDouble(world.Result(grower))==7,"Self-dispatch atomically replaces the caller without the old program deleting the new task");
        Check(world.Remove(grower)&&world.Result(grower)==null,"Removing a physical machine releases its transient result data");
        for(int i=0;i<16;i++)Check(world.Send(pump,i),"Inbox accepts bounded copied payload");Check(!world.Send(pump,17),"A full inbox rejects excess messages");while(world.InboxCount(pump)>0)world.Receive(pump);
        string corrupted=JsonUtility.ToJson(farm.Data);var invalid=JsonUtility.FromJson<ClassicSave>(corrupted);invalid.archipelago.units[0].width=999;
        bool rejected=false;try{ArchipelagoWorld.Validate(invalid.archipelago);}catch{rejected=true;}Check(rejected,"Corrupt hull saves are rejected before simulation");
        invalid=JsonUtility.FromJson<ClassicSave>(corrupted);invalid.archipelago.islands[0].items[0].amount=double.NaN;rejected=false;try{ArchipelagoWorld.Validate(invalid.archipelago);}catch{rejected=true;}Check(rejected,"Non-finite depot saves are rejected");
        var geometry=Fixture();geometry.Fleet.Data.order=18;geometry.Fleet.Data.sizeLevel=4;
        for(int island=0;island<6;island++)for(int tier=0;tier<5;tier++)
        {
            var unit=new FleetUnit{island=island,x=1,y=1,width=ArchipelagoWorld.Sizes[tier,0],height=ArchipelagoWorld.Sizes[tier,1]};
            foreach(int y in new[]{2,9})
            {
                var path=geometry.Fleet.Path(unit,15,y,false);Check(path.Count>0,"Every island admits hull tier "+tier+" to facility "+y);unit.x=15;unit.y=y;
                Check(geometry.Fleet.Path(unit,1,1,false).Count>0,"One-way loop returns hull tier "+tier+" to its dock");unit.x=unit.y=1;
            }
        }
        var autonomous=Fixture();var local=autonomous.Fleet;local.Data.order=5;local.Data.orderTimes.AddRange(new double[5]);local.Data.credits=100+ArchipelagoWorld.Contracts.Take(5).Sum(c=>c.reward);
        foreach(string path in Directory.GetFiles("docs/test-kits/full-flow-0.12.1/examples","*.py")){string code=File.ReadAllText(path);new ScriptParser().Parse(code);AddProgram(autonomous,Path.GetFileNameWithoutExtension(path),code);}
        var launcher=new ClassicSession(autonomous);launcher.Start(autonomous.Data.programs.First(p=>p.name=="local_master").source);
        for(int frame=0;frame<42000;frame++)launcher.Tick(1.0/60,1);
        Check(!launcher.Running&&launcher.Error==""&&local.Data.units.Count==4,"A finite master launches four separately named autonomous island programs");
        Check(local.Data.units.All(u=>local.ProgramRunning(u.id))&&local.Stock(2,"alloy")>0&&local.Data.programFaults==0,"Independent three-island production continues after the master has ended");
        Check(local.Audit()=="","Autonomous source-to-foundry network reconciles both cargo and credits");
        var recovery=Fixture();var merchant=recovery.Fleet;int recoveryPump=merchant.Build("irrigator",1,1,0);var vehicle=merchant.Unit(recoveryPump);vehicle.manual=true;vehicle.x=vehicle.y=0;
        for(int i=0;i<60&&merchant.Data.credits>0;i++){merchant.ManualStep(recoveryPump,-1,0);merchant.Advance(2);}
        Check(merchant.Data.credits==0&&merchant.Audit()=="","Repair penalties cannot create negative money or unrecorded charges");
        Check(merchant.Work(recoveryPump,0,"water",20),"A zero-credit player can still use the existing production machine");Advance(merchant,200);
        Check(merchant.Sell(0,"water",20)==1&&merchant.Data.credits==1,"Real surplus exports restore credit after a complete loss of spending money");
        Check(merchant.Sell(0,"water",100000)==0&&merchant.Sell(0,"water",-1)==0&&merchant.Audit()=="","Exports cannot sell missing goods, reverse a negative amount or break either ledger");
        vehicle.manual=true;vehicle.x=vehicle.y=0;vehicle.delay=0;merchant.ManualStep(recoveryPump,-1,0);merchant.Advance(2);Check(merchant.Data.credits==0,"Currency boundary fixture spends its remaining credit through an actual emergency fee");
        merchant.Work(recoveryPump,0,"water",50);Advance(merchant,450);for(int i=0;i<200;i++)merchant.Sell(0,"water",1);
        Check(merchant.Data.credits==10&&merchant.Audit()=="","Two hundred small exports reach the exact payable credit threshold without floating-point underfunding");
        Directory.CreateDirectory("InternalTestReports/0.12.1");File.WriteAllText("InternalTestReports/0.12.1/fleet-scenarios.json","{\"status\":\"passed\",\"checks\":"+checks+",\"midRecipeSave\":true,\"midFlightSave\":true,\"cargoLedger\":true,\"creditLedger\":true,\"subprograms\":true,\"isolatedMessages\":true,\"workerFailureIsolation\":true,\"allIslandHullPaths\":true}");Debug.Log("FLEET SCENARIOS PASSED / "+checks+" checks");
    }
}
