using System.Collections;
using System.Collections.Generic;
using Data.Cache;
using InGame;
using Manager.Net;
using UI.Game;
using UI.Game.CardSelection;
using UI.Game.UserStatus;
using UI.Game.Versus;
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

        public HUDGameVersusUIController GameVersusUIController { get; set; }
        public HUDGameUserStatusUIController UserStatusUIController { get; set; }
        public HUDGameSelectedCardUIController SelectedCardUIController { get; set; }

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
            TurnValue = DefaultTurnValue;
            TimerValue = DefaultTimerValue;
            CanCardSelect = false;
            IsInitialized = true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isHostPlayerController"></param>
        /// <param name="controller"></param>
        public void SetPlayerController(bool isHostPlayerController, PlayerController controller)
        {
            if (controller is null) return;

            if (isHostPlayerController)
                HostController = controller;
            else
            {
                ClientController = controller;
            }
        }

        public void CheckReadyToRunTimer()
        {
            StartCoroutine(WaitForRunningTimer());
        }

        public void StopTimer()
        {
            TimerValue = 0.1f;
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
            UserStatusUIController.UpdateNotify(HUDGameUserStatusUIController.NotifyWaitForOpponent, true);

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
            UserStatusUIController.UpdateNotify(HUDGameUserStatusUIController.NotifySelectCard, true);

            UserStatusUIController.UpdateTimer();
            UserStatusUIController.UpdateNotify("");
            //SelectedCardUIController.UpdateInvalidCards();
            SelectedCardUIController.OpenCardScroll();

            while (TimerValue > 0)
            {
                TimerValue -= Time.deltaTime;
                UserStatusUIController.UpdateTimer();
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
            UserStatusUIController.UpdateNotify(HUDGameUserStatusUIController.NotifyWaitForOpponent, true);

            TimerValue = 0;
            CanCardSelect = false;

            UserStatusUIController.UpdateTimer();
            SelectedCardUIController.CloseCardScroll();

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
            UserStatusUIController.UpdateNotify(HUDGameUserStatusUIController.NotifyProcessCard, true);
            var hostCards = NetGameStatusManager.Instance.CopyHostCardList();
            var clientCards = NetGameStatusManager.Instance.CopyClientCardList();
            
            for (int i = 0; i < MaxPhaseCnt; i++)
            {
                HostController.ShowEmoji(CacheEmojiSource.EmojiType.EmojiAngry, 1.5f);
                ClientController.ShowEmoji(CacheEmojiSource.EmojiType.EmojiCry, 1.5f);
                yield return CacheCoroutineSource.Instance.GetSource(3f);
            }
        }

        #endregion
    }
}