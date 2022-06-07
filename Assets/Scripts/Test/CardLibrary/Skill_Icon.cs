using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skill_Icon : MonoBehaviour, IPointerEnterHandler
{

    public CardData cardData;
    private Card cardPrefab;

    void Start()
    {
        cardPrefab = GameObject.Find("Card").GetComponent<Card>();

    }

    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardPrefab.SetData(cardData);
    }


}
