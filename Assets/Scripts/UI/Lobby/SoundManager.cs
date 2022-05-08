using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgm;
    public AudioMixer mixer;
    public Slider soundslider;
    public GameObject onbutton;
    public GameObject offbutton;
    private float currndsound;
    private void Awake()
    {
        bgm.Play();
        DontDestroyOnLoad(transform.gameObject);

    }
    // Start is called before the first frame update
    void Start()
    { 
        soundslider.value = 0.75f; 
        soundslider.value = PlayerPrefs.GetFloat("Music", 0.75f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetSoundSize(float silderValue)
    {
        mixer.SetFloat("Music", Mathf.Log10(silderValue)*20);
        PlayerPrefs.SetFloat("Music", silderValue);
        currndsound = silderValue;
    }

    public void BgmOff()
    {
        
        mixer.SetFloat("Music", -80f);
        offbutton.SetActive(false);
        onbutton.SetActive(true);
    }
    public void BgmOn()
    {

        mixer.SetFloat("Music", currndsound);
        offbutton.SetActive(true);
        onbutton.SetActive(false);
    }
}
