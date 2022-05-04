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

        private float x = -150;
        
        void Init()
        {
            GameManager.Instance.Init(panel);
        }

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("NetworkTestScene");
            }

            if (x < 30)
            {
                cam.transform.localEulerAngles = new Vector3(x, 0, 0);
                x += 2.25f;
            }
        }
    }   
}
