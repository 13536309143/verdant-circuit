using System;

namespace Verdant.Automation
{
    // Quantities and production rules only: there is no elapsed-time research gate.
    public static class ClassicBalance
    {
        public const string Revision="one-hour-2026-11-01";
        public const double ResearchGrowth=2.65;
        public const double SpeedGrowth=1.9;
        public const double PowerSpeedBonus=1.6;
        public const int CactusChainCap=16;
        public const int TailScaleCap=64;
        public const string CompetitionRevision="mid-tier-resource-targets-v1";
        public const int CompetitionTier=3;

        public static double CompetitionTarget(string item)
        {
            switch(item){case "hay":return 4096;case "wood":return 6000;case "carrot":return 2048;case "pumpkin":return 12000;case "power":return 8000;case "cactus":return 20000;case "bone":return 30000;case "gold":return 4096;default:return 1;}
        }
        public static void ConfigureCompetition(ClassicFarm farm,string item,bool single=false)
        {
            farm.Data.allowLockedSyntax=true;
            if(item=="reset")return;
            foreach(var tech in ClassicFarm.Techs)farm.Data.unlocks.Add(new Stock(tech.id,Math.Min(CompetitionTier,tech.max)));
            farm.Resize(8);if(single)farm.Data.droneLimit=1;
            foreach(string resource in ClassicFarm.Items)if(resource!=item)farm.Add(resource,1000000000);
        }

        public static double CompanionMultiplier(int level){return level<=0?1:1.5+.5*level;}
        public static double CactusMultiplier(int count){return count*Math.Min(count,CactusChainCap);}
        public static double BoneYield(int length,int level){return Math.Floor(length*Math.Min(length,TailScaleCap)*(1+.25*Math.Max(0,level-1)));}
        public static double GrowthSeconds(string entity)
        {
            switch(entity){case "grass":return 3;case "bush":return 4;case "tree":return 6;case "carrot":return 7;case "pumpkin":return 11;case "sunflower":return 8.5;case "cactus":return 1.5;default:return double.PositiveInfinity;}
        }
    }
}
