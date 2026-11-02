using System;
using System.Linq;
using System.Collections.Generic;
namespace Verdant.Automation
{
    public sealed class PlanetTechnology
    {
        public readonly int planet,max,cost;public readonly string id,en,zh,item,parent,category,effectEn,effectZh;
        public PlanetTechnology(int p,string id,string en,string zh,string item,int cost,string category,string effectEn,string effectZh,string parent="",int max=5)
        {planet=p;this.id=id;this.en=en;this.zh=zh;this.item=item;this.cost=cost;this.category=category;this.effectEn=effectEn;this.effectZh=effectZh;this.parent=parent;this.max=max;}
    }
    public sealed partial class ArchipelagoWorld
    {
        public static readonly PlanetTechnology[] PlanetTechnologies={
            new PlanetTechnology(-1,"navigation","Orbital navigation","轨道导航","bronze",24,"transport","UFO travel speed +15% per level.","每级UFO航行速度增加15%。"),
            new PlanetTechnology(-1,"docking","Docking automation","自动对接","silver",20,"automation","Docking approach time decreases with each level.","每级缩短母舰进离港对接时间。","navigation"),
            new PlanetTechnology(0,"ecology","Soil ecology","土壤生态","hay",120,"production","Original crop growth speed +12% per level.","每级原有作物生长速度增加12%。"),
            new PlanetTechnology(0,"retention","Water retention","保水层","wood",100,"ecology","Water evaporates more slowly; no automatic watering.","每级减少水分蒸发；不会自动浇水。","ecology"),
            new PlanetTechnology(1,"pollination","Pollination corridors","授粉走廊","rose",60,"production","Flower cultivation speed +18% per level.","每级花卉培育速度增加18%。"),
            new PlanetTechnology(1,"apiculture","Apiary networks","蜂巢网络","honey",50,"ecology","Honey yield +15% per level.","每级蜂蜜产量增加15%。","pollination"),
            new PlanetTechnology(1,"botany","Selective botany","定向育种","orchid",25,"production","Flower yield +12% per level.","每级花卉产量增加12%。","apiculture"),
            new PlanetTechnology(2,"sonar","Shoal sonar","鱼群声呐","carp",60,"automation","Fish catch speed +18% per level.","每级捕捞速度增加18%。"),
            new PlanetTechnology(2,"schooling","Sustainable schools","鱼群生态","salmon",45,"ecology","Fish catch yield +12% per level.","每级捕捞产量增加12%。","sonar"),
            new PlanetTechnology(2,"processing","Marine processing","海洋加工","fishmeal",50,"production","Fishmeal processing speed +20% per level.","每级鱼粉加工速度增加20%。","schooling"),
            new PlanetTechnology(3,"drilling","Adaptive drills","自适应钻探","stone",100,"automation","Ore mining speed +18% per level.","每级矿石开采速度增加18%。"),
            new PlanetTechnology(3,"refining","Precision refining","精密冶炼","bronze",60,"production","Smelting and cutting speed +16% per level.","每级冶炼与切割速度增加16%。","drilling"),
            new PlanetTechnology(3,"metallurgy","Materials recovery","材料回收","platinum",20,"ecology","Native mineral outputs +12% per level.","每级矿物产量增加12%。","refining")};
        public int PlanetLevel(int p,string id){return (int)Get(Data.planetResearch,p+":"+id);}
        public int TechnologyCost(PlanetTechnology t){return (int)Math.Ceiling(t.cost*Math.Pow(2,PlanetLevel(t.planet,t.id)));}
        public bool ResearchPlanet(int p,string id)
        {
            var t=PlanetTechnologies.FirstOrDefault(v=>v.planet==p&&v.id==id);
            if(t==null||!DepotUnlocked(p)||PlanetLevel(p,id)>=t.max||t.parent!=""&&PlanetLevel(p,t.parent)==0)return false;
            int depot=p==-1?0:p,cost=TechnologyCost(t);if(Stock(depot,t.item)<cost)return false;
            Add(depot,t.item,-cost);Put(Data.consumed,t.item,cost);Put(Data.planetResearch,p+":"+id,1);return true;
        }
        public object Technologies(int p)
        {return PlanetTechnologies.Where(t=>t.planet==p).Select(t=>(object)Dict("id",t.id,"planet",(double)p,"name",t.en,"level",(double)PlanetLevel(p,t.id),"max",(double)t.max,"item",t.item,"cost",(double)TechnologyCost(t),"parent",t.parent,"category",t.category,"effect",t.effectEn)).ToList();}
        public double ProductionSpeed(int p,string item)
        {return p==1?1+.18*PlanetLevel(p,"pollination"):p==2?1+(item=="fishmeal"?.20*PlanetLevel(p,"processing"):.18*PlanetLevel(p,"sonar")):p==3?1+(item=="stone"||item.EndsWith("_ore")||item=="rough_diamond"?.18*PlanetLevel(p,"drilling"):.16*PlanetLevel(p,"refining")):1;}
        public int ProductionOutput(FleetRecipe recipe,int area)
        {int p=recipe.island;double bonus=p==1?(recipe.item=="honey"?.15*PlanetLevel(p,"apiculture"):.12*PlanetLevel(p,"botany")):p==2&&recipe.item!="fishmeal"?.12*PlanetLevel(p,"schooling"):p==3?.12*PlanetLevel(p,"metallurgy"):0;return (int)Math.Floor(recipe.amount*area*(1+bonus));}
    }
}
