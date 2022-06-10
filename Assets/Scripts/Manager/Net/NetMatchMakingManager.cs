using System;
using System.Collections;
using System.Collections.Generic;
using Firesplash.UnityAssets.SocketIO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Manager.Net
{
    public class NetMatchMakingManager : MonoSingleton<NetMatchMakingManager>
    {
        #region Private variables

        private enum MatchMadeType
        {
            Host = 0,
            Client = 1
        }

        private const string SioEventAuth = "Auth";
        private const string SioEventDuplicateLogin = "DuplicateLogin";
        private const string SioEventCancelMatching = "CancelMatching";
        private const string SioEventStartMatching = "StartMatching";
        private const string SioEventMatchMade = "MatchMade";
        private const string SioEventSendMatchCode = "SendMatchCode";
        private const string SioEventReceiveMatchCode = "ReceiveMatchCode";

        private const string DestSceneName = "GameScene";

        private SocketIOCommunicator _sio;

        #endregion


        #region Public methods

        public override void Init()
        {
            if (!IsInitialized)
            {
                NetSocketIOManager.Instance.Init();
                _sio = NetSocketIOManager.Instance.Sio;
                StartCoroutine(MakeConnection());
                IsInitialized = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendAuthToServer()
        {
            if (!IsInitialized) return;

            Packet.Auth sendPacket = new Packet.Auth { id = UserManager.Instance.User.id };
            string jsonPayload = JsonUtility.ToJson(sendPacket);
            _sio.Instance.Emit(SioEventAuth, jsonPayload, false);
        }

        /// <summary>
        /// Start match making (send match making request to node.js server)
        /// </summary>
        public void MatchMaking()
        {
            if (!IsInitialized) return;
            _sio.Instance.Emit(SioEventStartMatching);
        }

        /// <summary>
        /// Stop match making (send stop match making request to node.js server)
        /// </summary>
        public void StopMatchMaking()
        {
            if (!IsInitialized) return;
            _sio.Instance.Emit(SioEventCancelMatching);
        }

        #endregion


        #region SocketIO Event Callbacks

        /// <summary>
        /// Auth to node.js server
        /// </summary>
        /// <param name="data"></param>
        private void OnAuthCallback(string data)
        {
            Packet.SioReqResult rcvPacket = JsonUtility.FromJson<Packet.SioReqResult>(data);
            Debug.Log(rcvPacket.result
                ? "Successfully authenticated to the Socket.IO server"
                : "Failed to authenticate to the Socket.IO server");
        }

        /// <summary>
        /// Duplicate login (duplicate auth to node.js server) callback
        /// </summary>
        /// <param name="data"></param>
        private void OnDuplicateLoginCallback(string data)
        {
            Debug.Log(data);
            //todo-expire the current user
        }

        /// <summary>
        /// Start match making callback
        /// </summary>
        /// <param name="data"></param>
        private void OnStartMatchingCallback(string data)
        {
            Packet.SioReqResult rcvPacket = JsonUtility.FromJson<Packet.SioReqResult>(data);
            Debug.Log(rcvPacket.result ? "Start match making was successful." : "Start match making was failed.");
        }

        /// <summary>
        /// Match made done callback
        /// Host: Send join code to client
        /// Client: Wait for receiving join code
        /// </summary>
        /// <param name="data"></param>
        private async void OnMatchMadeCallback(string data)
        {
            var rcvPacket = JsonUtility.FromJson<Packet.MatchMadeResult>(data);
            var type = (MatchMadeType)rcvPacket.type;
            UserManager.Instance.AddHostileInfo(rcvPacket.hostile);

            switch (type)
            {
                case MatchMadeType.Host:
                    // send join code to client
                    var joinCode = await NetRelayManager.Instance.StartHost();
                    Packet.MatchCode sendPacket = new Packet.MatchCode { room = rcvPacket.room, code = joinCode };
                    _sio.Instance.Emit(SioEventSendMatchCode, JsonUtility.ToJson(sendPacket), false);

                    // set user to host & change scene
                    UserManager.Instance.SetHost();
                    UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameGame);
                    //SceneManager.LoadSceneAsync("GameSceneRemaster");
                    break;

                case MatchMadeType.Client:
                    UserManager.Instance.SetClient();
                    break;

                default:
                    throw new Exception("UndefinedMatchMadeTypeException");
            }
        }

        /// <summary>
        /// Client Receive match code (joinCode) callback
        /// </summary>
        /// <param name="data"></param>
        private async void OnReceiveMatchCodeCallback(string data)
        {
            var rcvPacket = JsonUtility.FromJson<Packet.MatchCode>(data);
            await NetRelayManager.Instance.StartClient(rcvPacket.code);
            //UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameGame);
            SceneManager.LoadSceneAsync("GameSceneRemaster");
        }

        #endregion


        #region Coroutines

        private IEnumerator MakeConnection()
        {
            // Wait for connecting with socket.io server
            while (!_sio.Instance.IsConnected())
            {
                yield return null;
            }

            // Register callbacks to handle socket.io 'On' events.
            _sio.Instance.On(SioEventAuth, OnAuthCallback);
            _sio.Instance.On(SioEventDuplicateLogin, OnDuplicateLoginCallback);
            _sio.Instance.On(SioEventStartMatching, OnStartMatchingCallback);
            _sio.Instance.On(SioEventMatchMade, OnMatchMadeCallback);
            _sio.Instance.On(SioEventReceiveMatchCode, OnReceiveMatchCodeCallback);
        }

        #endregion
    }
}