using System;
using System.Collections.Generic;
using System.Linq;

namespace Verdant.Automation
{
    public sealed partial class ArchipelagoWorld
    {
        readonly Dictionary<string,UnitProgram> controls=new Dictionary<string,UnitProgram>();
        public bool ControlRunning(string scope){return controls.ContainsKey(scope);}
        public bool BindControl(string scope,string file)
        {
            if(farm.Level("spaceflight")==0||!ValidControl(scope))return false;
            var program=farm.Data.programs.FirstOrDefault(p=>ClassicProgramNames.Same(p.name,file));if(program==null)return false;
            StopControl(scope);var binding=Data.controls.FirstOrDefault(c=>c.scope==scope);if(binding==null){binding=new ControlBinding{scope=scope};Data.controls.Add(binding);}binding.program=program.name;binding.error="";return true;
        }
        bool ValidControl(string scope)
        {int island;return scope=="network"||scope.StartsWith("island:")&&int.TryParse(scope.Substring(7),out island)&&IsUnlocked(island);}
        public bool RunControl(string scope,string file)
        {
            if(!BindControl(scope,file))return false;
            var binding=Data.controls.First(c=>c.scope==scope);int island=scope=="network"?-1:int.Parse(scope.Substring(7));
            try
            {
                var task=new ScriptTask(new FleetProgramHost(this,-1,island),farm.Data.programs.First(p=>p.name==binding.program).source);
                task.context.scope.values["planet_id"]=(double)island;task.context.scope.values["worker_id"]=-1.0;task.context.scope.values["island_id"]=(double)island;task.context.scope.values["unit_id"]=-1.0;controls[scope]=new UnitProgram{task=task};return true;
            }
            catch(Exception e){binding.error=e.Message;return false;}
        }
        public bool StopControl(string scope){return controls.Remove(scope);}
        public void StopAllControls(){controls.Clear();}
        void TickControls()
        {
            foreach(string scope in controls.Keys.ToArray())
            {
                if(ProgramsPaused)return;
                UnitProgram runtime;if(!controls.TryGetValue(scope,out runtime))continue;runtime.delay-=.25;int budget=128;
                try
                {
                    while(!ProgramsPaused&&runtime.delay<=0&&budget-->0&&controls.ContainsKey(scope)&&ReferenceEquals(controls[scope],runtime))
                    {
                        if(!runtime.task.Step()){controls.Remove(scope);break;}runtime.delay+=Math.Max(runtime.task.Delay,1.0/1024);if(runtime.task.Line!=runtime.lastBreakLine&&Breakpoints.Contains(runtime.task.Line)){runtime.lastBreakLine=runtime.task.Line;DebugControl=scope;ProgramsPaused=true;}else if(!Breakpoints.Contains(runtime.task.Line))runtime.lastBreakLine=0;
                    }
                    if(runtime.delay<0)runtime.delay=0;
                }
                catch(Exception e){var binding=Data.controls.First(c=>c.scope==scope);binding.error=e.Message;controls.Remove(scope);Data.programFaults++;farm.Say(scope+" / "+e.Message);}
            }
        }
        public string[] ProgramBindings(string name)
        {
            return Data.controls.Where(c=>ClassicProgramNames.Same(c.program,name)).Select(c=>c.scope=="network"?"universe":c.scope.Replace("island:","planet:")).Concat(Data.units.Where(u=>ClassicProgramNames.Same(u.program,name)).Select(u=>"worker:"+u.id)).ToArray();
        }
        public bool AnyCodeRunning {get{return programs.Count>0||controls.Count>0;}}
        public ClassicDrone[] EarthDrones {get{return programs.Values.Select(p=>p.crop).Where(d=>d!=null).ToArray();}}
        public int ForkFarmer(int parent,object function,object[] arguments)
        {
            if(farm.Level("megafarm")==0||Data.units.Count(u=>u.island==0)>=WorkerLimit(0))return -1;var origin=Unit(parent);if(origin==null)return -1;
            var unit=new FleetUnit{id=Data.nextId++,role="drone",island=0,cropX=origin.cropX,cropY=origin.cropY,x=origin.x,y=origin.y,program=origin.program,transient=true,manual=true};int px,py;if(!FindParking(unit,out px,out py))return -1;unit.x=unit.cropX=px;unit.y=unit.cropY=py;Data.units.Add(unit);
            try
            {
                var host=new FleetProgramHost(this,unit.id);object clone=ScriptGraph.Clone(function);var args=(object[])ScriptGraph.Clone(arguments);
                var task=clone is ScriptFunction?new ScriptTask(host,(ScriptFunction)clone,args):new ScriptTask(host,(Native)clone,args);
                programs[unit.id]=new UnitProgram{task=task,crop=host.Crop,host=host};return unit.id;
            }
            catch{Data.units.Remove(unit);throw;}
        }
    }
}
