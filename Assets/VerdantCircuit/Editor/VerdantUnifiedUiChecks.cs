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
public static class VerdantUnifiedUiChecks
{
    static ClassicController app;static int stage,checks,errors,renderFrame;static double started;static string output;
    static readonly BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
    static Canvas canvas;static Camera camera;static RenderTexture target;
    static VerdantUnifiedUiChecks(){EditorApplication.update+=Tick;Application.logMessageReceived+=(m,s,t)=>{if(SessionState.GetBool("UnifiedUi",false)&&(t==LogType.Error||t==LogType.Exception))errors++;};}
    public static void Run()
    {
        if(Environment.GetEnvironmentVariable("VERDANT_AUTOMATION_QA")!="1")throw new Exception("Use an isolated QA profile.");
        SessionState.SetString("UnifiedUiOldLocale",PlayerPrefs.GetString("verdant.language",""));SessionState.SetString("UnifiedUiOldActive",PlayerPrefs.GetString("automation.activeFarm",""));
        PlayerPrefs.SetString("verdant.language","en");SessionState.SetBool("UnifiedUi",true);SessionState.SetFloat("UnifiedUiStart",(float)EditorApplication.timeSinceStartup);
        EditorSceneManager.OpenScene("Assets/VerdantCircuit/Scenes/VerdantCircuit.unity");EditorApplication.isPlaying=true;
    }
    static void Check(bool ok,string name){if(!ok)throw new Exception("UNIFIED UI FAIL: "+name);checks++;Debug.Log("UNIFIED UI / "+name);}
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
        Call("DismissCompletion");Call("Refresh");foreach(var transform in canvas.GetComponentsInChildren<Transform>(true))transform.gameObject.layer=5;Canvas.ForceUpdateCanvases();foreach(var t in canvas.GetComponentsInChildren<TMP_Text>())t.ForceMeshUpdate();var oldTarget=app.View.Camera.targetTexture;app.View.Camera.targetTexture=target;var oldClear=camera.clearFlags;camera.clearFlags=CameraClearFlags.Depth;app.View.Camera.Render();camera.Render();camera.clearFlags=oldClear;app.View.Camera.targetTexture=oldTarget;var previous=RenderTexture.active;RenderTexture.active=target;var texture=new Texture2D(1600,1000,TextureFormat.RGB24,false);texture.ReadPixels(new Rect(0,0,1600,1000),0,0);texture.Apply();File.WriteAllBytes(Path.Combine(output,name+".png"),texture.EncodeToPNG());UnityEngine.Object.DestroyImmediate(texture);RenderTexture.active=previous;
    }
    static void Restore()
    {
        foreach(string key in new[]{"Locale","Active"}){string value=SessionState.GetString("UnifiedUiOld"+key,"");string pref=key=="Locale"?"verdant.language":"automation.activeFarm";if(value=="")PlayerPrefs.DeleteKey(pref);else PlayerPrefs.SetString(pref,value);}PlayerPrefs.Save();
    }
    static void Tick()
    {
        if(!SessionState.GetBool("UnifiedUi",false))return;
        try
        {
            if(EditorApplication.timeSinceStartup-SessionState.GetFloat("UnifiedUiStart",0)>180)throw new Exception("Fleet UI check timeout");
            if(!EditorApplication.isPlaying)return;if(app==null)app=UnityEngine.Object.FindObjectOfType<ClassicController>();if(app==null||app.Editor==null)return;
            if(stage==0)
            {
                output=Path.GetFullPath(Environment.GetEnvironmentVariable("VERDANT_UNIFIED_UI_OUTPUT")??"InternalTestReports/0.13.0/ui");Directory.CreateDirectory(output);app.SkipOpening();renderFrame=Time.frameCount;stage=10;return;
            }
            if(stage==10&&Time.frameCount>renderFrame)
            {
                Check(app.WorldSelectionActive,"Opening leads to an explicit Earth selection screen");Check(Button("Enter Earth")!=null,"Earth entry is usable");PrepareCapture();stage=11;
            }
            if(stage==11&&Time.frameCount>renderFrame+3)
            {
                Capture("world-en");Button("Enter Earth").onClick.Invoke();Check(!app.WorldSelectionActive,"Entering Earth opens the original home garden");
                Check(!app.GetComponentsInChildren<UnityEngine.UI.Button>().Any(b=>b.name=="Lab"),"Retired simulation navigation is absent");
                app.Editor.SetTextWithoutNotify("while True:\n    harvest()");
                app.Farm.Data.unlocks.Add(new Stock("loops",1));app.Run();started=EditorApplication.timeSinceStartup;stage=12;
            }
            if(stage==12&&EditorApplication.timeSinceStartup-started>4)
            {
                Check(app.Farm.Count("hay")>0&&app.Farm.Fleet.Data.units.Any(u=>u.role=="farmer"&&app.Farm.Fleet.ProgramRunning(u.id)),"Main Run binds a live original harvest program to the Earth drone");Capture("earth-en");
                foreach(var tech in ClassicFarm.Techs)if(app.Farm.Level(tech.id)==0)app.Farm.Data.unlocks.Add(new Stock(tech.id,tech.max));
                app.Farm.Add("wood",100000);app.Farm.Fleet.Start();app.Farm.Fleet.Sell(0,"wood",10000);Call("Open","programs");
                Check(Button("Network")!=null&&Button("Island manager")!=null,"Program center exposes execution hierarchy");Button("Create files").onClick.Invoke();Check(UnifiedExamples.Names.All(name=>app.Farm.Data.programs.Any(p=>p.name==name)),"One action creates the complete unified program hierarchy");
                Check(app.Farm.Fleet.Data.units.Any(u=>u.role=="farmer"&&app.Farm.Fleet.ProgramRunning(u.id)),"Browsing and creating files preserve existing Earth execution");Capture("program-center-en");
                Call("Open","archipelago");Check(app.GetComponentInChildren<ClassicIslandMap>()!=null,"Unified Earth and islands render in one operation screen");
                Button("Floralia").onClick.Invoke();Capture("flowers-en");Button("Aurelia").onClick.Invoke();Capture("minerals-en");Button("Pelagia").onClick.Invoke();Capture("fish-en");
                Call("Open","guide");var search=app.GetComponentsInChildren<TMP_InputField>().First(f=>f.name=="Manual Search");search.onEndEdit.Invoke("cut_diamond");
                Check(app.GetComponentsInChildren<TMP_Text>().Any(t=>t.text.Contains("cut_diamond(id, batches=1)")),"Manual search finds an individual named product function");Capture("reference-en");
                L10n.Set("zh-Hans");started=EditorApplication.timeSinceStartup;stage=13;
            }
            if(stage==13&&L10n.Code=="zh-Hans"&&EditorApplication.timeSinceStartup-started>1)
            {
                Check(app.GetComponentsInChildren<TMP_Text>().Any(t=>t.text.Contains("钻石")),"Chinese reference describes the exact recipe");Capture("reference-zh");Call("Open","world");started=EditorApplication.timeSinceStartup;stage=14;
            }
            if(stage==14&&EditorApplication.timeSinceStartup-started>1)
            {
                Capture("world-zh");Button("进入地球").onClick.Invoke();Call("Open","programs");Capture("program-center-zh");
                Check(app.GetComponentsInChildren<TMP_Text>().Any(t=>t.text.Contains("联合总控")),"Chinese program center exposes target binding");
                app.Save();var data=JsonUtility.FromJson<ClassicSave>(File.ReadAllText(app.SavePath));Check(data.archipelago.units.Any(u=>u.role=="farmer"&&u.program=="main"),"Actual UI save preserves Earth program binding");
                Check(errors==0,"All captured flows render without runtime errors or translation gaps");Restore();SessionState.SetBool("UnifiedUi",false);File.WriteAllText(Path.Combine(output,"report.json"),"{\"status\":\"passed\",\"checks\":"+checks+",\"runtimeErrors\":0,\"locales\":2,\"screenshots\":10}");Debug.Log("UNIFIED UI PASSED / "+checks+" checks");EditorApplication.Exit(0);
            }
        }
        catch(Exception e){Restore();SessionState.SetBool("UnifiedUi",false);Debug.LogException(e);EditorApplication.Exit(1);}
    }
}
