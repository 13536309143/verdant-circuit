using System;
using UnityEngine;
namespace Verdant.Automation
{
    public sealed partial class ClassicView
    {
        readonly Vector3[] spinAxes=new Vector3[4];readonly float[] spinRates=new float[4];readonly Transform[] atmospheres=new Transform[4];
        Transform mothership,lander;float landingTime;int landingPlanet;Action landed;
        public bool Landing {get;private set;}public int LandingCompletions {get;private set;}
        public bool PlanetIsGrey(int p){return spheres[p].GetComponent<Renderer>().sharedMaterial.GetFloat("_Locked")>.5f;}
        public Quaternion PlanetRotation(int p){return spheres[p].rotation;}
        public void BeginLanding(int planet,Action complete)
        {
            if(Landing||!app.Farm.Fleet.IsUnlocked(planet))return;Landing=true;landingPlanet=planet;landingTime=0;landed=complete;lander.gameObject.SetActive(true);
        }
        public void CancelLanding(){Landing=false;landed=null;if(lander!=null)lander.gameObject.SetActive(false);}
        void BuildSpaceArt()
        {
            var random=new System.Random(Guid.NewGuid().GetHashCode());
            for(int p=0;p<4;p++)
            {
                spinAxes[p]=new Vector3((float)random.NextDouble()*.7f-.35f,1,(float)random.NextDouble()*.7f-.35f).normalized;spinRates[p]=(3+(float)random.NextDouble()*5)*(p%2==0?1:-1);
                var halo=new GameObject("Atmosphere "+p,typeof(MeshFilter),typeof(MeshRenderer));halo.transform.SetParent(transform);halo.GetComponent<MeshFilter>().sharedMesh=spheres[p].GetComponent<MeshFilter>().sharedMesh;
                halo.GetComponent<MeshRenderer>().sharedMaterial=MakeMaterial(ColorOf(p==0?"62D9C0":p==1?"F492BA":p==2?"64B5FF":"D5E3EE"),"Verdant/Atmosphere");halo.transform.localScale=Vector3.one*1.025f;atmospheres[p]=halo.transform;
            }
            mothership=new GameObject("Orbital Mothership").transform;mothership.SetParent(transform);mothership.position=MothershipPosition;
            Primitive("Command core",PrimitiveType.Capsule,mothership,Vector3.zero,new Vector3(5,3,8),ColorOf("C5D7E3"));
            Primitive("Bridge",PrimitiveType.Sphere,mothership,new Vector3(0,2,-2),new Vector3(3,2,4),ColorOf("65C8E3"));
            for(int i=0;i<2;i++){float sign=i==0?-1:1;Primitive("Solar wing",PrimitiveType.Cube,mothership,new Vector3(sign*8,0,0),new Vector3(10,.35f,6),ColorOf("356CA0"));Primitive("Docking light",PrimitiveType.Cylinder,mothership,new Vector3(sign*3,-1,0),new Vector3(2,.25f,2),ColorOf("74E5D4"));}
            lander=new GameObject("Landing shuttle").transform;lander.SetParent(transform);Primitive("Shuttle hull",PrimitiveType.Capsule,lander,Vector3.zero,new Vector3(2,.7f,2),ColorOf("DDEBE6"));Primitive("Landing canopy",PrimitiveType.Sphere,lander,new Vector3(0,.5f,-.3f),Vector3.one,ColorOf("65DAD7"));lander.gameObject.SetActive(false);
            var stars=new GameObject("Batched Starfield",typeof(MeshFilter),typeof(MeshRenderer));stars.transform.SetParent(transform);
            var vertices=new System.Collections.Generic.List<Vector3>();var tris=new System.Collections.Generic.List<int>();var colors=new System.Collections.Generic.List<Color>();
            for(int i=0;i<240;i++){float x=(float)random.NextDouble()*260-130,y=(float)random.NextDouble()*140-70,z=80+(float)random.NextDouble()*60,size=i%9==0?.20f:.08f;int n=vertices.Count;vertices.Add(new Vector3(x-size,y-size,z));vertices.Add(new Vector3(x-size,y+size,z));vertices.Add(new Vector3(x+size,y+size,z));vertices.Add(new Vector3(x+size,y-size,z));for(int c=0;c<4;c++)colors.Add(ColorOf(i%4==0?"A1C6F4":"BBCBCC"));tris.AddRange(new[]{n,n+1,n+2,n,n+2,n+3});}
            var mesh=new Mesh{name="One Draw Starfield"};mesh.SetVertices(vertices);mesh.SetColors(colors);mesh.SetTriangles(tris,0);mesh.RecalculateNormals();mesh.RecalculateBounds();ownedMeshes.Add(mesh);stars.GetComponent<MeshFilter>().sharedMesh=mesh;stars.GetComponent<MeshRenderer>().sharedMaterial=MakeMaterial(Color.white,"Verdant/PlanetSurface");
        }
        static Vector3 DepotPosition(int p){return p==-1?MothershipPosition:Orbits[p]+Vector3.up*20;}
        void UpdateSpaceArt(float dt)
        {
            mothership.gameObject.SetActive(ShowUniverse);mothership.localRotation=Quaternion.Euler(0,Mathf.Sin(Time.unscaledTime*.12f)*8,8);mothership.position=MothershipPosition+Vector3.up*Mathf.Sin(Time.unscaledTime*.6f)*.35f;
            for(int p=0;p<4;p++){atmospheres[p].gameObject.SetActive((ShowUniverse||p==SelectedPlanet)&&app.Farm.Fleet.IsUnlocked(p));atmospheres[p].position=ShowUniverse?Orbits[p]:Vector3.zero;}
            foreach(var vessel in app.Farm.Fleet.Data.ufos)
            {
                Transform model;if(!ufos.TryGetValue(vessel.id,out model)){model=new GameObject("UFO #"+vessel.id).transform;model.SetParent(transform);Primitive("Saucer",PrimitiveType.Sphere,model,Vector3.zero,new Vector3(2.4f,.45f,2.4f),ColorOf("AFC7DC"));Primitive("Cockpit",PrimitiveType.Sphere,model,Vector3.up*.4f,Vector3.one*.9f,ColorOf("68D8D8"));ufos[vessel.id]=model;}
                model.gameObject.SetActive(ShowUniverse);float t=vessel.phase==0?0:Mathf.Clamp01(1-(float)(vessel.timer/vessel.legDuration));
                Vector3 a=DepotPosition(vessel.planet),b=DepotPosition(vessel.phase==0?vessel.planet:vessel.legTarget);model.position=Vector3.Lerp(a,b,t)+Vector3.up*(3+Mathf.Sin(t*Mathf.PI)*9);model.Rotate(Vector3.up,dt*60);
            }
        }
        void UpdateLanding(float dt)
        {
            if(!Landing)return;landingTime+=dt;float t=Mathf.Clamp01(landingTime/2.8f),ease=t*t*(3-2*t);var target=Orbits[landingPlanet]+Vector3.back*(Radius+.8f);var position=Vector3.Lerp(MothershipPosition,target,ease)+Vector3.up*Mathf.Sin(t*Mathf.PI)*10;
            lander.position=position;lander.rotation=Quaternion.LookRotation((target-position+Vector3.forward*.01f).normalized);Camera.transform.position=Vector3.Lerp(new Vector3(4,14,-155),target+new Vector3(0,3,-9),ease*.8f);Camera.transform.LookAt(Vector3.Lerp(new Vector3(4,0,8),position,ease));
            if(t>=1){Landing=false;lander.gameObject.SetActive(false);LandingCompletions++;var callback=landed;landed=null;callback?.Invoke();}
        }
    }
}
