using System;
using System.Collections;
using System.Collections.Generic;
using Data.Cache;
using Manager;
using UnityEngine;
using Utils;
using UnityEngine.UI;

namespace UI.Lobby.CardLibrary
{
    public class CardLibraryUIController : MonoBehaviour
    {
        #region Private variables

        [Header("List of SKill Icon")] 
        [SerializeField] private GameObject skillIconList;
        [Header("2D, 3D Scroll UI Script")] 
        [SerializeField] private ScrollScript2D scroll2D;
        [SerializeField] private ScrollScript3D scroll3D;

        [Header("Skill Icon Prefab")] 
        [SerializeField] private GameObject skillIcon;

        private List<CardInScroll> _cardListPool;
        private CanvasGroup _cardLibraryCanvasGroup;

        #endregion


        #region Unity event methods

        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            if (scroll2D.ScrollAnimator != null)
                scroll2D.ScrollOpen();
        }

        private void Start()
        {
            scroll2D.ScrollOpen();
        }

        #endregion


        #region Private methods

        private void Init()
        {
            _cardListPool = new List<CardInScroll>();
            _cardLibraryCanvasGroup = GetComponent<CanvasGroup>();

            foreach (var key in TableDatas.Instance.cardDatas.Keys)
            {
                var cardData = TableDatas.Instance.cardDatas[key];
                var cardObj = Instantiate(skillIcon, skillIconList.transform);
                cardObj.GetComponent<Image>().sprite = CacheSpriteSource.Instance.GetSource(key);

                var cardInScroll = cardObj.GetComponent<CardInScroll>();
                cardInScroll.CardData = cardData;

                _cardListPool.Add(cardInScroll);
            }
        }

        private void EnableCardIcon(int category)
        {
            if (category < (int)Consts.SkillCategory.Special)
            {
                if (category == (int)Consts.SkillCategory.All)
                {
                    for (var i = 0; i < _cardListPool.Count; i++)
                    {
                        _cardListPool[i].gameObject.SetActive(true);
                    }
                }
                else
                {
                    for (var i = 0; i < _cardListPool.Count; i++)
                    {
                        var type = _cardListPool[i].CardData.type;
                        _cardListPool[i].gameObject.SetActive(type == category);
                    }
                }
            }
            else
            {
                for (var i = 0; i < _cardListPool.Count; i++)
                {
                    var type = _cardListPool[i].CardData.type;
                    _cardListPool[i].gameObject.SetActive(type >= category);
                }
            }
        }

        #endregion


        #region Button callbacks

        public void OnCategoryBtnClick(int type)
        {
            scroll2D.animate(); // close the scroll
            EnableCardIcon(type);
            scroll2D.animate(); // open the scroll
        }

        /// <summary>
        /// Back button callback
        /// </summary>
        public void OnCardLibraryBackBtnClick()
        {
            StartCoroutine(Scroll2DCloseCoroutine(() =>
            {
                scroll3D.OpenScroll();
                UIManager.Instance.Fade(UIManager.FadeType.FadeOut, _cardLibraryCanvasGroup,
                    UIManager.LobbyMenuFadeInOutDuration);
            }));
        }

        #endregion


        #region Coroutines

        private IEnumerator Scroll2DCloseCoroutine(Action callback)
        {
            scroll2D.ScrollClose();
            yield return CacheCoroutineSource.Instance.GetSource(0.8f);
            callback();
        }

        #endregion
    }
}