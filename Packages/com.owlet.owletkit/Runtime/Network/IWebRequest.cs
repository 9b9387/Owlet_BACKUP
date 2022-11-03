// IWebRequest.cs
// Author: shihongyang shihongyang@weile.com
// Data: 10/14/2022
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface IWebRequest
{
    public void Get(string url, Dictionary<string, string> headers,
        Action<UnityWebRequest> action, int timeout);

    public void Post(string url, Dictionary<string, string> headers,
        WWWForm postData, Action<UnityWebRequest> action, int timeout);

    public void Download(string url, string filePath,
        Action<UnityWebRequest> action, Action<UnityWebRequest> progressCallback);

    public void Upload(string url, byte[] data, Action<bool> action);
}
