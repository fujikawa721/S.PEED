using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameGuidance :MonoBehaviour
{

    AudioSource audioSource;
    public AudioClip voice_break;
    public AudioClip voice_spee;
    public AudioClip voice_do;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void play_se_voice_spee()
    {
        audioSource.clip = voice_spee;
        audioSource.Play();
    }

    public void play_se_voice_do()
    {
        audioSource.clip = voice_do;
        audioSource.Play();
    }

    public void play_se_voice_break()
    {
        audioSource.clip = voice_break;
        audioSource.Play();
    }

}
