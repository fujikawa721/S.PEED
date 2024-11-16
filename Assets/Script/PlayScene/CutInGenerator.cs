using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CutInGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cutInObject;

    //�J�b�g�C���̃L�����N�^�[�摜��ǂݍ���
    [SerializeField] public Sprite cutIn001;
    [SerializeField] public Sprite cutIn002;
    [SerializeField] private Image cutInImg;


    //�J�b�g�C���̃A�j���[�V�����p�̃I�u�W�F�N�g�ǂݍ���
    [SerializeField] private GameObject textTopObject;
    [SerializeField] private GameObject textSPNameObject;
    [SerializeField] private GameObject textSPInfoObject;
    [SerializeField] TextMeshProUGUI textTop;
    [SerializeField] TextMeshProUGUI textBottom;
    [SerializeField] TextMeshProUGUI textSpInfo;


    private const float CUTIN_SPEED = 0.5f;//�J�b�g�C�����\������鑬�x
    private const float CUTIN_TIME = 2.5f;
    private string specialName;

    private Tween tween1;

    

    public void ReadyGame()
    {
        cutInObject.SetActive(false);
    }

    /// <summary>
    /// �Q�[���J�n����Player.cs�ɌĂяo�����B
    /// </summary>
    public void CheckCutInImg(int specialId)
    {
        cutInImg.DOFillAmount(0f, 0);
        switch (specialId)
        {
            case 1:
                cutInImg.sprite = cutIn001;
                specialName = "�ΑM�a";
                break;
            case 2:
                cutInImg.sprite = cutIn002;
                specialName = "���₵�̉�";
                break;
            default:
                Debug.Log(@$"�X�y�V����ID�ɃG���[������܂�");
                break;
        }
        textBottom.text = specialName;
    }

    /// <summary>
    /// �J�b�g�C���̃A�j���[�V�����BSP���g���x�Ăяo�����B
    /// </summary>
    /// <returns></returns>
    public IEnumerator AnimateSpecialCutIn()
    {
        cutInObject.SetActive(true);
        StartCoroutine(AnimateCutInImage());
        StartCoroutine(AnimateTextTop());
        StartCoroutine(AnimateSPName());
        StartCoroutine(AnimateSPInfo());
        yield return new WaitForSeconds(CUTIN_TIME);
        cutInObject.SetActive(false);
    }

    public IEnumerator AnimateCutInImage()
    {
        cutInImg.DOFillAmount(1.0f, CUTIN_SPEED).OnComplete(() =>
        {
            cutInImg.DOFillAmount(0f, CUTIN_SPEED).SetDelay(CUTIN_TIME - CUTIN_SPEED * 2);
        });
        yield return null;
    }

    public IEnumerator AnimateTextTop()
    {
        textTopObject.transform.DOLocalMoveX(-650, CUTIN_SPEED).From().OnComplete(() =>
        {
            //textTopObject.transform.DOLocalMoveX(650, CUTIN_SPEED).From().SetRelative(true).SetDelay(2);
        });
        yield return null;
    }
    public IEnumerator AnimateSPName()
    {
        textSPNameObject.transform.DOLocalMoveX(-1000, CUTIN_SPEED).From().OnComplete(() =>
        {
            //textSPNameObject.transform.DOLocalMoveX(1000, CUTIN_SPEED).From().SetDelay(2);
        });
        yield return null;
    }

    public IEnumerator AnimateSPInfo()
    {
        textSPInfoObject.transform.DOLocalMoveX(-1000, CUTIN_SPEED).From().OnComplete(() =>
        {
            //textSPInfoObject.transform.DOLocalMoveX(1000, CUTIN_SPEED).From().SetDelay(2);
        });
        yield return null;
    }

}
