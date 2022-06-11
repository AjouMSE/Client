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

        private int _idx;
        private BitMask.BitField30 _bitIdx;

        private string _currentAnimation;
        private Animator _animator;
        private HUDGameUserInfoUIController _userInfoUIController;

        #endregion


        #region Public Variable

        public bool IsHostPlayerObject { get; private set; }

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
            _netHp = new NetworkVariable<int>();
            _netMana = new NetworkVariable<int>();
            _netPosX = new NetworkVariable<int>();
            _netPosY = new NetworkVariable<int>();

            // todo-add OnValueChanged callback

            _animator = GetComponent<Animator>();
            IsHostPlayerObject = !(IsHost ^ IsOwner);
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

        public void RestoreMana()
        {
            var mana = Mathf.Clamp(GameManager2.Instance.TurnValue + _netMana.Value, 0, MaxMana);
            _netMana.Value = mana;
        }

        public void PlayAnimation(string animationName)
        {
            StartCoroutine(ProcessAnimationCoroutine(animationName));
        }

        #endregion


        #region Network methods

        public override void OnNetworkSpawn()
        {
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
                    _netHp.Value = DefaultHp;
                    _netMana.Value = DefaultMana;
                }
                else
                {
                    // set clien object default position
                    transform.position = new Vector3(InitClientX * Consts.PanelX + PaddingX, 0.3f,
                        (Consts.Height - InitClientY - 1) * Consts.PanelY);
                    transform.localEulerAngles = new Vector3(0, DirLeft, 0);

                    _netPosX.Value = InitClientX;
                    _netPosY.Value = InitClientY;
                    _netHp.Value = DefaultHp;
                    _netMana.Value = DefaultMana;
                }
            }

            if (!IsHostPlayerObject)
                GetComponentInChildren<SkinnedMeshRenderer>().material = clientMaterial;

            GameManager2.Instance.SetPlayerController(this);
        }

        #endregion


        #region Coroutines

        private IEnumerator ProcessAnimationCoroutine(string animationName)
        {
            ProcessAnimation(animationName);
            yield return CacheCoroutineSource.Instance.GetSource(1f);
            ProcessAnimation(WizardAnimations.Idle);
        }

        #endregion
    }
}