using System.Collections;
using System.Collections.Generic;
using System.Text;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Login
{
    public class HUDLoginTitleUIController : MonoBehaviour
    {
        #region Private constants

        private const string HudNameTitle = "HUD_Title";
            
        private const int RotCamDirLeft = -1, RotCamDirRight = 1;
        private const int RotAngleMaxLeft = -60, RotAngleMaxRight = 60;
        private const float FadeInDuration = 1.5f, FadeOutDuration = 0.5f;
        
        #endregion
        
        
        #region Private variables
        
        [Header("Camera")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera hudCamera;
        
        [Header("Title Text")]
        [SerializeField] private Text titleText;

        [Header("3D Scroll UI")]
        [SerializeField] private ScrollScript3D signinScroll;

        private CanvasGroup _titleCvsGroup;
        private float _mainCamRotSpd = 6.0f, _mainCamRotAngle = 0;
        private int _rotCamCurrDir = RotCamDirRight;
        
        #endregion

        
        #region Custom methods

        void InitUI()
        {
            _titleCvsGroup = hudCamera.transform.Find(HudNameTitle).GetComponent<CanvasGroup>();
            titleText.text = CustomUtils.MakeTitleColor();
            AudioManager.Instance.PlayBgm(AudioManager.BgmTypes.MainBgm1, true);
        }

        #endregion

        
        #region Callbacks
        
        private void TitleFadeOutCallback()
        {
            _titleCvsGroup.gameObject.SetActive(false);
            signinScroll.OpenScroll();
        }
        
        public void OnToStartBtnClick()
        {
            //todo-Have to fix a bug where SpriteRenderer is not affected by CanvasGroup's alpha value
            //UIManager.Instance.Fade(UIManager.FadeType.FadeOut, _titleCvsGroup, FadeOutDuration, TitleFadeOutCallback);
            _titleCvsGroup.gameObject.SetActive(false);
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
            InitUI();
            UIManager.Instance.Fade(UIManager.FadeType.FadeIn, _titleCvsGroup, FadeInDuration);
        }
        
        private void Update()
        {
            RotateMainCamera();
        }

        #endregion
    }   
}
