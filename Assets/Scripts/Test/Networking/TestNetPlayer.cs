using UnityEngine;
using Unity.Netcode;

namespace Test.Networking
{
    public class TestNetPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> netPos = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
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
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = netPos.Value;
        }
    }
}