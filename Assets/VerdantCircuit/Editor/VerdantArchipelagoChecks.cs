using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Verdant.Automation;

public static class VerdantArchipelagoChecks
{
    static int checks;
    static void Check(bool ok,string description){if(!ok)throw new Exception("FLEET FAIL: "+description);checks++;UnityEngine.Debug.Log("FLEET CHECK / "+description);}
    public static void Probe()
    {
        string output=Path.GetFullPath(Environment.GetEnvironmentVariable("VERDANT_FLEET_OUTPUT")??"InternalTestReports/fleet-probe");Directory.CreateDirectory(output);
        var farm=new ClassicFarm();foreach(var tech in ClassicFarm.Techs)farm.Data.unlocks.Add(new Stock(tech.id,tech.max));farm.Add("wood",1000);farm.Add("carrot",1000);
        foreach(string file in ClassicFleetExample.Names)farm.Data.programs.Add(new ClassicProgram(file,ClassicFleetExample.Code(file)));
        var world=farm.Fleet;var session=new ClassicSession(farm);session.Start(ClassicFleetExample.Source);var wall=Stopwatch.StartNew();int previous=-1;long frames=0;double next=0;
        while(session.Running)
        {
            session.FixedTick();frames++;
            if(world.Data.order!=previous||world.Data.time>=next)
            {
                Check(world.Audit()=="",world.Audit());previous=world.Data.order;next=world.Data.time+900;File.WriteAllText(Path.Combine(output,"latest.json"),JsonUtility.ToJson(farm.Data,true));
                UnityEngine.Debug.Log("FLEET PROBE / order "+world.Data.order+" / seconds "+world.Data.time+" / wall "+wall.Elapsed.TotalSeconds);
            }
            if(world.Data.time>100000||wall.Elapsed.TotalSeconds>900)throw new Exception("Fleet probe exceeded budget; see latest.json");
        }
        File.WriteAllText(Path.Combine(output,"final-farm.json"),JsonUtility.ToJson(farm.Data,true));
        Check(session.Error=="",session.Error);Check(world.Complete,"Actual interpreter controller completes all 18 contracts");Check(world.Data.programFaults==0&&world.Data.peakPrograms>=3&&world.Data.messages>0,"Independent module programs and message calls execute without faults");Check(world.Data.nearMisses==0,"Automatic planning incurs no collisions or emergency fees");
        Check(world.Data.units.Count<=world.SlotLimit&&world.SlotLimit==8,"Fleet respects independently researched slot limit");Check(world.Data.sizeLevel==4,"All hull tiers are paid and unlocked");
        File.WriteAllText(Path.Combine(output,"report.json"),"{\"status\":\"passed\",\"fixtureOnly\":true,\"gameSeconds\":"+world.Data.time.ToString(System.Globalization.CultureInfo.InvariantCulture)+",\"orders\":18,\"frames\":"+frames+",\"wallSeconds\":"+wall.Elapsed.TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture)+"}");
        UnityEngine.Debug.Log("FLEET PROBE PASSED / "+world.Data.time+" seconds");
    }
    public static void Core()
    {
        var farm=new ClassicFarm();var world=farm.Fleet;
        Check(!world.Start()&&world.Build("hauler",1,1,0)<0,"Research and charter gate physical fleets");
        farm.Data.unlocks.Add(new Stock("carrots",1));farm.Add("wood",100);farm.Add("carrot",100);Check(world.Start()&&farm.Count("wood")==0&&farm.Count("carrot")==0,"Charter pays real resources exactly once");
        Check(world.Start()&&world.Data.credits==100,"Repeated charter does not grant more credit");
        Check(world.Build("hauler",2,2,0)<0&&world.Build("engineer",1,1,0)<0,"Hull size and specialism have separate gates");
        int pump=world.Build("irrigator",1,1,0),grow=world.Build("grower",1,1,0),haul=world.Build("hauler",1,1,0);
        Check(pump>0&&grow>0&&haul>0&&world.Build("grower",1,1,0)<0,"Build respects actual slot limit");
        Check(!world.Research("slots")&&!world.Research("size")&&!world.Research("engineer"),"Credit alone cannot bypass order discoveries");
        Check(!world.Work(haul,0,"grain",1)&&!world.Work(pump,0,"water",0)&&!world.Ship(haul,0,1,"water",1),"Roles, bounds and locked islands are validated");
        Check(world.Work(pump,0,"water",2)&&world.Work(grow,0,"grain",1),"Specialist work queues accept valid recipes");
        for(int i=0;i<1200;i++)world.Advance(.25);
        Check(world.Stock(0,"grain")==2&&world.Stock(0,"water")==7,"Recipe inputs are consumed and outputs conserved");
        farm.Data.unlocks.Add(new Stock("pumpkins",1));world.Data.order=3;world.Data.credits=500;Check(world.Research("size")&&world.Data.sizeLevel==1&&world.SlotLimit==3,"Hull purchase does not increase fleet count");
        Check(world.Research("slots")&&world.SlotLimit==4&&world.Data.sizeLevel==1,"Fleet purchase does not enlarge hulls");
        var large=new FleetUnit{island=0,x=1,y=1,width=3,height=3};var small=new FleetUnit{island=0,x=1,y=1};
        Check(!world.Fits(large,9,2,false)&&world.Fits(small,9,2,false)&&world.Fits(large,9,6,false),"Narrow passage rejects large complete footprints, broad crossing admits them");
        var route=world.Path(large,15,9,false);Check(route.Count>0&&route.All(p=>world.Fits(large,p%ArchipelagoWorld.Width,p/ArchipelagoWorld.Width,false)),"Every planned step fits the entire 3×3 body");
        double water=world.Stock(0,"water");Check(world.Ship(haul,0,1,"water",5),"Connected island shipment is accepted");
        bool noOverlap=true;
        for(int i=0;i<2400;i++)
        {
            world.Advance(.25);foreach(var unit in world.Data.units.Where(u=>u.flightTarget<0))noOverlap&=world.Fits(unit,unit.x,unit.y);
        }
        Check(noOverlap,"Atomic footprint reservations prevent overlap throughout the crossing");
        Check(world.Stock(0,"water")==water-5&&world.Stock(1,"water")==5&&world.Unit(haul).cargo==0,"Cross-island shipping conserves cargo across loading, flight and landing");
        world.Work(pump,1,"water",3);for(int i=0;i<1200;i++)world.Advance(.25);
        Check(world.Stock(1,"water")>=23,"Specialist physically rebases before remote work");
        string json=JsonUtility.ToJson(farm.Data);var resumed=new ClassicFarm{Data=JsonUtility.FromJson<ClassicSave>(json)};Check(JsonUtility.ToJson(resumed.Data)==json,"All island inventory, hulls and queues round-trip through save data");
        UnityEngine.Debug.Log("FLEET CORE PASSED / "+checks+" checks");
    }
}
