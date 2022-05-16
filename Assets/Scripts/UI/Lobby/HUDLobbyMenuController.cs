using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Lobby
{
    public class HUDLobbyMenuController : MonoBehaviour
    {
        #region Private variables

        [SerializeField] private CanvasGroup lobbyCvsGroup, userInfoCvsGroup, settingsCvsGroup, matchMakingCvsGroup;
        [SerializeField] private Text titleText;
        [SerializeField] private GameObject menuScroll3D;

        #endregion


        #region Callbacks
        
        private void FadeInCallback()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().OpenScroll();
            MatchMakingManager.Instance.Init();
        }
        
        public void OnPvpBtnClick()
        {
            //todo-Send Match making event to socket.io server
            menuScroll3D.GetComponent<ScrollScript3D>().CloseScroll();
            MatchMakingManager.Instance.MatchMaking();
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, matchMakingCvsGroup, UIManager.LobbyMenuFadeInOutDuration);
        }

        public void OnLeaderBoardBtnClick()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().CloseScroll();
        }

        public void OnCardLibraryBtnClick()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().CloseScroll();
        }

        public void OnUserInfoBtnClick()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().CloseScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, userInfoCvsGroup, UIManager.LobbyMenuFadeInOutDuration);
        }

        public void OnSettingsBtnClick()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().CloseScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, settingsCvsGroup, UIManager.LobbyMenuFadeInOutDuration);
        }

        #endregion


        #region Custom methods

        private void Init()
        {
            // Set title color
            titleText.text = CustomUtils.MakeTitleColor();
            
            // Start fade in effect
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, lobbyCvsGroup, UIManager.LobbyUIFadeInDuration, FadeInCallback);
            
            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.MainBgm3, true);
        }

        #endregion


        #region Unity event methods

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            // Test code
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuScroll3D.GetComponent<ScrollScript3D>().OpenScroll();
            }
        }

        #endregion
    }
}
