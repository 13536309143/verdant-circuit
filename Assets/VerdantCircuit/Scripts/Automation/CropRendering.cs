using System;
using System.Collections.Generic;
using UnityEngine;
namespace Verdant.Automation
{
    public sealed partial class ClassicView
    {
        sealed class CropBatch {public Mesh mesh;public Material material;public int count;public readonly Matrix4x4[] matrices=new Matrix4x4[1023];}
        readonly Dictionary<string,List<CropBatch>> cropGroups=new Dictionary<string,List<CropBatch>>();
        readonly Dictionary<string,Mesh> cropMeshes=new Dictionary<string,Mesh>();readonly Plane[] cropPlanes=new Plane[6];
        int renderedCrops;
        public static float GrowthFraction(ClassicCell cell)
        {double total=ClassicFarm.GrowTime(cell.entity);return double.IsInfinity(total)?0:Mathf.Clamp01((float)(cell.growth/total));}
        public static float CropScale(ClassicCell cell){return .15f+.85f*Mathf.Sqrt(GrowthFraction(cell));}
        void AddCrop(string key,Vector3 point,Quaternion rotation,Vector3 scale)
        {
            if(renderedCrops>=8000)return;List<CropBatch> group;
            if(!cropGroups.TryGetValue(key,out group)){group=new List<CropBatch>();cropGroups[key]=group;}
            CropBatch batch=null;foreach(var candidate in group)if(candidate.count<1023){batch=candidate;break;}
            if(batch==null){Mesh mesh;if(!cropMeshes.TryGetValue(key,out mesh)){mesh=CropGeometry(key);cropMeshes[key]=mesh;ownedMeshes.Add(mesh);}string hex=key=="grass"?"BBD782":key=="tree"||key=="bush"?"69BB78":key=="carrot"?"F2AC60":key=="pumpkin"?"EAA044":key=="sunflower"||key=="treasure"?"F6D365":key=="infected"?"C58ECC":key=="cactus"?"75C79A":"7B8267";batch=new CropBatch{mesh=mesh,material=MakeMaterial(ColorOf(hex),"Verdant/PlanetSurface")};group.Add(batch);}
            batch.matrices[batch.count++]=Matrix4x4.TRS(point,rotation,scale);renderedCrops++;
        }
        void BuildCropInstances()
        {
            foreach(var group in cropGroups.Values)foreach(var batch in group)batch.count=0;renderedCrops=0;
            GeometryUtility.CalculateFrustumPlanes(Camera,cropPlanes);Vector3 camera=Camera.transform.position;
            float apparent=Radius/Mathf.Max(.1f,camera.magnitude-Radius);int stride=FocusPlot?1:apparent<.7f?3:apparent<1.8f?2:1;
            // Reject sixteen-cell chunks before visiting individual crops. There are no per-plot GameObjects.
            for(int cy=0;cy<drawnHeight;cy+=16)for(int cx=0;cx<drawnWidth;cx+=16)
            {
                int cw=Math.Min(16,drawnWidth-cx),ch=Math.Min(16,drawnHeight-cy);Vector3 center=SurfacePoint(drawnWidth,drawnHeight,cx+cw*.5f-.5f,cy+ch*.5f-.5f);
                if(Vector3.Dot(center.normalized,(camera-center).normalized)<-.3f||!GeometryUtility.TestPlanesAABB(cropPlanes,new Bounds(center,Vector3.one*(Mathf.Max(cw,ch)*.6f+2))))continue;
                for(int y=cy;y<cy+ch;y+=stride)for(int x=cx;x<cx+cw;x+=stride)
                {
                    var cell=app.Farm.Cell(x,y);if(cell.entity=="")continue;var point=SurfacePoint(drawnWidth,drawnHeight,x,y,Radius+.065f);
                    if(Vector3.Dot(point.normalized,(camera-point).normalized)<-.05f||FocusPlot&&(camera-point).sqrMagnitude>100)continue;
                    var rotation=Quaternion.FromToRotation(Vector3.up,point.normalized);
                    if(app.Farm.maze){int walls=app.Farm.walls[y*drawnWidth+x];if((walls&1)!=0)AddCrop("hedge",SurfacePoint(drawnWidth,drawnHeight,x,y+.5f,Radius+.12f),rotation,new Vector3(.31f,.18f,.025f));if((walls&2)!=0)AddCrop("hedge",SurfacePoint(drawnWidth,drawnHeight,x+.5f,y,Radius+.12f),rotation,new Vector3(.025f,.18f,.31f));if(cell.entity!="treasure")continue;}
                    float growth=CropScale(cell);float height=cell.entity=="tree"?.55f:cell.entity=="cactus"?.36f:cell.entity=="sunflower"?.40f:cell.entity=="pumpkin"?.22f:.20f;
                    AddCrop(cell.infected?"infected":cell.entity,point,rotation,new Vector3(.23f*(.45f+.55f*growth),height*growth,.23f*(.45f+.55f*growth)));
                }
            }
            VisibleCropInstances=renderedCrops;
        }
        void DrawCrops()
        {
            CropDrawCalls=0;foreach(var group in cropGroups.Values)foreach(var batch in group)if(batch.count>0){cropCommands.DrawMeshInstanced(batch.mesh,0,batch.material,0,batch.matrices,batch.count);CropDrawCalls++;}
        }
        Mesh CropGeometry(string key)
        {
            var v=new List<Vector3>();var t=new List<int>();var c=new List<Color>();
            Action<Vector3,Vector3,Color> diamond=(center,size,color)=>
            {int n=v.Count;v.Add(center+Vector3.up*size.y);v.Add(center-Vector3.up*size.y);v.Add(center+Vector3.right*size.x);v.Add(center+Vector3.forward*size.z);v.Add(center-Vector3.right*size.x);v.Add(center-Vector3.forward*size.z);for(int i=0;i<6;i++)c.Add(color);for(int i=0;i<4;i++){int a=n+2+i,b=n+2+(i+1)%4;t.Add(n);t.Add(b);t.Add(a);t.Add(n+1);t.Add(a);t.Add(b);}};
            if(key=="grass"||key=="infected"){for(int i=0;i<3;i++)diamond(new Vector3((i-1)*.23f,.45f,.12f*(i%2)),new Vector3(.16f,.45f,.12f),Color.white);}
            else if(key=="tree"||key=="bush"){diamond(new Vector3(0,.2f,0),new Vector3(.15f,.2f,.15f),new Color(.7f,.55f,.3f));diamond(new Vector3(0,.65f,0),new Vector3(.58f,.35f,.58f),Color.white);diamond(new Vector3(.07f,.9f,0),new Vector3(.38f,.22f,.38f),new Color(.9f,1,.8f));}
            else if(key=="sunflower"){diamond(new Vector3(0,.4f,0),new Vector3(.05f,.4f,.05f),new Color(.45f,.8f,.35f));for(int i=0;i<6;i++){float a=i*Mathf.PI/3;diamond(new Vector3(Mathf.Cos(a)*.32f,.85f,Mathf.Sin(a)*.32f),new Vector3(.25f,.08f,.25f),Color.white);}diamond(new Vector3(0,.9f,0),new Vector3(.22f,.1f,.22f),new Color(.55f,.34f,.14f));}
            else if(key=="cactus"){diamond(new Vector3(0,.5f,0),new Vector3(.22f,.5f,.22f),Color.white);diamond(new Vector3(-.4f,.55f,0),new Vector3(.25f,.28f,.18f),Color.white);diamond(new Vector3(.35f,.7f,0),new Vector3(.20f,.25f,.18f),Color.white);}
            else diamond(new Vector3(0,.4f,0),new Vector3(.5f,.4f,.5f),Color.white);
            var mesh=new Mesh{name="Instanced "+key};mesh.SetVertices(v);mesh.SetColors(c);mesh.SetTriangles(t,0);mesh.RecalculateNormals();mesh.RecalculateBounds();return mesh;
        }
        struct HarvestEffect {public float age;public int planet;public Vector3 position;public bool active;}
        readonly HarvestEffect[] harvestEffects=new HarvestEffect[48];readonly Matrix4x4[] effectMatrices=new Matrix4x4[240];int nextEffect;Mesh effectMesh;Material effectMaterial;
        public int HarvestEffectsStarted {get;private set;}
        void InitializeHarvestEffects(){effectMesh=CropGeometry("spark");ownedMeshes.Add(effectMesh);effectMaterial=MakeMaterial(ColorOf("FFDE88"),"Verdant/PlanetSurface");}
        void OnHarvest(int x,int y,string item,double amount){OnProduction(0,x,y,item,amount);}
        void OnProduction(int planet,int x,int y,string item,double amount)
        {
            if(ShowUniverse||planet!=SelectedPlanet)return;var pos=SurfacePoint(app.Farm.Fleet.SurfaceWidth(planet),app.Farm.Fleet.SurfaceHeight(planet),x,y,Radius+.10f);
            harvestEffects[nextEffect]=new HarvestEffect{active=true,planet=planet,position=pos};nextEffect=(nextEffect+1)%harvestEffects.Length;HarvestEffectsStarted++;cropRefresh=0;
        }
        void DrawHarvestEffects(float dt)
        {
            int count=0;EffectCount=0;
            for(int i=0;i<harvestEffects.Length;i++)
            {
                var e=harvestEffects[i];if(!e.active)continue;e.age+=dt;if(e.age>=.85f){e.active=false;harvestEffects[i]=e;continue;}harvestEffects[i]=e;
                if(ShowUniverse||e.planet!=SelectedPlanet)continue;EffectCount++;var normal=e.position.normalized;var orientation=Quaternion.FromToRotation(Vector3.up,normal);float fade=1-e.age/.85f;
                for(int spark=0;spark<5;spark++){float angle=spark*Mathf.PI*.4f;var velocity=orientation*new Vector3(Mathf.Cos(angle)*.22f,.6f,Mathf.Sin(angle)*.22f);effectMatrices[count++]=Matrix4x4.TRS(e.position+velocity*e.age,orientation,Vector3.one*(.055f*fade));}
            }
            if(count>0)cropCommands.DrawMeshInstanced(effectMesh,0,effectMaterial,0,effectMatrices,count);
        }
    }
}
