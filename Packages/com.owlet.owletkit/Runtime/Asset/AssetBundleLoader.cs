using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Owlet
{
    public class AssetBundleLoader : MonoBehaviour, IAssetLoader
    {
        private readonly Dictionary<string, AssetBundle> m_LoadedAssetBundles = new Dictionary<string, AssetBundle>();
        private AssetBundleManifest m_AssetBundleManifest = null;
        private string rootPath;
        private static readonly string ManifestName = "gameassets";

        public void SetAssetRootPath(string path)
        {
            rootPath = path;
        }

        public string GetAssetBundlePath(string assetBundleName)
        {
            var persistentPath = Path.Combine(Application.persistentDataPath, assetBundleName);
            if(File.Exists(persistentPath))
            {
                return persistentPath;
            }

            return Path.Combine(Application.streamingAssetsPath, assetBundleName);
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public T Load<T>(string assetPath) where T : Object
        {
            var path = Path.Combine(rootPath, assetPath);

            var assetBundleName = GetAssetBundleName(path);
            if (string.IsNullOrEmpty(assetBundleName))
            {
                return null;
            }
            var assetBundlePath = GetAssetBundlePath(assetBundleName);
            AssetBundle asset = GetAssetBundle(assetBundleName, assetBundlePath);
            if (asset == null)
            {
                Debug.LogWarning($"asset is null {assetBundlePath}");
                return null;
            }

            T ret = asset.LoadAsset<T>(path.ToLower());

            return ret;
        }

        /// <summary>
        /// 获取AssetBundle资源名
        /// 规则为资源根目录下第一层目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetAssetBundleName(string path)
        {
            var relpath = path.Replace(rootPath, "");
            if(relpath.StartsWith("/"))
            {
                relpath = relpath.Substring(1, relpath.Length - 1);
            }

            return relpath.Split('/')[0].ToLower();
        }

        /// <summary>
        /// 通过名称获取AssetBundle资源
        /// </summary>
        /// <param name="assetname"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private AssetBundle GetAssetBundle(string assetname, string path)
        {
            if (m_LoadedAssetBundles.ContainsKey(assetname))
            {
                return m_LoadedAssetBundles[assetname];
            }
            return LoadAssetBundle(assetname, path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private AssetBundle LoadAssetBundle(string assetBundleName, string path)
        {
            LoadDependencies(assetBundleName);
            Unload(assetBundleName); // 确保asset首次加载

            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            // 缓存AssetBundle
            if (assetBundle != null && m_LoadedAssetBundles.ContainsKey(assetBundleName) == false)
            {
                m_LoadedAssetBundles.Add(assetBundleName, assetBundle);
            }
            return assetBundle;
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="unload"></param>
        public void Unload(string assetBundleName, bool unload = false)
        {
            if (m_LoadedAssetBundles.ContainsKey(assetBundleName))
            {
                m_LoadedAssetBundles[assetBundleName].Unload(unload);
                m_LoadedAssetBundles.Remove(assetBundleName);
            }
        }

        /// <summary>
        /// 加载依赖资源
        /// </summary>
        /// <param name="assetname"></param>
        private void LoadDependencies(string assetBundleName)
        {
            if (m_AssetBundleManifest == null)
            {
                m_AssetBundleManifest = LoadManifest();
            }

            if (m_AssetBundleManifest == null)
            {
                Debug.LogWarning("manifset is null.");
                return;
            }

            var deps = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
            //循环加载依赖项
            for (int i = 0; i < deps.Length; i++)
            {
                string dependAssetBundleName = deps[i];
                if (!m_LoadedAssetBundles.ContainsKey(dependAssetBundleName))
                {
                    string path = GetAssetBundlePath(deps[i].ToLower());

                    AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
                    if (assetBundle != null)
                    {
                        m_LoadedAssetBundles.Add(dependAssetBundleName, assetBundle);
                    }
                }
            }
        }

        /// <summary>
        /// 读取Manifest
        /// </summary>
        /// <returns></returns>
        private AssetBundleManifest LoadManifest()
        {
            var manifestPath = Path.Combine(Application.streamingAssetsPath, ManifestName);
            AssetBundle single = AssetBundle.LoadFromFile(manifestPath);
            return single.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        public void UnloadAll(bool unloadAllLoadedObjects)
        {
            foreach (var item in m_LoadedAssetBundles.Values)
            {
                item.Unload(unloadAllLoadedObjects);
            }
            m_LoadedAssetBundles.Clear();
        }
    }
}
