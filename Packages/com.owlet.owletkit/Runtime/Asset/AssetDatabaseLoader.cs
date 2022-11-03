#if UNITY_EDITOR
using System.IO;
using UnityEngine;
using UnityEditor;

namespace Owlet
{
    public class AssetDatabaseLoader : MonoBehaviour, IAssetLoader
    {
        private string rootPath;

        public void SetAssetRootPath(string path)
        {
            rootPath = path;
        }

        public T Load<T>(string assetPath) where T : Object
        {
            var path = Path.Combine(rootPath, assetPath);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        public void UnloadAll(bool unloadAllLoadedObjects)
        {
        }
    }
}
#endif