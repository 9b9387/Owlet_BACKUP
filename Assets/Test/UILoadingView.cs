using UnityEngine;
using UnityEngine.UI;
using Owlet;
using LitJson;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[UI(PrefabPath="UI/Loading/UILoadingView.prefab")]
public class UILoadingView : UIBaseView
{
	private Slider slider;
	private float progress;
	private float begin_progress;
	private float timer = 0;

	protected override void OnLoad()
	{
		WebRequest.Init<WebRequestImpl>();

		slider = FindComponent<Slider>("ProgressBar");
		progress = 0;
		slider.value = progress;
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
			var code = int.Parse(jsonData["code"].ToString());
			if(code == 0)
            {
				Debug.Log("up to data");
				progress = 1;
				return;
            }
			var url = jsonData["url"].ToString();
			var manifest = jsonData["manifest"];

			var dl = $"{url}/{manifest}";

			var path = Path.Combine(Application.persistentDataPath, manifest.ToString());
			if(Directory.Exists(Application.persistentDataPath) == false)
            {
				Directory.CreateDirectory(Application.persistentDataPath);
            }
			//Debug.Log(path);
			WebRequest.Download(dl, path, (r) => {
				var bytes = File.ReadAllBytes(path);
				var remote_manifest = new AssetManifest(bytes);

				var filterInfos = Patcher.FilterUpdateAssetInfo(local_manifest, remote_manifest);

				long totol_download_size = 0;
                for (int i = 0; i < filterInfos.Count; i++)
                {
					var info = filterInfos[i];
					totol_download_size += info.size;
				}

				DownloadAsset(url, Application.persistentDataPath, filterInfos, 0, totol_download_size, 0);

			}, null);
		});
	}

	private void DownloadAsset(string url, string dest, List<AssetInfo> list, int index, long totalSize, long downloadedSize)
    {
		if(index >= list.Count)
        {
			return;
        }

		var info = list[index];
		var download_url = $"{url}/{info.name}";
		var download_path = Path.Combine(dest, info.name);

		WebRequest.Download(download_url, download_path,
			(re) => {
				Debug.Log($"finish download {dest}");
				downloadedSize += (long)re.downloadedBytes;
                index++;
				DownloadAsset(url, dest, list, index, totalSize, downloadedSize);
			},
			(req) => {
                long current_download_size = (long)req.downloadedBytes + downloadedSize;
                progress = current_download_size / (float)totalSize;
				begin_progress = slider.value;
				timer = 0;
			});
	}

	public void OnClick_ResetButton()
	{
		AssetLoader.UnloadAll(false);
        //LoadMetadataForAOTAssemblies();

        Debug.Log(Application.persistentDataPath);
        byte[] dllBytes = AssetLoader.Load<TextAsset>("Assemblies/Assembly-CSharp.dll.bytes").bytes;
        System.Reflection.Assembly.Load(dllBytes);

        var obj = GameObject.Find("GameApp");
        DestroyImmediate(obj);
        AssetLoader.Instantiate("GameApp/GameApp.prefab");
    }

	public void OnClick_TestButton()
	{
		AssetLoader.UnloadAll(false);
		UIManager.PushView<UITestView>();
    }

	private void Update()
	{
		if(progress > slider.value && slider.value <= 1)
        {
			timer += Time.deltaTime;
			slider.value = Mathf.Lerp(begin_progress, progress, timer);
        }
    }
}