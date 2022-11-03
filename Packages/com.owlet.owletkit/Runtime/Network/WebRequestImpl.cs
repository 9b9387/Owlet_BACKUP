// WebRequestImpl.cs
// Author: shihongyang shihongyang@weile.com
// Data: 10/14/2022
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Owlet
{
    public class WebRequestImpl : MonoBehaviour, IWebRequest
    {
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="headers">消息头</param>
        /// <param name="action">回调方法</param>
        /// <param name="timeout">超时时间</param>
        public void Get(string url, Dictionary<string, string> headers,
            Action<UnityWebRequest> action, int timeout = 5)
        {
            StartCoroutine(DoGet(url, headers, action, timeout));
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <param name="action"></param>
        public void Download(string url, string filePath,
            Action<UnityWebRequest> action, Action<UnityWebRequest> progressCallback)
        {
            StartCoroutine(DoDownload(url, filePath, action, progressCallback));
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="postData"></param>
        /// <param name="action"></param>
        /// <param name="timeout"></param>
        public void Post(string url, Dictionary<string, string> headers,
            WWWForm postData, Action<UnityWebRequest> action, int timeout = 5)
        {
            StartCoroutine(DoPost(url, headers, postData, action, timeout));
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="action"></param>
        public void Upload(string url, byte[] data, Action<bool> action)
        {
            StartCoroutine(DoUpload(url, data, action));
        }

        /// <summary>
        /// 上传协程
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        private IEnumerator DoUpload(string url, byte[] data, Action<bool> action)
        {
            var request = new UnityWebRequest(url);
            UploadHandler handler = new UploadHandlerRaw(data);
            handler.contentType = "application/octet-stream";
            request.uploadHandler = handler;
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"WebRequest.DoUpload {url} error. {request.result}:{request.error}");
                action?.Invoke(false);
                yield break;
            }

            action?.Invoke(true);
            request.Dispose();
        }

        private IEnumerator DoPost(string url, Dictionary<string, string> headers,
            WWWForm postData, Action<UnityWebRequest> action, int timeout)
        {
            var request = UnityWebRequest.Post(url, postData);
            request.timeout = timeout;

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"WebRequest.Post {url} error. {request.result}:{request.error}");
            }

            action?.Invoke(request);
            request.Dispose();
        }


        private IEnumerator DoGet(string url, Dictionary<string, string> headers,
            Action<UnityWebRequest> action, int timeout)
        {
            var request = UnityWebRequest.Get(url);
            request.timeout = timeout;

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"WebRequest.Get {url} error. {request.result}:{request.error}");
            }

            action?.Invoke(request);
            request.Dispose();
        }

        private IEnumerator DoDownload(string url, string filePath,
            Action<UnityWebRequest> completeCallback, Action<UnityWebRequest> progressCallback)
        {
            Debug.Log(filePath);
            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            request.downloadHandler = new DownloadHandlerFile(filePath);
            request.SendWebRequest();

            while(request.isDone == false)
            {
                progressCallback?.Invoke(request);
                yield return null;
            }

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"WebRequest.GetFile {url} error. {request.result}:{request.error}");
            }

            progressCallback?.Invoke(request);
            completeCallback?.Invoke(request);
            request.Dispose();
        }
    }
}
