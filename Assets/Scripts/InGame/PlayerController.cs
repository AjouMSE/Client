using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Data.Cache;
using Manager;
using Manager.InGame;
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
        private const int PaddingX = -1;

        private const int DefaultHp = 100, DefaultMana = 3;
        private const int MaxHp = 100, MaxMana = 10;

        #endregion


        #region Private variables

        [Header("Client wizard material")] [SerializeField]
        private Material clientMaterial;

        private NetworkVariable<int> _netHp, _netMana;
        private NetworkVariable<int> _netPosX, _netPosY;
        private NetworkList<int> _buffList;
        private NetworkList<int> _debuffList;

        private int _idx;
        private BitMask.BitField30 _bitIdx;

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

        #endregion


        #region Unity event methods

        private void Awake()
        {
            Init();
        }

        #endregion


        #region Private methods

        private void Init()
        {
            _animator = GetComponent<Animator>();

            _netHp = new NetworkVariable<int>();
            _netMana = new NetworkVariable<int>();
            _netPosX = new NetworkVariable<int>();
            _netPosY = new NetworkVariable<int>();

            _buffList = new NetworkList<int>();
            _debuffList = new NetworkList<int>();
            
            SetDefaultValue();
        }

        private void ProcessAnimation(string animationName)
        {
            if (!string.IsNullOrEmpty(_currentAnimation))
                _animator.SetBool(_currentAnimation, false);

            _animator.SetBool(animationName, true);
            _currentAnimation = animationName;
        }

        #endregion


        #region Public methods

        public void SetDefaultValue()
        {
            _netHp.Value = DefaultHp;
            _netMana.Value = DefaultMana;

            if (IsHost)
            {
                _netPosX.Value = InitHostX;
                _netPosY.Value = InitHostY;
            }
            else
            {
                _netPosX.Value = InitClientX;
                _netPosY.Value = InitClientY;
            }
            
            _buffList.Clear();
            _debuffList.Clear();
        }
        
        public void ApplyDamage(int damage)
        {
            if (NetworkManager.Singleton.IsHost)
                _netHp.Value = Mathf.Clamp(_netHp.Value - damage, 0, Consts.MaximumHp);
        }

        public void UseMana(int mana)
        {
            if (NetworkManager.Singleton.IsHost)
                _netMana.Value = Mathf.Clamp(_netMana.Value - mana, 0, Consts.MaximumMana);
        }

        public void RestoreMana(int mana)
        {
            if (NetworkManager.Singleton.IsHost)
                _netMana.Value = Mathf.Clamp(_netMana.Value + mana, 0, Consts.MaximumMana);
        }

        public void PlayAnimation(string animationName)
        {
            StartCoroutine(ProcessAnimationCoroutine(animationName));
        }

        public void ShowEmoji(CacheEmojiSource.EmojiType type, float duration = 1f)
        {
            StartCoroutine(ProcessEmojiCoroutine(type, duration));
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
                
                switch (NetworkManager.Singleton.IsHost)
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
            if (IsHost)
            {
                if (IsHostPlayerObject)
                {
                    // set host player object default position
                    transform.position = new Vector3(InitHostX * Consts.PanelX - PaddingX, 0.3f,
                        (Consts.Height - InitHostY - 1) * Consts.PanelY);
                    transform.localEulerAngles = new Vector3(0, DirRight, 0);

                    _netPosX.Value = InitHostX;
                    _netPosY.Value = InitHostY;
                }
                else
                {
                    // set clien object default position
                    transform.position = new Vector3(InitClientX * Consts.PanelX + PaddingX, 0.3f,
                        (Consts.Height - InitClientY - 1) * Consts.PanelY);
                    transform.localEulerAngles = new Vector3(0, DirLeft, 0);

                    _netPosX.Value = InitClientX;
                    _netPosY.Value = InitClientY;
                }
            }

            // Set Client Material
            if (!IsHostPlayerObject)
                GetComponentInChildren<SkinnedMeshRenderer>().material = clientMaterial;

            GameManager2.Instance.SetPlayerController(IsHostPlayerObject, this);
        }

        #endregion


        #region Coroutines

        private IEnumerator ProcessAnimationCoroutine(string animationName)
        {
            ProcessAnimation(animationName);
            yield return CacheCoroutineSource.Instance.GetSource(1f);
            ProcessAnimation(WizardAnimations.Idle);
        }

        private IEnumerator ProcessEmojiCoroutine(CacheEmojiSource.EmojiType type, float duration)
        {
            var emoji = CacheEmojiSource.Instance.GetSource(type);
            
            // Play Emoji effect
            emoji.gameObject.SetActive(true);
            emoji.transform.position = transform.position + new Vector3(0, 4, 0);
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