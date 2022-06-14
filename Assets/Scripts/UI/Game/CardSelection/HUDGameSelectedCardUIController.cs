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

        private const int HostDefaultX = -1200, ClientDefaultX = 1200;
        private const int MaxCardCnt = 3;
        private const int ProcessCardMoveSpeed = ClientDefaultX * 4;

        #endregion


        #region Private Variables

        [Header("Host Card Selections")] [SerializeField]
        private GameObject cardListHost;

        [Header("Client Card Selections")] [SerializeField]
        private GameObject cardListClient;

        [Header("Base, Confirm card Images")] [SerializeField]
        private Sprite baseCardImg;

        [SerializeField] private Sprite confirmCardImg;

        [Header("3D card scroll UI")] [SerializeField]
        private CardListScrollUIController cardScroll3D;

        [Header("Process Card, Frame")] [SerializeField]
        private Image imageProcessCard;

        [SerializeField] private RectTransform processCardFrameRectTransform;

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
            NetGameStatusManager.Instance.SelectedCardUIController = this;
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
           var invalidCards = GameManager2.Instance.GetInvalidCards();

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
                GameManager2.Instance.StopTimer();
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

        public void ShowProcessingCard(int skillCode, bool isHostSkill, Action callback = null)
        {
            // Start Effect
            imageProcessCard.sprite = CacheSpriteSource.Instance.GetSource(skillCode);
            StartCoroutine(ProcessCardMovementEffectCoroutine(isHostSkill, callback));
        }

        #endregion


        #region Coroutines

        private IEnumerator ProcessCardMovementEffectCoroutine(bool isHostSkill, Action callback = null)
        {
            // Set default position
            processCardFrameRectTransform.localPosition = new Vector3(isHostSkill ? HostDefaultX : ClientDefaultX, 0, 0);

            var x = processCardFrameRectTransform.localPosition.x;
            while (Mathf.Abs(x) > 0.1f)
            {
                processCardFrameRectTransform.localPosition = Vector3.MoveTowards(
                    processCardFrameRectTransform.localPosition,
                    Vector3.zero, ProcessCardMoveSpeed * Time.deltaTime);
                x = processCardFrameRectTransform.localPosition.x;
                yield return null;
            }

            // movement : 0.25sec, waiting 1.25sec => 1.5sec
            yield return CacheCoroutineSource.Instance.GetSource(1.25f);

            processCardFrameRectTransform.localPosition = new Vector3(HostDefaultX, 0, 0);
            callback?.Invoke();
        }

        #endregion
    }
}