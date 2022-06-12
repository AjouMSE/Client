using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    public enum SkillType { Move = 1, Attack = 100, Special = 200 }
    
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text value;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private Image skillicon;
    [SerializeField] private GameObject rangeBoxParent;
    [SerializeField] private Image rangeBoxBackground;
    [SerializeField] private Image[] rangeBoxes = new Image[25];
    [SerializeField] private Image special_Icon;



    private void Start()
    {
        isCardDataNull(false);
    }

    public void SetData(CardData skillData)
    {
        isCardDataNull(true);


        rangeBoxes = rangeBoxParent.GetComponentsInChildren<Image>();

        name.text = skillData.text.ToString();
        value.text = skillData.value.ToString();
        cost.text = skillData.cost.ToString();
        skillicon.sprite = Resources.Load<Sprite>("Image/Skill_Icon/" + skillData.icon);

        int[] skillRangeCheck = new int[25];

        int tmp = 0;

        for (int i = 0; i < skillData.range.Length; i++)
        {
            if (skillData.range[i] == '1')
            {
                rangeBoxes[tmp].color = Color.red;
                tmp++;
            }
            else if (skillData.range[i] == '0')
            {
                rangeBoxes[tmp].color = new Color(255, 189, 0);
                tmp++;
            }
        }
        if(skillData.special == 0)
        {
            special_Icon.color = new Color(255, 255, 255, 0);
        }
        else if(skillData.special != 0)
        {
            special_Icon.sprite = Resources.Load<Sprite>("Image/Skill_Icon/" + TableDatas.Instance.GetSpecialData(skillData.special).icon);
            special_Icon.color = new Color(255, 255, 255, 255);
        }


    }

    private void isCardDataNull(bool boolean)
    {
        if (boolean)
        {
            skillicon.color = new Color(255, 255, 255, 255);
            rangeBoxBackground.color = new Color(0, 0, 0, 255);
            special_Icon.color = new Color(255, 255, 255, 255);
        }
        else if (!boolean)
        {
            skillicon.color = new Color(255, 255, 255, 0);
            rangeBoxBackground.color = new Color(0, 0, 0, 0);
            special_Icon.color = new Color(255, 255, 255, 0);
        }
    }
}
