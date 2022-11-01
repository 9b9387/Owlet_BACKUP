using System;
using UnityEngine;

[UI(PrefabPath="UI/Loading/UILoadingView.prefab")]
public class UILoadingView : UIBaseView
{
	protected override void OnLoad()
	{
	}

	protected override void OnUnload()
	{
	}

	public void OnClick_StartButton() 
	{
		Debug.Log("Click");
	}
}