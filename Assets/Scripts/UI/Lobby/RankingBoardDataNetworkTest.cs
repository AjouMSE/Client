using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Manager.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils;
using User = Utils.Packet.User;
using UserList = Utils.Packet.UserList;

public class RankingBoardDataNetworkTest : MonoBehaviour
{
   
    
    struct UserInformation//나중에 일로 정보 받아옴
    {
        public int ranking;
        public string nickname;
        public string tier;
        public int score;
        public int win;
        public int lose;
        public int draw;
    }


     UserInformation[] usernum;

    private int pageNum=1;
    public GameObject[] userinfo = new GameObject[10];


    void Start()
    {
        // Variable about page number
        pageNum = 1;

        // Http Get Request
        NetHttpRequestManager.Instance.Get($"/ranking/leader-board?page={pageNum}", Callback);
        //RankExpress(100);

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
                usernum[i].ranking = i;
                usernum[i].nickname = user.nickname;
                usernum[i].score = user.score;
                usernum[i].win = user.win;
                usernum[i].lose = user.lose;
                usernum[i].draw = user.draw;
                /* user.id, user.ranking, user.score ... etc
                 * textInfo.text = user.id;
                 * textInfo.text = user.score.ToString();
                 * ...
                 */
                Debug.Log(user.ToString());
            }
            RankExpress(userList.users.Count);
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
    // Update is called once per frame
    void Update()
    {
        
    }
  
    
    private void RankExpress(int rankusernum)
    {
      
        for (int i = 0; i < rankusernum; i++)
        {


            if (rankusernum > 9)
            {
                GameObject copytext = Instantiate(userinfo[0 % 10]);
                copytext.GetComponentsInChildren<Text>()[0].text = usernum[i].ranking.ToString();
                copytext.GetComponentsInChildren<Text>()[1].text = usernum[i].nickname;
                copytext.GetComponentsInChildren<Text>()[2].text = "null";//.ToString();
                copytext.GetComponentsInChildren<Text>()[3].text = usernum[i].score.ToString();
                copytext.GetComponentsInChildren<Text>()[4].text = usernum[i].win.ToString();
                copytext.GetComponentsInChildren<Text>()[5].text = usernum[i].draw.ToString();
                copytext.GetComponentsInChildren<Text>()[6].text = usernum[i].lose.ToString();
                userinfo[i].SetActive(false);
            }
            else
            {
                userinfo[i].GetComponentsInChildren<Text>()[0].text = usernum[i].ranking.ToString();
                userinfo[i].GetComponentsInChildren<Text>()[1].text = usernum[i].nickname;
                userinfo[i].GetComponentsInChildren<Text>()[2].text = "null";//.ToString();
                userinfo[i].GetComponentsInChildren<Text>()[3].text = usernum[i].score.ToString();
                userinfo[i].GetComponentsInChildren<Text>()[4].text = usernum[i].win.ToString();
                userinfo[i].GetComponentsInChildren<Text>()[5].text = usernum[i].draw.ToString();
                userinfo[i].GetComponentsInChildren<Text>()[6].text = usernum[i].lose.ToString();
                userinfo[i].SetActive(true);
            }
            

           
            
        }        
    }
    
    
  
}
