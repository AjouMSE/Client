using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum SkillType { Move = 1, Attack = 100, Special = 200 }
public class Card : MonoBehaviour
{
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text value;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private Image skillicon;

    public CardData skillData;


    private void Start()
    {
        name.text = skillData.text.ToString();
        value.text = skillData.value.ToString();
        cost.text = skillData.cost.ToString();
        //skillicon.sprite = Resources.Load<Sprite>("Image/SkillIcon", skillData.skillIcon);
    }
}
