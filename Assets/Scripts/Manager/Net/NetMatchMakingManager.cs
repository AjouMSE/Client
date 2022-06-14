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
        #region Private constants

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

        #endregion


        #region Private variables

        private SocketIOCommunicator _sio;
        private bool _isAuthed;

        #endregion


        #region Public methods

        public override void Init()
        {
            if (!IsInitialized)
            {
                NetSocketIOManager.Instance.Init();
                _sio = NetSocketIOManager.Instance.Sio;
                StartCoroutine(MakeConnectionCoroutine());
                IsInitialized = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SendAuthToServer()
        {
            if (!IsInitialized) return;
            if (_isAuthed) return;

            StartCoroutine(EmitEventCoroutine(() =>
            {
                var sendPacket = new Packet.Auth { id = UserManager.Instance.User.id };
                var jsonPayload = JsonUtility.ToJson(sendPacket);
                _sio.Instance.Emit(SioEventAuth, jsonPayload, false);
            }));
        }

        /// <summary>
        /// Start match making (send match making request to node.js server)
        /// </summary>
        public void MatchMaking()
        {
            if (!IsInitialized) return;
            StartCoroutine(EmitEventCoroutine(() => { _sio.Instance.Emit(SioEventStartMatching); }));
        }

        /// <summary>
        /// Stop match making (send stop match making request to node.js server)
        /// </summary>
        public void StopMatchMaking()
        {
            if (!IsInitialized) return;
            StartCoroutine(EmitEventCoroutine(() => { _sio.Instance.Emit(SioEventCancelMatching); }));
        }

        /// <summary>
        /// 
        /// </summary>
        public void Connect()
        {
            if (!IsInitialized) return;
            if (_sio.Instance.IsConnected()) return;
            _sio.Instance.Connect();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect()
        {
            if (!IsInitialized) return;
            if (!_sio.Instance.IsConnected()) return;
            _sio.Instance.Close();
            _isAuthed = false;
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
            _isAuthed = rcvPacket.result;
        }

        /// <summary>
        /// Duplicate login (duplicate auth to node.js server) callback
        /// </summary>
        /// <param name="data"></param>
        private void OnDuplicateLoginCallback(string data)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case UIManager.SceneNameLobby:
                    break;

                case UIManager.SceneNameGameRemaster:
                    if (UserManager.Instance.IsHost)
                    {
                        NetworkManager.Singleton.Shutdown();
                    }

                    break;
            }

            Disconnect();
            UserManager.Instance.SignOutUserInfo();
            UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameLogin);
            UIManager.Instance.DuplicateLoginOccur = true;
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
                    var sendPacket = new Packet.MatchCode { room = rcvPacket.room, code = joinCode };
                    _sio.Instance.Emit(SioEventSendMatchCode, JsonUtility.ToJson(sendPacket), false);

                    // set user to host & change scene
                    UserManager.Instance.SetHost();
                    UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameGameRemaster);
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
            UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameGameRemaster);
        }

        #endregion


        #region Coroutines

        private IEnumerator MakeConnectionCoroutine()
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

        private IEnumerator EmitEventCoroutine(Action callback)
        {
            while (!_sio.Instance.IsConnected())
            {
                yield return null;
            }

            callback();
        }

        #endregion
    }
}