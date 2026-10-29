using System;
using System.Collections.Generic;
using UnityEngine;
namespace Verdant
{
    [Serializable] public sealed class DroneJob
    {public string action,arg;public int x,y;public float age;public DroneJob(string a,string b,int px,int py){action=a;arg=b;x=px;y=py;}}
    [Serializable] public sealed class FarmDrone
    {public int id,x,y,width=1,height=1;public string kind="general",status="空闲";public float cooldown,maintenance;public List<DroneJob> jobs=new List<DroneJob>();}
    public sealed class VerdantFleet
    {
        public readonly VerdantFarm Farm;bool applying;float collisionWindow=-10;int collisionCharges;
        public bool Applying{get{return applying;}}
        public VerdantFleet(VerdantFarm f){Farm=f;Ensure(f.Data);}
        public static void Ensure(FarmSave d)
        {
            if(d.drones==null)d.drones=new List<FarmDrone>();
            if(d.drones.Count==0)d.drones.Add(new FarmDrone{id=0,x=d.x,y=d.y});
            foreach(var u in d.drones)if(u.jobs==null)u.jobs=new List<DroneJob>();
            if(d.droneCapacity<1)d.droneCapacity=1;
            if(d.nextDroneId<1)d.nextDroneId=1;foreach(var u in d.drones)d.nextDroneId=Math.Max(d.nextDroneId,u.id+1);
            if(d.drones.Find(u=>u.id==d.activeDrone)==null)d.activeDrone=d.drones[0].id;
        }
        public FarmDrone Find(int id){var u=Farm.Data.drones.Find(d=>d.id==id);if(u==null)throw new Exception(L10n.T("未知无人机编号：")+id);return u;}
        public bool Fits(FarmDrone u,int x,int y,int width=-1,int height=-1)
        {
            int w=width<0?u.width:width,h=height<0?u.height:height,n=Farm.Data.size;
            if(x<0||y<0||x+w>n||y+h>n)return false;
            foreach(var other in Farm.Data.drones)if(other.id!=u.id&&x<other.x+other.width&&x+w>other.x&&y<other.y+other.height&&y+h>other.y)return false;
            return true;
        }
        public void Select(int id){var u=Find(id);Farm.Data.activeDrone=id;Farm.Data.x=u.x;Farm.Data.y=u.y;Farm.Notify();}
        public bool Move(string direction)
        {
            var u=Find(Farm.Data.activeDrone);if(u.jobs.Count>0){Farm.Say(L10n.T("此无人机有待处理队列，请先取消队列"));return false;}
            int dx=0,dy=0;switch(direction.ToLowerInvariant()){case "east":dx=1;break;case "west":dx=-1;break;case "north":dy=1;break;case "south":dy=-1;break;default:throw new Exception(L10n.T("方向应为 East / West / North / South"));}
            int x=u.x+dx,y=u.y+dy;
            if(u.width==1&&u.height==1){x=(x+Farm.Data.size)%Farm.Data.size;y=(y+Farm.Data.size)%Farm.Data.size;}
            if(x<0||y<0||x+u.width>Farm.Data.size||y+u.height>Farm.Data.size){Farm.Say(L10n.T("机体越界：大型机体不能越界环绕"));return false;}
            if(!Fits(u,x,y))
            {
                if(Farm.Data.clock-collisionWindow>=10){collisionWindow=Farm.Data.clock;collisionCharges=0;}
                if(collisionCharges<2){int fee=Math.Min(20,Farm.Data.coins);Farm.Data.coins-=fee;collisionCharges++;Farm.Add("collisions");Farm.Say(L10n.T("无人机碰撞：维修扣费 ")+fee);}
                u.status="碰撞受阻";Farm.Notify();return false;
            }
            u.x=x;u.y=y;Farm.Data.x=x;Farm.Data.y=y;Farm.Add("moves");u.status="空闲";return true;
        }
        public bool Work(int id,string action,string arg="",bool programmed=true)
        {
            var u=Find(id);if(u.kind=="irrigator"&&action!="water"){Farm.Say(L10n.T("浇水无人机只能执行浇水作业"));return false;}
            if(u.kind=="harvester"&&action!="harvest"){Farm.Say(L10n.T("收获无人机只能执行收获作业"));return false;}
            int selected=Farm.Data.activeDrone,oldX=Farm.Data.x,oldY=Farm.Data.y;bool success=false;
            try{applying=true;Farm.Data.activeDrone=id;
                for(int oy=0;oy<u.height;oy++)for(int ox=0;ox<u.width;ox++)
                {Farm.Data.x=u.x+ox;Farm.Data.y=u.y+oy;success=Farm.Act(action,arg,programmed)||success;}
            }finally{applying=false;Farm.Data.activeDrone=selected;Farm.Data.x=oldX;Farm.Data.y=oldY;}
            return success;
        }
        public int DeploymentPrice(string kind){return (kind=="general"?320:kind=="irrigator"?480:640)*Farm.Data.drones.Count;}
        public int ResizePrice(int id,int w,int h){var u=Find(id);return Math.Max(0,w*h-u.width*u.height)*180*(Farm.Data.footprintTier+1);}
        public bool ShapeUnlocked(int w,int h){int tier=Farm.Data.footprintTier;return w==1&&h==1||tier>=1&&w==2&&h==2||tier>=2&&(w==3&&h==2||w==2&&h==3)||tier>=3&&w==3&&h==3;}
        public bool Deploy(string kind,int x,int y)
        {
            var d=Farm.Data;if(d.drones.Count>=d.droneCapacity){Farm.Say(L10n.T("无人机数量已达解锁上限"));return false;}
            if(kind!="general"&&kind!="irrigator"&&kind!="harvester")throw new Exception(L10n.T("机型应为 general / irrigator / harvester"));
            if(kind=="irrigator"&&!Farm.Has("drone_water")||kind=="harvester"&&!Farm.Has("drone_harvest")){Farm.Say(L10n.T("请先解锁对应无人机机型"));return false;}
            var u=new FarmDrone{id=d.nextDroneId,kind=kind,x=x,y=y};if(!Fits(u,x,y)){Farm.Say(L10n.T("部署位置无效或被占用"));return false;}
            int price=DeploymentPrice(kind);
            if(d.coins<price){Farm.Say(L10n.T("部署资金不足"));return false;}d.coins-=price;d.nextDroneId++;d.drones.Add(u);Farm.Say(L10n.T("无人机已部署，编号 ")+u.id);Farm.Notify();return true;
        }
        public bool Resize(int id,int w,int h)
        {
            var u=Find(id);int tier=Farm.Data.footprintTier;
            bool valid=w==1&&h==1||tier>=1&&w==2&&h==2||tier>=2&&(w==3&&h==2||w==2&&h==3)||tier>=3&&w==3&&h==3;
            if(!valid){Farm.Say(L10n.T("该机体规格尚未解锁"));return false;}
            if(u.jobs.Count>0){Farm.Say(L10n.T("调整机体前请先取消队列"));return false;}
            if(!Fits(u,u.x,u.y,w,h)){Farm.Say(L10n.T("完整机体占地必须空闲且不越界"));return false;}
            int price=ResizePrice(id,w,h);if(Farm.Data.coins<price){Farm.Say(L10n.T("机体改造资金不足"));return false;}
            Farm.Data.coins-=price;u.width=w;u.height=h;Farm.Say(L10n.T("机体规格已更新"));Farm.Notify();return true;
        }
        public bool Retire(int id)
        {
            if(id==0){Farm.Say(L10n.T("基础无人机不能拆除"));return false;}var u=Find(id);if(u.jobs.Count>0){Farm.Say(L10n.T("请先取消队列"));return false;}
            Farm.Data.drones.Remove(u);if(Farm.Data.activeDrone==id)Select(0);Farm.Say(L10n.T("无人机已收回，不返还购置费用"));Farm.Notify();return true;
        }
        public void Enqueue(int id,string action,string arg,int x,int y)
        {
            var u=Find(id);if(u.jobs.Count>=64)throw new Exception(L10n.T("每台无人机最多排队64项任务"));
            if(x<0||y<0||x+u.width>Farm.Data.size||y+u.height>Farm.Data.size)throw new Exception(L10n.T("目标超出完整机体可达范围"));
            u.jobs.Add(new DroneJob(action,arg,x,y));u.status="排队中";Farm.Notify();
        }
        Vector2Int? Next(FarmDrone u,int tx,int ty)
        {
            int n=Farm.Data.size;var q=new Queue<Vector2Int>();var previous=new Dictionary<Vector2Int,Vector2Int>();var start=new Vector2Int(u.x,u.y);q.Enqueue(start);previous[start]=start;
            int[] dx={1,0,-1,0},dy={0,1,0,-1};
            while(q.Count>0){var p=q.Dequeue();if(p.x==tx&&p.y==ty){while(previous[p]!=start&&p!=start)p=previous[p];return p;}
                for(int i=0;i<4;i++){var v=new Vector2Int(p.x+dx[i],p.y+dy[i]);if(!previous.ContainsKey(v)&&Fits(u,v.x,v.y)){previous[v]=p;q.Enqueue(v);}}}
            return null;
        }
        public void Tick(float dt)
        {
            if(Farm.Data.fleetPaused)return;
            foreach(var u in Farm.Data.drones)
            {
                if(u.jobs.Count==0){u.status="空闲";continue;}
                if(u.id!=0){u.maintenance=Mathf.Min(30,u.maintenance+dt);while(u.maintenance>=30){int fee=Math.Max(1,u.width*u.height*(u.kind=="general"?1:2));if(Farm.Data.coins<fee){u.status="维护费不足，暂停";break;}Farm.Data.coins-=fee;u.maintenance-=30;}if(u.maintenance>=30)continue;}
                var job=u.jobs[0];u.cooldown-=dt;job.age+=dt;if(u.cooldown>0)continue;
                if(u.x!=job.x||u.y!=job.y)
                {var next=Next(u,job.x,job.y);if(!next.HasValue){u.status="等待避让";u.cooldown=.25f;if(job.age>30){u.jobs.RemoveAt(0);Farm.Say(L10n.T("队列任务等待超时，已取消；请调整路线"));}continue;}
                 u.x=next.Value.x;u.y=next.Value.y;Farm.Add("moves");if(Farm.Data.activeDrone==u.id){Farm.Data.x=u.x;Farm.Data.y=u.y;}u.status="移动中";u.cooldown=Farm.ActionDelay*(1+.12f*(u.width*u.height-1));continue;}
                if(job.action!="move")Work(u.id,job.action,job.arg);
                u.jobs.RemoveAt(0);u.status=u.jobs.Count>0?"排队中":"空闲";u.cooldown=Farm.ActionDelay*Math.Max(1,u.width*u.height*.6f)*(u.kind=="general"?1f:.55f);
            }
        }
        public void CancelAll(){foreach(var u in Farm.Data.drones){u.jobs.Clear();u.status="空闲";}Farm.Data.fleetPaused=false;}
        static int Int(object o){double d=FarmVM.Number(o);if(d!=Math.Truncate(d)||d<int.MinValue||d>int.MaxValue)throw new Exception(L10n.T("无人机参数需要整数"));return (int)d;}
        public static bool IsAction(string n){return Array.IndexOf(new[]{"select_drone","deploy_drone","resize_drone","retire_drone","queue_move","queue_water","queue_harvest","queue_plant","cancel_queue"},n)>=0;}
        public static bool IsSensor(string n){return Array.IndexOf(new[]{"drone_count","drone_capacity","footprint_tier","drone_id","drone_x","drone_y","drone_width","drone_height","queue_length","drone_busy","can_fit"},n)>=0;}
        public void Invoke(string name,object[] args)
        {
            int expected=name=="deploy_drone"||name=="resize_drone"||name=="queue_move"||name=="queue_water"||name=="queue_harvest"?3:name=="queue_plant"?4:1;
            if(args.Length!=expected)throw new Exception(name+L10n.T("() 参数数量不正确"));
            int id=name=="deploy_drone"?-1:Int(args[0]);
            switch(name){case "select_drone":Select(id);break;case "deploy_drone":Deploy(Convert.ToString(args[0]),Int(args[1]),Int(args[2]));break;case "resize_drone":Resize(id,Int(args[1]),Int(args[2]));break;case "retire_drone":Retire(id);break;
                case "cancel_queue":Find(id).jobs.Clear();break;
                case "queue_move":case "queue_water":case "queue_harvest":Enqueue(id,name.Substring(6),"",Int(args[1]),Int(args[2]));break;
                case "queue_plant":Enqueue(id,"plant",Convert.ToString(args[1]),Int(args[2]),Int(args[3]));break;}
        }
        public object Sense(string name,object[] args)
        {
            int expected=name=="drone_count"||name=="drone_capacity"||name=="footprint_tier"?0:name=="can_fit"?3:1;
            if(args.Length!=expected)throw new Exception(name+L10n.T("() 参数数量不正确"));
            if(name=="drone_count")return (double)Farm.Data.drones.Count;if(name=="drone_capacity")return (double)Farm.Data.droneCapacity;if(name=="footprint_tier")return (double)Farm.Data.footprintTier;
            if(name=="drone_id"){int index=Int(args[0]);if(index<0||index>=Farm.Data.drones.Count)throw new Exception(L10n.T("无人机索引越界"));return (double)Farm.Data.drones[index].id;}
            var u=Find(Int(args[0]));switch(name){case "drone_x":return (double)u.x;case "drone_y":return (double)u.y;case "drone_width":return (double)u.width;case "drone_height":return (double)u.height;case "queue_length":return (double)u.jobs.Count;case "drone_busy":return u.jobs.Count>0;case "can_fit":return Fits(u,Int(args[1]),Int(args[2]));}return 0.0;
        }
    }
}
