using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Core;
using Manager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Game
{
    public class HUDGameCardSelectionUIController : MonoBehaviour
    {
        #region

        private const int MaxCardCnt = 3;
        
        #endregion
        
        
        #region Private Variables

        [Header("Host Card Selections")]
        [SerializeField] private Button[] hostCards;
        
        [Header("Client Card Selections")]
        [SerializeField] private Button[] clientCards;

        [Header("Host Confirm Button")] 
        [SerializeField] private Button hostConfirmBtn;

        [Header("Client Confirm Button")] 
        [SerializeField] private Button clientConfirmBtn;
        
        [Header("Card Images")] 
        [SerializeField] private Sprite[] cardImgs;
        [SerializeField] private Sprite baseCardImg;
        [SerializeField] private Sprite confirmCardImg;

        [Header("3D card scroll UI")]
        [SerializeField] private ScrollScript3D cardScroll3D;

        [Header("NetworkSynchronizer")] 
        [SerializeField] private NetworkSynchronizer _netSync;
        
        private Image[] _hostCardImages;
        private Image[] _clientCardImages;

        #endregion
        
        
        #region Callbacks

        /// <summary>
        /// Confirm card button callback
        /// </summary>
        public void OnConfirmBtnClick()
        {
            if(GameManager.Instance.canSelect)
                GameManager.Instance.StopTimer();
        }

        public void OnCardAddBtnClick(int id)
        {
            if (GameManager.Instance.canSelect)
            {
                _netSync.AddCardToList(id);
            }
        }

        public void OnCardRemoveBtnClick(int idx)
        {
            if (GameManager.Instance.canSelect)
            {
                _netSync.RemoveCardFromList(idx);
            }
        }

        #endregion


        #region Custom methods

        public void Init()
        {
            _hostCardImages = new Image[MaxCardCnt];
            _clientCardImages = new Image[MaxCardCnt];
            
            for (int i = 0; i < 3; i++)
            {
                _hostCardImages[i] = hostCards[i].gameObject.GetComponent<Image>();
                _clientCardImages[i] = clientCards[i].gameObject.GetComponent<Image>();
            }
            
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

        public void UpdateHostCardSelectionUI(int[] cards)
        {
            if (UserManager.Instance.IsHost)
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    int id = cards[i];
                    _hostCardImages[i].sprite = cardImgs[id];
                }

                for (int i = cards.Length; i < MaxCardCnt; i++)
                {
                    _hostCardImages[i].sprite = baseCardImg;
                }
            }
        }

        public void UpdateClientCardSelectionUI(int[] cards)
        {
            if (!UserManager.Instance.IsHost)
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    int id = cards[i];
                    _clientCardImages[i].sprite = cardImgs[id];
                }

                for (int i = cards.Length; i < MaxCardCnt; i++)
                {
                    _clientCardImages[i].sprite = baseCardImg;
                }
            }
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
