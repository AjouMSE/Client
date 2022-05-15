using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Lobby
{
    public class HUDLobbySettingsController : MonoBehaviour
    {
        #region Private variables
        
        [SerializeField] private GameObject scroll3D;
        [SerializeField] private CanvasGroup settingsCvsGroup;
        [SerializeField] private Text screenModeBtnText;
        [SerializeField] private Slider bgmSlider, sfxSlider;
        [SerializeField] private Button bgmToggleBtn, sfxToggleBtn;

        private Sprite _unmuteSprite, _muteSprite;

        private const string SrcPath = "Image/Button";
        public const string SrcNameSoundOn = "/SoundOn";
        public const string SrcNameSoundOff = "/SoundOff";

        #endregion
        
        
        
        #region Callbacks

        public void OnBgmValueChanged()
        {
            BgmManager.Instance.AdjustBgmVolume(bgmSlider.value);
        }

        public void OnSfxValueChanged()
        {
            //todo-Set SfxManager's Sfx volume
        }

        public void OnBgmToggleBtnClick()
        {
            if (BgmManager.Instance.muteType == BgmManager.MuteType.IsMute)
            {
                bgmToggleBtn.GetComponent<Image>().sprite = _unmuteSprite;
                BgmManager.Instance.UnmuteBgm();
            }
            else
            {
                bgmToggleBtn.GetComponent<Image>().sprite = _muteSprite;
                BgmManager.Instance.MuteBgm();
            }
        }

        public void OnSfxToggleBtnClick()
        {
            
        }

        public void OnResolutionBtnClick(int type)
        {
            UIManager.Instance.SetResolution((UIManager.Resolution169) type);
        }

        public void OnScreenModeBtnClick()
        {
            UIManager.Instance.ChangeScreenMode();
            screenModeBtnText.text = UIManager.Instance.isFullScreen ? "Full screen" : "Windowed";
        }

        public void OnSettingBackBtnClick()
        {
            scroll3D.GetComponent<ScrollScript3D>().OpenScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, settingsCvsGroup, UIManager.LobbyMenuFadeInOutDuration);
        }

        #endregion



        #region Custom methods

        private void Init()
        {
            bgmSlider.value = sfxSlider.value = 1f;
            _unmuteSprite = Resources.Load<Sprite>(String.Format("{0}{1}", SrcPath, SrcNameSoundOn));
            _muteSprite = Resources.Load<Sprite>(String.Format("{0}{1}", SrcPath, SrcNameSoundOff));
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