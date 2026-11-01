using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
namespace Verdant.Automation
{
    [Serializable] public sealed class RaceRecord {public string category,program;public double seconds;public int seed;}
    public sealed partial class ClassicController
    {
        List<RaceRecord> raceRecords{get{return Farm.Data.records;}}ClassicSession race;string raceItem,raceProgram;double raceTarget;int raceSeed;
        void BuildRecords()
        {
            Paragraph(Tr("Resource totals and local achievements. Challenge records use fixed starts and remain on this computer.","资源累计与本地成就。竞速使用固定开局，成绩仅保存在本机。"),75);
            foreach(string item in ClassicFarm.Items){double minute;lastMinute.TryGetValue(item,out minute);Paragraph(ItemName(item)+" / "+Tr("Last 60s: ","最近60秒：")+minute.ToString("N0")+" / "+Tr("Best: ","最佳：")+(Farm.Data.personalBest.Find(s=>s.id==item)?.amount??0).ToString("N0"),45);}
            foreach(var a in Achievements())Paragraph((a.Value||Farm.Data.achievements.Contains(a.Key)?"✓ ":"○ ")+AchievementLabel(a.Key),45);
            foreach(var r in raceRecords)Paragraph(r.category+" / "+r.seconds.ToString("F3")+"s / "+r.program+" / seed "+r.seed,45);
            Row(Tr("Local competition","本地竞速"),Tr("Test a program against a fixed production target.","用固定生产目标测试程序。"),Tr("Choose","选择"),()=>Open("challenges"));
        }
        Dictionary<string,bool> Achievements()
        {
            var h=Farm.Data.harvested;Func<string,double> count=id=>h.Find(x=>x.id==id)?.amount??0;
            return new Dictionary<string,bool>{
                {"first_hay",count("hay")>=1},{"thousand",h.Sum(s=>s.amount)>=1000},
                {"wood",count("wood")>=1000},{"carrot",count("carrot")>=1000},
                {"pumpkin",count("pumpkin")>=1000},{"power",count("power")>=1000},
                {"cactus",count("cactus")>=1000},{"gold",count("gold")>=100},
                {"bone",Farm.Count("bone")>=100},{"parallel",Farm.Level("megafarm")>0},
                {"field",Farm.MaxSize>=32},{"research",ClassicFarm.Techs.All(t=>Farm.Level(t.id)>=t.max)},
                {"race",raceRecords.Count>0}};
        }
        void BuildChallenges()
        {
            Paragraph(Tr("The selected program is evaluated on a separate 8×8 farm with all research, seed 41 and supplied inputs. Reach the target and let all workers terminate. Fastest reset starts with one plot and no resources. Stop cancels the challenge.","使用选中程序，在独立的 8×8 农场、全部研究、种子 41 和充足种植材料下测试。达标后需让所有工作机结束。最快重置从一格、零物资开始。停止可取消竞速。"),125);
            foreach(string id in new[]{"hay","wood","carrot","pumpkin","power","cactus","gold","bone"}){string item=id;Row(ItemName(item),Tr("Produce 1000 new units, then terminate the program.","新增生产 1000 份，然后结束程序。"),Tr("Start","开始"),()=>StartRace(item,1000));}
            Row(Tr("Fastest reset","最快重置"),Tr("Start from zero and unlock local competition through code.","从零开始，通过程序解锁本地竞速研究。"),Tr("Start","开始"),()=>StartRace("reset",1));
        }
        void StartRace(string item,double target)
        {
            StoreEditor();if(race!=null){Log(Tr("A challenge is already running.","已有竞速正在运行。"));return;}var farm=new ClassicFarm(41);farm.Data.programs=Farm.Data.programs.Select(p=>new ClassicProgram(p.name,p.source)).ToList();
            farm.Data.allowLockedSyntax=true;if(item!="reset"){foreach(var t in ClassicFarm.Techs)farm.Data.unlocks.Add(new Stock(t.id,t.max));farm.Resize(8);foreach(string resource in ClassicFarm.Items)if(resource!=item)farm.Add(resource,1000000000);}
            race=new ClassicSession(farm){IsSimulation=true};raceProgram=Farm.Data.programs[Farm.Data.selectedProgram].name;raceItem=item;raceTarget=target;raceSeed=41;race.Start(Editor.text);Close();Log(Tr("Challenge started on an isolated farm.","竞速已在独立农场启动。"));
        }
        void TickRace()
        {
            if(race==null)return;for(int i=0;i<240&&race.Running;i++)race.FixedTick();
            if(race.elapsed>3600){race.Stop();Log(Tr("Challenge exceeded the one-hour game-time limit.","竞速超出一小时游戏时间限制。"));race=null;return;}
            if(race.Running)return;bool won=race.Error==""&&(raceItem=="reset"?race.Farm.Level("leaderboard")>0:(race.Farm.Data.harvested.Find(s=>s.id==raceItem)?.amount??0)>=raceTarget);
            if(won){var record=raceRecords.Find(r=>r.category==raceItem&&r.seed==raceSeed);if(record==null)raceRecords.Add(new RaceRecord{category=raceItem,program=raceProgram,seconds=race.elapsed,seed=raceSeed});else if(race.elapsed<record.seconds){record.seconds=race.elapsed;record.program=raceProgram;}Log(Tr("Challenge completed: ","竞速完成：")+race.elapsed.ToString("F3")+"s");NotifyChallengeCompleted(raceItem,race.elapsed);}else Log(Tr("Challenge ended before reaching its target. ","竞速结束，尚未达到目标。")+race.Error);race=null;
        }
    }
}
