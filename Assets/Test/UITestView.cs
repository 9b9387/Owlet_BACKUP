using Owlet;
using UnityEngine;

[UI(PrefabPath="UITestView/UITestView.prefab")]
public class UITestView : UIBaseView
{
	protected override void OnLoad()
	{
	}

	protected override void OnUnload()
	{
	}

	public void OnClick_CloseButton()
	{
		Destroy();
	}
}