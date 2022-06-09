using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Display
{
    public class PerformanceDisplayController : MonoBehaviour
    {
        #region Private variables

        private Text _frameRateText;

        #endregion


        #region Unity event methods

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            ShowFrameRate();
        }

        #endregion


        #region Private variables

        private void Init()
        {
            _frameRateText = GetComponentInChildren<Text>();
            
        }

        private void ShowFrameRate()
        {
            int frameRate = (int)(1 / Time.deltaTime);
            _frameRateText.text = $"FPS : {frameRate.ToString()}";
        }

        #endregion
    }   
}
