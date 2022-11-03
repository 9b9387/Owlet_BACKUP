using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace Owlet
{
    public class Patcher
    {
        public static AssetManifest LoadLocalAssetManifest()
        {
            var localManifestPath = Path.Combine(Application.persistentDataPath, "asset.bin");
            if(File.Exists(localManifestPath) == false)
            {
                localManifestPath = Path.Combine(Application.streamingAssetsPath, "asset.bin");
            }

            if(File.Exists(localManifestPath) == false)
            {
                Debug.LogWarning($"Can not load local version file at:{localManifestPath}");
                return null;
            }
            Debug.Log(localManifestPath);

            var file = File.Open(localManifestPath, FileMode.Open);
            var data1 = new byte[file.Length];
            file.Read(data1, 0, (int)file.Length);
            var manifest = new AssetManifest(data1);
            file.Close();
            file.Dispose();

            return manifest;
        }

        //[MenuItem("AssetTest/CheckAssetsVersion")]
        public static void CheckAssetsVersion()
        {
            var localManifestPath = Path.Combine(Application.streamingAssetsPath, "asset_v1.bin");
            var remoteManifestPath = Path.Combine(Application.streamingAssetsPath, "asset.bin");

            var f1 = File.Open(localManifestPath, FileMode.Open);
            var data1 = new byte[f1.Length];
            f1.Read(data1, 0, (int)f1.Length);
            var m1 = new AssetManifest(data1);
            f1.Close();
            f1.Dispose();

            var f2 = File.Open(remoteManifestPath, FileMode.Open);
            var data2 = new byte[f2.Length];
            f2.Read(data2, 0, (int)f2.Length);
            var m2 = new AssetManifest(data2);
            f2.Close();
            f2.Dispose();

            if (m1.time >= m2.time)
            {
                Debug.Log("已经是最新版本，无需更新");
                return;
            }

            var updateAssetList = new List<AssetInfo>();
            for (int i = 0; i < m2.list.Count; i++)
            {
                var isMatch = false;
                var info2 = m2.list[i];

                for (int j = 0; j < m1.list.Count; j++)
                {
                    var info1 = m1.list[j];

                    if (info1.name == info2.name)
                    {
                        isMatch = true;
                        if (info1.md5 != info2.md5 && info1.time < info2.time)
                        {
                            Debug.Log($"{info1.name} 需要更新");
                            updateAssetList.Add(info2);
                        }
                        break;
                    }
                }

                if (isMatch == false)
                {
                    Debug.Log($"{info2.name} 新增资源");
                    updateAssetList.Add(info2);
                }
            }
        }
    }
}