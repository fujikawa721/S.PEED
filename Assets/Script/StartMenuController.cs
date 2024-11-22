using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private LoadingManager loadingManager;
    [SerializeField] private Image background;
    [SerializeField] private GameObject titleLogo;

    [SerializeField] private GameObject vsCpuButton;
    [SerializeField] private MenuButton vsCpuButtonScript;

    private const float LOADING_TIME = 1.0f;//�Œ�y1�b�ԁz���[�h��ʂ�����
    private const float SLIDE_SPEED = 0.5f;//�{�^���́y0.5�z�b�����ăX���C�h����B

    void Start()
    {
        StartCoroutine(Ready());
    }

    public IEnumerator Ready()
    {
        Debug.Log("���f�B�J�n");
        vsCpuButton.SetActive(false);
        vsCpuButtonScript.SetAction(ClickButtonVSCpu);
        yield return new WaitForSeconds(LOADING_TIME);
        yield return StartCoroutine(loadingManager.EndLoad());
        StartCoroutine(AnimateRightSlideObject(vsCpuButton));
    }

    public void ClickButtonVSCpu()
    {
        StartCoroutine(MoveSelectionCharacter());
    }

    public IEnumerator MoveSelectionCharacter()
    {
        yield return StartCoroutine(loadingManager.StartLoad());
        SceneManager.LoadScene("CharacterSelection");
    }


    public IEnumerator AnimateRightSlideObject(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.DOLocalMoveX(500, SLIDE_SPEED).From();
        yield return null;
    }
}
