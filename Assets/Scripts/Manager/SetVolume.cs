using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider soundslider;
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
    }
}
