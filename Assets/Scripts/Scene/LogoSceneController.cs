using System.Collections;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private const float FadeInDuration = 2f;
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

        void Init()
        {
            // Set maximum frame rate : 60
            Application.targetFrameRate = MaxFrameRate;
            
            // Set Screen orientation : landscape
            Screen.orientation = ScreenOrientation.Landscape;
            Screen.fullScreenMode = FullScreenMode.Windowed;

            // Set Bgm to Logo bgm
            BgmManager.Instance.SetBgm(BgmManager.SrcNameLogoBgm);
            BgmManager.Instance.AdjustBgmVolume(1);
            BgmManager.Instance.Play(false);

            // Start fade effect
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, canvasGroup, FadeInDuration, FadeInCallback);
        }
        
        #endregion


        #region Unity event functions
        
        void Start()
        {
            Init();
        }

        #endregion
    }
}