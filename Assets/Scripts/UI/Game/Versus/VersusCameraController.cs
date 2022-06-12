using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using Utils;

namespace UI.Game.Versus
{
    public class VersusCameraController : MonoBehaviour
    {
        #region Private constants

        private const float MinMainCameraAngle = -150;
        private const float MaxMainCameraAngle = 30;
        private const float RotationScale = 120f;

        #endregion

        #region Private variables
        
        private float _mainCameraXAngle;

        #endregion

        #region Public methods

        public void RotateCameraEffect(Action callback)
        {
            StartCoroutine(RotateCamera(callback));
        }

        #endregion


        #region Unity event methods

        private void Start()
        {
            Init();
        }

        #endregion


        #region Private methods

        private void Init()
        {
            _mainCameraXAngle = MinMainCameraAngle;
        }

        #endregion


        #region Coroutines

        /// <summary>
        /// Rotate main camera to show battle field
        /// </summary>
        /// <returns></returns>
        private IEnumerator RotateCamera(Action callback)
        {
            // Camera rotation effect
            while (_mainCameraXAngle < MaxMainCameraAngle)
            {
                _mainCameraXAngle += RotationScale * Time.deltaTime;
                transform.localEulerAngles = new Vector3(_mainCameraXAngle, 0, 0);
                yield return null;
            }
            callback();
        }

        #endregion
    }
}