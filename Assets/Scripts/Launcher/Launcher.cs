using UnityEngine;
using Owlet;
using HybridCLR;
using System.Collections.Generic;

public class Launcher : MonoBehaviour
{
    void Start()
    {
        AssetLoader.Init<AssetBundleLoader>("Assets/GameAssets");
        LoadMetadataForAOTAssemblies();

        Debug.Log(Application.persistentDataPath);
        byte[] dllBytes = AssetLoader.Load<TextAsset>("Assemblies/Assembly-CSharp.dll.bytes").bytes;
        System.Reflection.Assembly.Load(dllBytes);

        AssetLoader.Instantiate("GameApp/GameApp.prefab");
    }

    private static void LoadMetadataForAOTAssemblies()
    {
        var AOTMetaAssemblyNames = new List<string>()
        {
            "mscorlib.dll.bytes",
            "System.dll.bytes",
            "System.Core.dll.bytes",
        };

        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in AOTMetaAssemblyNames)
        {
            var res_key = $"Assemblies/{aotDllName}";
            byte[] dllBytes = AssetLoader.Load<TextAsset>(res_key).bytes;
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }
}
