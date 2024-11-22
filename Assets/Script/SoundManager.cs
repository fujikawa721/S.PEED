using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgm;//BGMÄ¶p
    [SerializeField] private AudioSource systemSE;//è{^VXeSE
    [SerializeField] private AudioSource cardSE;//h[ÈÇÌJ[hÖASE
    [SerializeField] private AudioSource playerSE;//Xe[^XAbvÈÇÌSE
    [SerializeField] private AudioSource cutInSE;//JbgCÌSE

    public AudioClip bgmCharacterSelection;
    public AudioClip bgmBattle;
    public AudioClip bgmResult;

    public AudioClip cursor;
    public AudioClip select;
    public AudioClip back;
    public AudioClip gameready;
    public AudioClip speed;
    public AudioClip whistle;
    public AudioClip voiceBreak;
    public AudioClip voiceSpee;
    public AudioClip voiceDo;

    public AudioClip deckMax;
    public AudioClip draw;


    public AudioClip spPointGet;
    public AudioClip spgaugeMax;
    public AudioClip doSp;
    public AudioClip sword;
    public AudioClip recoverHp;

    //BGM
    public void PlayBgmBattle()
    {
        bgm.clip = bgmBattle;
        bgm.Play();
    }

    public void PlayBgmCharacterSelection()
    {
        bgm.clip = bgmCharacterSelection;
        bgm.Play();
    }

    public void PlayBgmResult()
    {
        bgm.clip = bgmResult;
        bgm.Play();
    }

    //VXeSE
    public void PlayCursor()
    {
        systemSE.clip = cursor;
        systemSE.Play();
    }

    public void PlaySelect()
    {
        systemSE.clip = select;
        systemSE.Play();
    }

    public void PlayBack()
    {
        systemSE.clip = back;
        systemSE.Play();
    }

    public void PlayGameready()
    {
        systemSE.clip = gameready;
        systemSE.Play();
    }

    public void PlaySpeed()
    {
        systemSE.clip = speed;
        systemSE.Play();
    }

    public void PlayWhistle()
    {
        systemSE.clip = whistle;
        systemSE.Play();
    }

    public void PlayVoiceSPEE()
    {
        systemSE.clip = voiceSpee;
        systemSE.Play();
    }

    public void PlayVoiceDO()
    {
        systemSE.clip = voiceDo;
        systemSE.Play();
    }



    //J[hSE
    

    public void PlayDraw()
    {
        cardSE.clip = draw;
        cardSE.Play();
    }


    //vC[SE
    public void PlaySPPointGet()
    {
        playerSE.clip = spPointGet;
        playerSE.Play();
    }

    public void PlaySPGaugeMax()
    {
        playerSE.clip = spgaugeMax;
        playerSE.Play();
    }

    public void PlayDoSP()
    {
        playerSE.clip = doSp;
        playerSE.Play();
    }

    public void PlayDeckMax()
    {
        playerSE.clip = deckMax;
        playerSE.Play();
    }

}
