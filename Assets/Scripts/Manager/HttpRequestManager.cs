using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace Manager
{
    public class HttpRequestManager : MonoSingleton<HttpRequestManager>
    {
        private string host = "1.238.82.209";
        private ushort port = 8080;
        private bool secure = false;
        private string uri;

        private HttpRequestManager()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(secure ? "https" : "http");
            sb.Append("://");
            sb.Append(host);
            sb.Append(":");
            sb.Append(port);
            uri = sb.ToString();
        }
        
        #region Public HTTP methods
        
        /// <summary>
        /// Send Get request to REST api server
        /// if param path is "/user"
        /// uri is (not secure connection) "http://hostname:portnumber/user"
        /// </summary>
        /// <param name="path">Path of REST api resource (ex "/user")</param>
        /// <param name="callback">Callback to process the result of get method</param>
        public void Get(string path, Action<UnityWebRequest> callback)
        {
            StartCoroutine(GetReqCoroutine(uri + path, callback));
        }

        /// <summary>
        /// Send Post request to REST api server
        /// if param path is "/user"
        /// uri is (not secure connection) "http://hostname:portnumber/user"
        /// </summary>
        /// <param name="path">Path of REST api resource (ex "/user")</param>
        /// <param name="json">Json string to send REST api server</param>
        /// <param name="callback">Callback to process the result of post method</param>
        public void Post(string path, string json, Action<UnityWebRequest> callback)
        {
            StartCoroutine(PostReqCoroutine(uri + path, json, callback));
        }
        
        #endregion
        
        
        #region Coroutines

        IEnumerator GetReqCoroutine(string path, Action<UnityWebRequest> callback)
        {
            using (UnityWebRequest req = UnityWebRequest.Get(path))
            {
                yield return req.SendWebRequest();
                callback(req);
            }
        }

        IEnumerator PostReqCoroutine(string path, string json, Action<UnityWebRequest> callback)
        {
            byte[] jsonToRaw = new UTF8Encoding().GetBytes(json);
            using (UnityWebRequest req = UnityWebRequest.Post(path, json))
            {
                req.uploadHandler = new UploadHandlerRaw(jsonToRaw);
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");
                yield return req.SendWebRequest();
                callback(req);
            }
        }

        #endregion
    }   
}
