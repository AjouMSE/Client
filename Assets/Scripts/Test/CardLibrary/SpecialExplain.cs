using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SpecialExplain : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Card cardPrefab;
    [SerializeField] private GameObject explainSpecial;
    [SerializeField] private TMP_Text explainText;
    private string getSpecialName;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.gameObject.GetComponent<Image>().sprite != null)
            getSpecialName = transform.gameObject.GetComponent<Image>().sprite.name;
        else
            getSpecialName = null;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(getSpecialName != null)
        {
            explainSpecial.SetActive(true);

            switch (getSpecialName)
            {
                case "S_Burn":
                    explainText.text = "3 Damage to HP per phase, Maintain for 3 phase";
                    break;
                case "S_Heal":
                    explainText.text = "Recover 20 HP";
                    break;
                case "S_Shield":
                    explainText.text = "Reduce enemy skill damage by 50";
                    break;
                case "S_ManaOverload":
                    explainText.text = "Increase skill damage used in next phase by 1.5x";
                    break;
                case "S_Concentration":
                    explainText.text = "Reduce skill cost that next using skillby 5, Maintain for 3 phase";
                    break;
                case "S_ManaDisorder":
                    explainText.text = "Increase skill cost that next using skill by 1.5x, Maintain for 3 phase";
                    break;
                case "S_ManaCharge":
                    explainText.text = "Recover 2 Mana cost";
                    break;
                case "S_Cleanse":
                    explainText.text = "Remove all debuff";
                    break;
                case "S_Bind":
                    explainText.text = "Movement card not available, Maintain for 2 phase";
                    break;
                case "S_Stun":
                    explainText.text = "Cancel skill in next phase";
                    break;

            }
        }
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        explainSpecial.SetActive(false);
    }
}
