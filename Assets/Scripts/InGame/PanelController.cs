using System.Collections;
using System.Collections.Generic;
using System;
using Manager;
using UnityEngine;


namespace InGame
{
    public class PanelController : MonoBehaviour
    {
        #region Private constants

        private const int Width = 6, Height = 5;
        private const int PanelCnt = Width * Height;

        #endregion

        #region Private variables

        [Header("Panel Prefab")] 
        [SerializeField] private GameObject panel;

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
            _panels = new GameObject[PanelCnt];
            _panelRenderers = new MeshRenderer[PanelCnt];

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    int idx = i * Width + j;
                    _panels[idx] = Instantiate(panel, new Vector3(j * 4.2f, 0.2f, 12.8f - i * 3.2f),
                        Quaternion.identity);
                    _panelRenderers[idx] = _panels[idx].GetComponent<MeshRenderer>();
                }
            }
        }

        public GameObject GetPanelByIdx(int idx)
        {
            if (idx > PanelCnt - 1 || idx < 0) return null;
            return _panels[idx];
        }

        public void ChangeColor(int idx)
        {
            if (idx > PanelCnt - 1 || idx < 0) return;
            StartCoroutine(ChangePanelColor(idx));
        }

        #endregion


        #region Coroutines

        IEnumerator ChangePanelColor(int idx)
        {
            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0) _panelRenderers[idx].material.color = new Color(1, 0.5f, 0.5f);
                else _panelRenderers[idx].material.color = new Color(1, 1, 1);
                yield return new WaitForSeconds(0.05f);
            }
            
            _panelRenderers[idx].material.color = new Color(1, 0.5f, 0.5f);
            yield return new WaitForSeconds(1f);
            
            _panelRenderers[idx].material.color = new Color(1, 1, 1);
        }

        #endregion
    }
}