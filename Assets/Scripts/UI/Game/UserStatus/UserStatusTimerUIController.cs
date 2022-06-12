using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Manager;
using Manager.InGame;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Game.UserStatus
{
    public class UserStatusTimerUIController : MonoBehaviour
    {
        #region MyRegion

        private const float TextTimerFadeEffectDuration = 0.25f;
        private const float TextNotifyFadeEffectDuration = 0.75f;

        #endregion


        #region Private variables

        [Header("Stop Watch Object")] 
        [SerializeField] private GameObject stopWatchObj;

        [Header("Information texts")] 
        [SerializeField] private Text textTimer;
        [SerializeField] private Text textTurn;
        [SerializeField] private Text textNotify;

        private bool _fadeTextTimer, _fadeTextNotify;

        #endregion


        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        public void HideTimer()
        {
            stopWatchObj.SetActive(false);
            textTimer.gameObject.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowTimer()
        {
            stopWatchObj.SetActive(true);
            textTimer.gameObject.SetActive(true);
        }


        /// <summary>
        /// 
        /// </summary>
        public void HideNotify()
        {
            textNotify.gameObject.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowNotify()
        {
            textNotify.gameObject.SetActive(true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateTimerText()
        {
            var timer = GameManager2.Instance.TimerValue;
            if (timer >= 10)
            {
                textTimer.text = $"{Mathf.Round(timer).ToString(CultureInfo.CurrentCulture)}";
            }
            else if (timer > 0)
            {
                if (!_fadeTextTimer)
                {
                    _fadeTextTimer = true;
                    UIManager.Instance.FadeText(UIManager.FadeType.FadeIn, textTimer, TextTimerFadeEffectDuration,
                        TextTimerFadeInCallback);
                }

                textTimer.text = $"{timer:0.0}";
            }
            else
            {
                _fadeTextTimer = false;
                UIManager.Instance.FadeText(UIManager.FadeType.FadeIn, textTimer, TextTimerFadeEffectDuration);
                UIManager.Instance.SetTextAlpha(textTimer, 1);
                textTimer.text = "0";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateTurnText()
        {
            textTurn.text = $"Turn {GameManager2.Instance.TurnValue.ToString()}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fade"></param>
        public void UpdateNotifyText(string text = null, bool fade = false)
        {
            _fadeTextNotify = fade;
            if (text != null)
                textNotify.text = text;
            if (fade)
                UIManager.Instance.FadeText(UIManager.FadeType.FadeIn, textNotify, TextNotifyFadeEffectDuration,
                    TextNotifyFadeInCallback);
            else
            {
                UIManager.Instance.FadeText(UIManager.FadeType.FadeIn, textNotify, TextNotifyFadeEffectDuration);
                UIManager.Instance.SetTextAlpha(textNotify, 1);
            }
        }

        #endregion


        #region Callbacks

        /// <summary>
        /// 
        /// </summary>
        private void TextTimerFadeInCallback()
        {
            if (_fadeTextTimer)
            {
                UIManager.Instance.FadeText(UIManager.FadeType.FadeOut, textTimer, 
                    TextTimerFadeEffectDuration, TextTimerFadeOutCallback);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void TextTimerFadeOutCallback()
        {
            if (_fadeTextTimer)
                UIManager.Instance.FadeText(UIManager.FadeType.FadeIn, textTimer, 
                    TextTimerFadeEffectDuration, TextTimerFadeInCallback);
        }

        /// <summary>
        /// 
        /// </summary>
        private void TextNotifyFadeInCallback()
        {
            if (_fadeTextNotify)
            {
                UIManager.Instance.FadeText(UIManager.FadeType.FadeOut, textNotify, TextNotifyFadeEffectDuration,
                    TextNotifyFadeOutCallback);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void TextNotifyFadeOutCallback()
        {
            if (_fadeTextNotify)
                UIManager.Instance.FadeText(UIManager.FadeType.FadeIn, textNotify, TextNotifyFadeEffectDuration,
                    TextNotifyFadeInCallback);
        }

        #endregion
    }
}