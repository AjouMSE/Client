using System;
using System.Collections;
using System.Threading.Tasks;
using Data.Cache;
using Manager;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    /// <summary>
    /// HUDLogoUIController.cs
    /// Author: Lee Hong Jun (github.com/ARC4NE22, hong3883@naver.com)
    /// Last Modified: 2022. 05. 22
    /// Flow)
    /// Fade in loading canvas -> load resources -> Fade out loading canvas
    /// -> Fade in logo canvas -> Fade out logo canvas -> load next scene
    /// </summary>
    public class HUDLogoUIController : MonoBehaviour
    {
        #region Private constants

        private const int MaxFrameRate = 60;
        private const float LoadingFadeInDuration = 4f;
        private const float LoadingFadeOutDuration = 1f;
        private const float LogoFadeInDuration = 1f;
        private const float LogoFadeOutDuration = 1f;
        private const string DestSceneName = "LoginScene";

        #endregion


        #region Private variables

        [Header("LogoScene UI Canvas Group")] 
        [SerializeField] private CanvasGroup loadingCvsGroup;
        [SerializeField] private CanvasGroup logoCvsGroup;

        #endregion


        #region Callbacks

        private void LoadingFadeInCallback()
        {
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, loadingCvsGroup, LoadingFadeOutDuration, LoadingFadeOutCallback);
        }

        private void LoadingFadeOutCallback()
        {
            // Fade in logo canvas
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, logoCvsGroup, LogoFadeInDuration, LogoFadeInCallback);
            
            // Play logo bgm
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.BGM, 0.05f);
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.SFX, 1.0f);
            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.LogoBGM, false);
        }

        private void LogoFadeInCallback()
        {
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, logoCvsGroup, LogoFadeOutDuration, LogoFadeOutCallback);
        }

        private void LogoFadeOutCallback()
        {
            SceneManager.LoadScene(DestSceneName);
        }

        #endregion


        #region Private methods

        private void InitResources()
        {
            // Init Resources
            StartCoroutine(CacheAudioSource.Instance.Init());
            StartCoroutine(CacheSpriteSource.Instance.Init());
            StartCoroutine(CacheVFXSource.Instance.Init());
            
            InitManagers();
        }

        private void InitManagers()
        {
            // Init Managers
            AudioManager.Instance.Init();
        }

        #endregion
        

        #region Unity event functions

        private void Awake()
        {
            // Set maximum frame rate : 60
            Application.targetFrameRate = MaxFrameRate;

            // Set Screen orientation : landscape
            Screen.orientation = ScreenOrientation.Landscape;
            UIManager.Instance.SetResolution(UIManager.Resolution169.Resolution720);

            // Init Resources, Managers
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, loadingCvsGroup, LoadingFadeInDuration, LoadingFadeInCallback);
            InitResources();
        }

        #endregion
    }
}