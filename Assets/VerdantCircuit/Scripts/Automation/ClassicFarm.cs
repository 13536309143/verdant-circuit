using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verdant.Automation
{
    [Serializable] public sealed class Stock {public string id;public double amount;public Stock(string id,double amount){this.id=id;this.amount=amount;}}
    [Serializable] public sealed class ClassicCell
    {public string entity="grass",ground="grassland",companion;public double growth,water;public bool infected;public int measure,companionX,companionY;}
    [Serializable] public sealed class ClassicProgram {public string name,source;public ClassicProgram(string n,string s){name=n;source=s;}}
    [Serializable] public sealed class ClassicSave
    {
        public ArchipelagoSave archipelago;
        public string projectId=Guid.NewGuid().ToString("N");public int schema=2,size=1,height=1,defaultSize,defaultHeight;public double growthAccumulator;public uint randomState=731;public double time,waterTimer,fertilizerTimer;public string version=VerdantContent.Version;
        public List<Stock> items=new List<Stock>(),unlocks=new List<Stock>(),harvested=new List<Stock>();
        public List<ClassicCell> cells=new List<ClassicCell>{new ClassicCell()};public List<ClassicProgram> programs=new List<ClassicProgram>();
        public List<CompletionNotice> completionNotices=new List<CompletionNotice>();
        public List<string> achievements=new List<string>();public List<double> best=new List<double>();
        public int selectedPlanet;public int selectedProgram;public List<Stock> personalBest=new List<Stock>();public int droneLimit;public List<RaceRecord> records=new List<RaceRecord>();public int playerX,playerY;public bool maze;public int[] walls;public List<int> tail=new List<int>();public int dinosaur=-1,apple=-1,nextApple=-1,treasure=-1,mazeReuses;public bool allowLockedSyntax=false;public bool sunflowerPenalty;public float volume=.25f;
    }
    public sealed class ClassicTech
    {
        public string id,resource,prerequisite;public int max;public double cost;public ClassicTech(string id,string resource,double cost,int max=1,string prerequisite=""){this.id=id;this.resource=resource;this.cost=cost;this.max=max;this.prerequisite=prerequisite;}
    }
    public sealed class ClassicFarm
    {
        public ClassicSave Data;
        public event Action<int,int,string,double> Harvested;
        ArchipelagoWorld archipelago;
        public ArchipelagoWorld Fleet {get{return archipelago??(archipelago=new ArchipelagoWorld(this));}}
        public int[] walls {get{return Data.walls;}set{Data.walls=value;}}public List<int> tail {get{return Data.tail;}set{Data.tail=value;}}
        public int dinosaur {get{return Data.dinosaur;}set{Data.dinosaur=value;}}public int apple {get{return Data.apple;}set{Data.apple=value;}}
        public int nextApple {get{return Data.nextApple;}set{Data.nextApple=value;}}public int treasure {get{return Data.treasure;}set{Data.treasure=value;}}
        public int mazeReuses {get{return Data.mazeReuses;}set{Data.mazeReuses=value;}}public bool maze {get{return Data.maze;}set{Data.maze=value;}}public event Action<string> Message;
        public static readonly string[] Entities={"grass","bush","tree","carrot","pumpkin","sunflower","cactus"};
        public static readonly string[] Items={"hay","wood","carrot","pumpkin","solar_energy","cactus","bone","coins","water","fertilizer","catalyst"};
        public static readonly ClassicTech[] Techs={
            new ClassicTech("plant","hay",5),new ClassicTech("expand","hay",8,9,"plant"),new ClassicTech("speed","hay",20,8,"expand"),
            new ClassicTech("grass","hay",50,8,"plant"),new ClassicTech("trees","wood",30,8,"plant"),new ClassicTech("carrots","wood",40,8,"expand"),
            new ClassicTech("watering","wood",80,6,"carrots"),new ClassicTech("pumpkins","carrot",100,8,"carrots"),new ClassicTech("sunflowers","carrot",150,8,"watering"),
            new ClassicTech("fertilizer","pumpkin",100,6,"pumpkins"),new ClassicTech("polyculture","hay",500,6,"trees"),
            new ClassicTech("cactus","pumpkin",400,8,"pumpkins"),new ClassicTech("mazes","catalyst",30,8,"fertilizer"),new ClassicTech("dinosaurs","cactus",500,8,"cactus"),
            new ClassicTech("megafarm","coins",100,5,"mazes"),new ClassicTech("spaceflight","coins",500,1,"megafarm"),new ClassicTech("flower_planet","pumpkin",2000,1,"spaceflight"),new ClassicTech("mineral_planet","cactus",5000,1,"spaceflight"),new ClassicTech("ocean_planet","carrot",20000,1,"spaceflight"),new ClassicTech("leaderboard","coins",1000,1,"dinosaurs"),
            new ClassicTech("loops","hay",5),new ClassicTech("operators","hay",10,1,"loops"),new ClassicTech("variables","hay",15,1,"operators"),new ClassicTech("functions","wood",20,1,"variables"),
            new ClassicTech("lists","wood",50,1,"functions"),new ClassicTech("dicts","carrot",30,1,"lists"),new ClassicTech("import","carrot",50,1,"functions"),new ClassicTech("senses","hay",10),
            new ClassicTech("costs","wood",10),new ClassicTech("auto_unlock","carrot",60),new ClassicTech("timing","wood",20),new ClassicTech("debug","hay",5),new ClassicTech("hats","wood",25),new ClassicTech("debug2","wood",100,1,"debug"),new ClassicTech("utilities","wood",60,1,"operators"),new ClassicTech("the_farmers_remains","bone",170000,1,"leaderboard"),new ClassicTech("top_hat","coins",320000,1,"the_farmers_remains")
        };
        public ClassicFarm(uint seed=731){Data=new ClassicSave{randomState=seed==0?731:seed};}
        public double Count(string id){return Get(Data.items,id);}
        public int Level(string id){return (int)Get(Data.unlocks,id);}
        static double Get(List<Stock> list,string id){var s=list.Find(x=>x.id==id);return s==null?0:s.amount;}
        static void Add(List<Stock> list,string id,double amount){var s=list.Find(x=>x.id==id);if(s==null)list.Add(new Stock(id,Math.Max(0,amount)));else s.amount=Math.Max(0,s.amount+amount);}
        public void Add(string id,double amount){Add(Data.items,id,amount);}
        public double Random(){uint s=Data.randomState;s^=s<<13;s^=s>>17;s^=s<<5;Data.randomState=s==0?731:s;return Data.randomState/4294967296.0;}
        public int MaxDrones {get{return Data.droneLimit>0?Data.droneLimit:Math.Min(32,1<<Level("megafarm"));}}
        public int Height {get{return Math.Max(1,Data.height);}}
        public int MaxSize {get{return SurfaceStages[Math.Min(9,Level("expand")),0];}}
        public int MaxHeight {get{return SurfaceStages[Math.Min(9,Level("expand")),1];}}
        public static readonly int[,] SurfaceStages={{1,1},{2,2},{4,4},{8,8},{16,16},{32,32},{64,64},{128,128},{256,128},{360,128}};
        public double Speed {get{return Math.Pow(ClassicBalance.SpeedGrowth,Level("speed"))*(Count("solar_energy")>0?ClassicBalance.PowerSpeedBonus:1);}}
        public ClassicCell Cell(int x,int y){return Data.cells[y*Data.size+x];}
        public int Wrap(int n){return (n%Data.size+Data.size)%Data.size;}
        public Dictionary<object,object> Cost(string id,int level=-1)
        {
            var tech=Techs.FirstOrDefault(t=>t.id==id);if(tech!=null){int l=level<0?Level(id):level;if(l>=tech.max)return null;return new Dictionary<object,object>{{tech.resource,Math.Ceiling(tech.cost*Math.Pow(ClassicBalance.ResearchGrowth,l))}};}
            return SeedCost(id);
        }
        public Dictionary<object,object> SeedCost(string id)
        {
            switch(id){case "carrot":return new Dictionary<object,object>{{"hay",2.0*Math.Pow(2,Math.Max(0,Level("carrots")-1))},{"wood",2.0*Math.Pow(2,Math.Max(0,Level("carrots")-1))}};case "pumpkin":return new Dictionary<object,object>{{"carrot",Math.Pow(2,Math.Max(0,Level("pumpkins")-1))}};case "sunflower":return new Dictionary<object,object>{{"carrot",Math.Pow(2,Math.Max(0,Level("sunflowers")-1))}};case "cactus":return new Dictionary<object,object>{{"pumpkin",Math.Pow(2,Math.Max(0,Level("cactus")-1))}};default:return Entities.Contains(id)?new Dictionary<object,object>():null;}
        }
        public bool Spend(Dictionary<object,object> cost){if(cost==null||cost.Any(p=>Count((string)p.Key)<ScriptValue.Number(p.Value)))return false;foreach(var p in cost)Add((string)p.Key,-ScriptValue.Number(p.Value));return true;}
        public double Produced(string item){return Get(Data.harvested,item);}
        public Dictionary<object,object> Requirements(string id){return ClassicProgression.Requirements(id,Level(id)+1);}
        public bool ResearchRequirementsMet(string id)
        {
            var requirements=Requirements(id);return requirements!=null&&requirements.All(p=>Convert.ToString(p.Key).StartsWith("unlock:")?Level(Convert.ToString(p.Key).Substring(7))>=ScriptValue.Number(p.Value):Produced(Convert.ToString(p.Key).Substring(9))>=ScriptValue.Number(p.Value));
        }
        public bool CanUnlock(string id){var cost=Cost(id);return ResearchRequirementsMet(id)&&cost!=null&&cost.All(p=>Count((string)p.Key)>=ScriptValue.Number(p.Value));}
        public bool Unlock(string id)
        {
            var t=Techs.FirstOrDefault(x=>x.id==id);if(t==null)throw new Exception("Unknown research: "+id);if(!ResearchRequirementsMet(id)||!Spend(Cost(id)))return false;Add(Data.unlocks,id,1);if(id=="expand")ExpandSurface(MaxSize,MaxHeight);if(id=="spaceflight")Fleet.Start();return true;
        }
        public void Resize(int size){Resize(size,Math.Min(size,MaxHeight));}
        public void Resize(int width,int height){if(width<1||width>MaxSize||height<1||height>MaxHeight)throw new Exception("Surface extent is locked.");Data.sunflowerPenalty=false;Data.size=width;Data.height=height;Data.cells=Enumerable.Range(0,width*height).Select(_=>new ClassicCell()).ToList();maze=false;walls=null;tail.Clear();dinosaur=apple=treasure=-1;Data.playerX=Math.Min(Data.playerX,width-1);Data.playerY=Math.Min(Data.playerY,height-1);RefreshCompanions();}
        public void ExpandSurface(int width,int height)
        {
            int oldWidth=Data.size,oldHeight=Height;var old=Data.cells;Data.size=width;Data.height=height;Data.cells=Enumerable.Range(0,width*height).Select(_=>new ClassicCell()).ToList();
            int dx=(width-oldWidth)/2,dy=(height-oldHeight)/2;for(int y=0;y<Math.Min(height,oldHeight);y++)for(int x=0;x<Math.Min(width,oldWidth);x++)Data.cells[(y+dy)*width+x+dx]=old[y*oldWidth+x];Data.playerX+=dx;Data.playerY+=dy;if(Data.archipelago!=null)foreach(var u in Data.archipelago.units.Where(u=>u.island==0)){u.x=u.cropX+=dx;u.y=u.cropY+=dy;foreach(var j in u.jobs){if(j.kind=="route"){j.target+=dx;j.source+=dy;}if(j.workX>=0){j.workX+=dx;j.workY+=dy;}}}
            maze=false;walls=null;tail.Clear();dinosaur=apple=treasure=-1;RefreshCompanions();
        }
        public void Advance(double dt)
        {
            if(Data.archipelago!=null)Fleet.Advance(dt);
            Data.time+=dt;if(Level("watering")>0){Data.waterTimer+=dt;while(Data.waterTimer>=10){Data.waterTimer-=10;Add("water",Math.Pow(2,Level("watering")-1));}}
            if(Level("fertilizer")>0){Data.fertilizerTimer+=dt;while(Data.fertilizerTimer>=10){Data.fertilizerTimer-=10;Add("fertilizer",Math.Pow(2,Level("fertilizer")-1));}}
            Data.growthAccumulator+=dt;if(Data.growthAccumulator<.25)return;dt=Data.growthAccumulator;Data.growthAccumulator=0;
            int n=Data.size;for(int i=0;i<Data.cells.Count;i++){var p=Data.cells[i];p.water=Math.Max(0,p.water-dt*p.water*.01/(1+(Data.archipelago==null?0:.15*Fleet.PlanetLevel(0,"retention"))));if(!Entities.Contains(p.entity)||Ready(p))continue;double rate=(1+4*p.water)*(1+(Data.archipelago==null?0:.12*Fleet.PlanetLevel(0,"ecology")));if(p.entity=="tree"){int x=i%n,y=i/n;int adjacent=Neighbors(x,y).Count(j=>Data.cells[j].entity=="tree");rate/=Math.Pow(2,adjacent);}p.growth+=dt*rate;ResolvePumpkinMaturity(p);}
        }
        void ResolvePumpkinMaturity(ClassicCell p){if(p.entity=="pumpkin"&&Ready(p)&&p.measure==0){p.measure=1;if(Random()<.2)p.entity="dead_pumpkin";}}
        public IEnumerable<int> Neighbors(int x,int y){if(y+1<Height)yield return (y+1)*Data.size+x;if(x+1<Data.size)yield return y*Data.size+x+1;else if(Data.size==360)yield return y*Data.size;if(y>0)yield return (y-1)*Data.size+x;if(x>0)yield return y*Data.size+x-1;else if(Data.size==360)yield return y*Data.size+359;}
        public static double GrowTime(string entity){return ClassicBalance.GrowthSeconds(entity);}
        public bool Ready(ClassicCell p){return p.growth>=GrowTime(p.entity)&&Entities.Contains(p.entity);}
        bool UnlockedEntity(string e){string tech=e=="tree"?"trees":e=="carrot"?"carrots":e=="pumpkin"?"pumpkins":e=="sunflower"?"sunflowers":e;return e=="grass"||e=="bush"&&Level("plant")>0||Level(tech)>0;}
        public bool Plant(string entity,int x,int y)
        {
            if(!Entities.Contains(entity))throw new Exception("Unknown plant: "+entity);var p=Cell(x,y);if(!UnlockedEntity(entity)||p.entity!=""&&p.entity!="dead_pumpkin"||maze)return false;
            if(entity!="grass"&&entity!="bush"&&entity!="tree"&&p.ground!="soil")return false;if(!Spend(SeedCost(entity)))return false;
            p.entity=entity;p.growth=0;p.infected=false;p.measure=entity=="cactus"?(int)(Random()*10):entity=="sunflower"?7+(int)(Random()*9):0;
            SetCompanion(p,x,y);return true;
        }
        void SetCompanion(ClassicCell p,int x,int y)
        {
            p.companion=null;if(Data.size<2||!new[]{"grass","bush","tree","carrot"}.Contains(p.entity))return;var choices=new[]{"grass","bush","tree","carrot"}.Where(e=>e!=p.entity).ToArray();p.companion=choices[(int)(Random()*choices.Length)];var cells=new List<int>();for(int dy=-3;dy<=3;dy++)for(int dx=-3;dx<=3;dx++){if(Math.Abs(dx)+Math.Abs(dy)>3||dx==0&&dy==0)continue;int nx=x+dx,ny=y+dy;if(Data.size==360)nx=Wrap(nx);if(nx>=0&&nx<Data.size&&ny>=0&&ny<Height)cells.Add(ny*Data.size+nx);}if(cells.Count==0){p.companion=null;return;}int selected=cells[(int)(Random()*cells.Count)];p.companionX=selected%Data.size;p.companionY=selected/Data.size;
        }
        void RefreshCompanions(){for(int i=0;i<Data.cells.Count;i++)SetCompanion(Data.cells[i],i%Data.size,i/Data.size);}
        public void Produce(string item,double amount,bool infected=false){Produce(item,amount,infected?1.0:0.0);}
        public void Produce(string item,double amount,double infectedFraction){double weird=Math.Floor(amount*Math.Max(0,Math.Min(1,infectedFraction))*.5);if(weird>0){Add("catalyst",weird);Add(Data.harvested,"catalyst",weird);}amount-=weird;Add(item,amount);Add(Data.harvested,item,amount);}
        public bool Harvest(int x,int y)
        {
            int index=y*Data.size+x;var p=Data.cells[index];if(maze){if(index==treasure){Produce("coins",Data.cells.Count*Math.Pow(2,Math.Max(0,Level("mazes")-1)),false);ClearMaze();return true;}ClearMaze();return false;}
            if(!Ready(p))return false;string e=p.entity,item=e=="grass"?"hay":e=="bush"||e=="tree"?"wood":e=="sunflower"?"solar_energy":e;double amount=e=="tree"?5:1;
            string upgrade=e=="grass"?"grass":e=="bush"||e=="tree"?"trees":e=="carrot"?"carrots":e=="pumpkin"?"pumpkins":e=="sunflower"?"sunflowers":e;
            amount*=Math.Pow(2,Math.Max(0,Level(upgrade)-(e=="grass"?0:1)));
            if(Level("polyculture")>0&&p.companion!=null&&Cell(p.companionX,p.companionY).entity==p.companion)amount*=ClassicBalance.CompanionMultiplier(Level("polyculture"));
            var harvested=new List<int>{index};
            if(e=="pumpkin")
            {
                int best=1,bx=x,by=y;var previous=new int[Data.size+1];var row=new int[Data.size+1];
                for(int sy=0;sy<Height;sy++){Array.Clear(row,0,row.Length);for(int sx=0;sx<Data.size;sx++){var q=Cell(sx,sy);if(q.entity!="pumpkin"||!Ready(q))continue;int side=row[sx+1]=1+Math.Min(previous[sx],Math.Min(previous[sx+1],row[sx]));if(x<=sx&&y<=sy&&sx-side+1<=x&&sy-side+1<=y&&side>best){best=side;bx=sx-best+1;by=sy-best+1;}}var swap=previous;previous=row;row=swap;}
                harvested.Clear();for(int oy=0;oy<best;oy++)for(int ox=0;ox<best;ox++)harvested.Add((by+oy)*Data.size+bx+ox);amount*=best*best*Math.Min(best,6);
            }
            if(e=="cactus")
            {
                var region=new HashSet<int>();var q=new Queue<int>();region.Add(index);q.Enqueue(index);while(q.Count>0){int j=q.Dequeue();int cx=j%Data.size,cy=j/Data.size;foreach(int k in AdjacentBounded(cx,cy))if(!region.Contains(k)&&Data.cells[k].entity=="cactus"&&Ready(Data.cells[k])){region.Add(k);q.Enqueue(k);}}
                bool sorted=true;foreach(int j in region){int cx=j%Data.size,cy=j/Data.size;if(cx+1<Data.size&&region.Contains(j+1)&&Data.cells[j].measure>Data.cells[j+1].measure)sorted=false;if(cy+1<Height&&region.Contains(j+Data.size)&&Data.cells[j].measure>Data.cells[j+Data.size].measure)sorted=false;}
                if(sorted){harvested=region.ToList();amount*=ClassicBalance.CactusMultiplier(harvested.Count);}
            }
            if(e=="sunflower"){var all=Data.cells.Where(c=>c.entity=="sunflower").ToList();int maximum=all.Max(c=>c.measure);if(p.measure<maximum)Data.sunflowerPenalty=true;if(all.Count>=10&&p.measure==maximum&&!Data.sunflowerPenalty)amount*=8;}
            Harvested?.Invoke(x,y,e,amount);Produce(item,amount,harvested.Count(j=>Data.cells[j].infected)/(double)harvested.Count);foreach(int j in harvested){var cell=Data.cells[j];cell.entity=cell.ground=="grassland"?"grass":"";cell.growth=0;cell.companion=null;cell.infected=false;cell.measure=0;SetCompanion(cell,j%Data.size,j/Data.size);}if(e=="sunflower"&&!Data.cells.Any(c=>c.entity=="sunflower"))Data.sunflowerPenalty=false;return true;
        }
        IEnumerable<int> AdjacentBounded(int x,int y){if(x>0)yield return y*Data.size+x-1;if(x+1<Data.size)yield return y*Data.size+x+1;if(y>0)yield return (y-1)*Data.size+x;if(y+1<Height)yield return (y+1)*Data.size+x;}
        public bool Use(string item,int x,int y,int amount=1)
        {
            if(amount<1||Count(item)<amount)return false;var p=Cell(x,y);if(item=="water"){Add(item,-amount);p.water=Math.Min(1,p.water+.25*amount);return true;}
            if(item=="fertilizer"){if(!Entities.Contains(p.entity))return false;Add(item,-amount);p.growth+=2*amount;p.infected=true;ResolvePumpkinMaturity(p);return true;}
            if(item=="catalyst")
            {
                if(p.entity=="bush"&&Level("mazes")>0){int scale=1<<Math.Max(0,Level("mazes")-1);if(amount<Data.size*scale)return false;Add(item,-amount);MakeMaze(x,y);return true;}
                if(maze&&x+y*Data.size==treasure){int scale=1<<Math.Max(0,Level("mazes")-1);if(amount<Data.size*scale||mazeReuses>=300)return false;Add(item,-amount);Produce("coins",Data.cells.Count*scale,false);mazeReuses++;int old=treasure;for(int i=0;i<Math.Max(1,Data.size/2);i++){int a=(int)(Random()*Data.cells.Count),dir=(int)(Random()*4);int b=NeighborIndex(a,dir,false);if(b>=0){walls[a]&=~(1<<dir);walls[b]&=~(1<<((dir+2)%4));}}treasure=ChooseTreasure(old);Data.cells[old].entity="hedge";Data.cells[treasure].entity="treasure";return true;}
                Add(item,-amount);p.infected=!p.infected;foreach(int j in Neighbors(x,y).Distinct().Where(j=>j!=y*Data.size+x))Data.cells[j].infected=!Data.cells[j].infected;return true;
            }
            return false;
        }
        public int NeighborIndex(int index,int direction,bool wrap){int x=index%Data.size,y=index/Data.size;int[] dx={0,1,0,-1},dy={1,0,-1,0};x+=dx[direction];y+=dy[direction];if(!wrap&&(x<0||y<0||x>=Data.size||y>=Height))return -1;return ((y%Height+Height)%Height)*Data.size+Wrap(x);}
        void MakeMaze(int x,int y)
        {
            maze=true;mazeReuses=0;walls=Enumerable.Repeat(15,Data.cells.Count).ToArray();var visited=new HashSet<int>();var stack=new Stack<int>();int start=y*Data.size+x;visited.Add(start);stack.Push(start);
            while(stack.Count>0){int a=stack.Peek();var directions=Enumerable.Range(0,4).Where(d=>NeighborIndex(a,d,false)>=0&&!visited.Contains(NeighborIndex(a,d,false))).ToArray();if(directions.Length==0){stack.Pop();continue;}int dir=directions[(int)(Random()*directions.Length)],b=NeighborIndex(a,dir,false);walls[a]&=~(1<<dir);walls[b]&=~(1<<((dir+2)%4));visited.Add(b);stack.Push(b);}
            foreach(var p in Data.cells){p.entity="hedge";p.growth=0;}treasure=ChooseTreasure(start);Data.cells[treasure].entity="treasure";
        }
        int ChooseTreasure(int start)
        {
            var distances=Enumerable.Repeat(-1,Data.cells.Count).ToArray();var queue=new Queue<int>();distances[start]=0;queue.Enqueue(start);int maximum=0;
            while(queue.Count>0){int a=queue.Dequeue();for(int direction=0;direction<4;direction++){if((walls[a]&(1<<direction))!=0)continue;int b=NeighborIndex(a,direction,false);if(b<0||distances[b]>=0)continue;distances[b]=distances[a]+1;maximum=Math.Max(maximum,distances[b]);queue.Enqueue(b);}}
            if(maximum==0)return start;int minimum=Math.Max(1,(int)Math.Ceiling(maximum*.65));var choices=Enumerable.Range(0,distances.Length).Where(i=>distances[i]>=minimum).ToArray();return choices[(int)(Random()*choices.Length)];
        }
        public void ClearMaze(){maze=false;walls=null;treasure=-1;foreach(var p in Data.cells){p.entity=p.ground=="grassland"?"grass":"";p.growth=0;}RefreshCompanions();}
        public int SurfaceNeighbor(int index,int direction)
        {int target=NeighborIndex(index,direction,false);if(target<0&&!maze&&Data.size==360&&(direction==1||direction==3))target=(index/Data.size)*360+(direction==1?0:359);return target;}
        public bool CanMove(int index,int direction,bool snake=false){if(maze&&(walls[index]&(1<<direction))!=0)return false;int target=NeighborIndex(index,direction,false);if(target<0&&!maze&&Data.size==360&&(direction==1||direction==3)){int y=index/Data.size;target=y*360+(direction==1?0:359);}return target>=0&&(!snake||!tail.Take(index==apple?tail.Count:Math.Max(0,tail.Count-1)).Contains(target));}
        public object Companion(int x,int y){var p=Cell(x,y);return Level("polyculture")==0||p.companion==null?null:new object[]{p.companion,new object[]{(double)p.companionX,(double)p.companionY}};}
        public ClassicFarm Snapshot(){return new ClassicFarm{Data=JsonUtility.FromJson<ClassicSave>(JsonUtility.ToJson(Data)),maze=maze,walls=walls==null?null:(int[])walls.Clone(),treasure=treasure,mazeReuses=mazeReuses,tail=new List<int>(tail),dinosaur=dinosaur,apple=apple};}
        public void Say(string text){if(Message!=null)Message(text);}
    }
}
