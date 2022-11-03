using System;
using UnityEngine;
using UnityEngine.UI;
using Owlet;
using LitJson;
using System.IO;

[UI(PrefabPath="UI/Loading/UILoadingView.prefab")]
public class UILoadingView : UIBaseView
{
	private Slider slider;
	private bool isStart;

	protected override void OnLoad()
	{
		WebRequest.Init<WebRequestImpl>();

		slider = FindComponent<Slider>("ProgressBar");
		slider.value = 0;
	}

	protected override void OnUnload()
	{
	}

	public void OnClick_StartButton() 
	{
		var local_manifest = Patcher.LoadLocalAssetManifest();
		Debug.Log(local_manifest.time);
		var url = $"http://127.0.0.1:9376/check_res?ver={local_manifest.time}";
		WebRequest.Get(url, null, (res) =>
		{
			Debug.Log(res.downloadHandler.text);

			var jsonData = JsonMapper.ToObject(res.downloadHandler.text);
			var url = jsonData["url"];
			var manifest = jsonData["manifest"];

			var dl = $"{url}/{manifest}";

			var path = Path.Combine(Application.persistentDataPath, manifest.ToString());
			Debug.Log(path);
			WebRequest.Download(dl, path, (r) => {
				var bytes = File.ReadAllBytes(path);
				var m_data = new AssetManifest(bytes);
				Debug.Log($"size:{m_data.size}");
				for (int i = 0; i < m_data.list.Count; i++)
				{
					var assetInfo = m_data.list[i];
					var needUpdate = false; 
                    for (int j = 0; j < local_manifest.list.Count; j++)
                    {
						var local_asset_info = local_manifest.list[j];
						if(assetInfo.name == local_asset_info.name)
                        {
							if (assetInfo.time > local_asset_info.time && assetInfo.md5 != local_asset_info.md5)
                            {
								needUpdate = true;
							}
							break;
                        }

						needUpdate = true;
                    }

					if(needUpdate == false)
                    {
						continue;
                    }
					var u = $"{url}/{assetInfo.name}";
					var p = Path.Combine(Application.persistentDataPath, assetInfo.name);
					Debug.Log($"start download {u} to {p}...");

					WebRequest.Download(u, p, (re) => {
						Debug.Log($"finish download {p}");
					});
				}
			});
		});
	}

	public void OnClick_ResetButton()
	{
		isStart = false;
		slider.value = 0;
	}

	public void OnClick_TestButton()
	{
		UIManager.PushView<UITestView>();
    }

	private void Update()
	{
		if (isStart && slider.value < 1)
        {
			slider.value += Time.deltaTime * 0.5f;
        }
    }
}