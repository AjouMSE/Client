using System.Collections;
using System.Collections.Generic;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI.Login
{
    public class HUDSigninController : MonoBehaviour
    {
        #region Private variables

        private const string DestSceneName = "LobbyScene";
        
        [SerializeField] private TMP_InputField inputFieldId, inputFieldPw;
        [SerializeField] private Text textInformation;
        [SerializeField] private GameObject scrollSignup, scrollSignin;

        #endregion


        #region Callbacks
        
        public void SignInResultCallback(UnityWebRequest req)
        {
            if (req.result == UnityWebRequest.Result.Success) {
                // Init user info
                string json = req.downloadHandler.text;
                UserManager.Instance.InitUserInfo(json);
                SceneManager.LoadScene(DestSceneName);
            }
            else
            {
                // Occured Error (Account does not exist, Wrong password etc..)
                textInformation.text = LoginSceneHUDNotify.NotifyInvalidAccount;
                Debug.Log(req.error);
            }
        }

        public void OnSignInBtnClick()
        {
            if (inputFieldId.text.Length == 0)
                textInformation.text = LoginSceneHUDNotify.NotifyEmptyIdField;
            else if (inputFieldPw.text.Length == 0)
                textInformation.text = LoginSceneHUDNotify.NotifyEmptyPwField;
            else if (!CustomUtils.IsValidEmail(inputFieldId.text))
                textInformation.text = LoginSceneHUDNotify.NotifyInvalidIdForm;
            else
            {
                Packet.Account account = new Packet.Account { 
                    email = inputFieldId.text, 
                    password = CustomUtils.SHA(inputFieldPw.text, CustomUtils.SHA256)
                };
                string json = JsonUtility.ToJson(account);
                HttpRequestManager.Instance.Post("/user/sign-in", json, SignInResultCallback);
            }
        }
        
        public void OnSignUpBtnClick()
        {
            ScrollScript2D scroll2D = scrollSignup.GetComponent<ScrollScript2D>();
            ScrollScript3D scroll3D = scrollSignin.GetComponent<ScrollScript3D>();
            scroll2D.ScrollDown();
            scroll3D.CloseScroll();
        }

        #endregion
    }
}
