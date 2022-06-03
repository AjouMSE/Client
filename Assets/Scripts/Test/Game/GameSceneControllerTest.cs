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
using Utils;

namespace Scene
{
    public class GameSceneControllerTest : MonoBehaviour
    {
        [SerializeField] private GameObject obj;
        
        #region Custom methods

        private void Init()
        {
            Button btn = obj.GetComponentsInChildren<Button>()[0];
            if(btn == null) Debug.Log("Btn is null");
            Debug.Log(btn.name);
        }

        #endregion


        #region Unity event methods

        private void Start()
        {
            Init();
            TableLoader.Instance.LoadTableData();
            StartCoroutine(CacheVFXSource.Instance.InitCoroutine());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ShowAllVFXS());
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayVFX(101100000);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayVFX(101100001);
            }
        }

        private void PlayVFX(int id)
        {
            ParticleSystem vfx = CacheVFXSource.Instance.GetSource(id);
            vfx.gameObject.SetActive(true);
            Debug.Log(vfx.transform.localScale);
            vfx.transform.position = new Vector3(0, 2f, 6.4f);
            vfx.transform.LookAt(new Vector3(10, 2f, 6.4f));

            vfx.Stop();
            vfx.Play();
            StartCoroutine(MoveVFX(vfx.transform, new Vector3(10, 2f, 6.4f)));
        }

        IEnumerator ShowAllVFXS()
        {
            foreach (KeyValuePair<int, CardData> card in TableDatas.Instance.cardDatas)
            {
                if (card.Value.type != (int)Consts.SkillType.Move)
                {
                    PlayVFX(card.Key);
                    yield return new WaitForSeconds(2f);
                }
                else
                {
                    yield return null;
                }
            }
        }

        IEnumerator MoveVFX(Transform vfx, Vector3 dest)
        {
            Vector3 curr = vfx.position;

            while (true)
            {
                curr = Vector3.Lerp(vfx.position, dest, 0.1f);
                vfx.position = curr;
                yield return null;
            }
        }

        #endregion
    }
}