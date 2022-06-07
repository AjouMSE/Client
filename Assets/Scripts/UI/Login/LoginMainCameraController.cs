using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Login
{
    public class LoginMainCameraController : MonoBehaviour
    {
        #region Private constants

        private const int RotCamDirLeft = -1, RotCamDirRight = 1;
        private const int RotAngleMaxLeft = -45, RotAngleMaxRight = 45;

        #endregion


        #region Private variables

        [SerializeField] private GameObject portalParticle;

        private float _mainCamRotSpd = 6.0f, _mainCamRotAngle = 0;
        private int _rotCamCurrDir = RotCamDirRight;

        #endregion


        #region Unity event methods

        private void Update()
        {
            RotateMainCamera();
        }

        #endregion


        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        private void RotateMainCamera()
        {
            if (_mainCamRotAngle > RotAngleMaxRight)
                _rotCamCurrDir = RotCamDirLeft;
            if (_mainCamRotAngle < RotAngleMaxLeft)
                _rotCamCurrDir = RotCamDirRight;

            float rotValue = _mainCamRotSpd * _rotCamCurrDir * Time.deltaTime;
            _mainCamRotAngle += rotValue;
            transform.Rotate(Vector3.up * rotValue);
        }

        #endregion


        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        public void CameraMovementEffect(Action callback, float z, float moveSpd)
        {
            portalParticle.SetActive(true);
            StartCoroutine(MoveCameraForwardCoroutine(callback, z, moveSpd));
        }

        #endregion


        #region Coroutines

        private IEnumerator MoveCameraForwardCoroutine(Action callback, float z, float moveSpd)
        {
            transform.eulerAngles = Vector3.zero;
            Vector3 dest = portalParticle.transform.position;
            dest.z = z;

            while (transform.position != dest)
            {
                transform.position = Vector3.MoveTowards(transform.position, dest, moveSpd);
                yield return null;
            }

            callback();
        }

        #endregion
    }
}