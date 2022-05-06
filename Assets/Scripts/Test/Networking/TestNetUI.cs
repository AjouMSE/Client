using System;
using UnityEngine;
using Unity.Netcode;

namespace Test.Networking
{
    public class TestNetUI : MonoBehaviour
    {
        private void Update()
        {
            /*Debug.Log(NetworkManager.Singleton.IsClient);
            Debug.Log(NetworkManager.Singleton.IsHost);
            Debug.Log(NetworkManager.Singleton.IsServer);
            Debug.Log(NetworkManager.Singleton.IsListening);
            Debug.Log(NetworkManager.Singleton.IsConnectedClient);*/
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
                SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
            
            var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
            var player = playerObject.GetComponent<TestNetPlayer>();
            //GUILayout.Label(player.syncPos.Value + " / HP: " + player.syncHp.Value);
        }
        static void SubmitNewPosition()
        {
            /*if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
            {
                // Host, Client가 아닌 순수 서버인 경우
                if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
                {
                    Debug.Log("Im server");
                    foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                        NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<TestNetPlayer>().Move();
                }
                // Host or Client 인 경우
                else
                {
                    Debug.Log("Im client");
                    var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                    var player = playerObject.GetComponent<TestNetPlayer>();
                    player.Move();
                }
            }*/
        }
    }
}