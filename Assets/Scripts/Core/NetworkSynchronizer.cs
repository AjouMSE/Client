using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Manager;
using Scene;
using UI.Game;
using Unity.Netcode;
using UnityEngine;

namespace Core
{
    public class NetworkSynchronizer : NetworkBehaviour
    {
        #region Private constants

        private const int MaxCardCnt = 3;

        #endregion


        #region Private variables

        [Header("HUD Game Card Selection UI Controller")] 
        [SerializeField] private HUDGameCardSelectionUIController cardSelectionUIController;

        private NetworkVariable<bool> _hostReadyToRunTimer, _clientReadyToRunTimer;
        private NetworkVariable<bool> _hostReadyToProcessCard, _clientReadyToProcessCard;
        private NetworkVariable<int> _hostHP, _clientHP;
        private NetworkVariable<int> _hostMana, _clientMana;

        private NetworkList<int> _hostCardList, _clientCardList;

        #endregion


        #region Public variables

        public enum UserType
        {
            Host,
            Client
        }

        #endregion


        #region Unity event methods

        private void Awake()
        {
            Init();
        }

        #endregion


        #region Callbacks

        private void HostCardListOnChanged(NetworkListEvent<int> e)
        {
            int[] cards = new int[_hostCardList.Count];
            for (int i = 0; i < cards.Length; i++) cards[i] = _hostCardList[i];
            cardSelectionUIController.UpdateHostCardSelectionUI(cards);
        }

        private void ClientCardListOnChanged(NetworkListEvent<int> e)
        {
            int[] cards = new int[_clientCardList.Count];
            for (int i = 0; i < cards.Length; i++) cards[i] = _clientCardList[i];
            cardSelectionUIController.UpdateClientCardSelectionUI(cards);
        }

        #endregion


        #region Custom methods

        private void Init()
        {
            _hostReadyToRunTimer = new NetworkVariable<bool>();
            _clientReadyToRunTimer = new NetworkVariable<bool>();
            _hostReadyToProcessCard = new NetworkVariable<bool>();
            _clientReadyToProcessCard = new NetworkVariable<bool>();

            _hostHP = new NetworkVariable<int>();
            _clientHP = new NetworkVariable<int>();
            _hostMana = new NetworkVariable<int>();
            _clientMana = new NetworkVariable<int>();

            _hostCardList = new NetworkList<int>();
            _clientCardList = new NetworkList<int>();

            ReadyToRunTimer(false);
            ReadyToProcessCards(false);

            _hostCardList.OnListChanged += HostCardListOnChanged;
            _clientCardList.OnListChanged += ClientCardListOnChanged;
        }


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

        /// <summary>
        /// Pop element from front of the host card list
        /// </summary>
        /// <returns></returns>
        public int PopCardFromHostCardList()
        {
            int element = -1;
            if (_hostCardList.Count > 0)
            {
                element = _hostCardList[0];
                _hostCardList.RemoveAt(0);
            }

            return element;
        }

        /// <summary>
        /// Pop element from front of the client card list
        /// </summary>
        /// <returns></returns>
        public int PopCardFromClientCardList()
        {
            int element = -1;
            if (_clientCardList.Count > 0)
            {
                element = _clientCardList[0];
                _clientCardList.RemoveAt(0);
            }

            return element;
        }

        /// <summary>
        /// Remove front value from list
        /// </summary>
        /// <param name="type"></param>
        public void RemoveFrontOfList(UserType type)
        {
            switch (type)
            {
                case UserType.Host:
                    if (_hostCardList.Count > 0)
                        _hostCardList.RemoveAt(0);
                    break;

                case UserType.Client:
                    if (_clientCardList.Count > 0)
                        _clientCardList.RemoveAt(0);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int[] GetCopyList(UserType type)
        {
            int[] cards = null;
            switch (type)
            {
                case UserType.Host:
                    cards = new int[_hostCardList.Count];
                    for (int i = 0; i < cards.Length; i++)
                    {
                        cards[i] = _hostCardList[i];
                    }

                    break;

                case UserType.Client:
                    cards = new int[_clientCardList.Count];
                    for (int i = 0; i < cards.Length; i++)
                    {
                        cards[i] = _clientCardList[i];
                    }

                    break;
            }

            return cards;
        }

        #endregion
    }
}