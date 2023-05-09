using System;
using System.Collections;
using System.Collections.Generic;
using Cache;
using InGame;
using Manager;
using Manager.InGame;
using Manager.Net;
using UI.Game.CardSelection;
using UI.Game.UserStatus;
using UI.Game.Versus;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;
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

        private const string BattleStartReqPath = "/battle/start";

        #endregion

        #region Private variables

        [Header("Text Information")] 
        [SerializeField] private Text versusText;

        [Header("Game Info object")] 
        [SerializeField] private GameObject panelInfoTextHost;

        [SerializeField] private GameObject panelInfoTextClient;
        [SerializeField] private VersusBattleResultUIController battleResultUIControllerHost;
        [SerializeField] private VersusBattleResultUIController battleResultUIControllerClient;

        [Header("UI Controllers")] [SerializeField]
        private VersusCameraController versusCameraController;

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
            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.BattleBGM1, true);

            // Init Managers, Start Unity Network
            MagicaDuelGameManager.Instance.Init();
            MagicaDuelGameManager.Instance.GameVersusUIController = this;
            PanelManager.Instance.PanelTemplate = panelTemplate;
            PanelManager.Instance.Init();
            NetGameStatusManager.Instance.Init();

            if (UserManager.Instance.IsHost)
                NetworkManager.Singleton.StartHost();
            else
                NetworkManager.Singleton.StartClient();

            // Game start request
            NetHttpRequestManager.Instance.Post(BattleStartReqPath, "", req =>
            {
                using (req)
                {
                    if (req.result == UnityWebRequest.Result.Success)
                    {
                        StartCoroutine(ShowVersus());
                    }
                    else if (req.result == UnityWebRequest.Result.ProtocolError)
                    {
                        // Occured Error (Account does not exist, Wrong password etc..)
                        Debug.Log($"{req.responseCode.ToString()} / {req.error}");
                    }
                    else
                    {
                        // Occured Error (Server connection error)
                        Debug.Log($"{req.responseCode.ToString()} / {req.error}");
                    }
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isHost"></param>
        /// <param name="hostResult"></param>
        /// <param name="clientResult"></param>
        /// <param name="scoreGap"></param>
        public void ShowGameResult(bool isHost, Consts.BattleResult hostResult, Consts.BattleResult clientResult,
            int scoreGap = 0)
        {
            StartCoroutine(ShowGameResultCoroutine(isHost, hostResult, clientResult, scoreGap));
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

                MagicaDuelGameManager.Instance.BeginGame();
            });
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isHost"></param>
        /// <param name="hostResult"></param>
        /// <param name="clientResult"></param>
        /// <param name="scoreGap"></param>
        /// <returns></returns>
        private IEnumerator ShowGameResultCoroutine(bool isHost, Consts.BattleResult hostResult,
            Consts.BattleResult clientResult, int scoreGap)
        {
            versusText.text = "";
            yield return CacheCoroutineSource.Instance.GetSource(2f);

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
            battleResultUIControllerHost.SetBattleResult(isHost, hostResult, clientResult, scoreGap);
            battleResultUIControllerClient.SetBattleResult(isHost, hostResult, clientResult, scoreGap);

            yield return CacheCoroutineSource.Instance.GetSource(4f);

            // Shutdown the host
            if (NetworkManager.Singleton.IsHost)
                NetworkManager.Singleton.Shutdown();

            UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameLobby);
        }

        #endregion
    }
}