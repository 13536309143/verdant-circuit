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
        public string projectId=Guid.NewGuid().ToString("N");public int schema=1,size=1,defaultSize;public uint randomState=731;public double time,waterTimer,fertilizerTimer;public string version="0.11.1";
        public List<Stock> items=new List<Stock>(),unlocks=new List<Stock>(),harvested=new List<Stock>();
        public List<ClassicCell> cells=new List<ClassicCell>{new ClassicCell()};public List<ClassicProgram> programs=new List<ClassicProgram>();
        public List<CompletionNotice> completionNotices=new List<CompletionNotice>();
        public List<string> achievements=new List<string>();public List<double> best=new List<double>();
        public int selectedProgram;public List<Stock> personalBest=new List<Stock>();public int droneLimit;public List<RaceRecord> records=new List<RaceRecord>();public int playerX,playerY;public bool maze;public int[] walls;public List<int> tail=new List<int>();public int dinosaur=-1,apple=-1,nextApple=-1,treasure=-1,mazeReuses;public bool allowLockedSyntax=false;public bool sunflowerPenalty;public float volume=.25f;public string theme="default";
    }
    public sealed class ClassicTech
    {
        public string id,resource,prerequisite;public int max;public double cost;public ClassicTech(string id,string resource,double cost,int max=1,string prerequisite=""){this.id=id;this.resource=resource;this.cost=cost;this.max=max;this.prerequisite=prerequisite;}
    }
    public sealed class ClassicFarm
    {
        public ClassicSave Data;
        public int[] walls {get{return Data.walls;}set{Data.walls=value;}}public List<int> tail {get{return Data.tail;}set{Data.tail=value;}}
        public int dinosaur {get{return Data.dinosaur;}set{Data.dinosaur=value;}}public int apple {get{return Data.apple;}set{Data.apple=value;}}
        public int nextApple {get{return Data.nextApple;}set{Data.nextApple=value;}}public int treasure {get{return Data.treasure;}set{Data.treasure=value;}}
        public int mazeReuses {get{return Data.mazeReuses;}set{Data.mazeReuses=value;}}public bool maze {get{return Data.maze;}set{Data.maze=value;}}public event Action<string> Message;
        public static readonly string[] Entities={"grass","bush","tree","carrot","pumpkin","sunflower","cactus"};
        public static readonly string[] Items={"hay","wood","carrot","pumpkin","power","cactus","bone","gold","water","fertilizer","weird_substance"};
        public static readonly ClassicTech[] Techs={
            new ClassicTech("plant","hay",5),new ClassicTech("expand","hay",8,5,"plant"),new ClassicTech("speed","hay",20,8,"expand"),
            new ClassicTech("grass","hay",50,8,"plant"),new ClassicTech("trees","wood",30,8,"plant"),new ClassicTech("carrots","wood",40,8,"expand"),
            new ClassicTech("watering","wood",80,6,"carrots"),new ClassicTech("pumpkins","carrot",100,8,"carrots"),new ClassicTech("sunflowers","carrot",150,8,"watering"),
            new ClassicTech("fertilizer","pumpkin",100,6,"pumpkins"),new ClassicTech("polyculture","hay",500,6,"trees"),
            new ClassicTech("cactus","pumpkin",400,8,"pumpkins"),new ClassicTech("mazes","weird_substance",30,8,"fertilizer"),new ClassicTech("dinosaurs","cactus",500,8,"cactus"),
            new ClassicTech("megafarm","gold",100,5,"mazes"),new ClassicTech("simulation","bone",100,1,"dinosaurs"),new ClassicTech("leaderboard","gold",1000,1,"simulation"),
            new ClassicTech("loops","hay",5),new ClassicTech("operators","hay",10,1,"loops"),new ClassicTech("variables","hay",15,1,"operators"),new ClassicTech("functions","wood",20,1,"variables"),
            new ClassicTech("lists","wood",50,1,"functions"),new ClassicTech("dicts","carrot",30,1,"lists"),new ClassicTech("import","carrot",50,1,"functions"),new ClassicTech("senses","hay",10),
            new ClassicTech("costs","wood",10),new ClassicTech("auto_unlock","carrot",60),new ClassicTech("timing","wood",20),new ClassicTech("debug","hay",5),new ClassicTech("hats","wood",25),new ClassicTech("debug2","wood",100,1,"debug"),new ClassicTech("utilities","wood",60,1,"operators"),new ClassicTech("the_farmers_remains","bone",170000,1,"leaderboard"),new ClassicTech("top_hat","gold",320000,1,"the_farmers_remains")
        };
        public ClassicFarm(uint seed=731){Data=new ClassicSave{randomState=seed==0?731:seed};}
        public double Count(string id){return Get(Data.items,id);}
        public int Level(string id){return (int)Get(Data.unlocks,id);}
        static double Get(List<Stock> list,string id){var s=list.Find(x=>x.id==id);return s==null?0:s.amount;}
        static void Add(List<Stock> list,string id,double amount){var s=list.Find(x=>x.id==id);if(s==null)list.Add(new Stock(id,Math.Max(0,amount)));else s.amount=Math.Max(0,s.amount+amount);}
        public void Add(string id,double amount){Add(Data.items,id,amount);}
        public double Random(){uint s=Data.randomState;s^=s<<13;s^=s>>17;s^=s<<5;Data.randomState=s==0?731:s;return Data.randomState/4294967296.0;}
        public int MaxDrones {get{return Data.droneLimit>0?Data.droneLimit:Math.Min(32,1<<Level("megafarm"));}}
        public int MaxSize {get{return Math.Min(32,1<<Level("expand"));}}
        public double Speed {get{return Math.Pow(ClassicBalance.SpeedGrowth,Level("speed"))*(Count("power")>0?ClassicBalance.PowerSpeedBonus:1);}}
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
        public bool Unlock(string id)
        {
            var t=Techs.FirstOrDefault(x=>x.id==id);if(t==null)throw new Exception("Unknown research: "+id);if(t.prerequisite!=""&&Level(t.prerequisite)==0)return false;if(!Spend(Cost(id)))return false;Add(Data.unlocks,id,1);if(id=="expand")Resize(MaxSize);return true;
        }
        public void Resize(int size){if(size<1||size>MaxSize)throw new Exception("Farm size is locked.");Data.sunflowerPenalty=false;Data.size=size;Data.cells=Enumerable.Range(0,size*size).Select(_=>new ClassicCell()).ToList();maze=false;walls=null;tail.Clear();dinosaur=apple=treasure=-1;RefreshCompanions();}
        public void Advance(double dt)
        {
            Data.time+=dt;if(Level("watering")>0){Data.waterTimer+=dt;while(Data.waterTimer>=10){Data.waterTimer-=10;Add("water",Math.Pow(2,Level("watering")-1));}}
            if(Level("fertilizer")>0){Data.fertilizerTimer+=dt;while(Data.fertilizerTimer>=10){Data.fertilizerTimer-=10;Add("fertilizer",Math.Pow(2,Level("fertilizer")-1));}}
            int n=Data.size;for(int i=0;i<Data.cells.Count;i++){var p=Data.cells[i];p.water=Math.Max(0,p.water-dt*p.water*.01);if(!Entities.Contains(p.entity)||Ready(p))continue;double rate=1+4*p.water;if(p.entity=="tree"){int x=i%n,y=i/n;int adjacent=Neighbors(x,y).Count(j=>Data.cells[j].entity=="tree");rate/=Math.Pow(2,adjacent);}p.growth+=dt*rate;ResolvePumpkinMaturity(p);}
        }
        void ResolvePumpkinMaturity(ClassicCell p){if(p.entity=="pumpkin"&&Ready(p)&&p.measure==0){p.measure=1;if(Random()<.2)p.entity="dead_pumpkin";}}
        public IEnumerable<int> Neighbors(int x,int y){yield return Wrap(y+1)*Data.size+x;yield return y*Data.size+Wrap(x+1);yield return Wrap(y-1)*Data.size+x;yield return y*Data.size+Wrap(x-1);}
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
            p.companion=null;if(Data.size<2||!new[]{"grass","bush","tree","carrot"}.Contains(p.entity))return;var choices=new[]{"grass","bush","tree","carrot"}.Where(e=>e!=p.entity).ToArray();p.companion=choices[(int)(Random()*choices.Length)];int dx,dy;do{dx=(int)(Random()*7)-3;dy=(int)(Random()*7)-3;}while(dx==0&&dy==0||Math.Abs(dx)+Math.Abs(dy)>3||Wrap(x+dx)==x&&Wrap(y+dy)==y);p.companionX=Wrap(x+dx);p.companionY=Wrap(y+dy);
        }
        void RefreshCompanions(){for(int i=0;i<Data.cells.Count;i++)SetCompanion(Data.cells[i],i%Data.size,i/Data.size);}
        public void Produce(string item,double amount,bool infected=false){Produce(item,amount,infected?1.0:0.0);}
        public void Produce(string item,double amount,double infectedFraction){double weird=Math.Floor(amount*Math.Max(0,Math.Min(1,infectedFraction))*.5);if(weird>0)Add("weird_substance",weird);amount-=weird;Add(item,amount);Add(Data.harvested,item,amount);}
        public bool Harvest(int x,int y)
        {
            int index=y*Data.size+x;var p=Data.cells[index];if(maze){if(index==treasure){Produce("gold",Data.size*Data.size*Math.Pow(2,Math.Max(0,Level("mazes")-1)),false);ClearMaze();return true;}ClearMaze();return false;}
            if(!Ready(p))return false;string e=p.entity,item=e=="grass"?"hay":e=="bush"||e=="tree"?"wood":e=="sunflower"?"power":e;double amount=e=="tree"?5:1;
            string upgrade=e=="grass"?"grass":e=="bush"||e=="tree"?"trees":e=="carrot"?"carrots":e=="pumpkin"?"pumpkins":e=="sunflower"?"sunflowers":e;
            amount*=Math.Pow(2,Math.Max(0,Level(upgrade)-(e=="grass"?0:1)));
            if(Level("polyculture")>0&&p.companion!=null&&Cell(p.companionX,p.companionY).entity==p.companion)amount*=ClassicBalance.CompanionMultiplier(Level("polyculture"));
            var harvested=new List<int>{index};
            if(e=="pumpkin")
            {
                int best=1,bx=x,by=y;for(int side=Data.size;side>=2;side--){bool found=false;for(int sy=Math.Max(0,y-side+1);sy<=Math.Min(y,Data.size-side)&&!found;sy++)for(int sx=Math.Max(0,x-side+1);sx<=Math.Min(x,Data.size-side);sx++){bool full=true;for(int oy=0;oy<side&&full;oy++)for(int ox=0;ox<side;ox++){var q=Cell(sx+ox,sy+oy);if(q.entity!="pumpkin"||!Ready(q)){full=false;break;}}if(full){best=side;bx=sx;by=sy;found=true;break;}}if(found)break;}
                harvested.Clear();for(int oy=0;oy<best;oy++)for(int ox=0;ox<best;ox++)harvested.Add((by+oy)*Data.size+bx+ox);amount*=best*best*Math.Min(best,6);
            }
            if(e=="cactus")
            {
                var region=new HashSet<int>();var q=new Queue<int>();region.Add(index);q.Enqueue(index);while(q.Count>0){int j=q.Dequeue();int cx=j%Data.size,cy=j/Data.size;foreach(int k in AdjacentBounded(cx,cy))if(!region.Contains(k)&&Data.cells[k].entity=="cactus"&&Ready(Data.cells[k])){region.Add(k);q.Enqueue(k);}}
                bool sorted=true;foreach(int j in region){int cx=j%Data.size,cy=j/Data.size;if(cx+1<Data.size&&region.Contains(j+1)&&Data.cells[j].measure>Data.cells[j+1].measure)sorted=false;if(cy+1<Data.size&&region.Contains(j+Data.size)&&Data.cells[j].measure>Data.cells[j+Data.size].measure)sorted=false;}
                if(sorted){harvested=region.ToList();amount*=ClassicBalance.CactusMultiplier(harvested.Count);}
            }
            if(e=="sunflower"){var all=Data.cells.Where(c=>c.entity=="sunflower").ToList();int maximum=all.Max(c=>c.measure);if(p.measure<maximum)Data.sunflowerPenalty=true;if(all.Count>=10&&p.measure==maximum&&!Data.sunflowerPenalty)amount*=8;}
            Produce(item,amount,harvested.Count(j=>Data.cells[j].infected)/(double)harvested.Count);foreach(int j in harvested){var cell=Data.cells[j];cell.entity=cell.ground=="grassland"?"grass":"";cell.growth=0;cell.companion=null;cell.infected=false;cell.measure=0;SetCompanion(cell,j%Data.size,j/Data.size);}if(e=="sunflower"&&!Data.cells.Any(c=>c.entity=="sunflower"))Data.sunflowerPenalty=false;return true;
        }
        IEnumerable<int> AdjacentBounded(int x,int y){if(x>0)yield return y*Data.size+x-1;if(x+1<Data.size)yield return y*Data.size+x+1;if(y>0)yield return (y-1)*Data.size+x;if(y+1<Data.size)yield return (y+1)*Data.size+x;}
        public bool Use(string item,int x,int y,int amount=1)
        {
            if(amount<1||Count(item)<amount)return false;var p=Cell(x,y);if(item=="water"){Add(item,-amount);p.water=Math.Min(1,p.water+.25*amount);return true;}
            if(item=="fertilizer"){if(!Entities.Contains(p.entity))return false;Add(item,-amount);p.growth+=2*amount;p.infected=true;ResolvePumpkinMaturity(p);return true;}
            if(item=="weird_substance")
            {
                if(p.entity=="bush"&&Level("mazes")>0){int scale=1<<Math.Max(0,Level("mazes")-1);if(amount<Data.size*scale)return false;Add(item,-amount);MakeMaze(x,y);return true;}
                if(maze&&x+y*Data.size==treasure){int scale=1<<Math.Max(0,Level("mazes")-1);if(amount<Data.size*scale||mazeReuses>=300)return false;Add(item,-amount);Produce("gold",Data.size*Data.size*scale,false);mazeReuses++;int old=treasure;for(int i=0;i<Math.Max(1,Data.size/2);i++){int a=(int)(Random()*Data.cells.Count),dir=(int)(Random()*4);int b=NeighborIndex(a,dir,false);if(b>=0){walls[a]&=~(1<<dir);walls[b]&=~(1<<((dir+2)%4));}}treasure=ChooseTreasure(old);Data.cells[old].entity="hedge";Data.cells[treasure].entity="treasure";return true;}
                Add(item,-amount);p.infected=!p.infected;foreach(int j in Neighbors(x,y).Distinct().Where(j=>j!=y*Data.size+x))Data.cells[j].infected=!Data.cells[j].infected;return true;
            }
            return false;
        }
        public int NeighborIndex(int index,int direction,bool wrap){int x=index%Data.size,y=index/Data.size;int[] dx={0,1,0,-1},dy={1,0,-1,0};x+=dx[direction];y+=dy[direction];if(!wrap&&(x<0||y<0||x>=Data.size||y>=Data.size))return -1;return Wrap(y)*Data.size+Wrap(x);}
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
        public bool CanMove(int index,int direction,bool snake=false){if(maze&&(walls[index]&(1<<direction))!=0)return false;int target=NeighborIndex(index,direction,!maze&&!snake);return target>=0&&(!snake||!tail.Take(index==apple?tail.Count:Math.Max(0,tail.Count-1)).Contains(target));}
        public object Companion(int x,int y){var p=Cell(x,y);return Level("polyculture")==0||p.companion==null?null:new object[]{p.companion,new object[]{(double)p.companionX,(double)p.companionY}};}
        public ClassicFarm Snapshot(){return new ClassicFarm{Data=JsonUtility.FromJson<ClassicSave>(JsonUtility.ToJson(Data)),maze=maze,walls=walls==null?null:(int[])walls.Clone(),treasure=treasure,mazeReuses=mazeReuses,tail=new List<int>(tail),dinosaur=dinosaur,apple=apple};}
        public void Say(string text){if(Message!=null)Message(text);}
    }
}
