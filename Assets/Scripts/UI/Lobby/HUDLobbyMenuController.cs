using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Test.Networking;
using TMPro;
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
        }

        public void OnPvpBtnClick()
        {
            //todo-Send Match making event to socket.io server
            MatchMakingManager.Instance.MatchMaking();
        }

        public void OnLeaderBoardBtnClick()
        {
            
        }

        public void OnCardLibraryBtnClick()
        {
            
        }

        public void OnUserInfoBtnClick()
        {
            
        }

        public void OnSettingBtnClick()
        {
            
        }

        #endregion


        #region Custom methods

        private void Init()
        {
            // Set title color
            titleText.text = CustomUtils.MakeTitleColor();
            
            // Start fade in effect
            FadeEffectManager.Instance.Fade(FadeEffectManager.FadeType.FadeIn, lobbyCanvasGroup, 2f, FadeInCallback);
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
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                menuScroll3D.GetComponent<ScrollScript3D>().OpenScroll();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                menuScroll3D.GetComponent<ScrollScript3D>().CloseScroll();
            }
        }

        #endregion
    }
}
