using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
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

        private const string HudNameVersus = "HUD_Versus";
        private const string HudNameUserInfo = "HUD_UserInfo";
        private const string HudNameCardSelection = "HUD_CardSelection";
        private const string DestSceneName = "LobbyScene";

        private const float MinMainCameraAngle = -150;
        private const float MaxMainCameraAngle = 30;
        private const float RotationScale = 120f;

        #endregion

        #region Private variables

        [Header("Camera")] [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera hudCamera;

        [Header("Text Information")] [SerializeField]
        private Text versusText;

        [SerializeField] private Text[] hostInfoTextArr;
        [SerializeField] private Text[] clientInfoTextArr;

        [Header("Game Info object")] [SerializeField]
        private GameObject hostPanelInfo;

        [SerializeField] private GameObject clientPanelInfo;
        [SerializeField] private GameObject hostBattleResult;
        [SerializeField] private GameObject clientBattleResult;

        [Header("3D Scroll UI")] [SerializeField]
        private GameObject scroll3D;

        private GameObject _hudVersus, _hudUserInfo, _hudCardSelection;
        private float _mainCameraXAngle;

        #endregion


        #region Unity event methods

        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        #endregion


        #region Custom methods

        private void InitVersusInfoText()
        {
            Packet.User host, client;

            if (NetworkManager.Singleton.IsHost)
            {
                host = UserManager.Instance.User;
                client = UserManager.Instance.Hostile;
            }
            else
            {
                host = UserManager.Instance.Hostile;
                client = UserManager.Instance.User;
            }

            hostInfoTextArr[0].text = host.nickname;
            hostInfoTextArr[1].text = host.win.ToString();
            hostInfoTextArr[2].text = host.lose.ToString();
            hostInfoTextArr[3].text = host.draw.ToString();
            hostInfoTextArr[4].text = host.ranking.ToString();

            clientInfoTextArr[0].text = client.nickname;
            clientInfoTextArr[1].text = client.win.ToString();
            clientInfoTextArr[2].text = client.lose.ToString();
            clientInfoTextArr[3].text = client.draw.ToString();
            clientInfoTextArr[4].text = client.ranking.ToString();
        }

        private void Init()
        {
            _mainCameraXAngle = MinMainCameraAngle;

            _hudVersus = hudCamera.transform.Find(HudNameVersus).gameObject;
            _hudUserInfo = hudCamera.transform.Find(HudNameUserInfo).gameObject;
            _hudCardSelection = hudCamera.transform.Find(HudNameCardSelection).gameObject;

            if (UserManager.Instance.IsHost)
                NetworkManager.Singleton.StartHost();
            else
                NetworkManager.Singleton.StartClient();

            InitVersusInfoText();
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
            hostPanelInfo.SetActive(true);
            hostBattleResult.SetActive(false);

            clientPanelInfo.SetActive(true);
            clientBattleResult.SetActive(false);

            versusText.fontSize = 125;
            versusText.text = "VS";
            yield return new WaitForSeconds(3f);

            versusText.fontSize = 100;
            versusText.text = "Are you\nReady?";
            yield return new WaitForSeconds(3f);

            _hudVersus.SetActive(false);
            StartCoroutine(RotateCamera());
        }

        /// <summary>
        /// Rotate main camera to show battle field
        /// </summary>
        /// <returns></returns>
        private IEnumerator RotateCamera()
        {
            // Camera rotation effect
            while (_mainCameraXAngle < MaxMainCameraAngle)
            {
                _mainCameraXAngle += RotationScale * Time.deltaTime;
                mainCamera.transform.localEulerAngles = new Vector3(_mainCameraXAngle, 0, 0);
                yield return null;
            }

            // Set user info, card selection ui activation to true 
            _hudUserInfo.SetActive(true);
            _hudCardSelection.SetActive(true);

            // Init Game information, Card UI
            GetComponent<HUDGameUserInfoUIController>().Init();
            GetComponent<HUDGameCardSelectionUIController>().Init();

            // Checks that both host and client are ready to run timer
            GameManager.Instance.CheckTimerReady();

            GameManager.Instance.GetHostWizardController().PlayRecoveryAnimation();
            GameManager.Instance.GetClientWizardController().PlayRecoveryAnimation();
        }


        private IEnumerator ShowGameResultCoroutine(Consts.BattleResult hostResult, Consts.BattleResult clientResult)
        {
            yield return new WaitForSeconds(3f);

            _hudVersus.SetActive(true);
            _hudUserInfo.SetActive(false);
            _hudCardSelection.SetActive(false);

            hostPanelInfo.SetActive(false);
            hostBattleResult.SetActive(true);

            clientPanelInfo.SetActive(false);
            clientBattleResult.SetActive(true);

            versusText.fontSize = 60;
            versusText.text = "The match will\nend soon.";
            
            var hostResultTexts = hostBattleResult.GetComponentsInChildren<Text>();
            var clientResultTexts = clientBattleResult.GetComponentsInChildren<Text>();

            hostResultTexts[0].text = (hostResult == Consts.BattleResult.WIN) ? "WIN" : (clientResult == Consts.BattleResult.WIN ? "LOSE" : "DRAW");
            hostResultTexts[1].text = (hostResult == Consts.BattleResult.WIN) ? "+10" : (clientResult == Consts.BattleResult.WIN ? "-8" : "+0");
                
            clientResultTexts[0].text = (clientResult == Consts.BattleResult.WIN) ? "WIN" : (hostResult == Consts.BattleResult.WIN ? "LOSE" : "DRAW");
            clientResultTexts[1].text = (clientResult == Consts.BattleResult.WIN) ? "+10" : (hostResult == Consts.BattleResult.WIN ? "-8" : "+0");

            yield return new WaitForSeconds(5f);
            
            if(UserManager.Instance.IsHost)
                NetworkManager.Singleton.Shutdown();
            SceneManager.LoadSceneAsync(DestSceneName);
        }

        #endregion
    }
}