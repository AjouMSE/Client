using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Lobby.CardLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardLibrarySpecialCardUIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Private variables

    [SerializeField] private CardLibraryCardUIController cardUIController;
    [SerializeField] private GameObject explainSpecial;
    [SerializeField] private TMP_Text explainText;
    
    private string _specialName;
    private Image _specialImage;

    #endregion


    #region Unity event methods
    
    void Start()
    {
        Init();
    }

    void Update()
    {
        if (_specialImage.sprite != null)
            _specialName = _specialImage.sprite.name;
        else
            _specialName = null;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_specialName != null)
        {
            explainSpecial.SetActive(true);
            switch (_specialName)
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

    #endregion

    #region Private methods

    private void Init()
    {
        _specialImage = GetComponent<Image>();
    }

    #endregion
}