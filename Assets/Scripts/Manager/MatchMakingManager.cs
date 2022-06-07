using System;
using System.Collections;
using System.Collections.Generic;
using Firesplash.UnityAssets.SocketIO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Manager
{
    public class MatchMakingManager : MonoSingleton<MatchMakingManager>
    {
        #region Private variables

        private enum MatchMadeType
        {
            Host = 0,
            Client = 1
        }

        private const string SioEventAuth = "Auth";
        private const string SioEventCancelMatching = "CancelMatching";
        private const string SioEventStartMatching = "StartMatching";
        private const string SioEventMatchMade = "MatchMade";
        private const string SioEventSendMatchCode = "SendMatchCode";
        private const string SioEventReceiveMatchCode = "ReceiveMatchCode";

        private const string DestSceneName = "GameScene";

        private SocketIOCommunicator _sio;

        #endregion
        
        
        #region Public variables

        public SocketIOCommunicator sio => _sio;
        
        #endregion


        #region Callbacks
        
        private void OnAuthCallback(string data)
        {
            Packet.SioReqResult rcvPacket = JsonUtility.FromJson<Packet.SioReqResult>(data);
            Debug.Log("Auth" + rcvPacket.ToString());

            if (rcvPacket.result)
            {
                //todo- show auth result in ui
            }
            else
            {
                //todo- show auth result in ui
            }
        }
        
        private void OnStartMatchingCallback(string data)
        {
            Packet.SioReqResult rcvPacket = JsonUtility.FromJson<Packet.SioReqResult>(data);
            Debug.Log("Start Matching" + rcvPacket.ToString());
            
            if (rcvPacket.result)
            {
                //todo- show start matching result in ui
            }
            else
            {
                //todo- show start matching result in ui
            }
        }
        
        private async void OnMatchMadeCallback(string data)
        {
            Packet.MatchMadeResult rcvPacket = JsonUtility.FromJson<Packet.MatchMadeResult>(data);
            UserManager.Instance.AddHostileInfo(rcvPacket.hostile);
            
            Debug.Log("Match Made: " + rcvPacket);
            Debug.Log("Hostile: " + UserManager.Instance.Hostile);

            if (rcvPacket.type == (int)MatchMadeType.Host)
            {
                string joinCode = await RelayManager.Instance.StartHost();
                Packet.MatchCode sendPacket = new Packet.MatchCode { room = rcvPacket.room, code = joinCode };
                UserManager.Instance.SetHost();
                _sio.Instance.Emit(SioEventSendMatchCode, JsonUtility.ToJson(sendPacket), false);
                SceneManager.LoadSceneAsync(DestSceneName);
            }
            else
            {
                UserManager.Instance.SetClient();
            }
        }
        
        private async void OnReceiveMatchCodeCallback(string data)
        {
            Packet.MatchCode rcvPacket = JsonUtility.FromJson<Packet.MatchCode>(data);
            await RelayManager.Instance.StartClient(rcvPacket.code);
            Debug.Log("Client received code: " + rcvPacket);
            SceneManager.LoadSceneAsync(DestSceneName);
        }

        #endregion


        #region Custom methods

        public override void Init()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                
                // Set Up Socket.io
                _sio = gameObject.AddComponent<SocketIOCommunicator>();
                _sio.socketIOAddress = "localhost:8081";
                _sio.autoReconnect = true;
                _sio.Instance.Connect();
                StartCoroutine(MakeConnection());
            }
        }

        public void MatchMaking()
        {
            // Send match making request to socket.io server
            _sio.Instance.Emit(SioEventStartMatching);
        }

        public void StopMatchMaking()
        {
            _sio.Instance.Emit(SioEventCancelMatching);
        }

        #endregion


        #region Unity event methods
        
        #endregion




        #region Coroutines

        IEnumerator MakeConnection()
        {
            // Wait for connecting with socket.io server
            while (!_sio.Instance.IsConnected())
            {
                yield return null;
            }
                        
            // Register callbacks to handle socket.io 'On' events.
            _sio.Instance.On(SioEventAuth, OnAuthCallback);
            _sio.Instance.On(SioEventStartMatching, OnStartMatchingCallback);
            _sio.Instance.On(SioEventMatchMade, OnMatchMadeCallback);
            _sio.Instance.On(SioEventReceiveMatchCode, OnReceiveMatchCodeCallback);
            
            Packet.Auth sendPacket = new Packet.Auth { id = UserManager.Instance.User.id };
            string jsonPayload = JsonUtility.ToJson(sendPacket);
            _sio.Instance.Emit(SioEventAuth, jsonPayload, false);
        }

        #endregion
        
    }   
}
