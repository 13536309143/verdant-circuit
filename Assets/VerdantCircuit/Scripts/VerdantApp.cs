using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Verdant
{
    public sealed class VerdantApp : MonoBehaviour
    {
        public TMP_FontAsset uiFont,codeFont;
        public VerdantFarm Farm;public VerdantRunner Runner;public VerdantWorld World;public VerdantUI UI;public VerdantAudio Audio;
        public int RunningSlot=-1;public int Speed=1;public bool PausedWorld;public int IncomeRate;
        float saveTimer,inputTimer,incomeTimer;int lastEarned;bool wasChallenge;Vector2Int? navigate;
        readonly Queue<Vector2> income=new Queue<Vector2>();
        void Start()
        {
            Application.targetFrameRate=60;Farm=new VerdantFarm();Runner=new VerdantRunner(Farm);
            Audio=new GameObject("Sound Garden").AddComponent<VerdantAudio>();Audio.transform.SetParent(transform);Audio.Initialize(Farm.Data.volume);
            World=new GameObject("World").AddComponent<VerdantWorld>();World.transform.SetParent(transform);World.Initialize(this);
            UI=new GameObject("UI").AddComponent<VerdantUI>();UI.transform.SetParent(transform);UI.Initialize(this,uiFont,codeFont);
            if(Farm.Count("intro")==0)UI.Open("intro");
            wasChallenge=Farm.InChallenge;lastEarned=Farm.Count("earned");income.Enqueue(new Vector2(Farm.Data.clock,lastEarned));
        }
        void Update()
        {
            if(Farm==null)return;float dt=Time.unscaledDeltaTime;
            if(!PausedWorld)
            {
                Farm.Tick(dt*Speed);Runner.Tick(dt*Speed);
                if(wasChallenge&&!Farm.InChallenge){Runner.Stop();UI.Editor.SetTextWithoutNotify(Farm.Data.scripts[Farm.Data.slot]);}
                wasChallenge=Farm.InChallenge;
                if(!UI.Typing&&!Runner.Running)ManualInput(dt);
                incomeTimer+=dt*Speed;if(incomeTimer>=3){incomeTimer=0;int earned=Farm.Count("earned");income.Enqueue(new Vector2(Farm.Data.clock,earned));while(income.Count>1&&income.Peek().x<Farm.Data.clock-60)income.Dequeue();Vector2 first=income.Peek();IncomeRate=Mathf.Max(0,Mathf.RoundToInt((earned-first.y)/Mathf.Max(1,Farm.Data.clock-first.x)*60));lastEarned=earned;}
            }
            if(Input.GetKeyDown(KeyCode.Escape)){if(UI.ModalOpen)UI.Close();else UI.Open("settings");}
            if((Input.GetKey(KeyCode.LeftCommand)||Input.GetKey(KeyCode.RightCommand)||Input.GetKey(KeyCode.LeftControl)||Input.GetKey(KeyCode.RightControl))&&Input.GetKeyDown(KeyCode.Return))RunCode();
            if(UI.Typing&&Input.GetKeyDown(KeyCode.Tab))
            {int p=UI.Editor.stringPosition;UI.Editor.text=UI.Editor.text.Insert(p,"    ");UI.Editor.stringPosition=p+4;}
            saveTimer+=dt;if(saveTimer>30){saveTimer=0;Farm.Save();}
        }
        void ManualInput(float dt)
        {
            inputTimer-=dt;
            if(inputTimer<=0)
            {
                string direction=null;if(Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.W))direction="North";else if(Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.S))direction="South";else if(Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.A))direction="West";else if(Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.D))direction="East";
                if(direction!=null){Farm.Act("move",direction);navigate=null;inputTimer=.16f;}
                else if(navigate.HasValue)
                {var target=navigate.Value;if(Farm.Data.x!=target.x)Farm.Act("move",Farm.Data.x<target.x?"East":"West");else if(Farm.Data.y!=target.y)Farm.Act("move",Farm.Data.y<target.y?"North":"South");else navigate=null;inputTimer=.13f;}
            }
            if(Input.GetKeyDown(KeyCode.Space))Manual("plant");if(Input.GetKeyDown(KeyCode.E))Manual("harvest");if(Input.GetKeyDown(KeyCode.Q))Manual("water");if(Input.GetKeyDown(KeyCode.T))Manual("till");if(Input.GetKeyDown(KeyCode.F))Manual("fertilize");
            bool overUI=EventSystem.current!=null&&EventSystem.current.IsPointerOverGameObject();
            if(!overUI&&Input.GetMouseButtonDown(0))
            {RaycastHit hit;if(Physics.Raycast(World.Camera.ScreenPointToRay(Input.mousePosition),out hit,150)){var plot=hit.collider.GetComponent<VerdantPlotHit>();if(plot!=null)navigate=new Vector2Int(plot.x,plot.y);}}
            if(!overUI&&Mathf.Abs(Input.mouseScrollDelta.y)>.01f)World.Camera.orthographicSize=Mathf.Clamp(World.Camera.orthographicSize-Input.mouseScrollDelta.y*.35f,3,10);
        }
        public void Manual(string action)
        {
            if(Runner.Running){Farm.Say("请先停止程序再手动操作");return;}
            try{bool result=Farm.Act(action,action=="plant"?VerdantContent.Crops[Farm.Data.selectedCrop].id:"");if(result&&action=="harvest")Audio.PlayCue(620);}catch(Exception e){Farm.Say(e.Message);}
        }
        public void RunCode(){navigate=null;if(UI.ModalOpen)UI.Close();if(Runner.Start(UI.Editor.text))RunningSlot=Farm.Data.slot;}
        public void CycleSpeed(){Speed=Speed==1?2:Speed==2?4:Speed==4?8:1;UI.Refresh();}
        public void NewCampaign()
        {
            Runner.Stop();if(Farm.InChallenge)Farm.FinishChallenge(false);Farm.Save();
            try{if(File.Exists(Farm.SavePath))File.Copy(Farm.SavePath,Farm.SavePath+"."+DateTime.Now.ToString("yyyyMMdd-HHmmss")+".bak",true);}catch(Exception e){Farm.Say("备份失败，已取消新建："+e.Message);return;}
            var fresh=new VerdantFarm(false);Farm.Data=fresh.Data;UI.Editor.SetTextWithoutNotify(Farm.Data.scripts[0]);UI.ResetPrograms();Speed=1;income.Clear();income.Enqueue(Vector2.zero);World.Rebuild();Farm.Notify();Farm.Save();UI.Open("intro");
        }
        public void Quit(){Farm.Save();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying=false;
#else
            Application.Quit();
#endif
        }
        void OnApplicationPause(bool pause){if(pause&&Farm!=null)Farm.Save();}
        void OnApplicationQuit(){if(Farm!=null)Farm.Save();}
    }
}
