using System.Collections;
using System.Collections.Generic;
using Cache;
using UnityEngine;
using UnityEngine.UI;
using Utils;


namespace UI.Lobby.Settings
{
    public class SettingsCategoryButtonSubController : MonoBehaviour
    {
        #region Private variables

        private Image _buttonImage;
        private RectTransform _buttonTextRectTransform;

        #endregion


        #region Unity event methods

        private void Start()
        {
            Init();
        }

        #endregion


        #region Private methods

        private void Init()
        {
            _buttonImage = GetComponent<Image>();
            _buttonTextRectTransform = CustomUtils.FindComponentByName<RectTransform>(gameObject, "Text");
        }

        #endregion


        #region Public methods

        public void ChangeStatus(bool isPressed)
        {
            _buttonImage.sprite = CacheSpriteSource.Instance.GetSource(isPressed ? 3 : 2);
            _buttonTextRectTransform.offsetMin = _buttonTextRectTransform.offsetMax =
                isPressed ? new Vector2(0, -6) : new Vector2(0, 10);
        }

        #endregion
    }
}