using System;
using System.Collections.Generic;
namespace Verdant.Automation
{
    public sealed partial class ArchipelagoWorld
    {
        public HashSet<int> Breakpoints=new HashSet<int>();public int DebugWorker=-1;public string DebugControl="";
        public ScriptTask ProgramTask(int id){UnitProgram runtime;return programs.TryGetValue(id,out runtime)?runtime.task:null;}
        public ScriptTask ControlTask(string scope){UnitProgram runtime;return controls.TryGetValue(scope,out runtime)?runtime.task:null;}
        public bool StepProgram(int id)
        {
            UnitProgram r;if(!programs.TryGetValue(id,out r))return false;ProgramsPaused=true;
            try{if(!r.task.Step()){object result;if(!r.task.context.scope.Try("result",out result))result=r.task.Result;results[id]=ScriptGraph.Clone(result);StopProgram(id);}else r.delay=Math.Max(.001,r.task.Delay);DebugWorker=id;return true;}
            catch(Exception e){var unit=Unit(id);if(unit!=null)unit.codeError=e.Message;Data.programFaults++;StopProgram(id);return false;}
        }
        public bool StepControl(string scope)
        {
            UnitProgram r;if(!controls.TryGetValue(scope,out r))return false;ProgramsPaused=true;try{if(!r.task.Step())StopControl(scope);else r.delay=Math.Max(.001,r.task.Delay);DebugControl=scope;return true;}catch(Exception e){var c=Data.controls.Find(v=>v.scope==scope);if(c!=null)c.error=e.Message;StopControl(scope);return false;}
        }
    }
    public sealed partial class ClassicController
    {
        ScriptTask SelectedRuntime()
        {if(programTarget.StartsWith("group:")){var group=Farm.Fleet.Data.groups.Find(g=>g.name==programTarget.Substring(6));return group==null||group.members.Count==0?null:Farm.Fleet.ProgramTask(group.members[0]);}if(programTarget=="network"||programTarget.StartsWith("island:"))return Farm.Fleet.ControlTask(programTarget);return Farm.Fleet.ProgramTask(TargetDrone());}
        void StepSelectedTarget()
        {
            StoreEditor();var world=Farm.Fleet;if(SelectedRuntime()==null)RunTarget(Farm.Data.programs[Farm.Data.selectedProgram]);
            if(programTarget.StartsWith("group:")){var group=world.Data.groups.Find(g=>g.name==programTarget.Substring(6));if(group!=null)foreach(int member in group.members)world.StepProgram(member);}else if(programTarget=="network"||programTarget.StartsWith("island:"))world.StepControl(programTarget);else world.StepProgram(TargetDrone());Session.Paused=true;Refresh();
        }
    }
}
