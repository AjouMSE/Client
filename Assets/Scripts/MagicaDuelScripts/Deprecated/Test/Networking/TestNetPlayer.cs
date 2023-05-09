using System;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Netcode.Samples;

namespace Test.Networking
{
    public class TestNetPlayer : NetworkBehaviour
    {
        private float moveSpd = 20f;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
        
        Vector3 ProcessPos()
        {
            Vector3 pos = transform.position;
            if (Input.GetKey(KeyCode.W))
            {
                pos.z += moveSpd * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                pos.z -= moveSpd * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                pos.x += moveSpd * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                pos.x -= moveSpd * Time.deltaTime;
            }

            return pos;
        }
        
        private void Update()
        {
            if (IsOwner)
            {
                transform.position = ProcessPos();
            }
        }
    }
    
    /*public class TestNetPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> syncPos = new NetworkVariable<Vector3>();
        public NetworkVariable<int> syncHp = new NetworkVariable<int>();
        private float moveSpd = 20f;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                GetComponent<MeshRenderer>().material.color = Color.red;
                Vector3 spawnPos =
                    new Vector3(UnityEngine.Random.Range(-5f, 5f), 1f, UnityEngine.Random.Range(-5f, 5f));
                if (NetworkManager.Singleton.IsServer)
                {
                    syncPos.Value = spawnPos;
                }
                else
                {
                    SyncPosServerRpc(spawnPos);
                }
            }
        }

        Vector3 ProcessPos()
        {
            Vector3 pos = transform.position;
            if (Input.GetKey(KeyCode.W))
            {
                pos.z += moveSpd * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                pos.z -= moveSpd * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                pos.x += moveSpd * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                pos.x -= moveSpd * Time.deltaTime;
            }

            return pos;
        }

        /*void ProcessInput()
        {
            if (IsOwner)
            {
                Vector3 newPos = ProcessPos();
                
                if (NetworkManager.Singleton.IsServer)
                {
                    syncPos.Value = newPos;
                }
                else
                {
                    SyncPosServerRpc(newPos);
                }
            }
        }

        void ProcessInput2()
        {
            if (IsOwner)
            {
                transform.position = ProcessPos();
                
                if (NetworkManager.Singleton.IsServer)
                {
                    SyncPosClientRpc(transform.position);
                }
                else
                {
                    SyncPosServerRpc(transform.position);
                }
            }
        }

        [ClientRpc]
        void SyncPosClientRpc(Vector3 pos)
        {
            transform.position = pos;
        }

        [ServerRpc]
        void SyncPosServerRpc(Vector3 pos)
        {
            transform.position = pos;
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            ProcessInput2();
        }
    }*/
    
    
    /*public class TestNetPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> netPos = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                GetComponent<MeshRenderer>().material.color = Color.red;
                Move();
            }
        }
        
        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                netPos.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }
        
        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            netPos.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(UnityEngine.Random.Range(-5f, 5f), 1f, UnityEngine.Random.Range(-3f, 3f));
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = netPos.Value;
        }
    }*/
}