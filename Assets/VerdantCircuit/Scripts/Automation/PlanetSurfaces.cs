using System;
using System.Linq;
using System.Collections.Generic;

namespace Verdant.Automation
{
    public sealed partial class ArchipelagoWorld
    {
        public event Action<int,int,int,string,double> ProductionCompleted;
        public int SurfaceWidth(int p){return p==0?farm.Data.size:Data.islands[p].width;}
        public int SurfaceHeight(int p){return p==0?farm.Height:Data.islands[p].height;}
        public int WorkerLimit(int p){return p==0?Math.Max(farm.MaxDrones,1+Data.islands[p].slotTier):1+Data.islands[p].slotTier;}
        public int HullTier(int p){return Data.islands[p].hullTier;}
        public double WorkerPrice(int w,int h){return 10+5*w*h;}
        public bool HullAllowed(int p,int w,int h){for(int t=0;t<=HullTier(p);t++)if((Sizes[t,0]==w&&Sizes[t,1]==h)||(Sizes[t,1]==w&&Sizes[t,0]==h))return w<=SurfaceWidth(p)&&h<=SurfaceHeight(p);return false;}
        public int EnsurePlanetWorker(int p)
        {
            if(!IsUnlocked(p))return -1;var unit=Data.units.FirstOrDefault(u=>u.island==p);if(unit!=null)return unit.id;
            var starter=new FleetUnit{id=Data.nextId++,island=p,role=Roles[p],freeStarter=true,manual=true};Data.units.Add(starter);return starter.id;
        }
        public bool ResearchWorker(int p,string kind)
        {
            if(!IsUnlocked(p))return false;var surface=Data.islands[p];int level=kind=="slots"?surface.slotTier:surface.hullTier;
            if(kind!="slots"&&kind!="size"||level>=(kind=="slots"?7:4))return false;
            string material=p==0?"hay":p==1?"rose":p==2?"carp":"stone";int cost=(kind=="slots"?25:40)*(level+1);
            if(Stock(p,material)<cost)return false;
            if(kind=="size"&&(Sizes[level+1,0]>SurfaceWidth(p)||Sizes[level+1,1]>SurfaceHeight(p)))return false;
            Add(p,material,-cost);Put(Data.consumed,material,cost);if(kind=="slots")surface.slotTier++;else surface.hullTier++;return true;
        }
        public bool ExpandPlanet(int p)
        {
            if(!IsUnlocked(p))return false;if(p==0)return farm.Unlock("expand");var s=Data.islands[p];if(s.expansion>=9)return false;
            string item=p==1?"rose":p==2?"carp":"stone";int price=20*(s.expansion+1)*(s.expansion+1);
            if(Stock(p,item)<price)return false;Add(p,item,-price);Put(Data.consumed,item,price);int oldWidth=s.width,oldHeight=s.height;s.expansion++;s.width=ClassicFarm.SurfaceStages[s.expansion,0];s.height=ClassicFarm.SurfaceStages[s.expansion,1];int dx=(s.width-oldWidth)/2,dy=(s.height-oldHeight)/2;foreach(var u in Data.units.Where(u=>u.island==p)){u.x=u.cropX+=dx;u.y=u.cropY+=dy;foreach(var j in u.jobs){if(j.kind=="route"){j.target+=dx;j.source+=dy;}if(j.workX>=0){j.workX+=dx;j.workY+=dy;}}}return true;
        }
        public double Longitude(int p,int x){if(x<0||x>=SurfaceWidth(p))throw new Exception("Longitude cell is locked.");return x+.5-SurfaceWidth(p)*.5;}
        public double Latitude(int p,int y){if(y<0||y>=SurfaceHeight(p))throw new Exception("Latitude cell is locked.");return y+.5-SurfaceHeight(p)*.5;}
        public string Deposit(int x,int y){return new[]{"stone","copper_ore","tin_ore","silver_ore","gold_ore","platinum_ore","rough_diamond"}[(x+3*y)%7];}
        public string Shoal(int x,int y){return new[]{"carp","trout","salmon","cod","tuna","koi"}[(x+3*y)%6];}
        bool AvailableRecipe(FleetRecipe r)
        {
            if(r.island==1){int index=Array.IndexOf(new[]{"rose","tulip","lavender","lily","orchid","lotus"},r.item);return index<0||index==0||SurfaceWidth(1)>= (index<3?2:4);}
            if(r.island==2&&r.item!="fishmeal")return Enumerable.Range(0,SurfaceHeight(2)).Any(y=>Enumerable.Range(0,SurfaceWidth(2)).Any(x=>Shoal(x,y)==r.item));
            if(r.island==3&&(r.item=="stone"||r.item.EndsWith("_ore")||r.item=="rough_diamond"))return Enumerable.Range(0,SurfaceHeight(3)).Any(y=>Enumerable.Range(0,SurfaceWidth(3)).Any(x=>Deposit(x,y)==r.item));
            return true;
        }
        public object SurfaceState(int p)
        {
            if(!IsUnlocked(p))return null;return Dict("planet",(double)p,"width",(double)SurfaceWidth(p),"height",(double)SurfaceHeight(p),"max_width",360.0,"max_height",128.0,"latitude_min",-64.0,"latitude_max",64.0,"longitude_wrap",SurfaceWidth(p)==360,"worker_limit",(double)WorkerLimit(p),"hull_tier",(double)HullTier(p),"worker_type",Roles[p]);
        }
        public bool Terrain(int p,int x,int y){return IsUnlocked(p)&&x>=0&&x<SurfaceWidth(p)&&y>=0&&y<SurfaceHeight(p);}
        public bool Fits(FleetUnit u,int x,int y,bool traffic=true)
        {
            int w=SurfaceWidth(u.island),h=SurfaceHeight(u.island);if(x<0||x>=w||y<0||y+u.height>h||w<360&&x+u.width>w)return false;
            if(!traffic)return true;
            foreach(var other in Data.units.Where(v=>v!=u&&v.island==u.island))for(int dy=0;dy<u.height;dy++)for(int dx=0;dx<u.width;dx++)for(int oy=0;oy<other.height;oy++)for(int ox=0;ox<other.width;ox++)if((x+dx)%w==(other.x+ox)%w&&y+dy==other.y+oy)return false;
            return true;
        }
        bool FindParking(FleetUnit u,out int x,out int y){for(y=0;y<SurfaceHeight(u.island);y++)for(x=0;x<SurfaceWidth(u.island);x++)if(Fits(u,x,y))return true;x=y=0;return false;}
        public List<int> Path(FleetUnit u,int tx,int ty,bool traffic=true)
        {
            var result=new List<int>();int w=SurfaceWidth(u.island);if(!Fits(u,tx,ty,traffic))return result;int start=u.y*w+u.x,end=ty*w+tx;if(start==end)return result;
            var queue=new Queue<int>();var previous=new Dictionary<int,int>{{start,-1}};queue.Enqueue(start);
            while(queue.Count>0){int cell=queue.Dequeue();int x=cell%w,y=cell/w;foreach(var delta in new[]{new[]{1,0},new[]{0,1},new[]{-1,0},new[]{0,-1}}){int nx=x+delta[0],ny=y+delta[1];if(w==360)nx=(nx+w)%w;int next=ny*w+nx;if(!Fits(u,nx,ny,traffic)||previous.ContainsKey(next))continue;previous[next]=cell;if(next==end){for(int cur=end;cur!=start;cur=previous[cur])result.Add(cur);result.Reverse();return result;}queue.Enqueue(next);}}return result;
        }
        public bool ManualStep(int id,int dx,int dy)
        {
            var u=Unit(id);if(u==null||u.jobs.Count>0||u.delay>0||Math.Abs(dx)+Math.Abs(dy)!=1)return false;int x=u.x+dx,y=u.y+dy;if(SurfaceWidth(u.island)==360)x=(x+360)%360;
            if(!Fits(u,x,y)){u.status="blocked";return false;}if(u.island==0&&!farm.CanMove(u.y*farm.Data.size+u.x,dx==1?1:dx==-1?3:dy==1?0:2))return false;
            u.x=u.cropX=x;u.y=u.cropY=y;u.delay=(.18+.04*(u.width*u.height-1))/(u.island==0?farm.Speed:1);Data.totalDistance++;return true;
        }
        public bool Work(int id,int planet,string item,int batches)
        {
            var u=Unit(id);var recipe=Recipes.FirstOrDefault(r=>r.island==planet&&r.item==item);if(u==null||u.island!=planet||!IsUnlocked(planet)||recipe==null||!recipe.roles.Contains(u.role)||!AvailableRecipe(recipe)||batches<1||batches>10000||u.jobs.Count>=64)return false;
            u.jobs.Add(new FleetJob{kind="work",target=planet,item=item,remaining=batches});return true;
        }
        public bool Ship(int id,int source,int destination,string item,int amount){return false;}
        public bool Rebase(int id,int destination){return Unit(id)!=null&&Unit(id).island==destination;}
        public void HaltAll(){StopAllControls();foreach(var u in Data.units.ToArray()){StopProgram(u.id);Cancel(u.id);}foreach(var ufo in Data.ufos)CancelUfo(ufo.id);}
        public void Advance(double dt)
        {
            if(dt<=0)return;Data.accumulator+=dt;while(Data.accumulator>=.25){Data.accumulator-=.25;Data.time+=.25;Data.turn++;if(!ProgramsPaused){TickControls();TickPrograms();}TickUfos();
                foreach(var u in Data.units.ToArray()){u.delay=Math.Max(0,u.delay-.25);TickUnit(u);}Data.orderPeakHull=Math.Max(Data.orderPeakHull,Data.units.Where(u=>u.jobs.Count>0||ProgramRunning(u.id)).Select(u=>u.width*u.height).DefaultIfEmpty(1).Max());Data.orderPeakPrograms=Math.Max(Data.orderPeakPrograms,programs.Count);Data.peakPrograms=Math.Max(Data.peakPrograms,programs.Count);
            }
        }
        void TickUnit(FleetUnit u)
        {
            if(u.jobs.Count==0){u.status=ProgramRunning(u.id)?"running":"idle";return;}var job=u.jobs[0];
            if(job.kind=="route")
            {
                if(u.x==job.target&&u.y==job.source){u.jobs.RemoveAt(0);return;}if(u.delay>0)return;var path=Path(u,job.target,job.source);if(path.Count==0){u.status="traffic_queue";return;}int w=SurfaceWidth(u.island);u.x=u.cropX=path[0]%w;u.y=u.cropY=path[0]/w;u.delay=.2+.04*(u.width*u.height-1);u.status="moving";return;
            }
            if(job.workX<0){job.workX=u.x;job.workY=u.y;bool mineral=u.island==3&&(job.item=="stone"||job.item.EndsWith("_ore")||job.item=="rough_diamond"),fish=u.island==2&&job.item!="fishmeal";if(mineral||fish){bool found=false;for(int y=0;y<SurfaceHeight(u.island)&&!found;y++)for(int x=0;x<SurfaceWidth(u.island);x++)if((mineral?Deposit(x,y):Shoal(x,y))==job.item&&Fits(u,x,y,false)){job.workX=x;job.workY=y;found=true;break;}if(!found){u.status="resource_outside_extent";return;}}}
            if(u.x!=job.workX||u.y!=job.workY){if(u.delay>0)return;var path=Path(u,job.workX,job.workY);if(path.Count==0){u.status="traffic_queue";return;}int w=SurfaceWidth(u.island);u.x=u.cropX=path[0]%w;u.y=u.cropY=path[0]/w;u.delay=.2+.04*(u.width*u.height-1);u.status="moving";return;}
            var r=Recipes.First(v=>v.item==job.item&&v.island==u.island);int area=u.width*u.height;
            if(job.phase==0){if(r.inputs.Any(p=>Stock(u.island,p.Key)<p.Value*area)){u.status="waiting_inputs";return;}foreach(var input in r.inputs){Add(u.island,input.Key,-input.Value*area);Put(Data.consumed,input.Key,input.Value*area);}job.timer=r.seconds*(.8+.2*Math.Sqrt(area))/ProductionSpeed(u.island,r.item);job.phase=1;}
            u.status="working";job.timer-=.25;if(job.timer>0)return;int output=ProductionOutput(r,area);Add(u.island,r.item,output);Put(Data.islands[u.island].made,r.item,output);ProductionCompleted?.Invoke(u.island,u.x,u.y,r.item,output);Data.completedJobs++;job.phase=0;if(--job.remaining<=0)u.jobs.RemoveAt(0);
        }
    }
}
