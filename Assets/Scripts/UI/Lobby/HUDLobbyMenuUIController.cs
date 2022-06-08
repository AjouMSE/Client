using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Lobby
{
    public class HUDLobbyMenuUIController : MonoBehaviour
    {
        #region Private variables

        [Header("Camera")] [SerializeField] private Camera hudCamera;

        [Header("Match Making UI")] [SerializeField]
        private GameObject matchMakingUI;

        [Header("Canvas Groups")] [SerializeField]
        private CanvasGroup lobbyCvsGroup;

        [SerializeField] private CanvasGroup userInfoCvsGroup;
        [SerializeField] private CanvasGroup settingsCvsGroup;

        [Header("Title text")] [SerializeField]
        private Text titleText;

        [Header("3D Scroll menu UI")] [SerializeField]
        private GameObject menuScroll3D;

        private CanvasGroup _lobbyCanvasGroup;
        private CanvasGroup _leaderBoardCanvasGroup;
        private CanvasGroup _cardLibCanvasGroup;
        private CanvasGroup _userInfoCanvasGroup;
        private CanvasGroup _settingsCvsGroup;

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


        #region Private methods

        private void Init()
        {
            _lobbyCanvasGroup = GetComponent<CanvasGroup>();
            //_leaderBoardCanvasGroup = GetComponentsInChildren<CanvasGroup>()[0];
            
            // Set title color
            titleText.text = CustomUtils.MakeTitleColor();

            // Start fade in effect
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, lobbyCvsGroup, UIManager.LobbyUIFadeInDuration,
                FadeInCallback);

            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.MainBGM3, true);
        }

        #endregion


        #region Button Callbacks

        private void FadeInCallback()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().OpenScroll();
            MatchMakingManager.Instance.Init();
        }

        public void OnPvpBtnClick()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().CloseScroll();
            matchMakingUI.SetActive(true);
            MatchMakingManager.Instance.MatchMaking();
        }

        public void OnPvpCancelBtnClick()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().OpenScroll();
            matchMakingUI.SetActive(false);
            MatchMakingManager.Instance.StopMatchMaking();
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
    }
}