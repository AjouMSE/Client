using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UI.Lobby.CardLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardInScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Private constants

    private const string CardUIName = "Img_CardLibrary_Card";

    #endregion
    
    #region Private variables

    private bool _hideWhenExit;
    
    private CardLibraryCardUIController _cardUIController;

    #endregion


    #region Public variables

    public CardData CardData { get; set; }

    #endregion


    #region Unity Event methods

    private void Awake()
    {
        Init();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_cardUIController != null)
        {
            if(!_cardUIController.gameObject.activeInHierarchy)
                _cardUIController.gameObject.SetActive(true);
            _cardUIController.SetData(CardData);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_cardUIController != null)
        {
            if (_hideWhenExit)
            {
                _cardUIController.gameObject.SetActive(false);
            }   
        }
    }

    #endregion


    #region Private methods

    private void Init()
    {
        var obj = GameObject.Find(CardUIName);
        if (obj != null)
        {
            _cardUIController = obj.GetComponent<CardLibraryCardUIController>();
            if (_cardUIController.RectTrans.localPosition.y < -10)
                _cardUIController.RectTrans.localPosition = new Vector3(0, -10, 0);
        }


        switch (SceneManager.GetActiveScene().name)
        {
            case UIManager.SceneNameLobby:
                _hideWhenExit = false;
                break;
            
            case "GameSceneRemaster":
                _hideWhenExit = true;
                break;
        }
    }

    #endregion


    #region Public methods

    public void OnCardInScrollBtnClick()
    {
    }

    #endregion
}