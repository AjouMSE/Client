using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace UI.Lobby
{
    public class HUDLobbyMenuController : MonoBehaviour
    {
        #region Private variables

        [SerializeField] private GameObject menuScroll3D;


        #endregion


        #region Callbacks

        public void OnPvpBtnClick()
        {
            //todo-Send Match making event to socket.io server
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
