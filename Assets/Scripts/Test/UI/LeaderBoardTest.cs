using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils;
using User = Utils.Packet.User;
using UserList = Utils.Packet.UserList;

public class LeaderBoardTest : MonoBehaviour
{
    void Start()
    {
        // Variable about page number
        var pageNum = 1;
        
        // Http Get Request
        HttpRequestManager.Instance.Get($"/ranking/leader-board?page={pageNum}", Callback);
    }

    private void Callback(UnityWebRequest req)
    {
        if (req.result == UnityWebRequest.Result.Success)
        {
            // Get json string from server
            string json = req.downloadHandler.text;
            Debug.Log(json);
            
            // print all items in user list
            UserList userList = JsonUtility.FromJson<UserList>(json);
            Debug.Log(userList.ToString());

            for (int i = 0; i < userList.users.Count; i++)
            {
                // Do something
                User user = userList.users[i];
                
                /* user.id, user.ranking, user.score ... etc
                 * textInfo.text = user.id;
                 * textInfo.text = user.score.ToString();
                 * ...
                 */
                Debug.Log(user.ToString());
            }
        } 
        else if (req.result == UnityWebRequest.Result.ProtocolError)
        {
            // Error code (ex 400)
            Debug.Log($"Protocol error: {req.error}");
        }
        else
        {
            // Error code (ex 500)
            Debug.Log($"Another error: {req.error}");
        }
    }
}