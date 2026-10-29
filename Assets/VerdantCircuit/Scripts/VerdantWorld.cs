using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verdant
{
    public sealed class VerdantPlotHit : MonoBehaviour {public int x,y;}
    public sealed class VerdantWorld : MonoBehaviour
    {
        public VerdantApp App;public Camera Camera;public Transform Robot;
        Transform fieldRoot,robotVisual,marker,clouds;int size;float refresh;
        readonly Dictionary<int,Transform> fleetVisuals=new Dictionary<int,Transform>();
        Transform[] cropRoots;string[] keys;Renderer[] grounds;
        Dictionary<string,Material> mats=new Dictionary<string,Material>();
        MaterialPropertyBlock block;Vector3 lastRobot;Quaternion facing=Quaternion.identity;
        public void Initialize(VerdantApp app)
        {
            App=app;block=new MaterialPropertyBlock();
            RenderSettings.skybox=null;RenderSettings.ambientMode=UnityEngine.Rendering.AmbientMode.Flat;RenderSettings.ambientLight=new Color(.67f,.75f,.72f);
            var backdrop=new GameObject("Background Camera",typeof(Camera)).GetComponent<Camera>();backdrop.transform.SetParent(transform);backdrop.depth=-10;backdrop.clearFlags=CameraClearFlags.SolidColor;backdrop.backgroundColor=Hex("EEF0E5");backdrop.cullingMask=0;
            Camera=new GameObject("Farm Camera",typeof(Camera),typeof(AudioListener)).GetComponent<Camera>();Camera.tag="MainCamera";Camera.transform.SetParent(transform);
            Camera.clearFlags=CameraClearFlags.SolidColor;Camera.backgroundColor=Hex("E9E9DB");Camera.orthographic=true;Camera.nearClipPlane=.1f;Camera.farClipPlane=150;Camera.rect=new Rect(.225f,.12f,.435f,.73f);
            var sun=new GameObject("Afternoon Sun",typeof(Light)).GetComponent<Light>();sun.transform.SetParent(transform);sun.type=LightType.Directional;sun.transform.rotation=Quaternion.Euler(48,-36,0);sun.intensity=1.15f;sun.color=new Color(1,.94f,.83f);sun.shadows=LightShadows.Soft;sun.shadowStrength=.35f;
            QualitySettings.shadows=ShadowQuality.All;QualitySettings.shadowDistance=60;Application.runInBackground=true;
            Rebuild();
        }
        public static Color Hex(string hex){Color c;ColorUtility.TryParseHtmlString("#"+hex,out c);return c;}
        Material Mat(string hex)
        {
            Material m;if(mats.TryGetValue(hex,out m))return m;
            m=new Material(Shader.Find("Standard"));m.name="Verdant "+hex;m.color=Hex(hex);m.SetFloat("_Glossiness",.12f);mats[hex]=m;return m;
        }
        Transform Shape(string name,PrimitiveType type,Vector3 p,Vector3 s,string color,Transform parent,bool collider=false)
        {
            var go=GameObject.CreatePrimitive(type);go.name=name;go.transform.SetParent(parent,false);go.transform.localPosition=p;go.transform.localScale=s;go.GetComponent<Renderer>().sharedMaterial=Mat(color);
            var c=go.GetComponent<Collider>();if(!collider&&c!=null)Destroy(c);return go.transform;
        }
        public Vector3 Position(int x,int y){return new Vector3(x-(size-1)*.5f,.14f,y-(size-1)*.5f);}
        public void Rebuild()
        {
            if(fieldRoot!=null)Destroy(fieldRoot.gameObject);fieldRoot=new GameObject("Living Farm").transform;fieldRoot.SetParent(transform);
            size=App.Farm.Data.size;cropRoots=new Transform[size*size];grounds=new Renderer[size*size];keys=new string[size*size];
            float width=size+2.6f;
            Shape("Earth Foundation",PrimitiveType.Cube,new Vector3(0,-.42f,0),new Vector3(width,.55f,width),"8F7155",fieldRoot);
            Shape("Meadow Rim",PrimitiveType.Cube,new Vector3(0,-.1f,0),new Vector3(width+.15f,.16f,width+.15f),"8BA77E",fieldRoot);
            Shape("Water Horizon",PrimitiveType.Cube,new Vector3(0,-.86f,0),new Vector3(120,.15f,120),"CADBD2",fieldRoot);
            for(int y=0;y<size;y++)for(int x=0;x<size;x++)
            {
                int i=y*size+x;Vector3 p=Position(x,y);
                var tile=Shape("Plot "+x+","+y,PrimitiveType.Cube,p-Vector3.up*.07f,new Vector3(.91f,.14f,.91f),"8D694A",fieldRoot,true);
                var hit=tile.gameObject.AddComponent<VerdantPlotHit>();hit.x=x;hit.y=y;grounds[i]=tile.GetComponent<Renderer>();
                for(int row=0;row<3;row++)Shape("Soil Row",PrimitiveType.Cube,p+new Vector3(0,.005f,(row-1)*.22f),new Vector3(.72f,.015f,.025f),"78573D",fieldRoot);
                cropRoots[i]=new GameObject("Crop "+x+","+y).transform;cropRoots[i].SetParent(fieldRoot,false);cropRoots[i].localPosition=p;
            }
            float edge=size*.5f+.04f;
            Shape("Cedar North",PrimitiveType.Cube,new Vector3(0,.12f,edge),new Vector3(size+.2f,.2f,.09f),"CFAD77",fieldRoot);
            Shape("Cedar South",PrimitiveType.Cube,new Vector3(0,.12f,-edge),new Vector3(size+.2f,.2f,.09f),"CFAD77",fieldRoot);
            Shape("Cedar East",PrimitiveType.Cube,new Vector3(edge,.12f,0),new Vector3(.09f,.2f,size+.2f),"CFAD77",fieldRoot);
            Shape("Cedar West",PrimitiveType.Cube,new Vector3(-edge,.12f,0),new Vector3(.09f,.2f,size+.2f),"CFAD77",fieldRoot);
            for(int i=0;i<size+2;i++)
            {
                float x=i-(size+1)*.5f;float z=size*.5f+.9f;
                Shape("Fence Post",PrimitiveType.Cube,new Vector3(x,.22f,z),new Vector3(.09f,.53f,.09f),"EEE9D7",fieldRoot);
            }
            Shape("Fence Rail",PrimitiveType.Cube,new Vector3(0,.29f,size*.5f+.9f),new Vector3(size+1.3f,.09f,.08f),"EEE9D7",fieldRoot);
            for(int side=-1;side<=1;side+=2)for(int j=0;j<4;j++)
            {
                var p=new Vector3(side*(size*.5f+.8f),.1f,j*(size/3f)-size*.5f);
                var tuft=Shape("Wild Grass",PrimitiveType.Cube,p,new Vector3(.06f,.3f,.12f),"537E5E",fieldRoot);tuft.localRotation=Quaternion.Euler(0,25,25);
                Shape("Wild Flower",PrimitiveType.Sphere,p+new Vector3(.1f,.08f,.04f),Vector3.one*.12f,"F1CE76",fieldRoot);
            }
            BuildStation(new Vector3(-size*.5f-.7f,0,-size*.5f-.6f));
            if(Robot==null)
            {
                Robot=new GameObject("Pip - Farm Robot").transform;Robot.SetParent(transform);robotVisual=new GameObject("Robot Body").transform;robotVisual.SetParent(Robot,false);
                Shape("Track Left",PrimitiveType.Cube,new Vector3(-.22f,.1f,0),new Vector3(.13f,.2f,.49f),"293E3A",robotVisual);
                Shape("Track Right",PrimitiveType.Cube,new Vector3(.22f,.1f,0),new Vector3(.13f,.2f,.49f),"293E3A",robotVisual);
                Shape("Body",PrimitiveType.Cube,new Vector3(0,.22f,0),new Vector3(.38f,.25f,.42f),"F2EBD6",robotVisual);
                Shape("Head",PrimitiveType.Cube,new Vector3(0,.44f,0),new Vector3(.4f,.23f,.33f),"F2EBD6",robotVisual);
                Shape("Face",PrimitiveType.Cube,new Vector3(0,.45f,.174f),new Vector3(.3f,.13f,.018f),"203B36",robotVisual);
                Shape("Eye Left",PrimitiveType.Cube,new Vector3(-.07f,.46f,.19f),new Vector3(.046f,.035f,.01f),"90DFC1",robotVisual);
                Shape("Eye Right",PrimitiveType.Cube,new Vector3(.07f,.46f,.19f),new Vector3(.046f,.035f,.01f),"90DFC1",robotVisual);
                Shape("Top Badge",PrimitiveType.Cube,new Vector3(0,.562f,0),new Vector3(.16f,.016f,.14f),"71B7A1",robotVisual);
                Shape("Antenna",PrimitiveType.Cylinder,new Vector3(.12f,.62f,0),new Vector3(.025f,.07f,.025f),"293E3A",robotVisual);
                Shape("Beacon",PrimitiveType.Sphere,new Vector3(.12f,.71f,0),Vector3.one*.09f,"ECC46A",robotVisual);
                for(int prop=0;prop<4;prop++)Shape("Controller Rotor",PrimitiveType.Cylinder,new Vector3(prop%2==0?-.34f:.34f,.48f,prop<2?-.34f:.34f),new Vector3(.24f,.02f,.24f),"314C47",robotVisual);
                marker=new GameObject("Active Plot").transform;marker.SetParent(transform);
                for(int i=0;i<4;i++){bool horizontal=i<2;Shape("Selection",PrimitiveType.Cube,horizontal?new Vector3(0,0,i==0?-.46f:.46f):new Vector3(i==2?-.46f:.46f,0,0),horizontal?new Vector3(.94f,.022f,.025f):new Vector3(.025f,.022f,.94f),"F2D98D",marker);}
            }
            Robot.position=Position(App.Farm.Data.x,App.Farm.Data.y);lastRobot=Robot.position;facing=Quaternion.Euler(0,180,0);robotVisual.rotation=facing;
            Camera.transform.position=new Vector3(0,12,-10);Camera.transform.LookAt(Vector3.zero);Camera.orthographicSize=(size+3.2f)*.56f;
            refresh=0;
        }
        void BuildStation(Vector3 p)
        {
            Shape("Solar Station",PrimitiveType.Cube,p+Vector3.up*.18f,new Vector3(.55f,.35f,.5f),"D7D8C2",fieldRoot);
            var panel=Shape("Solar Panel",PrimitiveType.Cube,p+new Vector3(0,.48f,.03f),new Vector3(.7f,.05f,.55f),"456A76",fieldRoot);panel.rotation=Quaternion.Euler(18,0,0);
            Shape("Water Barrel",PrimitiveType.Cylinder,p+new Vector3(1,.28f,0),new Vector3(.45f,.29f,.45f),"5B8F8C",fieldRoot);
        }
        void Update()
        {
            if(App==null||App.Farm==null)return;
            if(size!=App.Farm.Data.size){Rebuild();return;}
            var selected=App.Farm.Fleet.Find(App.Farm.Data.activeDrone);
            Vector3 target=Position(selected.x,selected.y)+new Vector3((selected.width-1)*.5f,0,(selected.height-1)*.5f);
            robotVisual.localScale=new Vector3(selected.width,1,selected.height);marker.localScale=new Vector3(selected.width,1,selected.height);
            foreach(var pair in fleetVisuals)pair.Value.gameObject.SetActive(App.Farm.Data.drones.Exists(d=>d.id==pair.Key)&&pair.Key!=selected.id);
            foreach(var unit in App.Farm.Data.drones)if(unit.id!=selected.id)
            {Transform view;if(!fleetVisuals.TryGetValue(unit.id,out view)){view=new GameObject("Drone "+unit.id).transform;view.SetParent(transform);fleetVisuals[unit.id]=view;
              Shape("Worker Body",PrimitiveType.Cube,new Vector3(0,.36f,0),new Vector3(.76f,.35f,.76f),unit.kind=="irrigator"?"5B9DC2":unit.kind=="harvester"?"D9AE64":"D6E3CA",view);
              Shape("Worker Core",PrimitiveType.Sphere,new Vector3(0,.61f,0),new Vector3(.22f,.14f,.22f),"E9F6CC",view);
              for(int prop=0;prop<4;prop++)Shape("Rotor",PrimitiveType.Cylinder,new Vector3(prop%2==0?-.4f:.4f,.57f,prop<2?-.4f:.4f),new Vector3(.28f,.025f,.28f),"314C47",view);}
             view.gameObject.SetActive(true);view.localScale=new Vector3(unit.width,1,unit.height);view.position=Position(unit.x,unit.y)+new Vector3((unit.width-1)*.5f,0,(unit.height-1)*.5f);}

            if(target!=lastRobot)
            {
                Vector3 direction=target-lastRobot;if(direction.magnitude>1.1f)Robot.position=target;else if(direction.sqrMagnitude>.01f)facing=Quaternion.LookRotation(direction);
                lastRobot=target;
            }
            Robot.position=Vector3.Lerp(Robot.position,target,1-Mathf.Exp(-Time.unscaledDeltaTime*18));
            robotVisual.rotation=selected.width*selected.height>1?Quaternion.identity:Quaternion.Slerp(robotVisual.rotation,facing,Time.unscaledDeltaTime*14);
            robotVisual.localPosition=Vector3.up*(Mathf.Sin(Time.unscaledTime*3)*.015f);
            marker.position=target+Vector3.up*.025f;
            refresh-=Time.unscaledDeltaTime;if(refresh>0)return;refresh=.2f;
            for(int i=0;i<keys.Length;i++)
            {
                var p=App.Farm.Data.plots[i];var def=VerdantContent.Crop(p.crop);int stage=def==null?-1:Mathf.Min(3,(int)(p.growth/def.seconds*3));string key=p.crop+":"+stage;
                if(key!=keys[i]){keys[i]=key;for(int child=cropRoots[i].childCount-1;child>=0;child--)Destroy(cropRoots[i].GetChild(child).gameObject);if(def!=null)BuildCrop(cropRoots[i],def,stage,i);}
                Color dry=Hex(p.tilled?"8D694A":"9F8059");Color wet=Hex("584B37");block.SetColor("_Color",Color.Lerp(dry,wet,p.moisture*.45f));grounds[i].SetPropertyBlock(block);
            }
        }
        void BuildCrop(Transform root,CropDef d,int stage,int seed)
        {
            float scale=.4f+stage*.2f;root.localScale=Vector3.one*scale;
            if(stage==0){Shape("Seed",PrimitiveType.Sphere,new Vector3(0,.04f,0),new Vector3(.13f,.06f,.13f),"DDC18B",root);return;}
            if(d.id=="grain"||d.id=="flax")
            {
                for(int i=0;i<4;i++){float x=(i%2-.5f)*.34f,z=(i/2-.5f)*.3f;Shape("Stem",PrimitiveType.Cube,new Vector3(x,.24f,z),new Vector3(.04f,.48f,.04f),"6F945D",root);var ear=Shape("Ear",PrimitiveType.Cube,new Vector3(x,.52f,z),new Vector3(.13f,.27f,.12f),stage==3?d.color:"A5BB6A",root);ear.localRotation=Quaternion.Euler(8,seed*13,14);}
            }
            else if(d.id=="radish")
            {
                for(int i=0;i<3;i++){float x=(i-1)*.23f;Shape("Root",PrimitiveType.Sphere,new Vector3(x,.11f,0),new Vector3(.22f,.27f,.22f),stage==3?d.color:"A5AC77",root);var leaf=Shape("Leaf",PrimitiveType.Cube,new Vector3(x,.31f,0),new Vector3(.1f,.28f,.05f),"75A36F",root);leaf.localRotation=Quaternion.Euler(0,0,(i-1)*20);}
            }
            else if(d.id=="pumpkin")
            {
                Shape("Vine",PrimitiveType.Cube,new Vector3(0,.035f,0),new Vector3(.7f,.06f,.55f),"69865A",root);
                for(int i=-1;i<=1;i++)Shape("Pumpkin",PrimitiveType.Sphere,new Vector3(i*.12f,.22f,0),new Vector3(.24f,.34f,.41f),stage==3?d.color:"9AAA6B",root);
                Shape("Stalk",PrimitiveType.Cylinder,new Vector3(0,.44f,0),new Vector3(.06f,.07f,.06f),"50734F",root);
            }
            else if(d.id=="sunflower"||d.id=="starbloom")
            {
                Shape("Stem",PrimitiveType.Cylinder,new Vector3(0,.3f,0),new Vector3(.055f,.3f,.055f),"5D8F64",root);
                for(int i=0;i<6;i++){float angle=i*Mathf.PI/3;Shape("Petal",PrimitiveType.Sphere,new Vector3(Mathf.Cos(angle)*.18f,.63f,Mathf.Sin(angle)*.18f),new Vector3(.24f,.1f,.24f),stage==3?d.color:"A6BD8C",root);}
                Shape("Center",PrimitiveType.Sphere,new Vector3(0,.66f,0),new Vector3(.24f,.13f,.24f),d.id=="starbloom"?"F4E5BD":"806744",root);
                var leaf=Shape("Leaf",PrimitiveType.Cube,new Vector3(.15f,.29f,0),new Vector3(.3f,.045f,.14f),"66996C",root);leaf.localRotation=Quaternion.Euler(0,25,18);
            }
            else
            {
                Shape("Stem",PrimitiveType.Cylinder,new Vector3(0,.17f,0),new Vector3(.07f,.17f,.07f),"765C41",root);
                for(int i=0;i<3;i++)Shape("Foliage",PrimitiveType.Sphere,new Vector3((i-1)*.18f,.34f+i*.07f,0),new Vector3(.38f,.42f,.42f),d.id=="tea"?d.color:"6E9B72",root);
                if(d.id=="berry"&&stage==3)for(int i=0;i<5;i++)Shape("Berry",PrimitiveType.Sphere,new Vector3(Mathf.Sin(i*2.4f)*.3f,.4f+(i%2)*.2f,-.18f),Vector3.one*.12f,d.color,root);
            }
        }
        void OnDestroy(){foreach(var m in mats.Values)if(m!=null)Destroy(m);}
    }
}
