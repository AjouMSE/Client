using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace Manager.Net
{
    public class NetGameStatusManager : NetworkBehaviour
    {
        #region Private constants

        private const int MaxCardCnt = 3;

        #endregion


        #region Private variables

        private static NetGameStatusManager _instance;
        private static readonly object _lockObject = new object();

        private NetworkVariable<bool> _hostReadyToRunTimer, _clientReadyToRunTimer;
        private NetworkVariable<bool> _hostReadyToProcessCard, _clientReadyToProcessCard;

        private NetworkList<int> _hostCardList, _clientCardList;

        #endregion


        #region Public variables

        // Simple singleton pattern
        public static NetGameStatusManager Instance
        {
            get
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new GameObject().AddComponent<NetGameStatusManager>();
                        var obj = _instance.gameObject;
                        obj.AddComponent<NetworkObject>();
                        obj.name = $"(s) {typeof(NetGameStatusManager)}";
                        DontDestroyOnLoad(_instance);
                    }
                }

                return _instance;
            }
        }

        #endregion


        #region Unity event methods

        #endregion


        #region Private methods

        public void Init()
        {
            _hostReadyToRunTimer = new NetworkVariable<bool>();
            _clientReadyToRunTimer = new NetworkVariable<bool>();
            _hostReadyToProcessCard = new NetworkVariable<bool>();
            _clientReadyToProcessCard = new NetworkVariable<bool>();

            _hostCardList = new NetworkList<int>();
            _clientCardList = new NetworkList<int>();

            _hostCardList.OnListChanged += e =>
            {
                
            };

            _clientCardList.OnListChanged += e =>
            {

            };
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Checks that both host and client are ready to run card selection timer
        /// </summary>
        /// <returns></returns>
        public bool BothReadyToRunTimer()
        {
            return _hostReadyToRunTimer.Value && _clientReadyToRunTimer.Value;
        }

        /// <summary>
        /// Checks that both host and client are ready to process cards in card list
        /// </summary>
        /// <returns></returns>
        public bool BothReadyToProcessCards()
        {
            return _hostReadyToProcessCard.Value && _clientReadyToProcessCard.Value;
        }

        #endregion


        #region Network methods

        /// <summary>
        /// Set bool value of ReadyToRunTimer
        /// </summary>
        /// <param name="isReady"></param>
        public void ReadyToRunTimer(bool isReady)
        {
            if (UserManager.Instance.IsHost)
                _hostReadyToRunTimer.Value = isReady;
            else
                ClientReadyToRunTimerServerRpc(isReady);
        }

        /// <summary>
        /// Set bool value of ReadyToRunTimer (ServerRpc, client -> server)
        /// </summary>
        /// <param name="isReady"></param>
        [ServerRpc(RequireOwnership = false)]
        private void ClientReadyToRunTimerServerRpc(bool isReady)
        {
            _clientReadyToRunTimer.Value = isReady;
        }

        /// <summary>
        /// Set bool value of ReadyToProcessCard
        /// </summary>
        /// <param name="isReady"></param>
        public void ReadyToProcessCards(bool isReady)
        {
            if (UserManager.Instance.IsHost)
                _hostReadyToProcessCard.Value = isReady;
            else
                ClientReadyToProcessCardServerRpc(isReady);
        }

        /// <summary>
        /// Set bool value of ReadyToProcessCard (ServerRpc, client -> server)
        /// </summary>
        /// <param name="isReady"></param>
        [ServerRpc(RequireOwnership = false)]
        private void ClientReadyToProcessCardServerRpc(bool isReady)
        {
            _clientReadyToProcessCard.Value = isReady;
        }


        /// <summary>
        /// Add card to card list
        /// </summary>
        /// <param name="id"></param>
        public void AddCardToList(int id)
        {
            if (UserManager.Instance.IsHost)
            {
                if (_hostCardList.Count < MaxCardCnt && !_hostCardList.Contains(id))
                {
                    _hostCardList.Add(id);
                }
            }
            else
            {
                ClientAddCardToListServerRpc(id);
            }
        }

        /// <summary>
        /// Add card to card list (ServerRpc, client -> server)
        /// </summary>
        /// <param name="id"></param>
        [ServerRpc(RequireOwnership = false)]
        private void ClientAddCardToListServerRpc(int id)
        {
            if (_clientCardList.Count < MaxCardCnt && !_clientCardList.Contains(id))
            {
                _clientCardList.Add(id);
            }
        }

        /// <summary>
        /// Remove card from card list
        /// </summary>
        /// <param name="idx"></param>
        public void RemoveCardFromList(int idx)
        {
            if (UserManager.Instance.IsHost)
            {
                if (_hostCardList.Count > idx)
                {
                    _hostCardList.RemoveAt(idx);
                }
            }
            else
            {
                ClientRemoveCardFromListServerRpc(idx);
            }
        }

        /// <summary>
        /// Remove card from card list (ServerRpc, client -> server)
        /// </summary>
        /// <param name="idx"></param>
        [ServerRpc(RequireOwnership = false)]
        private void ClientRemoveCardFromListServerRpc(int idx)
        {
            if (_clientCardList.Count > idx)
            {
                _clientCardList.RemoveAt(idx);
            }
        }

        #endregion
    }
}