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

        private const float MinFadeValue = 0f, MaxFadeValue = 1f;
        private const float MaxSceneLoadProgress = 0.9f;
        private const int MaxResolutionCount = 4;

        #endregion


        #region Public constants

        public enum FadeType
        {
            FadeIn,
            FadeOut
        }

        public const float LobbyUIFadeInDuration = 1f;
        public const float LobbyMenuFadeInOutDuration = 0.2f;

        public const string SceneNameLogo = "LogoScene";
        public const string SceneNameLobby = "LobbyScene";
        public const string SceneNameLogin = "LoginScene";
        public const string SceneNameGame = "GameScene";
        public const string SceneNameGameRemaster = "GameSceneRemaster";

        #endregion


        #region Private variables

        private GameObject _performanceDisplay;
        private GameObject _signOutDisplay;
        private GameObject _exitGameDisplay;

        #endregion


        #region Public variables

        public List<int> SupportedFrameRates { get; private set; }
        public List<Vector2> SupportedResolutions { get; private set; }
        public int MaxWidth { get; private set; }
        public int MaxHeight { get; private set; }
        public int MaxFrameRate { get; private set; }
        public float ResolutionRatio { get; private set; }
        public bool IsFullScreen { get; private set; } = false;

        public bool ActivePerformanceDisplay { get; private set; }

        #endregion


        #region Unity event methods

        public void Update()
        {
            if (_exitGameDisplay != null && Input.GetKeyDown(KeyCode.Escape))
            {
                if (_exitGameDisplay.activeInHierarchy)
                    HideExitGameDisplay();
                else
                    ShowExitGameDisplay();
            }
        }

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
                Resolution[] resolutions = Screen.resolutions;
                SupportedResolutions = new List<Vector2>();
                SupportedFrameRates = new List<int>();

                // Init supported frame rates
                var defaultWidth = resolutions[0].width;
                var defaultHeight = resolutions[0].height;
                SupportedFrameRates.Add(resolutions[0].refreshRate);
                for (int i = 1; i < resolutions.Length; i++)
                {
                    if (resolutions[i].width == defaultWidth && resolutions[i].height == defaultHeight)
                        SupportedFrameRates.Add(resolutions[i].refreshRate);
                    else
                    {
                        MaxFrameRate = resolutions[i - 1].refreshRate;
                        break;
                    }
                }

                // Init supported resolutions
                MaxWidth = resolutions[resolutions.Length - 1].width;
                MaxHeight = resolutions[resolutions.Length - 1].height;
                ResolutionRatio = MaxWidth / (float)MaxHeight;
                SupportedResolutions.Add(new Vector2(MaxWidth, MaxHeight));

                var newWidth = MaxWidth;
                var newHeight = MaxHeight;
                for (int i = resolutions.Length - 2; i >= 0; i--)
                {
                    if (resolutions[i].width == newWidth && resolutions[i].height == newHeight)
                        continue;

                    var ratioGap = Math.Abs((resolutions[i].width / (float)resolutions[i].height) - ResolutionRatio);
                    if (ratioGap < 0.0001f)
                    {
                        SupportedResolutions.Add(new Vector2(resolutions[i].width, resolutions[i].height));
                        newWidth = resolutions[i].width;
                        newHeight = resolutions[i].height;
                    }

                    if (SupportedResolutions.Count == MaxResolutionCount)
                        break;
                }

                IsInitialized = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        public void SetPerformanceDisplay(GameObject display)
        {
            _performanceDisplay = display;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        public void SetSignOutDisplay(GameObject display)
        {
            _signOutDisplay = display;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="display"></param>
        public void SetExitGameDisplay(GameObject display)
        {
            _exitGameDisplay = display;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowPerformanceDisplay()
        {
            ActivePerformanceDisplay = true;
            _performanceDisplay.SetActive(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowSignOutDisplay()
        {
            _signOutDisplay.SetActive(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowExitGameDisplay()
        {
            _exitGameDisplay.SetActive(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void HidePerformanceDisplay()
        {
            ActivePerformanceDisplay = false;
            _performanceDisplay.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void HideSignOutDisplay()
        {
            _signOutDisplay.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void HideExitGameDisplay()
        {
            _exitGameDisplay.SetActive(false);
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
        /// 
        /// </summary>
        /// <param name="fadeType"></param>
        /// <param name="text"></param>
        /// <param name="duration"></param>
        /// <param name="callback"></param>
        public void FadeText(FadeType fadeType, Text text, float duration, Action callback = null)
        {
            StartCoroutine(TextFadeEffectCoroutine(fadeType, text, duration, callback));
        }


        /// <summary>
        /// Set display resolution
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetResolution(int width, int height)
        {
            Screen.SetResolution(width, height, IsFullScreen);
        }

        /// <summary>
        /// Set screen mode
        /// </summary>
        public void ChangeScreenMode()
        {
            IsFullScreen = !IsFullScreen;
            Screen.fullScreenMode = IsFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="a"></param>
        public void SetTextAlpha(Text text, float a)
        {
            var c = text.color;
            c.a = Mathf.Clamp(a, 0, 1);
            text.color = c;
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
        /// 
        /// </summary>
        /// <param name="fadeType"></param>
        /// <param name="text"></param>
        /// <param name="duration"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private IEnumerator TextFadeEffectCoroutine(FadeType fadeType, Text text, float duration,
            Action callback = null)
        {
            float gap;
            switch (fadeType)
            {
                case FadeType.FadeIn:
                    // Init alpha value
                    SetTextAlpha(text, MinFadeValue);

                    // Fade effect
                    while (text.color.a < MaxFadeValue)
                    {
                        if (text == null) yield break;
                        
                        gap = Time.deltaTime / duration;
                        SetTextAlpha(text, text.color.a + gap);
                        yield return null;
                    }

                    callback?.Invoke();
                    yield break;

                case FadeType.FadeOut:
                    // Init alpha value
                    SetTextAlpha(text, MaxFadeValue);

                    // Fade effect
                    while (text.color.a > MinFadeValue)
                    {
                        if (text == null) yield break;
                        
                        gap = Time.deltaTime / duration;
                        SetTextAlpha(text, text.color.a - gap);
                        yield return null;
                    }

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