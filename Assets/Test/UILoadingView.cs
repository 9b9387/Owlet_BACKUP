using System;
using UnityEngine;
using UnityEngine.UI;
using Owlet;

[UI(PrefabPath="UI/Loading/UILoadingView.prefab")]
public class UILoadingView : UIBaseView
{
	private Slider slider;
	private bool isStart;

	protected override void OnLoad()
	{
		slider = FindComponent<Slider>("ProgressBar");
		slider.value = 0;
	}

	protected override void OnUnload()
	{
	}

	public void OnClick_StartButton() 
	{
		isStart = true;
	}

	public void OnClick_ResetButton()
	{
		isStart = false;
		slider.value = 0;
	}

	private void Update()
	{
		if (isStart && slider.value < 1)
        {
			slider.value += Time.deltaTime * 0.5f;
        }
    }
}