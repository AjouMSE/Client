using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Lobby.MenuUI
{
    public class HUDLobbyMenuUIController : MonoBehaviour
    {
        #region Private variables

        [Header("Match Making UI")] 
        [SerializeField] private GameObject matchMakingUI;

        [Header("Canvas Groups")] 
        [SerializeField] private CanvasGroup[] canvasGroups;

        [Header("Title text")] 
        [SerializeField] private Text titleText;
        
        [Header("UI Display")] 
        [SerializeField] private GameObject performanceDisplay;
        [SerializeField] private GameObject signOutDisplay;

        [Header("3D Scroll menu UI")] 
        [SerializeField] private ScrollScript3D scroll3D;

        #endregion


        #region Unity event methods

        private void Start()
        {
            Init();
        }

        #endregion


        #region Private methods

        private void Init()
        {
            // Set title color
            titleText.text = CustomUtils.MakeTitleColor();

            // Start fade in effect
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, canvasGroups[0], UIManager.LobbyUIFadeInDuration, () =>
            {
                scroll3D.OpenScroll();
                MatchMakingManager.Instance.SendAuthToServer();
            });
            
            // Set UI display
            UIManager.Instance.SetPerformanceDisplay(performanceDisplay);
            UIManager.Instance.SetSignOutDisplay(signOutDisplay);

            // Set MainBgm
            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.MainBGM3, true);
        }

        #endregion


        #region Button Callbacks

        /// <summary>
        /// PVP button click callback
        /// </summary>
        public void OnPvpBtnClick()
        {
            scroll3D.CloseScroll();
            matchMakingUI.SetActive(true);
            MatchMakingManager.Instance.MatchMaking();
        }

        /// <summary>
        /// PVP cancel button click callback
        /// </summary>
        public void OnPvpCancelBtnClick()
        {
            scroll3D.OpenScroll();
            matchMakingUI.SetActive(false);
            MatchMakingManager.Instance.StopMatchMaking();
        }

        /// <summary>
        /// Leaderboard button click callback
        /// </summary>
        public void OnLeaderBoardBtnClick()
        {
            scroll3D.CloseScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, canvasGroups[1], UIManager.LobbyMenuFadeInOutDuration);
        }

        /// <summary>
        /// Card library button click callback
        /// </summary>
        public void OnCardLibraryBtnClick()
        {
            scroll3D.CloseScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, canvasGroups[2], UIManager.LobbyMenuFadeInOutDuration);
        }

        /// <summary>
        /// User info button click callback
        /// </summary>
        public void OnUserInfoBtnClick()
        {
            scroll3D.CloseScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, canvasGroups[3], UIManager.LobbyMenuFadeInOutDuration);
        }

        /// <summary>
        /// Settings button click callback
        /// </summary>
        public void OnSettingsBtnClick()
        {
            scroll3D.CloseScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, canvasGroups[4], UIManager.LobbyMenuFadeInOutDuration);
        }        
        
        /// <summary>
        /// Sign-out button click callback
        /// </summary>
        public void OnSignOutBtnClick()
        {
            scroll3D.CloseScroll();
            UIManager.Instance.ShowSignOutDisplay();
        }

        #endregion
    }
}