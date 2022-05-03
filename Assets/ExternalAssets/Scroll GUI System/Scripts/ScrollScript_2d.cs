/*
Thank you for purchasing this asset! If you have any questions, or need any help, 
please don't hesitate to contact me:  mr.patrick.ball@gmail.com
Happy Creating!
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript_2d : MonoBehaviour
{
    // Public Variables set in Inspector
    public GameObject MenuContent;
    public bool playSound = false;
   
    //other variables:
    Animator anim;
    AudioSource Sound;
    bool open = false;
    

    // Here, we use the Start function to assign the anim variable to the Animator, and the Sound variable to the AudioSource 
    // then, we set ths object's Canvas's (parent) "Render Camera" component to the HUDcam. 
    // the Render Camera can be set manualy in the inspector... 
    // but if you forget to set it when you drag in a new prefab, things stop working - which is the purpose for this automation.
    void Start()
    {
        anim = GetComponent<Animator>();
        Sound = gameObject.GetComponent<AudioSource>();
        gameObject.GetComponentInParent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("HUDcam").GetComponent<Camera>();
    }

    // the 'animate' function is responsible for triggering events (open and close) in the Animator,
    // and enabling/disabling the MenuContent. "MenuContent" is a Text object by default, but could be set to any UI object. 
    public void animate()
    {
        if (!open)
        {
            anim.SetTrigger("OpenTrig");
            PlaySound();
            open = true;
        }
        else if (open)
        {
            anim.SetTrigger("CloseTrig");
            PlaySound();
            open = false;
            MakeTextGoAway();
        }

        //the following are called in the above "animate" function. simple enough.
    }
    void MakeTextAppear()
    {
        MenuContent.gameObject.SetActive(true);
    }
    void MakeTextGoAway()
    {
        MenuContent.gameObject.SetActive(false);
    }

    void PlaySound()
    {
        if (playSound)
        {
            Sound.Play();
        }
    }
}
