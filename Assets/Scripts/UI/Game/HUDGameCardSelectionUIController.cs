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
            if (GameManager.Instance.canSelect)
                GameManager.Instance.StopTimer();
        }

        public void OnCardAddBtnClick(int id)
        {
            if (GameManager.Instance.canSelect)
            {
                _netSync.AddCardToList(id);
                UpdateValidCards();
            }
        }

        public void OnCardRemoveBtnClick(int idx)
        {
            if (GameManager.Instance.canSelect)
            {
                _netSync.RemoveCardFromList(idx);
                UpdateValidCards();
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

        public void UpdateValidCards()
        {
            List<int> validCards = GameManager.Instance.GetValidCards();
            for (int i = 0; i < validCards.Count; i++)
            {
                Debug.Log(validCards[i]);
            }
        }

        #endregion
    }
}