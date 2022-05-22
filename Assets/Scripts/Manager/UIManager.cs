using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Manager
{
    public class UIManager : MonoSingleton<UIManager>
    {
        #region Private variables

        private const int Width1080 = 1920, Height1080 = 1080;
        private const int Width900 = 1600, Height900 = 900;
        private const int Width720 = 1280, Height720 = 720;
        private const float MinFadeValue = 0f, MaxFadeValue = 1f;

        private bool _isFullScreen = false;

        #endregion


        
        #region Public variables

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

        public bool isFullScreen => _isFullScreen;

        public const float LobbyUIFadeInDuration = 1f;
        public const float LobbyMenuFadeInOutDuration = 0.25f;

        #endregion

        
        
        #region Custom methods
        
        public void Fade(FadeType fadeType, CanvasGroup group, float duration, Action callback = null)
        {
            StartCoroutine(FadeEffect(fadeType, group, duration, callback));
        }
        
        public void SetResolution(Resolution169 type)
        {
            switch (type)
            {
                case Resolution169.Resolution1080:
                    Screen.SetResolution(Width1080, Height1080, _isFullScreen);
                    break;
                
                case Resolution169.Resolution900:
                    Screen.SetResolution(Width900, Height900, _isFullScreen);
                    break;
                
                case Resolution169.Resolution720:
                    Screen.SetResolution(Width720, Height720, _isFullScreen);
                    break;
                
                default:
                    Debug.LogError("UndefinedResolutionTypeException");
                    break;
            }
        }

        public void ChangeScreenMode()
        {
            _isFullScreen = !_isFullScreen;
            if (_isFullScreen)
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            else
                Screen.fullScreenMode = FullScreenMode.Windowed;
        }

        #endregion
        
        
        
        #region Coroutines
        
        IEnumerator FadeEffect(FadeType fadeType, CanvasGroup group, float duration, Action callback = null)
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
                    if(callback != null) 
                        callback();
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
                    group.gameObject.SetActive(false);
                    if(callback != null) 
                        callback();
                    yield break;

                default:
                    Debug.LogError("UndefinedFadeTypeException");
                    break;
            }
        }
        
        #endregion
        
    }
}