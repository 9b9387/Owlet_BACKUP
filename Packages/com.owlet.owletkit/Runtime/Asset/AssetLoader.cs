using UnityEngine;

namespace Owlet
{
    public static class AssetLoader
    {
        public static IAssetLoader loaderImpl;

        public static void Init<T>(string assetRootPath) where T : MonoBehaviour, IAssetLoader
        {
            if(loaderImpl != null)
            {
                Debug.LogWarning("AssetLoader has initialized");
                return;
            }
            T impl = Object.FindObjectOfType<T>();

            if (impl == null)
            {
                var obj = new GameObject("AssetLoader");
                impl = obj.AddComponent<T>();
                impl.SetAssetRootPath(assetRootPath);
            }
            Object.DontDestroyOnLoad(impl.gameObject);

            loaderImpl = impl;
        }

        public static T Load<T>(string path) where T : Object
        {
            if(loaderImpl == null)
            {
                throw new System.Exception("AssetLoader has not been initialized");
            }
            
            return loaderImpl.Load<T>(path);
        }

        public static GameObject Instantiate(string path)
        {
            if(loaderImpl == null)
            {
                throw new System.Exception("AssetLoader has not been initialized");
            }

            var prefab = Load<GameObject>(path);
            if(prefab == null)
            {
                return null;
            }

            var obj = Object.Instantiate(prefab);
            obj.name = obj.name.Replace("(Clone)", "");
            return obj;
        }

        public static void UnloadAll(bool unloadAllLoadedObjects)
        {
            if (loaderImpl == null)
            {
                throw new System.Exception("AssetLoader has not been initialized");
            }

            loaderImpl.UnloadAll(unloadAllLoadedObjects);
        }
    }
}