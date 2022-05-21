using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.Netcode;
using UnityEngine;
using Utils;
using Core;

namespace InGame
{
    public class WizardController : NetworkBehaviour
    {
        #region Private constants

        private const int Width = 6, Height = 5;
        private const float Speed = 4.0f;

        private const int InitialX = 0, InitialY = 2, InitialIdx = 12;

        #endregion


        public Material mat1, mat2, mat3;


        #region Private variables

        private int _x, _y, _idx;
        private BitMask.BitField30 _bitIdx; // msb is idx 0, lsb is idx 29 (0 ~ 29)

        private Animator _animator;
        private PanelController _panelController;

        #endregion

        enum AnimationState
        {
            Idle = 0,
            MoveFront = 1,
            MoveBack = 2,
            Attack = 3
        }

        #region Network methods

        public override void OnNetworkSpawn()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                if (IsOwner)
                {
                    transform.position = new Vector3(0, 0.3f, 6.4f);
                    transform.localEulerAngles = new Vector3(0, 90, 0);
                }
                else
                {
                    transform.position = new Vector3(21, 0.3f, 6.4f);
                    transform.localEulerAngles = new Vector3(0, -90, 0);
                    GetComponentInChildren<SkinnedMeshRenderer>().material = mat2;
                }
            }
            else
            {
                if (IsOwner)
                    GetComponentInChildren<SkinnedMeshRenderer>().material = mat2;
            }
        }

        #endregion


        #region Custom methods


        public void Start()
        {
            _panelController = GameObject.Find("GameSceneObjectController").GetComponent<PanelController>();
            _animator = GetComponent<Animator>();
            InitPos();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                string range = "00000 00000 00000 00100 00000";
                BitMask.BitField30 fieldRange = new BitMask.BitField25(range).CvtBits25ToBits30();

                fieldRange.Shift(_x - 3, _y - 2);
                fieldRange.PrintBy2D();
                int destIdx = GetPanelIdx(fieldRange)[0];

                Move(destIdx);
            }
        }

        private void InitPos()
        {
            _x = InitialX;
            _y = InitialY;
            _idx = InitialIdx;
        }

        public void ProcessSkill(int code)
        {
            // ~ range
            // Skill -> 피격판정 -> UpdateClientHp()
            //       -> VFX + ChangeColor
            // UpdateUI
        }

        private void Move(int destIdx)
        {
            _x = destIdx % Width;
            _y = destIdx / Width;
            _idx = destIdx;
            _bitIdx = ConvertIdxToBitIdx(_idx);

            Vector3 destPosition = _panelController.GetPanelByIdx(_idx).transform.position;
            StartCoroutine(MoveAction(destPosition));
        }

        public IEnumerator MoveAction(Vector3 destination)
        {
            _animator.SetInteger("AnimationState", (int)AnimationState.MoveFront);

            Vector3 destinationPosition = new Vector3(destination.x, transform.position.y, destination.z);
            Vector3 dir = destinationPosition - transform.position;
            Vector3 dirNormalized = (destinationPosition - transform.position).normalized;

            while (dir.sqrMagnitude > 0.1f)
            {
                dir = destinationPosition - transform.position;
                dirNormalized = (destinationPosition - transform.position).normalized;

                transform.position += dirNormalized * Speed * Time.deltaTime;

                yield return null;
            }

            transform.position = destinationPosition;

            _animator.SetInteger("AnimationState", (int)AnimationState.Idle);
        }

        private List<int> GetPanelIdx(BitMask.BitField30 fieldRange)
        {
            List<int> idxes = new List<int>();

            int mask = BitMask.BitField30Msb;
            for (int i = 0; i < 30; i++)
            {
                if ((fieldRange.element & mask) > 0)
                {
                    idxes.Add(i);
                }

                mask = mask >> 1;
            }

            return idxes;
        }

        private BitMask.BitField30 ConvertIdxToBitIdx(int idx)
        {
            int zero = BitMask.BitField30Msb;
            return new BitMask.BitField30(zero >> idx);
        }


        #endregion
    }
}
