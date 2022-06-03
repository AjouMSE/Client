using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingBoardDataTest : MonoBehaviour
{
    private int rankusernum = 100;
    private int rankpagenum = 0;
    private int lastpagenum;
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

    //GameObject textparent;


    GameObject[] userinfo = new GameObject[100];

    void Start()
    {
        if(rankusernum%10==0)
        {
            lastpagenum = rankusernum / 10;
        }
        else
        {
            lastpagenum = rankusernum / 10 + 1;
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
        RankExpress(rankusernum);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RankExpress(int rankusernum)
    {
       

        for (int i = 0; i < rankusernum; i++)
        {


            if (i == 0)
            {

                parenttext.GetComponentsInChildren<Text>()[0].text = i+1.ToString();
                parenttext.GetComponentsInChildren<Text>()[1].text = usernum[i].nickname;
                parenttext.GetComponentsInChildren<Text>()[2].text = "null";//.ToString();
                parenttext.GetComponentsInChildren<Text>()[3].text = Random.Range(0, 1000).ToString();
                parenttext.GetComponentsInChildren<Text>()[4].text = Random.Range(0, 1000).ToString();
                parenttext.GetComponentsInChildren<Text>()[5].text = Random.Range(0, 1000).ToString();
                parenttext.GetComponentsInChildren<Text>()[6].text = Random.Range(0, 1000).ToString();
                parenttext.SetActive(true);
                userinfo[i] = parenttext;

            }
            if(i<10&&i>0)
            {
                GameObject copytext = Instantiate(parenttext, parent.transform);

                copytext.GetComponentsInChildren<Text>()[0].text = i+1.ToString();
                copytext.GetComponentsInChildren<Text>()[1].text = usernum[i].nickname;
                copytext.GetComponentsInChildren<Text>()[2].text = "null";//.ToString();
                copytext.GetComponentsInChildren<Text>()[3].text = Random.Range(0, 1000).ToString();
                copytext.GetComponentsInChildren<Text>()[4].text = Random.Range(0, 1000).ToString();
                copytext.GetComponentsInChildren<Text>()[5].text = Random.Range(0, 1000).ToString();
                copytext.GetComponentsInChildren<Text>()[6].text = Random.Range(0, 1000).ToString();

                //copytext.SetActive(true);
                userinfo[i] = copytext;
            }
            if(i>9)
            {
                GameObject copytext = Instantiate(userinfo[0 % 10], parent.transform);

                copytext.SetActive(false);
                copytext.GetComponentsInChildren<Text>()[0].text = i+1.ToString();
                copytext.GetComponentsInChildren<Text>()[1].text = usernum[i].nickname;
                copytext.GetComponentsInChildren<Text>()[2].text = "null";//.ToString();
                copytext.GetComponentsInChildren<Text>()[3].text = Random.Range(0, 1000).ToString();
                copytext.GetComponentsInChildren<Text>()[4].text = Random.Range(0, 1000).ToString();
                copytext.GetComponentsInChildren<Text>()[5].text = Random.Range(0, 1000).ToString();
                copytext.GetComponentsInChildren<Text>()[6].text = Random.Range(0, 1000).ToString();
                userinfo[i] = copytext;
            }
        }
    }
    public void RightButton()
    {
        rankpagenum++;
        if(rankpagenum!=lastpagenum)
        {
            leftbutton.SetActive(true);
        }
        
        for(int i = rankpagenum-1; i<10*(rankpagenum-1);i++)
        {
            userinfo[i].SetActive(false);
        }
        for(int i=(rankpagenum-1)*10;i<10*rankpagenum;i++)
        {
            userinfo[i].SetActive(true);
        }


        if(rankpagenum==lastpagenum)
        {
            rightbutton.SetActive(false);
        }
    }
    public void leftButton()
    {
        rankpagenum--;

        for (int i = 10*(rankpagenum+1); i < 10 * (rankpagenum + 2); i++)
        {
            userinfo[i].SetActive(false);
        }
        for (int i = rankpagenum * 10; i < 10 * (rankpagenum+1); i++)
        {
            userinfo[i].SetActive(true);
        }


        if (rankpagenum==1)
        {
            leftbutton.SetActive(false);
        }
    }
}
