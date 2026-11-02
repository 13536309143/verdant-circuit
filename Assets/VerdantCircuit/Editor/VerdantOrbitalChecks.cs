using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Verdant.Automation;
public static class VerdantOrbitalChecks
{
    static readonly List<string> checks=new List<string>();
    static void Check(bool value,string name){if(!value)throw new Exception("ORBITAL FAIL: "+name);checks.Add(name);}
    static void Tick(ClassicFarm f,double seconds){for(int i=0;i<seconds*4;i++)f.Fleet.Advance(.25);}
    static void Dock(ClassicFarm f){for(int i=0;i<100000;i++){if(f.Fleet.Data.ufos[0].phase==0)return;f.Fleet.Advance(.25);}throw new Exception("UFO docking timeout");}
    static ClassicFarm Fixture(){var f=new ClassicFarm();foreach(var t in ClassicFarm.Techs)f.Data.unlocks.Add(new Stock(t.id,t.max));f.Resize(8);foreach(string item in ClassicFarm.Items)f.Add(item,100000);f.Fleet.Sell(0,"wood",1000);return f;}
    public static void Run()
    {
        checks.Clear();var fresh=new ClassicFarm();float initial=ClassicView.CropScale(fresh.Cell(0,0));fresh.Advance(.5);float seedling=ClassicView.CropScale(fresh.Cell(0,0));fresh.Advance(.5);float growing=ClassicView.CropScale(fresh.Cell(0,0));fresh.Advance(3);Check(initial<seedling&&seedling<growing&&growing<ClassicView.CropScale(fresh.Cell(0,0)),"Crop shape follows continuous real growth rather than a maturity toggle");
        int harvests=0;fresh.Harvested+=(x,y,item,amount)=>harvests++;Check(fresh.Harvest(0,0)&&harvests==1,"Successful harvest emits one visual event");Check(!fresh.Harvest(0,0)&&harvests==1,"Failed harvest does not emit a fake effect");
        var f=Fixture();var w=f.Fleet;int ship=w.BuildUfo();double before=f.Count("water");Check(w.LaunchUfo(ship,0,-1,"water",50),"Mother warehouse is a distinct real transport destination");Check(w.Stock(-1,"water")==0,"Mother warehouse receives no instant goods");Dock(f);Check(w.Stock(-1,"water")==50&&w.Data.hubVisits>0,"Physical UFO unloads into mother warehouse");Check(w.Audit()=="","Mother warehouse participates in material conservation");
        Check(w.LaunchUfo(ship,-1,1,"water",30),"Goods can be retrieved from mother warehouse");Dock(f);Check(w.Stock(-1,"water")==20&&w.Stock(1,"water")==30,"Mother withdrawal reserves and delivers exactly once");
        Check(w.LaunchUfo(ship,0,2,"water",24),"Planet route accepted");bool visited=false;for(int i=0;i<10000&&w.Data.ufos[0].phase!=0;i++){f.Fleet.Advance(.25);if(w.Data.ufos[0].planet==-1)visited=true;}Check(visited&&w.Stock(2,"water")==24,"Planet-to-planet cargo physically passes through mothership");Check(w.Audit()=="","Hub routes preserve cargo ledger");
        Check(w.LaunchUfo(ship,0,3,"water",80),"Cancellable multi-hold route accepted");Tick(f,3);Check(w.CancelUfo(ship),"Cancel flight toward mother");Dock(f);Check(w.Stock(0,"water")+w.Stock(-1,"water")+w.Stock(1,"water")+w.Stock(2,"water")+w.Stock(3,"water")==before,"Cancel at any intermediate dock conserves all water");
        ArchipelagoWorld.Validate(JsonUtility.FromJson<ClassicSave>(JsonUtility.ToJson(f.Data)).archipelago);Check(true,"Intermediate-dock saves pass strict validation");
        foreach(var t in ArchipelagoWorld.PlanetTechnologies)
        {
            int depot=t.planet==-1?0:t.planet;
            if(t.parent!=""&&w.PlanetLevel(t.planet,t.parent)==0){var parent=ArchipelagoWorld.PlanetTechnologies.First(v=>v.planet==t.planet&&v.id==t.parent);w.Add(depot,parent.item,w.TechnologyCost(parent));Check(w.ResearchPlanet(parent.planet,parent.id),"Paid branch prerequisite: "+t.id);}
            double stock=w.Stock(depot,t.item);int cost=w.TechnologyCost(t);if(stock<cost)w.Add(depot,t.item,cost-stock);stock=w.Stock(depot,t.item);Check(w.ResearchPlanet(t.planet,t.id),"Unique technology can be purchased: "+t.id);Check(w.Stock(depot,t.item)==stock-cost&&w.PlanetLevel(t.planet,t.id)>0,"Research pays local stock and stores its own level: "+t.id);
        }
        Check(w.ProductionSpeed(1,"rose")>1&&w.ProductionSpeed(2,"carp")>1&&w.ProductionSpeed(3,"stone")>1,"Native technology modifies real production timing");Check(w.ProductionOutput(ArchipelagoWorld.Recipes.First(r=>r.item=="stone"),9)>36,"Mineral technology modifies actual output");
        var locked=new ClassicFarm();Check(!locked.Fleet.ResearchPlanet(1,"pollination")&&!locked.Fleet.LaunchUfo(1,0,-1,"water",1),"Locked planets and pre-spaceflight mothership reject actions");
        Check(ArchipelagoWorld.Contracts.Length==24&&ArchipelagoWorld.Contracts.Skip(8).All(c=>c.goals.Length>=3),"All twenty-four projects have explicit product objectives");Check(ArchipelagoWorld.Contracts.Skip(8).Any(c=>c.goals.Any(g=>g.island==-1)),"Post-colony projects use the orbital warehouse");
        var clone=new ClassicFarm{Data=JsonUtility.FromJson<ClassicSave>(JsonUtility.ToJson(f.Data))};Check(clone.Fleet.PlanetLevel(1,"pollination")==w.PlanetLevel(1,"pollination")&&clone.Fleet.Stock(-1,"water")==w.Stock(-1,"water"),"Saved specialist research and mothership inventory restore");
        Directory.CreateDirectory("InternalTestReports/0.13.2");File.WriteAllText("InternalTestReports/0.13.2/orbital.json",JsonUtility.ToJson(new Report{status="passed",checks=checks.Count,names=checks.ToArray()},true));Debug.Log("ORBITAL CHECKS PASSED / "+checks.Count);
    }
    [Serializable] public sealed class Report{public string status;public int checks;public string[] names;}
}
