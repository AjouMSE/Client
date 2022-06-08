using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby.Settings
{
    public class SettingsGraphicsController : MonoBehaviour
    {
        #region Private variables

        [Header("Graphics setting buttons")] 
        [SerializeField] private Button[] graphicsSettingButtons;

        private Text[] _buttonTexts;

        private int _resolutionIdx;
        private int _frameRateIdx;
        private int _windowModeIdx;

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
            var settingBtnCnt = graphicsSettingButtons.Length;
            _buttonTexts = new Text[settingBtnCnt];
            
            for (int i = 0; i < settingBtnCnt; i++)
            {
                _buttonTexts[i] = graphicsSettingButtons[i].GetComponentInChildren<Text>();
            }
        }

        #endregion


        #region Public UI callbacks

        public void OnResolutionButtonClick()
        {
            if (_resolutionIdx < 3)
                _resolutionIdx++;
            else
                _resolutionIdx = 0;
            
            var resolution = UIManager.Instance.SupportedResolutions[_resolutionIdx];
            _buttonTexts[0].text = $"{resolution.width.ToString()} x {resolution.height.ToString()}";
            UIManager.Instance.SetResolution(resolution);
        }

        public void OnFrameRateButtonClick()
        {
        }

        public void OnWindowModeButtonClick()
        {
            UIManager.Instance.ChangeScreenMode();
            _buttonTexts[2].text = UIManager.Instance.IsFullScreen ? "Full Screen" : "Windowed";
        }

        #endregion
    }
}