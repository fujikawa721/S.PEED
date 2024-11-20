using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySeHand : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip draw;


    public IEnumerator ReadyAudio()
    {
        audioSource = GetComponent<AudioSource>();
        yield return null;
    }

    public void PlaySeDraw()
    {
        audioSource.clip = draw;
        audioSource.Play();
    }
}
