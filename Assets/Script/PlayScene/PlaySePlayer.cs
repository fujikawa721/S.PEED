using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySePlayer : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip spPointGet;
    public AudioClip spgaugeMax;
    public AudioClip doSp;
    public AudioClip sword;
    public AudioClip recoverHp;

    public IEnumerator ReadyAudio()
    {
        audioSource = GetComponent<AudioSource>();
        yield return null;
    }

    public void PlaySeSpPointGet()
    {
        audioSource.clip = spPointGet;
        audioSource.Play();
    }

    public void PlaySeSpgaugeMax()
    {
        audioSource.clip = spgaugeMax;
        audioSource.Play();
    }

    public void PlaySeDoSp()
    {
        audioSource.clip = doSp;
        audioSource.Play();
    }
}
