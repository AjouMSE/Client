using System;
using System.Collections;
using System.Collections.Generic;
using Data.Cache;
using Manager;
using TMPro;
using UI.Login;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace UI.Logo
{
    public class HUDLoadingUIController : MonoBehaviour
    {
        #region Private constants

        private const int MaxFrameRate = 60;
        private const float LoadingFadeInDuration = 4f;
        private const float LoadingFadeOutDuration = 1f;

        private const string HudCamera = "HUDCamera";
        private const string UINameTxtGameTip = "Txt_GameTip";
        private const string UINameLoadingImage = "Img_Loading";

        private static string[] Tips { get; } =
        {
            "Tip. Try combining movement cards and attack cards appropriately.",
            "Tip. Use your movement cards appropriately to gain an advantage.",
            "Tip. Proper mana management is also a skill.",
            "Tip. Achieve the top ranking by winning matches against opponents",
            "Tip. Sometimes a powerful skill can turn things around.",
            "Tip. Fireball magic is very hot, so use it with care.",
            "Tip. In the hot summer, you can use ice magic to make ice cream.",
            "Tip. Do you want something tingly? There is lightning magic!",
            "Tip. The moment you are certain of victory, a crisis can come.",
            "Tip. Cards in an invalid range cannot be selected."
        };

        #endregion


        #region Private variables

        private CanvasGroup _loadingCanvasGroup;
        private TextMeshProUGUI _tmProGameTip;
        private Image _loadingIconImage;
        private GameObject _hudCamera;

        private HUDLogoUIController _hudLogoUIController;
        private HUDLoginSigninUIController _hudLoginSigninUIController;

        #endregion


        #region Unity event functions

        private void Awake()
        {
            Init();
            gameObject.SetActive(false);
        }

        #endregion


        #region Private methods

        private void Init()
        {
            _loadingCanvasGroup = GetComponent<CanvasGroup>();
            _tmProGameTip = CustomUtils.FindComponentByName<TextMeshProUGUI>(gameObject, UINameTxtGameTip);
            _loadingIconImage = CustomUtils.FindComponentByName<Image>(gameObject, UINameLoadingImage);
            _hudCamera = GameObject.FindWithTag(HudCamera);
            
            var currentScene = SceneManager.GetActiveScene();
            switch (currentScene.name)
            {
                case UIManager.SceneNameLogo:
                    _hudLogoUIController = _hudCamera.GetComponentInChildren<HUDLogoUIController>();

                    // Set maximum frame rate : 60
                    Application.targetFrameRate = MaxFrameRate;
                    UIManager.Instance.SetResolution(1920, 1080);

                    // Set Screen orientation : landscape
                    Screen.orientation = ScreenOrientation.Landscape;
                    break;

                case UIManager.SceneNameLogin:
                    _hudLoginSigninUIController = _hudCamera.GetComponentInChildren<HUDLoginSigninUIController>();
                    break;

                case UIManager.SceneNameLobby:
                    break;

                case UIManager.SceneNameGame:
                    break;
            }
        }

        #endregion


        #region Public methods

        public void ShowLoadingUI(Action fadeInCallback = null)
        {
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, _loadingCanvasGroup, LoadingFadeInDuration,
                fadeInCallback);
            
            var tipNum = Random.Range(0, Tips.Length);
            var uiNum = Random.Range(100, 105);
            
            _tmProGameTip.text = Tips[tipNum];
            _loadingIconImage.sprite = CacheSpriteSource.Instance.GetSource(uiNum);
        }

        public void HideLoadingUI(Action fadeOutCallback = null, bool disableAfterFadeOut = true)
        {
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, _loadingCanvasGroup, LoadingFadeOutDuration,
                fadeOutCallback, disableAfterFadeOut);
        }

        #endregion
    }
}