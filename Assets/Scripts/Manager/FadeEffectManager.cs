using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Manager
{
    public class FadeEffectManager : MonoSingleton<FadeEffectManager>
    {
        #region Private variables

        private const float MinFadeValue = 0f, MaxFadeValue = 1f;

        #endregion
        public enum FadeType
        {
            FadeIn,
            FadeOut
        }
        
        public void Fade(FadeType fadeType, CanvasGroup group, float duration, Action callback = null)
        {
            StartCoroutine(FadeEffect(fadeType, group, duration, callback));
        }
        
        
        #region Coroutines

        IEnumerator FadeEffect(FadeType fadeType, CanvasGroup group, float duration, Action callback = null)
        {
            float gap;
            
            switch (fadeType)
            {
                case FadeType.FadeIn:
                    group.alpha = MinFadeValue;
                    while (group.alpha < MaxFadeValue)
                    {
                        gap = Time.deltaTime / duration;
                        group.alpha += gap;
                        yield return null;
                    }

                    if(callback != null) callback();
                    break;

                case FadeType.FadeOut:
                    group.alpha = MaxFadeValue;
                    while (group.alpha > MinFadeValue)
                    {
                        gap = Time.deltaTime / duration;
                        group.alpha -= gap;
                        yield return null;
                    }

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