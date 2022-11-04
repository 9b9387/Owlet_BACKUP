using System.Collections.Generic;
using System.IO;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using UnityEditor;
using UnityEngine;

public class HybridCLRBuild
{
    [MenuItem("Owlet/Build/StandaloneOSX")]
    public static void Build()
    {
        var target = BuildTarget.StandaloneOSX;
        //PrebuildCommand.GenerateAll();
        //if (BuildPlayer(target) == false)
        //{
        //    Debug.Log("Build Player Failed.");
        //    return;
        //};
        CompileDllCommand.CompileDll(target);
        CopyAOTAssembliesToGameAssets();
        CopyHotUpdateAssembliesToGameAssets();
        Owlet.AssetBundleBuilder.Build(target);
    }

    public static bool BuildPlayer(BuildTarget target)
    {
        var scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }

        string outputPath = $"{SettingsUtil.ProjectDir}/Release-{target}";
        string location = $"{outputPath}/HybridDemo.app";

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
        {
            scenes = scenes,
            locationPathName = location,
            options = BuildOptions.CompressWithLz4,
            target = target,
            targetGroup = BuildTargetGroup.Standalone,
        };

        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        return report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded;
    }

    public static void CopyAOTAssembliesToGameAssets()
    {
        var target = EditorUserBuildSettings.activeBuildTarget;
        string aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
        string aotAssembliesDstDir = Path.Combine(Application.dataPath, "GameAssets", "Assemblies");

        var AOTMetaAssemblyNames = new List<string>()
        {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll",
        };

        foreach (var dll in AOTMetaAssemblyNames)
        {
            string srcDllPath = $"{aotAssembliesSrcDir}/{dll}";
            if (!File.Exists(srcDllPath))
            {
                Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                continue;
            }
            string dllBytesPath = $"{aotAssembliesDstDir}/{dll}.bytes";
            File.Copy(srcDllPath, dllBytesPath, true);
            Debug.Log($"[CopyAOTAssembliesToGameAssets] copy AOT dll {srcDllPath} -> {dllBytesPath}");
        }
    }

    public static void CopyHotUpdateAssembliesToGameAssets()
    {
        var target = EditorUserBuildSettings.activeBuildTarget;

        string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
        string hotfixAssembliesDstDir = Path.Combine(Application.dataPath, "GameAssets", "Assemblies");

        foreach (var dll in SettingsUtil.HotUpdateAssemblyFiles)
        {
            string dllPath = $"{hotfixDllSrcDir}/{dll}";
            string dllBytesPath = $"{hotfixAssembliesDstDir}/{dll}.bytes";
            File.Copy(dllPath, dllBytesPath, true);
            Debug.Log($"[CopyHotUpdateAssembliesToGameAssets] copy hotfix dll {dllPath} -> {dllBytesPath}");
        }
    }
}
