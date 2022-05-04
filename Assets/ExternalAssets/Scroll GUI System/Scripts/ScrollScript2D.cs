/*
Thank you for purchasing this asset! If you have any questions, or need any help, 
please don't hesitate to contact me:  mr.patrick.ball@gmail.com
Happy Creating!
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript2D : MonoBehaviour
{
    // Public Variables set in Inspector
    public GameObject MenuContent;
    public bool playSound = false;
   
    //other variables:
    private Animator _animator;
    private AudioSource _audioSource;
    private Vector2 _anchorPos;
    private bool isOpen = false;
    private bool isDown = false;


    // Here, we use the Start function to assign the anim variable to the Animator, and the Sound variable to the AudioSource 
    // then, we set ths object's Canvas's (parent) "Render Camera" component to the HUDcam. 
    // the Render Camera can be set manualy in the inspector... 
    // but if you forget to set it when you drag in a new prefab, things stop working - which is the purpose for this automation.
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        gameObject.GetComponentInParent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("HUDCamera").GetComponent<Camera>();
    }

    public void ScrollDown()
    {
        if (!isDown)
        {
            _animator.SetTrigger("DownTrig");
            isDown = true;
        }
    }

    public void ScrollUp()
    {
        if (isDown)
        {
            _animator.SetTrigger("UpTrig");
            isDown = false;
        }
    }

    public void ScrollOpen()
    {
        if (!isOpen)
        {
            _animator.SetTrigger("OpenTrig");
            isOpen = true;
        }
    }

    public void ScrollClose()
    {
        if (isOpen)
        {
            _animator.SetTrigger("CloseTrig");
            isOpen = false;
        }
    }

    private void Update()
    {
        if (isOpen)
        {
            GetComponent<RectTransform>().anchoredPosition = _anchorPos;
        }
    }

    public void SaveCurrentPos()
    {
        _anchorPos = GetComponent<RectTransform>().anchoredPosition;
    }

    // the 'animate' function is responsible for triggering events (open and close) in the Animator,
    // and enabling/disabling the MenuContent. "MenuContent" is a Text object by default, but could be set to any UI object. 
    public void animate()
    {
        if (!isOpen)
        {
            GetComponentInParent<CanvasGroup>().alpha = 1;
            _animator.SetTrigger("OpenTrig");
            PlaySound();
            isOpen = true;
        }
        else if (isOpen)
        {
            GetComponentInParent<CanvasGroup>().alpha = 0;
            _animator.SetTrigger("CloseTrig");
            PlaySound();
            isOpen = false;
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
            _audioSource.Play();
        }
    }
}
