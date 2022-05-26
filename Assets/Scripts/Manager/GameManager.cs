using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Core;
using InGame;
using Scene;
using UI.Game;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Unity.Netcode;

namespace Manager
{
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

        public void Init()
        {
            _netSync = GameObject.Find("NetworkSynchronizer").GetComponent<NetworkSynchronizer>();
            _panelController = GameObject.Find("GameSceneObjectController").GetComponent<PanelController>();

            GameObject uiController = GameObject.Find("GameSceneUIController");
            _userInfoUIController = uiController.GetComponent<HUDGameUserInfoUIController>();
            _cardSelectionUIController = uiController.GetComponent<HUDGameCardSelectionUIController>();

            _turnValue = DefaultTurnValue;
            _timerValue = DefaultTimerValue;
            _canSelect = false;
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
            int hostHp = _netSync.GetHostHP();
            int clientHp = _netSync.GetClientHP();

            if (hostHp <= 0 && clientHp <= 0)
                Debug.Log(Consts.BattleResult.DRAW);
            else if (clientHp <= 0)
            {
                if (UserManager.Instance.IsHost)
                {
                    _hostWizardController.BattleResultAction(Consts.BattleResult.WIN);
                    _clientWizardController.BattleResultAction(Consts.BattleResult.LOSE);

                    Debug.Log(Consts.BattleResult.WIN);
                }
                else
                    Debug.Log(Consts.BattleResult.LOSE);
            }
            else if (hostHp <= 0)
            {
                if (UserManager.Instance.IsHost)
                {
                    _hostWizardController.BattleResultAction(Consts.BattleResult.LOSE);
                    _clientWizardController.BattleResultAction(Consts.BattleResult.WIN);

                    Debug.Log(Consts.BattleResult.LOSE);
                }
                else
                    Debug.Log(Consts.BattleResult.WIN);
            }
        }

        public List<int> GetValidCards()
        {
            WizardController wizardController = null;
            if (UserManager.Instance.IsHost)
                wizardController = _hostWizardController;
            else
                wizardController = _clientWizardController;

            return wizardController.ValidCards();
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
            _cardSelectionUIController.UpdateValidCards();
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
            // int[] hostCards = _netSync.GetCopyList(NetworkSynchronizer.UserType.Host);
            // int[] clientCards = _netSync.GetCopyList(NetworkSynchronizer.UserType.Client);

            int[] hostCards = new int[] { 101100002, 101100002, 101100002 };
            int[] clientCards = new int[] { 101100000, 101100003, 101100000 };

            for (int i = 0; i < 3; i++)
            {
                // Process card id
                if (hostCards.Length > i)
                {
                    cardId = hostCards[i];

                    _panelController.ProcessEffect(cardId, 0, _hostWizardController.GetX(), _hostWizardController.GetY());
                    _hostWizardController.ProcessSkill(cardId);

                    if (UserManager.Instance.IsHost)
                    {
                        _netSync.RemoveFrontOfList(Consts.UserType.Host);
                    }

                    yield return new WaitForSeconds(3f);
                }

                // Process card id
                if (clientCards.Length > i)
                {
                    cardId = clientCards[i];

                    _panelController.ProcessEffect(cardId, 1, _clientWizardController.GetX(), _clientWizardController.GetY());
                    _clientWizardController.ProcessSkill(cardId);

                    if (UserManager.Instance.IsHost)
                    {
                        _netSync.RemoveFrontOfList(Consts.UserType.Client);
                    }

                    yield return new WaitForSeconds(3f);
                }

                if (ValidGameOver())
                {
                    GameOver();
                    yield break;
                }
            }

            // if the game is not ended,
            _hostWizardController.GainMana();
            _clientWizardController.GainMana();
            StartCoroutine(WaitForRunningTimer());
        }

        /*IEnumerator ProcessCard()
        {
            for (int i = 0; i < 3; i++)
            {
                NetworkSynchronizer.Instance.RemoveCardFromList(0);
                GameObject.Find("GameSceneUIController").GetComponent<GameSceneController>().UpdateHostCardUI();
                GameObject.Find("GameSceneUIController").GetComponent<GameSceneController>().UpdateClientCardUI();

                GameObject obj1 = Instantiate(skillVfx, new Vector3(UnityEngine.Random.Range(0f, 7f), 1.5f, UnityEngine.Random.Range(0f, 7f)), Quaternion.identity);
                obj1.transform.localScale = new Vector3(4f, 4f, 4f);
                GameObject obj2 = Instantiate(skillVfx2, new Vector3(UnityEngine.Random.Range(6f, 14f), 1.5f, UnityEngine.Random.Range(0f, 7f)), Quaternion.identity);
                obj2.transform.localScale = new Vector3(4f, 4f, 4f);

                yield return new WaitForSeconds(2f);
            }

            // for (int i = 0; i < 3; i++)
            // {
            //     int hostCardCode = NetworkSynchronizer.Instance.GetHostCardFromList(0);
            //     int clientCardCode = NetworkSynchronizer.Instance.GetClientCardFromList(0);
            //
            //     NetworkSynchronizer.Instance.RemoveCardFromList(0);
            //     GameObject.Find("GameSceneController").GetComponent<GameSceneController>().UpdateHostCardUI();
            //     GameObject.Find("GameSceneController").GetComponent<GameSceneController>().UpdateClientCardUI();
            //
            //
            //     if (NetworkManager.Singleton.IsServer)
            //     {
            //         // // @todo - get range from table
            //         // int[,] hostRange = new int[,] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 1, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
            //         // int[,] clientRange = new int[,] { { 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
            //         // NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<WizardController>().Move(hostRange);
            //         // NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(NetworkManager.Singleton.ConnectedClientsIds[1]).GetComponent<WizardController>().Move(clientRange);
            //
            //         // Test Code
            //         NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<SkillControllerTest>().Play(101000000);
            //         NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(NetworkManager.Singleton.ConnectedClientsIds[1]).GetComponent<SkillControllerTest>().Play(101000001);
            //     }
            //
            //     yield return new WaitForSeconds(2f);
            // }
            
        }*/

        #endregion
    }
}