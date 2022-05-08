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
    public class HUDSignupController : MonoBehaviour
    {
        #region Private variables

        [SerializeField] private TMP_InputField inputFieldId, inputFieldPw;
        [SerializeField] private TMP_InputField inputFieldPwConfirm, inputFieldNickname;
        [SerializeField] private Text textInformation;

        #endregion
        

        #region Callbacks
        
        private void SignupReqCallback(UnityWebRequest req)
        {
            Debug.Log(req.downloadHandler.text);
            if (req.result == UnityWebRequest.Result.Success) {
                
            }
            else {
                Debug.Log(req.error);
            }
        }

        public void OnSubmitBtnClick()
        {
            if (inputFieldId.text.Length == 0)
                textInformation.text = LoginSceneHUDNotify.NotifyEmptyIdField;
            else if (!CustomUtils.IsValidEmail(inputFieldId.text))
                textInformation.text = LoginSceneHUDNotify.NotifyInvalidIdForm;
            else if (inputFieldPw.text.Length == 0)
                textInformation.text = LoginSceneHUDNotify.NotifyEmptyPwField;
            else if (inputFieldPwConfirm.text.Length == 0)
                textInformation.text = LoginSceneHUDNotify.NotifyEmptyPwConfirmField;
            else if (!inputFieldPw.text.Equals(inputFieldPwConfirm.text))
                textInformation.text = LoginSceneHUDNotify.NotifyPwMismatch;
            else if (inputFieldNickname.text.Length == 0)
                textInformation.text = LoginSceneHUDNotify.NotifyEmptyNicknameField;
            else
            {
                Packet.Account account = new Packet.Account
                {
                    email = inputFieldId.text,
                    password = CustomUtils.SHA(inputFieldPw.text, CustomUtils.SHA256),
                    nickname = inputFieldNickname.text
                };
                string json = JsonUtility.ToJson(account);
                HttpRequestManager.Instance.Post("/user/sign-up", json, SignupReqCallback);
            }
        }

        #endregion
    }
}
