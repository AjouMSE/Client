using System;
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
    public class HUDLoginSignupUIController : MonoBehaviour
    {
        #region Private constants
        
        private const int MinScrollYPos = 0, MaxScrollYPos = 240;
        private const string SignUpReqPath = "/user/sign-up";
        
        #endregion
        
        
        #region Private variables

        [Header("Account Information InputFields")] 
        [SerializeField] private TMP_InputField inputFieldId;
        [SerializeField] private TMP_InputField inputFieldPw;
        [SerializeField] private TMP_InputField inputFieldPwConfirm;
        [SerializeField] private TMP_InputField inputFieldNickname;
        
        [Header("Information Text")]
        [SerializeField] private TextMeshProUGUI textInformation;

        [Header("2D Scroll Sign up UI")] 
        [SerializeField] private ScrollScript2D scroll2DSignup;

        [Header("3D Scroll Sign in UI")] 
        [SerializeField] private ScrollScript3D scroll3DSignin;
        
        

        private enum ScrollMoveType
        {
            Down,
            Up
        }

        #endregion
        
        
        #region Callbacks
        
        /// <summary>
        /// Callback for sign up request
        /// </summary>
        /// <param name="req"></param>
        private void SignupReqCallback(UnityWebRequest req)
        {
            string jsonPayload = req.downloadHandler.text;
            if (req.result == UnityWebRequest.Result.Success)
            {
                // close the sign up scroll
                scroll2DSignup.ScrollClose();
                ScrollMoveUp();
            }
            else
            {
                // Fail to sign up
                Packet.WebServerException exception = JsonUtility.FromJson<Packet.WebServerException>(jsonPayload);
                ShowInformation(exception.message);
                Debug.Log(exception.ToString());
            }
        }
        
        /// <summary>
        /// Callback for scroll2D move down behavior
        /// </summary>
        private void ScrollMoveDownCallback()
        {
            scroll2DSignup.ScrollOpen();
        }

        /// <summary>
        /// Callback for scroll2D move up behavior
        /// </summary>
        private void ScrollMoveUpCallback()
        {
            scroll3DSignin.OpenScroll();
        }

        /// <summary>
        /// Callback for submit button
        /// </summary>
        public void OnSubmitBtnClick()
        {
            if (inputFieldId.text.Length == 0)
                ShowInformation(HUDLoginNotify.NotifyEmptyIdField);
            else if (!CustomUtils.IsValidEmail(inputFieldId.text))
                ShowInformation(HUDLoginNotify.NotifyInvalidIdForm);
            else if (inputFieldPw.text.Length == 0)
                ShowInformation(HUDLoginNotify.NotifyEmptyPwField);
            else if (inputFieldPwConfirm.text.Length == 0)
                ShowInformation(HUDLoginNotify.NotifyEmptyPwConfirmField);
            else if (!inputFieldPw.text.Equals(inputFieldPwConfirm.text))
                ShowInformation(HUDLoginNotify.NotifyPwMismatch);
            else if (inputFieldNickname.text.Length == 0)
                ShowInformation(HUDLoginNotify.NotifyEmptyNicknameField);
            else
            {
                Packet.Account account = new Packet.Account
                {
                    email = inputFieldId.text,
                    password = CustomUtils.SHA(inputFieldPw.text, CustomUtils.SHA256),
                    nickname = inputFieldNickname.text
                };
                string json = JsonUtility.ToJson(account);
                HttpRequestManager.Instance.Post(SignUpReqPath, json, SignupReqCallback);
            }
        }

        /// <summary>
        /// Callback for back button
        /// </summary>
        public void OnBackBtnClick()
        {
            scroll2DSignup.ScrollClose();
            ScrollMoveUp();
        }

        #endregion


        #region Custom methods
        private void ShowInformation(string text)
        {
            StopCoroutine(ClearInformationText());
            StartCoroutine(ClearInformationText());
            textInformation.text = text;
        }
        
        private void ScrollMoveUp()
        {
            StartCoroutine(ScrollMovement(ScrollMoveType.Up, ScrollMoveUpCallback));
        }

        public void ScrollMoveDown()
        {
            StartCoroutine(ScrollMovement(ScrollMoveType.Down, ScrollMoveDownCallback));
        }
        
        #endregion


        #region Coroutines

        IEnumerator ScrollMovement(ScrollMoveType type, Action callback = null)
        {
            RectTransform rectTransform = scroll2DSignup.gameObject.GetComponent<RectTransform>();
            switch (type)
            {
                case ScrollMoveType.Down:
                    while (rectTransform.anchoredPosition.y > MinScrollYPos)
                    {
                        Vector2 tmpPos = rectTransform.anchoredPosition;
                        tmpPos.y -= MaxScrollYPos * Time.deltaTime;
                        rectTransform.anchoredPosition = tmpPos;
                        yield return null;
                    }
                    if(callback != null) callback();
                    break;
                
                case ScrollMoveType.Up:
                    while (rectTransform.anchoredPosition.y < MaxScrollYPos)
                    {
                        Vector2 tmpPos = rectTransform.anchoredPosition;
                        tmpPos.y += MaxScrollYPos * Time.deltaTime;
                        rectTransform.anchoredPosition = tmpPos;
                        yield return null;
                    }
                    if(callback != null) callback();
                    break;
                
                default:
                    Debug.LogError("UndefinedScrollMoveTypeException");
                    yield break;
            }
        }
        
        IEnumerator ClearInformationText()
        {
            yield return new WaitForSeconds(2f);
            textInformation.text = "";
        }

        #endregion
    }
}
