using System;
using System.Linq;
using UnityEngine;

namespace Verdant.Automation
{
    [Serializable] public sealed class FunctionReference
    {public string name,signature,category,en,zh,parameters,returns,requirements,errors,parametersEn,returnsEn,requirementsEn,errorsEn,example;}
    [Serializable] public sealed class FunctionReferences {public FunctionReference[] entries;}
    public static class UnifiedManual
    {
        public static readonly FunctionReference[] Functions=JsonUtility.FromJson<FunctionReferences>(Resources.Load<TextAsset>("UnifiedReference").text).entries;
        public static bool Matches(string chapter,string query)
        {return Category(chapter)!=null&&Functions.Any(f=>Category(chapter).Contains(f.category)&&(f.name+f.signature+f.zh+f.en+f.parameters+f.example).IndexOf(query,StringComparison.OrdinalIgnoreCase)>=0);}
        public static string[] Category(string chapter){return chapter=="api"?new[]{"earth","builtin"}:chapter=="world_api"?new[]{"fleet","control","legacy"}:chapter=="world_products"?new[]{"product"}:null;}
        public static ManualChapter Revise(ManualChapter c){return c;}
        public static readonly ManualChapter[] Chapters=new ManualChapter[0];
    }
    public sealed partial class ClassicController
    {
        void BuildFunctionReference(string chapter)
        {
            var categories=UnifiedManual.Category(chapter);if(categories==null)return;
            foreach(var entry in UnifiedManual.Functions.Where(f=>categories.Contains(f.category)&&(manualQuery==""||(f.name+f.signature+f.en+f.zh+f.parameters+f.example).IndexOf(manualQuery,StringComparison.OrdinalIgnoreCase)>=0)))
            {
                bool en=L10n.Code=="en";
                string text=entry.signature+"\n"+(en?entry.en:entry.zh)+"\n\n"+(en?"Parameters: "+entry.parametersEn+"\nReturns: "+entry.returnsEn+"\nRequires: "+entry.requirementsEn+"\nFailure: "+entry.errorsEn:"参数："+entry.parameters+"\n返回："+entry.returns+"\n需要："+entry.requirements+"\n失败："+entry.errors);
                int product=Array.IndexOf(ArchipelagoWorld.NatureFunctions,entry.name);
                if(product>=0)
                {
                    var recipe=ArchipelagoWorld.Recipes.First(r=>r.item==ArchipelagoWorld.NatureItems[product]);text+="\n\n"+(en?"Recipe / island ":"配方／岛号 ")+recipe.island+" / "+(en?"output ":"产出 ")+recipe.amount+" "+UnifiedProductNames.Name(recipe.item)+" / "+recipe.seconds+"s\n"+(en?"Inputs: ":"投入：")+(recipe.inputs.Count==0?(en?"none":"无"):string.Join(" + ",recipe.inputs.Select(p=>UnifiedProductNames.Name(p.Key)+" × "+p.Value)));
                }
                ManualBlock(text);ManualBlock(entry.example,true);
            }
        }
    }
}
