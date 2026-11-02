using System;
using System.Collections.Generic;
using System.Linq;

namespace Verdant.Automation
{
    [Serializable] public sealed class FleetJob
    {public int workX=-1,workY=-1;public string kind,item;public int target,remaining,reserved,source=-1,phase;public double timer;}
    [Serializable] public sealed class FleetUnit
    {
        public bool transient,freeStarter;public int cropX,cropY;public string hat="straw";public bool manual,stopAfterLanding;public string program="",codeError="";public int id,island,x,y,width=1,height=1,parkX,parkY;public string role="drone",status="idle",cargoItem="";public int cargo,cargoSource;
        public double delay,blocked,work;public List<FleetJob> jobs=new List<FleetJob>();public List<int> hops=new List<int>();public int flightTarget=-1;public double flight;
    }
    [Serializable] public sealed class IslandStock
    {public int island,width=1,height=1,expansion,slotTier,hullTier;public List<Stock> items=new List<Stock>(),made=new List<Stock>(),delivered=new List<Stock>();}
    [Serializable] public sealed class ArchipelagoSave
    {
        public List<Stock> earthSent=new List<Stock>(),earthReceived=new List<Stock>();public int networkSchema;public List<ControlBinding> controls=new List<ControlBinding>();public List<DroneGroup> groups=new List<DroneGroup>();public List<Stock> earthOpening=new List<Stock>();public bool started;public int order,slotLevel,sizeLevel,nextId=1,turn;public double time,accumulator,credits=100,fees,totalDistance,orderStarted,buildSpent,researchSpent,refunds,saleIncome;public List<Stock> consumed=new List<Stock>(),submitted=new List<Stock>(),exported=new List<Stock>();
        public List<Stock> mothership=new List<Stock>(),planetResearch=new List<Stock>();public int hubVisits;
        public List<UfoVessel> ufos=new List<UfoVessel>();public int nextUfoId=1,ufoCargoTier,ufoSlots;public List<string> usedRoles=new List<string>();public List<string> roles=new List<string>{"drone","bee","boat","mining_drone"};public List<FleetUnit> units=new List<FleetUnit>();
        public List<IslandStock> islands=new List<IslandStock>();public int programFaults,programRuns,messages,peakPrograms,orderPeakHull,orderPeakPrograms,orderPeakFlights;public int shipments,nearMisses,completedJobs;public List<double> orderTimes=new List<double>();
    }
    public sealed class IslandDefinition
    {public string id,en,zh;public int unlock;public IslandDefinition(string id,string en,string zh,int unlock){this.id=id;this.en=en;this.zh=zh;this.unlock=unlock;}}
    public sealed class FleetRecipe
    {
        public string item;public int island,amount,x,y;public double seconds;public string[] roles;public Dictionary<string,int> inputs;
        public FleetRecipe(string item,int island,int amount,double seconds,int y,string[] roles,params object[] inputs)
        {this.item=item;this.island=island;this.amount=amount;this.seconds=seconds;this.roles=roles;x=15;this.y=y;this.inputs=new Dictionary<string,int>();for(int i=0;i<inputs.Length;i+=2)this.inputs[(string)inputs[i]]=Convert.ToInt32(inputs[i+1]);}
    }
    public sealed class OrderGoal
    {public string item;public int island,amount;public OrderGoal(string item,int island,int amount){this.item=item;this.island=island;this.amount=amount;}}
    public sealed class FleetContract
    {
        public string en,zh,lessonEn,lessonZh;public OrderGoal[] goals;public int reward;public FleetContract(string en,string zh,string lessonEn,string lessonZh,int reward,params OrderGoal[] goals){this.en=en;this.zh=zh;this.lessonEn=lessonEn;this.lessonZh=lessonZh;this.reward=reward;this.goals=goals;}
    }
    public sealed partial class ArchipelagoWorld
    {
        public const int Width=360,Height=128;
        public static readonly string[] Goods=ClassicFarm.Items.Concat(new[]{"compost","fish_feed","honey","rose","tulip","lavender","lily","orchid","lotus","stone","copper_ore","tin_ore","silver_ore","gold_ore","platinum_ore","rough_diamond","copper","tin","bronze","silver","gold","platinum","diamond","carp","trout","salmon","cod","tuna","koi","fishmeal"}).Distinct().ToArray();
        public static readonly string[] Roles={"drone","bee","boat","mining_drone"};
        public static readonly int[] RoleOrders={0,0,0,0};
        public static readonly int[,] Sizes={{1,1},{2,1},{2,2},{3,2},{3,3}};
        public static readonly int[] SizeOrders={0,0,0,0,0};
        public static readonly IslandDefinition[] Definitions={new IslandDefinition("earth","Earth","地球",0),new IslandDefinition("floralia","Floralia · Flower Planet","芳华·花卉星球",0),new IslandDefinition("pelagia","Pelagia · Ocean Planet","沧澜·海洋星球",0),new IslandDefinition("aurelia","Aurelia · Mineral Planet","鎏金·矿物星球",0)};
        public static readonly int[,] Links={{0,1},{0,2},{0,3},{1,2},{1,3},{2,3}};
        public static readonly string[] IslandResearch={"","flower_planet","ocean_planet","mineral_planet"};
        public static readonly FleetRecipe[] Recipes=NatureRecipes();
        static OrderGoal G(string item,int planet,int amount){return new OrderGoal(item,planet,amount);}
        public static readonly FleetContract[] Contracts={
            new FleetContract("Earth prepares for orbit","地球的轨道准备","Keep original crops available while building transport supplies.","保留原有农田供应，准备星际物资。",80,G("wood",0,100),G("compost",0,30)),
            new FleetContract("First botanical colony","第一座花卉殖民地","Ship water and compost by UFO; let bees establish flowers.","用UFO送水和堆肥，让小蜜蜂建立花田。",100,G("rose",1,30),G("tulip",1,20)),
            new FleetContract("The mineral ladder","矿物阶梯","Mine copper and tin ores, then smelt true bronze.","采集铜矿与锡矿，冶炼真正的青铜。",140,G("stone",3,60),G("bronze",3,30)),
            new FleetContract("Blue-water fleet","蔚蓝船队","Send fish feed and coordinate local fishing boats.","运送鱼饵，编组本地渔船。",140,G("carp",2,30),G("trout",2,30),G("salmon",2,20)),
            new FleetContract("Pollination network","授粉网络","Use the six-flower chain and collect honey.","建立六类花卉生产链并收集蜂蜜。",200,G("lavender",1,40),G("lily",1,30),G("honey",1,20)),
            new FleetContract("Precious returns","贵金属回航","UFOs return processed silver and gold, not unrefined ore.","UFO运回冶炼后的白银与黄金，而非原矿。",250,G("silver",0,30),G("gold",0,15)),
            new FleetContract("Across four worlds","四星联动","Transfer fisheries inputs to flowers and botanical inputs to refineries.","将渔业原料送往花田，将花卉原料送往精炼厂。",300,G("lotus",1,20),G("platinum",3,10),G("tuna",2,20)),
            new FleetContract("The living universe","生生不息的宇宙","Complete independent swarms and interplanetary product deliveries.","用独立集群完成星际产物联合交付。",500,G("diamond",0,12),G("orchid",0,30),G("koi",0,20),G("honey",0,40)),
            new FleetContract("The orbital pantry","轨道补给站","Stock the mothership warehouse using actual UFO arrivals.","通过UFO实际到港，建立母舰补给仓库。",600,G("water",-1,45),G("compost",-1,30),G("fish_feed",-1,30),G("solar_energy",-1,45)),
            new FleetContract("Six gardens","六色花园","Maintain six flower supply chains and send the full collection to the mothership.","维持六种花卉供应链，将完整花卉组合送入母舰。",700,G("rose",-1,35),G("tulip",-1,35),G("lavender",-1,25),G("lily",-1,20),G("orchid",-1,10),G("lotus",-1,10)),
            new FleetContract("The foundry ring","环轨铸造带","Deliver smelted metals rather than ores; use larger mining hulls.","交付精炼金属，使用大型采矿机体组织开采与冶炼。",800,G("copper",-1,60),G("tin",-1,30),G("bronze",-1,45),G("silver",-1,25)),
            new FleetContract("Open ocean survey","远洋科考","Coordinate fishing routes for every common shoal.","协调多条渔船路线，覆盖每种常见鱼群。",800,G("carp",-1,45),G("trout",-1,35),G("salmon",-1,25),G("cod",-1,35),G("tuna",-1,20)),
            new FleetContract("Living soil","活土计划","Flower honey and marine fishmeal support Earth's ecological programme.","用花卉蜂蜜与海洋鱼粉支持地球生态工程。",900,G("honey",0,60),G("fishmeal",0,75),G("compost",0,120)),
            new FleetContract("Precision observatory","精密天文台","Combine bullion, platinum and botanical optics in orbit.","在轨道联合交付金锭、铂金及兰花材料。",1000,G("gold",-1,35),G("platinum",-1,25),G("orchid",-1,35)),
            new FleetContract("Ocean restoration","海洋修复工程","Produce lotus and koi through both marine and botanical logistics.","通过渔业与花卉物流联合生产莲花及锦鲤。",1100,G("lotus",2,60),G("koi",-1,25),G("fishmeal",1,90)),
            new FleetContract("Renewable infrastructure","可再生基础设施","Supply orbital infrastructure through four-world freight.","用四星货运供应轨道基础设施。",1200,G("solar_energy",-1,450),G("bronze",-1,120),G("silver",-1,75),G("honey",-1,75)),
            new FleetContract("Crystal telemetry","晶体遥测阵列","Diamonds require mining, flowers, silver and cutting; run independent swarms.","钻石依赖开采、花卉、白银和切割，请组织独立集群。",1400,G("diamond",-1,35),G("platinum",-1,45),G("orchid",-1,60)),
            new FleetContract("Botanical seed vault","宇宙种质库","Build a balanced six-species reserve in the mothership.","在母舰建立六种花卉的均衡种质储备。",1500,G("rose",-1,180),G("tulip",-1,150),G("lavender",-1,150),G("lily",-1,120),G("orchid",-1,75),G("lotus",-1,75)),
            new FleetContract("A silver ocean","银色海洋","Deliver fish and refined metals together without starving the colonies.","联合交付鱼类和精炼金属，同时维持殖民地原料。",1600,G("silver",-1,150),G("salmon",-1,120),G("tuna",-1,90),G("koi",-1,50)),
            new FleetContract("Deep core observatory","深核观测站","Sustain rare-ore processing alongside ordinary metal supply.","维持稀有矿石加工，并同时供应普通金属。",1800,G("diamond",-1,75),G("gold",-1,120),G("platinum",-1,90),G("bronze",-1,225)),
            new FleetContract("Distributed life support","分布式生命保障","Balance local and orbital depots across every planet.","平衡各星球本地仓库与轨道仓库的生命保障物资。",1800,G("water",1,300),G("fish_feed",2,240),G("solar_energy",3,360),G("honey",-1,180),G("fishmeal",-1,180)),
            new FleetContract("Four-world exhibition","四星博览会","Deliver a curated botanical, marine and mineral collection.","联合交付花卉、海洋及矿物的特色展品组合。",2000,G("orchid",-1,135),G("lotus",-1,135),G("koi",-1,105),G("gold",-1,180),G("diamond",-1,105)),
            new FleetContract("Orbital research city","轨道科研城","Produce and transport an interdependent supply network at industrial scale.","以工业规模生产并运输相互依赖的供应网络。",2400,G("diamond",-1,150),G("platinum",-1,210),G("silver",-1,300),G("honey",-1,240),G("tuna",-1,210)),
            new FleetContract("A self-sustaining universe","自持宇宙","Complete the final simultaneous delivery to your mothership.","向母舰完成最后一组联合交付，让四星供应链形成完整闭环。",3000,G("diamond",-1,210),G("orchid",-1,270),G("koi",-1,180),G("bronze",-1,360),G("lotus",-1,180),G("honey",-1,360))};
        public event Action<int,double> OrderCompleted;
        readonly ClassicFarm farm;
        public ArchipelagoSave Data {get{return farm.Data.archipelago;}}
        public ArchipelagoWorld(ClassicFarm farm){this.farm=farm;if(farm.Data.archipelago==null||EmptyPlaceholder(farm.Data.archipelago))farm.Data.archipelago=new ArchipelagoSave();for(int i=0;i<4;i++)if(!Data.islands.Any(s=>s.island==i))Data.islands.Add(new IslandStock{island=i});MigrateNetwork();}
        public int SlotLimit {get{return Enumerable.Range(0,4).Where(IsUnlocked).Sum(WorkerLimit);}}
        public int IslandCount {get{return 4;}}
        public int DiscoveredCount {get{return Enumerable.Range(0,4).Count(IsUnlocked);}}
        public bool IsUnlocked(int planet){return planet==0||planet>0&&planet<4&&farm.Level("spaceflight")>0&&farm.Level(IslandResearch[planet])>0;}
        public bool Complete {get{return Data.order>=Contracts.Length;}}
        public FleetContract Current {get{return Complete?null:Contracts[Data.order];}}
        public static double Get(List<Stock> stocks,string item){var found=stocks.Find(s=>s.id==item);return found==null?0:found.amount;}
        public static void Put(List<Stock> stocks,string item,double amount){var found=stocks.Find(s=>s.id==item);if(found==null)stocks.Add(new Stock(item,Math.Max(0,amount)));else found.amount=Math.Max(0,found.amount+amount);}
        public double Stock(int planet,string item){return planet==-1?Get(Data.mothership,item):planet==0?farm.Count(item):Get(Data.islands[planet].items,item);}
        public void Add(int planet,string item,double count){if(planet==0){farm.Add(item,count);Put(count<0?Data.earthSent:Data.earthReceived,item,Math.Abs(count));}else Put(planet==-1?Data.mothership:Data.islands[planet].items,item,count);}
        public bool Start(){Data.started=farm.Level("spaceflight")>0;return Data.started;}
        public bool Research(string id){return ResearchWorker(0,id);}
        public int Build(string role,int width,int height,int planet)
        {
            if(!IsUnlocked(planet)||role!=Roles[planet]||Data.units.Count(u=>u.island==planet)>=WorkerLimit(planet)||!HullAllowed(planet,width,height)||Data.credits<WorkerPrice(width,height))return -1;
            var unit=new FleetUnit{id=Data.nextId++,role=role,island=planet,width=width,height=height};int x,y;if(!FindParking(unit,out x,out y))return -1;unit.x=unit.cropX=x;unit.y=unit.cropY=y;unit.parkX=x;unit.parkY=y;double price=WorkerPrice(width,height);Data.credits-=price;Data.buildSpent+=price;Data.units.Add(unit);return unit.id;
        }
        public FleetUnit Unit(int id){return Data.units.FirstOrDefault(u=>u.id==id);}
        public int Capacity(FleetUnit u){return u.width*u.height;}
        public bool Cancel(int id)
        {
            var u=Unit(id);if(u==null)return false;
            if(u.jobs.Count>0&&u.jobs[0].kind=="work"&&u.jobs[0].phase==1){var recipe=Recipes.First(r=>r.item==u.jobs[0].item&&r.island==u.island);foreach(var input in recipe.inputs){Add(u.island,input.Key,input.Value*u.width*u.height);Put(Data.consumed,input.Key,-input.Value*u.width*u.height);}}
            u.jobs.Clear();u.status="idle";return true;
        }
        public bool Remove(int id){var u=Unit(id);if(u==null)return false;Cancel(id);StopProgram(id);double price=u.freeStarter||u.transient?0:WorkerPrice(u.width,u.height);Data.credits+=price;Data.refunds+=price;Data.units.Remove(u);programs.Remove(id);inboxes.Remove(id);results.Remove(id);foreach(var group in Data.groups)group.members.Remove(id);return true;}
        public int RequiredHull {get{return Data.order>=6?4:1;}}
        public int RequiredPrograms {get{return Data.order>=4?3:0;}}
        public int RequiredFlights {get{return 0;}}
        public bool Ready {get{return Data.started&&!Complete&&Current.goals.All(g=>DepotUnlocked(g.island)&&Stock(g.island,g.item)>=g.amount)&&Data.orderPeakHull>=RequiredHull&&Data.orderPeakPrograms>=RequiredPrograms;}}
        public bool Submit(){if(!Ready)return false;foreach(var g in Current.goals){Add(g.island,g.item,-g.amount);Put(Data.submitted,g.item,g.amount);}Data.credits+=Current.reward;Data.orderTimes.Add(Data.time-Data.orderStarted);Data.order++;Data.orderStarted=Data.time;Data.orderPeakHull=Data.orderPeakPrograms=0;farm.Data.completionNotices.Add(new CompletionNotice{kind="archipelago",id=(Data.order-1).ToString()});OrderCompleted?.Invoke(Data.order,Data.time);return true;}
    }
}
