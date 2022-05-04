using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Firesplash.UnityAssets.SocketIO;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

namespace Test.Networking
{
    public class TestNetSocketIO : MonoBehaviour
    {
        public Text allocIdText;
        
        private SocketIOCommunicator _sio;
        private string _playerId;
        private string _allocRegion;
        private string _joinCode;
        private List<Region> _regions;
        private SendMatchPacket _sendPacket;
        
        private Guid _hostAllocId;
        private Guid _clientAllocId;

        struct Packet
        {
            public int idx;

            public Packet(int idx)
            {
                this.idx = idx;
            }
        }
        
        struct ServerPacket
        {
            public bool result;

            public ServerPacket(bool result)
            {
                this.result = result;
            }

            public override string ToString()
            {
                return "Result: " + result;
            }
        }

        struct MatchMadePacket
        {
            public int type;
            public string room;

            public MatchMadePacket(int type, string room)
            {
                this.type = type;
                this.room = room;
            }
        }

        struct SendMatchPacket
        {
            public string room;
            public string code;

            public SendMatchPacket(string room, string code)
            {
                this.room = room;
                this.code = code;
            }
        }

        struct ReceiveMatchPacket
        {
            public string code;

            public ReceiveMatchPacket(string code)
            {
                this.code = code;
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (GUILayout.Button("Start Matching"))
            {
                _sio.Instance.Emit("StartMatching");
            }
            GUILayout.EndArea();
        }
        
        public async void InitService()
        {
            // Init Service
            await UnityServices.InitializeAsync();
            
            // Init Player
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            _playerId = AuthenticationService.Instance.PlayerId;
            
            // Init Region
            _regions = await RelayService.Instance.ListRegionsAsync();
            /*foreach(Region region in _regions)
            {
                Debug.Log("Id: " + region.Id + " / " + "Description: " + region.Description);
            }*/
        }

        public async void Allocation(string room)
        {
            // Init Allocation
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4, _regions.Count > 0 ? _regions[0].Id : null);
            _hostAllocId = allocation.AllocationId;
            _allocRegion = allocation.Region;
            
            // Init Join Code
            _joinCode = await RelayService.Instance.GetJoinCodeAsync(_hostAllocId);
            _sendPacket = new SendMatchPacket(room, _joinCode);
            _sio.Instance.Emit("SendMatchCode", JsonUtility.ToJson(_sendPacket), false);

            UpdateUI();
        }

        public async void Join(string code)
        {
            JoinAllocation join = await RelayService.Instance.JoinAllocationAsync(code);
            _clientAllocId = join.AllocationId;

            UpdateUI();
        }

        public void UpdateUI()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Host ID: ");
            sb.Append(_hostAllocId);
            sb.Append('\n');
            sb.Append("Client ID: ");
            sb.Append(_clientAllocId);
            allocIdText.text = sb.ToString();
        }
        

        // Start is called before the first frame update
        void Start()
        {
            _sio = GetComponent<SocketIOCommunicator>();
            _sio.Instance.Connect();
            InitService();
            UpdateUI();

            // On: Auth
            _sio.Instance.On("Auth", (data) =>
            {
                Debug.Log(data);
                Debug.Log("Auth Result : " + JsonUtility.FromJson<ServerPacket>(data));
            });
            
            // On: StartMatching
            _sio.Instance.On("StartMatching", data =>
            {
                Debug.Log("StartMatching Result : " + JsonUtility.FromJson<ServerPacket>(data));
            });
            
            // On: MatchMade
            _sio.Instance.On("MatchMade", data =>
            {
                _joinCode = null;
                Debug.Log(data);
                MatchMadePacket rcvPacket = JsonUtility.FromJson<MatchMadePacket>(data);
                switch (rcvPacket.type)
                {
                    //Todo-Make type to enum
                    case 0:
                        Allocation(rcvPacket.room);
                        break;
                    
                    case 1:
                        break;
                    
                    default:
                        break;
                }
                Debug.Log("MatchMade Result : " + JsonUtility.FromJson<MatchMadePacket>(data).type);
            });
            
            _sio.Instance.On("ReceiveMatchCode", data =>
            {
                ReceiveMatchPacket rcvPacket = JsonUtility.FromJson<ReceiveMatchPacket>(data);
                Join(rcvPacket.code);
            });
            
            StartCoroutine(Connection());
        }

        IEnumerator Connection()
        {
            while (!_sio.Instance.IsConnected())
            {
                yield return null;
            }

            _sio.Instance.Emit("Auth", JsonUtility.ToJson(new Packet(UnityEngine.Random.Range(0, 10000))), false);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("LogicTest");
            }
        }
    }
}
