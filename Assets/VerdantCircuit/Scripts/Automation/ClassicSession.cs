using System;
using System.Collections.Generic;
using System.Linq;

namespace Verdant.Automation
{
    public sealed class ClassicDrone
    {public int id,x,y;public string program,hat="straw";public ScriptTask task;public double delay;public bool finished;public object result;public int line,skipBreakpoint;public long ticks;}
    public sealed class ClassicSession
    {
        public ClassicFarm Farm;public List<ClassicDrone> drones=new List<ClassicDrone>();public bool Running,Paused,Simulating,IsSimulation;public string Error="";public long ticks;public double elapsed;public int FocusedId,PauseSerial;public ClassicDrone FocusedDrone{get{return drones.FirstOrDefault(d=>d.id==FocusedId&&!d.finished)??drones.FirstOrDefault(d=>!d.finished);}}public HashSet<int> breakpoints=new HashSet<int>();
        public int TotalSpawned{get{return nextId;}}int nextId;double accumulator;public double ExecutionLimit=double.PositiveInfinity;public double EffectiveSpeed{get{return Math.Max(.05,Math.Min(Farm.Speed,ExecutionLimit));}}public ClassicSession(ClassicFarm farm){Farm=farm;drones.Add(new ClassicDrone{id=0,x=Farm.Data.playerX,y=Farm.Data.playerY,finished=true});nextId=1;}
        public ClassicDrone Start(string source)
        {Stop();drones.Clear();nextId=0;Error="";ticks=0;elapsed=0;var d=new ClassicDrone{id=nextId++,x=Farm.Data.playerX,y=Farm.Data.playerY};d.task=new ScriptTask(new ClassicHost(this,d),source);drones.Add(d);Running=true;return d;}
        public ClassicDrone Launch(string source)
        {if(!Running)return Start(source);if(drones.Count(d=>!d.finished)>=Farm.MaxDrones)return null;var d=new ClassicDrone{id=nextId++,x=Farm.Data.playerX,y=Farm.Data.playerY};d.task=new ScriptTask(new ClassicHost(this,d),source);drones.Add(d);return d;}
        public ClassicDrone Spawn(object function,object[] arguments)
        {if(drones.Count(d=>!d.finished)>=Farm.MaxDrones)return null;var root=drones.FirstOrDefault(d=>!d.finished);var d=new ClassicDrone{id=nextId++,x=root==null?0:root.x,y=root==null?0:root.y};var cloned=ScriptGraph.Clone(function);d.task=cloned is ScriptFunction?new ScriptTask(new ClassicHost(this,d),(ScriptFunction)cloned,(object[])ScriptGraph.Clone(arguments)):new ScriptTask(new ClassicHost(this,d),(Native)cloned,(object[])ScriptGraph.Clone(arguments));drones.Add(d);return d;}
        void RestoreDebugWorld(){ExecutionLimit=double.PositiveInfinity;if(Farm.Data.defaultSize>0){int original=Farm.Data.defaultSize;Farm.Data.defaultSize=0;Farm.Resize(original);Farm.Data.playerX=Farm.Wrap(Farm.Data.playerX);Farm.Data.playerY=Farm.Wrap(Farm.Data.playerY);foreach(var d in drones){d.x=Farm.Wrap(d.x);d.y=Farm.Wrap(d.y);}}}
        public void Stop(){if(Farm.dinosaur>=0&&Farm.tail.Count>0)Farm.Produce("bone",Farm.tail.Count*Farm.tail.Count*Math.Pow(2,Math.Max(0,Farm.Level("dinosaurs")-1)));RestoreDebugWorld();Running=false;Paused=false;Simulating=false;drones.Clear();Farm.tail.Clear();if(Farm.apple>=0&&Farm.apple<Farm.Data.cells.Count&&Farm.Data.cells[Farm.apple].entity=="apple")Farm.Data.cells[Farm.apple].entity="";Farm.dinosaur=Farm.apple=Farm.nextApple=-1;nextId=1;drones.Add(new ClassicDrone{id=0,x=Farm.Data.playerX,y=Farm.Data.playerY,finished=true});accumulator=0;ExecutionLimit=double.PositiveInfinity;}
        public void Tick(double dt,int speed=1)
        {if(!Running||Paused){Farm.Advance(Math.Max(0,dt)*speed);return;}accumulator=Math.Min(2,accumulator+Math.Max(0,dt)*speed);int budget=240;while(accumulator>=1.0/60&&budget-->0&&Running&&!Paused){accumulator-=1.0/60;FixedTick();}}
        public void FixedTick()
        {
            if(!Simulating){Farm.Advance(1.0/60);elapsed+=1.0/60;}int count=drones.Count;
            for(int i=0;i<count;i++)
            {
                var d=drones[i];if(d.finished)continue;d.delay-=1.0/60;int budget=256;
                while(d.delay<=0&&budget-->0&&!d.finished)
                {
                    try{ticks++;if(!d.task.Step()){d.finished=true;d.result=d.task.Result;break;}int previousLine=d.line;d.line=d.task.Line;if(previousLine!=d.line)d.skipBreakpoint=0;if(d.id==0){Farm.Data.playerX=d.x;Farm.Data.playerY=d.y;}double delay=Math.Max(d.task.Delay,1.0/(400*EffectiveSpeed));d.delay+=delay;long spent=(long)Math.Max(1,Math.Round(delay*400*EffectiveSpeed));d.ticks+=spent;if(Farm.Count("power")>0)Farm.Add("power",-spent/6000.0);if(breakpoints.Contains(d.line)&&d.skipBreakpoint!=d.line){d.skipBreakpoint=d.line;FocusedId=d.id;PauseSerial++;Paused=true;return;}}
                    catch(Exception e){Error=e.Message;Farm.Say(Error);Running=false;foreach(var u in drones)u.finished=true;RestoreDebugWorld();return;}
                }
            }
            drones.RemoveAll(d=>d.finished&&d.id!=0);if(drones.All(d=>d.finished)){Running=false;RestoreDebugWorld();}
        }
        public void Step(){if(!Running)return;var d=FocusedDrone;if(d==null)return;try{if(!d.task.Step()){d.finished=true;d.result=d.task.Result;if(drones.All(x=>x.finished)){Running=false;RestoreDebugWorld();}}else{d.line=d.task.Line;d.skipBreakpoint=d.line;d.delay=Math.Max(d.task.Delay,1.0/(400*EffectiveSpeed));}Paused=true;PauseSerial++;}catch(Exception e){Error=e.Message;Farm.Say(Error);Running=false;RestoreDebugWorld();}}

        public IEnumerable<Pulse> Simulate(string file,object unlocks,object items,object globals,int seed,int speedup,Box result,int line)
        {
            if(Simulating||IsSimulation)throw new Exception("Nested simulations are not supported.");string source=Farm.Data.programs.FirstOrDefault(p=>p.name==file)?.source;if(source==null)throw new Exception("Simulation program not found: "+file);
            var world=new ClassicFarm((uint)(seed<0?DateTime.UtcNow.Ticks:seed+1));world.Data.allowLockedSyntax=true;world.Data.programs=Farm.Data.programs.Select(p=>new ClassicProgram(p.name,p.source)).ToList();
            if(unlocks is string&&(string)unlocks=="_all_unlocks")foreach(var tech in ClassicFarm.Techs)world.Data.unlocks.Add(new Stock(tech.id,tech.max));else if(unlocks is Dictionary<object,object>)foreach(var p in (Dictionary<object,object>)unlocks)world.Data.unlocks.Add(new Stock(Convert.ToString(p.Key).Replace("unlock:",""),Math.Max(0,ScriptValue.Integer(p.Value))));else if(unlocks!=null)foreach(var v in ScriptValue.Items(unlocks)){string id=Convert.ToString(v).Replace("unlock:","");var t=ClassicFarm.Techs.FirstOrDefault(x=>x.id==id);if(t==null)throw new Exception("Unknown simulation research: "+id);world.Data.unlocks.Add(new Stock(id,t.max));}
            world.Resize(world.MaxSize);if(items is Dictionary<object,object>)foreach(var p in (Dictionary<object,object>)items)world.Add(Convert.ToString(p.Key),ScriptValue.Number(p.Value));
            var session=new ClassicSession(world){IsSimulation=true};var drone=session.Start(source);if(globals is Dictionary<object,object>)foreach(var p in (Dictionary<object,object>)globals)drone.task.context.scope.values[Convert.ToString(p.Key)]=ScriptGraph.Clone(p.Value);
            Simulating=true;try{int budget=0;while(session.Running){for(int i=0;i<Math.Max(1,Math.Min(1024,speedup))&&session.Running;i++){session.FixedTick();if(++budget>216000)throw new Exception("Simulation exceeded one hour of game time.");}yield return new Pulse(line,1.0/60);}if(session.Error!="")throw new Exception(session.Error);result.value=session.elapsed;}finally{Simulating=false;}
        }
        public IEnumerable<Pulse> Leaderboard(string category,string file,Box result,int line)
        {
            if(Simulating||IsSimulation)throw new Exception("Nested challenges are not supported.");string key=category.Replace("_","").ToLowerInvariant();bool single=key.EndsWith("single");if(single)key=key.Substring(0,key.Length-6);
            string item=key=="carrots"?"carrot":key=="sunflowers"?"power":key=="dinosaur"?"bone":key=="maze"?"gold":key=="fastestreset"?"reset":key;
            if(item!="reset"&&!new[]{"hay","wood","carrot","pumpkin","power","cactus","gold","bone"}.Contains(item))throw new Exception("Unknown local challenge: "+category);
            string source=Farm.Data.programs.Find(p=>p.name==file)?.source;if(source==null)throw new Exception("Challenge program not found: "+file);var world=new ClassicFarm(41);world.Data.allowLockedSyntax=true;world.Data.programs=Farm.Data.programs.Select(p=>new ClassicProgram(p.name,p.source)).ToList();
            if(item!="reset"){foreach(var t in ClassicFarm.Techs)world.Data.unlocks.Add(new Stock(t.id,t.max));world.Resize(8);if(single)world.Data.droneLimit=1;foreach(string resource in ClassicFarm.Items)if(resource!=item)world.Add(resource,1000000000);}
            var run=new ClassicSession(world){IsSimulation=true};run.Start(source);Simulating=true;try{int ticks=0;while(run.Running){for(int i=0;i<100&&run.Running;i++){run.FixedTick();if(++ticks>216000)throw new Exception("Challenge exceeded one game hour.");}yield return new Pulse(line);}bool won=run.Error==""&&(item=="reset"?world.Level("leaderboard")>0:(world.Data.harvested.Find(s=>s.id==item)?.amount??0)>=1000);if(!won)throw new Exception("Challenge target not reached. "+run.Error);var record=Farm.Data.records.Find(r=>r.category==category&&r.seed==41);if(record==null)Farm.Data.records.Add(new RaceRecord{category=category,seconds=run.elapsed,program=file,seed=41});else if(run.elapsed<record.seconds){record.seconds=run.elapsed;record.program=file;}Farm.Say("Local record: "+category+" / "+run.elapsed.ToString("F3")+"s");result.value=null;}finally{Simulating=false;}
        }

    }
    public sealed class ClassicHost:IScriptHost,IScriptFeatures,IScriptSymbols
    {
        public ClassicSession session;public ClassicDrone drone;ClassicFarm Farm{get{return session.Farm;}}public ClassicHost(ClassicSession s,ClassicDrone d){session=s;drone=d;}
        static string Normalize(string s){return s.Replace("_","").ToLowerInvariant();}
        public bool Known(string name){return Api.Contains(name)||new[]{"Entities","Items","Grounds","Unlocks","Hats","Leaderboards"}.Contains(name)||new[]{"North","East","South","West"}.Contains(name);}
        public static readonly string[] Api={"harvest","can_harvest","plant","move","can_move","till","use_item","get_pos_x","get_pos_y","get_world_size","get_entity_type","get_ground_type","get_water","num_items","num_unlocked","get_cost","unlock","get_companion","measure","swap","clear","set_world_size","set_execution_speed","change_hat","get_time","get_tick_count","random","print","quick_print","do_a_flip","num_drones","max_drones","spawn_drone","has_finished","wait_for","simulate","leaderboard_run"};
        public object Resolve(string name)
        {
            if(new[]{"North","East","South","West"}.Contains(name))return name;
            if(name=="Unlocks")return "_all_unlocks";
            if(name.StartsWith("Entities.")){string s=Normalize(name.Substring(9));var map=new Dictionary<string,string>{{"grass","grass"},{"bush","bush"},{"tree","tree"},{"trees","tree"},{"carrot","carrot"},{"pumpkin","pumpkin"},{"deadpumpkin","dead_pumpkin"},{"sunflower","sunflower"},{"cactus","cactus"},{"hedge","hedge"},{"treasure","treasure"},{"apple","apple"}};if(map.ContainsKey(s))return map[s];throw new Exception("Unknown entity: "+name);}
            if(name.StartsWith("Items.")){string s=Normalize(name.Substring(6));string id=ClassicFarm.Items.FirstOrDefault(x=>Normalize(x)==s);if(id!=null)return id;throw new Exception("Unknown item: "+name);}
            if(name.StartsWith("Unlocks.")){string s=Normalize(name.Substring(8));string id=ClassicFarm.Techs.FirstOrDefault(t=>Normalize(t.id)==s)?.id;if(id!=null)return "unlock:"+id;throw new Exception("Unknown research: "+name);}
            if(name.StartsWith("Grounds."))return Normalize(name.Substring(8))=="soil"?"soil":"grassland";
            if(name.StartsWith("Hats."))return Normalize(name.Substring(5)).Replace("hat","");
            if(name.StartsWith("Leaderboards."))return name.Substring(13);
            return null;
        }
        public bool Allows(string feature){return Farm.Data.allowLockedSyntax||Farm.Level(feature)>0;}
        public string Module(string name){return Farm.Data.programs.FirstOrDefault(p=>p.name==name)?.source;}
        int Direction(object value){int i=Array.IndexOf(new[]{"North","East","South","West"},Convert.ToString(value));if(i<0)throw new Exception("Expected North, East, South or West.");return i;}
        int Index {get{return drone.y*Farm.Data.size+drone.x;}}
        object Measure(int index){if(Farm.maze)return new object[]{(double)(Farm.treasure%Farm.Data.size),(double)(Farm.treasure/Farm.Data.size)};var p=Farm.Data.cells[index];if(index==Farm.apple)return Farm.nextApple<0?null:new object[]{(double)(Farm.nextApple%Farm.Data.size),(double)(Farm.nextApple/Farm.Data.size)};if(p.entity=="cactus"||p.entity=="sunflower")return (double)p.measure;return null;}
        public IEnumerable<Pulse> Call(string name,object[] a,Box r,int line)
        {
            if(!Farm.Data.allowLockedSyntax)
            {
                string feature=new[]{"get_pos_x","get_pos_y","get_world_size","get_entity_type","get_ground_type","get_water","num_items","get_companion","measure","can_move"}.Contains(name)?"senses":name=="get_cost"?"costs":name=="unlock"?"auto_unlock":name=="get_time"||name=="get_tick_count"?"timing":name=="set_world_size"||name=="set_execution_speed"?"debug2":name=="random"?"utilities":name=="simulate"?"simulation":name=="leaderboard_run"?"leaderboard":name=="print"||name=="quick_print"?"debug":null;
                if(feature!=null&&Farm.Level(feature)==0)throw new Exception("Research needed: "+feature);
            }
            bool action=false;double seconds=0;var cell=Farm.Cell(drone.x,drone.y);r.value=null;
            switch(name)
            {
                case "get_pos_x":Verdant.Automation.Call.Require(a,0);r.value=(double)drone.x;break;case "get_pos_y":Verdant.Automation.Call.Require(a,0);r.value=(double)drone.y;break;case "get_world_size":Verdant.Automation.Call.Require(a,0);r.value=(double)Farm.Data.size;break;
                case "get_entity_type":Verdant.Automation.Call.Require(a,0);r.value=cell.entity==""?null:cell.entity;break;case "get_ground_type":Verdant.Automation.Call.Require(a,0);r.value=cell.ground;break;case "get_water":Verdant.Automation.Call.Require(a,0);r.value=cell.water;break;
                case "num_items":Verdant.Automation.Call.Require(a,1);r.value=Farm.Count(Convert.ToString(a[0]));break;case "num_unlocked":Verdant.Automation.Call.Require(a,1);r.value=(double)Farm.Level(Convert.ToString(a[0]).Replace("unlock:",""));break;
                case "get_cost":if(a.Length<1||a.Length>2)throw new Exception("get_cost expects 1 or 2 arguments.");string priceId=Convert.ToString(a[0]);r.value=priceId.StartsWith("unlock:")?Farm.Cost(priceId.Substring(7),a.Length==2?ScriptValue.Integer(a[1]):-1):Farm.SeedCost(priceId);break;
                case "unlock":Verdant.Automation.Call.Require(a,1);int before=Farm.Data.size;r.value=Farm.Unlock(Convert.ToString(a[0]).Replace("unlock:",""));if(Farm.Data.size!=before)foreach(var u in session.drones){u.x=Farm.Wrap(u.x);u.y=Farm.Wrap(u.y);}action=true;break;
                case "can_harvest":Verdant.Automation.Call.Require(a,0);r.value=Farm.Ready(cell)||Farm.maze&&Index==Farm.treasure;break;
                case "can_move":Verdant.Automation.Call.Require(a,1);r.value=Farm.CanMove(Index,Direction(a[0]),drone.hat=="dinosaur");break;
                case "move":
                    Verdant.Automation.Call.Require(a,1);int dir=Direction(a[0]),old=Index;bool snake=drone.hat=="dinosaur";r.value=Farm.CanMove(old,dir,snake);
                    if((bool)r.value){int next=Farm.NeighborIndex(old,dir,!Farm.maze&&!snake);if(snake){bool eat=old==Farm.apple;Farm.tail.Insert(0,old);if(!eat&&Farm.tail.Count>0)Farm.tail.RemoveAt(Farm.tail.Count-1);if(eat)NewApple();}drone.x=next%Farm.Data.size;drone.y=next/Farm.Data.size;}action=true;seconds=snake?Math.Max(.03,1*Math.Pow(.97,Farm.tail.Count)):.5;break;
                case "plant":Verdant.Automation.Call.Require(a,1);r.value=Farm.Plant(Convert.ToString(a[0]),drone.x,drone.y);action=true;break;
                case "harvest":Verdant.Automation.Call.Require(a,0);r.value=Farm.Harvest(drone.x,drone.y);action=true;break;
                case "till":Verdant.Automation.Call.Require(a,0);cell.ground=cell.ground=="soil"?"grassland":"soil";if(cell.entity=="grass"){cell.entity="";cell.growth=0;}action=true;break;
                case "use_item":if(a.Length<1||a.Length>2)throw new Exception("use_item expects 1 or 2 arguments.");r.value=Farm.Use(Convert.ToString(a[0]),drone.x,drone.y,a.Length==2?ScriptValue.Integer(a[1]):1);action=true;break;
                case "get_companion":Verdant.Automation.Call.Require(a,0);r.value=Farm.Companion(drone.x,drone.y);break;
                case "measure":if(a.Length>1)throw new Exception("measure expects at most one direction.");r.value=Measure(a.Length==0?Index:Farm.NeighborIndex(Index,Direction(a[0]),true));break;
                case "swap":Verdant.Automation.Call.Require(a,1);int other=Farm.NeighborIndex(Index,Direction(a[0]),true);r.value=false;if(!Farm.maze&&cell.entity=="cactus"&&Farm.Data.cells[other].entity=="cactus"){var p=Farm.Data.cells[other];Farm.Data.cells[other]=cell;Farm.Data.cells[Index]=p;r.value=true;}action=true;break;
                case "clear":Verdant.Automation.Call.Require(a,0);Farm.Resize(Farm.Data.size);foreach(var u in session.drones){u.x=0;u.y=0;}action=true;break;
                case "set_world_size":Verdant.Automation.Call.Require(a,1);if(Farm.Data.defaultSize==0)Farm.Data.defaultSize=Farm.Data.size;Farm.Resize(ScriptValue.Integer(a[0])==0?Farm.MaxSize:ScriptValue.Integer(a[0]));foreach(var u in session.drones){u.x=0;u.y=0;}break;
                case "set_execution_speed":Verdant.Automation.Call.Require(a,1);session.ExecutionLimit=Math.Max(.05,ScriptValue.Number(a[0]));break;
                case "change_hat":Verdant.Automation.Call.Require(a,1);string hat=Convert.ToString(a[0]);if(hat=="top"&&Farm.Level("top_hat")==0){r.value=false;break;}if(hat=="dinosaur"){if(Farm.Level("dinosaurs")==0||Farm.dinosaur>=0&&Farm.dinosaur!=drone.id){r.value=false;break;}Farm.dinosaur=drone.id;drone.hat=hat;Farm.tail.Clear();NewApple(true);}else{if(drone.hat=="dinosaur"){Farm.Produce("bone",Farm.tail.Count*Farm.tail.Count*Math.Pow(2,Math.Max(0,Farm.Level("dinosaurs")-1)));Farm.tail.Clear();if(Farm.apple>=0)Farm.Data.cells[Farm.apple].entity="";Farm.apple=Farm.dinosaur=-1;}drone.hat=hat;}action=true;r.value=true;break;
                case "get_time":Verdant.Automation.Call.Require(a,0);r.value=Farm.Data.time;break;case "get_tick_count":Verdant.Automation.Call.Require(a,0);r.value=(double)drone.ticks;break;case "random":Verdant.Automation.Call.Require(a,0);r.value=Farm.Random();break;
                case "print":case "quick_print":Farm.Say(string.Join(" ",a.Select(ScriptValue.Show)));seconds=name=="print"?.05:0;break;
                case "do_a_flip":Verdant.Automation.Call.Require(a,0);action=true;break;
                case "num_drones":Verdant.Automation.Call.Require(a,0);r.value=(double)session.drones.Count(d=>!d.finished);break;case "max_drones":Verdant.Automation.Call.Require(a,0);r.value=(double)Farm.MaxDrones;break;
                case "spawn_drone":if(a.Length<1||!(a[0] is ScriptFunction)&&!(a[0] is Native))throw new Exception("spawn_drone expects a function.");var d=session.Spawn(a[0],a.Skip(1).ToArray());if(d!=null){d.x=drone.x;d.y=drone.y;d.program=drone.program;}r.value=d;seconds=.1;break;
                case "has_finished":Verdant.Automation.Call.Require(a,1);if(!(a[0] is ClassicDrone))throw new Exception("Expected a drone handle.");r.value=((ClassicDrone)a[0]).finished;break;
                case "wait_for":Verdant.Automation.Call.Require(a,1);var joined=a[0] as ClassicDrone;if(joined==null||joined==drone)throw new Exception("Invalid drone handle.");while(!joined.finished){if(!session.Running)yield break;yield return new Pulse(line,.01);}r.value=joined.result;break;
                case "leaderboard_run":Verdant.Automation.Call.Require(a,3);foreach(var pulse in session.Leaderboard(Convert.ToString(a[0]),Convert.ToString(a[1]),r,line))yield return pulse;break;
                case "simulate":if(a.Length<1||a.Length>6)throw new Exception("simulate expects 1 to 6 arguments.");foreach(var p in session.Simulate(Convert.ToString(a[0]),a.Length>1?a[1]:null,a.Length>2?a[2]:null,a.Length>3?a[3]:null,a.Length>4?ScriptValue.Integer(a[4]):0,a.Length>5?ScriptValue.Integer(a[5]):64,r,line))yield return p;break;
                default:throw new Exception("Unknown function: "+name);
            }
            if(action){seconds=(seconds>0?seconds:.5)/session.EffectiveSpeed;}
            yield return new Pulse(line,seconds);
        }
        void NewApple(bool first=false)
        {
            if(Farm.apple>=0&&Farm.Data.cells[Farm.apple].entity=="apple")Farm.Data.cells[Farm.apple].entity="";Farm.apple=-1;
            if(Farm.Count("cactus")<1)return;var available=Enumerable.Range(0,Farm.Data.cells.Count).Where(i=>!Farm.tail.Contains(i)&&(Farm.Data.cells[i].entity==""||Farm.Data.cells[i].entity=="grass")).ToArray();if(available.Length==0)return;Farm.Add("cactus",-1);Farm.apple=first?Index:available.Contains(Farm.nextApple)?Farm.nextApple:available[(int)(Farm.Random()*available.Length)];Farm.Data.cells[Farm.apple].entity="apple";var future=available.Where(i=>i!=Farm.apple).ToArray();Farm.nextApple=future.Length==0?-1:future[(int)(Farm.Random()*future.Length)];
        }
    }
    public static class ScriptGraph
    {
        public static object Clone(object root){return Copy(root,new Dictionary<object,object>(new Identity()),0);}
        sealed class Identity:IEqualityComparer<object>{public new bool Equals(object a,object b){return ReferenceEquals(a,b);}public int GetHashCode(object o){return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(o);}}
        static object Copy(object v,Dictionary<object,object> map,int depth)
        {
            if(depth>64||map.Count>100000)throw new Exception("Fork memory limit.");if(v==null||v is string||v.GetType().IsValueType)return v;object found;if(map.TryGetValue(v,out found))return found;
            if(v is Scope){var source=(Scope)v;var scope=new Scope();map[v]=scope;scope.parent=(Scope)Copy(source.parent,map,depth+1);foreach(var p in source.values)scope.values[p.Key]=Copy(p.Value,map,depth+1);foreach(string n in source.globals)scope.globals.Add(n);return scope;}
            if(v is ScriptFunction){var f=(ScriptFunction)v;var copy=new ScriptFunction{name=f.name,parameters=f.parameters,body=f.body};map[v]=copy;copy.closure=(Scope)Copy(f.closure,map,depth+1);copy.defaults=(object[])Copy(f.defaults,map,depth+1);return copy;}
            if(v is List<object>){var list=new List<object>();map[v]=list;foreach(var x in (List<object>)v)list.Add(Copy(x,map,depth+1));return list;}
            if(v is object[]){var a=(object[])v;var copy=new object[a.Length];map[v]=copy;for(int i=0;i<a.Length;i++)copy[i]=Copy(a[i],map,depth+1);return copy;}
            if(v is Dictionary<object,object>){var d=new Dictionary<object,object>(ScriptKeys.Instance);map[v]=d;foreach(var p in (Dictionary<object,object>)v)d[Copy(p.Key,map,depth+1)]=Copy(p.Value,map,depth+1);return d;}
            if(v is HashSet<object>){var s=new HashSet<object>(ScriptKeys.Instance);map[v]=s;foreach(var x in (HashSet<object>)v)s.Add(Copy(x,map,depth+1));return s;}
            return v;
        }
    }
}
