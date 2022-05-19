using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Manager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Game
{
    public class HUDGameCardSelectionUIController : MonoBehaviour
    {
        #region Private Variables

        [Header("Host Card Selections")]
        [SerializeField] private Button[] hostCards;
        
        [Header("Client Card Selections")]
        [SerializeField] private Button[] clientCards;

        [Header("Host Confirm Button")] 
        [SerializeField] private Button hostConfirmBtn;

        [Header("Client Confirm Button")] 
        [SerializeField] private Button clientConfirmBtn;
        
        [SerializeField] private Sprite[] cardImgs;
        [SerializeField] private Sprite baseCardImg;

        [Header("3D card scroll UI")]
        [SerializeField] private ScrollScript3D cardScroll3D;

        #endregion
        
        
        #region Unity event methods

        private void Start()
        {
            Init();
        }

        #endregion
        

        #region Callbacks

        public void OnConfirmBtnClick()
        {
            if(GameManager.Instance.canSelect)
                GameManager.Instance.StopTimer();
        }

        #endregion


        #region Custom methods

        private void Init()
        {
            if (UserManager.Instance.IsHost) clientConfirmBtn.gameObject.SetActive(false);
            else hostConfirmBtn.gameObject.SetActive(false);
        }

        public void OpenCardScroll()
        {
            cardScroll3D.OpenScroll();
        }

        public void CloseCardScroll()
        {
            cardScroll3D.CloseScroll();
        }
        
        #endregion

        /*public void OnConfirmBtnClick()
        {
            GameManager.Instance.StopTimer();
        }

        #region Custom methods

        private void Init()
        {
            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.BattleBgm1, true);
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
        }*/
    }
}
