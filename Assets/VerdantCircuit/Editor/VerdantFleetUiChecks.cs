using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Verdant.Automation;
using L10n=Verdant.L10n;

[InitializeOnLoad]
public static class VerdantFleetUiChecks
{
    static ClassicController app;static int stage,checks,errors,renderFrame;static double started;static string output;
    static readonly BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
    static Canvas canvas;static Camera camera;static RenderTexture target;
    static VerdantFleetUiChecks(){EditorApplication.update+=Tick;Application.logMessageReceived+=(m,s,t)=>{if(SessionState.GetBool("FleetUi",false)&&(t==LogType.Error||t==LogType.Exception))errors++;};}
    public static void Run()
    {
        if(Environment.GetEnvironmentVariable("VERDANT_AUTOMATION_QA")!="1")throw new Exception("Use an isolated QA profile.");
        SessionState.SetString("FleetUiOldLocale",PlayerPrefs.GetString("verdant.language",""));SessionState.SetString("FleetUiOldActive",PlayerPrefs.GetString("automation.activeFarm",""));
        PlayerPrefs.SetString("verdant.language","en");SessionState.SetBool("FleetUi",true);SessionState.SetFloat("FleetUiStart",(float)EditorApplication.timeSinceStartup);
        EditorSceneManager.OpenScene("Assets/VerdantCircuit/Scenes/VerdantCircuit.unity");EditorApplication.isPlaying=true;
    }
    static void Check(bool ok,string name){if(!ok)throw new Exception("FLEET UI FAIL: "+name);checks++;Debug.Log("FLEET UI / "+name);}
    static void Call(string name,params object[] args){typeof(ClassicController).GetMethod(name,Private).Invoke(app,args);}
    static UnityEngine.UI.Button Button(string text){return app.GetComponentsInChildren<UnityEngine.UI.Button>().First(b=>b.GetComponentInChildren<TMP_Text>()?.text.Contains(text)==true);}
    static void PrepareCapture()
    {
        canvas=app.GetComponentsInChildren<Canvas>().First(c=>c.name=="Automation UI");
        foreach(var transform in canvas.GetComponentsInChildren<Transform>(true))transform.gameObject.layer=5;
        camera=new GameObject("QA UI Capture Camera",typeof(Camera)).GetComponent<Camera>();camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=ClassicView.ColorOf("E8EDE3");camera.cullingMask=1<<5;camera.orthographic=true;camera.nearClipPlane=.1f;camera.farClipPlane=10;
        target=new RenderTexture(1600,1000,24);camera.targetTexture=target;canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=camera;canvas.planeDistance=1;renderFrame=Time.frameCount;
    }
    static void Capture(string name)
    {
        foreach(var transform in canvas.GetComponentsInChildren<Transform>(true))transform.gameObject.layer=5;Canvas.ForceUpdateCanvases();foreach(var t in canvas.GetComponentsInChildren<TMP_Text>())t.ForceMeshUpdate();camera.Render();var previous=RenderTexture.active;RenderTexture.active=target;var texture=new Texture2D(1600,1000,TextureFormat.RGB24,false);texture.ReadPixels(new Rect(0,0,1600,1000),0,0);texture.Apply();File.WriteAllBytes(Path.Combine(output,name+".png"),texture.EncodeToPNG());UnityEngine.Object.DestroyImmediate(texture);RenderTexture.active=previous;
    }
    static void Restore()
    {
        foreach(string key in new[]{"Locale","Active"}){string value=SessionState.GetString("FleetUiOld"+key,"");string pref=key=="Locale"?"verdant.language":"automation.activeFarm";if(value=="")PlayerPrefs.DeleteKey(pref);else PlayerPrefs.SetString(pref,value);}PlayerPrefs.Save();
    }
    static void Tick()
    {
        if(!SessionState.GetBool("FleetUi",false))return;
        try
        {
            if(EditorApplication.timeSinceStartup-SessionState.GetFloat("FleetUiStart",0)>120)throw new Exception("Fleet UI check timeout");
            if(!EditorApplication.isPlaying)return;if(app==null)app=UnityEngine.Object.FindObjectOfType<ClassicController>();if(app==null||app.Editor==null)return;
            if(stage==0)
            {
                output=Path.GetFullPath(Environment.GetEnvironmentVariable("VERDANT_FLEET_UI_OUTPUT")??"InternalTestReports/0.12.1/fleet-ui");Directory.CreateDirectory(output);app.SkipOpening();renderFrame=Time.frameCount;stage=10;return;
            }
            if(stage==10&&Time.frameCount>renderFrame)
            {
                foreach(var tech in ClassicFarm.Techs)app.Farm.Data.unlocks.Add(new Stock(tech.id,tech.max));app.Farm.Add("wood",200);app.Farm.Add("carrot",200);Call("Open","archipelago");
                Check(Button("Sign charter")!=null,"Islands navigation opens the charter gate");Button("Sign charter").onClick.Invoke();Check(app.Farm.Fleet.Data.started&&app.Farm.Count("wood")==100&&app.Farm.Count("carrot")==100,"UI charter pays actual farm resources");
                Button("Build / ").onClick.Invoke();Check(app.Farm.Fleet.Data.units.Count==1,"UI build creates an independently gated physical machine");
                Button("Create modular example").onClick.Invoke();Check(ClassicFleetExample.Names.All(name=>app.Farm.Data.programs.Any(p=>p.name==name)),"One action creates all ten named child programs without changing existing main");
                app.Run();Check(app.Session.Running,"The entry program starts a real coordinator");started=EditorApplication.timeSinceStartup;stage=1;
            }
            if(stage==1&&EditorApplication.timeSinceStartup-started>4)
            {
                var world=app.Farm.Fleet;Check(world.Data.units.Count>=3&&world.Data.units.Any(u=>world.ProgramRunning(u.id)),"Coordinator launches independent specialist interpreters");
                Call("DismissCompletion");Call("Open","programs");Check(app.GetComponentsInChildren<ClassicProgramCard>().Any(c=>c.program=="fleet_worker"&&c.running),"Imported libraries show live fleet users");
                Check(app.GetComponentsInChildren<ClassicProgramCard>().Where(c=>c.running).All(c=>!c.GetComponentsInChildren<UnityEngine.UI.Button>().First(b=>b.GetComponentInChildren<TMP_Text>().text=="Remove").interactable),"Active fleet programs and shared libraries cannot be deleted");
                Call("Open","archipelago");Check(app.GetComponentInChildren<ClassicIslandMap>()!=null,"Physical map and live order render");
                app.Farm.Advance(60);double beforeExport=world.Data.credits;Button("Export 12").onClick.Invoke();Call("Refresh");
                Check(world.Data.credits>beforeExport&&world.Data.credits<beforeExport+1&&app.GetComponentsInChildren<TMP_Text>().Any(t=>t.text.Contains(world.Data.credits.ToString("F2"))),"Fractional export income is visible exactly instead of rounding to an unaffordable whole credit");
                PrepareCapture();stage=2;
            }
            if(stage==2&&Time.frameCount>renderFrame+4)
            {
                Capture("island-operations-en");L10n.Set("zh-Hans");started=EditorApplication.timeSinceStartup;stage=3;
            }
            if(stage==3&&L10n.Code=="zh-Hans"&&EditorApplication.timeSinceStartup-started>1)
            {
                Call("DismissCompletion");Check(app.GetComponentsInChildren<TMP_Text>().Any(t=>t.text.Contains("群岛／调度中心")),"Chinese UI rebuilds with live fleet state preserved");Capture("island-operations-zh");
                app.Save();ArchipelagoWorld.Validate(app.Farm.Fleet.Data);var loaded=JsonUtility.FromJson<ClassicSave>(File.ReadAllText(app.SavePath));Check(loaded.archipelago.units.Count==app.Farm.Fleet.Data.units.Count,"UI save includes physical machines and work queues");
                Check(errors==0,"No runtime exceptions or missing translations");Restore();SessionState.SetBool("FleetUi",false);File.WriteAllText(Path.Combine(output,"report.json"),"{\"status\":\"passed\",\"checks\":"+checks+",\"runtimeErrors\":0,\"locales\":2,\"screenshots\":2,\"modularCode\":true}");Debug.Log("FLEET UI PASSED / "+checks+" checks");EditorApplication.Exit(0);
            }
        }
        catch(Exception e){Restore();SessionState.SetBool("FleetUi",false);Debug.LogException(e);EditorApplication.Exit(1);}
    }
}
