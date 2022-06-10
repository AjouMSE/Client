using System.Collections;
using System.Collections.Generic;
using InGame;
using Manager.Net;
using UI.Game;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Manager.InGame
{
    public class GameManager2 : MonoSingleton<GameManager2>
    {
        #region Private constants

        private const int DefaultTurnValue = 0;
        private const float DefaultTimerValue = 60;
        private const int MaxPhaseCnt = 3;
        private const string HudCameraTag = "HUDCamera";

        #endregion


        #region Private variables

        private HUDGameVersusUIController _gameVersusUIController;
        private HUDGameUserInfoUIController _userInfoUIController;
        private HUDGameCardSelectionUIController _cardSelectionUIController;

        #endregion


        #region Public variables

        public PlayerController HostController { get; private set; }
        public PlayerController ClientController { get; private set; }

        public int TurnValue { get; private set; }
        public float TimerValue { get; private set; }
        public bool CanCardSelect { get; private set; }

        #endregion

        #region Public methods

        public override void Init()
        {
            var hudCamera = GameObject.FindWithTag(HudCameraTag);
            _gameVersusUIController = hudCamera.GetComponentInChildren<HUDGameVersusUIController>();
            _userInfoUIController = hudCamera.GetComponentInChildren<HUDGameUserInfoUIController>();
            _cardSelectionUIController = hudCamera.GetComponentInChildren<HUDGameCardSelectionUIController>();

            TurnValue = DefaultTurnValue;
            TimerValue = DefaultTimerValue;
            CanCardSelect = false;

            IsInitialized = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public void SetPlayerController(PlayerController controller)
        {
            if (controller is null) return;

            if (NetworkManager.Singleton.IsHost)
                HostController = controller;
            else
                ClientController = controller;
        }

        public void CheckReadyToRunTimer()
        {
            StartCoroutine(WaitForRunningTimer());
        }

        #endregion


        #region Game logic coroutines

        /// <summary>
        /// Wait for running timer
        /// Checks if both host and client are ready to run timer
        /// </summary>
        private IEnumerator WaitForRunningTimer()
        {
            NetGameStatusManager.Instance.ReadyToRunTimer(true);
            NetGameStatusManager.Instance.ReadyToProcessCards(false);

            while (!NetGameStatusManager.Instance.BothReadyToRunTimer())
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
            TurnValue++;
            TimerValue = DefaultTimerValue;
            CanCardSelect = true;

            _userInfoUIController.UpdateTimerText();
            _userInfoUIController.UpdateTurnText();
            _cardSelectionUIController.UpdateInvalidCards();
            _cardSelectionUIController.OpenCardScroll();

            while (TimerValue > 0)
            {
                TimerValue -= Time.deltaTime;
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
            NetGameStatusManager.Instance.ReadyToRunTimer(false);
            NetGameStatusManager.Instance.ReadyToProcessCards(true);

            TimerValue = 0;
            CanCardSelect = false;

            _userInfoUIController.UpdateTimerText();
            _cardSelectionUIController.CloseCardScroll();

            while (!NetGameStatusManager.Instance.BothReadyToProcessCards())
            {
                yield return null;
            }

            StartCoroutine(ProcessPhases());
        }

        /// <summary>
        /// Process cards in card list
        /// </summary>
        /// <returns></returns>
        private IEnumerator ProcessPhases()
        {
            for (int i = 0; i < MaxPhaseCnt; i++)
            {
                yield break;
            }
        }

        #endregion
    }
}