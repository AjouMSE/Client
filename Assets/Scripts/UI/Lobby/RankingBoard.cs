using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingBoard : MonoBehaviour
{
    public GameObject rankingboard;
    public GameObject[] rangkingpanel;
    
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
    }
    public void AplusSelect()
    {
        rangkingpanel[0].SetActive(true);
        for(int i=1;i<6;i++)
        {
            rangkingpanel[i].SetActive(false);
        }
    }
    public void AzeroSelect()
    {
        rangkingpanel[1].SetActive(true);
        for (int i = 0; i < 6; i++)
        {
            if(i==1)
            {
                continue;
            }
            rangkingpanel[i].SetActive(false);
        }
    }
    public void BplusSelect()
    {
        rangkingpanel[2].SetActive(true);
        for (int i = 0; i < 6; i++)
        {
            if (i == 2)
            {
                continue;
            }
            rangkingpanel[i].SetActive(false);
        }
    }
    public void BzeroSelect()
    {
        rangkingpanel[3].SetActive(true);
        for (int i = 0; i < 6; i++)
        {
            if (i == 3)
            {
                continue;
            }
            rangkingpanel[i].SetActive(false);
        }
    }
    public void CplusSelect()
    {
        rangkingpanel[4].SetActive(true);
        for (int i = 0; i < 6; i++)
        {
            if (i == 4)
            {
                continue;
            }
            rangkingpanel[i].SetActive(false);
        }
    }
    public void CzeroSelect()
    {
        rangkingpanel[5].SetActive(true);
        for (int i = 0; i < 5; i++)
        {
            
            rangkingpanel[i].SetActive(false);
        }
    }
}