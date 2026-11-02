using System;
using System.Linq;
using System.Collections.Generic;

namespace Verdant.Automation
{
    public sealed partial class ArchipelagoWorld
    {
        sealed class UnitProgram {public ScriptTask task;public FleetProgramHost host;public ClassicDrone crop;public int lastBreakLine;public double delay;}
        readonly Dictionary<int,UnitProgram> programs=new Dictionary<int,UnitProgram>();
        readonly Dictionary<int,object> results=new Dictionary<int,object>();
        readonly Dictionary<int,Queue<object>> inboxes=new Dictionary<int,Queue<object>>();
        public object Result(int id){object result;return results.TryGetValue(id,out result)?ScriptGraph.Clone(result):null;}
        public bool ProgramRunning(int id){return programs.ContainsKey(id);}
        public int ProgramUsers(string name){return programs.Count(p=>ClassicProgramNames.Same(Unit(p.Key)?.program,name)||p.Value.task.context.modules.Keys.Any(m=>ClassicProgramNames.Same(m,name)))+controls.Count(p=>Data.controls.Any(c=>c.scope==p.Key&&ClassicProgramNames.Same(c.program,name))||p.Value.task.context.modules.Keys.Any(m=>ClassicProgramNames.Same(m,name)));}
        public int InboxCount(int id){Queue<object> inbox;return inboxes.TryGetValue(id,out inbox)?inbox.Count:0;}
        public bool RunProgram(int id,string name)
        {
            var unit=Unit(id);if(unit==null)return false;var file=farm.Data.programs.FirstOrDefault(p=>ClassicProgramNames.Same(p.name,name));if(file==null)return false;
            try
            {
                if(unit.role!="drone"&&farm.Level("spaceflight")==0)return false;var host=new FleetProgramHost(this,id);var task=new ScriptTask(host,file.source);task.context.scope.values["worker_id"]=(double)id;task.context.scope.values["planet_id"]=(double)unit.island;task.context.scope.values["unit_id"]=(double)id;task.context.scope.values["island_id"]=(double)unit.island;task.context.scope.values["group_name"]=Data.groups.FirstOrDefault(g=>g.members.Contains(id))?.name??"";
                UnitProgram previous;if(programs.TryGetValue(id,out previous))previous.host?.Cleanup();
                programs[id]=new UnitProgram{task=task,crop=host.Crop,host=host};results.Remove(id);Data.programRuns++;unit.program=name;unit.codeError="";return true;
            }
            catch(Exception e){unit.codeError=e.Message;return false;}
        }
        public bool StopProgram(int id){var stopping=Unit(id);if(stopping==null)return false;UnitProgram old;if(programs.TryGetValue(id,out old))old.host?.Cleanup();programs.Remove(id);inboxes.Remove(id);if(stopping.transient){Data.units.Remove(stopping);foreach(var group in Data.groups)group.members.Remove(id);}return true;}
        public bool Send(int id,object payload)
        {
            if(Unit(id)==null)return false;Queue<object> inbox;if(!inboxes.TryGetValue(id,out inbox)){inbox=new Queue<object>();inboxes[id]=inbox;}
            if(inbox.Count>=16)return false;Data.messages++;inbox.Enqueue(ScriptGraph.Clone(payload));return true;
        }
        public object Receive(int id)
        {Queue<object> inbox;return inboxes.TryGetValue(id,out inbox)&&inbox.Count>0?inbox.Dequeue():null;}
        void TickPrograms()
        {
            foreach(int id in programs.Keys.ToArray())
            {
                if(ProgramsPaused)return;
                UnitProgram runtime;if(!programs.TryGetValue(id,out runtime))continue;var unit=Unit(id);if(unit==null){StopProgram(id);continue;}
                runtime.delay-=.25;int budget=unit.island==0?(int)Math.Min(4096,128*Math.Max(1,farm.Speed/4)):128;
                try
                {
                    while(!ProgramsPaused&&runtime.delay<=0&&budget-->0&&programs.ContainsKey(id)&&ReferenceEquals(programs[id],runtime))
                    {
                        if(!runtime.task.Step()){object result;if(!runtime.task.context.scope.Try("result",out result))result=runtime.task.Result;results[id]=ScriptGraph.Clone(result);runtime.host?.Cleanup();programs.Remove(id);break;}
                        runtime.delay+=Math.Max(runtime.task.Delay,1.0/1024);if(runtime.task.Line!=runtime.lastBreakLine&&Breakpoints.Contains(runtime.task.Line)){runtime.lastBreakLine=runtime.task.Line;DebugWorker=id;ProgramsPaused=true;}else if(!Breakpoints.Contains(runtime.task.Line))runtime.lastBreakLine=0;
                    }
                    // Drop deferred CPU debt: a later wait must still wait real simulation time.
                    if(runtime.delay<0)runtime.delay=0;if(unit.transient&&!programs.ContainsKey(id)){Data.units.Remove(unit);foreach(var group in Data.groups)group.members.Remove(id);}
                }
                catch(Exception e){unit.codeError=e.Message;Data.programFaults++;runtime.host?.Cleanup();programs.Remove(id);if(unit.transient){Data.units.Remove(unit);foreach(var group in Data.groups)group.members.Remove(id);}farm.Say("Fleet #"+id+" / "+e.Message);}
            }
        }
        sealed class FleetProgramHost:IScriptHost,IScriptSymbols,IScriptFeatures
        {
            readonly ArchipelagoWorld world;readonly int id,scopeIsland;readonly ClassicHost common;public ClassicDrone Crop;
            public FleetProgramHost(ArchipelagoWorld world,int id,int scopeIsland=-1){this.world=world;this.id=id;this.scopeIsland=scopeIsland;var unit=world.Unit(id);var drone=new ClassicDrone{id=id,x=unit?.cropX??0,y=unit?.cropY??0,hat=unit?.hat??"straw"};if(unit!=null&&unit.role=="drone"&&unit.island==0)Crop=drone;common=new ClassicHost(new ClassicSession(world.farm),drone);}
            public void Cleanup(){if(Crop!=null&&(world.farm.Data.defaultSize>0||world.farm.dinosaur==id))common.session.Stop();}
            public object Resolve(string name){return common.Resolve(name);}
            public bool Known(string name){return common.Known(name);}
            public bool Allows(string feature){return common.Allows(feature);}
            public string Module(string name){return common.Module(name);}
            public IEnumerable<Pulse> Call(string name,object[] args,Box result,int line)
            {
                if(name=="worker_wait"||name=="ufo_wait")name="fleet_wait";
                if(UniverseApi.Contains(name)){result.value=world.UniverseInvoke(name,args,id,scopeIsland);yield return new Pulse(line,name=="harvest_area"||name=="plant_area"?.5:0);yield break;}
                if(Crop!=null&&(name=="move"||name=="can_move"))
                {
                    Verdant.Automation.Call.Require(args,1);string direction=S(args[0]);int dx=direction=="East"?1:direction=="West"?-1:0,dy=direction=="North"?1:direction=="South"?-1:0;var u=world.Unit(id);Crop.x=u.x;Crop.y=u.y;int x=u.x+dx,y=u.y+dy;if(world.SurfaceWidth(0)==360)x=(x+360)%360;
                    if(name=="can_move")result.value=Math.Abs(dx)+Math.Abs(dy)==1&&world.Fits(u,x,y)&&world.farm.CanMove(u.y*world.farm.Data.size+u.x,dx==1?1:dx==-1?3:dy==1?0:2,Crop.hat=="dinosaur");
                    else if(Crop.hat=="dinosaur"){foreach(var pulse in common.Call(name,args,result,line))yield return pulse;u.x=Crop.x;u.y=Crop.y;u.cropX=u.x;u.cropY=u.y;yield break;}
                    else{u.delay=0;result.value=world.ManualStep(id,dx,dy);Crop.x=u.x;Crop.y=u.y;}yield return new Pulse(line,.25/world.farm.Speed);yield break;
                }
                if(name=="island_self"){Verdant.Automation.Call.Require(args,0);result.value=(double)(world.Unit(id)?.island??scopeIsland);yield return new Pulse(line);yield break;}
                if(name=="get_tick_count"){Verdant.Automation.Call.Require(args,0);UnitProgram runtime;result.value=world.programs.TryGetValue(id,out runtime)?(double)runtime.task.Steps:0.0;yield return new Pulse(line);yield break;}
                if(name=="fleet_self"){Verdant.Automation.Call.Require(args,0);result.value=(double)id;yield return new Pulse(line);yield break;}
                if(name=="fleet_receive"){Verdant.Automation.Call.Require(args,0);result.value=world.Receive(id);yield return new Pulse(line);yield break;}
                if(name=="fleet_wait")
                {Verdant.Automation.Call.Require(args,1);double delay=ScriptValue.Number(args[0]);if(delay<0||delay>3600)throw new Exception("Fleet wait must be between 0 and 3600 seconds.");result.value=null;yield return new Pulse(line,Math.Max(.25,delay));yield break;}
                if(Api.Contains(name)){result.value=world.Invoke(name,args);yield return new Pulse(line);yield break;}
                if(name=="spawn_drone")
                {if(Crop==null||args.Length<1)throw new Exception("spawn_drone needs an Earth worker and a function.");int child=world.ForkFarmer(id,args[0],args.Skip(1).ToArray());result.value=child<0?null:(object)(double)child;yield return new Pulse(line);yield break;}
                if(name=="num_drones"||name=="max_drones"){Verdant.Automation.Call.Require(args,0);result.value=(double)(name=="num_drones"?world.programs.Count:world.SlotLimit);yield return new Pulse(line);yield break;}
                if(name=="has_finished"||name=="wait_for")
                {Verdant.Automation.Call.Require(args,1);int other=N(args[0]);if(other==id)throw new Exception("A drone cannot wait for itself.");if(name=="wait_for"){while(world.ProgramRunning(other))yield return new Pulse(line,.25);result.value=world.Result(other);}else result.value=!world.ProgramRunning(other);yield return new Pulse(line);yield break;}
                if(Crop==null&&!new[]{"print","quick_print","get_time","random","num_unlocked","num_items","num_produced","get_cost","get_unlock_requirements","can_unlock","unlock"}.Contains(name))throw new Exception("Earth crop operations require a farmer drone on Earth; bind this file to an Earth drone.");
                if(Crop!=null){var current=world.Unit(id);Crop.x=current==null?0:world.farm.Wrap(current.x);Crop.y=current==null?0:Math.Max(0,Math.Min(world.farm.Height-1,current.y));}
                foreach(var pulse in common.Call(name,args,result,line))
                {
                    if(Crop!=null){var unit=world.Unit(id);if(unit!=null){unit.x=unit.cropX=Crop.x;unit.y=unit.cropY=Crop.y;unit.hat=Crop.hat;if(unit==world.Data.units.FirstOrDefault(u=>u.role=="drone")){world.farm.Data.playerX=Crop.x;world.farm.Data.playerY=Crop.y;}}}
                    yield return pulse;
                }
            }
        }
    }
}
