using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Verdant.Automation;
public static class VerdantUniverseCampaign
{
    static ClassicFarm farm;static ArchipelagoWorld world;static int ship,assertions;static readonly Dictionary<int,int> workers=new Dictionary<int,int>();static readonly List<double> times=new List<double>();
    static void Check(bool ok,string label){if(!ok)throw new Exception("UNIVERSE CAMPAIGN FAIL: "+label);assertions++;}
    static void Tick(){world.Advance(.25);}
    static void WaitShip(){for(int i=0;i<200000;i++){if(world.Data.ufos[0].phase==0)return;Tick();}throw new Exception("UFO fixture timeout");}
    static void Transfer(int from,int to,string item,int amount)
    {
        var u=world.Data.ufos[0];if(u.battery<40){if(world.Stock(u.planet,"solar_energy")<20){Check(world.RepositionUfo(ship,0),"Return empty UFO to Earth for fuel");WaitShip();}Check(world.RefuelUfo(ship,20),"Local UFO refueling consumes stored energy at "+u.planet);}
        Check(world.LaunchUfo(ship,from,to,item,amount),"Reserve "+item+" "+from+"→"+to);WaitShip();Check(world.Audit()=="","Cargo and credit ledger after "+item+" transfer");
    }
    static void Acquire(int p,string item,int amount,int depth=0)
    {
        if(depth>16)throw new Exception("Cyclic recipe dependency: "+item);int missing=amount-(int)world.Stock(p,item);if(missing<=0)return;
        var local=ArchipelagoWorld.Recipes.FirstOrDefault(r=>r.island==p&&r.item==item);
        if(local==null){var r=ArchipelagoWorld.Recipes.FirstOrDefault(v=>v.item==item);int from=r?.island??0;if(from==p)throw new Exception("Missing original Earth material: "+item);Acquire(from,item,missing,depth+1);Transfer(from,p,item,missing);return;}
        int cycles=(int)Math.Ceiling(missing/(double)(local.amount*4));int guard=0;
        while(local.inputs.Any(input=>world.Stock(p,input.Key)<input.Value*4*cycles)){if(++guard>40)throw new Exception("Inputs did not converge: "+item);foreach(var input in local.inputs)Acquire(p,input.Key,input.Value*4*cycles,depth+1);}
        int id=workers[p];Check(world.Work(id,p,item,cycles),"Native recipe accepted: "+item);
        for(int tick=0;tick<200000&&world.Unit(id).jobs.Count>0;tick++)Tick();Check(world.Unit(id).jobs.Count==0,"Native recipe completed: "+item);Check(world.Audit()=="","Production ledger after "+item);
    }
    public static void Run()
    {
        assertions=0;workers.Clear();times.Clear();farm=new ClassicFarm();foreach(var tech in ClassicFarm.Techs)farm.Data.unlocks.Add(new Stock(tech.id,tech.max));farm.Resize(8);foreach(string item in ClassicFarm.Items)farm.Add(item,1000000);world=farm.Fleet;world.Sell(0,"wood",1000);
        for(int p=0;p<4;p++){if(p>0){world.Data.islands[p].width=8;world.Data.islands[p].height=8;world.Data.islands[p].expansion=3;}world.Data.islands[p].hullTier=2;int id=world.Build(ArchipelagoWorld.Roles[p],2,2,p);Check(id>0,"Paid native 2×2 worker on "+p);workers[p]=id;string file="observer_"+p;farm.Data.programs.Add(new ClassicProgram(file,"while True:\n    worker_wait(10)"));world.RunProgram(id,file);}
        ship=world.BuildUfo();Check(ship>0,"Paid initial UFO");for(int p=1;p<4;p++)Transfer(0,p,"solar_energy",120);
        double start=world.Data.time;
        while(!world.Complete)
        {
            int order=world.Data.order;double began=world.Data.time;int attempts=0;
            while(!world.Ready&&attempts++<30){foreach(var goal in world.Current.goals)Acquire(goal.island,goal.item,goal.amount);}
            Check(world.Ready&&world.Submit(),"All goals submitted for contract "+(order+1));Check(world.Audit()=="","Post-contract global conservation "+(order+1));times.Add(world.Data.time-began);
            if(order==2){Acquire(0,"bronze",10);Check(world.ResearchUfo("cargo"),"Upgrade UFO payload using an actual returned bronze shipment");}
        }
        Directory.CreateDirectory("InternalTestReports/0.13.1");File.WriteAllText("InternalTestReports/0.13.1/campaign.json",JsonUtility.ToJson(new Report{status="passed",checks=assertions,gameSeconds=world.Data.time-start,shipments=world.Data.shipments,contracts=world.Data.order,orderSeconds=times.ToArray(),scope="Integration fixture: all research and original Earth inputs supplied; all native manufactured products, UFO fuel, physical queues and 8 contracts produced through actual rules."},true));File.WriteAllText("InternalTestReports/0.13.1/campaign-final.json",JsonUtility.ToJson(farm.Data,true));Debug.Log("UNIVERSE CAMPAIGN PASSED / "+assertions+" / "+world.Data.time);
    }
    [Serializable] public sealed class Report {public string status,scope;public int checks,contracts,shipments;public double gameSeconds;public double[] orderSeconds;}
}
