using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingBoardDataTest : MonoBehaviour
{

    
    struct UserInformation//나중에 일로 정보 받아옴
    {
        public int ranking;
        public int score;
        public int win;
        public int lose;
        public int draw;
    }
    public Text[] text;
    public GameObject[] parenttext;
    //public int[] usernum;//나중에 일로 각 티어별 유저 수 받아옴
    // Start is called before the first frame update

    UserInformation[] aplus = new UserInformation[100];
    UserInformation[] azero = new UserInformation[100];
    UserInformation[] bplus = new UserInformation[100];
    UserInformation[] bzero = new UserInformation[100];
    UserInformation[] cplus = new UserInformation[100];
    UserInformation[] czero = new UserInformation[100];
 



    void Start()
    {
        AplusRankingExpression(100);
        AzeroRankingExpression(100);
        BplusRankingExpression(100);
        BzeroRankingExpression(100);
        CplusRankingExpression(100);
        CzeroRankingExpression(100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void AplusRankingExpression(int rankusernum)
    {                  
        for (int i = 0; i < rankusernum; i++)
        {
            Debug.Log(i);                       
            aplus[i].ranking = i+1;
            aplus[i].score = Random.Range(0, 1000);
            aplus[i].win = Random.Range(0, 1000);
            aplus[i].lose = Random.Range(0, 1000);
            aplus[i].draw = Random.Range(0, 1000);
            if (i == 0)
            {
                text[0].text = "Ranking " + aplus[i].ranking + " Score " + aplus[i].score + " Win " + aplus[i].win + " lose " + aplus[i].lose + " Draw " + aplus[i].draw;
            }
            else
            {
                Text clonetext = Instantiate(text[0], parenttext[0].transform);
                clonetext.text = "Ranking " + aplus[i].ranking + " Score " + aplus[i].score + " Win " + aplus[i].win + " lose " + aplus[i].lose + " Draw " + aplus[i].draw;
            }
        }        
    }
    private void AzeroRankingExpression(int rankusernum)
    {
        Debug.Log("1");
        for (int i = 0; i < rankusernum; i++)
        {
            Debug.Log(i);
            azero[i].ranking = i + 1;
            azero[i].score = Random.Range(0, 1000);
            azero[i].win = Random.Range(0, 1000);
            azero[i].lose = Random.Range(0, 1000);
            azero[i].draw = Random.Range(0, 1000);
            if (i == 0)
            {
                text[1].text = "Ranking " + azero[i].ranking + " Score " + azero[i].score + " Win " + azero[i].win + " lose " + azero[i].lose + " Draw " + azero[i].draw;
            }
            else
            {
                Text clonetext = Instantiate(text[1], parenttext[1].transform);
                clonetext.text = "Ranking " + azero[i].ranking + " Score " + azero[i].score + " Win " + azero[i].win + " lose " + azero[i].lose + " Draw " + azero[i].draw;
            }
        }
    }
    private void BplusRankingExpression(int rankusernum)
    {
        for (int i = 0; i < rankusernum; i++)
        {
            Debug.Log(i);
            bplus[i].ranking = i + 1;
            bplus[i].score = Random.Range(0, 1000);
            bplus[i].win = Random.Range(0, 1000);
            bplus[i].lose = Random.Range(0, 1000);
            bplus[i].draw = Random.Range(0, 1000);
            if (i == 0)
            {
                text[2].text = "Ranking " + bplus[i].ranking + " Score " + bplus[i].score + " Win " + bplus[i].win + " lose " + bplus[i].lose + " Draw " + bplus[i].draw;
            }
            else
            {
                Text clonetext = Instantiate(text[2], parenttext[2].transform);
                clonetext.text = "Ranking " + bplus[i].ranking + " Score " + bplus[i].score + " Win " + bplus[i].win + " lose " + bplus[i].lose + " Draw " + bplus[i].draw;
            }
        }
    }
    private void BzeroRankingExpression(int rankusernum)
    {
        for (int i = 0; i < rankusernum; i++)
        {
            Debug.Log(i);
            bzero[i].ranking = i + 1;
            bzero[i].score = Random.Range(0, 1000);
            bzero[i].win = Random.Range(0, 1000);
            bzero[i].lose = Random.Range(0, 1000);
            bzero[i].draw = Random.Range(0, 1000);
            if (i == 0)
            {
                text[3].text = "Ranking " + bzero[i].ranking + " Score " + bzero[i].score + " Win " + bzero[i].win + " lose " + bzero[i].lose + " Draw " + bzero[i].draw;
            }
            else
            {
                Text clonetext = Instantiate(text[3], parenttext[3].transform);
                clonetext.text = "Ranking " + bzero[i].ranking + " Score " + bzero[i].score + " Win " + bzero[i].win + " lose " + bzero[i].lose + " Draw " + bzero[i].draw;
            }
        }
    }
    private void CplusRankingExpression(int rankusernum)
    {
        for (int i = 0; i < rankusernum; i++)
        {
            Debug.Log(i);
            cplus[i].ranking = i + 1;
            cplus[i].score = Random.Range(0, 1000);
            cplus[i].win = Random.Range(0, 1000);
            cplus[i].lose = Random.Range(0, 1000);
            cplus[i].draw = Random.Range(0, 1000);
            if (i == 0)
            {
                text[4].text = "Ranking " + cplus[i].ranking + " Score " + cplus[i].score + " Win " + cplus[i].win + " lose " + cplus[i].lose + " Draw " + cplus[i].draw;
            }
            else
            {
                Text clonetext = Instantiate(text[4], parenttext[4].transform);
                clonetext.text = "Ranking " + cplus[i].ranking + " Score " + cplus[i].score + " Win " + cplus[i].win + " lose " + cplus[i].lose + " Draw " + cplus[i].draw;
            }
        }
    }
    private void CzeroRankingExpression(int rankusernum)
    {
        for (int i = 0; i < rankusernum; i++)
        {
            Debug.Log(i);
            czero[i].ranking = i + 1;
            czero[i].score = Random.Range(0, 1000);
            czero[i].win = Random.Range(0, 1000);
            czero[i].lose = Random.Range(0, 1000);
            czero[i].draw = Random.Range(0, 1000);
            if (i == 0)
            {
                text[5].text = "Ranking " + czero[i].ranking + " Score " + czero[i].score + " Win " + czero[i].win + " lose " + czero[i].lose + " Draw " + czero[i].draw;
            }
            else
            {
                Text clonetext = Instantiate(text[5], parenttext[5].transform);
                clonetext.text = "Ranking " + czero[i].ranking + " Score " + czero[i].score + " Win " + czero[i].win + " lose " + czero[i].lose + " Draw " + czero[i].draw;
            }
        }
    }
}
