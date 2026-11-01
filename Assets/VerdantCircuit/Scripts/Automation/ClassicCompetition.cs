using System;
using System.Linq;
using System.Collections.Generic;
namespace Verdant.Automation
{
    public sealed partial class ClassicController
    {
        sealed class Sample {public double time;public Dictionary<string,double> totals;}
        readonly Queue<Sample> productionSamples=new Queue<Sample>();readonly Dictionary<string,double> lastMinute=new Dictionary<string,double>();double sampleTime=-10;
        void UpdateProduction()
        {
            if(Farm.Data.time<sampleTime){productionSamples.Clear();sampleTime=-10;}
            if(Farm.Data.time-sampleTime>=1){sampleTime=Farm.Data.time;productionSamples.Enqueue(new Sample{time=sampleTime,totals=Farm.Data.harvested.ToDictionary(s=>s.id,s=>s.amount)});while(productionSamples.Count>1&&productionSamples.ElementAt(1).time<=sampleTime-60)productionSamples.Dequeue();var first=productionSamples.Peek();foreach(string item in ClassicFarm.Items){double total=Farm.Data.harvested.Find(s=>s.id==item)?.amount??0,prior;first.totals.TryGetValue(item,out prior);double amount=Math.Max(0,total-prior);lastMinute[item]=amount;var best=Farm.Data.personalBest.Find(s=>s.id==item);if(best==null)Farm.Data.personalBest.Add(new Stock(item,amount));else best.amount=Math.Max(best.amount,amount);}}
            foreach(var a in Achievements())if(a.Value&&!Farm.Data.achievements.Contains(a.Key)){Farm.Data.achievements.Add(a.Key);Log(Tr("Achievement unlocked: ","成就解锁：")+AchievementLabel(a.Key));}
        }
        static string AchievementLabel(string id){switch(id){case "first_hay":return Tr("First hay","第一捆干草");case "thousand":return Tr("A thousand harvests","千份收成");case "wood":return Tr("The woodland","林木生产");case "carrot":return Tr("Root production","根系生产");case "pumpkin":return Tr("A giant harvest","巨型丰收");case "power":return Tr("Solar reserve","能量储备");case "cactus":return Tr("Order in the field","田间有序");case "gold":return Tr("Treasure route","宝藏路线");case "bone":return Tr("Long-tail explorer","长尾探索者");case "parallel":return Tr("Parallel production","并行生产");case "field":return Tr("Open horizons","开阔天地");case "research":return Tr("Research complete","研究完成");case "race":return Tr("A recorded strategy","策略留名");default:return id;}}
    }
}
