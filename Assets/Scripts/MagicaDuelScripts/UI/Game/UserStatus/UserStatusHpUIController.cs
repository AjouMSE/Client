using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Game.UserStatus
{
    public class UserStatusHpUIController : MonoBehaviour
    {
        #region private constant

        private const int MaxWidth = 300;

        #endregion


        #region Private variable

        [SerializeField] private bool isHostUI;

        private Image _imageHpBar;
        private RectTransform _rectTransform;
        private Text _textHpValue;

        #endregion


        #region Unity event methods

        private void Awake()
        {
            _imageHpBar = GetComponentsInChildren<Image>()[0];
            _rectTransform = _imageHpBar.GetComponent<RectTransform>();
            _textHpValue = GetComponentInChildren<Text>();
            UpdateHpUI(Consts.DefaultHp);
        }

        #endregion

        #region Public methods

        public void UpdateHpUI(int currentHp)
        {
            if (currentHp > Consts.MaximumHp || currentHp < 0) return;
            if (_rectTransform == null) return;
            
            var size = _rectTransform.sizeDelta;
            size.x = MaxWidth * (currentHp / (float)Consts.MaximumHp);
            _rectTransform.sizeDelta = size;
            _textHpValue.text = currentHp.ToString();
        }

        #endregion
    }
}