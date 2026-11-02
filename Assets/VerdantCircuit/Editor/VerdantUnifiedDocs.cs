using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Verdant.Automation;
public static class VerdantUnifiedDocs
{
    [Serializable] public sealed class Example {public string name,code;}
    [Serializable] public sealed class Recipe {public string item,function,role;public int island,output;public double seconds;public Stock[] inputs;}
    [Serializable] public sealed class Island {public int id,order;public string key,en,zh,research;}
    [Serializable] public sealed class Technology {public int planet,max,cost;public string id,en,zh,item,parent,category,effectEn,effectZh;}
    [Serializable] public sealed class Goal {public int planet,amount;public string item;}
    [Serializable] public sealed class Project {public string en,zh,lessonEn,lessonZh;public int reward;public Goal[] goals;}
    [Serializable] public sealed class Reference
    {public string version;public Technology[] technologies;public Project[] projects;public ManualChapter[] chapters;public FunctionReference[] functions;public Example[] examples;public Recipe[] recipes;public Island[] islands;public string[] coreApi,worldApi;}
    public static void Export()
    {
        var data=new Reference{technologies=ArchipelagoWorld.PlanetTechnologies.Select(t=>new Technology{planet=t.planet,max=t.max,cost=t.cost,id=t.id,en=t.en,zh=t.zh,item=t.item,parent=t.parent,category=t.category,effectEn=t.effectEn,effectZh=t.effectZh}).ToArray(),projects=ArchipelagoWorld.Contracts.Select(c=>new Project{en=c.en,zh=c.zh,lessonEn=c.lessonEn,lessonZh=c.lessonZh,reward=c.reward,goals=c.goals.Select(g=>new Goal{planet=g.island,amount=g.amount,item=g.item}).ToArray()}).ToArray(),version=Verdant.VerdantContent.Version,chapters=ClassicManual.Chapters,functions=UnifiedManual.Functions,coreApi=ClassicHost.Api,worldApi=ArchipelagoWorld.Api,
            examples=UnifiedExamples.Names.Select(name=>new Example{name=name,code=UnifiedExamples.Code(name)}).ToArray(),
            recipes=ArchipelagoWorld.Recipes.Select(r=>new Recipe{item=r.item,island=r.island,output=r.amount,seconds=r.seconds,role=string.Join(" / ",r.roles),function=Array.IndexOf(ArchipelagoWorld.NatureItems,r.item)<0?"fleet_work":ArchipelagoWorld.NatureFunctions[Array.IndexOf(ArchipelagoWorld.NatureItems,r.item)],inputs=r.inputs.Select(p=>new Stock(p.Key,p.Value)).ToArray()}).ToArray(),
            islands=ArchipelagoWorld.Definitions.Select((d,i)=>new Island{id=i,key=d.id,en=d.en,zh=d.zh,order=d.unlock,research=i==0?"":ArchipelagoWorld.IslandResearch[i]}).ToArray()};
        Directory.CreateDirectory("docs/reference");File.WriteAllText("docs/reference/world-"+Verdant.VerdantContent.Version+".json",JsonUtility.ToJson(data,true));Debug.Log("Unified runtime documentation exported: "+data.functions.Length+" functions / "+data.chapters.Length+" chapters");
    }
}
