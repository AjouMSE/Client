using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.Netcode;
using UnityEngine;

public class SkillCard : MonoBehaviour
{
    public int idx, id;

    void SyncCard()
    {
        int[] cards = GameManager.Instance.selectedCardList.ToArray();
        
        if(NetworkManager.Singleton.IsServer)
            NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<WizardController>().SyncCardClientRpc(cards);
        else
            NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<WizardController>().SyncCardServerRpc(cards);
    }

    public void OnCardAddBtnClickListener()
    {
        if (GameManager.Instance.selectedCardList.Count < 3 && !GameManager.Instance.selectedCardList.Contains(id))
        {
            GameManager.Instance.selectedCardList.Add(id);
            SyncCard();
        }
    }

    public void OnCardRemoveBtnClickListener()
    {
        GameManager.Instance.selectedCardList.RemoveAt(idx);
        SyncCard();
    }
}
