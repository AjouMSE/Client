using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class HUDGameInfoController : MonoBehaviour
    {
        #region Private variables
        
        [SerializeField] private Text hostNicknameText;
        [SerializeField] private Text clientNicknameText;
        
        #endregion
        
        #region Custom methods

        private void Init()
        {
            if (!UserManager.Instance.IsHost)
            {
                Text tmp = hostNicknameText;
                hostNicknameText = clientNicknameText;
                clientNicknameText = tmp;
            }
            
            hostNicknameText.text = UserManager.Instance.User.nickname;
            clientNicknameText.text = UserManager.Instance.Hostile.nickname;
        }

        #endregion
        
        
        #region Unity event methods
        
        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        #endregion
    }   
}
