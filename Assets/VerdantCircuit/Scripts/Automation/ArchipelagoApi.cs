using System;
using System.Linq;
using System.Collections.Generic;

namespace Verdant.Automation
{
    public sealed partial class ArchipelagoWorld
    {
        static string[] api;public static string[] Api {get{return api??(api=new[]{"fleet_start","fleet_state","fleet_order","fleet_units","fleet_stock","fleet_build","fleet_remove","fleet_research","fleet_work","fleet_route","fleet_step","fleet_rotate","fleet_cancel","fleet_submit","fleet_wait","fleet_plan","fleet_run","fleet_stop","fleet_send","fleet_receive","fleet_self","fleet_result","fleet_map","fleet_halt_all","fleet_market","fleet_sell"}.Concat(UnifiedApi).Concat(NatureFunctions).Concat(UniverseApi).ToArray());}}
        static Dictionary<object,object> Dict(params object[] items){var result=new Dictionary<object,object>();for(int i=0;i<items.Length;i+=2)result[items[i]]=items[i+1];return result;}
        public object State(){return Dict("started",Data.started,"order",(double)Data.order,"total_orders",(double)Contracts.Length,"complete",Complete,"credits",Data.credits,"slot_limit",(double)SlotLimit,"size_tier",(double)Data.sizeLevel,"planets",(double)DiscoveredCount,"islands",(double)DiscoveredCount,"unlocked_islands",Enumerable.Range(0,Definitions.Length).Where(IsUnlocked).Select(i=>(object)(double)i).ToList(),"time",Data.time,"shipments",(double)Data.shipments,"hub_visits",(double)Data.hubVisits,"mothership",Data.mothership.ToDictionary(s=>(object)s.id,s=>(object)s.amount),"fees",Data.fees,"roles",Data.roles.Select(r=>(object)r).ToList());}
        public object Order()
        {
            if(Complete)return null;
            return Dict("index",(double)Data.order,"name",Current.en,"reward",(double)Current.reward,"required_hull_area",(double)RequiredHull,"required_programs",(double)RequiredPrograms,"required_flights",(double)RequiredFlights,"ready",Ready,"goals",Current.goals.Select(g=>(object)Dict("item",g.item,"planet",(double)g.island,"island",(double)g.island,"amount",(double)g.amount,"stock",Stock(g.island,g.item))).ToList());
        }
        public object Units(){return Data.units.Select(u=>(object)Dict("id",(double)u.id,"planet",(double)u.island,"role",u.role,"island",(double)u.island,"x",(double)u.x,"y",(double)u.y,"width",(double)u.width,"height",(double)u.height,"capacity",(double)Capacity(u),"jobs",(double)u.jobs.Count,"status",u.status,"cargo",(double)u.cargo,"cargo_item",u.cargoItem,"flying",u.flightTarget>=0,"program",u.program,"program_running",ProgramRunning(u.id),"code_error",u.codeError,"inbox",(double)InboxCount(u.id),"shipment_target",u.jobs.Count>0&&u.jobs[0].kind=="ship"?(double)u.jobs[0].target:-1.0,"shipment_item",u.jobs.Count>0&&u.jobs[0].kind=="ship"?u.jobs[0].item:"","shipment_remaining",u.jobs.Count>0&&u.jobs[0].kind=="ship"?(double)u.jobs[0].remaining:0.0,"production_item",u.jobs.Count>0&&u.jobs[0].kind=="work"&&u.jobs[0].phase==1?u.jobs[0].item:"","production_amount",u.jobs.Count>0&&u.jobs[0].kind=="work"&&u.jobs[0].phase==1?(double)ProductionOutput(Recipes.First(r=>r.island==u.jobs[0].target&&r.item==u.jobs[0].item),u.width*u.height):0.0)).ToList();}
        static int N(object value){return PythonUtilities.Integer(value);}static string S(object value){return Convert.ToString(value);}
        public object Invoke(string name,object[] a)
        {
            if(UniverseApi.Contains(name))return UniverseInvoke(name,a);
            if(UnifiedApi.Contains(name)||NatureFunctions.Contains(name))return UnifiedInvoke(name,a);
            switch(name)
            {
                case "fleet_map":Call.Require(a,1);return Map(N(a[0]));
                case "fleet_run":Call.Require(a,2);return RunProgram(N(a[0]),S(a[1]));
                case "fleet_result":Call.Require(a,1);return Result(N(a[0]));
                case "fleet_stop":Call.Require(a,1);return StopProgram(N(a[0]));
                case "fleet_send":Call.Require(a,2);return Send(N(a[0]),a[1]);
                case "fleet_receive":Call.Require(a,0);return null;
                case "fleet_self":Call.Require(a,0);return -1.0;
                case "fleet_halt_all":Call.Require(a,0);HaltAll();return true;
                case "fleet_market":Call.Require(a,0);return ExportPrices.ToDictionary(p=>(object)p.Key,p=>(object)p.Value);
                case "fleet_sell":Call.Require(a,3);return Sell(N(a[0]),S(a[1]),N(a[2]));
                case "fleet_start":Call.Require(a,0);return Start();
                case "fleet_state":Call.Require(a,0);return State();
                case "fleet_order":Call.Require(a,0);return Order();
                case "fleet_units":Call.Require(a,0);return Units();
                case "fleet_stock":Call.Require(a,2);if(!IsUnlocked(N(a[0]))||!Goods.Contains(S(a[1])))throw new Exception("Unknown or locked island / cargo item.");return Stock(N(a[0]),S(a[1]));
                case "fleet_build":Call.Require(a,4);return (double)Build(S(a[0]),N(a[1]),N(a[2]),N(a[3]));
                case "fleet_remove":Call.Require(a,1);return Remove(N(a[0]));
                case "fleet_research":Call.Require(a,1);return Research(S(a[0]));
                case "fleet_work":Call.Require(a,4);return Work(N(a[0]),N(a[1]),S(a[2]),N(a[3]));
                case "fleet_ship":Call.Require(a,5);return Ship(N(a[0]),N(a[1]),N(a[2]),S(a[3]),N(a[4]));
                case "fleet_rebase":Call.Require(a,2);return Rebase(N(a[0]),N(a[1]));
                case "fleet_cancel":Call.Require(a,1);return Cancel(N(a[0]));
                case "fleet_submit":Call.Require(a,0);return Submit();
                case "fleet_route":Call.Require(a,3);var routeUnit=Unit(N(a[0]));if(routeUnit==null||routeUnit.jobs.Count>=64||!Fits(routeUnit,N(a[1]),N(a[2]),false))return false;routeUnit.manual=false;routeUnit.jobs.Add(new FleetJob{kind="route",target=N(a[1]),source=N(a[2])});return true;
                case "fleet_plan":Call.Require(a,3);var planner=Unit(N(a[0]));if(planner==null)return new List<object>();return Path(planner,N(a[1]),N(a[2]),false).Select(p=>(object)new object[]{(double)(p%SurfaceWidth(planner.island)),(double)(p/SurfaceWidth(planner.island))}).ToList();
                case "fleet_step":Call.Require(a,2);string direction=S(a[1]);return ManualStep(N(a[0]),direction=="East"?1:direction=="West"?-1:0,direction=="North"?1:direction=="South"?-1:0);
                case "fleet_rotate":Call.Require(a,1);var u=Unit(N(a[0]));if(u==null||u.jobs.Count>0||u.flightTarget>=0)return false;u.manual=true;int width=u.width;u.width=u.height;u.height=width;if(!Fits(u,u.x,u.y)){width=u.width;u.width=u.height;u.height=width;return false;}return true;
                default:throw new Exception("Unknown fleet operation: "+name);
            }
        }
    }
}
