using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    [SerializeField] private Text name;
    [SerializeField] private Text value;

    public CardData skilldata;


    private void Start()
    {
        name.text = skilldata.text.ToString();
        value.text = skilldata.value.ToString();
    }
}
