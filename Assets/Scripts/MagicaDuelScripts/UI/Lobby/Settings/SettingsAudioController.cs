using System;
using System.Collections;
using System.Collections.Generic;
using Cache;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby.Settings
{
    public class SettingsAudioController : MonoBehaviour
    {
        #region Private variables

        [Header("BGM, SFX Slider")] 
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        [Header("BGM, SFX Toggle Button")] 
        [SerializeField] private Button bgmToggleBtn;
        [SerializeField] private Button sfxToggleBtn;

        [Header("BGM, SFX Percent Text")] 
        [SerializeField] private Text bgmPercentText;
        [SerializeField] private Text sfxPercentText;

        private Image _bgmToggleBtnImg;
        private Image _sfxToggleBtnImg;

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
            _bgmToggleBtnImg = bgmToggleBtn.GetComponent<Image>();
            _sfxToggleBtnImg = sfxToggleBtn.GetComponent<Image>();

            bgmSlider.value = AudioManager.Instance.BGMVolume;
            sfxSlider.value = AudioManager.Instance.SFXVolume;

            bgmPercentText.text = $"{CalculatePercent(bgmSlider).ToString()} %";
            sfxPercentText.text = $"{CalculatePercent(sfxSlider).ToString()} %";
        }

        #endregion


        #region Private methods

        private int CalculatePercent(Slider slider)
        {
            var gap = slider.maxValue - slider.minValue;
            var current = slider.value;

            return (int)(100 * current / gap);
        }

        #endregion


        #region Public UI Callbacks

        /// <summary>
        /// BGM slider callback
        /// </summary>
        public void OnBgmValueChanged()
        {
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.BGM, bgmSlider.value);
            bgmPercentText.text = $"{CalculatePercent(bgmSlider).ToString()} %";
        }

        /// <summary>
        /// SFX slider callback
        /// </summary>
        public void OnSfxValueChanged()
        {
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.SFX, sfxSlider.value);
            sfxPercentText.text = $"{CalculatePercent(sfxSlider).ToString()} %";
        }

        /// <summary>
        /// BGM toggle button callback
        /// </summary>
        public void OnBgmToggleBtnClick()
        {
            _bgmToggleBtnImg.sprite = CacheSpriteSource.Instance.GetSource(AudioManager.Instance.IsBgmMute ? 0 : 1);
            AudioManager.Instance.MuteOrUnmute(AudioManager.VolumeTypes.BGM, !AudioManager.Instance.IsBgmMute);
        }

        /// <summary>
        /// SFX toggle button callback
        /// </summary>
        public void OnSfxToggleBtnClick()
        {
            _sfxToggleBtnImg.sprite = CacheSpriteSource.Instance.GetSource(AudioManager.Instance.IsSfxMute ? 0 : 1);
            AudioManager.Instance.MuteOrUnmute(AudioManager.VolumeTypes.SFX, !AudioManager.Instance.IsSfxMute);
        }

        #endregion
    }
}