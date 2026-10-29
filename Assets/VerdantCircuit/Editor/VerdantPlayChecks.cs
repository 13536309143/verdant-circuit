using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Verdant;
using L10n = Verdant.L10n;

// Isolated real-frame UI smoke test. No player save is loaded or overwritten.
[InitializeOnLoad]
public static class VerdantPlayChecks
{
    static double started;static int stage,frame;static float clock;static VerdantApp app;static int errors;static string oldLocale;static bool hadLocale;
    static VerdantPlayChecks(){EditorApplication.update+=Update;Application.logMessageReceived+=Log;}
    public static void Run()
    {
        if(Environment.GetEnvironmentVariable("VERDANT_UI_QA")!="1")throw new Exception("Set VERDANT_UI_QA=1 for the isolated UI test.");
        SessionState.SetBool("VerdantPlayChecks",true);SessionState.SetFloat("VerdantPlayStarted",(float)EditorApplication.timeSinceStartup);
        EditorSceneManager.OpenScene("Assets/VerdantCircuit/Scenes/VerdantCircuit.unity");EditorApplication.isPlaying=true;
    }
    static void Log(string message,string stack,LogType type){if(type==LogType.Exception||type==LogType.Error)errors++;}
    static void Check(bool value,string label){if(!value)throw new Exception("UI FAIL: "+label);}

    static void Update()
    {
        if(!SessionState.GetBool("VerdantPlayChecks",false))return;
        try
        {
            if(EditorApplication.timeSinceStartup-SessionState.GetFloat("VerdantPlayStarted",0)>90)throw new Exception("UI verification timed out.");
            if(!EditorApplication.isPlaying)return;
            if(app==null)app=UnityEngine.Object.FindObjectOfType<VerdantApp>();if(app==null||app.UI==null)return;
            if(stage==0)
            {
                started=EditorApplication.timeSinceStartup;frame=Time.frameCount;clock=app.Farm.Data.clock;hadLocale=PlayerPrefs.HasKey("verdant.language");oldLocale=PlayerPrefs.GetString("verdant.language");
                L10n.Set("en");app.UI.Close();stage=1;return;
            }
            double age=EditorApplication.timeSinceStartup-started;
            if(stage==1&&age>3)
            {
                Check(Time.frameCount>frame+20&&app.Farm.Data.clock>clock+1,"Real frames and simulation advance");
                Check(L10n.Code=="en"&&app.UI.GetComponentsInChildren<TMPro.TMP_Text>().Any(t=>t.text=="Fleet"),"English runtime UI");
                var f=app.Farm;f.Resize(8);f.Data.contract=29;f.Data.coins=100000;f.Data.knowledge=100000;foreach(var t in VerdantContent.Techs)if(!f.Has(t.id))f.Data.tech.Add(t.id);f.Data.droneCapacity=8;f.Data.footprintTier=3;
                f.Fleet.Resize(0,2,2);f.Fleet.Deploy("irrigator",4,0);f.Fleet.Resize(1,3,2);f.Fleet.Deploy("harvester",0,5);f.Fleet.Resize(2,2,3);f.Fleet.Deploy("general",5,5);f.Fleet.Resize(3,3,3);app.World.Rebuild();f.Notify();stage=2;
            }
            if(stage==2&&age>6){var button=app.UI.GetComponentsInChildren<UnityEngine.UI.Button>().First(b=>b.GetComponentInChildren<TMPro.TMP_Text>().text=="Fleet");
                var hits=new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();var pointer=new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current){position=RectTransformUtility.WorldToScreenPoint(null,button.transform.position)};UnityEngine.EventSystems.EventSystem.current.RaycastAll(pointer,hits);
                Check(hits.Count>0&&hits[0].gameObject==button.gameObject,"Fleet button is raycast reachable");button.onClick.Invoke();Check(app.UI.ModalOpen,"Fleet button opens management panel");stage=3;}
            if(stage==3&&age>8){app.UI.Close();L10n.Set("zh-Hans");stage=4;}
            if(stage==4&&age>11){Check(app.UI.GetComponentsInChildren<TMPro.TMP_Text>().Any(t=>t.text=="机群"),"Chinese runtime UI");app.UI.Open("research");stage=5;}
            if(stage==5&&age>14){app.UI.Close();app.Farm.Fleet.Enqueue(1,"water","",4,3);frame=Time.frameCount;clock=app.Farm.Data.clock;stage=6;}
            if(stage==6&&age>19)
            {
                Check(Time.frameCount>frame+20&&app.Farm.Data.clock>clock+1,"Fleet scenario advances actual frames");Check(app.Farm.Fleet.Find(1).y==3&&app.Farm.Fleet.Find(1).jobs.Count==0,"Queued area irrigation in real play");
                Check(errors==0,"No runtime errors");if(hadLocale)PlayerPrefs.SetString("verdant.language",oldLocale);else PlayerPrefs.DeleteKey("verdant.language");PlayerPrefs.Save();
                File.WriteAllText("InternalTestReports/release-0.10.2/playmode.json","{\"actualFrames\":"+(Time.frameCount-frame)+",\"runtimeErrors\":0,\"locales\":2,\"fleetQueueCompleted\":true}");
                SessionState.SetBool("VerdantPlayChecks",false);EditorApplication.Exit(0);
            }
        }
        catch(Exception e){SessionState.SetBool("VerdantPlayChecks",false);Debug.LogException(e);EditorApplication.Exit(1);}
    }
}
