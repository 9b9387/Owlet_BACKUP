using System;
using Owlet;
using UnityEngine;
using UnityEngine.Events;

public class UITest : MonoBehaviour
{
    private void Start()
    {
        TestLoadingView();
    }

    [ContextMenu("TestLoadingView")]
    public void TestLoadingView()
    {
        AssetLoader.Init<AssetBundleLoader>("Assets/GameAssets");

        UIManager.PushView<UILoadingView>();
    }

    [ContextMenu("TestEvent")]
    public void TestEvent()
    {
        EventCenter.UnsubscribeAll();

        UnityAction<object> l = p =>
        {
            Debug.Log($"111 {p}");
        };

        EventCenter.Subscribe(1, l);

        EventCenter.Subscribe(2, (p) =>
        {
            Debug.Log($"222 {p}");
        });

        EventCenter.Trigger(1, 1);
        EventCenter.Trigger(2, "2");
        EventCenter.Trigger(1, null);

        EventCenter.Unsubscribe(1, l);
        EventCenter.Trigger(1, 1);

        EventCenter.UnsubscribeAll();
        EventCenter.Trigger(1);
        EventCenter.Trigger(2);

    }
}
