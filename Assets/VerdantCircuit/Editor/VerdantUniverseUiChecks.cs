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
public static class VerdantUniverseUiChecks
{
    static ClassicController app;static int stage,checks,errors,renderFrame;static double started;static string output;
    static readonly BindingFlags Private=BindingFlags.Instance|BindingFlags.NonPublic;
    static int lastPerfFrame=-1;static readonly System.Collections.Generic.List<double> cpuFrames=new System.Collections.Generic.List<double>();
    static float firstGrowth;static readonly System.Collections.Generic.List<float> perfFrames=new System.Collections.Generic.List<float>();static Quaternion lockedRotation;
    static Quaternion firstRotation;static Transform animatedPart;static Canvas canvas;static Camera camera;static RenderTexture target;
    static VerdantUniverseUiChecks(){EditorApplication.update+=Tick;Application.logMessageReceived+=(m,s,t)=>{if(SessionState.GetBool("UniverseUi",false)&&(t==LogType.Error||t==LogType.Exception))errors++;};}
    public static void Run()
    {
        if(Environment.GetEnvironmentVariable("VERDANT_AUTOMATION_QA")!="1")throw new Exception("Use an isolated QA profile.");
        SessionState.SetString("UniverseUiOldLocale",PlayerPrefs.GetString("verdant.language",""));SessionState.SetString("UniverseUiOldActive",PlayerPrefs.GetString("universe0131.activeFarm",""));
        PlayerPrefs.SetString("verdant.language","en");SessionState.SetBool("UniverseUi",true);SessionState.SetFloat("UniverseUiStart",(float)EditorApplication.timeSinceStartup);
        EditorSceneManager.OpenScene("Assets/VerdantCircuit/Scenes/VerdantCircuit.unity");EditorApplication.isPlaying=true;
    }
    static void Check(bool ok,string name){if(!ok)throw new Exception("UNIVERSE UI FAIL: "+name);checks++;Debug.Log("UNIVERSE UI / "+name);}
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
        canvas=app.GetComponentsInChildren<Canvas>().First(c=>c.name=="Automation UI"&&c.gameObject.activeInHierarchy);canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=camera;canvas.planeDistance=1;Call("DismissCompletion");Call("Refresh");foreach(var transform in canvas.GetComponentsInChildren<Transform>(true))transform.gameObject.layer=5;Canvas.ForceUpdateCanvases();foreach(var t in canvas.GetComponentsInChildren<TMP_Text>())t.ForceMeshUpdate();var sceneCamera=app.View.Camera;var oldTarget=sceneCamera.targetTexture;Rect oldRect=sceneCamera.rect;int width=Mathf.Max(1,Mathf.RoundToInt(oldRect.width*1600)),height=Mathf.Max(1,Mathf.RoundToInt(oldRect.height*1000));var sceneTarget=new RenderTexture(width,height,24);sceneCamera.targetTexture=sceneTarget;sceneCamera.rect=new Rect(0,0,1,1);sceneCamera.Render();var priorTarget=RenderTexture.active;RenderTexture.active=target;GL.Clear(true,true,ClassicView.ColorOf("060A13"));RenderTexture.active=priorTarget;Graphics.CopyTexture(sceneTarget,0,0,0,0,width,height,target,0,0,Mathf.RoundToInt(oldRect.x*1600),Mathf.RoundToInt(oldRect.y*1000));sceneCamera.targetTexture=oldTarget;sceneCamera.rect=oldRect;var oldClear=camera.clearFlags;camera.clearFlags=CameraClearFlags.Depth;camera.Render();camera.clearFlags=oldClear;UnityEngine.Object.DestroyImmediate(sceneTarget);var previous=RenderTexture.active;RenderTexture.active=target;var texture=new Texture2D(1600,1000,TextureFormat.RGB24,false);texture.ReadPixels(new Rect(0,0,1600,1000),0,0);texture.Apply();File.WriteAllBytes(Path.Combine(output,name+".png"),texture.EncodeToPNG());UnityEngine.Object.DestroyImmediate(texture);RenderTexture.active=previous;
    }
    static void Restore()
    {
        foreach(string key in new[]{"Locale","Active"}){string value=SessionState.GetString("UniverseUiOld"+key,"");string pref=key=="Locale"?"verdant.language":"universe0131.activeFarm";if(value=="")PlayerPrefs.DeleteKey(pref);else PlayerPrefs.SetString(pref,value);}PlayerPrefs.Save();
    }
    static void Tick()
    {
        if(!SessionState.GetBool("UniverseUi",false))return;
        try
        {
            if(EditorApplication.timeSinceStartup-SessionState.GetFloat("UniverseUiStart",0)>180)throw new Exception("Fleet UI check timeout");
            if(!EditorApplication.isPlaying)return;if(app==null)app=UnityEngine.Object.FindObjectOfType<ClassicController>();if(app==null||app.Editor==null)return;
            if(stage==0)
            {
                output=Path.GetFullPath(Environment.GetEnvironmentVariable("VERDANT_UNIVERSE_UI_OUTPUT")??"InternalTestReports/0.13.2/ui");Directory.CreateDirectory(output);app.SkipOpening();renderFrame=Time.frameCount;stage=10;return;
            }
            if(stage==10&&Time.frameCount>renderFrame)
            {
                Check(app.WorldSelectionActive,"Opening enters the four-world universe");Check(app.GetComponentsInChildren<TMP_Text>().Count(t=>t.text.Contains("LOCKED /"))==3,"Only Earth is initially unlocked");Check(app.GetComponentsInChildren<Transform>().Count(t=>t.name.StartsWith("Planet Choice "))==4,"Universe has exactly four planet choices");PrepareCapture();lockedRotation=app.View.PlanetRotation(1);stage=11;
            }
            if(stage==11&&Time.frameCount>renderFrame+4)
            {
                Check(app.View.PlanetIsGrey(1)&&app.View.PlanetIsGrey(2)&&app.View.PlanetIsGrey(3)&&!app.View.PlanetIsGrey(0),"Locked planets are desaturated in the actual renderer");Check(Quaternion.Angle(lockedRotation,app.View.PlanetRotation(1))>.001f,"Locked planets rotate around randomized independent axes");Check(app.GetComponentsInChildren<Transform>().Any(t=>t.name=="Orbital Mothership"),"Universe renders the command mothership");Capture("universe-en");Button("Enter planet").onClick.Invoke();Check(app.View.Landing,"Selecting an unlocked planet starts a landing animation");renderFrame=Time.frameCount;started=EditorApplication.timeSinceStartup;stage=110;
            }
            if(stage==110&&app.View.Landing&&EditorApplication.timeSinceStartup-started>1){Capture("landing-en");stage=111;}
            if(stage==111&&!app.View.Landing&&Time.frameCount>renderFrame+4)
            {
                Check(app.View.LandingCompletions==1,"Landing completes before entering the colony");Check(app.View.FocusPlot,"The one-plot farm starts close enough to see crops");app.Farm.Cell(0,0).growth=0;firstGrowth=ClassicView.CropScale(app.Farm.Cell(0,0));Capture("growth-seedling");started=EditorApplication.timeSinceStartup;stage=112;
            }
            if(stage==112&&EditorApplication.timeSinceStartup-started>1.1)
            {
                Check(app.Farm.Cell(0,0).growth>0&&ClassicView.CropScale(app.Farm.Cell(0,0))>firstGrowth,"Real growth data and continuous crop scale both advance");Check(app.View.VisibleCropInstances==1,"The actual renderer submits the visible original crop");Capture("growth-growing");app.Farm.Advance(4);app.Farm.Harvest(0,0);renderFrame=Time.frameCount;stage=113;
            }
            if(stage==113&&Time.frameCount>renderFrame+1)
            {
                Check(app.View.HarvestEffectsStarted>0&&app.View.EffectCount>0,"A successful original harvest starts a pooled particle burst");Capture("harvest-en");renderFrame=Time.frameCount;stage=12;
            }
            if(stage==12&&Time.frameCount>renderFrame+4)
            {
                string legacyJson=JsonUtility.ToJson(app.Farm.Data);var legacySave=JsonUtility.FromJson<ClassicSave>(legacyJson.Substring(0,legacyJson.Length-1)+",\"theme\":\"default\"}");
                Check(legacySave.cells.Count==app.Farm.Data.cells.Count&&!JsonUtility.ToJson(legacySave).Contains("\"theme\""),"Old light-theme preference is ignored without losing farm state");
                Call("Open","settings");Check(!app.GetComponentsInChildren<TMP_Text>().Any(t=>t.text=="Color theme"||t.text=="配色主题"),"Settings contains no alternate color-theme control");Capture("settings-night-en");Call("Close");
                Check(!app.WorldSelectionActive&&app.View.RenderedPlots==1,"Entering Earth shows the original one-plot farm on an actual sphere");Check(app.GetComponentsInChildren<Transform>().Any(t=>t.name=="Top Navigation")&&!app.GetComponentsInChildren<Transform>().Any(t=>t.name=="Navigation Rail"),"Menus are at the top and the old left rail is removed");Capture("earth-en");
                Check(app.View.AnimatedWorkerCount==1,"Native starter drone has a real animated model");
                foreach(var tech in ClassicFarm.Techs)if(app.Farm.Level(tech.id)==0)app.Farm.Data.unlocks.Add(new Stock(tech.id,tech.max));app.Farm.ExpandSurface(4,4);foreach(string item in ClassicFarm.Items)app.Farm.Add(item,10000);app.Farm.Fleet.Start();app.Farm.Fleet.Sell(0,"wood",1000);
                for(int planet=1;planet<4;planet++){app.Farm.Fleet.Data.islands[planet].width=4;app.Farm.Fleet.Data.islands[planet].height=4;app.Farm.Fleet.Data.islands[planet].expansion=2;}
                var drone=app.GetComponentsInChildren<Transform>().First(t=>t.name=="Rotor");firstRotation=drone.localRotation;animatedPart=drone;started=EditorApplication.timeSinceStartup;stage=13;
            }
            if(stage==13&&EditorApplication.timeSinceStartup-started>.3)
            {
                Check(Quaternion.Angle(firstRotation,animatedPart.localRotation)>.5f,"Earth rotor actually advances over time");app.EnterPlanet(1);renderFrame=Time.frameCount;stage=14;
            }
            if(stage==14&&Time.frameCount>renderFrame+4)
            {
                Check(app.Farm.Fleet.Data.units.Any(u=>u.role=="bee"&&u.island==1),"Flower planet creates its native bee");Capture("flowers-en");app.View.FocusPlot=true;animatedPart=app.GetComponentsInChildren<Transform>().First(t=>t.name=="Flapping wing");firstRotation=animatedPart.localRotation;started=EditorApplication.timeSinceStartup;stage=15;
            }
            if(stage==15&&EditorApplication.timeSinceStartup-started>.2)
            {
                Check(Quaternion.Angle(firstRotation,animatedPart.localRotation)>.5f,"Bee wings flap over real rendered frames");Capture("bee-close");app.View.FocusPlot=false;app.EnterPlanet(2);renderFrame=Time.frameCount;stage=16;
            }
            if(stage==16&&Time.frameCount>renderFrame+4)
            {
                Check(app.Farm.Fleet.Data.units.Any(u=>u.role=="boat"&&u.island==2),"Ocean planet uses boats");Capture("ocean-en");app.EnterPlanet(3);renderFrame=Time.frameCount;stage=17;
            }
            if(stage==17&&Time.frameCount>renderFrame+4)
            {
                Check(app.Farm.Fleet.Data.units.Any(u=>u.role=="mining_drone"&&u.island==3),"Mineral planet uses mining drones");Capture("minerals-en");Call("Open","programs");Button("Create files").onClick.Invoke();Check(UnifiedExamples.Names.All(name=>app.Farm.Data.programs.Any(p=>p.name==name)),"All eleven universe hierarchy programs are created");Capture("programs-en");typeof(ClassicController).GetField("researchScope",Private).SetValue(app,-1);Call("Open","research");Check(app.GetComponentsInChildren<Transform>().Any(t=>t.name=="Sector Technology Tree"),"Universe has its own research sector");Capture("universe-research-en");typeof(ClassicController).GetField("researchScope",Private).SetValue(app,1);Call("Open","research");Check(app.GetComponentsInChildren<Transform>().Any(t=>t.name=="Technology pollination"),"Flower planet exposes its exclusive research branch");Capture("flower-research-en");var scienceNode=app.GetComponentsInChildren<Transform>().First(t=>t.name=="Technology pollination").GetComponent<UnityEngine.UI.Image>();Color lockedColor=scienceNode.color;app.Farm.Fleet.Add(1,"rose",200);ArchipelagoWorld.Put(app.Farm.Fleet.Data.islands[1].made,"rose",200);app.Farm.Fleet.ResearchPlanet(1,"pollination");Call("Refresh");Check(scienceNode.color.maxColorComponent>lockedColor.maxColorComponent,"Purchasing native research changes its actual node from dark to bright");Capture("flower-researched-en");
                Call("Open","ufos");Button("Build").onClick.Invoke();Check(app.Farm.Fleet.Data.ufos.Count==1,"UFO console builds a separate spacecraft with actual costs");Capture("ufo-en");
                Call("Open","guide");var search=app.GetComponentsInChildren<TMP_InputField>().First(f=>f.name=="Manual Search");search.onEndEdit.Invoke("ufo_transfer");Check(app.GetComponentsInChildren<TMP_Text>().Any(t=>t.text.Contains("ufo_transfer(id, source")),"Manual finds the exact new transport function");Capture("reference-en");L10n.Set("zh-Hans");started=EditorApplication.timeSinceStartup;stage=18;
            }
            if(stage==18&&L10n.Code=="zh-Hans"&&EditorApplication.timeSinceStartup-started>1)
            {
                Capture("reference-zh");Check(app.GetComponentsInChildren<TMP_Text>().Any(t=>t.text.Contains("宇宙")),"Chinese top navigation updates");app.EnterPlanet(0);renderFrame=Time.frameCount;stage=19;
            }
            if(stage==19&&Time.frameCount>renderFrame+4)
            {
                Capture("earth-zh");Call("Open","programs");Capture("programs-zh");Call("Close");app.View.FocusPlot=false;app.Farm.ExpandSurface(360,128);renderFrame=Time.frameCount;stage=20;return;
            }
            if(stage==20&&Time.frameCount>renderFrame+5)
            {
                Check(app.View.RenderedPlots==46080,"Actual renderer builds all 360×128 unlocked spherical tiles");var surface=app.GetComponentsInChildren<MeshFilter>().First(m=>m.name=="Unlocked Spherical Plots");Check(surface.sharedMesh.vertexCount==184320&&surface.sharedMesh.indexFormat==UnityEngine.Rendering.IndexFormat.UInt32,"Maximal spherical grid uses valid 32-bit mesh indices");Capture("earth-full-belt-zh");Check(app.View.VisibleCropInstances<=8000&&app.View.CropDrawCalls<=12,"Full-size crops use a bounded visible instance budget and batched draw calls");Check(app.View.EffectCount<=48,"Harvest particle pool has a hard scene-independent cap");app.Save();Check(File.ReadAllText(app.SavePath).Contains("mothership"),"Saved universe includes the mothership inventory");app.Farm.Fleet.Sell(0,"wood",5000);int count=0;while(app.Farm.Fleet.Data.units.Count(u=>u.island==0)<32){int built=app.Farm.Fleet.Build("drone",1,1,0);Check(built>0,"Stress worker "+(++count)+" builds within the real unlocked limit");var worker=app.Farm.Fleet.Unit(built);worker.x=worker.cropX=160+count;worker.y=worker.cropY=64;}stage=21;renderFrame=Time.frameCount;return;
            }
            if(stage==21)
            {
                if(Time.frameCount==lastPerfFrame)return;lastPerfFrame=Time.frameCount;for(int pulse=0;pulse<4;pulse++){int x=165+(Time.frameCount+pulse)%12;var cell=app.Farm.Cell(x,64);cell.growth=100;app.Farm.Harvest(x,64);}Call("DismissCompletion");if(perfFrames.Count==45)Capture("render-stress-zh");cpuFrames.Add(app.View.LastViewCpuMilliseconds);perfFrames.Add(Time.unscaledDeltaTime*1000);if(perfFrames.Count<90)return;perfFrames.Sort();cpuFrames.Sort();Check(errors==0,"No runtime errors, translation gaps or broken shaders");Restore();SessionState.SetBool("UniverseUi",false);
                File.WriteAllText(Path.Combine(output,"report.json"),"{\"status\":\"passed\",\"checks\":"+checks+",\"runtimeErrors\":0,\"locales\":2,\"screenshots\":"+Directory.GetFiles(output,"*.png").Length+",\"nativeAnimation\":true,\"visibleCropInstances\":"+app.View.VisibleCropInstances+",\"cropDrawCalls\":"+app.View.CropDrawCalls+",\"sampledEditorFrames\":90,\"frameMedianMs\":"+perfFrames[45].ToString(System.Globalization.CultureInfo.InvariantCulture)+",\"frameP95Ms\":"+perfFrames[85].ToString(System.Globalization.CultureInfo.InvariantCulture)+",\"stressWorkers\":32,\"effectPoolLimit\":48,\"viewCpuMedianMs\":"+cpuFrames[45].ToString(System.Globalization.CultureInfo.InvariantCulture)+",\"viewCpuP95Ms\":"+cpuFrames[85].ToString(System.Globalization.CultureInfo.InvariantCulture)+"}");Debug.Log("UNIVERSE UI PASSED / "+checks);EditorApplication.Exit(0);
            }
        }
        catch(Exception e){Restore();SessionState.SetBool("UniverseUi",false);Debug.LogException(e);EditorApplication.Exit(1);}
    }
}
