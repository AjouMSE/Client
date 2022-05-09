using System.Collections;
using System.Collections.Generic;
using Firesplash.UnityAssets.SocketIO;
using UnityEngine;
using Utils;

namespace Manager
{
    public class MatchMakingManager : MonoSingleton<MatchMakingManager>
    {
        #region Private variables

        private const string SioEventAuth = "Auth";
        private const string SioEventStartMatch = "StartMatching";
        private const string SioEventMatchMade = "MatchMade";
        private const string SioEventReceiveMatchCode = "ReceiveMatchCode";

        private SocketIOCommunicator _sio;

        #endregion
        
        
        #region Public variables

        public SocketIOCommunicator sio => _sio;
        
        #endregion


        #region Callbacks

        private void OnAuthCallback(string data)
        {
            Debug.Log(JsonUtility.FromJson<Packet.AuthResult>(data).ToString());
            //todo- indicate connection result with socket.io server to client
        }
        
        private void OnStartMatchCallback(string data)
        {
            throw new System.NotImplementedException();
        }
        
        private void OnMatchMadeCallback(string data)
        {
            throw new System.NotImplementedException();
        }
        
        private void OnReceiveMatchCodeCallback(string data)
        {
            throw new System.NotImplementedException();
        }

        #endregion


        #region Custom methods

        private void Init()
        {
            // Init & Make connection with socket.io server
            _sio = GetComponent<SocketIOCommunicator>();
            _sio.Instance.Connect();
            StartCoroutine(MakeConnection());
            
            // Register callbacks to handle socket.io 'On' events.
            _sio.Instance.On(SioEventAuth, OnAuthCallback);
            _sio.Instance.On(SioEventStartMatch, OnStartMatchCallback);
            _sio.Instance.On(SioEventMatchMade, OnMatchMadeCallback);
            _sio.Instance.On(SioEventReceiveMatchCode, OnReceiveMatchCodeCallback);
        }

        public void MatchMaking()
        {
            _sio.Instance.Emit(SioEventStartMatch);
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

            string jsonPayload = JsonUtility.ToJson(new Packet.Auth() { id = UserManager.Instance.User.id });
            _sio.Instance.Emit(SioEventAuth, jsonPayload);
        }

        #endregion
        
    }   
}
