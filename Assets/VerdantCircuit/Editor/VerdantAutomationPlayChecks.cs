using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Verdant.Automation;
[InitializeOnLoad]
public static class VerdantAutomationPlayChecks
{
    static int stage,frame,errors;static double started;static bool hadLocale;static string oldLocale,oldActive;static bool hadActive;static ClassicController app;
    static VerdantAutomationPlayChecks(){EditorApplication.update+=Update;Application.logMessageReceived+=(m,s,t)=>{if(t==LogType.Error||t==LogType.Exception)errors++;};}
    public static void Run(){if(Environment.GetEnvironmentVariable("VERDANT_AUTOMATION_QA")!="1")throw new Exception("Use VERDANT_AUTOMATION_QA=1.");SessionState.SetBool("AutomationPlay",true);SessionState.SetFloat("AutomationStarted",(float)EditorApplication.timeSinceStartup);EditorSceneManager.OpenScene("Assets/VerdantCircuit/Scenes/VerdantCircuit.unity");EditorApplication.isPlaying=true;}
    static void Check(bool ok,string name){if(!ok)throw new Exception("PLAY FAIL: "+name);}
    static void Update()
    {
        if(!SessionState.GetBool("AutomationPlay",false))return;
        try
        {
            if(EditorApplication.timeSinceStartup-SessionState.GetFloat("AutomationStarted",0)>90)throw new Exception("Automation play test timed out.");if(!EditorApplication.isPlaying)return;if(app==null)app=UnityEngine.Object.FindObjectOfType<ClassicController>();if(app==null||app.Editor==null)return;
            if(stage==0){started=EditorApplication.timeSinceStartup;frame=Time.frameCount;hadLocale=PlayerPrefs.HasKey("verdant.language");oldLocale=PlayerPrefs.GetString("verdant.language");hadActive=PlayerPrefs.HasKey("automation.activeFarm");oldActive=PlayerPrefs.GetString("automation.activeFarm");Verdant.L10n.Set("en");stage=1;return;}
            double age=EditorApplication.timeSinceStartup-started;
            if(stage==1&&age>4){Check(Time.frameCount>frame+20&&app.Farm.Data.time>2,"Real idle frames advance crops");app.Editor.text="harvest()";app.Run();stage=2;}
            if(stage==2&&age>6){Check(app.Farm.Count("hay")>=1,"Fresh game can produce its first resource");foreach(var t in ClassicFarm.Techs)app.Farm.Data.unlocks.Add(new Stock(t.id,t.max));app.Farm.Resize(8);app.Farm.Data.allowLockedSyntax=true;foreach(string i in ClassicFarm.Items)app.Farm.Add(i,100000);app.Editor.text=ClassicExamples.Sources[6];app.Run();stage=3;}
            if(stage==3&&age>9){Check(app.Session.Error==""&&app.Session.TotalSpawned>1,"Independent drone programs execute in play mode");var buttons=app.GetComponentsInChildren<UnityEngine.UI.Button>();var research=buttons.First(b=>b.GetComponentInChildren<TMPro.TMP_Text>().text=="Research");var rect=(RectTransform)research.transform;var pointer=new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current){position=RectTransformUtility.WorldToScreenPoint(null,rect.TransformPoint(rect.rect.center))};var hits=new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();UnityEngine.EventSystems.EventSystem.current.RaycastAll(pointer,hits);Check(hits.Count>0&&hits[0].gameObject.GetComponentInParent<UnityEngine.UI.Button>()==research,"Research button receives UI raycasts");UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(hits[0].gameObject,pointer,UnityEngine.EventSystems.ExecuteEvents.pointerClickHandler);Check(app.GetComponentsInChildren<TMPro.TMP_Text>().Any(t=>t.text.Contains("Field expansion")),"Research panel renders new research");Verdant.L10n.Set("zh-Hans");stage=4;}
            if(stage==4&&age>12){Check(app.GetComponentsInChildren<TMPro.TMP_Text>().Any(t=>t.text=="科技"),"Chinese runtime interface refreshes");app.Save();Check(File.Exists(app.SavePath)&&app.SavePath.Contains("qa-"),"UI test uses an isolated save");stage=5;}
            if(stage==5&&age>15)
            {
                var flags=System.Reflection.BindingFlags.Instance|System.Reflection.BindingFlags.NonPublic;string beforeControl=app.Editor.text;typeof(Verdant.VerdantCodeInput).GetMethod("Append",flags,null,new[]{typeof(char)},null).Invoke(app.Editor,new object[]{(char)27});Check(app.Editor.text==beforeControl,"Escape control characters cannot overwrite code");Check(!app.Editor.restoreOriginalTextOnEscape,"Escape cannot revert the whole editing session");typeof(ClassicController).GetMethod("NewProgram",flags).Invoke(app,new object[]{"helpers","def double(value):\n    return value*2"});var program=app.Farm.Data.programs.Last();typeof(ClassicController).GetMethod("OpenCodeWindow",flags).Invoke(app,new object[]{program});Check(app.GetComponentsInChildren<TMPro.TMP_InputField>().Length>=2,"Floating code window opens");
                typeof(ClassicController).GetMethod("ExportFiles",flags).Invoke(app,null);string folder=(string)typeof(ClassicController).GetProperty("CodeFolder",flags).GetValue(app);Check(File.ReadAllText(Path.Combine(folder,"__builtins__.py")).Contains("class Entities"),"External editor interface hints include enums");File.WriteAllText(Path.Combine(folder,"helpers.py"),"print(42)");typeof(ClassicController).GetMethod("ImportFiles",flags).Invoke(app,null);Check(app.Farm.Data.programs.Last().source=="print(42)","External code reloads");
                typeof(ClassicController).GetMethod("CreateFarm",flags).Invoke(app,new object[]{"qa-profile-"+DateTime.UtcNow.Ticks});Check(app.Farm.Data.size==1&&app.Farm.Count("hay")==0&&app.Farm.Data.programs.Count==1,"A new profile starts with independent progress and code");typeof(ClassicController).GetMethod("LoadFarm",flags).Invoke(app,new object[]{"qa-0.11.1"});Check(app.Farm.Data.size==8&&app.Farm.Data.programs.Count==2,"Farm profiles restore their own progress and code");stage=6;
            }
            if(stage==6&&age>19){Check(errors==0,"No runtime exceptions");if(hadLocale)PlayerPrefs.SetString("verdant.language",oldLocale);else PlayerPrefs.DeleteKey("verdant.language");if(hadActive)PlayerPrefs.SetString("automation.activeFarm",oldActive);else PlayerPrefs.DeleteKey("automation.activeFarm");PlayerPrefs.Save();Directory.CreateDirectory("InternalTestReports/0.11.1");File.WriteAllText("InternalTestReports/0.11.1/playmode.json","{\"frames\":"+(Time.frameCount-frame)+",\"runtimeErrors\":0,\"firstHarvest\":true,\"independentDrones\":true,\"locales\":2,\"profiles\":true,\"externalFiles\":true,\"floatingWindows\":true}");SessionState.SetBool("AutomationPlay",false);EditorApplication.Exit(0);}

        }
        catch(Exception e){if(stage>0){if(hadLocale)PlayerPrefs.SetString("verdant.language",oldLocale);else PlayerPrefs.DeleteKey("verdant.language");if(hadActive)PlayerPrefs.SetString("automation.activeFarm",oldActive);else PlayerPrefs.DeleteKey("automation.activeFarm");PlayerPrefs.Save();}SessionState.SetBool("AutomationPlay",false);Debug.LogException(e);EditorApplication.Exit(1);}
    }
}
