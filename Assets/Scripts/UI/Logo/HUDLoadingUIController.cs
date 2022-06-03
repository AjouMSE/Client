using System.Collections;
using System.Collections.Generic;
using Data.Cache;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace UI.Logo
{
    public class HUDLoadingUIController : MonoBehaviour
    {
        #region Private constants

        private const int MaxFrameRate = 60;
        private const float LoadingFadeInDuration = 5f;
        private const float LoadingFadeOutDuration = 1f;
        private const string HudCamera = "HUDCamera";
        private const string TxtGameTip = "Txt_GameTip";

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
        private GameObject _hudCamera;
        private UnityEngine.SceneManagement.Scene _currentScene;

        private HUDLogoUIController _hudLogoUIController;

        #endregion


        #region Unity event functions

        private void Awake()
        {
            _loadingCanvasGroup = GetComponent<CanvasGroup>();
            _tmProGameTip = CustomUtils.FindComponentByName<TextMeshProUGUI>(gameObject, TxtGameTip);
            _hudCamera = GameObject.FindWithTag(HudCamera);
            _currentScene = SceneManager.GetActiveScene();

            switch (_currentScene.name)
            {
                case UIManager.SceneNameLogo:
                    _hudLogoUIController = _hudCamera.GetComponentInChildren<HUDLogoUIController>();

                    // Set maximum frame rate : 60
                    Application.targetFrameRate = MaxFrameRate;

                    // Set Screen orientation : landscape
                    Screen.orientation = ScreenOrientation.Landscape;

                    // Init Resources
                    InitResources();
                    break;

                case UIManager.SceneNameLogin:
                    break;

                case UIManager.SceneNameLobby:
                    break;

                case UIManager.SceneNameGame:
                    break;
            }

            int tipNum = Random.Range(0, Tips.Length);
            _tmProGameTip.text = Tips[tipNum];
        }

        #endregion


        #region Callbacks

        /// <summary>
        /// Loading ui fade in result callback
        /// </summary>
        private void LoadingFadeInCallback()
        {
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, _loadingCanvasGroup, LoadingFadeOutDuration,
                LoadingFadeOutCallback, false);
        }

        /// <summary>
        /// Loading ui fade out result callback
        /// Play fade effect of logo ui
        /// </summary>
        private void LoadingFadeOutCallback()
        {
            switch (_currentScene.name)
            {
                case UIManager.SceneNameLogo:
                    _hudLogoUIController.ProcessFadeEffect();
                    break;

                case UIManager.SceneNameLogin:
                    UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameLobby);
                    break;

                case UIManager.SceneNameLobby:
                    UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameGame);
                    break;

                case UIManager.SceneNameGame:
                    UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameLobby);
                    break;
            }
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Initialize resources
        /// </summary>
        private void InitResources()
        {
            // Init Resources
            TableLoader.Instance.LoadTableData();
            StartCoroutine(CacheAudioSource.Instance.InitCoroutine());
            StartCoroutine(CacheSpriteSource.Instance.InitCoroutine());
            StartCoroutine(CacheVFXSource.Instance.InitCoroutine());
            StartCoroutine(CacheCoroutineSource.Instance.InitCoroutine());
            StartCoroutine(WaitForInitResources());
        }

        /// <summary>
        ///  Initialize managers
        /// </summary>
        private void InitManagers()
        {
            // Init Managers
            AudioManager.Instance.Init();
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.BGM, 0.05f);
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.SFX, 1.0f);

            UIManager.Instance.Init();
            UIManager.Instance.SetResolution(UIManager.Resolution169.Resolution720);
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, _loadingCanvasGroup, LoadingFadeInDuration,
                LoadingFadeInCallback);

            HttpRequestManager.Instance.Init();
            UserManager.Instance.Init();
        }

        #endregion


        #region Coroutines

        private IEnumerator WaitForInitResources()
        {
            bool result = false;
            while (!result)
            {
                result = CacheAudioSource.IsInitialized && CacheSpriteSource.IsInitialized &&
                         CacheVFXSource.IsInitialized && CacheCoroutineSource.IsInitialized;

                yield return null;
            }

            InitManagers();
        }

        #endregion
    }
}