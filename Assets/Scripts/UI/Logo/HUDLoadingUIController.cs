using System.Collections;
using System.Collections.Generic;
using Data.Cache;
using Manager;
using UnityEngine;

namespace UI.Logo
{
    public class HUDLoadingUIController : MonoBehaviour
    {
        #region Private constants

        private const int MaxFrameRate = 60;
        private const float LoadingFadeInDuration = 4f;
        private const float LoadingFadeOutDuration = 1f;
        private const string HudCamera = "HUDCamera";

        #endregion


        #region Private variables

        private CanvasGroup _loadingCanvasGroup;
        private HUDLogoUIController _hudLogoUIController;

        #endregion


        #region Unity event functions

        private void Awake()
        {
            _loadingCanvasGroup = GetComponent<CanvasGroup>();
            _hudLogoUIController = GameObject.FindWithTag(HudCamera).GetComponentInChildren<HUDLogoUIController>();

            // Set maximum frame rate : 60
            Application.targetFrameRate = MaxFrameRate;

            // Set Screen orientation : landscape
            Screen.orientation = ScreenOrientation.Landscape;

            // Init Resources
            InitResources();
        }

        #endregion


        #region Callbacks

        /// <summary>
        /// Loading ui fade in result callback
        /// </summary>
        private void LoadingFadeInCallback()
        {
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, _loadingCanvasGroup, LoadingFadeOutDuration,
                LoadingFadeOutCallback);
        }

        /// <summary>
        /// Loading ui fade out result callback
        /// Play fade effect of logo ui
        /// </summary>
        private void LoadingFadeOutCallback()
        {
            _hudLogoUIController.ProcessFadeEffect();
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Initialize resources
        /// </summary>
        private void InitResources()
        {
            // Init Resources
            TableLoader.Instance.LoadTableData();
            StartCoroutine(CacheAudioSource.Instance.InitCoroutine());
            StartCoroutine(CacheSpriteSource.Instance.InitCoroutine());
            StartCoroutine(CacheVFXSource.Instance.InitCoroutine());
            StartCoroutine(CacheCoroutineSource.Instance.InitCoroutine());
            StartCoroutine(WaitForInitResources());
        }

        /// <summary>
        ///  Initialize managers
        /// </summary>
        private void InitManagers()
        {
            // Init Managers
            AudioManager.Instance.Init();
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.BGM, 0.05f);
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.SFX, 1.0f);

            UIManager.Instance.Init();
            UIManager.Instance.SetResolution(UIManager.Resolution169.Resolution720);
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, _loadingCanvasGroup, LoadingFadeInDuration,
                LoadingFadeInCallback);
            
            HttpRequestManager.Instance.Init();
        }

        #endregion


        #region Coroutines

        private IEnumerator WaitForInitResources()
        {
            bool result = false;
            while (!result)
            {
                result = CacheAudioSource.IsInitialized && CacheSpriteSource.IsInitialized &&
                         CacheVFXSource.IsInitialized && CacheCoroutineSource.IsInitialized;

                yield return null;
            }

            InitManagers();
        }

        #endregion
    }
}