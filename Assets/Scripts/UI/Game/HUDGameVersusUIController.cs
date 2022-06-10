using System;
using System.Collections;
using System.Collections.Generic;
using Data.Cache;
using InGame;
using Manager;
using Manager.InGame;
using Manager.Net;
using UI.Game.Versus;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI.Game
{
    public class HUDGameVersusUIController : MonoBehaviour
    {
        #region Private constants
        
        private const int VSTextSize = 125;
        private const int AreYouReadyTextSize = 100;

        #endregion

        #region Private variables

        [Header("Text Information")] 
        [SerializeField] private Text versusText;

        [Header("Game Info object")] 
        [SerializeField] private GameObject hostPanelInfo;
        [SerializeField] private GameObject clientPanelInfo;

        [SerializeField] private GameObject hostBattleResult;
        [SerializeField] private GameObject clientBattleResult;

        private VersusCameraController _versusCameraController;
        private HUDGameUserInfoUIController _hudUserInfo;
        private HUDGameCardSelectionUIController _hudCardSelection;

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
            var mainCamera = GameObject.FindWithTag("MainCamera");
            var hudCamera = GameObject.FindWithTag("HUDCamera");

            _versusCameraController = mainCamera.GetComponent<VersusCameraController>();
            _hudUserInfo = hudCamera.GetComponentInChildren<HUDGameUserInfoUIController>();
            _hudCardSelection = hudCamera.GetComponentInChildren<HUDGameCardSelectionUIController>();

            // Start Unity Network
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
            GameManager.Instance.Init();
            NetGameStatusManager.Instance.Init();

            hostPanelInfo.SetActive(true);
            hostBattleResult.SetActive(false);

            clientPanelInfo.SetActive(true);
            clientBattleResult.SetActive(false);

            versusText.fontSize = VSTextSize;
            versusText.text = "VS";
            yield return CacheCoroutineSource.Instance.GetSource(3f);

            versusText.fontSize = AreYouReadyTextSize;
            versusText.text = "Are you\nReady?";
            yield return CacheCoroutineSource.Instance.GetSource(3f);

            _versusCameraController.RotateCameraEffect(() =>
            {
                _hudUserInfo.gameObject.SetActive(true);
                _hudCardSelection.gameObject.SetActive(true);
                
                GameManager2.Instance.HostController.PlayAnimation(WizardAnimations.Recovery);
                GameManager2.Instance.ClientController.PlayAnimation(WizardAnimations.Recovery);
                GameManager2.Instance.CheckReadyToRunTimer();
            });
            gameObject.SetActive(false);
        }

        private IEnumerator ShowGameResultCoroutine(Consts.BattleResult hostResult, Consts.BattleResult clientResult)
        {
            yield return new WaitForSeconds(3f);

            _hudUserInfo.gameObject.SetActive(false);
            _hudCardSelection.gameObject.SetActive(false);

            hostPanelInfo.SetActive(false);
            hostBattleResult.SetActive(true);

            clientPanelInfo.SetActive(false);
            clientBattleResult.SetActive(true);

            versusText.fontSize = 60;
            versusText.text = "The match will\nend soon.";

            var hostResultTexts = hostBattleResult.GetComponentsInChildren<Text>();
            var clientResultTexts = clientBattleResult.GetComponentsInChildren<Text>();

            hostResultTexts[0].text = (hostResult == Consts.BattleResult.WIN)
                ? "WIN"
                : (clientResult == Consts.BattleResult.WIN ? "LOSE" : "DRAW");
            hostResultTexts[1].text = (hostResult == Consts.BattleResult.WIN)
                ? "+10"
                : (clientResult == Consts.BattleResult.WIN ? "-8" : "+0");

            clientResultTexts[0].text = (clientResult == Consts.BattleResult.WIN)
                ? "WIN"
                : (hostResult == Consts.BattleResult.WIN ? "LOSE" : "DRAW");
            clientResultTexts[1].text = (clientResult == Consts.BattleResult.WIN)
                ? "+10"
                : (hostResult == Consts.BattleResult.WIN ? "-8" : "+0");

            yield return new WaitForSeconds(5f);

            if (UserManager.Instance.IsHost)
                NetworkManager.Singleton.Shutdown();

            UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameLobby);
        }

        #endregion
    }
}