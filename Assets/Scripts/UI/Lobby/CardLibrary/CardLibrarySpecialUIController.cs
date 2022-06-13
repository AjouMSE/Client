using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Lobby.CardLibrary;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Lobby.CardLibrary
{
    public class CardLibrarySpecialUIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Private constants

        private const string SpecialBurn = "S_Burn";
        private const string SpecialHeal = "S_Heal";
        private const string SpecialShield = "S_Shield";
        private const string SpecialManaOverload = "S_ManaOverload";
        private const string SpecialConcentration = "S_Concentration";
        private const string SpecialManaDisorder = "S_ManaDisorder";
        private const string SpecialManaCharge = "S_ManaCharge";
        private const string SpecialCleanse = "S_Cleanse";
        private const string SpecialBind = "S_Bind";
        private const string SpecialStun = "S_Stun";

        #endregion
        
        
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
                if (explainSpecial != null)
                    explainSpecial.SetActive(true);

                switch (_specialName)
                {
                    case SpecialBurn:
                        explainText.text = "3 Damage to HP per phase, Maintain for 3 phase";
                        break;
                    case SpecialHeal:
                        explainText.text = "Recover 20 HP";
                        break;
                    case SpecialShield:
                        explainText.text = "Reduce enemy skill damage by 50";
                        break;
                    case SpecialManaOverload:
                        explainText.text = "Increase skill damage used in next phase by 1.5x";
                        break;
                    case SpecialConcentration:
                        explainText.text = "Reduce skill cost that next using skillby 5, Maintain for 3 phase";
                        break;
                    case SpecialManaDisorder:
                        explainText.text = "Increase skill cost that next using skill by 1.5x, Maintain for 3 phase";
                        break;
                    case SpecialManaCharge:
                        explainText.text = "Recover 2 Mana cost";
                        break;
                    case SpecialCleanse:
                        explainText.text = "Remove all debuff";
                        break;
                    case SpecialBind:
                        explainText.text = "Movement card not available, Maintain for 2 phase";
                        break;
                    case SpecialStun:
                        explainText.text = "Cancel skill in next phase";
                        break;
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (explainSpecial != null)
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
}