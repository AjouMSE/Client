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

        private const int FhdWidth = 1920, FhdHeight = 1080;
        private const int HdWidth = 1280, HdHeight = 720;
        private const float MinFadeValue = 0f, MaxFadeValue = 1f;

        private bool _isFullScreen = false;

        #endregion


        
        #region Public variables

        public enum Resolution
        {
            Fhd,
            Hd
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
        
        public void SetResolution(Resolution res)
        {
            switch (res)
            {
                case Resolution.Fhd:
                    Screen.SetResolution(FhdWidth, FhdHeight, _isFullScreen);
                    break;
                
                case Resolution.Hd:
                    Screen.SetResolution(HdWidth, HdHeight, _isFullScreen);
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
                    if(callback != null) callback();
                    break;

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
                    if(callback != null) callback();
                    break;

                default:
                    Debug.LogError("UndefinedFadeTypeException");
                    break;
            }
        }
        
        #endregion
        
    }
}