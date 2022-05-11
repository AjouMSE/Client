using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Manager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            Application.targetFrameRate = 60;   // test code
            GameManager.Instance.InitPanel(panel);

            if (UserManager.Instance.IsHost)
                NetworkManager.Singleton.StartHost();
            else
                NetworkManager.Singleton.StartClient();
        }

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (x < 30)
            {
                cam.transform.localEulerAngles = new Vector3(x, 0, 0);
                x += 2.5f;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                scroll3D.GetComponent<ScrollScript3D>().OpenOrCloseScroll();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                NetworkManager.Singleton.StartHost();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}
