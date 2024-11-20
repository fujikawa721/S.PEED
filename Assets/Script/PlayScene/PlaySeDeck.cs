using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySeDeck : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip deckMax;


    public IEnumerator ReadyAudio()
    {
        audioSource = GetComponent<AudioSource>();
        yield return null;
    }

    public void PlaySeDeckMax()
    {
        audioSource.clip = deckMax;
        audioSource.Play();
    }

}
