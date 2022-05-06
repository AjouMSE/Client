using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillType { Move = 1, Attack = 100, Special = 200 }
public class Card : MonoBehaviour
{
    [SerializeField] private Text name;
    [SerializeField] private Text value;

    public CardData skillData;


    private void Start()
    {
        name.text = skillData.text.ToString();
        value.text = skillData.value.ToString();
    }
}
