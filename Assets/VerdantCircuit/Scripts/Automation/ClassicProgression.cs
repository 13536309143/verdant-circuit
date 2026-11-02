using System;
using System.Collections.Generic;
using System.Linq;

namespace Verdant.Automation
{
    // Entries name the discovery required before buying each successive level.
    // Every requirement references a first level, so the level graph stays acyclic.
    public static class ClassicProgression
    {
        public static readonly Dictionary<string,string[]> TierDiscoveries=new Dictionary<string,string[]>
        {
            {"expand",new[]{"plant","trees","pumpkins","cactus","dinosaurs","spaceflight","flower_planet","mineral_planet","ocean_planet"}},
            {"speed",new[]{"trees","carrots","pumpkins","sunflowers","cactus","mazes","dinosaurs","megafarm"}},
            {"grass",new[]{"plant","trees","carrots","pumpkins","sunflowers","cactus","mazes","dinosaurs"}},
            {"trees",new[]{"","carrots","pumpkins","sunflowers","cactus","mazes","dinosaurs","megafarm"}},
            {"carrots",new[]{"","pumpkins","sunflowers","cactus","mazes","dinosaurs","megafarm","spaceflight"}},
            {"pumpkins",new[]{"","sunflowers","cactus","mazes","dinosaurs","megafarm","spaceflight","leaderboard"}},
            {"sunflowers",new[]{"","cactus","mazes","dinosaurs","megafarm","spaceflight","leaderboard","the_farmers_remains"}},
            {"cactus",new[]{"","mazes","dinosaurs","megafarm","spaceflight","leaderboard","the_farmers_remains","top_hat"}},
            {"mazes",new[]{"","dinosaurs","megafarm","spaceflight","leaderboard","the_farmers_remains","top_hat","top_hat"}},
            {"dinosaurs",new[]{"","megafarm","spaceflight","leaderboard","the_farmers_remains","top_hat","top_hat","top_hat"}},
            {"watering",new[]{"","pumpkins","sunflowers","cactus","mazes","dinosaurs"}},
            {"fertilizer",new[]{"","cactus","mazes","dinosaurs","megafarm","spaceflight"}},
            {"polyculture",new[]{"","pumpkins","sunflowers","cactus","mazes","dinosaurs"}},
            {"megafarm",new[]{"dinosaurs","dinosaurs","spaceflight","leaderboard","the_farmers_remains"}}
        };
        public static readonly Dictionary<string,Stock> ProductionGoals=new Dictionary<string,Stock>
        {
            {"trees",new Stock("hay",160)},
            {"carrots",new Stock("wood",600)},
            {"pumpkins",new Stock("carrot",1500)},
            {"sunflowers",new Stock("pumpkin",8000)},
            {"cactus",new Stock("solar_energy",6000)},
            {"mazes",new Stock("cactus",30000)},
            {"dinosaurs",new Stock("coins",30000)}
        };
        public static readonly Dictionary<string,string> CropDiscoveries=new Dictionary<string,string>
        { {"sunflowers","pumpkins"},{"cactus","sunflowers"},{"mazes","cactus"},{"dinosaurs","mazes"} };

        public static Dictionary<object,object> Requirements(string id,int nextLevel)
        {
            var tech=ClassicFarm.Techs.FirstOrDefault(t=>t.id==id);
            if(tech==null||nextLevel<1||nextLevel>tech.max)return null;
            var result=new Dictionary<object,object>();
            if(tech.prerequisite!="")result["unlock:"+tech.prerequisite]=1.0;
            string[] stages;
            if(TierDiscoveries.TryGetValue(id,out stages)&&nextLevel<=stages.Length&&stages[nextLevel-1]!="")result["unlock:"+stages[nextLevel-1]]=1.0;
            string discovery;Stock goal;
            if(nextLevel==1)
            {
                if(CropDiscoveries.TryGetValue(id,out discovery))result["unlock:"+discovery]=1.0;
                if(ProductionGoals.TryGetValue(id,out goal))result["produced:"+goal.id]=goal.amount;
            }
            if(id=="spaceflight"){result["unlock:import"]=1.0;result["unlock:dicts"]=1.0;result["unlock:watering"]=1.0;}
            return result;
        }
    }
}
