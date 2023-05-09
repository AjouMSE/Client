using System.Collections;
using System.Collections.Generic;
using Manager;
using Manager.Net;
using TMPro;
using UI.Logo;
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
        private const float CameraMovementEffectSpd = 12f;

        #endregion


        #region Private variables

        [Header("Main Camera Controller")] 
        [SerializeField] private LoginMainCameraController mainCameraController;

        [Header("HUD Loading, Signup UI Controller")] 
        [SerializeField] private HUDLoadingUIController loadingUIController;
        [SerializeField] private HUDLoginSignupUIController signupUIController;

        [Header("Id, Pw InputFields")] 
        [SerializeField] private TMP_InputField inputFieldId;
        [SerializeField] private TMP_InputField inputFieldPw;

        [Header("Information Text")] 
        [SerializeField] private Text textInfo;

        [Header("3D Scroll Sign in UI")] 
        [SerializeField] private ScrollScript3D scroll3DSignin;

        #endregion


        #region Callbacks

        /// <summary>
        /// Callbacks of Sign In button
        /// </summary>
        public void OnSignInBtnClick()
        {
            if (inputFieldId.text.Length == 0)
                UIManager.Instance.ShowInfoText(textInfo, HUDLoginNotify.NotifyEmptyIdField);
            else if (!CustomUtils.IsValidEmail(inputFieldId.text))
                UIManager.Instance.ShowInfoText(textInfo, HUDLoginNotify.NotifyInvalidIdForm);
            else if (inputFieldPw.text.Length == 0)
                UIManager.Instance.ShowInfoText(textInfo, HUDLoginNotify.NotifyEmptyPwField);
            else if (!CustomUtils.IsValidEmail(inputFieldId.text))
                UIManager.Instance.ShowInfoText(textInfo, HUDLoginNotify.NotifyInvalidIdForm);
            else
            {
                Packet.Account account = new Packet.Account
                {
                    email = inputFieldId.text,
                    password = CustomUtils.SHA(inputFieldPw.text, CustomUtils.SHA256)
                };
                string json = JsonUtility.ToJson(account);
                NetHttpRequestManager.Instance.Post(SignInReqPath, json, SignInResultCallback);
            }
        }

        /// <summary>
        /// Callbacks of Sign up button
        /// </summary>
        public void OnSignUpBtnClick()
        {
            // Close sign in scroll and open sign up scroll
            scroll3DSignin.CloseScroll();
            signupUIController.ScrollMoveDown();
        }

        /// <summary>
        /// Callback of sign in result
        /// </summary>
        /// <param name="req"></param>
        private void SignInResultCallback(UnityWebRequest req)
        {
            using (req)
            {
                if (req.result == UnityWebRequest.Result.Success)
                {
                    // Save user information to UserManager
                    string json = req.downloadHandler.text;
                    var user = JsonUtility.FromJson<Packet.User>(json);
                    UserManager.Instance.SignInUserInfo(user);

                    // Process scroll, camera movement effect
                    scroll3DSignin.CloseScroll();
                    mainCameraController.CameraMovementEffect(() =>
                    {
                        loadingUIController.gameObject.SetActive(true);
                        loadingUIController.ShowLoadingUI(() =>
                        {
                            loadingUIController.HideLoadingUI(() =>
                            {
                                UIManager.Instance.ChangeSceneAsync(UIManager.SceneNameLobby);
                            }, false);
                        });
                    }, 10, CameraMovementEffectSpd);
                }
                else if (req.result == UnityWebRequest.Result.ProtocolError)
                {
                    // Occured Error (Account does not exist, Wrong password etc..)
                    UIManager.Instance.ShowInfoText(textInfo, HUDLoginNotify.NotifyInvalidAccount);
                    Debug.Log($"{req.responseCode.ToString()} / {req.error}");
                }
                else
                {
                    // Occured Error (Server connection error)
                    UIManager.Instance.ShowInfoText(textInfo, HUDLoginNotify.NotifyServerError);
                    Debug.Log($"{req.responseCode.ToString()} / {req.error}");
                }
                
                // explicit dispose the resource 
                req.Dispose();
            }
        }

        #endregion
    }
}