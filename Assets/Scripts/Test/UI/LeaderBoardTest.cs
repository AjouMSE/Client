using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils;

public class LeaderBoardTest : MonoBehaviour
{
    [Serializable]
    public struct Users
    {
        public List<Packet.User> users;
    }

    void Start()
    {
        var pageNum = 1;
        HttpRequestManager.Instance.Get($"/ranking/leader-board?page={pageNum}", Callback);
    }

    private void Callback(UnityWebRequest req)
    {
        if (req.result == UnityWebRequest.Result.Success)
        {
            string json = req.downloadHandler.text;
            Users userStruct = JsonUtility.FromJson<Users>(json);

            for (int i = 0; i < userStruct.users.Count; i++)
            {
                Packet.User user = userStruct.users[i];
                
                Debug.Log(user.ToString());
            }
        } 
        else if (req.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log($"Protocol error: {req.error}");
        }
        else
        {
            Debug.Log($"Another eror: {req.error}");
        }
    }
}