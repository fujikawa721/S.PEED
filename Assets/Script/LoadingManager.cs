using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingManager : MonoBehaviour
{
    public GameObject loadingDisplay;
    public Image blackPanel;
    private const float FADE_SPEED = 1.0f;//�y3�b�z�����ĉ�ʂ��Â�/���邭����B

    //���[�h����O�Ɏ��s������B���񂾂�Â�����
    public IEnumerator StartLoad()
    {
        Debug.Log("���[�h�J�n");
        loadingDisplay.SetActive(true);
        blackPanel.DOFade(0, 0);
        blackPanel.DOFade(1, FADE_SPEED);
        yield return null;
    }

    //���[�h���I�������Ɏ��s������B���񂾂񖾂邭����
    public IEnumerator EndLoad()
    {
        loadingDisplay.SetActive(true);
        blackPanel.DOFade(1, 0);
        blackPanel.DOFade(0, FADE_SPEED).OnComplete(() =>
        {
            loadingDisplay.SetActive(false);
        });

        yield return null;

    }
}
