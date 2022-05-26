using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.Netcode;
using UnityEngine;
using Utils;
using Core;
using UI.Game;
using Unity.Netcode.Components;

namespace InGame
{
    public class WizardController : NetworkBehaviour
    {
        #region Private constants

        private const float Speed = 4.0f;

        private const int InitHostX = 0, InitHostY = 2;
        private const int InitClientX = 5, InitClientY = 2;

        private const int DirLeft = -90, DirRight = 90;

        private const int PaddingX = 1;

        private string[] AttackAnimations = new string[] { WizardAnimations.Attack01, WizardAnimations.Attack02, WizardAnimations.Attack03, WizardAnimations.Attack04 };

        #endregion


        public Material mat1, mat2, mat3;


        #region Private variables

        private int _x, _y, _idx;
        private BitMask.BitField30 _bitIdx; // msb is idx 0, lsb is idx 29 (0 ~ 29)
        private string _currentAnimation;

        private int _currMana;

        private Animator _animator;
        private PanelController _panelController;
        private HUDGameUserInfoUIController _userInfoUIController;

        private NetworkSynchronizer _netSync;

        #endregion


        #region Unity event methods

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _panelController = GameObject.Find("GameSceneObjectController").GetComponent<PanelController>();
            _userInfoUIController = GameObject.Find("GameSceneUIController").GetComponent<HUDGameUserInfoUIController>();
            _netSync = GameObject.Find("NetworkSynchronizer").GetComponent<NetworkSynchronizer>();
        }

        #endregion


        #region Network methods

        public override void OnNetworkSpawn()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                if (IsOwner)
                {
                    transform.position = new Vector3(InitHostX * Consts.PanelX - PaddingX, 0.3f, (Consts.Height - InitHostY - 1) * Consts.PanelY);
                    transform.localEulerAngles = new Vector3(0, DirRight, 0);

                    InitHostPos();
                    GameManager.Instance.SetHostWizardController(this);

                    _currMana = Consts.StartMana;
                }
                else
                {
                    transform.position = new Vector3(InitClientX * Consts.PanelX + PaddingX, 0.3f, (Consts.Height - InitClientY - 1) * Consts.PanelY);
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

        private void InitHostPos()
        {
            _x = InitHostX;
            _y = InitHostY;
            _idx = Consts.Width * InitHostY + InitHostX;
        }

        private void InitClientPos()
        {
            _x = InitClientX;
            _y = InitClientY;
            _idx = Consts.Width * InitClientY + InitClientX;
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

        public int GetMana()
        {
            return _currMana;
        }

        private void SetAnimation(string animationName)
        {
            if (!string.IsNullOrEmpty(_currentAnimation))
                _animator.SetBool(_currentAnimation, false);

            _animator.SetBool(animationName, true);
            _currentAnimation = animationName;
        }

        public void ProcessSkill(int code)
        {
            CardData data = TableDatas.Instance.GetCardData(code);
            BitMask.BitField30 range = ParseRangeWthCurrPos(data.range);
            List<int> panelIdxes = GetPanelIdx(range);

            switch (data.type)
            {
                case (int)Consts.SkillType.Move:
                    UseMana(data.cost);
                    SetPosition(panelIdxes[0]);
                    if (UserManager.Instance.IsHost)
                        Move(panelIdxes[0]);
                    break;

                case (int)Consts.SkillType.Attack:
                    UseMana(data.cost);
                    if (UserManager.Instance.IsHost)
                        Attack(data, range, panelIdxes);
                    break;
            }
        }

        private void SetPosition(int destIdx)
        {
            _x = destIdx % Consts.Width;
            _y = destIdx / Consts.Width;
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
            WizardController hostileController;
            Consts.UserType hostileType;
            if (IsOwner)
            {
                hostileController = GameManager.Instance.GetClientWizardController();
                hostileType = Consts.UserType.Client;
            }
            else
            {
                hostileController = GameManager.Instance.GetHostWizardController();
                hostileType = Consts.UserType.Host;
            }

            StartCoroutine(HitAction());

            if (CheckPlayerHit(hostileController.GetIdx(), range))
            {
                _netSync.UpdateGameValue(hostileType, Consts.GameUIType.HP, -data.value);
                StartCoroutine(hostileController.GetHitAction());
            }
        }

        private IEnumerator MoveAction(Vector3 destination)
        {
            // Animation Move Front
            SetAnimation(WizardAnimations.WalkForward);

            Vector3 destinationPosition = new Vector3(destination.x, transform.position.y, destination.z);
            if (IsOwner)
                destinationPosition.x -= PaddingX;
            else
                destinationPosition.x += PaddingX;

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

            // Animation Move Idle
            SetAnimation(WizardAnimations.Idle);
        }

        private BitMask.BitField30 ParseRangeWthCurrPos(string range)
        {
            BitMask.BitField30 fieldRange = new BitMask.BitField25(range).CvtBits25ToBits30();
            fieldRange.Shift(_x - 3, _y - 2);
            return fieldRange;
        }

        private List<int> GetPanelIdx(BitMask.BitField30 fieldRange)
        {
            List<int> idxes = new List<int>();

            int mask = BitMask.BitField30Msb;
            for (int i = 0; i < Consts.PanelCnt; i++)
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
            if (idx < 0 || idx > Consts.PanelCnt - 1) return default;
            return new BitMask.BitField30(BitMask.BitField30Msb >> idx);
        }

        private bool CheckPlayerHit(int idx, BitMask.BitField30 range)
        {
            return (CvtPlayerIdxToBitField30(idx).element & range.element) > 0 ? true : false;
        }

        private IEnumerator HitAction()
        {
            int idx = UnityEngine.Random.Range(0, AttackAnimations.Length);

            // Animation Battle GetHit
            SetAnimation(AttackAnimations[idx]);

            yield return new WaitForSeconds(1f);

            // Animation Idle
            SetAnimation(WizardAnimations.Idle);
        }

        private IEnumerator GetHitAction()
        {
            yield return new WaitForSeconds(1f);

            // Animation Battle GetHit
            SetAnimation(WizardAnimations.GetHit);

            yield return new WaitForSeconds(1f);

            // Animation Idle
            SetAnimation(WizardAnimations.Idle);
        }

        private void UseMana(int cost)
        {
            if (IsOwner)
            {
                _currMana -= cost;

                if (UserManager.Instance.IsHost)
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

                if (UserManager.Instance.IsHost)
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

        public void BattleResultAction(Consts.BattleResult battleResult)
        {
            switch (battleResult)
            {
                case Consts.BattleResult.WIN:
                    // Animation Victory
                    SetAnimation(WizardAnimations.Victory);
                    break;

                case Consts.BattleResult.LOSE:
                    // Animation Die
                    SetAnimation(WizardAnimations.Die);
                    break;
            }
        }

        public List<int> InvalidCards()
        {
            List<int> invalidCards = new List<int>();

            int saveIdx = _idx;
            int saveMana = _currMana;

            int[] cardList = null;
            if (UserManager.Instance.IsHost)
                cardList = _netSync.GetCopyList(Consts.UserType.Host);
            else
                cardList = _netSync.GetCopyList(Consts.UserType.Client);

            for (int i = 0; i < cardList.Length; i++)
            {
                CardData data = TableDatas.Instance.GetCardData(cardList[i]);
                BitMask.BitField30 range = ParseRangeWthCurrPos(data.range);
                List<int> panelIdxes = GetPanelIdx(range);

                switch (data.type)
                {
                    case (int)Consts.SkillType.Move:
                        SetPosition(panelIdxes[0]);
                        break;

                    case (int)Consts.SkillType.Attack:
                        _currMana -= data.cost;
                        break;
                }
            }

            foreach (KeyValuePair<int, CardData> cardData in TableDatas.Instance.cardDatas)
            {
                BitMask.BitField30 range = ParseRangeWthCurrPos(cardData.Value.range);
                List<int> panelIdxes = GetPanelIdx(range);

                switch (cardData.Value.type)
                {
                    case (int)Consts.SkillType.Move:
                        if (panelIdxes.Count != 1)
                            invalidCards.Add(cardData.Value.code);
                        break;

                    case (int)Consts.SkillType.Attack:
                        if (_currMana - cardData.Value.cost < 0)
                            invalidCards.Add(cardData.Value.code);
                        break;
                }
            }

            SetPosition(saveIdx);
            _currMana = saveMana;

            return invalidCards;
        }

        #endregion
    }
}
