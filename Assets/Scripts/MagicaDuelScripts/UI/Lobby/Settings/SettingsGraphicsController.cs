using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Manager;
using Unity.Netcode;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Lobby.Settings
{
    public class SettingsGraphicsController : MonoBehaviour
    {
        #region Private constants

        private const string ScreenModeFullScreen = "Full Screen";
        private const string ScreenModeWindowed = "Windowed";

        private const string PerformanceDisplayEnabled = "Enabled";
        private const string PerformanceDisplayDisabled = "Disabled";

        #endregion

        #region Private variables

        [Header("Graphics setting buttons")] 
        [SerializeField] private Button[] graphicsSettingButtons;

        private Text[] _buttonTexts;

        private int _resolutionIdx;
        private int _frameRateIdx;
        private int _windowModeIdx;
        private bool _showPerformanceDisplay;

        #endregion


        #region Unity event methods

        private void Start()
        {
            Init();
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Init methods
        /// </summary>
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

        /// <summary>
        /// Resolution button click listener
        /// </summary>
        public void OnResolutionButtonClick()
        {
            if (_resolutionIdx < UIManager.Instance.SupportedResolutions.Count - 1)
                _resolutionIdx++;
            else
                _resolutionIdx = 0;

            var resolution = UIManager.Instance.SupportedResolutions[_resolutionIdx];
            _buttonTexts[0].text =
                $"{resolution.x.ToString(CultureInfo.CurrentCulture)} x {resolution.y.ToString(CultureInfo.CurrentCulture)}";
            UIManager.Instance.SetResolution((int)resolution.x, (int)resolution.y);
        }

        /// <summary>
        /// Frame rate button click listener
        /// </summary>
        public void OnFrameRateButtonClick()
        {
            if (_frameRateIdx < UIManager.Instance.SupportedFrameRates.Count - 1)
                _frameRateIdx++;
            else
                _frameRateIdx = 0;

            var frameRate = UIManager.Instance.SupportedFrameRates[_frameRateIdx];
            _buttonTexts[1].text = $"{frameRate.ToString()} hz";
            Application.targetFrameRate = frameRate;
        }

        /// <summary>
        /// Window mode button click listener
        /// </summary>
        public void OnWindowModeButtonClick()
        {
            UIManager.Instance.ChangeScreenMode();
            _buttonTexts[2].text = UIManager.Instance.IsFullScreen ? ScreenModeFullScreen : ScreenModeWindowed;
        }

        /// <summary>
        /// Performance display button click listener
        /// </summary>
        public void OnPerformanceDisplayButtonClick()
        {
            _buttonTexts[3].text = UIManager.Instance.ActivePerformanceDisplay
                ? PerformanceDisplayDisabled
                : PerformanceDisplayEnabled;
            if (UIManager.Instance.ActivePerformanceDisplay)
                UIManager.Instance.HidePerformanceDisplay();
            else
                UIManager.Instance.ShowPerformanceDisplay();
        }

        #endregion
    }
}