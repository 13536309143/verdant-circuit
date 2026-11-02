using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Verdant.Automation
{
    public sealed class ClassicDrag:MonoBehaviour,IBeginDragHandler,IDragHandler
    {public RectTransform target;Vector2 offset;public void OnBeginDrag(PointerEventData e){offset=(Vector2)target.position-e.position;}public void OnDrag(PointerEventData e){target.position=e.position+offset;}}
    public sealed partial class ClassicView:MonoBehaviour
    {
        public Camera Camera;public int SelectedPlanet;public bool ShowUniverse,FocusPlot;
        public int AnimatedWorkerCount {get{return agents.Count;}}public int RenderedPlots {get{return drawnWidth*drawnHeight;}}
        public double LastViewCpuMilliseconds {get;private set;}
        public int VisibleCropInstances {get;private set;}public int CropDrawCalls {get;private set;}public int EffectCount {get;private set;}
        UnityEngine.Rendering.CommandBuffer cropCommands;
        ClassicController app;Transform[] spheres;Transform plotRoot;Mesh gridMesh;Material gridMaterial;
        readonly Dictionary<int,Transform> agents=new Dictionary<int,Transform>();readonly Dictionary<int,Transform[]> motors=new Dictionary<int,Transform[]>();readonly Dictionary<int,Transform> ufos=new Dictionary<int,Transform>();
        readonly List<Material> materials=new List<Material>();readonly Dictionary<string,Material> materialCache=new Dictionary<string,Material>();readonly List<Mesh> ownedMeshes=new List<Mesh>();
        static readonly Vector3[] Orbits={new Vector3(-39,9,0),new Vector3(-11,-12,12),new Vector3(21,11,2),new Vector3(49,-10,12)};
        public static readonly Vector3 MothershipPosition=new Vector3(5,33,-12);
        int drawnPlanet=-1,drawnWidth,drawnHeight;float cropRefresh;float azimuth,elevation=-32,distance=54;Vector3 lastMouse;
        public const float Radius=18;
        public static Vector3 SurfacePoint(int width,int height,float x,float y,float radius=Radius)
        {float lon=(x+.5f-width*.5f)*Mathf.Deg2Rad,lat=(y+.5f-height*.5f)*Mathf.Deg2Rad;return new Vector3(Mathf.Sin(lon)*Mathf.Cos(lat),Mathf.Sin(lat),-Mathf.Cos(lon)*Mathf.Cos(lat))*radius;}
        Material MakeMaterial(Color color,string shader="Standard")
        {
            string key=shader+ColorUtility.ToHtmlStringRGBA(color);Material m;if(materialCache.TryGetValue(key,out m))return m;
            Shader sh=shader.StartsWith("Verdant/")?Resources.Load<Shader>(shader.Substring(8)):Shader.Find(shader);
            m=new Material(sh);if(m.HasProperty("_Tint"))m.SetColor("_Tint",color);else m.color=color;m.enableInstancing=true;materials.Add(m);materialCache[key]=m;return m;
        }
        Transform Primitive(string name,PrimitiveType type,Transform parent,Vector3 pos,Vector3 scale,Color color)
        {var go=GameObject.CreatePrimitive(type);go.name=name;go.transform.SetParent(parent,false);go.transform.localPosition=pos;go.transform.localScale=scale;var r=go.GetComponent<Renderer>();r.sharedMaterial=MakeMaterial(color);r.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;r.receiveShadows=false;Destroy(go.GetComponent<Collider>());return go.transform;}
        public void Initialize(ClassicController controller)
        {
            app=controller;SelectedPlanet=app.Farm.Data.selectedPlanet;FocusPlot=app.Farm.Fleet.SurfaceWidth(SelectedPlanet)<=8;
            var sky=new GameObject("Universe Background",typeof(Camera)).GetComponent<Camera>();sky.transform.SetParent(transform);sky.depth=-10;sky.clearFlags=CameraClearFlags.SolidColor;sky.backgroundColor=ColorOf("060A13");sky.cullingMask=0;
            Camera=new GameObject("Orbit Camera",typeof(Camera),typeof(AudioListener)).GetComponent<Camera>();Camera.transform.SetParent(transform);Camera.clearFlags=CameraClearFlags.SolidColor;Camera.backgroundColor=ColorOf("060A13");Camera.fieldOfView=42;Camera.nearClipPlane=.03f;Camera.farClipPlane=1000;cropCommands=new UnityEngine.Rendering.CommandBuffer{name="Visible Crops and Pooled Harvest"};Camera.AddCommandBuffer(UnityEngine.Rendering.CameraEvent.AfterForwardOpaque,cropCommands);
            var sun=new GameObject("Solar Key",typeof(Light)).GetComponent<Light>();sun.transform.SetParent(transform);sun.type=LightType.Directional;sun.transform.rotation=Quaternion.Euler(35,-35,0);sun.intensity=1.2f;sun.shadows=LightShadows.None;
            spheres=new Transform[4];for(int i=0;i<4;i++){var go=new GameObject("Planet "+i,typeof(MeshFilter),typeof(MeshRenderer));go.transform.SetParent(transform);spheres[i]=go.transform;go.GetComponent<MeshFilter>().sharedMesh=Globe(i);var material=new Material(Resources.Load<Shader>("PlanetSurface"));material.SetFloat("_Clouds",i==3?.1f:.35f);materials.Add(material);go.GetComponent<MeshRenderer>().sharedMaterial=material;}
            plotRoot=new GameObject("Unlocked Spherical Plots",typeof(MeshFilter),typeof(MeshRenderer)).transform;plotRoot.SetParent(transform);gridMaterial=MakeMaterial(Color.white,"Verdant/PlanetSurface");plotRoot.GetComponent<MeshRenderer>().sharedMaterial=gridMaterial;
            BuildSpaceArt();InitializeHarvestEffects();app.Farm.Harvested+=OnHarvest;app.Farm.Fleet.ProductionCompleted+=OnProduction;
        }
        Mesh Globe(int planet)
        {
            const int segments=96,rings=48;var vertices=new List<Vector3>();var normals=new List<Vector3>();var colors=new List<Color>();var indices=new List<int>();
            for(int y=0;y<=rings;y++)for(int x=0;x<=segments;x++){float lon=x*Mathf.PI*2/segments,lat=-Mathf.PI*.5f+y*Mathf.PI/rings;var n=new Vector3(Mathf.Sin(lon)*Mathf.Cos(lat),Mathf.Sin(lat),-Mathf.Cos(lon)*Mathf.Cos(lat));vertices.Add(n*Radius);normals.Add(n);float noise=Mathf.PerlinNoise(n.x*2.9f+planet*3+5,n.z*2.9f+n.y*3+7);Color c=planet==0?ColorOf(noise>.48f?"5AA98C":"296C90"):planet==1?ColorOf(noise>.53f?"C46A85":"408B69"):planet==2?ColorOf(noise>.55f?"4287B7":"173E72"):ColorOf(noise>.48f?"B6C2CB":"6B7A8D");if(Mathf.Abs(n.y)>.94f)c=ColorOf("CDDDE0");colors.Add(c);if(y<rings&&x<segments){int a=y*(segments+1)+x;indices.AddRange(new[]{a,a+segments+1,a+1,a+1,a+segments+1,a+segments+2});}}
            var mesh=new Mesh{name="Procedural Planet "+planet};mesh.SetVertices(vertices);mesh.SetNormals(normals);mesh.SetColors(colors);mesh.SetTriangles(indices,0);mesh.RecalculateBounds();return mesh;
        }
        void BuildGrid()
        {
            drawnPlanet=SelectedPlanet;drawnWidth=app.Farm.Fleet.SurfaceWidth(SelectedPlanet);drawnHeight=app.Farm.Fleet.SurfaceHeight(SelectedPlanet);if(gridMesh!=null)Destroy(gridMesh);gridMesh=new Mesh{name="Unlocked 1 Degree Cells",indexFormat=UnityEngine.Rendering.IndexFormat.UInt32};var vertices=new List<Vector3>(drawnWidth*drawnHeight*4);var normals=new List<Vector3>();var colors=new List<Color>();var indices=new List<int>();
            var corners=new[]{new Vector2(-.45f,-.45f),new Vector2(-.45f,.45f),new Vector2(.45f,.45f),new Vector2(.45f,-.45f)};
            for(int y=0;y<drawnHeight;y++)for(int x=0;x<drawnWidth;x++){int start=vertices.Count;foreach(var corner in corners){var point=SurfacePoint(drawnWidth,drawnHeight,x+corner.x,y+corner.y,Radius+.04f);vertices.Add(point);normals.Add(point.normalized);colors.Add(ColorOf(SelectedPlanet==0?"346D58":SelectedPlanet==1?"436349":SelectedPlanet==2?"245888":"70808C"));}indices.Add(start);indices.Add(start+1);indices.Add(start+2);indices.Add(start);indices.Add(start+2);indices.Add(start+3);}
            gridMesh.SetVertices(vertices);gridMesh.SetNormals(normals);gridMesh.SetColors(colors);gridMesh.SetTriangles(indices,0);gridMesh.RecalculateBounds();plotRoot.GetComponent<MeshFilter>().sharedMesh=gridMesh;cropRefresh=0;
        }
        Transform BuildAgent(FleetUnit u)
        {
            var body=new GameObject("Animated "+u.role+" #"+u.id).transform;body.SetParent(transform);var parts=new List<Transform>();
            if(u.role=="bee")
            {Primitive("Bee abdomen",PrimitiveType.Sphere,body,Vector3.zero,new Vector3(.23f,.14f,.32f),ColorOf("EBC664"));Primitive("Bee stripe",PrimitiveType.Cube,body,new Vector3(0,0,.025f),new Vector3(.235f,.145f,.045f),ColorOf("443B34"));for(int i=0;i<2;i++){var wing=Primitive("Flapping wing",PrimitiveType.Sphere,body,new Vector3(i==0?-.13f:.13f,.06f,0),new Vector3(.24f,.025f,.14f),ColorOf("D6E9EE"));parts.Add(wing);}}
            else if(u.role=="boat")
            {Primitive("Fishing hull",PrimitiveType.Cube,body,Vector3.zero,new Vector3(.27f,.07f,.42f),ColorOf("D5E6EC"));Primitive("Cabin",PrimitiveType.Cube,body,new Vector3(0,.09f,-.07f),new Vector3(.16f,.14f,.17f),ColorOf("3D789B"));parts.Add(Primitive("Propeller",PrimitiveType.Cylinder,body,new Vector3(0,-.04f,.24f),new Vector3(.12f,.015f,.025f),ColorOf("879CA6")));}
            else
            {Primitive("Drone chassis",PrimitiveType.Cube,body,Vector3.zero,new Vector3(.23f,.09f,.25f),ColorOf(u.role=="mining_drone"?"AFBBC9":"BCE5DB"));Primitive("Signal visor",PrimitiveType.Cube,body,new Vector3(0,.02f,-.13f),new Vector3(.13f,.035f,.015f),ColorOf("3DA7C7"));for(int i=0;i<4;i++)parts.Add(Primitive("Rotor",PrimitiveType.Cube,body,new Vector3(i%2==0?-.17f:.17f,.07f,i<2?-.17f:.17f),new Vector3(.18f,.015f,.045f),ColorOf("608A9E")));if(u.role=="mining_drone")parts.Add(Primitive("Drill",PrimitiveType.Cylinder,body,new Vector3(0,-.10f,.07f),new Vector3(.08f,.08f,.08f),ColorOf("DFBE75")));}
            motors[u.id]=parts.ToArray();return body;
        }
        public void SelectSurface(int planet){SelectedPlanet=planet;FocusPlot=app.Farm.Fleet.SurfaceWidth(planet)<=8;azimuth=0;elevation=-32;cropRefresh=0;}
        void Update()
        {
            if(app==null||app.Farm==null||Camera==null)return;long frameStarted=System.Diagnostics.Stopwatch.GetTimestamp();var world=app.Farm.Fleet;float dt=Time.unscaledDeltaTime;
            UpdateSpaceArt(dt);
            for(int p=0;p<4;p++){spheres[p].gameObject.SetActive(ShowUniverse||p==SelectedPlanet);spheres[p].position=ShowUniverse?Orbits[p]:Vector3.zero;spheres[p].rotation=ShowUniverse?Quaternion.AngleAxis(Time.unscaledTime*spinRates[p],spinAxes[p]):Quaternion.identity;spheres[p].GetComponent<Renderer>().sharedMaterial.SetFloat("_Locked",world.IsUnlocked(p)?0:1);}
            Camera.rect=ShowUniverse?new Rect(0,0,1,1):new Rect(.025f,.265f,.595f,.44f);Camera.backgroundColor=ColorOf("060A13");
            if(ShowUniverse){if(!Landing) {Camera.transform.position=new Vector3(4,14,-155);Camera.transform.LookAt(new Vector3(4,0,8));}UpdateLanding(dt);}
            else
            {
                if(drawnPlanet!=SelectedPlanet||drawnWidth!=world.SurfaceWidth(SelectedPlanet)||drawnHeight!=world.SurfaceHeight(SelectedPlanet))BuildGrid();
                bool uiPointer=EventSystem.current!=null&&EventSystem.current.IsPointerOverGameObject();if(!uiPointer&&Input.GetMouseButtonDown(1))lastMouse=Input.mousePosition;
                if(!uiPointer&&Input.GetMouseButton(1)&&Camera.pixelRect.Contains(Input.mousePosition)){var delta=Input.mousePosition-lastMouse;azimuth+=delta.x*.18f;elevation=Mathf.Clamp(elevation+delta.y*.18f,-65,65);lastMouse=Input.mousePosition;cropRefresh=0;}
                if(!uiPointer&&Camera.pixelRect.Contains(Input.mousePosition)&&Math.Abs(Input.mouseScrollDelta.y)>.01f){distance=Mathf.Clamp(distance-Input.mouseScrollDelta.y*2,Radius+1.5f,75);cropRefresh=0;}
                FleetUnit worker=null;foreach(var candidate in world.Data.units)if(candidate.island==SelectedPlanet){worker=candidate;break;}
                Vector3 ground=SurfacePoint(drawnWidth,drawnHeight,worker?.x??0,worker?.y??0);Vector3 normal=ground.normalized;
                if(FocusPlot){var target=ground+normal*.12f;var local=Quaternion.FromToRotation(Vector3.back,normal)*Quaternion.Euler(-elevation,azimuth,0)*Vector3.back;Camera.transform.position=target+local*2.5f;Camera.transform.LookAt(target,Quaternion.FromToRotation(Vector3.back,normal)*Vector3.up);}
                else{Camera.transform.position=Quaternion.Euler(-elevation,azimuth,0)*Vector3.back*distance;Camera.transform.LookAt(Vector3.zero,Vector3.up);}
            }
            plotRoot.gameObject.SetActive(!ShowUniverse);cropRefresh-=dt;
            cropCommands.Clear();if(!ShowUniverse&&SelectedPlanet==0){if(cropRefresh<=0){cropRefresh=FocusPlot?.10f:.25f;BuildCropInstances();}DrawCrops();}else{VisibleCropInstances=0;CropDrawCalls=0;}
            foreach(var u in world.Data.units)
            {
                Transform body;if(!agents.TryGetValue(u.id,out body)){body=BuildAgent(u);agents[u.id]=body;body.position=SurfacePoint(world.SurfaceWidth(u.island),world.SurfaceHeight(u.island),u.x,u.y);}
                body.gameObject.SetActive(!ShowUniverse&&u.island==SelectedPlanet);if(!body.gameObject.activeSelf)continue;
                var position=SurfacePoint(world.SurfaceWidth(u.island),world.SurfaceHeight(u.island),u.x+(u.width-1)*.5f,u.y+(u.height-1)*.5f,Radius+(u.role=="boat"?.11f:.58f));body.position=Vector3.Lerp(body.position,position,1-Mathf.Exp(-dt*14));body.rotation=Quaternion.FromToRotation(Vector3.up,position.normalized);body.localScale=new Vector3(u.width,1,u.height);
                var animated=motors[u.id];for(int i=0;i<animated.Length;i++){if(u.role=="bee")animated[i].localRotation=Quaternion.Euler(0,0,Mathf.Sin(Time.unscaledTime*48)*(i==0?50:-50));else animated[i].Rotate(u.role=="boat"?Vector3.forward:Vector3.up,dt*(u.role=="boat"?720:1800));}
            }
            staleAgents.Clear();foreach(var pair in agents)if(world.Unit(pair.Key)==null)staleAgents.Add(pair.Key);foreach(int id in staleAgents){Destroy(agents[id].gameObject);agents.Remove(id);motors.Remove(id);}
            DrawHarvestEffects(dt);LastViewCpuMilliseconds=(System.Diagnostics.Stopwatch.GetTimestamp()-frameStarted)*1000.0/System.Diagnostics.Stopwatch.Frequency;
        }
        readonly List<int> staleAgents=new List<int>();
        public void RefreshPalette(){}
        public static Color ColorOf(string hex){Color c;ColorUtility.TryParseHtmlString("#"+hex,out c);return c;}
        void OnDestroy(){if(cropCommands!=null){if(Camera!=null)Camera.RemoveCommandBuffer(UnityEngine.Rendering.CameraEvent.AfterForwardOpaque,cropCommands);cropCommands.Release();}if(app!=null&&app.Farm!=null){app.Farm.Harvested-=OnHarvest;app.Farm.Fleet.ProductionCompleted-=OnProduction;}foreach(var material in materials)if(material!=null)Destroy(material);foreach(var mesh in ownedMeshes)if(mesh!=null)Destroy(mesh);if(gridMesh!=null)Destroy(gridMesh);if(spheres!=null)foreach(var sphere in spheres)if(sphere!=null){var mesh=sphere.GetComponent<MeshFilter>().sharedMesh;if(mesh!=null)Destroy(mesh);}}
    }
}
