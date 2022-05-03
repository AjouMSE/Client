using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class FadeEffectController : MonoBehaviour
    {
        public enum FadeType
        {
            FadeIn = 0,
            FadeOut = 1
        }
        
        public void Fade(Action callback)
        {
            StartCoroutine(FadeEffect(callback));
        }
        
        
        #region Coroutines

        IEnumerator FadeEffect(Action callback)
        {
            yield return null;
        }

        #endregion
    }
}