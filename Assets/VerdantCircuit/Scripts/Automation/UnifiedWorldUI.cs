using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Verdant.Automation
{
    public sealed partial class ClassicController
    {
        string programTarget="earth";int programIsland;TMP_Text studioTarget;
        public bool WorldSelectionActive {get{return modalKind=="world";}}
        int activePlanet;bool hasEnteredPlanet;readonly System.Collections.Generic.List<GameObject> universeHidden=new System.Collections.Generic.List<GameObject>();
        void RestoreUniverseUi(){foreach(var go in universeHidden)if(go!=null)go.SetActive(true);universeHidden.Clear();if(View!=null){View.CancelLanding();View.ShowUniverse=false;}}
        public void EnterPlanet(int p)
        {
            if(!Farm.Fleet.IsUnlocked(p)){researchScope=-1;selectedResearch=ArchipelagoWorld.IslandResearch[p];Open("research");return;}
            StoreEditor();Close();foreach(var window in codeWindows.Values)if(window!=null)Destroy(window.transform.parent.gameObject);codeWindows.Clear();
            if(canvas!=null){canvas.gameObject.SetActive(false);Destroy(canvas.gameObject);}activePlanet=p;Farm.Data.selectedPlanet=p;hasEnteredPlanet=true;programIsland=p;int id=Farm.Fleet.EnsurePlanetWorker(p);programTarget="drone:"+id;var worker=Farm.Fleet.Unit(id);string file=worker!=null&&!string.IsNullOrEmpty(worker.program)?worker.program:p==0?"main":ArchipelagoWorld.Definitions[p].id+"_main";int index=Farm.Data.programs.FindIndex(v=>v.name==file);if(index<0){string code=p==1?"cultivate_rose(worker_self(), 1)":p==2?"catch_carp(worker_self(), 1)":p==3?"mine_stone(worker_self(), 1)":"harvest()";Farm.Data.programs.Add(new ClassicProgram(file,code));index=Farm.Data.programs.Count-1;}Farm.Data.selectedProgram=index;BuildUI();if(View!=null)View.SelectSurface(p);Save();
        }
        void RebuildLocaleUi()
        {StoreEditor();string kind=modalKind;Close();foreach(var field in codeWindows.Values)if(field!=null)Destroy(field.transform.parent.gameObject);codeWindows.Clear();if(canvas!=null){canvas.gameObject.SetActive(false);Destroy(canvas.gameObject);}Font=L10n.UIFont;BuildUI();if(kind!=null)Open(kind);}
        void BuildWorldSelection()
        {
            foreach(Transform child in canvas)if(child.name!="Top Navigation"){universeHidden.Add(child.gameObject);child.gameObject.SetActive(false);}if(View!=null)View.ShowUniverse=true;modal=Panel("Universe Navigation",canvas,new Vector2(.01f,.03f),new Vector2(.99f,.91f),Color.clear);
            Label(Tr("A SMALL UNIVERSE","一个小小的宇宙"),modal,new Vector2(.025f,.84f),new Vector2(.80f,.98f),46,Color.white);
            Label(Tr("Your orbital mothership connects four worlds. Choose Earth to descend.","轨道母舰连接四颗星球。选择地球，乘穿梭机降落。"),modal,new Vector2(.028f,.77f),new Vector2(.85f,.86f),20,ClassicView.ColorOf("A7BDCE"));
            for(int i=0;i<4;i++){int planet=i;float x=.025f+i*.245f;var card=Surface("Planet Choice "+i,modal,new Vector2(x,.04f),new Vector2(x+.23f,.30f),ClassicView.ColorOf("152536"),true);Label(IslandName(i),card,new Vector2(.07f,.65f),new Vector2(.93f,.95f),24,Color.white);Label(Farm.Fleet.IsUnlocked(i)?Tr("READY / enter colony","已开放 / 进入殖民地"):Tr("LOCKED / research required","尚未开放 / 需要科技"),card,new Vector2(.07f,.39f),new Vector2(.93f,.63f),14,Muted);Skin(Button(Farm.Fleet.IsUnlocked(i)?Tr("Enter planet","进入星球"):Tr("View research","查看科技"),card,new Vector2(.07f,.07f),new Vector2(.93f,.32f),()=>{if(Farm.Fleet.IsUnlocked(planet))RequestPlanetLanding(planet);else{researchScope=-1;selectedResearch=ArchipelagoWorld.IslandResearch[planet];Open("research");}}),ClassicView.ColorOf(!Farm.Fleet.IsUnlocked(i)?"414A55":i==0?"258E83":i==1?"C86C87":i==2?"368EC5":"7B8DA2"),Color.white);}
            Button("English / 中文",modal,new Vector2(.77f,.90f),new Vector2(.97f,.97f),()=>L10n.Set(L10n.Code=="en"?"zh-Hans":"en"));
        }
        public void EnterEarth(){EnterPlanet(0);}
        public void RequestPlanetLanding(int p){if(View==null||!Farm.Fleet.IsUnlocked(p))return;View.BeginLanding(p,()=>EnterPlanet(p));}
        int TargetDrone()
        {if(programTarget=="earth")return Farm.Fleet.EnsureEarthDrone();int id;return programTarget.StartsWith("drone:")&&int.TryParse(programTarget.Substring(6),out id)?id:-1;}
        string TargetName()
        {if(programTarget=="earth")return Tr("Earth / drone","地球／无人机");if(programTarget=="network")return Tr("Universe controller","宇宙总控");if(programTarget.StartsWith("island:"))return IslandName(int.Parse(programTarget.Substring(7)))+Tr(" / manager","／管理程序");if(programTarget.StartsWith("drone:")){int id;if(int.TryParse(programTarget.Substring(6),out id)){var u=Farm.Fleet.Unit(id);if(u!=null)return FleetRoleName(u.role)+" #"+u.id;}}return programTarget;}
        void BuildStudioTarget(Transform studio)
        {
            var target=Button("",studio,new Vector2(.04f,.823f),new Vector2(.96f,.87f),()=>Open("programs"));studioTarget=target.GetComponentInChildren<TMP_Text>();target.GetComponent<UnityEngine.UI.Image>().color=ClassicView.ColorOf("36564A");
        }
        bool RunTarget(ClassicProgram program,bool run=true)
        {
            var world=Farm.Fleet;world.Breakpoints=Session.breakpoints;bool ok;
            if(programTarget=="network"||programTarget.StartsWith("island:"))ok=run?world.RunControl(programTarget,program.name):world.BindControl(programTarget,program.name);
            else if(programTarget.StartsWith("group:"))
            {var group=world.Data.groups.FirstOrDefault(g=>g.name==programTarget.Substring(6));ok=group!=null&&group.members.Count>0;if(group!=null)foreach(int id in group.members)ok=(run?world.RunProgram(id,program.name):world.BindProgram(id,program.name))&&ok;}
            else{int id=TargetDrone();ok=id>=0&&(run?world.RunProgram(id,program.name):world.BindProgram(id,program.name));}
            Log(ok?TargetName()+" → "+program.name+(run?Tr(" / started","／已运行"):Tr(" / bound","／已绑定")):Tr("Cannot bind: unlock the target, check drone capacity and choose an existing program.","无法绑定：请先解锁目标，检查无人机名额并选择现有程序。"));Save();return ok;
        }
        void StopTarget()
        {
            var world=Farm.Fleet;if(programTarget=="network"||programTarget.StartsWith("island:"))world.StopControl(programTarget);
            else if(programTarget.StartsWith("group:")){var group=world.Data.groups.FirstOrDefault(g=>g.name==programTarget.Substring(6));if(group!=null)foreach(int id in group.members)world.StopProgram(id);}
            else world.StopProgram(TargetDrone());Save();Open("programs");
        }
        RectTransform ProgramPane(Transform parent,string name,float min,float max)
        {
            var panel=Panel(name,parent,new Vector2(min,0),new Vector2(max,1),Paper);panel.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;var scroll=panel.gameObject.AddComponent<UnityEngine.UI.ScrollRect>();
            var viewport=Panel("Viewport",panel,Vector2.zero,new Vector2(.975f,1),Paper);viewport.gameObject.AddComponent<UnityEngine.UI.RectMask2D>();viewport.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;
            var content=Panel("Content",viewport,new Vector2(0,1),new Vector2(1,1),Color.clear);content.pivot=new Vector2(.5f,1);var layout=content.gameObject.AddComponent<UnityEngine.UI.VerticalLayoutGroup>();layout.spacing=10;layout.childControlWidth=true;layout.childControlHeight=true;layout.childForceExpandHeight=false;layout.padding=new RectOffset(4,4,4,4);content.gameObject.AddComponent<UnityEngine.UI.ContentSizeFitter>().verticalFit=UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
            scroll.viewport=viewport;scroll.content=content;scroll.horizontal=false;scroll.movementType=UnityEngine.UI.ScrollRect.MovementType.Clamped;scroll.scrollSensitivity=28;
            var track=Panel("Scrollbar",panel,new Vector2(.98f,0),Vector2.one,new Color(Accent.r,Accent.g,Accent.b,.12f));track.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;var bar=track.gameObject.AddComponent<UnityEngine.UI.Scrollbar>();var handle=Panel("Handle",track,Vector2.zero,Vector2.one,Accent);handle.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;bar.handleRect=handle;bar.targetGraphic=handle.GetComponent<UnityEngine.UI.Image>();bar.direction=UnityEngine.UI.Scrollbar.Direction.BottomToTop;scroll.verticalScrollbar=bar;scroll.verticalScrollbarVisibility=UnityEngine.UI.ScrollRect.ScrollbarVisibility.AutoHide;return content;
        }
        void BuildProgramTargets()
        {
            var world=Farm.Fleet;
            Paragraph(Tr("CONTROL HIERARCHY / Universe → planet → swarm → worker","控制层级 / 宇宙总控 → 星球管理 → 集群 → 单体"),55);
            var select=Panel("Program Target Selector",modalBody,Vector2.zero,Vector2.one,Dark);select.gameObject.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight=68;
            string[] labels={Tr("Home drone","地球无人机"),Tr("Universe","宇宙总控"),Tr("Planet manager","星球管理"),Tr("Worker","本地载具")};
            string[] targets={"earth","network","island:"+programIsland,"drone:"+(world.Data.units.FirstOrDefault(u=>u.island==programIsland)?.id??world.EnsureEarthDrone())};
            for(int i=0;i<4;i++){string scope=targets[i];Skin(Button(labels[i],select,new Vector2(i*.25f+.008f,.12f),new Vector2((i+1)*.25f-.008f,.88f),()=>{programTarget=scope;Open("programs");}),programTarget==scope?Accent:Dark,Color.white);}
            var islands=Panel("Island Target Tabs",modalBody,Vector2.zero,Vector2.one,Paper);islands.gameObject.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight=145;
            for(int i=0;i<ArchipelagoWorld.Definitions.Length;i++){int island=i;float x=(i%3)/3f,y=1-(i/3+1)/3f;var button=Button(IslandName(i),islands,new Vector2(x+.005f,y+.01f),new Vector2(x+.326f,y+.32f),()=>{programIsland=island;programTarget="island:"+island;Open("programs");});button.interactable=world.IsUnlocked(i);}
            var current=Surface("Current Program Binding",modalBody,Vector2.zero,Vector2.one,Dark);current.gameObject.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight=120;
            Label(TargetName()+"\n"+Tr("Selected file: ","所选文件：")+Farm.Data.programs[Farm.Data.selectedProgram].name+".py",current,new Vector2(.025f,.15f),new Vector2(.56f,.9f),20,Color.white);
            Button(Tr("Bind only","仅绑定"),current,new Vector2(.59f,.56f),new Vector2(.78f,.9f),()=>{StoreEditor();RunTarget(Farm.Data.programs[Farm.Data.selectedProgram],false);Open("programs");});
            Button(Tr("Bind + run","绑定并运行"),current,new Vector2(.80f,.56f),new Vector2(.975f,.9f),()=>{StoreEditor();RunTarget(Farm.Data.programs[Farm.Data.selectedProgram]);Open("programs");});
            Button(Tr("Stop target","停止此目标"),current,new Vector2(.59f,.12f),new Vector2(.975f,.46f),StopTarget);
            foreach(var binding in world.Data.controls)
            {
                var b=binding;Row(b.scope+" → "+b.program+".py",(world.ControlRunning(b.scope)?Tr("Running","运行中"):Tr("Stopped","已停止"))+(b.error==""?"":" / "+b.error),Tr("Select","选择"),()=>{programTarget=b.scope;Open("programs");});
            }
            foreach(var unit in world.Data.units)
            {
                var u=unit;Row("#"+u.id+" / "+IslandName(u.island)+" / "+FleetRoleName(u.role),u.program+".py / "+(world.ProgramRunning(u.id)?Tr("Running","运行中"):Tr("Stopped","已停止"))+(u.codeError==""?"":" / "+u.codeError),Tr("Select drone","选择此机"),()=>{programTarget="drone:"+u.id;Open("programs");});
            }
            foreach(var group in world.Data.groups){var g=group;Row(Tr("Group ","机群 ")+g.name,string.Join(", ",g.members.Select(id=>"#"+id)),Tr("Select group","选择机群"),()=>{programTarget="group:"+g.name;Open("programs");});}
            if(Farm.Level("spaceflight")>0)
            {
                var row=Row(Tr("Create / update drone group","创建／更新无人机群"),Tr("Name and comma-separated drone IDs, for example 1,2,3.","输入名称与用逗号分隔的无人机编号，例如1,2,3。"),Tr("Save group","保存机群"),()=>{});row.GetComponent<UnityEngine.UI.LayoutElement>().preferredHeight=160;
                var name=MakeInput("Group Name",row,new Vector2(.02f,.05f),new Vector2(.32f,.28f),false);name.SetTextWithoutNotify("group_1");var ids=MakeInput("Group Drone IDs",row,new Vector2(.35f,.05f),new Vector2(.76f,.28f),false);
                var button=row.GetComponentInChildren<UnityEngine.UI.Button>();button.onClick.RemoveAllListeners();button.onClick.AddListener(()=>{var members=new System.Collections.Generic.List<int>();foreach(string value in ids.text.Split(',')){int id;if(!int.TryParse(value.Trim(),out id)){Log(Tr("Use comma-separated numeric drone IDs.","请填写逗号分隔的无人机数字编号。"));return;}members.Add(id);}if(world.CreateGroup(name.text.Trim(),members)){Save();Open("programs");}else Log(Tr("Invalid group: check IDs, duplicates and name.","机群无效，请检查编号、重复项与名称。"));});
            }
            Row(Tr("Create the unified example","创建统一控制范例"),Tr("Network, island managers, Earth worker, specialist workers and transport examples.","包含联合总控、各岛管理、地球农务、专用机与运输子程序。"),Tr("Create files","创建文件"),()=>{StoreEditor();foreach(string file in UnifiedExamples.Names)if(!Farm.Data.programs.Any(p=>p.name==file))Farm.Data.programs.Add(new ClassicProgram(file,UnifiedExamples.Code(file)));programTarget="network";Switch(Farm.Data.programs.FindIndex(p=>p.name=="universe_controller"));Save();Open("programs");});
        }
    }
    [RequireComponent(typeof(CanvasRenderer))]
    public sealed class ClassicPlanetArt:UnityEngine.UI.Graphic
    {
        void Update(){SetVerticesDirty();}
        protected override void OnPopulateMesh(UnityEngine.UI.VertexHelper mesh)
        {
            mesh.Clear();var rect=rectTransform.rect;float radius=Mathf.Min(rect.width,rect.height)*.41f;Vector2 center=rect.center;
            for(int y=-36;y<=36;y++)for(int x=-36;x<=36;x++)
            {
                float nx=x/37f,ny=y/37f,r=nx*nx+ny*ny;if(r>1)continue;float z=Mathf.Sqrt(1-r),lon=Mathf.Atan2(nx,z)+Time.unscaledTime*.035f;
                float land=Mathf.Sin(lon*3+Mathf.Sin(ny*5))*Mathf.Cos(ny*7)+Mathf.Sin(lon*7+ny*4)*.32f;Color c=ClassicView.ColorOf(land>.15f?"80B393":"356D85");if(Mathf.Abs(ny)>.87f)c=ClassicView.ColorOf("CFDDD2");float light=.35f+.65f*Mathf.Max(0,(-nx*.5f+ny*.25f+z*.8f));c*=light;c.a=1;
                var p=center+new Vector2(nx,ny)*radius;float cell=radius/37f+1;int n=mesh.currentVertCount;mesh.AddVert(new Vector3(p.x,p.y),c,Vector2.zero);mesh.AddVert(new Vector3(p.x,p.y+cell),c,Vector2.zero);mesh.AddVert(new Vector3(p.x+cell,p.y+cell),c,Vector2.zero);mesh.AddVert(new Vector3(p.x+cell,p.y),c,Vector2.zero);mesh.AddTriangle(n,n+1,n+2);mesh.AddTriangle(n,n+2,n+3);
            }
        }
    }
}
