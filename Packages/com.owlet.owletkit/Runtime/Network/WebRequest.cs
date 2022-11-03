// WebRequest.cs
// Author: shihongyang shihongyang@weile.com
// Data: 10/14/2022
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class WebRequest
{
    private static IWebRequest impl;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <typeparam name="T">IWebRequest的实现类型</typeparam>
    public static void Init<T>() where T : MonoBehaviour, IWebRequest
    {
        if (impl != null)
        {
            Debug.LogWarning("WebRequest has initialized");
            return;
        }
        T webRequestImpl = UnityEngine.Object.FindObjectOfType<T>();
        if(webRequestImpl == null)
        {
            var obj = new GameObject("WebRequest");
            webRequestImpl = obj.AddComponent<T>();
        }
        UnityEngine.Object.DontDestroyOnLoad(webRequestImpl.gameObject);

        impl = webRequestImpl;
    }

    public static void Get(string url, Dictionary<string, string> headers,
        Action<UnityWebRequest> action, int timeout = 5)
    {
        if(impl == null)
        {
            throw new Exception("WebRequest has not been initialized");
        }

        impl.Get(url, headers, action, timeout);
    }

    public static void Post(string url, Dictionary<string, string> headers,
        WWWForm postData, Action<UnityWebRequest> action, int timeout = 5)
    {
        if(impl == null)
        {
            throw new Exception("WebRequest has not been initialized");
        }

        impl.Post(url, headers, postData, action, timeout);
    }

    public static void Download(string url, string filePath,
        Action<UnityWebRequest> action = null, Action<UnityWebRequest> progressCallback = null)
    {
        if (impl == null)
        {
            throw new Exception("WebRequest has not been initialized");
        }

        impl.Download(url, filePath, action, progressCallback);
    }

    public static void Upload(string url, byte[] data, Action<bool> action)
    {
        if (impl == null)
        {
            throw new Exception("WebRequest has not been initialized");
        }

        impl.Upload(url, data, action);
    }
}
