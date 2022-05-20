using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public class HUDLobbySettingsUIController : MonoBehaviour
    {
        #region MyRegion

        private const string SrcPath = "Image/Button";
        private const string SrcNameSoundOn = "/SoundOn";
        private const string SrcNameSoundOff = "/SoundOff";

        private const float DefaultVolumeSliderValue = 0.5f;

        #endregion
        
        
        #region Private variables
        
        [Header("Settings Canvas Group")]
        [SerializeField] private CanvasGroup settingsCvsGroup;

        [Header("BGM, SFX Slider")] 
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        [Header("BGM, SFX Toggle Button")] 
        [SerializeField] private Button bgmToggleBtn;
        [SerializeField] private Button sfxToggleBtn;

        [Header("3D Scroll Menu UI")]
        [SerializeField] private GameObject scroll3D;
        
        [Header("Screen Mode Button Text")]
        [SerializeField] private Text screenModeBtnText;

        private Sprite _unmuteSprite, _muteSprite;
        private Image _bgmButtonImage, _sfxButtonImage;

        #endregion
        
        
        
        #region Callbacks

        /// <summary>
        /// BGM slider callback
        /// </summary>
        public void OnBgmValueChanged()
        {
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.Bgm, bgmSlider.value);
        }

        /// <summary>
        /// SFX slider callback
        /// </summary>
        public void OnSfxValueChanged()
        {
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.Sfx, sfxSlider.value);
        }

        /// <summary>
        /// BGM toggle button callback
        /// </summary>
        public void OnBgmToggleBtnClick()
        {
            if(ReferenceEquals(_bgmButtonImage, null)) 
                _bgmButtonImage = bgmToggleBtn.GetComponent<Image>();
            
            if (AudioManager.Instance.isBgmMute)
                _bgmButtonImage.sprite = _unmuteSprite;
            else
                _bgmButtonImage.sprite = _muteSprite;
            
            AudioManager.Instance.MuteOrUnmute(AudioManager.VolumeTypes.Bgm, !AudioManager.Instance.isBgmMute);
        }

        /// <summary>
        /// SFX toggle button callback
        /// </summary>
        public void OnSfxToggleBtnClick()
        {
            if (ReferenceEquals(_sfxButtonImage, null))
                _sfxButtonImage = sfxToggleBtn.GetComponent<Image>();
            
            if (AudioManager.Instance.isSfxMute)
                _sfxButtonImage.sprite = _unmuteSprite;
            else
                _sfxButtonImage.sprite = _muteSprite;
            
            AudioManager.Instance.MuteOrUnmute(AudioManager.VolumeTypes.Sfx, !AudioManager.Instance.isSfxMute);
        }

        /// <summary>
        /// Resolution button callback
        /// </summary>
        /// <param name="type"></param>
        public void OnResolutionBtnClick(int type)
        {
            UIManager.Instance.SetResolution((UIManager.Resolution169) type);
        }

        /// <summary>
        /// Screen mode button callback
        /// </summary>
        public void OnScreenModeBtnClick()
        {
            UIManager.Instance.ChangeScreenMode();
            screenModeBtnText.text = UIManager.Instance.isFullScreen ? "Full screen" : "Windowed";
        }

        /// <summary>
        /// Back button callback
        /// </summary>
        public void OnSettingBackBtnClick()
        {
            scroll3D.GetComponent<ScrollScript3D>().OpenScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, settingsCvsGroup, UIManager.LobbyMenuFadeInOutDuration);
        }

        #endregion



        #region Custom methods

        private void Init()
        {
            // Set Default volume slider value
            bgmSlider.value = AudioManager.Instance.bgmVolume;
            sfxSlider.value = AudioManager.Instance.sfxVolume;
            
            // Read mute/unmute image sprite from Resources dir
            _unmuteSprite = Resources.Load<Sprite>($"{SrcPath}{SrcNameSoundOn}");
            _muteSprite = Resources.Load<Sprite>($"{SrcPath}{SrcNameSoundOff}");
        }

        #endregion
        

        #region Unity event methods

        private void Start()
        {
            Init();
        }

        #endregion
    }
}