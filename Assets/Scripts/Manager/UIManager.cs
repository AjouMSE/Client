using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Data.Cache;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Manager
{
    public class UIManager : MonoSingleton<UIManager>
    {
        #region Private constants

        private const int Width1080 = 1920, Height1080 = 1080;
        private const int Width900 = 1600, Height900 = 900;
        private const int Width720 = 1280, Height720 = 720;
        private const float MinFadeValue = 0f, MaxFadeValue = 1f;
        private const float MaxSceneLoadProgress = 0.9f;

        #endregion


        #region Public constants

        public enum Resolution169
        {
            Resolution1080 = 0,
            Resolution900 = 1,
            Resolution720 = 2
        }

        public enum FadeType
        {
            FadeIn,
            FadeOut
        }

        public const float LobbyUIFadeInDuration = 1f;
        public const float LobbyMenuFadeInOutDuration = 0.25f;

        public const string SceneNameLogo = "LogoScene";
        public const string SceneNameLobby = "LobbyScene";
        public const string SceneNameLogin = "LoginScene";
        public const string SceneNameGame = "GameScene";

        #endregion


        #region Public variables

        public bool isFullScreen { get; private set; }

        #endregion


        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void Init()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
            }
        }


        /// <summary>
        /// Fade effect for canvas group
        /// </summary>
        /// <param name="fadeType"></param>
        /// <param name="group"></param>
        /// <param name="duration"></param>
        /// <param name="callback"></param>
        /// <param name="disableAfterFadeOut"></param>
        public void Fade(FadeType fadeType, CanvasGroup group, float duration, Action callback = null,
            bool disableAfterFadeOut = true)
        {
            StartCoroutine(FadeEffectCoroutine(fadeType, group, duration, callback, disableAfterFadeOut));
        }

        /// <summary>
        ///  Set display resolution
        /// </summary>
        /// <param name="type"></param>
        public void SetResolution(Resolution169 type)
        {
            switch (type)
            {
                case Resolution169.Resolution1080:
                    Screen.SetResolution(Width1080, Height1080, isFullScreen);
                    break;

                case Resolution169.Resolution900:
                    Screen.SetResolution(Width900, Height900, isFullScreen);
                    break;

                case Resolution169.Resolution720:
                    Screen.SetResolution(Width720, Height720, isFullScreen);
                    break;

                default:
                    Debug.LogError("UndefinedResolutionTypeException");
                    break;
            }
        }

        /// <summary>
        /// Set screen mode
        /// </summary>
        public void ChangeScreenMode()
        {
            isFullScreen = !isFullScreen;
            Screen.fullScreenMode = isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        }

        /// <summary>
        /// Change scene async
        /// </summary>
        /// <param name="destSceneName"></param>
        public void ChangeSceneAsync(string destSceneName)
        {
            StartCoroutine(ChangeSceneAsyncCoroutine(destSceneName));
        }

        /// <summary>
        /// Show info Text for a while
        /// </summary>
        /// <param name="textInfo"></param>
        /// <param name="info"></param>
        public void ShowInfoText(Text textInfo, string info)
        {
            StopCoroutine(ClearInfoTextCoroutine(textInfo));
            StartCoroutine(ClearInfoTextCoroutine(textInfo));
            textInfo.text = info;
        }

        /// <summary>
        /// Show info TextMeshProUGUI for a while
        /// </summary>
        /// <param name="textInfo"></param>
        /// <param name="info"></param>
        public void ShowInfoTmPro(TextMeshProUGUI textInfo, string info)
        {
            StopCoroutine(ClearInfoTmProCoroutine(textInfo));
            StartCoroutine(ClearInfoTmProCoroutine(textInfo));
            textInfo.text = info;
        }

        #endregion


        #region Private Coroutines

        /// <summary>
        /// Fade effect (Coroutine)
        /// </summary>
        /// <param name="fadeType"></param>
        /// <param name="group"></param>
        /// <param name="duration"></param>
        /// <param name="callback"></param>
        /// <param name="disableAfterFadeOut"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private IEnumerator FadeEffectCoroutine(FadeType fadeType, CanvasGroup group, float duration,
            Action callback = null, bool disableAfterFadeOut = true)
        {
            float gap;

            switch (fadeType)
            {
                case FadeType.FadeIn:
                    // Set active: true
                    group.gameObject.SetActive(true);

                    // Init alpha value
                    group.alpha = MinFadeValue;

                    // Fade effect
                    while (group.alpha < MaxFadeValue)
                    {
                        gap = Time.deltaTime / duration;
                        group.alpha += gap;
                        yield return null;
                    }

                    // Fade effect callback
                    callback?.Invoke();
                    yield break;

                case FadeType.FadeOut:
                    // Init alpha value
                    group.alpha = MaxFadeValue;

                    // Fade effect
                    while (group.alpha > MinFadeValue)
                    {
                        gap = Time.deltaTime / duration;
                        group.alpha -= gap;
                        yield return null;
                    }

                    // Fade effect callback
                    if (disableAfterFadeOut)
                        group.gameObject.SetActive(false);
                    callback?.Invoke();
                    yield break;

                default:
                    throw new Exception("UndefinedFadeTypeException");
            }
        }

        /// <summary>
        /// Change scene async (coroutine)
        /// </summary>
        /// <param name="destSceneName"></param>
        /// <returns></returns>
        private IEnumerator ChangeSceneAsyncCoroutine(string destSceneName)
        {
            var operation = SceneManager.LoadSceneAsync(destSceneName);
            operation.allowSceneActivation = false;
            while (!operation.isDone)
            {
                if (operation.progress >= MaxSceneLoadProgress)
                {
                    operation.allowSceneActivation = true;
                    yield break;
                }

                yield return null;
            }
        }

        /// <summary>
        /// Clear all text in Text
        /// </summary>
        /// <param name="textInfo"></param>
        /// <returns></returns>
        private IEnumerator ClearInfoTextCoroutine(Text textInfo)
        {
            yield return CacheCoroutineSource.Instance.GetSource(2f);
            textInfo.text = "";
        }

        /// <summary>
        /// Clear all text in TextMeshProUGUI
        /// </summary>
        /// <param name="textInfoTmPro"></param>
        /// <returns></returns>
        private IEnumerator ClearInfoTmProCoroutine(TextMeshProUGUI textInfoTmPro)
        {
            yield return CacheCoroutineSource.Instance.GetSource(2f);
            textInfoTmPro.text = "";
        }

        #endregion
    }
}