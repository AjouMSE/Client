using System.Collections;
using System.Collections.Generic;
using Data.Cache;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby.Settings
{
    public class SettingsCategoryButtonRootController : MonoBehaviour
    {
        #region Private variables

        private SettingsUIController _settingsUIController;
        private SettingsCategoryButtonSubController[] _categoryButtonSubControllers;

        #endregion


        #region Unity event methods

        void Start()
        {
            StartCoroutine(CacheSpriteSource.Instance.InitCoroutine()); // temp code
            Init();
        }

        #endregion


        #region Private methods

        private void Init()
        {
            _settingsUIController = GetComponentInParent<SettingsUIController>();
            _categoryButtonSubControllers = GetComponentsInChildren<SettingsCategoryButtonSubController>();
        }

        #endregion


        #region Button callbacks

        public void OnCategoryButtonClick(int idx)
        {
            _settingsUIController.ChangeCategory(idx);

            for (int i = 0; i < _categoryButtonSubControllers.Length; i++)
            {
                _categoryButtonSubControllers[i].ChangeStatus(i == idx);
            }
        }

        #endregion
    }
}