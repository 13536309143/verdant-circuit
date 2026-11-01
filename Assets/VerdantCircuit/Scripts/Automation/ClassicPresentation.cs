using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
namespace Verdant.Automation
{
    public sealed class ClassicDrag:MonoBehaviour,IBeginDragHandler,IDragHandler
    {public RectTransform target;Vector2 offset;public void OnBeginDrag(PointerEventData e){offset=(Vector2)target.position-e.position;}public void OnDrag(PointerEventData e){target.position=e.position+offset;}}
    public sealed class ClassicView:MonoBehaviour
    {
        sealed class CropView {public Transform root;public string key;public Renderer[] parts;public bool infected;}
        ClassicController app;public Camera Camera;Transform root,wallsRoot;int size;float refresh;int[] drawnWalls;int drawnReuse=-1;
        readonly List<CropView> plants=new List<CropView>();readonly List<Renderer> grounds=new List<Renderer>();readonly List<Transform> units=new List<Transform>();readonly List<Renderer> hats=new List<Renderer>();
        readonly Dictionary<string,Material> materials=new Dictionary<string,Material>();MaterialPropertyBlock block;
        Material Material(string hex){Material m;if(!materials.TryGetValue(hex,out m)){m=new Material(Shader.Find("Standard"));m.color=ColorOf(hex);m.enableInstancing=true;materials[hex]=m;}return m;}
        public void Initialize(ClassicController controller)
        {
            app=controller;block=new MaterialPropertyBlock();
            var background=new GameObject("Sky",typeof(Camera)).GetComponent<Camera>();background.transform.SetParent(transform);background.depth=-10;background.clearFlags=CameraClearFlags.SolidColor;background.backgroundColor=ColorOf("EBEFE5");background.cullingMask=0;
            Camera=new GameObject("Automation Camera",typeof(Camera),typeof(AudioListener)).GetComponent<Camera>();Camera.transform.SetParent(transform);Camera.orthographic=true;Camera.clearFlags=CameraClearFlags.SolidColor;Camera.backgroundColor=ColorOf("DAE5D3");Camera.rect=new Rect(.015f,.225f,.595f,.635f);Camera.nearClipPlane=.1f;Camera.farClipPlane=200;
            var sun=new GameObject("Sun",typeof(Light)).GetComponent<Light>();sun.transform.SetParent(transform);sun.type=LightType.Directional;sun.transform.rotation=Quaternion.Euler(45,-35,0);sun.intensity=1.1f;Rebuild();
        }
        Transform Shape(string name,PrimitiveType type,Vector3 p,Vector3 scale,string color,Transform parent)
        {var o=GameObject.CreatePrimitive(type);o.name=name;o.transform.SetParent(parent,false);o.transform.localPosition=p;o.transform.localScale=scale;o.GetComponent<Renderer>().sharedMaterial=Material(color);var c=o.GetComponent<Collider>();if(c!=null)Destroy(c);return o.transform;}
        Vector3 Position(int x,int y){return new Vector3(x-(size-1)*.5f,0,y-(size-1)*.5f);}
        void Rebuild()
        {
            if(root!=null)Destroy(root.gameObject);root=new GameObject("Living Field").transform;root.SetParent(transform);plants.Clear();grounds.Clear();size=app.Farm.Data.size;drawnWalls=null;drawnReuse=-1;
            for(int y=0;y<size;y++)for(int x=0;x<size;x++){Vector3 p=Position(x,y);grounds.Add(Shape("Plot",PrimitiveType.Cube,p,new Vector3(.96f,.12f,.96f),"BCCB9D",root).GetComponent<Renderer>());plants.Add(new CropView());}
            Shape("Island Foundation",PrimitiveType.Cube,new Vector3(0,-.3f,0),new Vector3(size+.5f,.5f,size+.5f),"A4B58B",root);
            Camera.transform.position=new Vector3(0,14,-11);Camera.transform.LookAt(Vector3.zero);Camera.orthographicSize=Math.Max(2,(size+2)*.59f);refresh=0;
        }
        void Update()
        {
            if(app==null||app.Farm==null)return;if(size!=app.Farm.Data.size)Rebuild();var data=app.Farm.Data;
            while(units.Count<app.Session.drones.Count){var body=new GameObject("Drone").transform;body.SetParent(transform);Shape("Body",PrimitiveType.Cube,Vector3.zero,new Vector3(.4f,.25f,.4f),"E5E9CF",body);Shape("Visor",PrimitiveType.Cube,new Vector3(0,.05f,-.21f),new Vector3(.27f,.08f,.035f),"294C43",body);for(int i=0;i<4;i++)Shape("Rotor",PrimitiveType.Cylinder,new Vector3(i%2==0?-.28f:.28f,.14f,i<2?-.28f:.28f),new Vector3(.2f,.015f,.2f),"36554C",body);hats.Add(Shape("Hat",PrimitiveType.Cylinder,new Vector3(0,.24f,0),new Vector3(.3f,.06f,.3f),"D9B66C",body).GetComponent<Renderer>());units.Add(body);}
            for(int i=0;i<units.Count;i++){bool exists=i<app.Session.drones.Count&&(!app.Session.drones[i].finished||app.Session.drones[i].id==0);units[i].gameObject.SetActive(exists);if(exists){var u=app.Session.drones[i];units[i].position=Vector3.Lerp(units[i].position,Position(u.x,u.y)+Vector3.up*(.9f+Mathf.Sin(Time.unscaledTime*3+i)*.025f),1-Mathf.Exp(-Time.unscaledDeltaTime*20));block.SetColor("_Color",u.hat=="dinosaur"?ColorOf("65B4BE"):u.hat=="straw"?ColorOf("D9B66C"):Color.HSVToRGB((Math.Abs(u.hat.GetHashCode()%1000)/1000f),.55f,.85f));hats[i].SetPropertyBlock(block);}}
            refresh-=Time.unscaledDeltaTime;if(refresh>0)return;refresh=.16f;var tail=new HashSet<int>(app.Farm.tail);var merged=MergedPumpkins();var covered=new HashSet<int>();foreach(var m in merged)for(int oy=0;oy<m.Value;oy++)for(int ox=0;ox<m.Value;ox++)if(ox!=0||oy!=0)covered.Add(m.Key+oy*size+ox);
            for(int i=0;i<plants.Count;i++)
            {
                var c=data.cells[i];int side;merged.TryGetValue(i,out side);if(side<1)side=1;string entity=tail.Contains(i)?"tail":c.entity;if(covered.Contains(i))entity="";int stage=app.Farm.Ready(c)?3:Math.Min(2,(int)(c.growth/ClassicFarm.GrowTime(c.entity)*3));bool ordered=entity!="cactus"||Ordered(i);
                string key=entity+":"+stage+":"+c.measure+":"+side+":"+ordered;
                var view=plants[i];if(view.key!=key){if(view.root!=null)Destroy(view.root.gameObject);view.key=key;view.root=new GameObject("Crop "+i).transform;view.root.SetParent(root,false);view.root.localPosition=Position(i%size,i/size)+new Vector3((side-1)*.5f,.07f,(side-1)*.5f);BuildCrop(view.root,entity,stage,c.measure,side,ordered);view.parts=view.root.GetComponentsInChildren<Renderer>();view.infected=false;}
                if(view.infected!=c.infected){foreach(var part in view.parts){if(c.infected){block.SetColor("_Color",ColorOf("BD8BB5"));part.SetPropertyBlock(block);}else part.SetPropertyBlock(null);}view.infected=c.infected;}
                block.SetColor("_Color",Color.Lerp(ColorOf(c.ground=="soil"?"99795A":"BCCB9D"),ColorOf("665847"),(float)c.water*.6f));grounds[i].SetPropertyBlock(block);
            }
            bool redraw=app.Farm.maze&&(!ReferenceEquals(drawnWalls,app.Farm.walls)||drawnReuse!=app.Farm.mazeReuses)||!app.Farm.maze&&wallsRoot!=null;
            if(redraw){if(wallsRoot!=null)Destroy(wallsRoot.gameObject);wallsRoot=null;drawnWalls=app.Farm.walls;drawnReuse=app.Farm.mazeReuses;if(app.Farm.maze&&drawnWalls!=null){wallsRoot=new GameObject("Maze Walls").transform;wallsRoot.SetParent(root,false);for(int i=0;i<drawnWalls.Length;i++)for(int d=0;d<4;d++)if((drawnWalls[i]&(1<<d))!=0&&(d<2||d==2&&i/size==0||d==3&&i%size==0)){Vector3 p=Position(i%size,i/size)+(d==0?Vector3.forward:d==1?Vector3.right:d==2?Vector3.back:Vector3.left)*.5f;Shape("Hedge",PrimitiveType.Cube,p+Vector3.up*.4f,d%2==0?new Vector3(1,.8f,.15f):new Vector3(.15f,.8f,1),"345B45",wallsRoot);}}}
        }
        bool Ordered(int i){var c=app.Farm.Data.cells[i];if(!app.Farm.Ready(c))return false;int x=i%size,y=i/size;int[] neighbors={x>0?i-1:-1,y>0?i-size:-1,x+1<size?i+1:-1,y+1<size?i+size:-1};for(int n=0;n<4;n++){if(neighbors[n]<0)continue;var other=app.Farm.Data.cells[neighbors[n]];if(other.entity!="cactus"||!app.Farm.Ready(other))return false;if(n<2?other.measure>c.measure:other.measure<c.measure)return false;}return true;}
        Dictionary<int,int> MergedPumpkins()
        {
            int[,] dp=new int[size+1,size+1];var candidates=new List<int[]>();for(int y=0;y<size;y++)for(int x=0;x<size;x++){var c=app.Farm.Cell(x,y);if(c.entity=="pumpkin"&&app.Farm.Ready(c)){int side=dp[y+1,x+1]=1+Math.Min(dp[y,x],Math.Min(dp[y+1,x],dp[y,x+1]));if(side>=2)candidates.Add(new[]{side,x-side+1,y-side+1});}}
            var result=new Dictionary<int,int>();var taken=new HashSet<int>();foreach(var v in candidates.OrderByDescending(v=>v[0])){bool free=true;for(int y=0;y<v[0]&&free;y++)for(int x=0;x<v[0];x++)if(taken.Contains((v[2]+y)*size+v[1]+x)){free=false;break;}if(!free)continue;int origin=v[2]*size+v[1];result[origin]=v[0];for(int y=0;y<v[0];y++)for(int x=0;x<v[0];x++)taken.Add(origin+y*size+x);}return result;
        }
        void BuildCrop(Transform t,string entity,int stage,int measure,int side,bool ordered)
        {
            if(entity=="")return;if(entity=="tail"){Shape("Tail",PrimitiveType.Cube,new Vector3(0,.18f,0),new Vector3(.8f,.32f,.8f),"5AADC0",t);return;}if(entity=="hedge")return;
            if(entity=="treasure"){Shape("Chest",PrimitiveType.Cube,new Vector3(0,.2f,0),new Vector3(.7f,.4f,.5f),"D9AD54",t);Shape("Lock",PrimitiveType.Cube,new Vector3(0,.25f,-.26f),new Vector3(.15f,.16f,.04f),"FFF0A5",t);return;}
            if(entity=="apple"){Shape("Fruit",PrimitiveType.Sphere,new Vector3(0,.2f,0),Vector3.one*.4f,"D96B60",t);Shape("Leaf",PrimitiveType.Cube,new Vector3(.1f,.44f,0),new Vector3(.2f,.05f,.12f),"668D5C",t);return;}
            if(entity=="dead_pumpkin"){Shape("Wilted",PrimitiveType.Sphere,new Vector3(0,.12f,0),new Vector3(.55f,.2f,.5f),"776C61",t);return;}
            float scale=.4f+stage*.2f;t.localScale=Vector3.one*scale;if(stage==0){Shape("Seed",PrimitiveType.Sphere,new Vector3(0,.05f,0),new Vector3(.16f,.08f,.16f),"DFC591",t);return;}
            if(entity=="grass"){for(int i=0;i<4;i++){float x=(i%2-.5f)*.3f,z=(i/2-.5f)*.3f;Shape("Stem",PrimitiveType.Cube,new Vector3(x,.22f,z),new Vector3(.05f,.44f,.05f),"719B60",t);var ear=Shape("Ear",PrimitiveType.Cube,new Vector3(x,.48f,z),new Vector3(.12f,.24f,.1f),stage==3?"C7BD6C":"A0BB70",t);ear.localRotation=Quaternion.Euler(8,20,12);}return;}
            if(entity=="tree"||entity=="bush"){float h=entity=="tree"?.75f:.25f;Shape("Trunk",PrimitiveType.Cylinder,new Vector3(0,h*.5f,0),new Vector3(.13f,h*.5f,.13f),"80664A",t);Shape("Foliage",PrimitiveType.Sphere,new Vector3(0,h+.2f,0),new Vector3(.75f,entity=="tree"?.9f:.5f,.75f),entity=="tree"?"487A58":"73A16B",t);return;}
            if(entity=="carrot"){for(int i=-1;i<=1;i++){Shape("Root",PrimitiveType.Sphere,new Vector3(i*.2f,.11f,0),new Vector3(.2f,.3f,.2f),"E39458",t);var leaf=Shape("Leaf",PrimitiveType.Cube,new Vector3(i*.2f,.35f,0),new Vector3(.09f,.28f,.06f),"75A36F",t);leaf.localRotation=Quaternion.Euler(0,0,i*20);}return;}
            if(entity=="pumpkin"){t.localScale=new Vector3(Math.Max(1,side*.88f),Mathf.Sqrt(side),Math.Max(1,side*.88f));for(int i=-1;i<=1;i++)Shape("Rib",PrimitiveType.Sphere,new Vector3(i*.13f,.26f,0),new Vector3(.28f,.44f,.55f),stage==3?"E3A24F":"9BB17C",t);Shape("Stalk",PrimitiveType.Cylinder,new Vector3(0,.55f,0),new Vector3(.06f,.1f,.06f),"617A4C",t);return;}
            if(entity=="sunflower"){Shape("Stem",PrimitiveType.Cylinder,new Vector3(0,.28f,0),new Vector3(.05f,.28f,.05f),"5D8F64",t);int petals=Math.Max(7,measure);for(int i=0;i<petals;i++){float angle=i*Mathf.PI*2/petals;Shape("Petal",PrimitiveType.Sphere,new Vector3(Mathf.Cos(angle)*.2f,.6f,Mathf.Sin(angle)*.2f),new Vector3(.16f,.08f,.16f),"F1CB60",t);}Shape("Center",PrimitiveType.Sphere,new Vector3(0,.63f,0),new Vector3(.24f,.1f,.24f),"806744",t);return;}
            if(entity=="cactus"){string color=ordered?"6AA580":"A18E63";float h=.3f+.06f*measure;Shape("Cactus",PrimitiveType.Cube,new Vector3(0,h*.5f,0),new Vector3(.28f,h,.3f),color,t);Shape("Arm",PrimitiveType.Cube,new Vector3(.2f,h*.55f,0),new Vector3(.35f,.13f,.16f),color,t);Shape("Arm Tip",PrimitiveType.Cube,new Vector3(.34f,h*.7f,0),new Vector3(.12f,.3f,.16f),color,t);}
        }
        public static Color ColorOf(string hex){Color c;ColorUtility.TryParseHtmlString("#"+hex,out c);return c;}
        void OnDestroy(){foreach(var m in materials.Values)if(m!=null)Destroy(m);}
    }
}
