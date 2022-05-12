using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public class HUDLobbyUserInfoController : MonoBehaviour
    {
        #region Private variables
        
        [SerializeField] private GameObject scroll3D;
        [SerializeField] private CanvasGroup userInfoCvsGroup;
        [SerializeField] private Text nicknameText, winText, loseText, drawText, rankingText;

        #endregion
        
        
        
        #region Callbacks
        
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