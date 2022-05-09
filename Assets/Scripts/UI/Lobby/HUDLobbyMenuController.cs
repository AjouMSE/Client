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
        
        [SerializeField] private CanvasGroup lobbyCanvasGroup;
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
        }

        public void OnSettingsBtnClick()
        {
            menuScroll3D.GetComponent<ScrollScript3D>().CloseScroll();
        }

        #endregion


        #region Custom methods

        private void Init()
        {
            // Set title color
            titleText.text = CustomUtils.MakeTitleColor();
            
            // Start fade in effect
            FadeEffectManager.Instance.Fade(FadeEffectManager.FadeType.FadeIn, lobbyCanvasGroup, 2f, FadeInCallback);
            
            BgmManager.Instance.SetBgm(BgmManager.SrcNameMainBgm3);
            BgmManager.Instance.Play();
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
