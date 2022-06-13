using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Utils;

namespace Manager.InGame
{
    public class PanelManager : MonoSingleton<PanelManager>
    {
        #region Private constants

        private readonly Color DefaultColor = new Color(0.8f, 0.8f, 0.8f);
        private readonly Color HostRangeColor = new Color(0.5f, 0.5f, 1f);
        private readonly Color ClientRangeColor = new Color(0.5f, 1f, 0.5f);

        #endregion


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
                    _panels[idx] = Instantiate(PanelTemplate,
                        new Vector3(j * Consts.PanelX, 0.2f, (Consts.Height - i - 1) * Consts.PanelY),
                        Quaternion.identity);
                    _panelRenderers[idx] = _panels[idx].GetComponent<MeshRenderer>();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitPanelColor()
        {
            for (var i = 0; i < _panelRenderers.Length; i++)
            {
                _panelRenderers[i].material.color = DefaultColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <param name="shiftX"></param>
        /// <param name="shiftY"></param>
        public void ShowSuitablePanels(string range)
        {
            var rangeColor = UserManager.Instance.IsHost ? HostRangeColor : ClientRangeColor;

            int shiftX, shiftY;
            if (UserManager.Instance.IsHost)
            {
                shiftX = GameManager2.Instance.HostController.X - Consts.DefaultSkillX;
                shiftY = GameManager2.Instance.HostController.Y - Consts.DefaultSkillY;
            }
            else
            {
                shiftX = GameManager2.Instance.ClientController.X - Consts.DefaultSkillX;
                shiftY = GameManager2.Instance.ClientController.Y - Consts.DefaultSkillY;
            }
            
            // Parse skill range
            var field = new BitMask.BitField25(range).CvtBits25ToBits30();
            field.Shift(shiftX, shiftY);

            var mask = BitMask.BitField30Msb;
            for (var i = 0; i < _panelRenderers.Length; i++)
            {
                if ((field.element & mask) > 0)
                    _panelRenderers[i].material.color = rangeColor;
                else
                    _panelRenderers[i].material.color = DefaultColor;
                mask >>= 1;
            }
        }

        #endregion
    }
}