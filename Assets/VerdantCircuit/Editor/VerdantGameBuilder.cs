using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using Verdant;

public static class VerdantGameBuilder
{
    const string Root="Assets/VerdantCircuit";
    [MenuItem("Tools/禾序/创建主游戏场景")]
    public static void Build()
    {
        if(Application.isPlaying)throw new InvalidOperationException("请先停止运行");
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().isDirty)throw new InvalidOperationException("请先保存当前场景");
        Folder(Root+"/Fonts");Folder(Root+"/Scenes");Folder(Root+"/Resources");
        if(AssetDatabase.LoadAssetAtPath<Material>(Root+"/Resources/StandardReference.mat")==null)AssetDatabase.CreateAsset(new Material(Shader.Find("Standard")),Root+"/Resources/StandardReference.mat");
        var font=AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(Root+"/Fonts/NotoUI.asset");
        if(font==null)
        {
            var source=AssetDatabase.LoadAssetAtPath<Font>(Root+"/Fonts/NotoSansCJKsc-Regular.otf");
            if(source==null)throw new Exception("未找到 Noto 字体文件");
            font=TMP_FontAsset.CreateFontAsset(source,40,4,GlyphRenderMode.SDFAA,2048,2048,AtlasPopulationMode.Dynamic,true);font.name="Noto UI";
            string glyphs=new string(Directory.GetFiles(Root,"*.cs",SearchOption.AllDirectories).SelectMany(path=>File.ReadAllText(path)).Where(c=>c>=' '&&!char.IsSurrogate(c)).Concat(Enumerable.Range(32,95).Select(i=>(char)i)).Distinct().ToArray());
            string missing;font.TryAddCharacters(glyphs,out missing);font.atlasPopulationMode=AtlasPopulationMode.Static;
            AssetDatabase.CreateAsset(font,Root+"/Fonts/NotoUI.asset");
            if(font.material!=null)AssetDatabase.AddObjectToAsset(font.material,font);
            foreach(var texture in font.atlasTextures)if(texture!=null)AssetDatabase.AddObjectToAsset(texture,font);
            Debug.Log("禾序字体已烘焙："+glyphs.Length+" 字符，缺失："+missing);
        }
        font.atlasPopulationMode=AtlasPopulationMode.Dynamic;
        string allGlyphs=new string(Directory.GetFiles(Root,"*.cs",SearchOption.AllDirectories).SelectMany(path=>File.ReadAllText(path)).Where(c=>c>=' '&&!char.IsSurrogate(c)).Distinct().ToArray());
        string remaining;font.TryAddCharacters(allGlyphs,out remaining);
        foreach(var texture in font.atlasTextures)if(texture!=null&&!AssetDatabase.Contains(texture))AssetDatabase.AddObjectToAsset(texture,font);
        font.atlasPopulationMode=AtlasPopulationMode.Static;EditorUtility.SetDirty(font);
        var mono=AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(Root+"/Fonts/CodeFont.asset");
        if(mono==null)
        {
            if(!AssetDatabase.CopyAsset("Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF.asset",Root+"/Fonts/CodeFont.asset"))throw new Exception("未找到 TMP 拉丁字体");
            mono=AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(Root+"/Fonts/CodeFont.asset");mono.fallbackFontAssetTable.Add(font);EditorUtility.SetDirty(mono);
        }
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);scene.name="VerdantCircuit";
        var root=new GameObject("禾序 · Verdant Circuit").AddComponent<VerdantApp>();root.uiFont=font;root.codeFont=mono;
        EditorSceneManager.SaveScene(scene,Root+"/Scenes/VerdantCircuit.unity");
        EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(Root+"/Scenes/VerdantCircuit.unity",true)};
        PlayerSettings.companyName="Fieldworks";PlayerSettings.productName="禾序";PlayerSettings.bundleVersion=VerdantContent.Version;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone,"com.fieldworks.verdantcircuit");
        PlayerSettings.defaultScreenWidth=1600;PlayerSettings.defaultScreenHeight=1000;PlayerSettings.fullScreenMode=FullScreenMode.Windowed;PlayerSettings.resizableWindow=true;PlayerSettings.runInBackground=true;
        AssetDatabase.SaveAssets();Selection.activeGameObject=root.gameObject;
        Debug.Log("禾序主场景已保存，点击 Play 开始。");
    }
    static void Folder(string path){if(AssetDatabase.IsValidFolder(path))return;string parent=Path.GetDirectoryName(path).Replace('\\','/');Folder(parent);AssetDatabase.CreateFolder(parent,Path.GetFileName(path));}
}
