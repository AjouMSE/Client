using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CardLibrary : MonoBehaviour
{
    [SerializeField] private GameObject cardGrid;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Dropdown cardCategory;
    [SerializeField] private GameObject scrollPrefab;

    List<GameObject> cardlist = new List<GameObject>();

    ScrollScript2D scroll2d;

    enum SkillCategory { All = 0, Move = 1, Attack = 2, Special = 3}

    // Start is called before the first frame update
    void Start()
    {
        TableLoader.Instance.LoadTableData();   //Test Code
        scroll2d = scrollPrefab.GetComponent<ScrollScript2D>();
        scroll2d.animate();

        foreach(KeyValuePair<int, CardData> cardData in TableDatas.Instance.cardDatas)
        {
            GameObject tmp = Instantiate<GameObject>(cardPrefab, cardGrid.transform);

            Card card = tmp.GetComponent<Card>();

            card.skillData = cardData.Value;
            cardlist.Add(tmp);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CategoryChange()
    {
        switch (cardCategory.value)
        {
            case (int)SkillCategory.All:
                scroll2d.animate();
                for (int i = 0; i < cardlist.Count; i++)
                {
                    cardlist[i].SetActive(true);
                }
                scroll2d.animate();
                break;
            case (int)SkillCategory.Move:
                scroll2d.animate();
                for (int i = 0; i < cardlist.Count; i++)
                {
                    if(cardlist[i].GetComponent<Card>().skillData.type == (int)SkillType.Move)
                    {
                        cardlist[i].SetActive(true);
                    }
                    else
                    {
                        cardlist[i].SetActive(false);
                    }
                }
                scroll2d.animate();
                break;
            case (int)SkillCategory.Attack:
                scroll2d.animate();
                for (int i = 0; i < cardlist.Count; i++)
                {
                    if (cardlist[i].GetComponent<Card>().skillData.type == (int)SkillType.Attack)
                    {
                        cardlist[i].SetActive(true);
                    }
                    else
                    {
                        cardlist[i].SetActive(false);
                    }
                }
                scroll2d.animate();
                break;
            case (int)SkillCategory.Special:
                scroll2d.animate();
                for (int i = 0; i < cardlist.Count; i++)
                {
                    if (cardlist[i].GetComponent<Card>().skillData.type == (int)SkillType.Special)
                    {
                        cardlist[i].SetActive(true);
                    }
                    else
                    {
                        cardlist[i].SetActive(false);
                    }
                }
                scroll2d.animate();
                break;
        }
    }
}
