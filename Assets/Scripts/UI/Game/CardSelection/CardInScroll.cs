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
    
    private UnityEngine.SceneManagement.Scene _currentScene;
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
        if (_cardUIController == null) return;
        
        // Set card data
        _cardUIController.SetData(CardData);
        if (_currentScene.name.Equals(UIManager.SceneNameGameRemaster))
            _cardUIController.RectTrans.localPosition = new Vector3(0, -10, 0);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_cardUIController == null) return;
        
        if (_currentScene.name.Equals(UIManager.SceneNameGameRemaster))
            _cardUIController.RectTrans.localPosition = new Vector3(0, -1000, 0);
    }

    #endregion


    #region Private methods

    private void Init()
    {
        var obj = GameObject.Find(CardUIName);
        if (obj == null) return;
        _cardUIController = obj.GetComponent<CardLibraryCardUIController>();
        _currentScene = SceneManager.GetActiveScene();
    }

    #endregion


    #region Public methods

    public void OnCardInScrollBtnClick()
    {
    }

    #endregion
}