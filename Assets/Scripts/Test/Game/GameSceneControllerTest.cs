using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Data.Cache;
using Manager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scene
{
    public class GameSceneControllerTest : MonoBehaviour
    {
        #region Custom methods

        private void Init()
        {

        }

        #endregion



        #region Unity event methods

        private void Start()
        {
            Init();
            TableLoader.Instance.LoadTableData();
            StartCoroutine(CacheVFXSource.Instance.Init());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ParticleSystem vfx = CacheVFXSource.Instance.GetSource(101100000);
                vfx.gameObject.SetActive(true);
                vfx.transform.position = new Vector3(0, 0.3f, 6.4f);
                vfx.Play();
            }            
            
            if (Input.GetKeyDown(KeyCode.B))
            {
                ParticleSystem vfx = CacheVFXSource.Instance.GetSource(101100013);
                vfx.gameObject.SetActive(true);
                vfx.transform.position = new Vector3(0, 0.3f, 6.4f);
                vfx.Play();
            }
        }

        #endregion
    }
}
