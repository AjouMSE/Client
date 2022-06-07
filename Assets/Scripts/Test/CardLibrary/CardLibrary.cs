using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CardLibrary : MonoBehaviour
{
    [SerializeField] private GameObject iconGrid;
    [SerializeField] private TMP_Dropdown cardCategory;
    [SerializeField] private GameObject scrollPrefab;

    [SerializeField] private GameObject skill_IconPrefab;

    List<GameObject> cardlist = new List<GameObject>();

    private ScrollScript2D scroll2d;

    enum SkillCategory { All = 0, Move = 1, Attack = 2, Special = 3}

    // Start is called before the first frame update
    void Start()
    {
        TableLoader.Instance.LoadTableData();   //Test Code


        foreach(KeyValuePair<int, CardData> cardData in TableDatas.Instance.cardDatas)
        {
            GameObject tmp = Instantiate<GameObject>(skill_IconPrefab, iconGrid.transform);

            Skill_Icon icon = tmp.GetComponent<Skill_Icon>();
            skill_IconPrefab.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Skill_Icon/" + cardData.Value.icon);

            icon.cardData = cardData.Value;
            cardlist.Add(tmp);
        }

        scroll2d =  scrollPrefab.GetComponent<ScrollScript2D>();
        scroll2d.animate();
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
                    if(cardlist[i].GetComponent<Skill_Icon>().cardData.type == (int)Card.SkillType.Move)
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
                    if (cardlist[i].GetComponent<Skill_Icon>().cardData.type == (int) Card.SkillType.Attack)
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
                    if (cardlist[i].GetComponent<Skill_Icon>().cardData.type == (int)Card.SkillType.Special)
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
