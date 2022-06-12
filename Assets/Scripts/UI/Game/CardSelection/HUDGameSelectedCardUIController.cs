using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Core;
using Data.Cache;
using Manager;
using Manager.InGame;
using Manager.Net;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Game.CardSelection
{
    public class HUDGameSelectedCardUIController : MonoBehaviour
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
        [SerializeField] private CardListScrollUIController cardScroll3D;

        private Image[] _hostCardImages;
        private Image[] _clientCardImages;

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
            var hostCardListBtnArr = cardListHost.GetComponentsInChildren<Button>();
            var clientCardListBtnArr = cardListClient.GetComponentsInChildren<Button>();

            _hostCardImages = new Image[MaxCardCnt];
            _clientCardImages = new Image[MaxCardCnt];

            for (int i = 0; i < 3; i++)
            {
                _hostCardImages[i] = hostCardListBtnArr[i].GetComponent<Image>();
                _clientCardImages[i] = clientCardListBtnArr[i].GetComponent<Image>();
            }

            if (UserManager.Instance.IsHost)
                cardListClient.SetActive(false);
            else
                cardListHost.SetActive(false);

            GameManager2.Instance.SelectedCardUIController = this;
        }

        #endregion


        #region Public methods

        public void OpenCardScroll()
        {
            cardScroll3D.OpenScroll();
        }

        public void CloseCardScroll()
        {
            cardScroll3D.CloseScroll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cards"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cards"></param>
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

        /// <summary>
        /// 
        /// </summary>
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


        #region Callbacks

        /// <summary>
        /// Confirm card button callback
        /// </summary>
        public void OnConfirmBtnClick()
        {
            if (GameManager2.Instance.CanCardSelect)
                GameManager.Instance.StopTimer();
        }

        public void OnCardAddBtnClick(int id)
        {
            if (GameManager2.Instance.CanCardSelect)
            {
                NetGameStatusManager.Instance.AddCardToList(id);
            }
        }

        public void OnCardRemoveBtnClick(int idx)
        {
            if (GameManager2.Instance.CanCardSelect)
            {
                NetGameStatusManager.Instance.RemoveCardFromList(idx);
            }
        }

        #endregion
    }
}