using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.Netcode;
using UnityEngine;
using Utils;
using Core;
using UI.Game;

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

        private int _currMana;

        private Animator _animator;
        private PanelController _panelController;
        private HUDGameUserInfoUIController _userInfoUIController;

        private NetworkSynchronizer _netSync;

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
                    transform.position = new Vector3(-1, 0.3f, 6.4f);
                    transform.localEulerAngles = new Vector3(0, DirRight, 0);

                    InitHostPos();
                    GameManager.Instance.SetHostWizardController(this);

                    _currMana = Consts.StartMana;
                }
                else
                {
                    transform.position = new Vector3(22, 0.3f, 6.4f);
                    transform.localEulerAngles = new Vector3(0, DirLeft, 0);
                    GetComponentInChildren<SkinnedMeshRenderer>().material = mat2;

                    InitClientPos();
                    GameManager.Instance.SetClientWizardController(this);
                }
                _netSync = GameObject.Find("NetworkSynchronizer").GetComponent<NetworkSynchronizer>();
            }
            else
            {
                if (IsOwner)
                {
                    GetComponentInChildren<SkinnedMeshRenderer>().material = mat2;

                    InitClientPos();
                    GameManager.Instance.SetClientWizardController(this);

                    _currMana = Consts.StartMana;
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
            _animator = GetComponent<Animator>();
            _panelController = GameObject.Find("GameSceneObjectController").GetComponent<PanelController>();
            _userInfoUIController = GameObject.Find("GameSceneUIController").GetComponent<HUDGameUserInfoUIController>();
        }

        public void Update()
        {

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
                    UseMana(data);
                    if (UserManager.Instance.IsHost)
                    {
                        Attack(data, range, panelIdxes);
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

        private void Attack(CardData data, BitMask.BitField30 range, List<int> panelIdxes)
        {
            // Host만 실행
            int hostileIdx = -1;
            if (IsOwner)
            {
                hostileIdx = GameManager.Instance.GetClientWizardController().GetIdx();
                if (CheckPlayerHit(hostileIdx, range))
                {
                    Debug.Log("Hit!");
                    _netSync.UpdateClientValue(Consts.GameUIType.HP, -data.value);
                }
            }
            else
            {
                hostileIdx = GameManager.Instance.GetHostWizardController().GetIdx();
                if (CheckPlayerHit(hostileIdx, range))
                {
                    Debug.Log("Hit!");
                    _netSync.UpdateHostValue(Consts.GameUIType.HP, -data.value);
                }
            }
        }

        public IEnumerator MoveAction(Vector3 destination)
        {
            _animator.SetInteger("MoveState", (int)AnimationState.MoveFront);

            Vector3 destinationPosition = new Vector3(destination.x, transform.position.y, destination.z);
            if (IsOwner)
                destinationPosition.x -= 1;
            else
                destinationPosition.x += 1;

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

            Rotate();

            _animator.SetInteger("MoveState", (int)AnimationState.Idle);
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

        private void UseMana(CardData data)
        {
            if (IsOwner)
            {
                _currMana -= data.cost;

                if (NetworkManager.Singleton.IsServer)
                    _userInfoUIController.UpdateHostUI(Consts.GameUIType.Mana, _currMana);
                else
                    _userInfoUIController.UpdateClientUI(Consts.GameUIType.Mana, _currMana);
            }
        }

        public void GainMana()
        {
            if (IsOwner)
            {
                int gainManaAmt = GameManager.Instance.turnValue;
                _currMana += gainManaAmt;

                if (_currMana > Consts.MaxMana)
                    _currMana = Consts.MaxMana;

                if (NetworkManager.Singleton.IsServer)
                    _userInfoUIController.UpdateHostUI(Consts.GameUIType.Mana, _currMana);
                else
                    _userInfoUIController.UpdateClientUI(Consts.GameUIType.Mana, _currMana);
            }
        }

        private void Rotate()
        {
            WizardController hostileController = null;
            if (IsOwner)
                hostileController = GameManager.Instance.GetClientWizardController();
            else
                hostileController = GameManager.Instance.GetHostWizardController();

            if (transform.position.x < hostileController.transform.position.x)
            {
                RotateDirection(DirRight);
                hostileController.RotateDirection(DirLeft);
            }
            else
            {
                RotateDirection(DirLeft);
                hostileController.RotateDirection(DirRight);
            }
        }

        public void RotateDirection(int dir)
        {
            transform.localEulerAngles = new Vector3(0, dir, 0);
        }

        #endregion
    }
}
