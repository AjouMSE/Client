using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Manager.InGame
{
    public class PanelManager : MonoSingleton<PanelManager>
    {
        #region Private variables

        public GameObject PanelTemplate { get; set; }
        private GameObject[] _panels;
        private MeshRenderer[] _panelRenderers;

        #endregion


        #region Public methods
        
        public override void Init()
        {
            _panels = new GameObject[Consts.PanelCnt];
            _panelRenderers = new MeshRenderer[Consts.PanelCnt];

            for (var i = 0; i < Consts.Height; i++)
            {
                for (var j = 0; j < Consts.Width; j++)
                {
                    var idx = i * Consts.Width + j;
                    _panels[idx] = Instantiate(PanelTemplate, new Vector3(j * Consts.PanelX, 0.2f, (Consts.Height - i - 1) * Consts.PanelY), Quaternion.identity);
                    _panelRenderers[idx] = _panels[idx].GetComponent<MeshRenderer>();
                }
            }
        }
        
        #endregion
    }
}