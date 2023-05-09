using System.Collections;
using System.Collections.Generic;
using Cache;
using Cache;
using DataTable;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI.Lobby.CardLibrary
{
    public class CardLibraryCardUIController : MonoBehaviour
    {
        #region Private variables

        [Header("Card Information TMPro")]
        [SerializeField] private TMP_Text tmProCardName;
        [SerializeField] private TMP_Text tmProCardValue;
        [SerializeField] private TMP_Text tmProCardCost;
        
        [Header("Icon Images")]
        [SerializeField] private Image imageSkillIcon;
        [SerializeField] private Image specialIcon;
        
        [Header("Grid Parent Object")]
        [SerializeField] private GameObject skillRangeGridObj;

        private Image[] _gridImages;

        #endregion


        #region Public variables

        public RectTransform RectTrans { get; private set; }

        #endregion


        #region Unity event methods

        private void Start()
        {
            Init();
        }

        #endregion


        #region Private variables

        private void Init()
        {
            _gridImages = skillRangeGridObj.GetComponentsInChildren<Image>();
            RectTrans = GetComponent<RectTransform>();
            if(SceneManager.GetActiveScene().name.Equals(UIManager.SceneNameInGame))
                RectTrans.localPosition = new Vector3(UserManager.Instance.IsHost ? 780 : -780, -1000, 0);
            IsEmptyCard(false);
        }

        private void IsEmptyCard(bool empty)
        {
            if (empty)
            {
                imageSkillIcon.color = new Color(255, 255, 255, 255);
                //rangeBoxBackground.color = new Color(0, 0, 0, 255);
                specialIcon.color = new Color(255, 255, 255, 255);
            }
            else
            {
                imageSkillIcon.color = new Color(255, 255, 255, 0);
                //rangeBoxBackground.color = new Color(0, 0, 0, 0);
                specialIcon.color = new Color(255, 255, 255, 0);
            }
        }

        #endregion


        #region Public variables

        public void SetData(CardData skillData)
        {
            IsEmptyCard(true);

            tmProCardName.text = skillData.text;
            tmProCardValue.text = skillData.value.ToString();
            tmProCardCost.text = skillData.cost.ToString();
            imageSkillIcon.sprite = CacheSpriteSource.Instance.GetSource(skillData.code);

            // Set grid color
            BitMask.BitField25 skillRange = new BitMask.BitField25(skillData.range);
            var mask = BitMask.BitField25Msb;
            for (int i = 0; i < _gridImages.Length; i++)
            {
                if ((skillRange.element & mask) > 0)
                    _gridImages[i].color = new Color(1, 0.3f, 0);
                else
                    _gridImages[i].color = new Color(1, 0.9f, 0.9f);

                mask >>= 1;
            }
            
            if (skillData.special == 0)
            {
                // Hide special icon
                specialIcon.color = new Color(255, 255, 255, 0);
            }
            else
            {
                // show special icon
                specialIcon.sprite = CacheSpriteSource.Instance.GetSource(skillData.special);
                specialIcon.color = new Color(255, 255, 255, 255);
            }
        }

        #endregion
    }
}