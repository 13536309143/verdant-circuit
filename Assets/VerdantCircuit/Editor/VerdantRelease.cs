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
        string folder=Path.GetFullPath("InternalTestReports/release");
        Directory.CreateDirectory(folder);
        File.WriteAllText(Path.Combine(folder,"verification.json"),VerdantChecks.RunAll());
        File.WriteAllText(Path.Combine(folder,"campaign.json"),VerdantChecks.Playthrough());
        Debug.Log("禾序发行回归通过："+folder);
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
        Debug.Log("禾序构建成功："+output+"；错误 "+report.summary.totalErrors+"，警告 "+report.summary.totalWarnings);
    }
}
