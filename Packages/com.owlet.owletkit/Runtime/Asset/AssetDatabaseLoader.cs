#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Owlet
{
    public class AssetDatabaseLoader : IAssetLoader
    {
        private readonly string assetRootFolder;

        public AssetDatabaseLoader(string rootPath)
        {
            assetRootFolder = rootPath;
        }

        public T Load<T>(string assetPath) where T : Object
        {
            var path = Path.Combine(assetRootFolder, assetPath);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}
#endif