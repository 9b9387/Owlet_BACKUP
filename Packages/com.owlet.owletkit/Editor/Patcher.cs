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

        [MenuItem("AssetTest/GenerateAssetManifest")]
        public static void GenerateAssetManifest()
        {
            var path = Path.Combine(Application.dataPath, "ResDemo");
            Debug.Log(path);
            var files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            var manifest = new AssetManifest();
            manifest.time = DateTime.Now.Ticks;

            for (int i = 0; i < files.Length; i++)
            {
                var am = new AssetInfo();
                var file = files[i];
                
                if (file.EndsWith(".meta") || file.StartsWith("."))
                {
                    continue;
                }

                var fileInfo = new FileInfo(file);
                am.size = fileInfo.Length;
                am.time = fileInfo.LastWriteTime.Ticks;
                am.name = fileInfo.Name;
                using (var fs = File.Open(file, FileMode.Open))
                {
                    var md5 = MD5.Create();
                    var fileMD5Bytes = md5.ComputeHash(fs);
                    am.md5 = BitConverter.ToString(fileMD5Bytes).Replace("-", "").ToLower();
                }
                Debug.Log(am.ToString());
                manifest.list.Add(am);
            }

            var f = File.Create(Path.Combine(Application.streamingAssetsPath, "asset.bin"));
            var bytes = manifest.ToBytes();
            f.Write(bytes, 0, bytes.Length);
            f.Close();
            f.Dispose();

            f = File.Open(Path.Combine(Application.streamingAssetsPath, "asset.bin"), FileMode.Open);
            var data = new byte[f.Length];
            f.Read(data, 0, (int)f.Length);
            var m = new AssetManifest(data);
            f.Close();
            f.Dispose();

            for (int i = 0; i < m.list.Count; i++)
            {
                Debug.Log(m.list[i].ToString());
            }
        }
    }
}