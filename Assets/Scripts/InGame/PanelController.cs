using System.Collections;
using System.Collections.Generic;
using System;
using Manager;
using TMPro;
using UnityEngine;
using Core;
using Utils;

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
            GameManager.Instance.Init();
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

        private void GenerateVfx(int idx, int vfxId)
        {
            // show effect here
            Vector3 vfxPos = _panels[idx].transform.position + new Vector3(0, 0.1f, 0);
            Vector3 vfxSize = new Vector3(3, 3, 3);
            if (vfxId == 1) Instantiate(vfx, vfxPos, Quaternion.identity).transform.localScale = vfxSize;
            else Instantiate(vfx2, vfxPos, Quaternion.identity).transform.localScale = vfxSize;
        }

        public GameObject GetPanelByIdx(int idx)
        {
            if (idx > Consts.PanelCnt - 1 || idx < 0) return null;
            return _panels[idx];
        }

        public void ProcessEffect(int code, int type, int x, int y)
        {
            CardData data = TableDatas.Instance.GetCardData(code);
            BitMask.BitField30 fieldRange = new BitMask.BitField25(data.range).CvtBits25ToBits30();
            fieldRange.Shift(x - 3, y - 2);

            int mask = BitMask.BitField30Msb;
            for (int idx = 0; idx < Consts.PanelCnt; idx++)
            {
                if ((fieldRange.element & mask) > 0)
                {
                    StartCoroutine(ShowEffect(idx, 0, type));
                }

                mask = mask >> 1;
            }
        }

        #endregion


        #region Coroutines

        IEnumerator ShowEffect(int idx, int vfxId, int type)
        {
            Color baseColor, changedColor;
            baseColor = new Color(0.8f, 0.8f, 0.8f);
            if (type == 0) changedColor = new Color(0.5f, 0.5f, 1f);
            else changedColor = new Color(0.5f, 1f, 0.5f);

            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    _panelRenderers[idx].material.color = changedColor;
                else
                    _panelRenderers[idx].material.color = baseColor;

                yield return new WaitForSeconds(0.02f);
            }

            GenerateVfx(idx, vfxId);
            _panelRenderers[idx].material.color = changedColor;
            yield return new WaitForSeconds(2f);

            _panelRenderers[idx].material.color = baseColor;
        }

        #endregion
    }
}