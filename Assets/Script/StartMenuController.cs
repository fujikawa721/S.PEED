using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private LoadingManager loadingManager;
    [SerializeField] private Image background;
    [SerializeField] private GameObject titleLogo;

    [SerializeField] private GameObject vsCpuButton;
    [SerializeField] private MenuButton vsCpuButtonScript;

    private const float SLIDE_SPEED = 0.5f;//ボタンは【0.5】秒かけてスライドする。
    private const float LOAD_TIME = 1.0f;//画面遷移するまでに最低でも【1秒間】待つ

    void Start()
    {
        StartCoroutine(Ready());
        soundManager.PlayBgmResult();
    }

    public IEnumerator Ready()
    {
        Debug.Log("レディ開始");
        vsCpuButton.SetActive(false);
        vsCpuButtonScript.SetAction(ClickButtonVSCpu);
        yield return new WaitForSeconds(LOAD_TIME);
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
        yield return new WaitForSeconds(LOAD_TIME);
        SceneManager.LoadScene("CharacterSelection");
    }


    public IEnumerator AnimateRightSlideObject(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.DOLocalMoveX(500, SLIDE_SPEED).From();
        yield return null;
    }
}
