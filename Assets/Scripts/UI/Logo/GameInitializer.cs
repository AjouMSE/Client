using System.Collections;
using System.Collections.Generic;
using Data.Cache;
using Manager;
using UnityEngine;

namespace UI.Logo
{
    public class GameInitializer : MonoBehaviour
    {
        #region Private variables

        [Header("HUD Loading, Logo UI Controller")] 
        [SerializeField] private HUDLoadingUIController loadingUIController;
        [SerializeField] private HUDLogoUIController logoUIController;

        #endregion


        #region Unity event methods

        private void Start()
        {
            // Init resources
            InitResources();
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
                result = CacheSpriteSource.IsInitialized;
                yield return null;
            }

            loadingUIController.gameObject.SetActive(true);
            loadingUIController.ShowLoadingUI();

            result = false;
            while (!result)
            {
                result = CacheAudioSource.IsInitialized && CacheVFXSource.IsInitialized &&
                         CacheCoroutineSource.IsInitialized;

                yield return null;
            }

            InitManagers();


            yield return CacheCoroutineSource.Instance.GetSource(4f);
            loadingUIController.HideLoadingUI(() => { logoUIController.ProcessFadeEffect(); });
        }

        #endregion`
    }
}