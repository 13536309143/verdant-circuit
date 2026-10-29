using System;
using UnityEngine;

namespace Verdant
{
    public sealed class VerdantRunner
    {
        public FarmVM VM;public VerdantFarm Farm;public bool Running,Paused;public string Status=L10n.T("就绪"),Error="";public int ErrorLine;
        public event Action Changed;float delay;int challengeAtStart;
        public VerdantRunner(VerdantFarm farm){Farm=farm;}
        public bool Start(string source)
        {
            try
            {
                var program=new FarmCompiler().Compile(source);Farm.Fleet.CancelAll();VM=new FarmVM(Farm,program);Running=true;Paused=false;delay=0;Error="";ErrorLine=0;Status=L10n.T("运行中");challengeAtStart=Farm.challenge;Farm.Say(L10n.T("程序启动 · ")+program.code.Count+L10n.T(" 条指令"));Notify();return true;
            }catch(FarmCodeException e){Stop();Error=e.Message;ErrorLine=e.Line;Status=L10n.T("语法错误");Farm.Say(Error);Notify();return false;}
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
                if(VM.Finished){Running=false;Paused=false;Status=L10n.T("已完成");Farm.Say(L10n.T("程序执行完成"));Notify();}
            }catch(FarmCodeException e){Farm.Fleet.CancelAll();Running=false;Paused=false;Error=e.Message;ErrorLine=e.Line;Status=L10n.T("执行错误");Farm.Say(Error);Notify();}
        }
        public void TogglePause(){if(!Running)return;Paused=!Paused;Farm.Data.fleetPaused=Paused;Status=Paused?L10n.T("已暂停"):L10n.T("运行中");Notify();}
        public void Step(string source)
        {
            if(!Running){if(!Start(source))return;Paused=true;}Farm.Data.fleetPaused=true;
            if(!Paused){Paused=true;Status=L10n.T("已暂停");}
            Execute(true);if(Running)Status=L10n.T("单步 · 已暂停");Notify();
        }
        public void Stop(){Farm.Fleet.CancelAll();Running=false;Paused=false;delay=0;Status=L10n.T("已停止");Notify();}
        void Notify(){if(Changed!=null)Changed();}
    }
}
