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

        #endregion


        #region Private variables

        [Header("Main Camera Controller")] 
        [SerializeField] private LoginMainCameraController mainCameraController;

        [Header("Title Text")] 
        [SerializeField] private Text titleText;

        [Header("3D Scroll UI")] 
        [SerializeField] private ScrollScript3D signinScroll;

        private CanvasGroup _titleCanvasGroup;

        #endregion


        #region Event methods

        private void Start()
        {
            InitUI();
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, _titleCanvasGroup, FadeInDuration);
        }

        #endregion


        #region Private methdos

        private void InitUI()
        {
            _titleCanvasGroup = GetComponent<CanvasGroup>();
            titleText.text = CustomUtils.MakeTitleColor();
            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.MainBGM1, true);
        }

        #endregion


        #region Callbacks

        public void OnToStartBtnClick()
        {
            mainCameraController.CameraMovementEffect(() =>
            {
                _titleCanvasGroup.gameObject.SetActive(false);
                signinScroll.OpenScroll();
            }, -2, 0.2f);
        }

        #endregion
    }
}