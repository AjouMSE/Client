using System.Collections;
using System.Collections.Generic;
using Cache;
using DataTable;
using Manager;
using Manager.Net;
using UnityEngine;

namespace UI.Logo
{
    public class GameInitializer : MonoBehaviour
    {
        #region Private variables

        [Header("HUD Loading, Logo UI Controller")] [SerializeField]
        private HUDLoadingUIController loadingUIController;

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
            StartCoroutine(WaitForInitTables());
        }

        /// <summary>
        ///  Initialize managers
        /// </summary>
        private void InitManagers()
        {
            // Init Managers
            UIManager.Instance.Init();
            // UIManager.Instance.ChangeScreenMode();
            // UIManager.Instance.SetResolution(1280, 720);

            AudioManager.Instance.Init();
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.BGM, 0.05f);
            AudioManager.Instance.SetVolume(AudioManager.VolumeTypes.SFX, 1.0f);

            // Init Net, User Managers
            NetHttpRequestManager.Instance.Init();
            NetMatchMakingManager.Instance.Init();
            UserManager.Instance.Init();
        }

        #endregion


        #region Coroutines

        private IEnumerator WaitForInitTables()
        {
            // Init Tables
            TableLoader.Instance.LoadTableData();

            // Wait for init tables
            while (!TableLoader.Instance.IsLoaded)
            {
                yield return null;
            }

            // Init Caches coroutine
            StartCoroutine(WaitForInitResources());
        }

        private IEnumerator WaitForInitResources()
        {
            // Init Caches
            StartCoroutine(CacheAudioSource.Instance.InitCoroutine());
            StartCoroutine(CacheSpriteSource.Instance.InitCoroutine());
            StartCoroutine(CacheVFXSource.Instance.InitCoroutine());
            StartCoroutine(CacheEmojiSource.Instance.InitCoroutine());
            StartCoroutine(CacheCoroutineSource.Instance.InitCoroutine());

            // wait for init sprite sources
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
                         CacheEmojiSource.IsInitialized && CacheCoroutineSource.IsInitialized;

                yield return null;
            }

            InitManagers();


            yield return CacheCoroutineSource.Instance.GetSource(4f);
            loadingUIController.HideLoadingUI(() => { logoUIController.ProcessFadeEffect(); });
        }

        #endregion`
    }
}