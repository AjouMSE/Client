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
        
        [SerializeField] private Text userNameText;
        [SerializeField] private Text hostileNameText;
        
        #endregion
        
        #region Custom methods

        private void Init()
        {
            userNameText.text = UserManager.Instance.User.nickname;
            hostileNameText.text = UserManager.Instance.Hostile.nickname;
        }

        #endregion
        
        
        #region Unity event methods
        
        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        #endregion
    }   
}
