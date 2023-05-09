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
//using usercount = Utils.Packet.


public class RankingBoardServer : MonoBehaviour
{
    private int rankusernum = 23;
    private int rankpagenum = 0;
    private int lastpagenum;
    private int pageNum;
    public GameObject rightbutton;
    public GameObject leftbutton;
    public GameObject parenttext;
    public GameObject parent;
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


    UserInformation[] usernum = new UserInformation[100];




    GameObject[] userinfo = new GameObject[100];

    void Start()
    {
        if (rankusernum > 10)
        {
            if (rankusernum % 10 == 0)
            {
                lastpagenum = rankusernum / 10 - 1;
            }
            else
            {
                lastpagenum = rankusernum / 10;
                Debug.Log(lastpagenum);
            }
        }
        if (rankusernum > 10)
        {
            rightbutton.SetActive(true);
        }
        else
        {
            rightbutton.SetActive(false);
        }
        leftbutton.SetActive(false);

        pageNum = 1;
        NetHttpRequestManager.Instance.Get($"/ranking/leader-board?page={pageNum.ToString()}", Callback);
        RankExpress(rankusernum);

    }

    private void Callback(UnityWebRequest req)
    {
        using (req)
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
              
                    User user = userList.users[i];
                    usernum[i].nickname = user.nickname;
                    usernum[i].score = user.score;
                    usernum[i].win = user.win;
                    usernum[i].lose = user.lose;
                    usernum[i].draw = user.draw;
                    usernum[i].tier = TierCal(usernum[i].score);            
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

    private void RankExpress(int rankusernum)
    {


        for (int i = 0; i < rankusernum; i++)
        {
            int ranknumber = i + 1;

            if (i == 0)
            {

                parenttext.GetComponentsInChildren<Text>()[0].text = ranknumber.ToString();
                parenttext.GetComponentsInChildren<Text>()[1].text = usernum[i].score.ToString();
                parenttext.GetComponentsInChildren<Text>()[2].text = usernum[i].nickname;
                parenttext.GetComponentsInChildren<Text>()[3].text = usernum[i].tier;
                parenttext.GetComponentsInChildren<Text>()[4].text = usernum[i].win.ToString(); 
                parenttext.GetComponentsInChildren<Text>()[5].text = usernum[i].draw.ToString();
                parenttext.GetComponentsInChildren<Text>()[6].text = usernum[i].lose.ToString();
                parenttext.SetActive(true);
                userinfo[i] = parenttext;


            }
            if (i < 10 && i > 0)
            {

                GameObject copytext = Instantiate(parenttext, parent.transform);
                copytext.GetComponentsInChildren<Text>()[0].text = ranknumber.ToString();
                copytext.GetComponentsInChildren<Text>()[1].text = usernum[i].score.ToString();
                copytext.GetComponentsInChildren<Text>()[2].text = usernum[i].nickname;
                copytext.GetComponentsInChildren<Text>()[3].text = usernum[i].tier;
                copytext.GetComponentsInChildren<Text>()[4].text = usernum[i].win.ToString();
                copytext.GetComponentsInChildren<Text>()[5].text = usernum[i].draw.ToString();
                copytext.GetComponentsInChildren<Text>()[6].text = usernum[i].lose.ToString();
                copytext.SetActive(true);
                userinfo[i] = copytext;
            }
            if (i > 9)
            {

                GameObject copytext = Instantiate(userinfo[0 % 10], parent.transform);
                copytext.SetActive(false);
                copytext.GetComponentsInChildren<Text>()[0].text = ranknumber.ToString();
                copytext.GetComponentsInChildren<Text>()[1].text = usernum[i].score.ToString();
                copytext.GetComponentsInChildren<Text>()[2].text = usernum[i].nickname;
                copytext.GetComponentsInChildren<Text>()[3].text = usernum[i].tier;
                copytext.GetComponentsInChildren<Text>()[4].text = usernum[i].win.ToString();
                copytext.GetComponentsInChildren<Text>()[5].text = usernum[i].draw.ToString();
                copytext.GetComponentsInChildren<Text>()[6].text = usernum[i].lose.ToString();
                userinfo[i] = copytext;
            }
        }
    }
    public void RightButton()
    {
        rankpagenum++;
        if (rankpagenum != 0)
        {
            leftbutton.SetActive(true);
        }
        if (rankpagenum == lastpagenum)
        {
            rightbutton.SetActive(false);
        }
        if (rankpagenum == 1)
        {
            parenttext.SetActive(false);
            for (int i = 0; i < 10; i++)
            {
                userinfo[i].SetActive(false);
            }
            for (int i = 10; i < 20; i++)
            {
                userinfo[i].SetActive(true);
            }
        }
        else if (rankpagenum == lastpagenum)
        {
            for (int i = (rankpagenum - 1) * 10; i < rankusernum; i++)
            {
                userinfo[i].SetActive(false);
            }
            for (int i = (rankpagenum) * 10; i < 10 * (rankpagenum + 1); i++)
            {
                userinfo[i].SetActive(true);
            }
        }
        else
        {
            for (int i = (rankpagenum - 1) * 10; i < 10 * (rankpagenum); i++)
            {
                userinfo[i].SetActive(false);
            }
            for (int i = (rankpagenum) * 10; i < 10 * (rankpagenum + 1); i++)
            {
                userinfo[i].SetActive(true);
            }
        }



    }
    public void leftButton()
    {
        rankpagenum--;
        if (rankpagenum == lastpagenum - 1)
        {
            for (int i = 10 * (rankpagenum + 1); i < rankusernum; i++)
            {
                userinfo[i].SetActive(false);
            }
            for (int i = rankpagenum * 10; i < 10 * (rankpagenum + 1); i++)
            {
                userinfo[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 10 * (rankpagenum + 1); i < 10 * (rankpagenum + 2); i++)
            {
                userinfo[i].SetActive(false);
            }
            for (int i = rankpagenum * 10; i < 10 * (rankpagenum + 1); i++)
            {
                userinfo[i].SetActive(true);
            }
        }

        if (rankpagenum == 0)
        {
            leftbutton.SetActive(false);
        }
        if (rightbutton.activeSelf == false && rankpagenum != lastpagenum)
        {
            rightbutton.SetActive(true);
        }
    }
    public String TierCal(int score)
    {
         String tier;

        if(score>400)
        {
            tier = "A+";
        }
        else if(score>=350&&score<400)
        {
            tier = "A";
        }
        else if(score >= 300 && score < 350)
        {
            tier = "B+";
        }
        else if (score >= 250 && score < 300)
        {
            tier = "B";
        }
        else if (score >= 200 && score < 250)
        {
            tier = "C+";
        }
        else
        {
            tier = "C";
        }

        return tier;
    }
}
