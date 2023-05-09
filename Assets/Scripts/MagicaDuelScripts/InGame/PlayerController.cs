using System;
using System.Collections;
using System.Collections.Generic;
using Cache;
using DataTable;
using Manager;
using Manager.InGame;
using Manager.Net;
using UI.Game;
using UI.Game.UserStatus;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace InGame
{
    public class PlayerController : NetworkBehaviour
    {
        #region Private constants

        private const int InitHostX = 0, InitHostY = 2;
        private const int InitClientX = 5, InitClientY = 2;
        private const int DirLeft = -90, DirRight = 90;
        private const int PaddingX = 1;

        private enum Directions
        {
            Forward,
            Back,
            Left,
            Right
        };

        private string[] AttackAnimations =
        {
            WizardAnimations.Attack01, WizardAnimations.Attack02, WizardAnimations.Attack03, WizardAnimations.Attack04
        };

        #endregion


        #region Private variables

        [Header("Client wizard material")] 
        [SerializeField] private Material clientMaterial;

        private NetworkVariable<int> _netHp, _netMana;
        private NetworkVariable<int> _netPosX, _netPosY;
        private NetworkList<int> _buffList;
        private NetworkList<int> _debuffList;

        private int _idx, _dir;
        private float _speed = 4.0f;

        private string _currentAnimation;
        private Animator _animator;

        #endregion


        #region Public Variable

        public bool IsHostPlayerObject { get; private set; }
        public HUDGameUserStatusUIController UserStatusUIController { get; set; }

        public int Hp => _netHp?.Value ?? 0;
        public int Mana => _netMana?.Value ?? 0;
        public int X => _netPosX?.Value ?? 0;
        public int Y => _netPosY?.Value ?? 0;
        
        public int EstimateX { get; private set; }
        public int EstimateY { get; private set; }

        public int Idx => _idx;

        #endregion


        #region Unity event methods

        private void Awake()
        {
            Init();
        }

        private void OnApplicationQuit()
        {
            DisposeAll();
        }

        #endregion


        #region Private methods

        private void DisposeAll()
        {
            if(_netHp != null)
                _netHp.Dispose();
            
            if(_netMana != null)
                _netMana.Dispose();
            
            if(_netPosX != null)
                _netPosX.Dispose();
            
            if(_netPosY != null)
                _netPosY.Dispose();

            _netHp = null;
            _netMana = null;
            _netPosX = null;
            _netPosY = null;

            _debuffList = null;
            _buffList = null;
        }

        private void Init()
        {
            DisposeAll();
            
            _animator = GetComponent<Animator>();

            _netHp = new NetworkVariable<int>();
            _netMana = new NetworkVariable<int>();
            _netPosX = new NetworkVariable<int>();
            _netPosY = new NetworkVariable<int>();

            _buffList = new NetworkList<int>();
            _debuffList = new NetworkList<int>();

            if(UserManager.Instance.IsHost)
                StartCoroutine(SetDefaultValueCoroutine());
        }

        private void ProcessAnimation(string animationName)
        {
            if (!string.IsNullOrEmpty(_currentAnimation))
                _animator.SetBool(_currentAnimation, false);

            _animator.SetBool(animationName, true);
            _currentAnimation = animationName;
        }

        private void RotateDirection(int dir)
        {
            transform.localEulerAngles = new Vector3(0, dir, 0);
        }
        
        private void MoveX(int x)
        {
            if (x < 0 || x > Consts.Width - 1) return;
            if (!NetworkManager.Singleton.IsServer) return;
            _netPosX.Value = x;
        }

        private void MoveY(int y)
        {
            if (y < 0 || y > Consts.Height - 1) return;
            if (!NetworkManager.Singleton.IsServer) return;
            _netPosY.Value = y;
        }


        private void SetPosition(int destIdx)
        {
            var x = destIdx % Consts.Width;
            var y = destIdx / Consts.Width;

            MoveX(x);
            MoveY(y);
            _idx = destIdx;
        }


        private BitMask.BitField30 ParseRangeWithPos(string range, int px, int py)
        {
            var fieldRange = new BitMask.BitField25(range).CvtBits25ToBits30();
            fieldRange.Shift(px - Consts.DefaultSkillX, py - Consts.DefaultSkillY);
            return fieldRange;
        }

        private List<int> GetPanelIdx(BitMask.BitField30 fieldRange)
        {
            var idxes = new List<int>();
            var mask = BitMask.BitField30Msb;

            for (var i = 0; i < Consts.PanelCnt; i++)
            {
                if ((fieldRange.element & mask) > 0)
                    idxes.Add(i);

                mask >>= 1;
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

        #endregion


        #region Public methods

        public void SetDefaultValue()
        {
            _netHp.Value = Consts.DefaultHp;
            _netMana.Value = Consts.DefaultMana;

            if (IsHostPlayerObject)
            {
                _netPosX.Value = InitHostX;
                _netPosY.Value = InitHostY;
            }
            else
            {
                _netPosX.Value = InitClientX;
                _netPosY.Value = InitClientY;
            }

            // _buffList.Clear();
            // _debuffList.Clear();
        }

        public void ApplyDamage(int damage)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            _netHp.Value = Mathf.Clamp(_netHp.Value - damage, 0, Consts.MaximumHp);
        }

        public void RestoreHp(int hp)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            Debug.Log("here3");
            _netHp.Value = Mathf.Clamp(_netHp.Value + hp, 0, Consts.MaximumHp);
        }

        public void UseMana(int mana)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            _netMana.Value = Mathf.Clamp(_netMana.Value - mana, 0, Consts.MaximumMana);
        }

        public void RestoreMana(int mana)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            _netMana.Value = Mathf.Clamp(_netMana.Value + mana, 0, Consts.MaximumMana);
        }
        
        public void PlayAnimation(string animationName, float duration = 0f)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            StartCoroutine(ProcessAnimationCoroutine(animationName, duration));
        }

        public void ShowEmoji(CacheEmojiSource.EmojiType type, float duration = 1f)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            StartCoroutine(ProcessEmojiCoroutine(type, duration));
            ShowEmojiClientRpc(type, duration);
        }

        public List<int> InvalidCards()
        {
            var invalidCards = new List<int>();
            var saveMana = _netMana.Value;
            EstimateX = _netPosX.Value;
            EstimateY = _netPosY.Value;

            int[] cardList;
            if (UserManager.Instance.IsHost)
                cardList = NetGameStatusManager.Instance.CopyHostCardList();
            else
                cardList = NetGameStatusManager.Instance.CopyClientCardList();

            // Calculate current status
            for (var i = 0; i < cardList.Length; i++)
            {
                var data = TableDatas.Instance.GetCardData(cardList[i]);
                var range = ParseRangeWithPos(data.range, EstimateX, EstimateY);
                var panelIdxes = GetPanelIdx(range);

                saveMana -= data.cost;

                switch (data.type)
                {
                    case (int)Consts.SkillType.Move:
                        EstimateX = panelIdxes[0] % Consts.Width;
                        EstimateY = panelIdxes[0] / Consts.Width;
                        break;

                    case (int)Consts.SkillType.ManaCharge:
                        saveMana += data.value;
                        break;
                }
            }

            // Find invalid cards
            foreach (KeyValuePair<int, CardData> cardData in TableDatas.Instance.cardDatas)
            {
                var range = ParseRangeWithPos(cardData.Value.range, EstimateX, EstimateY);
                var panelIdxes = GetPanelIdx(range);

                switch (cardData.Value.type)
                {
                    case (int)Consts.SkillType.Move:
                        if (panelIdxes.Count == 0 || saveMana - cardData.Value.cost < 0)
                            invalidCards.Add(cardData.Value.code);
                        break;

                    default:
                        if (saveMana - cardData.Value.cost < 0)
                            invalidCards.Add(cardData.Value.code);
                        break;
                }
            }

            return invalidCards;
        }

        public void ProcessSkill(int code)
        {
            var data = TableDatas.Instance.GetCardData(code);
            var range = ParseRangeWithPos(data.range, _netPosX.Value, _netPosY.Value);
            var panelIdxes = GetPanelIdx(range);

            UseMana(data.cost);
            switch (data.type)
            {
                case (int)Consts.SkillType.Move:
                    var movingDir = CalculateDir(panelIdxes[0]);
                    if (UserManager.Instance.IsHost) 
                        Move(panelIdxes[0], movingDir);
                    break;

                case (int)Consts.SkillType.Attack:
                    if (UserManager.Instance.IsHost) 
                        Attack(data, range);
                    break;

                case (int)Consts.SkillType.ManaCharge:
                    RestoreMana(data.value);
                    Debug.Log($"{data.value.ToString()}");
                    StartCoroutine(HitAction());
                    break;
                
                case (int)Consts.SkillType.LifeRecovery:
                    Debug.Log("here2");
                    RestoreHp(data.value);
                    StartCoroutine(HitAction());
                    break;
                // 임시
                default:
                    if (UserManager.Instance.IsHost) 
                        StartCoroutine(HitAction());
                    break;
            }
        }

        private Directions CalculateDir(int destIdx)
        {
            int destX = destIdx % Consts.Width;
            int destY = destIdx / Consts.Width;

            if (_netPosX.Value < destX)
                return _dir == DirRight ? Directions.Forward : Directions.Back;
            else if (_netPosX.Value > destX)
                return _dir == DirRight ? Directions.Back : Directions.Forward;
            else if (_netPosY.Value < destY)
                return _dir == DirRight ? Directions.Right : Directions.Left;
            else if (_netPosY.Value > destY)
                return _dir == DirRight ? Directions.Left : Directions.Right;

            return Directions.Forward;
        }

        private void Move(int destIdx, Directions movingDir)
        {
            var dest = PanelManager.Instance.GetPanelByIdx(destIdx).transform.position;
            StartCoroutine(MoveAction(destIdx, dest, movingDir));
        }

        private void Attack(CardData data, BitMask.BitField30 range)
        {
            PlayerController hostileController;

            if (IsOwner)
                hostileController = MagicaDuelGameManager.Instance.ClientController;
            else 
                hostileController = MagicaDuelGameManager.Instance.HostController;
        

            StartCoroutine(HitAction());
            if (CheckPlayerHit(hostileController.Idx, range))
            {
                hostileController.ApplyDamage(data.value);
                StartCoroutine(hostileController.GetHitAction());
            }
        }

        private void ManaCharge(int value)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            _netMana.Value += value;
            StartCoroutine(HitAction());
        }
        
        private void LifeRecovery(int value)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            _netHp.Value += value;
            StartCoroutine(HitAction());
        }

        private IEnumerator MoveAction(int destIdx, Vector3 destination, Directions movingDir)
        {
            // Animation Move Front
            switch (movingDir)
            {
                case Directions.Forward:
                    PlayAnimation(WizardAnimations.WalkForward);
                    break;

                case Directions.Back:
                    PlayAnimation(WizardAnimations.WalkBack);
                    break;

                case Directions.Left:
                    PlayAnimation(WizardAnimations.WalkLeft);
                    break;

                case Directions.Right:
                    PlayAnimation(WizardAnimations.WalkRight);
                    break;
            }

            var destinationPosition = new Vector3(destination.x, transform.position.y, destination.z);
            if (IsOwner)
                destinationPosition.x -= PaddingX;
            else
                destinationPosition.x += PaddingX;

            var dir = destinationPosition - transform.position;
            var dirNormalized = (destinationPosition - transform.position).normalized;

            while (dir.sqrMagnitude > 0.1f)
            {
                dir = destinationPosition - transform.position;
                dirNormalized = (destinationPosition - transform.position).normalized;

                transform.position += dirNormalized * (_speed * Time.deltaTime);

                yield return null;
            }

            transform.position = destinationPosition;
            SetPosition(destIdx);
            Rotate();

            // Animation Move Idle
            PlayAnimation(WizardAnimations.Idle);
        }

        private void Rotate()
        {
            PlayerController hostileController;
            if (IsOwner)
                hostileController = MagicaDuelGameManager.Instance.ClientController;
            else
                hostileController = MagicaDuelGameManager.Instance.HostController;

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

        private bool CheckPlayerHit(int idx, BitMask.BitField30 range)
        {
            return (CvtPlayerIdxToBitField30(idx).element & range.element) > 0;
        }

        private IEnumerator HitAction()
        {
            var idx = UnityEngine.Random.Range(0, AttackAnimations.Length);
            PlayAnimation(AttackAnimations[idx], 1.0f);

            yield break;
        }

        private IEnumerator GetHitAction()
        {
            yield return new WaitForSeconds(1f);
            PlayAnimation(WizardAnimations.GetHit, 1f);
            ShowEmoji(CacheEmojiSource.EmojiType.EmojiInjured, 1.5f);
        }

        #endregion


        #region Network methods

        public override void OnNetworkSpawn()
        {
            IsHostPlayerObject = IsOwnedByServer;
            name = IsHostPlayerObject ? $"(h){name}" : $"(c){name}";

            // Add OnValueChanged Listener
            _netHp.OnValueChanged += (value, newValue) =>
            {
                if (UserStatusUIController is null) return;

                if (IsHostPlayerObject)
                    UserStatusUIController.UpdateHostUI(Consts.GameUIType.Hp, newValue);
                else
                    UserStatusUIController.UpdateClientUI(Consts.GameUIType.Hp, newValue);
            };

            _netMana.OnValueChanged += (value, newValue) =>
            {
                if (UserStatusUIController is null) return;

                switch (NetworkManager.Singleton.IsServer)
                {
                    case true when IsHostPlayerObject:
                        UserStatusUIController.UpdateHostUI(Consts.GameUIType.Mana, newValue);
                        break;
                    case false when !IsHostPlayerObject:
                        UserStatusUIController.UpdateClientUI(Consts.GameUIType.Mana, newValue);
                        break;
                }
            };

            // todo-add OnValueChanged callback

            _buffList.OnListChanged += e => { };
            _debuffList.OnListChanged += e => { };

            // Set Position
            if (NetworkManager.Singleton.IsServer)
            {
                if (IsHostPlayerObject)
                {
                    // set host player object default position
                    transform.position = new Vector3(InitHostX * Consts.PanelX - PaddingX, 0.3f,
                        (Consts.Height - InitHostY - 1) * Consts.PanelY);
                    transform.localEulerAngles = new Vector3(0, DirRight, 0);

                    _netPosX.Value = InitHostX;
                    _netPosY.Value = InitHostY;
                    _idx = InitHostY * Consts.Width + InitHostX;
                }
                else
                {
                    // set clien object default position
                    transform.position = new Vector3(InitClientX * Consts.PanelX + PaddingX, 0.3f,
                        (Consts.Height - InitClientY - 1) * Consts.PanelY);
                    transform.localEulerAngles = new Vector3(0, DirLeft, 0);

                    _netPosX.Value = InitClientX;
                    _netPosY.Value = InitClientY;
                    _idx = InitClientY * Consts.Width + InitClientX;
                }
            }

            // Set Client Material
            if (!IsHostPlayerObject)
                GetComponentInChildren<SkinnedMeshRenderer>().material = clientMaterial;

            MagicaDuelGameManager.Instance.SetPlayerController(IsHostPlayerObject, this);
        }

        [ClientRpc]
        private void ShowEmojiClientRpc(CacheEmojiSource.EmojiType type, float duration)
        {
            if (NetworkManager.Singleton.IsServer) return;
            StartCoroutine(ProcessEmojiCoroutine(type, duration));
        }

        #endregion


        #region Coroutines

        private IEnumerator SetDefaultValueCoroutine()
        {
            while (!IsSpawned)
                yield return null;
            
            SetDefaultValue();
        }

        private IEnumerator ProcessAnimationCoroutine(string animationName, float duration)
        {
            ProcessAnimation(animationName);
            yield return CacheCoroutineSource.Instance.GetSource(duration);
            if (duration > 0)
                ProcessAnimation(WizardAnimations.Idle);
        }

        private IEnumerator ProcessEmojiCoroutine(CacheEmojiSource.EmojiType type, float duration)
        {
            var emoji = CacheEmojiSource.Instance.GetSource(type);

            // Play Emoji effect
            emoji.gameObject.SetActive(true);
            emoji.transform.position = transform.position + new Vector3(0, 3, 0);
            emoji.Play();

            yield return CacheCoroutineSource.Instance.GetSource(duration);

            // Stop Emoji effect
            emoji.Stop();
            emoji.transform.position = Vector3.zero;
            emoji.gameObject.SetActive(false);
        }

        #endregion
    }
}