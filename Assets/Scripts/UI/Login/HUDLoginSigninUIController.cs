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
    public class HUDLoginSigninUIController : MonoBehaviour
    {
        #region Private constants

        private const string SignInReqPath = "/user/sign-in";
        private const string DestSceneName = "LobbyScene";
        
        #endregion
        
        
        #region Private variables

        [Header("Id, Pw InputFields")]
        [SerializeField] private TMP_InputField inputFieldId;
        [SerializeField] private TMP_InputField inputFieldPw;
        
        [Header("Information Text")]
        [SerializeField] private Text textInformation;
        
        [Header("3D Scroll Sign in UI")]
        [SerializeField] private ScrollScript3D scroll3DSignin;

        #endregion


        #region Callbacks

        /// <summary>
        /// Callback of sign in result
        /// </summary>
        /// <param name="req"></param>
        private void SignInResultCallback(UnityWebRequest req)
        {
            if (req.result == UnityWebRequest.Result.Success)
            {
                // Save user information to UserManager
                string json = req.downloadHandler.text;
                UserManager.Instance.SignInUserInfo(json);
                SceneManager.LoadSceneAsync(DestSceneName);
            }
            else
            {
                // Occured Error (Account does not exist, Wrong password etc..)
                ShowInformation(HUDLoginNotify.NotifyInvalidAccount);
                Debug.Log(req.error);
            }
        }

        /// <summary>
        /// Callbacks of Sign In button
        /// </summary>
        public void OnSignInBtnClick()
        {
            StopCoroutine(ClearInformationText());
            StartCoroutine(ClearInformationText());
            
            if (inputFieldId.text.Length == 0)
                ShowInformation(HUDLoginNotify.NotifyEmptyIdField);
            else if (inputFieldPw.text.Length == 0)
                ShowInformation(HUDLoginNotify.NotifyEmptyPwField);
            else if (!CustomUtils.IsValidEmail(inputFieldId.text))
                ShowInformation(HUDLoginNotify.NotifyInvalidIdForm);
            else
            {
                Packet.Account account = new Packet.Account
                {
                    email = inputFieldId.text,
                    password = CustomUtils.SHA(inputFieldPw.text, CustomUtils.SHA256)
                };
                string json = JsonUtility.ToJson(account);
                HttpRequestManager.Instance.Post(SignInReqPath, json, SignInResultCallback);
            }
        }

        /// <summary>
        /// Callbacks of Sign up button
        /// </summary>
        public void OnSignUpBtnClick()
        {
            // Close sign in scroll and open sign up scroll
            scroll3DSignin.CloseScroll();
            GetComponent<HUDLoginSignupUIController>().ScrollMoveDown();
        }

        #endregion
        
        
        
        #region Custom methods

        private void ShowInformation(string text)
        {
            StopCoroutine(ClearInformationText());
            StartCoroutine(ClearInformationText());
            textInformation.text = text;
        }
        
        #endregion

        
        #region Coroutines

        IEnumerator ClearInformationText()
        {
            yield return new WaitForSeconds(2f);
            textInformation.text = "";
        }

        #endregion
    }
}