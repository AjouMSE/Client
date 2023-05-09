using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UI.Game.CardSelection;
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
        
        private NetworkVariable<bool> _hostReadyToRunTimer, _clientReadyToRunTimer;
        private NetworkVariable<bool> _hostReadyToProcessCard, _clientReadyToProcessCard;

        private NetworkList<int> _hostCardList, _clientCardList;
        private bool _hostProcessCard;

        #endregion


        #region Public variables

        public HUDGameSelectedCardUIController SelectedCardUIController { get; set; }

        #endregion


        #region Public variables

        // Simple singleton pattern
        public static NetGameStatusManager Instance { get; private set; }

        #endregion


        #region Unity event methods

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;   
            }
        }

        private void OnApplicationQuit()
        {
            DisposeAllStatus();
        }

        #endregion


        #region Private methods

        private void DisposeAllStatus()
        {
            if(_hostReadyToRunTimer != null)
                _hostReadyToRunTimer.Dispose();
            
            if(_hostReadyToProcessCard != null)
                _hostReadyToProcessCard.Dispose();
            
             if(_clientReadyToRunTimer != null)
                _clientReadyToRunTimer.Dispose();
             
             if(_clientReadyToProcessCard != null)
                _clientReadyToProcessCard.Dispose();
             
             if(_hostCardList != null)
                 _hostCardList.Dispose();
             
             if(_clientCardList != null)
                 _clientCardList.Dispose();

             
            _hostReadyToRunTimer = null;
            _hostReadyToProcessCard = null;
            _clientReadyToRunTimer = null;
            _clientReadyToProcessCard = null;
            _hostCardList = null;
            _clientCardList = null;
        }

        private void ClearAllStatus()
        {
            _hostReadyToRunTimer.Value = _hostReadyToProcessCard.Value =
                _clientReadyToRunTimer.Value = _clientReadyToProcessCard.Value = false;
            
            // _hostCardList.Clear();
            // _clientCardList.Clear();
        }

        private void CreateNetworkValue()
        {
            _hostReadyToRunTimer = new NetworkVariable<bool>();
            _hostReadyToProcessCard = new NetworkVariable<bool>();
            _clientReadyToRunTimer = new NetworkVariable<bool>();
            _clientReadyToProcessCard = new NetworkVariable<bool>();
            
            _hostCardList = new NetworkList<int>();
            _clientCardList = new NetworkList<int>();

            _hostCardList.OnListChanged += e =>
            {
                if (SelectedCardUIController == null) return;

                var cards = CopyHostCardList();
                SelectedCardUIController.UpdateHostCardSelectionUI(cards);
                if (NetworkManager.Singleton.IsServer)
                    SelectedCardUIController.UpdateInvalidCards();
            };

            _clientCardList.OnListChanged += e =>
            {
                if (SelectedCardUIController == null) return;

                var cards = CopyClientCardList();
                SelectedCardUIController.UpdateClientCardSelectionUI(cards);
                if (!NetworkManager.Singleton.IsServer)
                    SelectedCardUIController.UpdateInvalidCards();
            };
            
            ReadyToRunTimer(false);
            ReadyToProcessCard(false);
        }

        #endregion


        #region Public methods

        public void Init()
        {
            if(UserManager.Instance.IsHost)
                DisposeAllStatus();
            
            CreateNetworkValue();
            switch (UserManager.Instance.IsHost)
            {
                case true:
                    ClearAllStatus();
                    break;
                case false:
                    _hostProcessCard = false;
                    break;
            }
        }

        public string GetStatusDump()
        {
            var sb = new StringBuilder();
            sb.Append("Host Ready To Run Timer: ");
            sb.Append(_hostReadyToRunTimer.Value);
            sb.Append('\n');
            sb.Append("Client Ready To Run Timer: ");
            sb.Append(_clientReadyToRunTimer.Value);
            sb.Append('\n');
            sb.Append("Host Ready To Process Card: ");
            sb.Append(_hostReadyToProcessCard.Value);
            sb.Append('\n');
            sb.Append("Client Ready To Process Card: ");
            sb.Append(_clientReadyToProcessCard.Value);
            sb.Append('\n');
            if (!NetworkManager.Singleton.IsServer)
            {
                sb.Append("Host Processing card: ");
                sb.Append(_hostProcessCard);
            }

            return sb.ToString();
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsHostProcessingCard()
        {
            return _hostProcessCard;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int[] CopyHostCardList()
        {
            var cards = new int[_hostCardList.Count];
            for (var i = 0; i < cards.Length; i++)
            {
                cards[i] = _hostCardList[i];
            }

            return cards;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int[] CopyClientCardList()
        {
            var cards = new int[_clientCardList.Count];
            for (var i = 0; i < cards.Length; i++)
            {
                cards[i] = _clientCardList[i];
            }

            return cards;
        }

        #endregion


        #region Network methods

        /// <summary>
        /// Set bool value of ReadyToRunTimer
        /// </summary>
        /// <param name="isReady"></param>
        public void ReadyToRunTimer(bool isReady)
        {
            if (NetworkManager.Singleton.IsServer)
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
        public void ReadyToProcessCard(bool isReady)
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
            if (NetworkManager.Singleton.IsServer)
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
        /// Poll Card from list (remove first element in list)
        /// </summary>
        /// <param name="isHostList"></param>
        public void PollCardFromList(bool isHostList)
        {
            if (!NetworkManager.Singleton.IsServer) return;

            if (isHostList)
            {
                if (_hostCardList.Count > 0)
                    _hostCardList.RemoveAt(0);
            }
            else
            {
                if (_clientCardList.Count > 0)
                    _clientCardList.RemoveAt(0);
            }
        }

        /// <summary>
        /// Remove card from card list
        /// </summary>
        /// <param name="idx"></param>
        public void RemoveCardFromList(int idx)
        {
            if (NetworkManager.Singleton.IsServer)
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
        /// 
        /// </summary>
        /// <param name="isProcessing"></param>
        [ClientRpc]
        public void HostProcessCardClientRpc(bool isProcessing)
        {
            if (NetworkManager.Singleton.IsHost) return;
            _hostProcessCard = isProcessing;
        }

        #endregion
    }
}