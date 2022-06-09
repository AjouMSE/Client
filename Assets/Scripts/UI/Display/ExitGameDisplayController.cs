using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace UI.Display
{
    public class ExitGameDisplayController : MonoBehaviour
    {
        #region Button callbacks

        /// <summary>
        /// 
        /// </summary>
        public void OnCancelBtnClick()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnExitGameBtnClick()
        {
#if UNITY_EDITOR
            // todo-UnityEditor.EditorApplication.isPlaying=false;
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #endregion
    }
}