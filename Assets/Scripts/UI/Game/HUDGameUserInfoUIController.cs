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

namespace UI.Game
{
    public class HUDGameUserInfoUIController : MonoBehaviour
    {
        #region Private variables

        [Header("Nickname Text")] [SerializeField]
        private Text hostNicknameText;

        [SerializeField] private Text clientNicknameText;

        [Header("Host HP, Mana Text")] [SerializeField]
        private GameObject hostHpUI;

        [SerializeField] private GameObject hostManaUI;

        [Header("Client HP, Mana UI")] [SerializeField]
        private GameObject clientHpUI;

        [SerializeField] private GameObject clientManaUI;

        [Header("Timer, Turn Text")] [SerializeField]
        private Text timerText;

        [SerializeField] private Text turnText;

        private Text _textHostHp, _textHostMana;
        private Text _textClientHp, _textClientMana;

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
            // Get Host Hp, Mana Text components
            _textHostHp = hostHpUI.GetComponentInChildren<Text>();
            _textHostMana = hostManaUI.GetComponentInChildren<Text>();

            // Get Client Hp, Mana Text components
            _textClientHp = clientHpUI.GetComponentInChildren<Text>();
            _textClientMana = clientManaUI.GetComponentInChildren<Text>();

            // Set text to default values
            _textHostHp.text = Consts.MaxHP.ToString();
            _textHostMana.text = Consts.StartMana.ToString();
            _textClientHp.text = Consts.MaxHP.ToString();
            _textClientMana.text = Consts.StartMana.ToString();

            Packet.User host, client;
            if (UserManager.Instance.IsHost)
            {
                host = UserManager.Instance.User;
                client = UserManager.Instance.Hostile;
                clientManaUI.SetActive(false);
            }
            else
            {
                host = UserManager.Instance.Hostile;
                client = UserManager.Instance.User;
                hostManaUI.SetActive(false);
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
                    _textHostHp.text = $"{value.ToString()}";
                    break;

                case Consts.GameUIType.Mana:
                    _textHostMana.text = $"{value.ToString()}";
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
                    _textClientHp.text = $"{value.ToString()}";
                    break;

                case Consts.GameUIType.Mana:
                    _textClientMana.text = $"{value.ToString()}";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #endregion
    }
}