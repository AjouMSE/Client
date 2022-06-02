using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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

    UserInformation[] usernum = new UserInformation[100];

    //GameObject textparent;
    public GameObject userinfoparent;
    public GameObject userinfo;


    void Start()
    {
        StartCoroutine(RankingBoardServer());
        //RankExpress(100);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator RankingBoardServer()
    {
        string url = "~/ranking/leader-board";

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("error");
        }

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
