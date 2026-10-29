using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
public static class VerdantFleetChecks
{
    static List<string> checks;static void Check(bool ok,string label){if(!ok)throw new Exception("FAIL: "+label);checks.Add(label);}
    static Verdant.VerdantFarm Farm(){var f=new Verdant.VerdantFarm(false,"verdant-fleet-verification.json");f.Resize(8);f.Data.contract=29;f.Data.coins=100000;f.Data.knowledge=100000;return f;}
    public static string Run()
    {
        checks=new List<string>();var f=Farm();Check(f.Data.droneCapacity==1&&f.Data.footprintTier==0&&f.Data.drones.Count==1,"Independent initial capacity and size tier");
        int coins=f.Data.coins;Check(!f.Fleet.Deploy("general",2,0)&&f.Data.coins==coins,"Locked capacity cannot be bypassed");
        f.Data.tech.Add("motor1");Check(f.Research("fleet2")&&f.Data.droneCapacity==2&&f.Data.footprintTier==0,"Capacity research does not unlock size");
        Check(f.Fleet.Deploy("general",2,0)&&f.Data.drones.Count==2,"Deploy second drone");coins=f.Data.coins;
        Check(!f.Fleet.Resize(1,2,2)&&f.Data.coins==coins,"Locked body upgrade has no cost");
        f.Data.tech.Add("water");Check(f.Research("body2")&&f.Data.footprintTier==1&&f.Data.droneCapacity==2,"Body research does not increase capacity");
        Check(f.Fleet.Resize(1,2,2)&&!f.Fleet.Fits(f.Fleet.Find(0),3,1),"Full 2x2 footprint reserves four plots");
        coins=f.Data.coins;Check(!f.Fleet.Resize(0,3,3)&&f.Data.coins==coins,"3x3 remains locked");
        f.Fleet.Select(0);f.Fleet.Enqueue(0,"move","",1,0);f.Fleet.Enqueue(1,"move","",1,0);
        for(int i=0;i<100;i++)f.Tick(.05f);
        var a=f.Fleet.Find(0);var b=f.Fleet.Find(1);Check(!(a.x>=b.x&&a.x<b.x+b.width&&a.y>=b.y&&a.y<b.y+b.height),"Queued contested destinations never overlap");
        Check(f.Count("collisions")==0,"Safe queue waits without collision fees");
        f.Fleet.CancelAll();f.Fleet.Select(0);a.x=1;a.y=0;f.Data.x=1;f.Data.y=0;b.x=2;b.y=0;
        coins=f.Data.coins;f.Act("move","East");f.Act("move","East");f.Act("move","East");
        Check(f.Data.coins==coins-40&&f.Count("collisions")==2,"Direct collisions charged at most twice per 10 seconds");
        Check(a.x==1&&b.x==2,"Collision does not teleport either drone");
        b.x=4;b.y=4;f.Data.droneCapacity=4;f.Data.tech.Add("drone_water");Check(f.Fleet.Deploy("irrigator",0,6),"Deploy specialized irrigator");
        var water=f.Data.drones[f.Data.drones.Count-1];coins=f.Data.coins;Check(!f.Fleet.Work(water.id,"plant","grain")&&f.Data.coins==coins,"Irrigator rejects planting without cost");
        f.Data.footprintTier=3;Check(f.Fleet.Resize(water.id,3,2),"3x2 airframe accepted at unlocked tier");
        for(int y=6;y<8;y++)for(int x=0;x<3;x++)f.Data.plots[y*8+x].moisture=0;
        f.Data.water=60;int count=f.Count("watered");f.Fleet.Work(water.id,"water");Check(f.Count("watered")==count+6&&Math.Abs(f.Data.water-54)<.01f,"Area irrigation bills six real cells");
        f.Data.fleetPaused=true;f.Fleet.Enqueue(1,"move","",6,4);int beforeX=b.x;for(int i=0;i<100;i++)f.Tick(.1f);Check(b.x==beforeX,"Fleet pause freezes queued movement");
        f.Save();var loaded=new Verdant.VerdantFarm(true,"verdant-fleet-verification.json");Check(loaded.Data.drones.Count==3&&loaded.Data.footprintTier==3&&loaded.Data.droneCapacity==4&&loaded.Data.fleetPaused,"Fleet capacity, dimensions and paused saved queues roundtrip");
        loaded.Fleet.CancelAll();Check(loaded.Data.drones.TrueForAll(u=>u.jobs.Count==0),"Stopping clears pending queues");
        f=Farm();f.Data.droneCapacity=2;f.Fleet.Deploy("general",2,0);for(int j=0;j<64;j++)f.Fleet.Enqueue(1,"move","",7,j%2==0?7:0);f.Data.coins=0;for(int i=0;i<700;i++)f.Tick(.1f);
        Check(f.Data.coins>=0&&f.Fleet.Find(1).maintenance<=30,"Unaffordable upkeep never accumulates unbounded debt");
        Check(f.Fleet.Find(1).jobs.Count>0&&f.Data.coins==0,"Unfunded active worker really pauses");f.Fleet.CancelAll();f.Fleet.Select(0);f.Data.x=1;f.Data.y=1;f.Fleet.Find(0).x=1;f.Fleet.Find(0).y=1;Check(f.Act("plant","grain"),"Zero-money recovery remains available");f.Tick(20);f.Act("harvest");
        var vm=new Verdant.FarmVM(f,new Verdant.FarmCompiler().Compile("print(drone_capacity())\nprint(footprint_tier())\nqueue_move(1, 5, 5)\nwhile drone_busy(1):\n    wait(0.2)"));
        for(int frame=0;frame<2000&&!vm.Finished;frame++){f.Tick(.05f);for(int budget=0;budget<160;budget++)if(vm.Step())break;}
        Check(f.Fleet.Find(1).x==5&&f.Fleet.Find(1).y==5,"New multi-argument VM queue interfaces run end to end");
        f=Farm();f.Data.tech.Add("fleet4");f.Data.tech.Add("body32");Check(f.Research("fleet8")&&f.Data.droneCapacity==8&&f.Data.footprintTier==0,"Eight-unit capacity unlock is independent");Check(f.Research("body3")&&f.Data.footprintTier==3,"3x3 tier unlock");
        Check(f.Fleet.Resize(0,3,3),"Unlocked 3x3 airframe fits");
        int[,] placements={{4,0,3,2},{0,4,2,2},{3,4,2,3},{6,4,2,2},{0,7,1,1},{2,7,1,1},{6,7,1,1}};
        for(int i=0;i<7;i++){Check(f.Fleet.Deploy("general",placements[i,0],placements[i,1]),"Deploy mixed-fleet worker "+(i+1));Check(f.Fleet.Resize(i+1,placements[i,2],placements[i,3]),"Fit mixed airframe "+(i+1));}
        var random=new System.Random(1029);for(int tick=0;tick<1000;tick++)
        {foreach(var u in f.Data.drones)if(u.jobs.Count==0)f.Fleet.Enqueue(u.id,"move","",random.Next(f.Data.size-u.width+1),random.Next(f.Data.size-u.height+1));f.Tick(.1f);
         for(int i=0;i<f.Data.drones.Count;i++)for(int j=i+1;j<f.Data.drones.Count;j++){var u=f.Data.drones[i];var v=f.Data.drones[j];if(u.x<v.x+v.width&&u.x+u.width>v.x&&u.y<v.y+v.height&&u.y+u.height>v.y)throw new Exception("Stress overlap");}}
        Check(f.Count("collisions")==0,"Eight mixed-size drones: 1000 safe scheduling ticks without overlap or collision fees");
        var legacy=JsonUtility.FromJson<Verdant.FarmSave>("{\"schema\":1,\"x\":2,\"y\":3}");Verdant.VerdantFleet.Ensure(legacy);
        Check(legacy.drones.Count==1&&legacy.drones[0].id==0&&legacy.drones[0].x==2&&legacy.droneCapacity==1&&legacy.footprintTier==0,"Legacy save missing fleet fields migrates at the existing cursor");
        f=Farm();f.Data.tech.Add("water");f.Data.tech.Add("drone_water");f.Data.droneCapacity=2;f.Fleet.Deploy("irrigator",2,0);f.Fleet.Enqueue(0,"water","",0,0);f.Fleet.Enqueue(1,"water","",2,0);f.Fleet.Tick(.01f);
        Check(f.Fleet.Find(1).cooldown<f.Fleet.Find(0).cooldown*.6f,"Specialists trade a narrow role for faster queued work");
        var runner=new Verdant.VerdantRunner(f);runner.Step("queue_move(1, 4, 0)");Check(runner.Paused&&f.Data.fleetPaused,"Single step leaves the fleet paused");runner.Stop();
        f=Farm();f.Data.droneCapacity=2;f.Fleet.Deploy("general",7,0);var laneRunner=new Verdant.VerdantRunner(f);laneRunner.Start(Verdant.VerdantContent.Templates[9]);for(int tick=0;tick<2000;tick++){f.Tick(.05f);laneRunner.Tick(.05f);}
        Check(f.Count("harvested")>50&&f.Count("collisions")==0&&laneRunner.Error=="","Built-in two-drone template produces grain without collisions");
        return JsonUtility.ToJson(new VerdantChecks.Report{passed=checks.Count,checks=checks.ToArray()},true);
    }
    public static void VerifyAll()
    {
        var init=LocalizationSettings.InitializationOperation;init.WaitForCompletion();LocalizationSettings.SelectedLocale=LocalizationSettings.AvailableLocales.GetLocale("en");Verdant.L10n.ResetCache();
        Directory.CreateDirectory("InternalTestReports/release-0.10.2");
        File.WriteAllText("InternalTestReports/release-0.10.2/fleet.json",Run());
        File.WriteAllText("InternalTestReports/release-0.10.2/rules.json",VerdantChecks.RunAll());
        File.WriteAllText("InternalTestReports/release-0.10.2/campaign.json",VerdantChecks.Playthrough());
        int entries=0;foreach(var col in UnityEditor.Localization.LocalizationEditorSettings.GetStringTableCollections())foreach(var key in col.SharedData.Entries)foreach(var t in col.StringTables){if(t.GetEntry(key.Id)==null||string.IsNullOrWhiteSpace(t.GetEntry(key.Id).Value))throw new Exception("Translation gap: "+key.Key);entries++;}
        if(entries==0)throw new Exception("No localization entries checked.");
        var en=LocalizationSettings.AvailableLocales.GetLocale("en");var zh=LocalizationSettings.AvailableLocales.GetLocale("zh-Hans");
        LocalizationSettings.SelectedLocale=en;Verdant.L10n.ResetCache();if(Verdant.L10n.T("机群")!="Fleet")throw new Exception("English lookup failed");
        LocalizationSettings.SelectedLocale=zh;Verdant.L10n.ResetCache();if(Verdant.L10n.T("机群")!="机群")throw new Exception("Chinese lookup failed");
        LocalizationSettings.SelectedLocale=en;Verdant.L10n.ResetCache();
        File.WriteAllText("InternalTestReports/release-0.10.2/localization.json","{\"checkedEntries\":"+entries+",\"gaps\":0}");
        Debug.Log("VERDANT 0.10.2 ALL CHECKS PASSED");
    }
}
