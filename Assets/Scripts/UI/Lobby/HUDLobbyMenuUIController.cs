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
        #region Private constants

        private const string CvsNameLobby = "HUD_Lobby";
        private const string CvsNameLobbyUI = "Cvs_Lobby_UI";
        private const string CvsNameUserInfo = "Cvs_Lobby_UserInfo";
        private const string CvsNameSettings = "Cvs_Lobby_Settings";
        private const string CvsNameMatchMaking = "Obj_Lobby_MatchMaking";


        #endregion
        
        
        #region Private variables

        [Header("Camera")] 
        [SerializeField] private Camera hudCamera;

        [Header("Canvas Groups")] 
        [SerializeField] private CanvasGroup lobbyCvsGroup;
        [SerializeField] private CanvasGroup userInfoCvsGroup;
        [SerializeField] private CanvasGroup settingsCvsGroup;
        [SerializeField] private CanvasGroup matchMakingCvsGroup;
        
        [Header("Title text")]
        [SerializeField] private Text titleText;
        
        [Header("3D Scroll menu UI")]
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
            
            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.MainBGM3, true);
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
