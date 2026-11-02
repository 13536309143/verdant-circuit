using System;
using System.Collections.Generic;
using System.Linq;

namespace Verdant.Automation
{
    [Serializable] public sealed class ControlBinding {public string scope,program,error="";}
    [Serializable] public sealed class DroneGroup {public string name;public List<int> members=new List<int>();}
    public sealed partial class ArchipelagoWorld
    {
        public const int Earth=0,Flowers=1,Minerals=3,Fish=2;
        public static readonly string[] NatureFunctions={"make_compost","prepare_fish_feed","pump_water","cultivate_rose","cultivate_tulip","cultivate_lavender","cultivate_lily","cultivate_orchid","cultivate_lotus","collect_honey","mine_stone","mine_copper_ore","mine_tin_ore","mine_silver_ore","mine_gold_ore","mine_platinum_ore","mine_rough_diamond","smelt_copper","smelt_tin","smelt_bronze","smelt_silver","smelt_gold","refine_platinum","cut_diamond","catch_carp","catch_trout","catch_salmon","catch_cod","catch_tuna","catch_koi","make_fishmeal"};
        public static readonly string[] NatureItems={"compost","fish_feed","water","rose","tulip","lavender","lily","orchid","lotus","honey","stone","copper_ore","tin_ore","silver_ore","gold_ore","platinum_ore","rough_diamond","copper","tin","bronze","silver","gold","platinum","diamond","carp","trout","salmon","cod","tuna","koi","fishmeal"};
        public static readonly string[] UnifiedApi={"world_islands","island_products","product_info","island_self","program_bind","program_run","program_stop","program_state","network_run","island_run","island_stop","control_state","group_create","group_units","group_run","group_stop"};
        static FleetRecipe[] NatureRecipes(){return new[]{
            new FleetRecipe("compost",0,4,18,0,new[]{"drone"},"hay",4,"carrot",2),
            new FleetRecipe("fish_feed",0,4,12,0,new[]{"drone"},"carrot",1,"pumpkin",1),
            new FleetRecipe("water",0,4,8,0,new[]{"drone"}),
            new FleetRecipe("rose",1,3,24,0,new[]{"bee"},"water",2,"compost",1),
            new FleetRecipe("tulip",1,4,30,0,new[]{"bee"},"water",2,"compost",1),
            new FleetRecipe("lavender",1,3,28,0,new[]{"bee"},"water",1,"compost",1),
            new FleetRecipe("lily",1,2,40,0,new[]{"bee"},"water",3,"fertilizer",1),
            new FleetRecipe("orchid",1,2,65,0,new[]{"bee"},"water",2,"silver",1,"lavender",1),
            new FleetRecipe("lotus",1,2,70,0,new[]{"bee"},"water",4,"fishmeal",1),
            new FleetRecipe("honey",1,3,20,0,new[]{"bee"},"rose",1,"water",1),
            new FleetRecipe("stone",3,4,18,0,new[]{"mining_drone"}),
            new FleetRecipe("copper_ore",3,3,22,0,new[]{"mining_drone"},"solar_energy",1),
            new FleetRecipe("tin_ore",3,3,24,0,new[]{"mining_drone"},"solar_energy",1),
            new FleetRecipe("silver_ore",3,2,32,0,new[]{"mining_drone"},"solar_energy",2),
            new FleetRecipe("gold_ore",3,2,42,0,new[]{"mining_drone"},"solar_energy",3),
            new FleetRecipe("platinum_ore",3,1,54,0,new[]{"mining_drone"},"solar_energy",4),
            new FleetRecipe("rough_diamond",3,1,65,0,new[]{"mining_drone"},"solar_energy",5),
            new FleetRecipe("copper",3,3,24,0,new[]{"mining_drone"},"copper_ore",3,"solar_energy",1),
            new FleetRecipe("tin",3,3,24,0,new[]{"mining_drone"},"tin_ore",3,"solar_energy",1),
            new FleetRecipe("bronze",3,4,32,0,new[]{"mining_drone"},"copper",3,"tin",1,"solar_energy",1),
            new FleetRecipe("silver",3,2,40,0,new[]{"mining_drone"},"silver_ore",2,"solar_energy",2),
            new FleetRecipe("gold",3,2,50,0,new[]{"mining_drone"},"gold_ore",2,"solar_energy",3),
            new FleetRecipe("platinum",3,1,70,0,new[]{"mining_drone"},"platinum_ore",1,"rose",1,"solar_energy",4),
            new FleetRecipe("diamond",3,1,90,0,new[]{"mining_drone"},"rough_diamond",1,"orchid",1,"solar_energy",5),
            new FleetRecipe("carp",2,3,24,0,new[]{"boat"},"fish_feed",1),
            new FleetRecipe("trout",2,3,32,0,new[]{"boat"},"fish_feed",1),
            new FleetRecipe("salmon",2,2,45,0,new[]{"boat"},"fish_feed",2),
            new FleetRecipe("cod",2,3,48,0,new[]{"boat"},"fish_feed",2),
            new FleetRecipe("tuna",2,2,70,0,new[]{"boat"},"fish_feed",3),
            new FleetRecipe("koi",2,1,90,0,new[]{"boat"},"lotus",1,"fish_feed",2),
            new FleetRecipe("fishmeal",2,3,18,0,new[]{"boat"},"carp",2)};}
        static double NaturePrice(string item){if(item=="diamond")return 8;if(item=="platinum")return 5;if(item=="gold")return 3;if(item=="silver"||item=="orchid"||item=="koi")return 1.5;if(item=="stone")return .05;return .2;}
        void MigrateNetwork(){if(Data.mothership==null)Data.mothership=new System.Collections.Generic.List<Stock>();if(Data.planetResearch==null)Data.planetResearch=new System.Collections.Generic.List<Stock>();if(Data.networkSchema<3)foreach(var u in Data.ufos){u.legTarget=u.phase==3?u.destination:u.source;}Data.networkSchema=3;Data.roles=Roles.ToList();Data.started=farm.Level("spaceflight")>0;}
        string AuditOffworld()
        {
            foreach(string item in Goods)
            {
                double made=Data.islands.Sum(i=>Get(i.made,item));
                double stock=Get(Data.mothership,item)+Data.islands.Where(i=>i.island!=0).Sum(i=>Get(i.items,item));
                double held=Data.ufos.Where(u=>u.item==item).Sum(u=>u.reserved);
                double cargo=Data.ufos.Where(u=>u.item==item).Sum(u=>u.cargo);
                double net=made-Get(Data.consumed,item)-Get(Data.submitted,item)-Get(Data.exported,item)+Get(Data.earthSent,item)-Get(Data.earthReceived,item);
                if(Math.Abs(net-stock-held-cargo)>.0001)return "Cargo ledger mismatch: "+item;
            }
            return "";
        }
        public int EnsureEarthDrone(){return EnsurePlanetWorker(0);}
        public object Islands(){return Enumerable.Range(0,Definitions.Length).Select(i=>(object)Dict("id",(double)i,"key",Definitions[i].id,"name",Definitions[i].en,"unlocked",IsUnlocked(i),"research",i==0?"":IslandResearch[i],"program",Data.controls.FirstOrDefault(c=>c.scope=="island:"+i)?.program??"")).ToList();}
        public object ProductInfo(string item)
        {
            var recipe=Recipes.FirstOrDefault(r=>r.item==item);if(recipe==null)return null;
            return Dict("item",item,"planet",(double)recipe.island,"available",IsUnlocked(recipe.island)&&AvailableRecipe(recipe),"island",(double)recipe.island,"unlocked",IsUnlocked(recipe.island),"function",Array.IndexOf(NatureItems,item)>=0?NatureFunctions[Array.IndexOf(NatureItems,item)]:"fleet_work","amount",(double)recipe.amount,"seconds",recipe.seconds,"roles",recipe.roles.Select(x=>(object)x).ToList(),"inputs",recipe.inputs.ToDictionary(p=>(object)p.Key,p=>(object)(double)p.Value),"price",ExportPrices[item]);
        }
        public bool ProgramsPaused;
        public bool BindProgram(int id,string file)
        {
            var unit=Unit(id);var program=farm.Data.programs.FirstOrDefault(p=>ClassicProgramNames.Same(p.name,file));if(unit==null||program==null)return false;
            if(ProgramRunning(id))StopProgram(id);unit.program=program.name;unit.codeError="";return true;
        }
        public bool CreateGroup(string name,List<int> members)
        {
            if(farm.Level("spaceflight")==0||string.IsNullOrWhiteSpace(name)||name.Length>80||members.Count<1||members.Count>SlotLimit||members.Any(id=>Unit(id)==null)||members.Distinct().Count()!=members.Count||members.Select(id=>Unit(id).island).Distinct().Count()!=1)return false;
            var group=Data.groups.FirstOrDefault(g=>g.name==name);if(group==null){if(Data.groups.Count>=32)return false;group=new DroneGroup{name=name};Data.groups.Add(group);}group.members=members;return true;
        }
        public object UnifiedInvoke(string name,object[] a)
        {
            int function=Array.IndexOf(NatureFunctions,name);
            if(function>=0){if(a.Length<1||a.Length>2)throw new Exception(name+" expects drone_id and optional batches.");var recipe=Recipes.First(r=>r.item==NatureItems[function]);return Work(N(a[0]),recipe.island,recipe.item,a.Length==2?N(a[1]):1);}
            switch(name)
            {
                case "world_islands":Call.Require(a,0);return Islands();
                case "island_products":Call.Require(a,1);if(!IsUnlocked(N(a[0])))return new List<object>();return Recipes.Where(r=>r.island==N(a[0])).Select(r=>(object)r.item).Distinct().ToList();
                case "product_info":Call.Require(a,1);return ProductInfo(S(a[0]));
                case "island_self":Call.Require(a,0);return -1.0;
                case "program_bind":Call.Require(a,2);return BindProgram(N(a[0]),S(a[1]));
                case "program_run":Call.Require(a,1);return RunProgram(N(a[0]),Unit(N(a[0]))?.program??"");
                case "program_stop":Call.Require(a,1);return StopProgram(N(a[0]));
                case "program_state":Call.Require(a,1);var unit=Unit(N(a[0]));return unit==null?null:Dict("program",unit.program,"running",ProgramRunning(unit.id),"error",unit.codeError,"result",Result(unit.id));
                case "network_run":Call.Require(a,1);return RunControl("network",S(a[0]));
                case "island_run":Call.Require(a,2);return RunControl("island:"+N(a[0]),S(a[1]));
                case "island_stop":Call.Require(a,1);return StopControl("island:"+N(a[0]));
                case "control_state":Call.Require(a,0);return Data.controls.Select(c=>(object)Dict("scope",c.scope=="network"?"universe":c.scope.Replace("island:","planet:"),"program",c.program,"running",ControlRunning(c.scope),"error",c.error)).ToList();
                case "group_create":Call.Require(a,2);var ids=a[1] as List<object>;if(ids==null)throw new Exception("group_create needs a list of drone IDs.");return CreateGroup(S(a[0]),ids.Select(N).ToList());
                case "group_units":Call.Require(a,1);return (Data.groups.FirstOrDefault(g=>g.name==S(a[0]))?.members??new List<int>()).Select(i=>(object)(double)i).ToList();
                case "group_run":Call.Require(a,2);var group=Data.groups.FirstOrDefault(g=>g.name==S(a[0]));if(group==null)return false;bool ok=true;foreach(int id in group.members)ok=RunProgram(id,S(a[1]))&&ok;return ok;
                case "group_stop":Call.Require(a,1);var stopped=Data.groups.FirstOrDefault(g=>g.name==S(a[0]));if(stopped==null)return false;foreach(int id in stopped.members)StopProgram(id);return true;
                default:throw new Exception("Unknown world function: "+name);
            }
        }
    }
}
