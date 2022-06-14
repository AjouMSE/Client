using System.Collections;
using System.Collections.Generic;
using Core;
using Data.Cache;
using InGame;
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
        private GameObject _playerTemplate;
        private GameObject[] _panels;
        private MeshRenderer[] _panelRenderers;

        #endregion


        #region Public methods

        public override void Init()
        {
            _panels = new GameObject[Consts.PanelCnt];
            _panelRenderers = new MeshRenderer[Consts.PanelCnt];
            _playerTemplate = GameObject.Find("PlayerTemplate");

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
        /// <param name="idx"></param>
        /// <returns></returns>
        public GameObject GetPanelByIdx(int idx)
        {
            if (idx > Consts.PanelCnt - 1 || idx < 0) return null;
            return _panels[idx];
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitPanelColor()
        {
            for (var i = 0; i < _panelRenderers.Length; i++)
                _panelRenderers[i].material.color = DefaultColor;

            _playerTemplate.transform.position = new Vector3(0, 100, 0);
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
            var playerController = UserManager.Instance.IsHost
                ? GameManager2.Instance.HostController
                : GameManager2.Instance.ClientController;
            
            var ex = playerController.EstimateX;
            var ey = playerController.EstimateY;
            var shiftX = ex - Consts.DefaultSkillX;
            var shiftY = ey - Consts.DefaultSkillY;

            // Show player template
            if (ex != playerController.X || ey != playerController.Y)
            {
                _playerTemplate.transform.rotation = playerController.transform.rotation;
                var pos = GetPanelByIdx(ey * Consts.Width + ex).transform.position;
                pos.y = 0.3f;
                _playerTemplate.transform.position = pos;   
            }

            // Parse skill range
            var field = new BitMask.BitField25(range).CvtBits25ToBits30();
            field.Shift(shiftX, shiftY);

            // Show estimated range
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
        
        
        public void ProcessEffect(int code, Consts.UserType userType, PlayerController srcController, PlayerController destController)
        {
            // parse range
            var data = TableDatas.Instance.GetCardData(code);
            var fieldRange = new BitMask.BitField25(data.range).CvtBits25ToBits30();
            fieldRange.Shift(srcController.X - Consts.DefaultSkillX, srcController.Y - Consts.DefaultSkillY);

            // Show panel color effect (indicate skill range)
            var mask = BitMask.BitField30Msb;
            for (int idx = 0; idx < Consts.PanelCnt; idx++)
            {
                if ((fieldRange.element & mask) > 0)
                    StartCoroutine(ChangeColor(idx, userType));

                mask >>= 1;
            }
            
            // show vfx
            if (data.type != (int)Consts.SkillType.Move)
                StartCoroutine(ShowEffect(code, srcController, destController));
        }

        #endregion


        #region MyRegion

        private IEnumerator ChangeColor(int idx, Consts.UserType userType)
        {
            Color baseColor, changedColor;
            baseColor = new Color(0.8f, 0.8f, 0.8f);
            if (userType == Consts.UserType.Host) changedColor = new Color(0.5f, 0.5f, 1f);
            else changedColor = new Color(0.5f, 1f, 0.5f);

            for (int i = 0; i < 20; i++)
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
        
        private IEnumerator ShowEffect(int code, PlayerController srcController, PlayerController destController)
        {
            yield return new WaitForSeconds(0.5f);

            var vfx = CacheVFXSource.Instance.GetSource(code);
            vfx.gameObject.SetActive(true);

            var idx = Consts.Width * srcController.Y + srcController.X;
            var panelPos = _panels[idx].transform.position;
            vfx.transform.position = new Vector3(panelPos.x, panelPos.y + 0.3f, panelPos.z);
            vfx.Play();

            var data = TableDatas.Instance.GetCardData(code);
            if (data.type == (int)Consts.SkillType.Attack)
                StartCoroutine(MoveVFX(vfx.transform, destController.transform.position));

            yield return new WaitForSeconds(2f);

            vfx.transform.position = new Vector3(0, 100, 0);
            vfx.gameObject.SetActive(false);
        }

        private IEnumerator MoveVFX(Transform vfx, Vector3 dest)
        {
            var curr = vfx.position;

            while (Vector3.Distance(curr, dest) > 0.1f)
            {
                curr = Vector3.Lerp(vfx.position, dest, 0.05f);
                vfx.position = curr;
                yield return null;
            }
        }

        #endregion
    }
}