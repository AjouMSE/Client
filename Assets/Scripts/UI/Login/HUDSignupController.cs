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
    public class HUDSignupController : MonoBehaviour
    {
        #region Private variables

        private const int MinScrollYPos = 0, MaxScrollYPos = 240;
        private const string SignUpReqPath = "/user/sign-up";

        [SerializeField] private TMP_InputField inputFieldId, inputFieldPw;
        [SerializeField] private TMP_InputField inputFieldPwConfirm, inputFieldNickname;
        [SerializeField] private Text textInformation;
        [SerializeField] private GameObject scrollSignup, scrollSignin;

        #endregion


        #region Public variables

        public enum ScrollMoveType
        {
            Down,
            Up
        }

        #endregion
        

        #region Callbacks
        
        private void SignupReqCallback(UnityWebRequest req)
        {
            string jsonPayload = req.downloadHandler.text;
            if (req.result == UnityWebRequest.Result.Success)
            {
                // close the scroll
                scrollSignup.GetComponent<ScrollScript2D>().ScrollClose();
                ScrollMoveUp();
            }
            else
            {
                Packet.WebServerException exception = JsonUtility.FromJson<Packet.WebServerException>(jsonPayload);
                Debug.Log(exception.ToString());
                textInformation.text = exception.message;
            }
        }

        private void ScrollMoveDownCallback()
        {
            scrollSignup.GetComponent<ScrollScript2D>().ScrollOpen();
        }

        private void ScrollMoveUpCallback()
        {
            scrollSignin.GetComponent<ScrollScript3D>().OpenScroll();
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
                HttpRequestManager.Instance.Post(SignUpReqPath, json, SignupReqCallback);
            }
        }

        #endregion


        #region Custom methods

        public void ScrollMoveDown()
        {
            StartCoroutine(ScrollMovement(ScrollMoveType.Down, ScrollMoveDownCallback));
        }

        public void ScrollMoveUp()
        {
            StartCoroutine(ScrollMovement(ScrollMoveType.Up, ScrollMoveUpCallback));
        }

        #endregion


        #region Coroutines

        IEnumerator ScrollMovement(ScrollMoveType type, Action callback = null)
        {
            RectTransform rectTransform = scrollSignup.GetComponent<RectTransform>();
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

        #endregion
    }
}
