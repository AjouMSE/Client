using System.Collections;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Scene
{
    /// <summary>
    /// LogoSceneController.cs
    /// Author: Lee Hong Jun (Arcane22, hong3883@naver.com)
    /// Version: 1.0.1
    /// Last Modified: 2022. 04. 04
    /// Description: LogoScene's fade effect controller
    /// 1.0.0 - Add Basic LogoScene control script
    /// 1.0.1 - Add BgmManager
    ///       - Change fade effect coroutine to FadeManager
    /// </summary>
    public class LogoSceneController : MonoBehaviour
    {
        #region Private variables
        
        private const int MaxFrameRate = 60;
        private const float FadeInDuration = 1.5f;
        private const float FadeOutDuration = 1f;
        private const string DestSceneName = "LoginScene";

        [SerializeField] private CanvasGroup canvasGroup;

        #endregion


        #region Callbacks

        private void FadeInCallback()
        {
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, canvasGroup, FadeOutDuration, FadeOutCallback);
        }

        private void FadeOutCallback()
        {
            SceneManager.LoadScene(DestSceneName);
        }

        #endregion
        

        #region Custom methods

        private void InitManager()
        {
            AudioManager.Instance.Init();
        }

        private void InitScene()
        {
            // Set maximum frame rate : 60
            Application.targetFrameRate = MaxFrameRate;
            
            // Set Screen orientation : landscape
            Screen.orientation = ScreenOrientation.Landscape;
            Screen.fullScreenMode = FullScreenMode.Windowed;

            // Set Bgm to Logo bgm
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.Bgm, 1);
            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.LogoBgm, false);

            // Start fade effect
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, canvasGroup, FadeInDuration, FadeInCallback);
        }
        
        #endregion


        #region Unity event functions

        private void Awake()
        {
            InitManager();
        }
        
        void Start()
        {
            InitScene();
        }

        #endregion
    }
}