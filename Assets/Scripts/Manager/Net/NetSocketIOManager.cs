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
    public class NetSocketIOManager : MonoSingleton<NetSocketIOManager>
    {
        #region Private constants
        
        private const string Host = "14.33.110.230";
        private const ushort Port = 8081;

        #endregion


        #region Public variables

        public SocketIOCommunicator Sio { get; private set; }

        #endregion


        #region Public methods

        public override void Init()
        {
            if (!IsInitialized)
            {
                Sio = gameObject.AddComponent<SocketIOCommunicator>();
                Sio.socketIOAddress = $"{Host}:{Port.ToString()}";
                Sio.autoReconnect = true;
                Sio.Instance.Connect();
                IsInitialized = true;
            }
        }

        #endregion
    }
}