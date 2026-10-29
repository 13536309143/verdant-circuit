using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class VerdantRelease
{
    [MenuItem("Tools/禾序/发行/运行规则与主线回归")]
    public static void Verify()
    {
        if(EditorApplication.isPlayingOrWillChangePlaymode)
            throw new InvalidOperationException("请先停止 Play 模式。");
        VerdantFleetChecks.VerifyAll();
    }

    [MenuItem("Tools/禾序/发行/构建 macOS 通用版")]
    public static void BuildMac()
    {
        if(EditorApplication.isPlayingOrWillChangePlaymode)
            throw new InvalidOperationException("请先停止 Play 模式。");
        string output=Environment.GetEnvironmentVariable("VERDANT_BUILD_PATH");
        if(string.IsNullOrEmpty(output))output="Builds/HeXu-"+Verdant.VerdantContent.Version+".app";
        output=Path.GetFullPath(output);
        Directory.CreateDirectory(Path.GetDirectoryName(output));
        if(!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone,BuildTarget.StandaloneOSX))
            throw new InvalidOperationException("无法切换到 macOS。请安装 Mac Build Support。");
        PlayerSettings.SetArchitecture(NamedBuildTarget.Standalone,2);
        PlayerSettings.bundleVersion=Verdant.VerdantContent.Version;
        var options=new BuildPlayerOptions
        {
            scenes=new[]{"Assets/VerdantCircuit/Scenes/VerdantCircuit.unity"},
            locationPathName=output,
            target=BuildTarget.StandaloneOSX,
            options=BuildOptions.CompressWithLz4
        };
        BuildReport report=BuildPipeline.BuildPlayer(options);
        if(report.summary.result!=BuildResult.Succeeded)
            throw new InvalidOperationException("构建失败："+report.summary.result);
        File.WriteAllText(output+".build-report.json","{\"result\":\"succeeded\",\"errors\":"+report.summary.totalErrors+",\"warnings\":"+report.summary.totalWarnings+",\"bytes\":"+report.summary.totalSize+"}");
        Debug.Log("禾序构建成功："+output+"；错误 "+report.summary.totalErrors+"，警告 "+report.summary.totalWarnings);
    }
    [MenuItem("Tools/禾序/发行/构建 Windows x64 Mono版")]
    public static void BuildWindows()
    {
        if(EditorApplication.isPlayingOrWillChangePlaymode)throw new InvalidOperationException("Stop Play mode first.");
        string folder=Environment.GetEnvironmentVariable("VERDANT_WINDOWS_BUILD_DIR");
        if(string.IsNullOrEmpty(folder))folder="Builds/HeXu-Windows-x64-"+Verdant.VerdantContent.Version;
        folder=Path.GetFullPath(folder);Directory.CreateDirectory(folder);
        if(!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone,BuildTarget.StandaloneWindows64))throw new InvalidOperationException("Windows build module is missing.");
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone,ScriptingImplementation.Mono2x);
        PlayerSettings.bundleVersion=Verdant.VerdantContent.Version;
        BuildReport report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{"Assets/VerdantCircuit/Scenes/VerdantCircuit.unity"},locationPathName=Path.Combine(folder,"HeXu.exe"),target=BuildTarget.StandaloneWindows64,options=BuildOptions.CompressWithLz4});
        if(report.summary.result!=BuildResult.Succeeded)throw new InvalidOperationException("Windows build failed: "+report.summary.result);
        string summary="{\"result\":\"succeeded\",\"errors\":"+report.summary.totalErrors+",\"warnings\":"+report.summary.totalWarnings+",\"bytes\":"+report.summary.totalSize+"}";
        File.WriteAllText(Path.Combine(folder,"build-report.json"),summary);Debug.Log("Windows build succeeded: "+folder);
    }

}
