using UnityEngine;

namespace Owlet.Asset
{
    public static class AssetLoader
    {
        public static AssetConfigSO config;
        public static IAssetLoader loaderImp;

        public static void Init(AssetConfigSO assetConfigSO)
        {
            config = assetConfigSO;
            var root = config.assetRootPath;

#if !UNITY_EDITOR
        config.loadType = AssetLoadType.AssetBundle;
#endif

            if (config.loadType == AssetLoadType.Local)
            {
                loaderImp = new AssetDatabaseLoader(root);
            }
            else if (config.loadType == AssetLoadType.AssetBundle)
            {
                loaderImp = new AssetBundleLoader(root);
            }
        }

        public static T Load<T>(string path) where T : Object
        {
            return loaderImp.Load<T>(path);
        }

        public static GameObject Instantiate(string path)
        {
            if(loaderImp == null)
            {
                return null;
            }

            var prefab = Load<GameObject>(path);
            if(prefab == null)
            {
                return null;
            }

            return Object.Instantiate(prefab);
        }
    }

    public enum AssetLoadType
    {
        Local,
        AssetBundle
    }
}