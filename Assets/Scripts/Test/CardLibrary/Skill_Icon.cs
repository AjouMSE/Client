using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Skill_Icon : MonoBehaviour, IPointerEnterHandler
{

    public CardData cardData;
    private Card cardPrefab;

    void Start()
    {
        this.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Skill_Icon/" + cardData.icon);

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
