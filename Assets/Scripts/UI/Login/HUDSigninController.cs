using System.Collections;
using System.Collections.Generic;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils;

namespace UI.Login
{
    public class HUDSigninController : MonoBehaviour
    {
        #region Static variables
        private const string NotifyEmptyIdField = "Please enter your Id.";
        private const string NotifyEmptyPwField = "Please enter your Password.";
        private const string NotifyInvalidIdForm = "Invalid Id form. Enter it like 'ooo@ooooo.ooo'";
        private const string NotifyInvalidAccount = "Id or Password do not match.";
        
        #endregion


        #region Private variables
        [SerializeField] private TMP_InputField inputFieldId, inputFieldPw;
        [SerializeField] private Text textInformation;
        [SerializeField] private GameObject scrollSignup, scrollSignin;

        #endregion


        #region Callbacks
        
        public void SignInResultCallback(UnityWebRequest req)
        {
            if (req.result == UnityWebRequest.Result.Success) {
                // Show results as text
                Debug.Log(req.downloadHandler.text);
 
                // Or retrieve results as binary data
                byte[] results = req.downloadHandler.data;
                
                //todo-process result of sign-in post request
            }
            else {
                Debug.Log(req.error);
            }
        }

        public void OnSignInBtnClick()
        {
            if (inputFieldId.text.Length == 0)
                textInformation.text = NotifyEmptyIdField;
            else if (inputFieldPw.text.Length == 0)
                textInformation.text = NotifyEmptyPwField;
            else if (!CustomUtils.IsValidEmail(inputFieldId.text))
                textInformation.text = NotifyInvalidIdForm;
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
            scroll2D.animate();
            scroll3D.CloseScroll();
        }

        #endregion
    }
}
