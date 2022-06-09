using System;
using System.Collections;
using System.Collections.Generic;
using Data.Cache;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby.Settings
{
    public class SettingsUIController : MonoBehaviour
    {
        #region Private variables

        [Header("Settings category panels")] 
        [SerializeField] private GameObject[] categoryPanels;

        [Header("3D Scroll Menu UI")] 
        [SerializeField] private GameObject scroll3D;

        private CanvasGroup _settingsCanvasGroup;

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
            _settingsCanvasGroup = GetComponent<CanvasGroup>();
        }

        #endregion


        #region Public methods

        public void ChangeCategory(int idx)
        {
            for (int i = 0; i < categoryPanels.Length; i++)
            {
                categoryPanels[i].SetActive(idx == i);
            }
        }

        #endregion


        #region Callbacks

        /// <summary>
        /// Back button callback
        /// </summary>
        public void OnSettingBackBtnClick()
        {
            scroll3D.GetComponent<ScrollScript3D>().OpenScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, _settingsCanvasGroup,
                UIManager.LobbyMenuFadeInOutDuration);
        }

        #endregion
    }
}