using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Manager;
using Manager.InGame;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using TMPro;

namespace UI.Game.UserStatus
{
    public class HUDGameUserInfoUIController : MonoBehaviour
    {
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
        [SerializeField] private Text timerText;
        [SerializeField] private Text turnText;

        #endregion


        #region Unity event methods

        private void Start()
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
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Update timer text area
        /// </summary>
        public void UpdateTimerText()
        {
            var timer = GameManager2.Instance.TimerValue;
            if (timer >= 10) timerText.text = $"{Mathf.Round(timer).ToString(CultureInfo.CurrentCulture)}";
            else if (timer > 0) timerText.text = $"{timer:0.0}";
            else timerText.text = "0";
        }

        /// <summary>
        /// Update turn text area
        /// </summary>
        public void UpdateTurnText()
        {
            turnText.text = $"Turn {GameManager.Instance.turnValue.ToString()}";
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