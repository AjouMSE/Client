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
        var pageNum = 1;
        HttpRequestManager.Instance.Init();
        HttpRequestManager.Instance.Get($"/ranking/leader-board?page={pageNum.ToString()}", Callback);
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
            

            for (int i = 0; i < userList.totalCount; i++)
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
            Debug.Log($"Protocol error: {req.error} {req.responseCode.ToString()}");
        }
        else
        {
            // Error code (ex 500)
            Debug.Log($"Another error: {req.error} {req.responseCode.ToString()}");
        }
    }
}