using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgm;//BGM�Đ��p
    [SerializeField] private AudioSource systemSE;//����{�^�����V�X�e��SE
    [SerializeField] private AudioSource cardSE;//�h���[�Ȃǂ̃J�[�h�֘ASE
    [SerializeField] private AudioSource playerSE;//�X�e�[�^�X�A�b�v�Ȃǂ�SE
    [SerializeField] private AudioSource cutInSE;//�J�b�g�C����SE
    [SerializeField] private AudioSource voiceSE;//�L�����N�^�[�{�C�X��SE

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

    public AudioClip draw;

    public AudioClip spPointGet;
    public AudioClip spgaugeMax;
    public AudioClip combo;
    public AudioClip deckMax;

    public AudioClip doSp;
    public AudioClip sword;
    public AudioClip recoverHp;
    public AudioClip powerUp;
    public AudioClip bigHit;

    public AudioClip cvGameStart;
    public AudioClip cvElementSkill;
    public AudioClip cvPassivSkill;
    public AudioClip cvSP;

    //������������������BGM������������������
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

    //�������������������V�X�e��SE������������������
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


    //�������������������J�[�hSE������������������
    

    public void PlayDraw()
    {
        cardSE.clip = draw;
        cardSE.Play();
    }


    //�������������������v���C���[SE������������������
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

    public void PlayCombo()
    {
        playerSE.clip = combo;
        playerSE.Play();
    }

    //�������������������J�b�g�C��SE������������������
    public void PlaySword()
    {
        cutInSE.clip = sword;
        cutInSE.Play();
    }

    public void PlayRecover()
    {
        cutInSE.clip = recoverHp;
        cutInSE.Play();
    }

    public void PlayPowerUp()
    {
        cutInSE.clip = powerUp;
        cutInSE.Play();
    }

    public void PlayBigHit()
    {
        cutInSE.clip = bigHit;
        cutInSE.Play();
    }

    public void PlayEventCutIn()
    {
        cutInSE.clip = spPointGet;
        cutInSE.Play();
    }

    //�������������������{�C�XSE������������������
    public void SetCharacterVoice(CharacterData characterData)
    {
        cvGameStart = characterData.voiceGameStart;
        cvElementSkill= characterData.voiceElementSkill;
        cvPassivSkill = characterData.voicePassivSkill;
        cvSP = characterData.voiceSP;
    }

    public void PlayCVGameStart()
    {
        voiceSE.clip = cvGameStart;
        voiceSE.Play();
    }

    public void PlayCVElementSkill()
    {
        voiceSE.clip = cvElementSkill;
        voiceSE.Play();
    }

    public void PlayCVPassivSkill()
    {
        voiceSE.clip = cvPassivSkill;
        voiceSE.Play();
    }

    public void PlayCVSP()
    {
        voiceSE.clip = cvSP;
        voiceSE.Play();
    }

    public void PlayVoiceSPEE()
    {
        voiceSE.clip = voiceSpee;
        voiceSE.Play();
    }

    public void PlayVoiceDO()
    {
        voiceSE.clip = voiceDo;
        voiceSE.Play();
    }
}
