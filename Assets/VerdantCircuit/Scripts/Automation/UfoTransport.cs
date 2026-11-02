using System;
using System.Linq;
using System.Collections.Generic;
namespace Verdant.Automation
{
    [Serializable] public sealed class UfoVessel
    {
        public int id,planet,source,destination,remaining,reserved,cargo,phase,legTarget;public string item="",status="docked";
        public double timer,legDuration,battery=120;public bool recall;
    }
    public sealed partial class ArchipelagoWorld
    {
        public const int Mothership=-1;
        public int UfoCapacity {get{return new[]{24,72,144,288}[Data.ufoCargoTier];}}
        public bool DepotUnlocked(int p){return p==-1?Data.started:IsUnlocked(p);}
        public double FlightTime(int a,int b){return (a==-1||b==-1?10+3*(Math.Max(a,b)+1):20+3*(a+b+2))/(1+.15*PlanetLevel(-1,"navigation")) + 4/(1+.2*PlanetLevel(-1,"docking"));}
        public int FlightFuel(int a,int b){return a==b?0:(a==-1||b==-1?2:4);}
        public int BuildUfo()
        {
            if(!Data.started||Data.ufos.Count>=1+Data.ufoSlots||Data.credits<50||farm.Count("wood")<20||farm.Count("solar_energy")<10)return -1;
            Data.credits-=50;Data.buildSpent+=50;Add(0,"wood",-20);Put(Data.consumed,"wood",20);Add(0,"solar_energy",-10);Put(Data.consumed,"solar_energy",10);var u=new UfoVessel{id=Data.nextUfoId++};Data.ufos.Add(u);return u.id;
        }
        public bool LaunchUfo(int id,int from,int to,string item,int amount)
        {
            var u=Data.ufos.FirstOrDefault(v=>v.id==id);
            if(u==null||u.phase!=0||from==to||!DepotUnlocked(from)||!DepotUnlocked(to)||!Goods.Contains(item)||amount<1||amount>1000000||Stock(from,item)<amount)return false;
            int trips=(int)Math.Ceiling(amount/(double)UfoCapacity);
            int fuel=FlightFuel(u.planet,from)+trips*FlightFuel(from,to)+(trips-1)*FlightFuel(to,from);
            if(u.battery<fuel)return false;u.battery-=fuel;
            Add(from,item,-amount);u.item=item;u.source=from;u.destination=to;u.remaining=u.reserved=amount;u.cargo=0;u.recall=false;
            if(u.planet==from)LoadUfo(u);else RouteUfo(u,u.planet!=-1&&from!=-1?-1:from,1);return true;
        }
        void RouteUfo(UfoVessel u,int target,int phase)
        {
            u.legTarget=target;u.phase=phase;u.status=phase==1?"repositioning":phase==2?"mothership_transfer":"delivering";
            u.legDuration=u.timer=FlightTime(u.planet,target);
        }
        void LoadUfo(UfoVessel u)
        {
            int amount=Math.Min(UfoCapacity,u.reserved);u.reserved-=amount;u.cargo=amount;
            RouteUfo(u,u.planet!=-1&&u.destination!=-1?-1:u.destination,u.planet!=-1&&u.destination!=-1?2:3);
        }
        public bool CancelUfo(int id)
        {
            var u=Data.ufos.FirstOrDefault(v=>v.id==id);if(u==null)return false;
            if(u.reserved>0){Add(u.source,u.item,u.reserved);u.reserved=0;}
            if(u.phase==0)return true;u.recall=true;u.remaining=u.cargo;u.status="finishing_current_leg";return true;
        }
        public bool ResearchUfo(string kind)
        {
            if(!Data.started)return false;int level=kind=="cargo"?Data.ufoCargoTier:Data.ufoSlots;if(kind!="cargo"&&kind!="slots"||level>=3)return false;
            string material=level==0?"bronze":level==1?"silver":"platinum";int amount=10*(level+1);if(Stock(0,material)<amount)return false;Add(0,material,-amount);Put(Data.consumed,material,amount);if(kind=="cargo")Data.ufoCargoTier++;else Data.ufoSlots++;return true;
        }
        public bool RepositionUfo(int id,int destination)
        {
            var u=Data.ufos.FirstOrDefault(v=>v.id==id);if(u==null||u.phase!=0||!DepotUnlocked(destination)||u.planet==destination||u.battery<FlightFuel(u.planet,destination))return false;
            u.battery-=FlightFuel(u.planet,destination);u.source=destination;u.destination=destination;u.reserved=u.cargo=u.remaining=0;u.recall=false;
            RouteUfo(u,u.planet!=-1&&destination!=-1?-1:destination,1);return true;
        }
        public bool RefuelUfo(int id,int amount)
        {var u=Data.ufos.FirstOrDefault(v=>v.id==id);if(u==null||u.phase!=0||amount<1||amount>200||Stock(u.planet,"solar_energy")<amount||u.battery+amount*5>1000)return false;Add(u.planet,"solar_energy",-amount);Put(Data.consumed,"solar_energy",amount);u.battery+=amount*5;return true;}
        void DockUfo(UfoVessel u){u.phase=0;u.status="docked";u.item="";u.remaining=u.reserved=u.cargo=0;u.timer=0;}
        void TickUfos()
        {
            foreach(var u in Data.ufos)
            {
                if(u.phase==0)continue;u.timer-=.25;if(u.timer>0)continue;
                u.planet=u.legTarget;if(u.planet==-1)Data.hubVisits++;
                if(u.phase==1)
                {
                    if(u.recall){DockUfo(u);continue;}
                    if(u.planet!=u.source){RouteUfo(u,u.source,1);continue;}
                    if(u.remaining==0){DockUfo(u);continue;}LoadUfo(u);continue;
                }
                if(u.phase==2&&!u.recall){RouteUfo(u,u.destination,3);continue;}
                if(u.cargo>0){Add(u.planet,u.item,u.cargo);if(u.planet>=0)Put(Data.islands[u.planet].delivered,u.item,u.cargo);u.remaining-=u.cargo;Data.shipments++;u.cargo=0;}
                if(u.reserved>0&&!u.recall)RouteUfo(u,u.planet!=-1&&u.source!=-1?-1:u.source,1);else DockUfo(u);
            }
        }
        public object Ufos(){return Data.ufos.Select(u=>(object)Dict("id",(double)u.id,"planet",(double)u.planet,"source",(double)u.source,"destination",(double)u.destination,"leg_target",(double)u.legTarget,"item",u.item,"cargo",(double)u.cargo,"reserved",(double)u.reserved,"remaining",(double)u.remaining,"battery",u.battery,"eta",Math.Max(0,u.timer),"capacity",(double)UfoCapacity,"status",u.status,"busy",u.phase!=0)).ToList();}
    }
}
