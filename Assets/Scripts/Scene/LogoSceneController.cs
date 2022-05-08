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
        private const float FadeEffectDuration = 2f;
        private const string DestSceneName = "LoginScene";

        [SerializeField] private CanvasGroup canvasGroup;

        #endregion


        #region Callbacks

        private void FadeInCallback()
        {
            FadeEffectManager.Instance.Fade(FadeEffectManager.FadeType.FadeOut, canvasGroup, FadeEffectDuration, FadeOutCallback);
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

            // Set Bgm to Logo bgm
            BgmManager.Instance.SetBgm(BgmManager.SrcNameLogoBgm);
            BgmManager.Instance.AdjustBgmVolume(-10);
            BgmManager.Instance.Play();
            
            // Start fade effect
            FadeEffectManager.Instance.Fade(FadeEffectManager.FadeType.FadeIn, canvasGroup, FadeEffectDuration, FadeInCallback);
        }
        
        #endregion


        #region Unity event functions
        
        void Awake()
        {
            Init();
        }

        #endregion
    }
}