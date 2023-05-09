using System;
using System.Collections;
using System.Collections.Generic;
using Cache;
using DataTable;
using InGame;
using Manager.Net;
using UI.Game;
using UI.Game.CardSelection;
using UI.Game.UserStatus;
using UI.Game.Versus;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Utils;

namespace Manager.InGame
{
    public class MagicaDuelGameManager : MonoSingleton<MagicaDuelGameManager>
    {
        #region Private constants

        private const int DefaultTurnValue = 0;
        private const float DefaultTimerValue = 60;
        private const int MaxPhaseCnt = 3;

        private const string HudCameraTag = "HUDCamera";
        private const string BattleResultReqPath = "/battle/result";

        #endregion


        #region Private variables

        public HUDGameVersusUIController GameVersusUIController { get; set; }
        public HUDGameUserStatusUIController UserStatusUIController { get; set; }
        public HUDGameSelectedCardUIController SelectedCardUIController { get; set; }

        private bool _showBeginEffect;

        #endregion


        #region Public variables

        public PlayerController HostController { get; private set; }
        public PlayerController ClientController { get; private set; }

        public int TurnValue { get; private set; }
        public float TimerValue { get; private set; }
        public bool CanCardSelect { get; private set; }

        #endregion


        #region Private variables

        private bool CheckContinueGame()
        {
            return HostController.Hp > 0 && ClientController.Hp > 0;
        }

        private IEnumerator GameOver(bool allTurnOver)
        {
            Consts.BattleResult hResult, cResult;
            var resultPacket = new Packet.BattleResult();

            if (allTurnOver)
            {
                hResult = HostController.Hp > ClientController.Hp ? Consts.BattleResult.WIN : Consts.BattleResult.LOSE;
                cResult = ClientController.Hp > HostController.Hp ? Consts.BattleResult.WIN : Consts.BattleResult.LOSE;
            }
            else
            {
                hResult = HostController.Hp > 0 ? Consts.BattleResult.WIN : Consts.BattleResult.LOSE;
                cResult = ClientController.Hp > 0 ? Consts.BattleResult.WIN : Consts.BattleResult.LOSE;
            }

            // Set packet, play result animation
            if (UserManager.Instance.IsHost)
            {
                // Host plays Animation & Emoji
                HostController.PlayAnimation(hResult == Consts.BattleResult.WIN
                    ? WizardAnimations.Victory
                    : WizardAnimations.Die);

                HostController.ShowEmoji(
                    hResult == Consts.BattleResult.WIN
                        ? CacheEmojiSource.EmojiType.EmojiXD
                        : CacheEmojiSource.EmojiType.EmojiSad, 2f);

                // Client plays Animation & Emoji
                ClientController.PlayAnimation(cResult == Consts.BattleResult.WIN
                    ? WizardAnimations.Victory
                    : WizardAnimations.Die);

                ClientController.ShowEmoji(
                    cResult == Consts.BattleResult.WIN
                        ? CacheEmojiSource.EmojiType.EmojiCool
                        : CacheEmojiSource.EmojiType.EmojiCry, 2f);

                resultPacket.result = hResult == Consts.BattleResult.WIN ? "WIN" :
                    cResult == Consts.BattleResult.WIN ? "LOSE" : "DRAW";
            }
            else
            {
                resultPacket.result = cResult == Consts.BattleResult.WIN ? "WIN" :
                    hResult == Consts.BattleResult.WIN ? "LOSE" : "DRAW";
            }


            yield return CacheCoroutineSource.Instance.GetSource(3f);

            // Send result to server
            NetHttpRequestManager.Instance.Post(BattleResultReqPath, JsonUtility.ToJson(resultPacket), req =>
            {
                using (req)
                {
                    if (req.result == UnityWebRequest.Result.Success)
                    {
                        var scoreGap = UserManager.Instance.User.score;
                        var json = req.downloadHandler.text;

                        UserManager.Instance.UpdateUserInfo(json);
                        UserManager.Instance.RemoveHostileInfo();
                        scoreGap = UserManager.Instance.User.score - scoreGap;

                        GameVersusUIController.gameObject.SetActive(true);
                        UserStatusUIController.gameObject.SetActive(false);
                        SelectedCardUIController.gameObject.SetActive(false);
                        GameVersusUIController.ShowGameResult(UserManager.Instance.IsHost, hResult, cResult,
                            scoreGap);
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
        /// Process phase (1turn -> max 3 phase) (process each cards)
        /// </summary>
        /// <param name="isHostSkill"></param>
        /// <param name="skillCode"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        private void ProcessPhase(bool isHostSkill, int skillCode)
        {
            SelectedCardUIController.ShowProcessingCard(skillCode, isHostSkill, () =>
            {
                if (isHostSkill)
                {
                    PanelManager.Instance.ProcessEffect(skillCode, Consts.UserType.Host, HostController,
                        ClientController, () => { HostController.ProcessSkill(skillCode); });
                }
                else
                {
                    PanelManager.Instance.ProcessEffect(skillCode, Consts.UserType.Client, ClientController,
                        HostController, () => { ClientController.ProcessSkill(skillCode); });
                }

                if (UserManager.Instance.IsHost)
                    NetGameStatusManager.Instance.PollCardFromList(isHostSkill);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animationName"></param>
        private void BothRandomEmojiEffect(string animationName)
        {
            var nums = CustomUtils.MakeCoupleRandomNum(CacheEmojiSource.Instance.EmojiCount);

            HostController.PlayAnimation(animationName, 1.2f);
            HostController.ShowEmoji((CacheEmojiSource.EmojiType)nums[0], 1.5f);
            PanelManager.Instance.ProcessEffect(101202000, Consts.UserType.Host, HostController, ClientController);

            ClientController.PlayAnimation(animationName, 1.2f);
            ClientController.ShowEmoji((CacheEmojiSource.EmojiType)nums[1], 1.5f);
            PanelManager.Instance.ProcessEffect(101203001, Consts.UserType.Client, ClientController, HostController);
        }

        #endregion


        #region Public methods

        public override void Init()
        {
            TurnValue = DefaultTurnValue;
            TimerValue = DefaultTimerValue;
            CanCardSelect = false;
            _showBeginEffect = false;
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

        /// <summary>
        /// 
        /// </summary>
        public void BeginGame()
        {
            StartCoroutine(WaitForRunningTimer());
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopTimer()
        {
            TimerValue = 0.1f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<int> GetInvalidCards()
        {
            return UserManager.Instance.IsHost ? HostController.InvalidCards() : ClientController.InvalidCards();
        }

        #endregion


        #region Game logic coroutines

        /// <summary>
        /// Wait for running timer
        /// Checks if both host and client are ready to run timer
        /// </summary>
        private IEnumerator WaitForRunningTimer()
        {
            UserStatusUIController.UpdateNotify(HUDGameUserStatusUIController.NotifyWaitForOpponent, true);
            NetGameStatusManager.Instance.ReadyToRunTimer(true);
            NetGameStatusManager.Instance.ReadyToProcessCard(false);

            while (!NetGameStatusManager.Instance.BothReadyToRunTimer())
            {
                yield return null;
            }

            if (!_showBeginEffect)
            {
                BothRandomEmojiEffect(WizardAnimations.Recovery);
                _showBeginEffect = true;
            }

            StartCoroutine(RunTimer());
        }

        /// <summary>
        /// Run card selection timer
        /// </summary>
        /// <returns></returns>
        private IEnumerator RunTimer()
        {
            // Update values
            TurnValue++;
            TimerValue = DefaultTimerValue;
            CanCardSelect = true;

            // Update UI
            UserStatusUIController.UpdateNotify(HUDGameUserStatusUIController.NotifySelectCard, true);
            UserStatusUIController.UpdateTimer();
            UserStatusUIController.UpdateTurn();
            SelectedCardUIController.UpdateInvalidCards();
            SelectedCardUIController.OpenCardScroll();

            while (TimerValue > 0)
            {
                TimerValue -= Time.deltaTime;
                UserStatusUIController.UpdateTimer();
                yield return null;
            }

            // Update values
            TimerValue = 0;
            CanCardSelect = false;

            // Update UI
            UserStatusUIController.UpdateTimer();
            SelectedCardUIController.CloseCardScroll();

            StartCoroutine(WaitForProcessingCards());
        }

        /// <summary>
        /// Wait for processing cards
        /// Checks if both host and client are ready to process cards in card list
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForProcessingCards()
        {
            UserStatusUIController.UpdateNotify(HUDGameUserStatusUIController.NotifyWaitForOpponent, true);
            NetGameStatusManager.Instance.ReadyToRunTimer(false);
            NetGameStatusManager.Instance.ReadyToProcessCard(true);

            while (!NetGameStatusManager.Instance.BothReadyToProcessCards())
            {
                yield return null;
            }

            StartCoroutine(ProcessTurn());
        }

        /// <summary>
        /// Process turn
        /// </summary>
        /// <returns></returns>
        private IEnumerator ProcessTurn()
        {
            UserStatusUIController.UpdateNotify(HUDGameUserStatusUIController.NotifyProcessCard, true);

            var hostCards = NetGameStatusManager.Instance.CopyHostCardList();
            var clientCards = NetGameStatusManager.Instance.CopyClientCardList();

            if (hostCards.Length == 0 && clientCards.Length == 0)
                yield return CacheCoroutineSource.Instance.GetSource(3f);

            // Process cards
            for (var i = 0; i < MaxPhaseCnt; i++)
            {
                int hostSkillCode, clientSkillCode;
                if (hostCards.Length > i && clientCards.Length > i)
                {
                    hostSkillCode = hostCards[i];
                    clientSkillCode = clientCards[i];

                    var hostPriority = TableDatas.Instance.GetCardData(hostSkillCode).priority;
                    var clientPriority = TableDatas.Instance.GetCardData(clientSkillCode).priority;
                    var isHostFirst = hostPriority <= clientPriority;

                    if (isHostFirst)
                    {
                        ProcessPhase(true, hostSkillCode);
                        yield return CacheCoroutineSource.Instance.GetSource(5f);

                        ProcessPhase(false, clientSkillCode);
                        yield return CacheCoroutineSource.Instance.GetSource(5f);
                    }
                    else
                    {
                        ProcessPhase(false, clientSkillCode);
                        yield return CacheCoroutineSource.Instance.GetSource(5f);

                        ProcessPhase(true, hostSkillCode);
                        yield return CacheCoroutineSource.Instance.GetSource(5f);
                    }
                }
                else if (hostCards.Length > i)
                {
                    hostSkillCode = hostCards[i];
                    ProcessPhase(true, hostSkillCode);
                    yield return CacheCoroutineSource.Instance.GetSource(5f);
                }
                else if (clientCards.Length > i)
                {
                    clientSkillCode = clientCards[i];
                    ProcessPhase(false, clientSkillCode);
                    yield return CacheCoroutineSource.Instance.GetSource(5f);
                }
                else break;

                if (!CheckContinueGame())
                {
                    // game over
                    StartCoroutine(GameOver(false));
                    IsInitialized = false;
                    yield break;
                }
                
                if (TurnValue >= 15)
                {
                    // game over
                    StartCoroutine(GameOver(true));
                    IsInitialized = false;
                    yield break;
                }
            }

            HostController.RestoreMana(TurnValue);
            ClientController.RestoreMana(TurnValue);
            BothRandomEmojiEffect(WizardAnimations.Defend);
            StartCoroutine(WaitForRunningTimer());
        }

        #endregion
    }
}