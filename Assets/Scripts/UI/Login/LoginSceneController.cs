using System.Collections;
using System.Collections.Generic;
using System.Text;
using Manager;
using MiniJSON;
using Test.Networking;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils;

namespace Scene
{
    public class LoginSceneController : MonoBehaviour
    {
        #region Static variables
        private const int RotCamDirLeft = -1, RotCamDirRight = 1;
        private const int RotAngleMaxLeft = -60, RotAngleMaxRight = 60;
        
        private const string NotifyEmptyIdField = "Please enter your Id.";
        private const string NotifyEmptyPwField = "Please enter your Password.";
        private const string NotifyInvalidIdForm = "Invalid Id form. Enter it like 'ooo@ooooo.ooo'";
        private const string NotifyInvalidAccount = "Id or Password do not match.";
        
        #endregion
        
        
        #region Private instance variables
        // HUD 2D Title UI
        [SerializeField] private Text textTitle;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Canvas canvasHUD2D;
        
        // HUD 2D Sign-up UI
        
        // HUD 3D Sign-in UI
        [SerializeField] private TMP_InputField inputFieldId, inputFieldPw;
        [SerializeField] private Text textInformation;
        [SerializeField] private GameObject scroll2DVertical, scroll3DTall;
        

        private float _mainCamRotSpd = 6.0f, _mainCamRotAngle = 0;
        private int _rotCamCurrDir = RotCamDirRight;

        #endregion


        #region Init methods
        
        void InitTitle()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(CustomUtils.GenColorText("m", 140, 128, 255));
            sb.Append('a');
            sb.Append(CustomUtils.GenColorText("g", 140, 128, 255));
            sb.Append('i');
            sb.Append(CustomUtils.GenColorText("c", 140, 128, 255));
            sb.Append("a ");
            sb.Append(CustomUtils.GenColorText("d", 140, 128, 255));
            sb.Append('u');
            sb.Append(CustomUtils.GenColorText("e", 140, 128, 255));
            sb.Append('l');
            textTitle.text = sb.ToString();
        }
        
        #endregion
        
        #region Action callbacks

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
        
        
        #endregion

        #region Button Click Listener

        public void OnToStartBtnClick()
        {
            canvasHUD2D.gameObject.SetActive(false);
            ScrollScript3D scroll3D = scroll3DTall.GetComponent<ScrollScript3D>();
            scroll3D.OpenScroll();
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
            ScrollScript2D scroll2D = scroll2DVertical.GetComponent<ScrollScript2D>();
            ScrollScript3D scroll3D = scroll3DTall.GetComponent<ScrollScript3D>();
            scroll2D.animate();
            scroll3D.CloseScroll();
        }

        #endregion
        
        #region Custom Methods

        private void RotateMainCamera()
        {
            if (_mainCamRotAngle > RotAngleMaxRight) 
                _rotCamCurrDir = RotCamDirLeft;
            if (_mainCamRotAngle < RotAngleMaxLeft)
                _rotCamCurrDir = RotCamDirRight;

            float rotValue = _mainCamRotSpd * _rotCamCurrDir * Time.deltaTime;
            _mainCamRotAngle += rotValue;
            mainCamera.transform.Rotate(Vector3.up * rotValue);
        }
        
        #endregion
        

        #region Unity Event methods
        
        void Start()
        {
            // Init title text
            InitTitle();
        }
        
        void Update()
        {
            // Rotate main camera
            RotateMainCamera();
        }
        
        #endregion
    }
}
