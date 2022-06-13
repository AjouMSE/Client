using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using InGame;
using Manager;
using Manager.InGame;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using TMPro;

namespace UI.Game.UserStatus
{
    public class HUDGameUserStatusUIController : MonoBehaviour
    {
        #region Public constants

        public const string NotifyWaitForOpponent = "Waiting for opponent...";
        public const string NotifySelectCard = "Choose the card\nyou want to use";
        public const string NotifyProcessCard = "Processing card logic...";

        #endregion


        #region Private variables

        [Header("Nickname Text")] 
        [SerializeField] private Text hostNicknameText;
        [SerializeField] private Text clientNicknameText;

        [Header("Host Hp, Mana UI Controller")] 
        [SerializeField] private UserStatusHpUIController hostHpUIController;
        [SerializeField] private UserStatusManaUIController hostManaUIController;

        [Header("Client Hp, Mana UI Controller")] 
        [SerializeField] private UserStatusHpUIController clientHpUIController;
        [SerializeField] private UserStatusManaUIController clientManaUIController;

        [Header("Timer, Turn Text")] 
        [SerializeField] private UserStatusTimerUIController timerUIController;

        #endregion


        #region Unity event methods

        private void Awake()
        {
            Init();
        }

        #endregion


        #region Private methods

        private void Init()
        {
            Packet.User host, client;
            if (UserManager.Instance.IsHost)
            {
                host = UserManager.Instance.User;
                client = UserManager.Instance.Hostile;
            }
            else
            {
                host = UserManager.Instance.Hostile;
                client = UserManager.Instance.User;
            }

            // Set Nickname
            hostNicknameText.text = host.nickname;
            clientNicknameText.text = client.nickname;

            GameManager2.Instance.UserStatusUIController = this;
            
            var players = GameObject.FindGameObjectsWithTag("Player");
            for (var i = 0; i < players.Length; i++)
            {
                var controller = players[i].GetComponent<PlayerController>();
                controller.UserStatusUIController = this;
            }
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Update timer text area
        /// </summary>
        public void UpdateTimer()
        {
            timerUIController.UpdateTimerText();
        }

        /// <summary>
        /// Update turn text area
        /// </summary>
        public void UpdateTurn()
        {
            timerUIController.UpdateTurnText();
        }

        public void UpdateNotify(string text = null, bool fade = false)
        {
            timerUIController.UpdateNotifyText(text, fade);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void UpdateHostUI(Consts.GameUIType type, int value)
        {
            switch (type)
            {
                case Consts.GameUIType.Hp:
                    hostHpUIController.UpdateHpUI(value);
                    break;

                case Consts.GameUIType.Mana:
                    hostManaUIController.UpdateManaUI(value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void UpdateClientUI(Consts.GameUIType type, int value)
        {
            switch (type)
            {
                case Consts.GameUIType.Hp:
                    clientHpUIController.UpdateHpUI(value);
                    break;

                case Consts.GameUIType.Mana:
                    clientManaUIController.UpdateManaUI(value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #endregion
    }
}