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

        [Header("3D Scroll menu UI")] 
        [SerializeField] private GameObject menuScroll3D;

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
                menuScroll3D.GetComponent<ScrollScript3D>().OpenScroll();
                MatchMakingManager.Instance.Init();
            });

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
            menuScroll3D.GetComponent<ScrollScript3D>().CloseScroll();
            matchMakingUI.SetActive(true);
            MatchMakingManager.Instance.MatchMaking();
        }

        /// <summary>
        /// PVP cancel button click callback
        /// </summary>
        public void OnPvpCancelBtnClick()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().OpenScroll();
            matchMakingUI.SetActive(false);
            MatchMakingManager.Instance.StopMatchMaking();
        }

        /// <summary>
        /// Leaderboard button click callback
        /// </summary>
        public void OnLeaderBoardBtnClick()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().CloseScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, canvasGroups[1], UIManager.LobbyMenuFadeInOutDuration);
        }

        /// <summary>
        /// Card library button click callback
        /// </summary>
        public void OnCardLibraryBtnClick()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().CloseScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, canvasGroups[2], UIManager.LobbyMenuFadeInOutDuration);
        }

        /// <summary>
        /// User info button click callback
        /// </summary>
        public void OnUserInfoBtnClick()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().CloseScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, canvasGroups[3], UIManager.LobbyMenuFadeInOutDuration);
        }

        /// <summary>
        /// Settings button click callback
        /// </summary>
        public void OnSettingsBtnClick()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().CloseScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, canvasGroups[4], UIManager.LobbyMenuFadeInOutDuration);
        }

        #endregion
    }
}