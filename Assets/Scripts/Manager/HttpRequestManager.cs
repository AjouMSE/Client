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
        #region Private constants

        private const string Host = "localhost";
        private const ushort Port = 8080;
        private const bool secure = false;

        private enum RequestType
        {
            Get,
            Post,
            Put,
            Delete
        }

        #endregion


        #region Private variables

        private string _uri;

        #endregion


        #region Public methods

        public override void Init()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(secure ? "https" : "http");
            sb.Append("://");
            sb.Append(Host);
            sb.Append(":");
            sb.Append(Port);
            _uri = sb.ToString();
        }

        /// <summary>
        /// Send Get request to REST api server
        /// if param path is "/user"
        /// uri is (not secure connection) "http://{hostname}:{port number}/user"
        /// </summary>
        /// <param name="path">Path of REST api resource (ex "/user")</param>
        /// <param name="callback">Callback to process the result of get method</param>
        public void Get(string path, Action<UnityWebRequest> callback)
        {
            StartCoroutine(GetOrDeleteReqCoroutine(RequestType.Get, $"{_uri}{path}", callback));
        }

        /// <summary>
        /// Send Post request to REST api server
        /// if param path is "/user"
        /// uri is (not secure connection) "http://{hostname}:{port number}/user"
        /// </summary>
        /// <param name="path">Path of REST api resource (ex "/user")</param>
        /// <param name="json">Json string to send REST api server</param>
        /// <param name="callback">Callback to process the result of post method</param>
        public void Post(string path, string json, Action<UnityWebRequest> callback)
        {
            StartCoroutine(PostOrPutReqCoroutine(RequestType.Post, $"{_uri}{path}", json, callback));
        }

        /// <summary>
        /// Send Put request to REST api server
        /// if param path is "/user"
        /// uri is (not secure connection) "http://{hostname}:{port number}/user"
        /// </summary>
        /// <param name="path"></param>
        /// <param name="json"></param>
        /// <param name="callback"></param>
        public void Put(string path, string json, Action<UnityWebRequest> callback)
        {
            StartCoroutine(PostOrPutReqCoroutine(RequestType.Put, $"{_uri}{path}", json, callback));
        }

        /// <summary>
        /// Send Delete request to REST api server
        /// if param path is "/user"
        /// uri is (not secure connection) "http://{hostname}:{port number}/user"
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public void Delete(string path, Action<UnityWebRequest> callback)
        {
            StartCoroutine(GetOrDeleteReqCoroutine(RequestType.Delete, $"{_uri}{path}", callback));
        }

        #endregion


        #region Coroutines

        /// <summary>
        /// Get or delete request coroutine
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dest"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private IEnumerator GetOrDeleteReqCoroutine(RequestType type, string dest,
            Action<UnityWebRequest> callback)
        {
            UnityWebRequest req;
            if (type == RequestType.Get)
                req = UnityWebRequest.Get(dest);
            else if (type == RequestType.Delete)
                req = UnityWebRequest.Delete(dest);
            else
                throw new Exception("UndefinedRequestTypeException");

            yield return req.SendWebRequest();
            callback(req);
        }

        /// <summary>
        /// Post or put request coroutine
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dest"></param>
        /// <param name="json"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private IEnumerator PostOrPutReqCoroutine(RequestType type, string dest, string json,
            Action<UnityWebRequest> callback)
        {
            UnityWebRequest req;
            byte[] jsonToRaw = new UTF8Encoding().GetBytes(json);

            if (type == RequestType.Post)
                req = UnityWebRequest.Post(dest, json);
            else if (type == RequestType.Put)
                req = UnityWebRequest.Put(dest, json);
            else
                throw new Exception("UndefinedRequestTypeException");

            req.uploadHandler = new UploadHandlerRaw(jsonToRaw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            yield return req.SendWebRequest();
            callback(req);
        }

        #endregion
    }
}