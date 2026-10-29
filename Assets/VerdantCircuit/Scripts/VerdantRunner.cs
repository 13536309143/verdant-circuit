using System;
using UnityEngine;

namespace Verdant
{
    public sealed class VerdantRunner
    {
        public FarmVM VM;public VerdantFarm Farm;public bool Running,Paused;public string Status="就绪",Error="";public int ErrorLine;
        public event Action Changed;float delay;int challengeAtStart;
        public VerdantRunner(VerdantFarm farm){Farm=farm;}
        public bool Start(string source)
        {
            try
            {
                var program=new FarmCompiler().Compile(source);VM=new FarmVM(Farm,program);Running=true;Paused=false;delay=0;Error="";ErrorLine=0;Status="运行中";challengeAtStart=Farm.challenge;Farm.Say("程序启动 · "+program.code.Count+" 条指令");Notify();return true;
            }catch(FarmCodeException e){Stop();Error=e.Message;ErrorLine=e.Line;Status="语法错误";Farm.Say(Error);Notify();return false;}
        }
        public void Tick(float dt)
        {
            if(!Running||Paused||VM==null)return;
            if(Farm.challenge!=challengeAtStart){Stop();return;}
            delay-=dt;if(delay>0)return;
            Execute(false);
        }
        void Execute(bool single)
        {
            try
            {
                int budget=160;while(budget-->0&&!VM.Finished)
                {
                    bool action=VM.Step();if(action){delay=Mathf.Max(0.01f,VM.Wait);break;}
                    if(single&&budget==0)break;
                }
                if(VM.Finished){Running=false;Paused=false;Status="已完成";Farm.Say("程序执行完成");Notify();}
            }catch(FarmCodeException e){Running=false;Paused=false;Error=e.Message;ErrorLine=e.Line;Status="执行错误";Farm.Say(Error);Notify();}
        }
        public void TogglePause(){if(!Running)return;Paused=!Paused;Status=Paused?"已暂停":"运行中";Notify();}
        public void Step(string source)
        {
            if(!Running){if(!Start(source))return;Paused=true;}
            if(!Paused){Paused=true;Status="已暂停";}
            Execute(true);if(Running)Status="单步 · 已暂停";Notify();
        }
        public void Stop(){Running=false;Paused=false;delay=0;Status="已停止";Notify();}
        void Notify(){if(Changed!=null)Changed();}
    }
}
