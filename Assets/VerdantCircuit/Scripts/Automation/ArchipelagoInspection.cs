using System;
using System.Linq;
namespace Verdant.Automation
{
    public sealed partial class ArchipelagoWorld
    {
        public object Map(int p){CheckPlanet(p);return Dict("planet",(double)p,"width",(double)SurfaceWidth(p),"height",(double)SurfaceHeight(p),"surface",SurfaceState(p),"worker_type",Roles[p]);}
        public string Audit(){try{Validate(Data);}catch(Exception e){return e.Message;}foreach(var u in Data.units)if(!Fits(u,u.x,u.y))return "Overlapping worker footprint: "+u.id;double rewards=Contracts.Take(Data.order).Sum(c=>c.reward);if(Math.Abs(Data.credits-(100+rewards+Data.refunds+Data.saleIncome-Data.buildSpent))>.001)return "Credit ledger mismatch";return AuditOffworld();}
    }
}
