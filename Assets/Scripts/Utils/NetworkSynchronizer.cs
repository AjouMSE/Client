using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Manager;
using Scene;
using Unity.Netcode;
using UnityEngine;

public class NetworkSynchronizer : NetworkBehaviour
{
    private static NetworkSynchronizer _instance;
    
    public NetworkVariable<bool> hostReadyToRunTimer, clientReadyToRunTimer;
    public NetworkVariable<bool> hostReadToProcessCard, clientReadyToProcessCard;
    
    public NetworkList<int> hostCardList, clientCardList;

    public GameSceneController controller;

    public static NetworkSynchronizer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("NetworkSynchronizer").GetComponent<NetworkSynchronizer>();
            }
            
            return _instance;
        }
    }

    private void Awake()
    {
        hostCardList = new NetworkList<int>();
        clientCardList = new NetworkList<int>();
    }

    public void Init()
    {
        ReadToRunTimer(false);
        ReadToProcessCard(false);
        
        hostCardList.OnListChanged += HostCardListOnOnListChanged;
        clientCardList.OnListChanged += ClientCardListOnOnListChanged;
    }

    #region Callbacks

    private void HostCardListOnOnListChanged(NetworkListEvent<int> e)
    {
        controller.UpdateCardUI(0);
    }
    
    private void ClientCardListOnOnListChanged(NetworkListEvent<int> e)
    {
        controller.UpdateCardUI(1);
    }

    #endregion


    #region Network methods 
    
    // Check that user is ready to run card selection timer
    public void ReadToRunTimer(bool isReady)
    {
        if (UserManager.Instance.IsHost)
            hostReadyToRunTimer.Value = isReady;
        else
            ClientReadToRunTimerServerRpc(isReady);
    }
    
    [ServerRpc(RequireOwnership = false)] 
    public void ClientReadToRunTimerServerRpc(bool isReady)
    {
        clientReadyToRunTimer.Value = isReady;
    }
    
    
    
    // Add card to cardList
    public void AddCardToList(int id)
    {
        if (UserManager.Instance.IsHost)
        {
            if (hostCardList.Count < 3 && !hostCardList.Contains(id))
            {
                hostCardList.Add(id);
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
        if (clientCardList.Count < 3 && !clientCardList.Contains(id))
        {
            clientCardList.Add(id);
        }
    }
    
    
    
    // Remove a card from cardList
    public void RemoveCardFromList(int idx)
    {
        if (UserManager.Instance.IsHost)
        {
            if (hostCardList.Count > idx)
            {
                hostCardList.RemoveAt(idx);
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
        if (clientCardList.Count > idx)
        {
            clientCardList.RemoveAt(idx);
        }
    }


    
    // Remove all cards from cardList
    public void RemoveAllCardsFromList()
    {
        if (UserManager.Instance.IsHost)
        {
            hostCardList.Clear();
        }
        else
        {
            ClientRemoveAllCardsFromListServerRpc();
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void ClientRemoveAllCardsFromListServerRpc()
    {
        clientCardList.Clear();
    }
    
    
    
    // Check that user is ready to process cardList
    public void ReadToProcessCard(bool isReady)
    {
        if (UserManager.Instance.IsHost)
            hostReadToProcessCard.Value = isReady;
        else
            ClientReadToProcessCardServerRpc(isReady);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClientReadToProcessCardServerRpc(bool isReady)
    {
        clientReadyToProcessCard.Value = isReady;
    }
    
    #endregion
}
