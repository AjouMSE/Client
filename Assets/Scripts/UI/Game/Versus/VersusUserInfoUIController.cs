using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Game.Versus
{
    public class VersusUserInfoUIController : MonoBehaviour
    {
        #region Private variables

        [SerializeField] private bool isHostInfo;
        [SerializeField] private Text[] textVersusInfos;

        #endregion


        #region Unity event methods

        private void Start()
        {
            Init();
        }

        #endregion


        #region Private variables

        private void Init()
        {
            Packet.User user;
            
            if (isHostInfo)
                user = UserManager.Instance.IsHost ? UserManager.Instance.User : UserManager.Instance.Hostile;
            else
                user = UserManager.Instance.IsHost ? UserManager.Instance.Hostile : UserManager.Instance.User;

            textVersusInfos[0].text = user.nickname;
            textVersusInfos[1].text = user.win.ToString();
            textVersusInfos[2].text = user.lose.ToString();
            textVersusInfos[3].text = user.draw.ToString();
            textVersusInfos[4].text = user.ranking.ToString();
        }

        #endregion
    }
}