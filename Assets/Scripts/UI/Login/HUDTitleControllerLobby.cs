using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Login
{
    public class HUDTitleControllerLobby : MonoBehaviour
    {
        #region Static variables
        
        private const int RotCamDirLeft = -1, RotCamDirRight = 1;
        private const int RotAngleMaxLeft = -60, RotAngleMaxRight = 60;
        
        #endregion
        
        
        #region Private variables
        
        [SerializeField] private Text titleText;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Canvas titleCanvas;
        [SerializeField] private ScrollScript3D signinScroll;
        
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
            titleText.text = sb.ToString();
        }

        #endregion

        
        #region Callbacks
        
        public void OnToStartBtnClick()
        {
            titleCanvas.gameObject.SetActive(false);
            signinScroll.OpenScroll();
        }
        
        #endregion
        

        #region Event methods
        
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
        
        private void Start()
        {
            InitTitle();
        }
        
        private void Update()
        {
            RotateMainCamera();
        }

        #endregion
    }   
}
