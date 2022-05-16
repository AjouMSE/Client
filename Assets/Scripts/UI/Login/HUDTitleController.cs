using System.Collections;
using System.Collections.Generic;
using System.Text;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Login
{
    public class HUDTitleController : MonoBehaviour
    {
        #region Static variables
        
        private const int RotCamDirLeft = -1, RotCamDirRight = 1;
        private const int RotAngleMaxLeft = -60, RotAngleMaxRight = 60;
        private const float FadeEffectDuration = 2f;
        
        #endregion
        
        
        #region Private variables
        
        [SerializeField] private Text titleText;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CanvasGroup titleCanvasGroup;
        [SerializeField] private ScrollScript3D signinScroll;
        
        private float _mainCamRotSpd = 6.0f, _mainCamRotAngle = 0;
        private int _rotCamCurrDir = RotCamDirRight;
        
        #endregion

        
        #region Custom methods

        void InitTitle()
        {
            titleText.text = CustomUtils.MakeTitleColor();
            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.MainBgm1, true);
        }

        #endregion

        
        #region Callbacks
        
        public void OnToStartBtnClick()
        {
            titleCanvasGroup.gameObject.SetActive(false);
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
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, titleCanvasGroup, FadeEffectDuration);
        }
        
        private void Update()
        {
            RotateMainCamera();
        }

        #endregion
    }   
}
