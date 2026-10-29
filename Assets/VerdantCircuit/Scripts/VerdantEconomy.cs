using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verdant
{
    public sealed class Recipe
    {
        public string id,a,b;string rawName,rawNote;public string name{get{return L10n.T(rawName);}}public string note{get{return L10n.T(rawNote);}}public int amountA,amountB,energy,knowledge;
        public Recipe(string id,string name,string note,string a,int countA,string b,int countB,int energy,int knowledge)
        {this.id=id;this.rawName=name;this.rawNote=note;this.a=a;amountA=countA;this.b=b;amountB=countB;this.energy=energy;this.knowledge=knowledge;}
    }
    public sealed class SupplyOrder
    {public string title,a,b;public int amountA,amountB,coins,knowledge,slot;}
    public static class VerdantEconomy
    {
        public static readonly Recipe[] Recipes={
            new Recipe("ricebox",L10n.T("丰收米箱"),L10n.T("把基础作物加工成易于运输的补给。"),"grain",30,"radish",8,0,12),
            new Recipe("linen",L10n.T("蓝纹织物"),L10n.T("纤维与茶园包装所需的耐用织物。"),"flax",24,"grain",10,0,18),
            new Recipe("berryjam",L10n.T("晨露果酱"),L10n.T("蓝莓与谷物糖浆，适合沿岸商队。"),"berry",25,"grain",15,0,20),
            new Recipe("teablend",L10n.T("日光茶礼"),L10n.T("茶叶与向日葵花瓣的共生配方。"),"tea",20,"sunflower",12,0,28),
            new Recipe("starcell",L10n.T("星辉电池"),L10n.T("把星辉花的研究价值转化为远航能源。"),"starbloom",18,"flax",15,12,45)
        };
        public static bool Available(VerdantFarm f,Recipe r){return f.Chapter>=2&&f.Unlocked(r.a)&&f.Unlocked(r.b);}
        public static string Name(string id){var crop=VerdantContent.Crop(id);if(crop!=null)return crop.name;var r=Array.Find(Recipes,x=>x.id==id);return r==null?id:r.name;}
        public static int Value(string id){var c=VerdantContent.Crop(id);if(c!=null)return c.value;var r=Array.Find(Recipes,x=>x.id==id);return r==null?0:(r.amountA*VerdantContent.Crop(r.a).value+r.amountB*VerdantContent.Crop(r.b).value)/2;}
        public static bool Craft(VerdantFarm f,Recipe r)
        {
            if(f.InChallenge){f.Say(L10n.T("加工工坊只在主线农场开放"));return false;}
            if(!Available(f,r)){f.Say(L10n.T("第三章及对应作物研究完成后解锁"));return false;}
            if(f.Count("stock:"+r.a)<r.amountA||f.Count("stock:"+r.b)<r.amountB||f.Data.energy<r.energy){f.Say(L10n.T("配方所需作物或光能不足"));return false;}
            f.Add("stock:"+r.a,-r.amountA);f.Add("stock:"+r.b,-r.amountB);f.Data.energy-=r.energy;f.Add("stock:"+r.id);f.Add("crafted");f.Data.knowledge+=r.knowledge;
            f.Say(L10n.T("加工完成：")+r.name+L10n.T(" +1 / 研究 +")+r.knowledge);f.Notify();f.Save();return true;
        }
        public static SupplyOrder Order(VerdantFarm f,int slot)
        {
            var options=new List<string>();foreach(var c in VerdantContent.Crops)if(f.Unlocked(c.id))options.Add(c.id);
            foreach(var r in Recipes)if(Available(f,r))options.Add(r.id);
            int sequence=f.Count("order:"+slot),index=(sequence*7+slot*3)%options.Count;
            string a=options[index],b=options[(index+1+slot)%options.Count];if(a==b)b=options[(index+1)%options.Count];
            bool processedA=VerdantContent.Crop(a)==null,processedB=VerdantContent.Crop(b)==null;
            int amountA=processedA?1+sequence%3:20+(sequence%4)*10;int amountB=processedB?1:10+slot*5;
            return new SupplyOrder{title=L10n.F("{0} · 第 {1} 单",new[]{L10n.T("灯塔补给站"),L10n.T("沿岸商队"),L10n.T("山间研究所")}[slot],sequence+1),a=a,b=b,amountA=amountA,amountB=amountB,coins=Mathf.CeilToInt((Value(a)*amountA+Value(b)*amountB)*0.35f),knowledge=12+sequence%10*3+f.Chapter*5,slot=slot};
        }
        public static bool Deliver(VerdantFarm f,int slot)
        {
            if(f.InChallenge||f.Chapter<1){f.Say(L10n.T("供给订单在第二章开放"));return false;}var o=Order(f,slot);
            if(f.Count("stock:"+o.a)<o.amountA||f.Count("stock:"+o.b)<o.amountB){f.Say(L10n.T("库存不足，先安排相应作物或加工配方"));return false;}
            f.Add("stock:"+o.a,-o.amountA);f.Add("stock:"+o.b,-o.amountB);f.Data.coins+=o.coins;f.Data.knowledge+=o.knowledge;f.Add("orders");f.Add("order:"+slot);
            f.Say(L10n.T("供给完成：")+o.title+L10n.T(" / 金币 +")+o.coins);f.Notify();f.Save();return true;
        }
    }
}
