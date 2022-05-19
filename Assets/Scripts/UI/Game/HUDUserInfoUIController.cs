using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Game
{
    public class HUDUserInfoUIController : MonoBehaviour
    {
        #region Private variables
        
        [Header("Nickname Text")]
        [SerializeField] private Text hostNicknameText;
        [SerializeField] private Text clientNicknameText;
        
        #endregion
        
        #region Custom methods

        private void Init()
        {
            Packet.User host, client;
            
            if (NetworkManager.Singleton.IsHost)
            {
                host = UserManager.Instance.User;
                client = UserManager.Instance.Hostile;
            }
            else
            {
                host = UserManager.Instance.Hostile;
                client = UserManager.Instance.User;
            }
            
            hostNicknameText.text = host.nickname;
            clientNicknameText.text = client.nickname;
        }

        #endregion
        
        
        #region Unity event methods
        
        void Start()
        {
            Init();
        }

        #endregion
    }   
}
