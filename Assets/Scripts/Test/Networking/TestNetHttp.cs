using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace Test.Networking
{
    public class TestNetHttp : MonoSingleton<TestNetHttp>
    {
        #region Public HTTP methods
        
        public void Get(string uri, Action<UnityWebRequest> callback)
        {
            StartCoroutine(GetReqCoroutine(uri, callback));
        }

        public void Post(string uri, string json, Action<UnityWebRequest> callback)
        {
            StartCoroutine(PostReqCoroutine(uri, json, callback));
        }
        
        #endregion
        
        
        #region Coroutines

        IEnumerator GetReqCoroutine(string uri, Action<UnityWebRequest> callback)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(uri))
            {
                yield return req.SendWebRequest();
                callback(req);
            }
        }

        IEnumerator PostReqCoroutine(string uri, string json, Action<UnityWebRequest> callback)
        {
            using (UnityWebRequest req = UnityWebRequest.Post(uri, json))
            {
                byte[] jsonToRaw = new UTF8Encoding().GetBytes(json);
                req.uploadHandler = new UploadHandlerRaw(jsonToRaw);
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");
                yield return req.SendWebRequest();
                callback(req);
            }
        }
        
        IEnumerator PostRequestSignIn(string uri, string json)
        {
            byte[] jsonToRaw = new UTF8Encoding().GetBytes(json);
            using (UnityWebRequest request = UnityWebRequest.Post(uri, json))
            {
                request.uploadHandler = new UploadHandlerRaw(jsonToRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log(request.downloadHandler.text);
                    Debug.Log(request.downloadHandler.data);
                }
                else
                {
                    Debug.Log(request.error);
                }
                Debug.Log(request.responseCode);
            }
        }
        
        #endregion
    }
}