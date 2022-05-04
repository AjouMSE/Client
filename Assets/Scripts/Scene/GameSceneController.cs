using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class GameSceneController : MonoBehaviour
    {
        public GameObject cam;
        public GameObject panel;
        public GameObject scroll3D;

        private float x = -120;

        void Init()
        {
            GameManager.Instance.InitPanel(panel);
        }

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                scroll3D.GetComponent<ScrollScript3D>().OpenOrCloseScroll();
            }

            if (x < 30)
            {
                cam.transform.localEulerAngles = new Vector3(x, 0, 0);
                x += 2.5f;
            }
        }
    }
}
