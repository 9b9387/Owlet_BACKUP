using Owlet;
using UnityEngine;

public class UITest : MonoBehaviour
{
    private void Start()
    {
        TestLoadingView();
    }

    [ContextMenu("TestLoadingView")]
    public void TestLoadingView()
    {
        var config = ScriptableObject.CreateInstance<AssetConfigSO>();
        config.assetRootPath = "Assets/GameAssets";
        config.loadType = AssetLoadType.Local;
        AssetLoader.Init(config);

        UIManager.PushView<UILoadingView>();
    }
}
