using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CutInGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cutInObject;


    [SerializeField] public Sprite cutIn001;
    [SerializeField] public Sprite cutIn002;
    [SerializeField] private Image catInImg;

    [SerializeField] TextMeshProUGUI textTop;
    [SerializeField] TextMeshProUGUI textBottom;


    private const float CUTIN_SPEED = 0.5f;//�J�b�g�C�����\������鑬�x
    private string specialName;

    AudioSource audioSource;
    public AudioClip spDo;

    public void ReadyGame()
    {
        audioSource = GetComponent<AudioSource>();
        cutInObject.SetActive(false);
    }

    public IEnumerator AnimateSpecialCutIn()
    {
        playSeSpDo();
        cutInObject.SetActive(true);
        textBottom.text = specialName;
        catInImg.DOFillAmount(1.0f, CUTIN_SPEED).OnComplete(() =>
        {
            catInImg.DOFillAmount(0f, CUTIN_SPEED).SetDelay(5);
        });
        yield return new WaitForSeconds(6.0f);
        cutInObject.SetActive(false);
    }


    public void CheckCutInImg(int specialId)
    {
        catInImg = GetComponent<Image>();
        catInImg.DOFillAmount(0f, 0);
        switch (specialId)
        {
            case 1:
                catInImg.sprite = cutIn001;
                specialName = "�ΑM�a";
                break;
            case 2:
                catInImg.sprite = cutIn002;
                specialName = "���₵�̉�";
                break;
            default:
                Debug.Log(@$"�X�y�V����ID�ɃG���[������܂�");
                break;
        }
    }

    //�����ʉ��Đ�
    public void playSeSpDo()
    {
        audioSource.clip = spDo;
        audioSource.Play();
    }
}
