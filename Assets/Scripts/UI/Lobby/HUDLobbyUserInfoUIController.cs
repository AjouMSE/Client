using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public class HUDLobbyUserInfoUIController : MonoBehaviour
    {
        #region Private variables
        
        [Header("User Info Canvas Group")]
        [SerializeField] private CanvasGroup userInfoCvsGroup;

        [Header("User Info Text")] 
        [SerializeField] private Text nicknameText;
        [SerializeField] private Text winText;
        [SerializeField] private Text loseText;
        [SerializeField] private Text drawText;
        [SerializeField] private Text rankingText;
        
        [Header("3D Scroll Menu UI")]
        [SerializeField] private GameObject scroll3D;

        #endregion
        
        
        
        #region Callbacks
        
        /// <summary>
        /// Back button callback
        /// </summary>
        public void OnUserInfoBackBtnClick()
        {
            scroll3D.GetComponent<ScrollScript3D>().OpenScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, userInfoCvsGroup, UIManager.LobbyMenuFadeInOutDuration);
        }

        #endregion

        
        
        #region Custom methods

        private void Init()
        {
            nicknameText.text = UserManager.Instance.User.nickname;
            winText.text = UserManager.Instance.User.win.ToString();
            loseText.text = UserManager.Instance.User.lose.ToString();
            drawText.text = UserManager.Instance.User.draw.ToString();
            rankingText.text = UserManager.Instance.User.ranking.ToString();
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