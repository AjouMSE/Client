using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby.UserInfo
{
    public class UserInfoUIController : MonoBehaviour
    {
        #region Private variables
        
        [Header("User Info Text")] 
        [SerializeField] private Text nicknameText;
        [SerializeField] private Text winText;
        [SerializeField] private Text loseText;
        [SerializeField] private Text drawText;
        [SerializeField] private Text rankingText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text rankText;

        [Header("3D Scroll Menu UI")] 
        [SerializeField] private GameObject scroll3D;

        private CanvasGroup _userInfoCanvasGroup;

        #endregion


        #region Unity event methods

        void Start()
        {
            Init();
        }

        #endregion


        #region Private methods
        
        private void Init()
        {
            _userInfoCanvasGroup = GetComponent<CanvasGroup>();

            nicknameText.text = UserManager.Instance.User.nickname;
            winText.text = UserManager.Instance.User.win.ToString();
            loseText.text = UserManager.Instance.User.lose.ToString();
            drawText.text = UserManager.Instance.User.draw.ToString();
            rankingText.text = UserManager.Instance.User.ranking.ToString();
            scoreText.text = UserManager.Instance.User.score.ToString();
            rankText.text = GetTier();
        }

        private string GetTier()
        {
            var tier = "";
            var score = UserManager.Instance.User.score;
            foreach(var key in TableDatas.Instance.tierDatas.Keys)
            {
                var tierData = TableDatas.Instance.GetTierData(key);
                if (score >= tierData.required)
                    tier = tierData.tier;
                else
                    break;
            }

            return tier;
        }

        #endregion


        #region Callbacks

        /// <summary>
        /// Back button callback
        /// </summary>
        public void OnUserInfoBackBtnClick()
        {
            scroll3D.GetComponent<ScrollScript3D>().OpenScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, _userInfoCanvasGroup, UIManager.LobbyMenuFadeInOutDuration);
        }

        #endregion
    }
}