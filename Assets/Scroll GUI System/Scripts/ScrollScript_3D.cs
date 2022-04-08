/*
Thank you for purchasing this asset! If you have any questions, or need any help, 
please don't hesitate to contact me:  mr.patrick.ball@gmail.com
Happy Creating!

the challenge of this script was to make the 3D scroll object and its UI canvas sibling work in tandem to create the illusion of a scrolling scroll.

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScrollScript_3D : MonoBehaviour
{
    // public variables set in the inspector
    public Canvas MenuCanvas;
    public GameObject backdrop;

    public float[] BackdropPositions;
    public int menuPositionInt = 1;
    public float MenuSlideSpeed = 0.1f;
    public float MenuFadeInSpeed = 0.2f;

    public bool playSound = true;

    //other variables:
    // 'anim' holds the Animator Controller for this object,
    // 'scrollOpen' is a simple bool to determine the state of this object(open or closed) used int the "animate" function below.

    Animator anim;
    AudioSource Sound;
    RectTransform RecTran;
    Vector3 SliderPosition;
    bool scrollOpen = false;
    float menPosY;
    float menPosZ;


    
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        Sound = gameObject.GetComponent<AudioSource>();
        RecTran = backdrop.GetComponent<RectTransform>();
        menPosY = RecTran.localPosition.y;
        menPosZ = RecTran.localPosition.z;
        MenuCanvas.worldCamera = GameObject.FindGameObjectWithTag("HUDcam").GetComponent<Camera>();
    }


    /* <-----DELETE THESE LINES (1 of 2) if you want to bind the menu activation to a keypress event (TAB by default)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            OpenOrCloseScroll();
        }
    }
    DELETE THESE LINES (2 of 2)------------->*/ 


    // I use FixedUpdate for anything having to do with animation or physics.
    //Here, I use it to update the scrolling menu backdrop position to the current desired position.
    void FixedUpdate()
    {

        SliderPosition = new Vector3(BackdropPositions[menuPositionInt], menPosY, menPosZ);
        RecTran.localPosition = Vector3.MoveTowards(RecTran.localPosition, SliderPosition, MenuSlideSpeed);
       
    }

    // The following functions handle opening and closing the scroll. 
    // These are all 'public' so they can be accessed easily by things like OnClick events (UI buttons), animation events, etc.
    //------------------------------------------------------
    public void OpenOrCloseScroll()
    {
        if (!scrollOpen)
        {
            OpenScroll();  
        }
        else if (scrollOpen)
        {
            CloseScroll();
        }
    }
    public void OpenScroll()
    {
        if (!scrollOpen) {
            anim.SetTrigger("Opentrig");
            scrollOpen = true;
            // the "EnableMenu" function you might expect to be here is called by an animation event, 
            // to keep it from happening before the animation finishes.
        }
    }

   public void CloseScroll()
    {
        if (scrollOpen) {
            anim.SetTrigger("Closetrig");
            scrollOpen = false;
            DisableMenu();
        }
    }
//------------------------------------------------------------------------

    // This function is CALLED BY AN EVENT AT THE END OF THE 'OPEN' ANIMATION, which is triggered by the OpenScroll() function above...
    // ...which is called by the OpenOrCloseScroll() function above that.
    void EnableMenu()
    {
        MenuCanvas.gameObject.SetActive(true);

        //for fading in - because fading in is fancy.
        CanvasRenderer canRen = backdrop.GetComponent<CanvasRenderer>();
        canRen.SetAlpha(0.0f);
        backdrop.GetComponent<Image>().CrossFadeAlpha(1.0f, MenuFadeInSpeed, false);
    }
  
    // Called by the CloseScroll() function above
    void DisableMenu()
    {

        MenuCanvas.gameObject.SetActive(false);
    }

    //----------------------------------------------
    //The following functions trigger the scroll rolling left and right animations and 
    // updates the 'menuPositionInt' for the FixedUpdate() function to slide the menu to the correct position
     
    public void MoveMenuleft()
    {
        if (menuPositionInt < (BackdropPositions.Length - 1 ))
        {

            menuPositionInt = (menuPositionInt + 1);
            anim.SetTrigger("RollLeftTrig");
            PlaySound();
        }

    }

    public void MoveMenuRight()
    {
        if (menuPositionInt > 0)
        {
            menuPositionInt = (menuPositionInt - 1);
            anim.SetTrigger("RollRightTrig");
            PlaySound();


        }
    }
//-----------------------------------------------------

      //If this function is called, a little paper sliding sound clip will play. Settable in the Inspector.
    void PlaySound()
    {
        if (playSound)
        {
            Sound.Play();
        }
    }

}
