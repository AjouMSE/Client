using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Game.UserStatus
{
    public class UserStatusManaUIController : MonoBehaviour
    {
        #region private constant

        private const int MaxWidth = 300;

        #endregion


        #region Private variable

        [SerializeField] private bool isHostUI;

        private Image _imageManaBar;
        private RectTransform _rectTransform;
        private Text _textManaValue;

        #endregion


        #region Unity event methods

        private void Start()
        {
            gameObject.SetActive(!(isHostUI ^ UserManager.Instance.IsHost));
            _imageManaBar = GetComponentsInChildren<Image>()[0];
            _rectTransform = _imageManaBar.GetComponent<RectTransform>();
            _textManaValue = GetComponentInChildren<Text>();
        }

        #endregion

        #region Public methods

        public void UpdateManaUI(int currentMana)
        {
            if (currentMana > Consts.MaximumMana || currentMana < 0) return;

            var size = _rectTransform.sizeDelta;
            size.x = MaxWidth * (currentMana / (float)Consts.MaximumMana);
            _rectTransform.sizeDelta = size;

            _textManaValue.text = currentMana.ToString();
        }

        #endregion
    }
}