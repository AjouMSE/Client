using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Display
{
    public class DuplicateLoginDisplayController : MonoBehaviour
    {
        #region Unity event methods

        private void Awake()
        {
            if (SceneManager.GetActiveScene().name.Equals(UIManager.SceneNameLogin))
            {
                gameObject.SetActive(UIManager.Instance.DuplicateLoginOccur);
                UIManager.Instance.SetDuplicateLoginDisplay(gameObject);
            }
        }

        #endregion

        #region Button callbacks

        /// <summary>
        /// 
        /// </summary>
        public void OnOkBtnClick()
        {
            gameObject.SetActive(false);
        }

        #endregion
    }
}