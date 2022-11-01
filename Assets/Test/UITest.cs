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


    [ContextMenu("TestEvent")]
    public void TestEvent()
    {
        EventCenter.UnsubscribeAll();

        EventCenter.Subscribe("test", (p) =>
        {
            Debug.Log($"111 {p}");
        });

        EventCenter.Subscribe("test", (p) =>
        {
            Debug.Log($"222 {p}");
        });

        EventCenter.Trigger("test", 1);
        EventCenter.Trigger("test", "2");
        EventCenter.Trigger("test", null);
    }
}
