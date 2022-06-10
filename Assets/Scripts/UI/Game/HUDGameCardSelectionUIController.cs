using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Core;
using Data.Cache;
using Manager;
using Manager.Net;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Game
{
    public class HUDGameCardSelectionUIController : MonoBehaviour
    {
        #region Constants

        private const int MaxCardCnt = 3;

        #endregion


        #region Private Variables

        [Header("Host Card Selections")] 
        [SerializeField] private GameObject cardListHost;
        
        [Header("Client Card Selections")] 
        [SerializeField] private GameObject cardListClient;

        [Header("Base, Confirm card Images")]
        [SerializeField] private Sprite baseCardImg;
        [SerializeField] private Sprite confirmCardImg;

        [Header("3D card scroll UI")]
        [SerializeField] private ScrollScript3DTest cardScroll3D;

        private Image[] _hostCardImages;
        private Image[] _clientCardImages;

        #endregion


        #region Callbacks

        /// <summary>
        /// Confirm card button callback
        /// </summary>
        public void OnConfirmBtnClick()
        {
            if (GameManager.Instance.canSelect)
                GameManager.Instance.StopTimer();
        }

        public void OnCardAddBtnClick(int id)
        {
            if (GameManager.Instance.canSelect)
            {
                NetGameStatusManager.Instance.AddCardToList(id);
            }
        }

        public void OnCardRemoveBtnClick(int idx)
        {
            if (GameManager.Instance.canSelect)
            {
                NetGameStatusManager.Instance.RemoveCardFromList(idx);
            }
        }

        #endregion


        #region Unity event methods

        private void Start()
        {
            Init();
        }

        #endregion


        #region Custom methods

        public void Init()
        {
            Button[] hostCardListBtns = cardListHost.GetComponentsInChildren<Button>();
            Button[] clientCardListBtns = cardListClient.GetComponentsInChildren<Button>();
            
            _hostCardImages = new Image[MaxCardCnt];
            _clientCardImages = new Image[MaxCardCnt];

            for (int i = 0; i < 3; i++)
            {
                _hostCardImages[i] = hostCardListBtns[i].GetComponent<Image>();
                _clientCardImages[i] = clientCardListBtns[i].GetComponent<Image>();
            }

            if (UserManager.Instance.IsHost) cardListClient.SetActive(false);
            else cardListHost.SetActive(false);
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
                    _hostCardImages[i].sprite = CacheSpriteSource.Instance.GetSource(id);
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
                    _clientCardImages[i].sprite = CacheSpriteSource.Instance.GetSource(id);
                }

                for (int i = cards.Length; i < MaxCardCnt; i++)
                {
                    _clientCardImages[i].sprite = baseCardImg;
                }
            }
        }

        public void UpdateInvalidCards()
        {
            List<int> invalidCards = GameManager.Instance.GetInvalidCards();

            foreach (Image img in cardScroll3D.cardImageDict.Values)
            {
                img.color = new Color(1, 1, 1);
            }

            foreach (Button btn in cardScroll3D.buttonDict.Values)
            {
                btn.interactable = true;
            }

            for (int i = 0; i < invalidCards.Count; i++)
            {
                int id = invalidCards[i];
                cardScroll3D.cardImageDict[id].color = new Color(1, 0.3f, 0.3f);
                cardScroll3D.buttonDict[id].interactable = false;
            }
        }

        #endregion
    }
}