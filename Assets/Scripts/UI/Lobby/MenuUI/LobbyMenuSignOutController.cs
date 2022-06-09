using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace UI.Lobby.MenuUI
{
    public class LobbyMenuSignOutController : MonoBehaviour
    {
        #region Private constants

        private const string SignOutReqPath = "/user/sign-out";

        #endregion


        #region Private variables

        [Header("3D Scroll Menu UI")] 
        [SerializeField] private ScrollScript3D scroll3D;

        #endregion


        #region Button callbacks

        /// <summary>
        /// 
        /// </summary>
        public void OnCancelBtnClick()
        {
            scroll3D.OpenScroll();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnSignOutBtnClick()
        {
            HttpRequestManager.Instance.Post(SignOutReqPath, "", (req) =>
            {
                UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameLogin);
                UserManager.Instance.SignOutUserInfo();
                gameObject.SetActive(false);
            });
        }

        #endregion
    }
}