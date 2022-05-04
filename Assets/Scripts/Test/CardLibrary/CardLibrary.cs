using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLibrary : MonoBehaviour
{
    [SerializeField] private GameObject cardgrid;
    [SerializeField] private GameObject cardprefab;
    [SerializeField] private Dropdown cardcategory;
    List<GameObject> cardlist = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        TableLoader.Instance.LoadTableData();   //Test Code

        int i = 0;
        foreach(KeyValuePair<int, CardData> cardData in TableDatas.Instance.cardDatas)
        {
            GameObject tmp = Instantiate<GameObject>(cardprefab, cardgrid.transform);

            Card card = tmp.GetComponent<Card>();

            card.skilldata = cardData.Value;
            cardlist.Add(tmp);
            Debug.Log(card.skilldata.text);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(cardcategory.value == 0)
        {
            for (int i = 0; i < cardlist.Count; i++)
            {
                cardlist[i].SetActive(true);
            }
        }
        if (cardcategory.value == 1)
        {
            for (int i = 0; i < cardlist.Count; i++){
                cardlist[i].SetActive(true);
                Card tmpcard =  cardlist[i].GetComponent<Card>();
                if(tmpcard.skilldata.type != 1)
                {
                    cardlist[i].SetActive(false);
                }
            }
        }
        if (cardcategory.value == 2)
        {

            for (int i = 0; i < cardlist.Count; i++)
            {
                cardlist[i].SetActive(true);
                Card tmpcard = cardlist[i].GetComponent<Card>();
                if (tmpcard.skilldata.type != 100)
                {
                    cardlist[i].SetActive(false);
                }
            }
        }
        if (cardcategory.value == 3)
        {
            for (int i = 0; i < cardlist.Count; i++)
            {
                cardlist[i].SetActive(true);
                Card tmpcard = cardlist[i].GetComponent<Card>();
                if (tmpcard.skilldata.type != 200)
                {
                    cardlist[i].SetActive(false);
                }
            }
        }
    }
}
