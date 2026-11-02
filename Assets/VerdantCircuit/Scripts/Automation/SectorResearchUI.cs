using System;
using System.Linq;
using TMPro;
using UnityEngine;
namespace Verdant.Automation
{
    public sealed partial class ClassicController
    {
        static int TechScope(string id){return id=="spaceflight"||id=="flower_planet"||id=="ocean_planet"||id=="mineral_planet"?-1:0;}
        Color ResearchCategoryColor(string id)
        {
            if(TechScope(id)==-1)return ClassicView.ColorOf("9387EB");
            if(new[]{"loops","operators","variables","functions","lists","dicts","import","utilities"}.Contains(id))return ClassicView.ColorOf("D7A15B");
            if(new[]{"senses","costs","auto_unlock","timing","debug","debug2","hats","megafarm"}.Contains(id))return ClassicView.ColorOf("559FD1");
            return ClassicView.ColorOf("56B58D");
        }
        Color CategoryColor(string type){return ClassicView.ColorOf(type=="transport"?"9387EB":type=="automation"?"559FD1":type=="ecology"?"E69B6B":"56B58D");}
        void BuildResearchScopes(Transform parent)
        {
            for(int i=0;i<5;i++){int scope=i-1;string title=scope==-1?Tr("Universe","宇宙"):IslandName(scope);Skin(Button(title,parent,new Vector2(.025f+i*.173f,.875f),new Vector2(.19f+i*.173f,.918f),()=>{researchScope=scope;localEarthResearch=false;Open("research");}),researchScope==scope?Accent:Dark,Color.white);}
        }
        void BuildSectorResearch()
        {
            ConsoleWindow(Tr("RESEARCH / ","科技／")+(researchScope==-1?Tr("Universe","宇宙"):IslandName(researchScope)));
            var tabs=Panel("Research Scope Tabs",modalBody,Vector2.zero,Vector2.one,Paper);tabs.gameObject.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight=76;
            for(int i=0;i<5;i++){int scope=i-1;Skin(Button(scope==-1?Tr("Universe","宇宙"):IslandName(scope),tabs,new Vector2(i*.2f+.005f,.1f),new Vector2((i+1)*.2f-.005f,.90f),()=>{researchScope=scope;localEarthResearch=false;Open("research");}),researchScope==scope?Accent:Dark,Color.white);}
            Paragraph(Tr("Green: production · blue: automation · amber: ecology · violet: transport. Dark nodes are locked; bright nodes are researched.","绿色：生产 · 蓝色：自动化 · 橙色：生态 · 紫色：运输。暗色未研究，亮色已研究。"),65);
            if(researchScope==-1)
            {
                foreach(var tech in ClassicFarm.Techs.Where(t=>TechScope(t.id)==-1))
                {
                    var t=tech;var row=Row(TechName(t.id),"",Tr("Research","研究"),()=>{if(Farm.Unlock(t.id)){Save();Open("research");}});var label=row.GetComponentsInChildren<TMP_Text>()[1];
                    modalRefreshers.Add(()=>{bool unlocked=Farm.Level(t.id)>0;row.GetComponent<UnityEngine.UI.Image>().color=Color.Lerp(Dark,ResearchCategoryColor(t.id),unlocked?.72f:.12f);label.text=ResearchRequirementsText(t.id,false)+"\n"+(unlocked?Tr("RESEARCHED","已研究"):string.Join(" + ",Farm.Cost(t.id).Select(p=>ItemName((string)p.Key)+" "+Farm.Count((string)p.Key).ToString("N0")+" / "+p.Value)));row.GetComponentInChildren<UnityEngine.UI.Button>().interactable=Farm.CanUnlock(t.id);});
                }
            }
            if(!Farm.Fleet.DepotUnlocked(researchScope)){Paragraph(Tr("Unlock this planet in the Universe research sector first.","请先在宇宙科技中解锁此星球。"),80);return;}
            var map=Surface("Sector Technology Tree",modalBody,Vector2.zero,Vector2.one,Dark);map.gameObject.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight=310;
            var technologies=ArchipelagoWorld.PlanetTechnologies.Where(t=>t.planet==researchScope).ToArray();
            for(int i=0;i<technologies.Length;i++)
            {
                var t=technologies[i];float x=.025f+i*.32f;Color color=CategoryColor(t.category);
                if(i>0)Panel("Research Connection",map,new Vector2(x-.06f,.49f),new Vector2(x+.005f,.50f),color);
                var node=Surface("Technology "+t.id,map,new Vector2(x,.08f),new Vector2(x+.29f,.92f),Dark,true);
                var title=Label(L10n.Code=="en"?t.en:t.zh,node,new Vector2(.06f,.76f),new Vector2(.94f,.95f),22,Color.white);
                var detail=Label("",node,new Vector2(.06f,.23f),new Vector2(.94f,.74f),15,Ink);detail.enableWordWrapping=true;
                var action=Skin(Button(Tr("Research","研究"),node,new Vector2(.06f,.05f),new Vector2(.94f,.21f),()=>{if(Farm.Fleet.ResearchPlanet(t.planet,t.id)){Save();Open("research");}}),color,Color.white);
                modalRefreshers.Add(()=>{int level=Farm.Fleet.PlanetLevel(t.planet,t.id);node.GetComponent<UnityEngine.UI.Image>().color=Color.Lerp(Dark,color,level>0?.68f:.12f);detail.text=level+" / "+t.max+"\n"+(L10n.Code=="en"?t.effectEn:t.effectZh)+"\n"+CargoName(t.item)+" "+Farm.Fleet.Stock(t.planet==-1?0:t.planet,t.item).ToString("N0")+" / "+Farm.Fleet.TechnologyCost(t)+(t.parent==""?"":"\n← "+(technologies.First(v=>v.id==t.parent) is PlanetTechnology parent?(L10n.Code=="en"?parent.en:parent.zh):t.parent));action.interactable=level<t.max&&(t.parent==""||Farm.Fleet.PlanetLevel(t.planet,t.parent)>0)&&Farm.Fleet.Stock(t.planet==-1?0:t.planet,t.item)>=Farm.Fleet.TechnologyCost(t);});
            }
            if(researchScope>=0)
            {
                Row(Tr("Surface exploration","表层勘探"),Farm.Fleet.SurfaceWidth(researchScope)+" × "+Farm.Fleet.SurfaceHeight(researchScope),Tr("Research","研究"),()=>{if(Farm.Fleet.ExpandPlanet(researchScope)){Save();Open("research");}});
                Row(Tr("Worker count","集群数量"),(1+Farm.Fleet.Data.islands[researchScope].slotTier)+" / 8",Tr("Research","研究"),()=>{if(Farm.Fleet.ResearchWorker(researchScope,"slots")){Save();Open("research");}});
                Row(Tr("Hull scale","机体规模"),Farm.Fleet.HullTier(researchScope)+" / 4",Tr("Research","研究"),()=>{if(Farm.Fleet.ResearchWorker(researchScope,"size")){Save();Open("research");}});
            }
            else
            {
                Row(Tr("UFO payload","UFO载荷"),Farm.Fleet.UfoCapacity.ToString(),Tr("Research","研究"),()=>{if(Farm.Fleet.ResearchUfo("cargo")){Save();Open("research");}});
                Row(Tr("UFO count","UFO数量"),(Farm.Fleet.Data.ufoSlots+1)+" / 4",Tr("Research","研究"),()=>{if(Farm.Fleet.ResearchUfo("slots")){Save();Open("research");}});
            }
        }
    }
}
