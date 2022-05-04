using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgm;
    public GameObject onbutton;
    public GameObject offbutton;
  
    // Start is called before the first frame update

    private void Awake()
    {
        bgm.Play();
        DontDestroyOnLoad(transform.gameObject);
       
    }


  

    // Update is called once per frame

  
    public void bgmon()
    {
        bgm.Play();
        offbutton.SetActive(true);
        onbutton.SetActive(false);
    }
    public void bgmoff()
    {
        bgm.Pause();
        offbutton.SetActive(false);
        onbutton.SetActive(true);

    }
}
