using System;
using System.Collections;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace UI.Logo
{
    public class HUDLogoUIController : MonoBehaviour
    {
        #region Private constants

        private const float LogoFadeInDuration = 1f;
        private const float LogoFadeOutDuration = 1f;

        #endregion


        #region Private variables

        private CanvasGroup _logoCanvasGroup;

        #endregion


        #region Unity event methods

        private void Awake()
        {
            _logoCanvasGroup = GetComponent<CanvasGroup>();
        }

        #endregion


        #region Callbacks

        /// <summary>
        /// Logo ui fade in result callback
        /// </summary>
        private void LogoFadeInCallback()
        {
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, _logoCanvasGroup, LogoFadeOutDuration,
                LogoFadeOutCallback, false);
        }

        /// <summary>
        /// Logo ui fade out result callback
        /// </summary>
        private void LogoFadeOutCallback()
        {
            UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameLogin);
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Process fade effect of logo UI
        /// Loading UI -> Logo UI
        /// </summary>
        public void ProcessFadeEffect()
        {
            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.LogoBGM, false);
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, _logoCanvasGroup, LogoFadeInDuration,
                LogoFadeInCallback);
        }

        #endregion
    }
}