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

        public GameObject panel, scroll3D;

        [SerializeField] private Button[] hostCards;
        [SerializeField] private Button[] clientCards;
        [SerializeField] private Sprite[] cardImgs;
        [SerializeField] private Sprite baseCardImg;
        [SerializeField] private Button hostConfirm, clientConfirm;

        #endregion

        public void OnConfirmBtnClick()
        {
            GameManager.Instance.StopTimer();
        }



        #region Custom methods

        private void Init()
        {
            BgmManager.Instance.SetBgm(BgmManager.SrcNameBattleBgm);
            BgmManager.Instance.Play(true);
            GameManager.Instance.Init(panel);
            
            if(UserManager.Instance.IsHost) 
                clientConfirm.gameObject.SetActive(false);
            else
                hostConfirm.gameObject.SetActive(false);
        }

        public void UpdateHostCardUI()
        {
            int cnt = NetworkSynchronizer.Instance.hostCardList.Count;
            
            for (int i = 0; i < cnt; i++)
            {
                int id = NetworkSynchronizer.Instance.hostCardList[i];
                hostCards[i].GetComponent<Image>().sprite = cardImgs[id];
            }

            for (int i = cnt; i < 3; i++)
            {
                hostCards[i].GetComponent<Image>().sprite = baseCardImg;
            }
        }

        public void UpdateClientCardUI()
        {
            int cnt = NetworkSynchronizer.Instance.clientCardList.Count;
            
            for (int i = 0; i < cnt; i++)
            {
                int id = NetworkSynchronizer.Instance.clientCardList[i];
                clientCards[i].GetComponent<Image>().sprite = cardImgs[id];
            }
                
            for (int i = cnt; i < 3; i++)
            {
                clientCards[i].GetComponent<Image>().sprite = baseCardImg;
            }
        }

        public void UpdateCardUI(int type)
        {
            if (type == 0)
            {
                int cnt = NetworkSynchronizer.Instance.hostCardList.Count;
                
                if (UserManager.Instance.IsHost)
                {
                    UpdateHostCardUI();
                }
                else
                {
                    if (cnt == 3)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            int id = NetworkSynchronizer.Instance.hostCardList[i];
                            hostCards[i].GetComponent<Image>().sprite = cardImgs[id];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            hostCards[i].GetComponent<Image>().sprite = baseCardImg;
                        }
                    }
                }
            }
            else
            {
                int cnt = NetworkSynchronizer.Instance.clientCardList.Count;

                if (UserManager.Instance.IsHost)
                {
                    if (cnt == 3)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            int id = NetworkSynchronizer.Instance.clientCardList[i];
                            clientCards[i].GetComponent<Image>().sprite = cardImgs[id];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            clientCards[i].GetComponent<Image>().sprite = baseCardImg;
                        }
                    }
                }
                else
                {
                    UpdateClientCardUI();
                }
            }
        }

        #endregion



        #region Unity event methods

        private void Start()
        {
            Init();
        }

        #endregion
    }
}
