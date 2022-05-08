using System.Collections;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    /// <summary>
    /// LogoSceneController.cs
    /// Author: Lee Hong Jun (Arcane22, hong3883@naver.com)
    /// Version: 1.0
    /// Last Modified: 2022. 04. 04
    /// Description: LogoScene's fade effect controller
    /// </summary>
    public class LogoSceneController : MonoBehaviour
    {
        #region Private variables

        private enum FadeTypes
        {
            FadeIn = 0,
            FadeOut = 1,
        }

        private const float MinAlphaValue = 0f, MaxAlphaValue = 1f;
        private const float MaxFadeTime = 1f;
        private const int MaxFrameRate = 60;

        [SerializeField] private CanvasGroup canvasGroup;

        #endregion

        #region Init, Unity event methods

        void Init()
        {
            // Set maximum frame rate : 60
            Application.targetFrameRate = MaxFrameRate;
            
            // Set Screen orientation : landscape
            Screen.orientation = ScreenOrientation.Landscape;
            
            // Set canvas group's alpha value to 0
            canvasGroup.alpha = MinAlphaValue;
            
            // Set Bgm to Logo bgm
            BgmManager.Instance.SetBgm(BgmManager.SrcNameLogoBgm);
            BgmManager.Instance.AdjustBgmVolume(-10);
            BgmManager.Instance.Play();
            SfxManager.Instance.ToString();


            // Start fade effect
            StartCoroutine(FadeEffect((int)FadeTypes.FadeIn));
        }

        void Awake()
        {
            Init();
        }

        #endregion


        #region Fade Effect coroutine

        IEnumerator FadeEffect(int fadeType)
        {
            float gap = Time.deltaTime / MaxFadeTime;

            switch (fadeType)
            {
                case (int)FadeTypes.FadeIn:
                    while (canvasGroup.alpha < MaxAlphaValue)
                    {
                        canvasGroup.alpha += gap;
                        yield return new WaitForSeconds(Time.deltaTime);
                    }

                    StartCoroutine(FadeEffect((int)FadeTypes.FadeOut));
                    break;

                case (int)FadeTypes.FadeOut:
                    while (canvasGroup.alpha > MinAlphaValue)
                    {
                        canvasGroup.alpha -= gap;
                        yield return new WaitForSeconds(Time.deltaTime);
                    }

                    SceneManager.LoadScene("LoginScene");
                    break;

                default:
                    Debug.LogError("UndefinedFadeTypeError");
                    break;
            }
        }

        #endregion
    }
}