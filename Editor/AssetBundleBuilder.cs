// AssetBundleBuilder.cs
// Author: shihongyang shihongyang@weile.com
// Data: 8/18/2022
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public static class AssetBundleBuilder
{
    private static readonly string AssetRootDirectory = "GameAssets";
    private static readonly string ManifestName = "GameAssets";

    [MenuItem("AssetTest/BuildStandaloneOSX")]
    public static void BuildStandaloneOSX()
    {
        Build(BuildTarget.StandaloneOSX);
    }

    private static void RenameManifest(BuildTarget buildTarget, string newName)
    {
        string outpath = Path.Combine(Application.dataPath, $"../AssetBundle/{buildTarget}/");
        string file1 = Path.Combine(outpath, $"{buildTarget}");
        string newfile1 = Path.Combine(outpath, $"{newName}");


        string file2 = Path.Combine(outpath, $"{buildTarget}.manifest");
        string newfile2 = Path.Combine(outpath, $"{newName}.manifest");

        try
        {
            File.Move(file1, newfile1);
            File.Move(file2, newfile2);
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning(ex.ToString());
        }
    }

    public static void Build(BuildTarget buildTarget)
    {
        AutoBundleName();

        string outpath = Path.Combine(Application.dataPath, $"../AssetBundle/{buildTarget}/");
        if (Directory.Exists(outpath))
        {
            Directory.Delete(outpath, true);
        }

        Directory.CreateDirectory(outpath);
        var manifest = BuildPipeline.BuildAssetBundles(outpath,
            BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
        if (manifest == null)
        {
            Debug.LogError("Build AssetBundle 失败");
            return;
        }

        RenameManifest(buildTarget, ManifestName);

        var assetbundlePath = new DirectoryInfo(outpath);
        var allfiles = assetbundlePath.GetFiles("*.*", SearchOption.AllDirectories);

        if(Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.Delete(Application.streamingAssetsPath, true);
        }
        Directory.CreateDirectory(Application.streamingAssetsPath);
        for (int i = 0; i < allfiles.Length; i++)
        {
            var file = allfiles[i];
            var dest = Path.Combine(Application.streamingAssetsPath, file.Name);
            File.Copy(file.FullName, dest, true);
        }
    }

    [MenuItem("AssetTest/AutoBundleName")]
    public static void AutoBundleName()
    {
        var path = Path.Combine(Application.dataPath, AssetRootDirectory);
        var rootDir = new DirectoryInfo(path);

        var assetDirs = rootDir.GetDirectories();

        for (int i = 0; i < assetDirs.Length; i++)
        {
            var assetDir = assetDirs[i];
            var list = GetAssetFileList(assetDir);
            for (int j = 0; j < list.Count; j++)
            {
                var file = list[j];
                var relatPath = file.FullName.Substring(Application.dataPath.Length + 1);
                relatPath = Path.Combine("Assets", relatPath);

                var assetImporter = AssetImporter.GetAtPath(relatPath);
                if (assetImporter != null)
                {
                    assetImporter.assetBundleName = assetDir.Name.ToLower();
                }
            }
        }
    }

    [MenuItem("AssetTest/ClearBundleName")]
    public static void ClearBundleName()
    {
        var path = Path.Combine(Application.dataPath, AssetRootDirectory);
        var rootDir = new DirectoryInfo(path);
        var assetDirs = rootDir.GetDirectories();

        for (int i = 0; i < assetDirs.Length; i++)
        {
            var assetDir = assetDirs[i];
            var list = GetAssetFileList(assetDir);
            for (int j = 0; j < list.Count; j++)
            {
                var file = list[j];
                var relatPath = file.FullName.Substring(Application.dataPath.Length + 1);
                relatPath = Path.Combine("Assets", relatPath);

                var assetImporter = AssetImporter.GetAtPath(relatPath);
                if (assetImporter != null)
                {
                    assetImporter.assetBundleName = string.Empty;
                }
            }
        }
    }

    private static List<FileInfo> GetAssetFileList(DirectoryInfo dir)
    {
        var list = new List<FileInfo>();
        FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            if(file.Name.StartsWith("."))
            {
                continue;
            }

            if(file.Extension == ".meta"
                || file.Extension == ".cs"
                || file.Extension == ".unitypackage")
            {
                continue;
            }

            list.Add(file);
        }

        return list;
    }
}
