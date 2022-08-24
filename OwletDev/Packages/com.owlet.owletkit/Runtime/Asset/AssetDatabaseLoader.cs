using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AssetDatabaseLoader : IAssetLoader
{
    private readonly string assetRootFolder;

    public AssetDatabaseLoader(string rootPath)
    {
        assetRootFolder = rootPath;
    }

    public T Load<T>(string assetPath) where T : Object
    {
#if UNITY_EDITOR
        var path = Path.Combine(assetRootFolder, assetPath);
        return AssetDatabase.LoadAssetAtPath<T>(path);
#else
        return null;
#endif
    }
}
