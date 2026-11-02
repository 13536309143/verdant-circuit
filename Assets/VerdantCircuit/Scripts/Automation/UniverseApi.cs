using System;
using System.Linq;
using System.Collections.Generic;
namespace Verdant.Automation
{
    public sealed partial class ArchipelagoWorld
    {
        public static readonly string[] UniverseApi={"mothership_inventory","planet_technologies","planet_research","universe_planets","universe_state","universe_run","planet_run","planet_stop","planet_self","planet_inventory","planet_products","planet_surface","planet_expand","planet_scan","worker_build","worker_starter","worker_units","worker_self","worker_move","worker_route","worker_path","worker_work","worker_cancel","worker_remove","worker_research","worker_rotate","worker_wait","worker_state","worker_send","worker_receive","worker_result","swarm_create","swarm_run","swarm_stop","swarm_members","ufo_build","ufo_transfer","ufo_fleet","ufo_cancel","ufo_research","ufo_refuel","ufo_reposition","ufo_capacity","ufo_wait","get_world_width","get_world_height","get_longitude","get_latitude","harvest_area","plant_area","surface_distance","market_prices","market_sell","contract_current","contract_submit","stop_all"};
        public object UniverseInvoke(string name,object[] a,int id=-1,int planet=0)
        {
            var unit=Unit(id);if(unit!=null)planet=unit.island;
            switch(name)
            {
                case "mothership_inventory":Call.Require(a,1);if(!Goods.Contains(S(a[0])))throw new Exception("Unknown item identifier.");return Stock(-1,S(a[0]));
                case "planet_technologies":Call.Require(a,1);return Technologies(N(a[0]));
                case "planet_research":Call.Require(a,2);return ResearchPlanet(N(a[0]),S(a[1]));
                case "universe_state":Call.Require(a,0);return State();
                case "universe_planets":Call.Require(a,0);return Islands();
                case "universe_run":Call.Require(a,1);return RunControl("network",S(a[0]));
                case "planet_run":Call.Require(a,2);return RunControl("island:"+N(a[0]),S(a[1]));
                case "planet_stop":Call.Require(a,1);return StopControl("island:"+N(a[0]));
                case "planet_self":Call.Require(a,0);return (double)(unit?.island??planet);
                case "planet_inventory":Call.Require(a,2);CheckPlanet(N(a[0]));if(!Goods.Contains(S(a[1])))throw new Exception("Unknown item identifier.");return Stock(N(a[0]),S(a[1]));
                case "planet_products":Call.Require(a,1);CheckPlanet(N(a[0]));return Recipes.Where(r=>r.island==N(a[0])).Select(r=>(object)r.item).ToList();
                case "planet_surface":Call.Require(a,1);CheckPlanet(N(a[0]));return SurfaceState(N(a[0]));
                case "planet_expand":Call.Require(a,1);return ExpandPlanet(N(a[0]));
                case "planet_scan":Call.Require(a,3);int p=N(a[0]),x=N(a[1]),y=N(a[2]);if(!Terrain(p,x,y))return null;return Dict("planet",(double)p,"x",(double)x,"y",(double)y,"longitude",Longitude(p,x),"latitude",Latitude(p,y),"resource",p==3?Deposit(x,y):p==2?Shoal(x,y):p==0?farm.Cell(x,y).entity:"flower_bed");
                case "worker_starter":Call.Require(a,1);return (double)EnsurePlanetWorker(N(a[0]));
                case "worker_build":if(a.Length!=1&&a.Length!=3)throw new Exception("worker_build expects planet or planet,width,height.");int target=N(a[0]);return target>=0&&target<4?(double)Build(Roles[target],a.Length==1?1:N(a[1]),a.Length==1?1:N(a[2]),target):-1.0;
                case "worker_units":if(a.Length>1)throw new Exception("worker_units expects zero or one planet.");return a.Length==0?Units():((List<object>)Units()).Where(v=>N(((Dictionary<object,object>)v)["planet"])==N(a[0])).ToList();
                case "worker_self":Call.Require(a,0);return (double)id;
                case "worker_move":Call.Require(a,2);return Invoke("fleet_step",a);
                case "worker_route":Call.Require(a,3);return Invoke("fleet_route",a);
                case "worker_path":Call.Require(a,3);return Invoke("fleet_plan",a);
                case "worker_work":Call.Require(a,3);var w=Unit(N(a[0]));return w!=null&&Work(w.id,w.island,S(a[1]),N(a[2]));
                case "worker_cancel":Call.Require(a,1);return Cancel(N(a[0]));
                case "worker_remove":Call.Require(a,1);return Remove(N(a[0]));
                case "worker_research":Call.Require(a,2);return ResearchWorker(N(a[0]),S(a[1]));
                case "worker_rotate":return Invoke("fleet_rotate",a);
                case "worker_state":Call.Require(a,1);return ((List<object>)Units()).FirstOrDefault(v=>N(((Dictionary<object,object>)v)["id"])==N(a[0]));
                case "worker_send":Call.Require(a,2);return Send(N(a[0]),a[1]);
                case "worker_receive":Call.Require(a,0);return Receive(id);
                case "worker_result":Call.Require(a,1);return Result(N(a[0]));
                case "swarm_create":return UnifiedInvoke("group_create",a);
                case "swarm_run":return UnifiedInvoke("group_run",a);
                case "swarm_stop":return UnifiedInvoke("group_stop",a);
                case "swarm_members":return UnifiedInvoke("group_units",a);
                case "ufo_build":Call.Require(a,0);return (double)BuildUfo();
                case "ufo_transfer":Call.Require(a,5);return LaunchUfo(N(a[0]),N(a[1]),N(a[2]),S(a[3]),N(a[4]));
                case "ufo_fleet":Call.Require(a,0);return Ufos();
                case "ufo_cancel":Call.Require(a,1);return CancelUfo(N(a[0]));
                case "ufo_research":Call.Require(a,1);return ResearchUfo(S(a[0]));
                case "ufo_reposition":Call.Require(a,2);return RepositionUfo(N(a[0]),N(a[1]));
                case "ufo_refuel":Call.Require(a,2);return RefuelUfo(N(a[0]),N(a[1]));
                case "ufo_capacity":Call.Require(a,0);return (double)UfoCapacity;
                case "get_world_width":Call.Require(a,0);CheckPlanet(planet);return (double)SurfaceWidth(planet);
                case "get_world_height":Call.Require(a,0);CheckPlanet(planet);return (double)SurfaceHeight(planet);
                case "get_longitude":Call.Require(a,0);if(unit==null)throw new Exception("A bound worker is required.");return Longitude(planet,unit.x);
                case "get_latitude":Call.Require(a,0);if(unit==null)throw new Exception("A bound worker is required.");return Latitude(planet,unit.y);
                case "harvest_area":Call.Require(a,0);if(unit==null||planet!=0)throw new Exception("Earth drone required.");int count=0;for(int dy=0;dy<unit.height;dy++)for(int dx=0;dx<unit.width;dx++)if(farm.Harvest((unit.x+dx)%SurfaceWidth(0),unit.y+dy))count++;return (double)count;
                case "plant_area":Call.Require(a,1);if(unit==null||planet!=0)throw new Exception("Earth drone required.");int planted=0;for(int dy=0;dy<unit.height;dy++)for(int dx=0;dx<unit.width;dx++)if(farm.Plant(S(a[0]),(unit.x+dx)%SurfaceWidth(0),unit.y+dy))planted++;return (double)planted;
                case "surface_distance":Call.Require(a,5);int world=N(a[0]);CheckPlanet(world);int ax=N(a[1]),ay=N(a[2]),bx=N(a[3]),by=N(a[4]);if(!Terrain(world,ax,ay)||!Terrain(world,bx,by))throw new Exception("Locked coordinate.");int horizontal=Math.Abs(ax-bx);if(SurfaceWidth(world)==360)horizontal=Math.Min(horizontal,360-horizontal);return (double)(horizontal+Math.Abs(ay-by));
                case "market_prices":return Invoke("fleet_market",a);
                case "market_sell":return Invoke("fleet_sell",a);
                case "contract_current":return Invoke("fleet_order",a);
                case "contract_submit":return Invoke("fleet_submit",a);
                case "stop_all":Call.Require(a,0);HaltAll();return true;
                default:throw new Exception("Unknown universe function: "+name);
            }
        }
        void CheckPlanet(int p){if(!IsUnlocked(p))throw new Exception("Planet is locked or unknown.");}
    }
}
