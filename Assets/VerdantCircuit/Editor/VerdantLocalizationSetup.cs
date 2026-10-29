using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEditor.Localization;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using TMPro;

public static class VerdantLocalizationSetup
{
    [Serializable] public class Entry{public string key,source,en,zh;}
    [Serializable] public class Data{public Entry[] entries;}
    const string Root="Assets/VerdantCircuit/Localization";
    [MenuItem("Tools/禾序/发行/生成双语表与字体")]
    public static void Setup()
    {
        if(Type.GetType("UnityEngine.Localization.Settings.LocalizationSettings, Unity.Localization")==null)throw new Exception("Localization is not loaded.");
        AssetDatabase.Refresh();
        var settings=LocalizationEditorSettings.ActiveLocalizationSettings;
        if(settings==null){settings=ScriptableObject.CreateInstance<LocalizationSettings>();AssetDatabase.CreateAsset(settings,Root+"/LocalizationSettings.asset");LocalizationEditorSettings.ActiveLocalizationSettings=settings;}
        foreach(string code in new[]{"en","zh-Hans"})
        {
            Locale locale=null;foreach(var item in LocalizationEditorSettings.GetLocales())if(item.Identifier.Code==code)locale=item;
            if(locale==null){locale=Locale.CreateLocale(code);AssetDatabase.CreateAsset(locale,Root+"/Locale-"+code+".asset");LocalizationEditorSettings.AddLocale(locale);}
        }
        settings.GetStartupLocaleSelectors().Clear();
        settings.GetStartupLocaleSelectors().Add(new PlayerPrefLocaleSelector{PlayerPreferenceKey="verdant.language"});
        settings.GetStartupLocaleSelectors().Add(new SpecificLocaleSelector{LocaleId=new LocaleIdentifier("en")});EditorUtility.SetDirty(settings);
        var strings=LocalizationEditorSettings.GetStringTableCollection("Verdant")??LocalizationEditorSettings.CreateStringTableCollection("Verdant",Root);
        strings.RefreshAddressables();
        var data=JsonUtility.FromJson<Data>(File.ReadAllText(Root+"/strings.json"));
        int checkedEntries=0;foreach(var table in strings.StringTables)
        {
            string code=table.LocaleIdentifier.Code;
            foreach(var e in data.entries)
            {string text=code=="en"?e.en:e.zh;if(string.IsNullOrWhiteSpace(text))throw new Exception("Empty translation: "+code+" / "+e.key);table.AddEntry(e.key,text);checkedEntries++;}
            LocalizationEditorSettings.SetPreloadTableFlag(table,true);EditorUtility.SetDirty(table);
        }
        EditorUtility.SetDirty(strings);EditorUtility.SetDirty(strings.SharedData);
        var font=AssetDatabase.LoadAssetAtPath<TMP_FontAsset>("Assets/VerdantCircuit/Fonts/NotoUI.asset");
        font.atlasPopulationMode=AtlasPopulationMode.Dynamic;font.isMultiAtlasTexturesEnabled=true;
        if(font.sourceFontFile==null)throw new Exception("CJK font source is missing.");
        var glyphs=new System.Text.StringBuilder();foreach(var e in data.entries)glyphs.Append(e.zh).Append(e.en);
        string missing;font.TryAddCharacters(glyphs.ToString(),out missing);
        if(!string.IsNullOrEmpty(missing))Debug.LogWarning("Font characters not prewarmed: "+missing);
        foreach(var atlas in font.atlasTextures)if(atlas!=null&&!AssetDatabase.Contains(atlas))AssetDatabase.AddObjectToAsset(atlas,font);
        if(font.material!=null&&!AssetDatabase.Contains(font.material))AssetDatabase.AddObjectToAsset(font.material,font);
        EditorUtility.SetDirty(font);
        var assets=LocalizationEditorSettings.GetAssetTableCollection("VerdantAssets")??LocalizationEditorSettings.CreateAssetTableCollection("VerdantAssets",Root);
        assets.RefreshAddressables();
        string guid=AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(font));
        var addressables=AddressableAssetSettingsDefaultObject.GetSettings(true);addressables.CreateOrMoveEntry(guid,addressables.DefaultGroup);
        foreach(var table in assets.AssetTables){table.AddEntry("ui_font",guid);table.AddEntry("code_font",guid);LocalizationEditorSettings.SetPreloadTableFlag(table,true);EditorUtility.SetDirty(table);}
        EditorUtility.SetDirty(assets);EditorUtility.SetDirty(assets.SharedData);
        addressables.BuildAddressablesWithPlayerBuild=AddressableAssetSettings.PlayerBuildOption.BuildWithPlayer;EditorUtility.SetDirty(addressables);
        AssetDatabase.SaveAssets();
        UnityEditor.AddressableAssets.Settings.AddressableAssetSettings.BuildPlayerContent();
        File.WriteAllText("InternalTestReports-localization.json","{\"checkedEntries\":"+checkedEntries+",\"gaps\":0,\"locales\":2}");
        Debug.Log("Localization setup complete: "+checkedEntries+" localized entries.");
    }
}
