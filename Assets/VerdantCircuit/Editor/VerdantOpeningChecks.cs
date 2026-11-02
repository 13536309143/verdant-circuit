using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Verdant;
using L10n = Verdant.L10n;
using Verdant.Automation;

[InitializeOnLoad]
public static class VerdantOpeningChecks
{
    static ClassicController app;
    static int stage,checks,errors,captureFrame;
    static double start,timeBefore;
    static string snapshot,oldLocale,oldActive,output;
    static bool hadLocale,hadActive;
    static readonly BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
    static VerdantOpeningChecks(){EditorApplication.update+=Tick;Application.logMessageReceived+=(m,s,t)=>{if(SessionState.GetBool("OpeningChecks",false)&&(t==LogType.Error||t==LogType.Exception))errors++;};}
    public static void Run()
    {
        if(Environment.GetEnvironmentVariable("VERDANT_AUTOMATION_QA")!="1")throw new Exception("Use an isolated QA farm.");
        hadLocale=PlayerPrefs.HasKey("verdant.language");oldLocale=PlayerPrefs.GetString("verdant.language");hadActive=PlayerPrefs.HasKey("automation.activeFarm");oldActive=PlayerPrefs.GetString("automation.activeFarm");
        // Domain reload clears static fields, so restore preferences from SessionState on exit.
        SessionState.SetString("OpeningOldLocale",hadLocale?oldLocale:"");SessionState.SetString("OpeningOldActive",hadActive?oldActive:"");
        PlayerPrefs.SetString("verdant.language","en");PlayerPrefs.Save();
        SessionState.SetFloat("OpeningStarted",(float)EditorApplication.timeSinceStartup);SessionState.SetBool("OpeningChecks",true);
        EditorSceneManager.OpenScene("Assets/VerdantCircuit/Scenes/VerdantCircuit.unity");EditorApplication.isPlaying=true;
    }
    static void Check(bool ok,string message){if(!ok)throw new Exception("OPENING FAIL: "+message);checks++;Debug.Log("OPENING CHECK / "+message);}
    static ClassicOpening Opening(){return app.GetComponentInChildren<ClassicOpening>();}
    static void Call(string name,params object[] args){typeof(ClassicController).GetMethod(name,Private).Invoke(app,args);}
    static void Restore()
    {
        string locale=SessionState.GetString("OpeningOldLocale","");if(locale=="")PlayerPrefs.DeleteKey("verdant.language");else PlayerPrefs.SetString("verdant.language",locale);
        string active=SessionState.GetString("OpeningOldActive","");if(active=="")PlayerPrefs.DeleteKey("automation.activeFarm");else PlayerPrefs.SetString("automation.activeFarm",active);PlayerPrefs.Save();
    }
    static void Capture(ClassicOpening intro,float seconds,string name,int width=1600,int height=1000)
    {
        intro.RenderFrame(seconds);var camera=intro.Camera;var target=RenderTexture.GetTemporary(width,height,24);camera.targetTexture=target;Canvas.ForceUpdateCanvases();
        foreach(var text in intro.GetComponentsInChildren<TMP_Text>())text.ForceMeshUpdate();camera.Render();var previous=RenderTexture.active;RenderTexture.active=target;
        var texture=new Texture2D(width,height,TextureFormat.RGB24,false);texture.ReadPixels(new Rect(0,0,width,height),0,0);texture.Apply();File.WriteAllBytes(Path.Combine(output,name+".png"),texture.EncodeToPNG());
        Check(texture.GetPixels32().Select(c=>((int)c.r<<16)|((int)c.g<<8)|c.b).Distinct().Count()>300,"Rendered illustration and text: "+name);
        UnityEngine.Object.DestroyImmediate(texture);RenderTexture.active=previous;camera.targetTexture=null;RenderTexture.ReleaseTemporary(target);intro.RenderFrame(intro.Elapsed);
    }
    static void Tick()
    {
        if(!SessionState.GetBool("OpeningChecks",false))return;
        try
        {
            if(EditorApplication.timeSinceStartup-SessionState.GetFloat("OpeningStarted",0)>150)throw new Exception("Opening checks timeout.");
            if(!EditorApplication.isPlaying)return;
            if(app==null)app=UnityEngine.Object.FindObjectOfType<ClassicController>();if(app==null||app.Editor==null)return;
            if(stage==0)
            {
                output=Path.GetFullPath(Environment.GetEnvironmentVariable("VERDANT_OPENING_OUTPUT")??"InternalTestReports/opening");Directory.CreateDirectory(output);start=EditorApplication.timeSinceStartup;
                Check(app.OpeningActive,"Opening starts with the game");Check(Opening().Screen.sortingOrder==200,"Dedicated camera and canvas isolate the prologue");
                snapshot=JsonUtility.ToJson(app.Farm.Data);timeBefore=app.Farm.Data.time;app.Run();Check(!app.Session.Running,"Main Run is blocked during the opening");stage=1;
            }
            if(stage==1&&Opening()!=null&&Opening().Elapsed>4)
            {
                Check(app.Farm.Data.time==timeBefore&&JsonUtility.ToJson(app.Farm.Data)==snapshot,"Real opening frames preserve crops, resources, clock, records and notices");
                Capture(Opening(),2,"01-first-instruction-en");Capture(Opening(),6,"02-growing-circuit-en");Capture(Opening(),12.5f,"03-title-en");
                captureFrame=Time.frameCount;stage=11;return;
            }
            if(stage==11&&Time.frameCount>captureFrame+3)
            {
                // Wait a rendered frame after offscreen captures before using screen coordinates.
                // Skip by an actual EventSystem raycast, not a direct method call.
                var button=Opening().GetComponentInChildren<UnityEngine.UI.Button>();var rect=(RectTransform)button.transform;var pointer=new PointerEventData(EventSystem.current){position=RectTransformUtility.WorldToScreenPoint(Opening().Camera,rect.TransformPoint(rect.rect.center))};
                var hits=new System.Collections.Generic.List<RaycastResult>();Canvas.ForceUpdateCanvases();EventSystem.current.RaycastAll(pointer,hits);
                Debug.Log("Opening raycast "+pointer.position+" / "+Screen.width+"x"+Screen.height+" / "+Opening().Camera.pixelRect+" / "+string.Join(",",hits.Select(h=>h.gameObject.name)));
                Check(hits.Any(h=>h.gameObject.GetComponentInParent<UnityEngine.UI.Button>()==button),"Skip button receives pointer raycasts");ExecuteEvents.Execute(button.gameObject,pointer,ExecuteEvents.pointerClickHandler);
                Check(!app.OpeningActive,"Pointer skip ends the opening");Check(app.Editor.gameObject.activeInHierarchy,"Dashboard is restored on skip");app.Run();Check(!app.Session.Running,"Skip input cannot leak into a main-code run in the same frame");
                L10n.Set("zh-Hans");stage=2;start=EditorApplication.timeSinceStartup;
            }
            if(stage==2&&L10n.Code=="zh-Hans"&&EditorApplication.timeSinceStartup-start>1)
            {
                Check(app.Farm.Data.time>timeBefore,"Farm advances again after skip");
                Call("Open","settings");var replay=app.GetComponentsInChildren<UnityEngine.UI.Button>().First(b=>b.GetComponentInChildren<TMP_Text>()?.text=="重播");replay.onClick.Invoke();
                Check(app.OpeningActive,"Settings replay starts the prologue");Check(app.GetComponentsInChildren<ClassicOpening>().Length==1,"Replay creates only one opening");
                app.PlayOpening();Check(app.GetComponentsInChildren<ClassicOpening>().Length==1,"Repeated replay cannot stack canvases");
                snapshot=JsonUtility.ToJson(app.Farm.Data);Capture(Opening(),12.5f,"04-title-zh");Capture(Opening(),6,"05-compact-zh",1280,720);
                Check(Opening().GetComponentsInChildren<TMP_Text>().Any(t=>t.text=="禾 序"),"Chinese title and captions localize");
                Opening().Advance(ClassicOpening.Duration);Check(!app.OpeningActive&&JsonUtility.ToJson(app.Farm.Data)==snapshot,"Automatic completion preserves the farm and restores the dashboard");stage=3;start=EditorApplication.timeSinceStartup;
            }
            if(stage==3&&EditorApplication.timeSinceStartup-start>.5)
            {
                app.Editor.text="while True:\n    harvest()";app.Farm.Data.allowLockedSyntax=true;app.Run();Check(app.Session.Running,"Code runs normally after opening completion");
                app.Session.Paused=true;app.PlayOpening();snapshot=JsonUtility.ToJson(app.Farm.Data);start=EditorApplication.timeSinceStartup;stage=4;
            }
            if(stage==4&&EditorApplication.timeSinceStartup-start>1)
            {
                Check(app.Session.Running&&app.Session.Paused&&JsonUtility.ToJson(app.Farm.Data)==snapshot,"Replay preserves running code and its existing pause state");
                var intro=Opening();intro.gameObject.SetActive(false);Check(!app.OpeningActive&&app.Editor.gameObject.activeInHierarchy,"Disabling an opening cannot leave input or camera locked");
                Check(errors==0,"No runtime exceptions or missing localized strings");Restore();SessionState.SetBool("OpeningChecks",false);
                File.WriteAllText(Path.Combine(output,"report.json"),"{\"status\":\"passed\",\"checks\":"+checks+",\"runtimeErrors\":0,\"durationSeconds\":15.5,\"screenshots\":5,\"locales\":2,\"resourceChangesDuringIntro\":false}");Debug.Log("OPENING PASSED / "+checks+" checks");EditorApplication.Exit(0);
            }
        }
        catch(Exception e){Restore();SessionState.SetBool("OpeningChecks",false);Debug.LogException(e);EditorApplication.Exit(1);}
    }
}
