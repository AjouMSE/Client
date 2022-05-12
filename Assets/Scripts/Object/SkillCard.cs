using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : MonoBehaviour
{
    public int idx, id;

    public void OnCardAddBtnClickListener()
    {
        NetworkSynchronizer.Instance.AddCardToList(id);
    }

    public void OnCardRemoveBtnClickListener()
    {
        Debug.Log("Remove!");
        NetworkSynchronizer.Instance.RemoveCardFromList(idx);
    }
}
