using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UITest : MonoBehaviour
{
    public GameObject settint_panel;
    public GameObject Ranking_panel;
    public GameObject card_library_panel;
    public GameObject card_library_panel_p1;
    public GameObject card_library_panel_p2;
    public GameObject[] Card = new GameObject[10];
    public GameObject[] Card2 = new GameObject[10];
    public GameObject[] Card3 = new GameObject[10];
    public GameObject rightbutton;
    public GameObject leftbutton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void start_button()
    {
        SceneManager.LoadScene("GameTest");
    }
    public void setting_button()
    {
        if (settint_panel.activeSelf == true)
        {
            settint_panel.SetActive(false);
        }
        else if (settint_panel.activeSelf == false)
        {
            settint_panel.SetActive(true);
        }
    }
    public void Card_Library_button()
    {
        if (card_library_panel.activeSelf == true)
        {
            card_library_panel.SetActive(false);
        }
        else if (card_library_panel.activeSelf == false&& card_library_panel_p1.activeSelf ==false&& card_library_panel_p2.activeSelf == false)
        {
            card_library_panel.SetActive(true);
        }
        else if(card_library_panel_p1.activeSelf == true)
        {
            card_library_panel_p1.SetActive(false);
        }
        else if (card_library_panel_p2.activeSelf == true)
        {
            card_library_panel_p2.SetActive(false);
        }
    }
    public void card_library_panel_right_move_button()
    {
            card_library_panel.SetActive(false);
            card_library_panel_p1.SetActive(true);
    }
    public void card_library_panel_p1_right_move_button()
    {
        card_library_panel_p1.SetActive(false);
        card_library_panel_p2.SetActive(true);
    }
    public void card_library_panel_p1_left_move_button()
    {
        card_library_panel_p1.SetActive(false);
        card_library_panel.SetActive(true);
    }
    public void card_library_panel_p2_left_move_button()
    {
        card_library_panel_p2.SetActive(false);
        card_library_panel_p1.SetActive(true);
    }
    public void game_over()
    {
        Application.Quit();
    }
    public void rangking_panel()
    {if (Ranking_panel.activeSelf == true)
        {
            Ranking_panel.SetActive(false);
        }
        else if  (Ranking_panel.activeSelf == false)
        {
            Ranking_panel.SetActive(true);
        }
    }
    
}
