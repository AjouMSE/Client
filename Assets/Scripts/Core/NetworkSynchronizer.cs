using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Manager;
using Scene;
using Unity.Netcode;
using UnityEngine;

public class NetworkSynchronizer : NetworkBehaviour
{
    #region Private variables

    private NetworkVariable<bool> _hostReadyToRunTimer, _clientReadyToRunTimer;
    private NetworkVariable<bool> _hostReadyToProcessCard, _clientReadyToProcessCard;
    
    private NetworkList<int> _hostCardList, _clientCardList;

    #endregion
    
    

    #region Callbacks

    private void HostCardListOnOnListChanged(NetworkListEvent<int> e)
    {
        
    }

    private void ClientCardListOnOnListChanged(NetworkListEvent<int> e)
    {
        
    }

    #endregion
    
    private void Init()
    {
        _hostReadyToRunTimer = new NetworkVariable<bool>();
        _clientReadyToRunTimer = new NetworkVariable<bool>();
        _hostReadyToProcessCard = new NetworkVariable<bool>();
        _clientReadyToProcessCard = new NetworkVariable<bool>();
        
        _hostCardList = new NetworkList<int>();
        _clientCardList = new NetworkList<int>();
        
        ReadyToRunTimer(false);
        ReadyToProcessCards(false);

        _hostCardList.OnListChanged += HostCardListOnOnListChanged;
        _clientCardList.OnListChanged += ClientCardListOnOnListChanged;
    }
    
    
    #region Custom methods

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
    /// 
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
    /// 
    /// </summary>
    /// <param name="isReady"></param>
    [ServerRpc(RequireOwnership = false)]
    private void ClientReadyToRunTimerServerRpc(bool isReady)
    {
        _clientReadyToRunTimer.Value = isReady;
    }

    /// <summary>
    /// 
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
    /// 
    /// </summary>
    /// <param name="isReady"></param>
    [ServerRpc(RequireOwnership = false)]
    private void ClientReadyToProcessCardServerRpc(bool isReady)
    {
        _clientReadyToProcessCard.Value = isReady;
    }



    // Add card to cardList
    public void AddCardToList(int id)
    {
        if (UserManager.Instance.IsHost)
        {
            if (_hostCardList.Count < 3 && !_hostCardList.Contains(id))
            {
                _hostCardList.Add(id);
            }
        }
        else
        {
            ClientAddCardToListServerRpc(id);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClientAddCardToListServerRpc(int id)
    {
        if (_clientCardList.Count < 3 && !_clientCardList.Contains(id))
        {
            _clientCardList.Add(id);
        }
    }



    // Remove a card from cardList
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

    [ServerRpc(RequireOwnership = false)]
    public void ClientRemoveCardFromListServerRpc(int idx)
    {
        if (_clientCardList.Count > idx)
        {
            _clientCardList.RemoveAt(idx);
        }
    }



    // Remove all cards from cardList
    public void RemoveAllCardsFromList()
    {
        if (UserManager.Instance.IsHost)
        {
            _hostCardList.Clear();
        }
        else
        {
            ClientRemoveAllCardsFromListServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClientRemoveAllCardsFromListServerRpc()
    {
        _clientCardList.Clear();
    }

    

    // Get a card from cardList
    public int GetHostCardFromList(int idx)
    {
        int code = 0;
        if (_hostCardList.Count > idx)
        {
            code = _hostCardList[idx];
        }
        return code;
    }

    public int GetClientCardFromList(int idx)
    {
        int code = 0;
        if (_clientCardList.Count > idx)
        {
            code = _clientCardList[idx];
        }
        return code;
    }

    #endregion
    
    
    #region Unity event methods

    private void Awake()
    {
        Init();
    }
    #endregion
}
