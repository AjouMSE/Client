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
    /*
    public Text rankingtext;
    public Text scoretext;
    public Text nicknametext;
    public Text tiertext;
    public Text wintext;
    public Text drawtext;
    public Text losetext;
    */
    //public int[] usernum;//나중에 일로 각 티어별 유저 수 받아옴
    // Start is called before the first frame update

     UserInformation[] usernum;

    //GameObject textparent;
    //public GameObject userinfoparent;
    public GameObject[] userinfo = new GameObject[10];


    void Start()
    {
        // Variable about page number
        var pageNum = 1;

        // Http Get Request
        HttpRequestManager.Instance.Get($"/ranking/leader-board?page={pageNum}", Callback);
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
  
    /*
    private void RankExpress(int rankusernum)
    {
        usernum[0].ranking = 1;
        usernum[0].score = Random.Range(0, 1000);
        usernum[0].win = Random.Range(0, 1000);
        usernum[0].lose = Random.Range(0, 1000);
        usernum[0].draw = Random.Range(0, 1000);
        userinfo.GetComponentsInChildren<Text>()[0].text = usernum[0].ranking.ToString();
        userinfo.GetComponentsInChildren<Text>()[1].text = "null";
        userinfo.GetComponentsInChildren<Text>()[2].text = "null";//.ToString();
        userinfo.GetComponentsInChildren<Text>()[3].text = usernum[0].score.ToString();
        userinfo.GetComponentsInChildren<Text>()[4].text = usernum[0].win.ToString();
        userinfo.GetComponentsInChildren<Text>()[5].text = usernum[0].draw.ToString();
        userinfo.GetComponentsInChildren<Text>()[6].text = usernum[0].lose.ToString();
        for (int i = 1; i < rankusernum; i++)
        {
            GameObject copytext = Instantiate(userinfo, userinfoparent.transform);
           
            

            //usernum[i].nickname = "null";
            usernum[i].ranking = i+1;
            usernum[i].score = Random.Range(0, 1000);
            usernum[i].win = Random.Range(0, 1000);
            usernum[i].lose = Random.Range(0, 1000);
            usernum[i].draw = Random.Range(0, 1000);
            //usernum[i].tier = "null";
            // Text copyranking = Instantiate(scoretext, copytext.transform);
            copytext.GetComponentsInChildren<Text>()[0].text = usernum[i].ranking.ToString();
            copytext.GetComponentsInChildren<Text>()[1].text = "null";
            copytext.GetComponentsInChildren<Text>()[2].text = "null";//.ToString();
            copytext.GetComponentsInChildren<Text>()[3].text = usernum[i].score.ToString();
            copytext.GetComponentsInChildren<Text>()[4].text = usernum[i].win.ToString();
            copytext.GetComponentsInChildren<Text>()[5].text = usernum[i].draw.ToString();
            copytext.GetComponentsInChildren<Text>()[6].text = usernum[i].lose.ToString();
        }        
    }
    */
  
}
