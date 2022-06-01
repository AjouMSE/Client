using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingBoardManager : MonoBehaviour
{
    public GameObject rankingboard;
    public GameObject rankingtitle;
    //public GameObject[] rangkingpanel;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RankBoardOnInvoke()
    {
        Invoke("RankBoardOn", 1.3f);
    }
    public void RankBoardOn()
    {
        rankingboard.SetActive(true);
        rankingtitle.SetActive(true);
    }
    
}