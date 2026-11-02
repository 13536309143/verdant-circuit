using System;
using System.Linq;
namespace Verdant.Automation
{
    public sealed partial class ArchipelagoWorld
    {
        static bool Finite(double n){return !double.IsNaN(n)&&!double.IsInfinity(n)&&n>=0;}
        public static bool EmptyPlaceholder(ArchipelagoSave s){return s!=null&&!s.started&&(s.islands==null||s.islands.Count==0)&&(s.units==null||s.units.Count==0);}
        public static void Validate(ArchipelagoSave s)
        {
            if(s==null||EmptyPlaceholder(s))return;if(s.networkSchema!=3||s.islands==null||s.islands.Count!=4||s.units==null||s.ufos==null||s.order<0||s.order>Contracts.Length||!Finite(s.credits)||!Finite(s.time)||s.orderTimes==null||s.orderTimes.Count!=s.order)throw new Exception("Invalid universe save.");
            for(int p=0;p<4;p++){var grid=s.islands[p];if(grid.island!=p||grid.width<1||grid.width>360||grid.height<1||grid.height>128||grid.slotTier<0||grid.slotTier>7||grid.hullTier<0||grid.hullTier>4||grid.expansion<0||grid.expansion>9)throw new Exception("Invalid planet surface.");foreach(var list in new[]{grid.items,grid.made,grid.delivered})if(list==null||list.Any(v=>v==null||!Goods.Contains(v.id)||!Finite(v.amount)))throw new Exception("Invalid planet inventory.");}
            foreach(var ledger in new[]{s.mothership,s.consumed,s.submitted,s.exported,s.earthSent,s.earthReceived})if(ledger==null||ledger.Any(v=>v==null||!Goods.Contains(v.id)||!Finite(v.amount)))throw new Exception("Invalid universe material ledger.");
            if(s.units.Any(u=>u==null||u.id<1||u.id>=s.nextId||u.island<0||u.island>3||u.role!=Roles[u.island]||u.width<1||u.width>3||u.height<1||u.height>3||u.x<0||u.y<0||u.jobs==null||u.jobs.Count>64)||s.units.Select(u=>u.id).Distinct().Count()!=s.units.Count)throw new Exception("Invalid worker binding.");
            foreach(var u in s.units)foreach(var j in u.jobs)if(j==null||j.kind!="work"&&j.kind!="route"||j.kind=="work"&&(!Recipes.Any(r=>r.island==u.island&&r.item==j.item)||j.remaining<1||j.phase<0||j.phase>1||!Finite(j.timer)))throw new Exception("Invalid work queue.");
            if(s.ufos.Count>4||s.ufoCargoTier<0||s.ufoCargoTier>3||s.ufoSlots<0||s.ufoSlots>3||s.ufos.Any(u=>u==null||u.id<1||u.id>=s.nextUfoId||u.planet< -1||u.planet>3||u.source< -1||u.source>3||u.destination< -1||u.destination>3||u.phase!=0&&u.phase!=1&&u.phase!=2&&u.phase!=3||u.reserved<0||u.cargo<0||u.cargo>new[]{24,72,144,288}[s.ufoCargoTier]||!Finite(u.battery)||u.battery>1000||!Finite(u.timer)||u.legTarget< -1||u.legTarget>3||u.phase!=0&&u.remaining!=u.reserved+u.cargo||u.phase!=0&&u.remaining>0&&!Goods.Contains(u.item))||s.ufos.Select(u=>u.id).Distinct().Count()!=s.ufos.Count)throw new Exception("Invalid UFO manifest.");
            if(s.planetResearch==null||s.planetResearch.Any(v=>v==null||!Finite(v.amount)||!PlanetTechnologies.Any(t=>t.planet+":"+t.id==v.id&&v.amount<=t.max&&v.amount==Math.Floor(v.amount))))throw new Exception("Invalid specialist research.");
            if(s.groups==null||s.controls==null||s.groups.Any(g=>g==null||g.members==null||g.members.Any(id=>!s.units.Any(u=>u.id==id))))throw new Exception("Invalid swarm.");
        }
    }
}
