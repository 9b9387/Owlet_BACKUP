using System.IO;
using LitJson;
using UnityEngine;
using Owlet;

public class WebRequestTest : MonoBehaviour
{
    [ContextMenu("TestInit")]
    public void TestInit()
    {
        WebRequest.Init<WebRequestImpl>();
    }

    [ContextMenu("TestGet")]
    public void TestGet()
    {
        var client_ver = 11;
        var url = $"http://127.0.0.1:9376/check_res?ver={client_ver}";

        WebRequest.Get(url, null, (res) =>
        {
            Debug.Log(res.downloadHandler.text);
        });
    }

    [ContextMenu("RequestRemoteVersion")]
    public static void RequestRemoteVersion()
    {
        var client_ver = 11;
        var url = $"http://127.0.0.1:9376/check_res?ver={client_ver}";
        WebRequest.Get(url, null, (data) => {
            if (data == null)
            {
                Debug.LogWarning("response is null.");
            }
            Debug.Log(data.downloadHandler.text);
            var jsonData = JsonMapper.ToObject(data.downloadHandler.text);
            var url = jsonData["url"];
            var manifest = jsonData["manifest"];

            var dl = $"{url}/{manifest}";

            var path = Path.Combine(Application.persistentDataPath, manifest.ToString());
            Debug.Log(path);

            WebRequest.Download(dl, path, (r) => {
                var bytes = File.ReadAllBytes(path);
                var m_data = new AssetManifest(bytes);

                for (int i = 0; i < m_data.list.Count; i++)
                {
                    var u = $"{url}/{m_data.list[i].name}";
                    var p = Path.Combine(Application.persistentDataPath, m_data.list[i].name);
                    Debug.Log($"start download {u} to {p}...");

                    WebRequest.Download(u, p, (re) => {
                        Debug.Log($"finish download {p}");
                    });
                }
            });
        });
    }
}
