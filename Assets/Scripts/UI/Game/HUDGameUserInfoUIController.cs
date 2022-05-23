using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Manager;
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

        [Header("Nickname Text")]
        [SerializeField] private Text hostNicknameText;
        [SerializeField] private Text clientNicknameText;

        [Header("HP Text")]
        [SerializeField] private TextMeshProUGUI hostHPText;
        [SerializeField] private TextMeshProUGUI clientHPText;

        [Header("Mana Text")]
        [SerializeField] private TextMeshProUGUI hostManaText;
        [SerializeField] private TextMeshProUGUI clientManaText;

        [Header("Timer, Turn Text")]
        [SerializeField] private Text timerText;

        [SerializeField] private Text turnText;

        #endregion


        #region Custom methods

        /// <summary>
        /// Initialize host, client information UI
        /// </summary>
        public void Init()
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

            hostNicknameText.text = host.nickname;
            clientNicknameText.text = client.nickname;
        }

        /// <summary>
        /// Update timer text area
        /// </summary>
        public void UpdateTimerText()
        {
            float timer = GameManager.Instance.timerValue;
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

        public void UpdateHostUI(int type, int value)
        {
            switch (type)
            {
                case 0:
                    hostHPText.text = $"{value}";
                    break;

                case 1:
                    hostManaText.text = $"{value}";
                    break;
            }
        }

        public void UpdateClientUI(int type, int value)
        {
            switch (type)
            {
                case 0:
                    clientHPText.text = $"{value}";
                    break;

                case 1:
                    clientManaText.text = $"{value}";
                    break;
            }
        }

        #endregion
    }
}
