using System;
using System.Collections.Generic;
using System.Linq;
namespace Verdant.Automation
{
    public sealed partial class ArchipelagoWorld
    {
        public static readonly Dictionary<string,double> ExportPrices;
        static ArchipelagoWorld(){ExportPrices=MakeExportPrices();}
        static Dictionary<string,double> MakeExportPrices(){return Goods.ToDictionary(v=>v,v=>NaturePrice(v));}
        public double Sell(int island,string item,int amount)
        {
            if(!Data.started||!IsUnlocked(island)||!ExportPrices.ContainsKey(item)||amount<1||amount>1000000||Stock(island,item)<amount)return 0;
            double income=Math.Round(amount*ExportPrices[item],2,MidpointRounding.AwayFromZero);Add(island,item,-amount);Put(Data.exported,item,amount);Data.saleIncome=Math.Round(Data.saleIncome+income,2,MidpointRounding.AwayFromZero);Data.credits=Math.Round(Data.credits+income,2,MidpointRounding.AwayFromZero);return income;
        }
    }
}
