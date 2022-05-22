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
    /// Last Modified: 2022. 05. 19
    /// </summary>
    public class HUDLogoUIController : MonoBehaviour
    {
        #region Private constants
        
        private const int MaxFrameRate = 60;
        private const float FadeInDuration = 1.5f;
        private const float FadeOutDuration = 1f;
        private const string DestSceneName = "LoginScene";
        
        #endregion
        
        
        #region Private variables
        
        [Header("LogoScene UI Canvas Group")]
        [SerializeField] private CanvasGroup logoCvsGroup;

        #endregion


        #region Callbacks

        private void FadeInCallback()
        {
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, logoCvsGroup, FadeOutDuration, FadeOutCallback);
        }

        private void FadeOutCallback()
        {
            SceneManager.LoadScene(DestSceneName);
        }

        #endregion
        

        #region Custom methods

        private void InitResources()
        {
            CacheAudioSource.Instance.Init();
        }
        
        /// <summary>
        /// Generate & Initialize instance of singleton
        /// </summary>
        private void InitManagers()
        {
            AudioManager.Instance.Init();
        }

        /// <summary>
        /// Initialize basic game settings
        /// </summary>
        private void InitUI()
        {
            // Set maximum frame rate : 60
            Application.targetFrameRate = MaxFrameRate;
            
            // Set Screen orientation : landscape
            Screen.orientation = ScreenOrientation.Landscape;
            UIManager.Instance.SetResolution(UIManager.Resolution169.Resolution720);

            // Set Bgm to Logo bgm
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.BGM, 0.05f);
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.SFX, 1.0f);
            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.LogoBGM, false);

            // Start fade effect
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, logoCvsGroup, FadeInDuration, FadeInCallback);
        }
        
        #endregion


        #region Unity event functions

        private void Awake()
        {
            InitResources();
            InitManagers();
        }
        
        void Start()
        {
            InitUI();
        }

        #endregion
    }
}