using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Manager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scene
{
    public class GameSceneController : MonoBehaviour
    {
        #region Public variables
        
        public GameObject panel;
        public GameObject scroll3D;
        
        #endregion
        
        
        
        #region Custom methods

        private void Init()
        {
            BgmManager.Instance.SetBgm(BgmManager.SrcNameBattleBgm);
            BgmManager.Instance.Play(true);
            GameManager.Instance.InitPanel(panel);

            /*if (UserManager.Instance.IsHost)
                NetworkManager.Singleton.StartHost();
            else
                NetworkManager.Singleton.StartClient();*/
        }

        #endregion



        #region Unity event methods

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                scroll3D.GetComponent<ScrollScript3D>().OpenOrCloseScroll();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                NetworkManager.Singleton.StartHost();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                NetworkManager.Singleton.StartClient();
            }
        }

        #endregion
    }
}
