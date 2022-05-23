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

        private const int InitHostX = 0, InitHostY = 2, InitHostIdx = 12;
        private const int InitClientX = 5, InitClientY = 2, InitClientIdx = 17;

        private const int DirLeft = -90, DirRight = 90;

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
                    transform.localEulerAngles = new Vector3(0, DirRight, 0);

                    InitHostPos();
                    GameManager.Instance.SetHostWizardController(this);
                }
                else
                {
                    transform.position = new Vector3(21, 0.3f, 6.4f);
                    transform.localEulerAngles = new Vector3(0, DirLeft, 0);
                    GetComponentInChildren<SkinnedMeshRenderer>().material = mat2;

                    InitClientPos();
                    GameManager.Instance.SetClientWizardController(this);
                }
            }
            else
            {
                if (IsOwner)
                {
                    GetComponentInChildren<SkinnedMeshRenderer>().material = mat2;

                    InitClientPos();
                    GameManager.Instance.SetClientWizardController(this);
                }
                else
                {
                    InitHostPos();
                    GameManager.Instance.SetHostWizardController(this);
                }
            }
        }

        #endregion


        #region Custom methods


        public void Start()
        {
            _panelController = GameObject.Find("GameSceneObjectController").GetComponent<PanelController>();
            _animator = GetComponent<Animator>();
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

        private void InitHostPos()
        {
            _x = InitHostX;
            _y = InitHostY;
            _idx = InitHostIdx;
        }

        private void InitClientPos()
        {
            _x = InitClientX;
            _y = InitClientY;
            _idx = InitClientIdx;
        }

        public int GetIdx()
        {
            return _idx;
        }

        public int GetX()
        {
            return _x;
        }

        public int GetY()
        {
            return _y;
        }

        public void ProcessSkill(int code)
        {
            CardData data = TableDatas.Instance.GetCardData(code);
            BitMask.BitField30 range = ParseRangeWthCurrPos(data);
            List<int> panelIdxes = GetPanelIdx(range);

            switch (data.type)
            {
                case (int)Consts.SkillType.Move:
                    SetPosition(panelIdxes[0]);
                    if (UserManager.Instance.IsHost)
                    {
                        Move(panelIdxes[0]);
                    }
                    break;

                case (int)Consts.SkillType.Attack:
                    if (UserManager.Instance.IsHost)
                    {
                        Attack(range, panelIdxes);
                    }
                    break;
            }
        }

        private void SetPosition(int destIdx)
        {
            _x = destIdx % Width;
            _y = destIdx / Width;
            _idx = destIdx;
            _bitIdx = ConvertIdxToBitIdx(_idx);
        }

        private void Move(int destIdx)
        {
            Vector3 destPosition = _panelController.GetPanelByIdx(_idx).transform.position;
            StartCoroutine(MoveAction(destPosition));
        }

        private void Attack(BitMask.BitField30 range, List<int> panelIdxes)
        {
            // Host만 실행

            // Skill -> 피격판정 -> UpdateClientHp()
            //       -> VFX + ChangeColor
            // UpdateUI

            int hostileIdx = -1;
            if (IsOwner)
            {
                hostileIdx = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(NetworkManager.Singleton.ConnectedClientsIds[1]).GetComponent<WizardController>().GetIdx();
            }
            else
            {
                hostileIdx = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<WizardController>().GetIdx();
            }

            if (CheckPlayerHit(hostileIdx, range))
            {
                Debug.Log("Hit!");
            }
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

        private BitMask.BitField30 ParseRangeWthCurrPos(CardData data)
        {
            BitMask.BitField30 fieldRange = new BitMask.BitField25(data.range).CvtBits25ToBits30();
            fieldRange.Shift(_x - 3, _y - 2);
            return fieldRange;
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

        private BitMask.BitField30 CvtPlayerIdxToBitField30(int idx)
        {
            if (idx < 0 || idx > 29) return default;
            return new BitMask.BitField30(BitMask.BitField30Msb >> idx);
        }

        private bool CheckPlayerHit(int idx, BitMask.BitField30 range)
        {
            return (CvtPlayerIdxToBitField30(idx).element & range.element) > 0 ? true : false;
        }

        #endregion
    }
}
