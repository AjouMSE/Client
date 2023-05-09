using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Core;
using InGame;
using Manager.Net;
using UI.Game;
using UI.Game.UserStatus;
using UI.Game.Versus;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using UnityEngine.Networking;

namespace Manager
{
    [Obsolete]
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Private constants

        private const int DefaultTurnValue = 0;
        private const float DefaultTimerValue = 60;

        #endregion


        #region Private variables

        private NetworkSynchronizer _netSync;
        private PanelController _panelController;
        private WizardController _hostWizardController, _clientWizardController;
        private HUDGameVersusUIController _gameVersusUIController;
        private HUDGameUserInfoUIController _userInfoUIController;
        private HUDGameCardSelectionUIController _cardSelectionUIController;

        private int _turnValue;
        private float _timerValue;
        private bool _canSelect;

        #endregion


        #region Public variables

        public int turnValue => _turnValue;
        public float timerValue => _timerValue;
        public bool canSelect => _canSelect;

        #endregion


        #region Custom methods

        public override void Init()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
            
                _netSync = GameObject.Find("NetworkSynchronizer").GetComponent<NetworkSynchronizer>();
                _panelController = GameObject.Find("GameSceneObjectController").GetComponent<PanelController>();

                GameObject uiController = GameObject.Find("GameSceneUIController");
                _gameVersusUIController = uiController.GetComponent<HUDGameVersusUIController>();
                _userInfoUIController = uiController.GetComponent<HUDGameUserInfoUIController>();
                _cardSelectionUIController = uiController.GetComponent<HUDGameCardSelectionUIController>();

                _turnValue = DefaultTurnValue;
                _timerValue = DefaultTimerValue;
                _canSelect = false;
            }
        }

        public void CheckTimerReady()
        {
            StartCoroutine(WaitForRunningTimer());
        }

        public void StopTimer()
        {
            _timerValue = 0;
            _userInfoUIController.UpdateTimerText();
        }

        private BitMask.BitField30 CvtPlayerIdxToBitField30(int idx)
        {
            if (idx < 0 || idx > 29) return default;
            return new BitMask.BitField30(BitMask.BitField30Msb >> idx);
        }

        private bool CheckPlayerHit(int idx, BitMask.BitField30 range)
        {
            return (CvtPlayerIdxToBitField30(idx).element & range.element) > 0 ? true : false;
        }

        public WizardController GetHostWizardController()
        {
            return _hostWizardController;
        }

        public void SetHostWizardController(WizardController wizardController)
        {
            _hostWizardController = wizardController;
        }

        public WizardController GetClientWizardController()
        {
            return _clientWizardController;
        }

        public void SetClientWizardController(WizardController wizardController)
        {
            _clientWizardController = wizardController;
        }

        private bool ValidGameOver()
        {
            return _netSync.GetHostHP() <= 0 || _netSync.GetClientHP() <= 0;
        }

        private void GameOver()
        {
            var hostHp = _netSync.GetHostHP();
            var clientHp = _netSync.GetClientHP();
            var hostResult = hostHp > 0 ? Consts.BattleResult.WIN : Consts.BattleResult.LOSE;
            var clientResult = clientHp > 0 ? Consts.BattleResult.WIN : Consts.BattleResult.LOSE;
            var resultPacket = new Packet.BattleResult();
            
            if (UserManager.Instance.IsHost)
            {
                resultPacket.result = hostHp > 0 ? "WIN" : (clientHp > 0 ? "LOSE" : "DRAW");
                _hostWizardController.BattleResultAction(hostResult);
                _clientWizardController.BattleResultAction(clientResult);
            }
            else
            {
                resultPacket.result = clientHp > 0 ? "WIN" : (hostHp > 0 ? "LOSE" : "DRAW");
            }


            NetHttpRequestManager.Instance.Post("/battle/result", JsonUtility.ToJson(resultPacket), req =>
            {
                if (req.result == UnityWebRequest.Result.Success)
                {
                    // Save user information to UserManager
                    string json = req.downloadHandler.text;
                    UserManager.Instance.UpdateUserInfo(json);
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
            });
            
            _gameVersusUIController.ShowGameResult(UserManager.Instance.IsHost, hostResult, clientResult);
        }

        public List<int> GetInvalidCards()
        {
            WizardController wizardController = null;
            if (UserManager.Instance.IsHost)
                wizardController = _hostWizardController;
            else
                wizardController = _clientWizardController;

            return wizardController.InvalidCards();
        }

        #endregion


        #region Unity event functions

        #endregion


        #region Coroutines

        /// <summary>
        /// Wait for running timer
        /// Checks if both host and client are ready to run timer
        /// </summary>
        private IEnumerator WaitForRunningTimer()
        {
            _netSync.ReadyToRunTimer(true);
            _netSync.ReadyToProcessCards(false);

            while (!_netSync.BothReadyToRunTimer())
            {
                yield return null;
            }

            StartCoroutine(RunTimer());
        }

        /// <summary>
        /// Run card selection timer
        /// </summary>
        /// <returns></returns>
        private IEnumerator RunTimer()
        {
            _turnValue++;
            _timerValue = DefaultTimerValue;
            _canSelect = true;

            _userInfoUIController.UpdateTimerText();
            _userInfoUIController.UpdateTurnText();
            _cardSelectionUIController.UpdateInvalidCards();
            _cardSelectionUIController.OpenCardScroll();

            while (_timerValue > 0)
            {
                _timerValue -= Time.deltaTime;
                _userInfoUIController.UpdateTimerText();
                yield return null;
            }

            StartCoroutine(WaitForProcessingCards());
        }

        /// <summary>
        /// Wait for processing cards
        /// Checks if both host and client are ready to process cards in card list
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForProcessingCards()
        {
            _netSync.ReadyToRunTimer(false);
            _netSync.ReadyToProcessCards(true);

            _timerValue = 0;
            _canSelect = false;

            _userInfoUIController.UpdateTimerText();
            _cardSelectionUIController.CloseCardScroll();

            while (!_netSync.BothReadyToProcessCards())
            {
                yield return null;
            }

            StartCoroutine(ProcessCards());
        }

        /// <summary>
        /// Process cards in card list
        /// </summary>
        /// <returns></returns>
        private IEnumerator ProcessCards()
        {
            int cardId = 0;

            int[] hostCards = _netSync.GetCopyList(Consts.UserType.Host);
            int[] clientCards = _netSync.GetCopyList(Consts.UserType.Client);

            for (int i = 0; i < 3; i++)
            {
                // Process card id
                if (hostCards.Length > i)
                {
                    cardId = hostCards[i];

                    _panelController.ProcessEffect(cardId, Consts.UserType.Host, _hostWizardController,
                        _clientWizardController);
                    _hostWizardController.ProcessSkill(cardId);

                    yield return new WaitForSeconds(3f);

                    if (UserManager.Instance.IsHost)
                        _netSync.RemoveFrontOfList(Consts.UserType.Host);
                }

                // Process card id
                if (clientCards.Length > i)
                {
                    cardId = clientCards[i];

                    _panelController.ProcessEffect(cardId, Consts.UserType.Client, _clientWizardController,
                        _hostWizardController);
                    _clientWizardController.ProcessSkill(cardId);

                    yield return new WaitForSeconds(3f);

                    if (UserManager.Instance.IsHost)
                        _netSync.RemoveFrontOfList(Consts.UserType.Client);
                }

                if (ValidGameOver())
                {
                    GameOver();
                    IsInitialized = false;
                    yield break;
                }
            }

            // if the game is not ended,
            _hostWizardController.GainMana();
            _clientWizardController.GainMana();
            StartCoroutine(WaitForRunningTimer());
        }

        #endregion
    }
}