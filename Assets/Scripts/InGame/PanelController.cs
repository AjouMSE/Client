using System.Collections;
using System.Collections.Generic;
using System;
using Manager;
using TMPro;
using UnityEngine;
using Core;
using Utils;
using Data.Cache;

namespace InGame
{
    public class PanelController : MonoBehaviour
    {
        #region Private variables

        [Header("Panel Prefab")]
        [SerializeField] private GameObject panel;

        [Header("VFX Prefab")]
        [SerializeField] private GameObject vfx;

        [SerializeField] private GameObject vfx2;

        private GameObject[] _panels;
        private MeshRenderer[] _panelRenderers;

        #endregion


        #region Unity event methods

        private void Awake()
        {
            //GameManager.Instance.Init();
        }

        private void Start()
        {
            InitPanels();
        }

        #endregion


        #region Custom methods

        private void InitPanels()
        {
            _panels = new GameObject[Consts.PanelCnt];
            _panelRenderers = new MeshRenderer[Consts.PanelCnt];

            for (int i = 0; i < Consts.Height; i++)
            {
                for (int j = 0; j < Consts.Width; j++)
                {
                    int idx = i * Consts.Width + j;
                    _panels[idx] = Instantiate(panel, new Vector3(j * Consts.PanelX, 0.2f, (Consts.Height - i - 1) * Consts.PanelY), Quaternion.identity);
                    _panelRenderers[idx] = _panels[idx].GetComponent<MeshRenderer>();
                    // _panels[idx].GetComponentInChildren<TextMeshPro>().text = idx.ToString();
                }
            }
        }

        public GameObject GetPanelByIdx(int idx)
        {
            if (idx > Consts.PanelCnt - 1 || idx < 0) return null;
            return _panels[idx];
        }

        public void ProcessEffect(int code, Consts.UserType userType, WizardController myWizardController, WizardController hostileWizardController)
        {
            CardData data = TableDatas.Instance.GetCardData(code);
            BitMask.BitField30 fieldRange = new BitMask.BitField25(data.range).CvtBits25ToBits30();
            fieldRange.Shift(myWizardController.GetX() - 3, myWizardController.GetY() - 2);

            int mask = BitMask.BitField30Msb;
            for (int idx = 0; idx < Consts.PanelCnt; idx++)
            {
                if ((fieldRange.element & mask) > 0)
                {
                    StartCoroutine(ChangeColor(idx, userType));
                }

                mask = mask >> 1;
            }

            if (data.type != (int)Consts.SkillType.Move)
                StartCoroutine(ShowEffect(code, myWizardController, hostileWizardController));
        }

        #endregion


        #region Coroutines

        private IEnumerator ChangeColor(int idx, Consts.UserType userType)
        {
            Color baseColor, changedColor;
            baseColor = new Color(0.8f, 0.8f, 0.8f);
            if (userType == Consts.UserType.Host) changedColor = new Color(0.5f, 0.5f, 1f);
            else changedColor = new Color(0.5f, 1f, 0.5f);

            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    _panelRenderers[idx].material.color = changedColor;
                else
                    _panelRenderers[idx].material.color = baseColor;

                yield return new WaitForSeconds(0.02f);
            }

            _panelRenderers[idx].material.color = changedColor;
            yield return new WaitForSeconds(2f);

            _panelRenderers[idx].material.color = baseColor;
        }

        private IEnumerator ShowEffect(int code, WizardController myWizardController, WizardController hostileWizardController)
        {
            yield return new WaitForSeconds(0.5f);

            ParticleSystem vfx = CacheVFXSource.Instance.GetSource(code);
            vfx.gameObject.SetActive(true);

            int idx = Consts.Width * myWizardController.GetY() + myWizardController.GetX();
            Vector3 panelPos = GetPanelByIdx(idx).transform.position;
            vfx.transform.position = new Vector3(panelPos.x, panelPos.y + 0.3f, panelPos.z);

            vfx.Play();

            CardData data = TableDatas.Instance.GetCardData(code);
            if (data.type == (int)Consts.SkillType.Attack)
                StartCoroutine(MoveVFX(vfx.transform, hostileWizardController.transform.position));

            yield return new WaitForSeconds(2f);

            vfx.transform.position = new Vector3(0, 100, 0);
            vfx.gameObject.SetActive(false);
        }

        private IEnumerator MoveVFX(Transform vfx, Vector3 dest)
        {
            Vector3 curr = vfx.position;

            while (true)
            {
                curr = Vector3.Lerp(vfx.position, dest, 0.05f);
                vfx.position = curr;
                yield return null;
            }
        }

        #endregion
    }
}