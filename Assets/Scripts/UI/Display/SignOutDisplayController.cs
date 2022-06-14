using System.Collections;
using System.Collections.Generic;
using Manager;
using Manager.Net;
using UnityEngine;

namespace UI.Display
{
    public class SignOutDisplayController : MonoBehaviour
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
            NetHttpRequestManager.Instance.Post(SignOutReqPath, "", (req) =>
            {
                UserManager.Instance.SignOutUserInfo();
                NetMatchMakingManager.Instance.Disconnect();
                UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameLogin);
                gameObject.SetActive(false);
            });
        }

        #endregion
    }
}