using System.Collections;
using System.Collections.Generic;
using System.Text;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Login
{
    public class HUDLoginTitleUIController : MonoBehaviour
    {
        #region Private constants
        
        private const float FadeInDuration = 1.5f, FadeOutDuration = 0.5f;
        private const float CameraMovementEffectSpd = 12f;

        #endregion


        #region Private variables

        [Header("Main Camera Controller")] 
        [SerializeField] private LoginMainCameraController mainCameraController;

        [Header("Title Text")] 
        [SerializeField] private Text titleText;

        [Header("Exit Game Display")] 
        [SerializeField] private GameObject exitGameDisplay;

        [Header("3D Scroll UI")] 
        [SerializeField] private ScrollScript3D signinScroll;

        private CanvasGroup _titleCanvasGroup;

        #endregion


        #region Event methods

        private void Start()
        {
            Init();
        }

        #endregion


        #region Private methdos

        private void Init()
        {
            _titleCanvasGroup = GetComponent<CanvasGroup>();
            titleText.text = CustomUtils.MakeTitleColor();
            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.MainBGM1, true);
            UIManager.Instance.SetExitGameDisplay(exitGameDisplay);
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, _titleCanvasGroup, FadeInDuration);
        }

        #endregion


        #region Callbacks

        /// <summary>
        /// 
        /// </summary>
        public void OnStartBtnClick()
        {
            mainCameraController.CameraMovementEffect(() =>
            {
                signinScroll.OpenScroll();
            }, -2, CameraMovementEffectSpd);
            gameObject.SetActive(false);
        }

        #endregion
    }
}