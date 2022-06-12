using System.Collections;
using System.Collections.Generic;
using UI.Lobby.CardLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInScroll : MonoBehaviour, IPointerEnterHandler
{
    #region Private variables

    private CardLibraryCardUIController _cardUIController;

    #endregion


    #region Public variables

    public CardData CardData { get; set; }

    #endregion


    #region Unity Event methods

    private void Start()
    {
        _cardUIController = GameObject.Find("Img_CardLibrary_Card").GetComponent<CardLibraryCardUIController>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_cardUIController != null)
            _cardUIController.SetData(CardData);
    }

    #endregion


    #region Public methods

    public void OnCardInScrollBtnClick()
    {
    }

    #endregion
}