using System;
using System.Collections;
using System.Collections.Generic;
using Data.Cache;
using InGame;
using Manager;
using Manager.InGame;
using Manager.Net;
using UI.Game.CardSelection;
using UI.Game.UserStatus;
using UI.Game.Versus;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI.Game.Versus
{
    public class HUDGameVersusUIController : MonoBehaviour
    {
        #region Private constants

        private const int VSTextSize = 125;
        private const int AreYouReadyTextSize = 100;
        private const string TextVS = "VS";
        private const string TextAreYouReady = "Are you\nReady?";
        private const string TextMatchWillBeEnd = "The match will\nend soon.";

        #endregion

        #region Private variables

        [Header("Text Information")] 
        [SerializeField] private Text versusText;

        [Header("Game Info object")] 
        [SerializeField] private GameObject panelInfoTextHost;
        [SerializeField] private GameObject panelInfoTextClient;
        [SerializeField] private VersusBattleResultUIController battleResultUIControllerHost;
        [SerializeField] private VersusBattleResultUIController battleResultUIControllerClient;
        
        [Header("UI Controllers")]
        [SerializeField] private VersusCameraController versusCameraController;
        [SerializeField] private HUDGameUserStatusUIController hudUserInfo;
        [SerializeField] private HUDGameSelectedCardUIController hudCardSelection;

        [Header("Panel Template")]
        [SerializeField] private GameObject panelTemplate;

        #endregion


        #region Unity event methods

        void Start()
        {
            Init();
        }

        #endregion


        #region Private methods

        private void Init()
        {
            // Init Managers, Start Unity Network
            GameManager2.Instance.Init();
            GameManager2.Instance.GameVersusUIController = this;
            PanelManager.Instance.PanelTemplate = panelTemplate;
            PanelManager.Instance.Init();
            NetGameStatusManager.Instance.Init();
            
            if (UserManager.Instance.IsHost)
                NetworkManager.Singleton.StartHost();
            else
                NetworkManager.Singleton.StartClient();

            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.BattleBGM1, true);
            StartCoroutine(ShowVersus());
        }

        public void ShowGameResult(Consts.BattleResult hostResult, Consts.BattleResult clientResult)
        {
            StartCoroutine(ShowGameResultCoroutine(hostResult, clientResult));
        }

        #endregion

        #region Coroutines

        /// <summary>
        /// Shows information of host & client
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowVersus()
        {
            panelInfoTextHost.SetActive(true);
            panelInfoTextClient.SetActive(true);

            battleResultUIControllerHost.gameObject.SetActive(false);
            battleResultUIControllerClient.gameObject.SetActive(false);

            versusText.fontSize = VSTextSize;
            versusText.text = TextVS;
            yield return CacheCoroutineSource.Instance.GetSource(3f);

            versusText.fontSize = AreYouReadyTextSize;
            versusText.text = TextAreYouReady;
            yield return CacheCoroutineSource.Instance.GetSource(3f);

            versusCameraController.RotateCameraEffect(() =>
            {
                hudUserInfo.gameObject.SetActive(true);
                hudCardSelection.gameObject.SetActive(true);
                
                GameManager2.Instance.CheckReadyToRunTimer();
            });
            gameObject.SetActive(false);
        }

        private IEnumerator ShowGameResultCoroutine(Consts.BattleResult hostResult, Consts.BattleResult clientResult)
        {
            yield return CacheCoroutineSource.Instance.GetSource(3f);

            // disable the game hud object
            hudUserInfo.gameObject.SetActive(false);
            hudCardSelection.gameObject.SetActive(false);

            // disable the user info object in versus ui
            panelInfoTextHost.SetActive(false);
            panelInfoTextClient.SetActive(false);

            // enable the battle result object in versus ui
            battleResultUIControllerHost.gameObject.SetActive(true);
            battleResultUIControllerClient.gameObject.SetActive(true);

            // set versus text
            versusText.fontSize = 60;
            versusText.text = TextMatchWillBeEnd;

            // set result
            battleResultUIControllerHost.SetBattleResult(hostResult, clientResult);
            battleResultUIControllerClient.SetBattleResult(hostResult, clientResult);

            yield return CacheCoroutineSource.Instance.GetSource(5f);

            // Shutdown the host
            if (NetworkManager.Singleton.IsHost)
                NetworkManager.Singleton.Shutdown();

            UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameLobby);
        }

        #endregion
    }
}