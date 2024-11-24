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

    [SerializeField] private GameObject endGameButton;
    [SerializeField] private MenuButton endGameButtonScript;

    private const float SLIDE_SPEED = 0.5f;//É{É^ÉìÇÕÅy0.5ÅzïbÇ©ÇØÇƒÉXÉâÉCÉhÇ∑ÇÈÅB
    private const float LOAD_TIME = 1.0f;//âÊñ ëJà⁄Ç∑ÇÈÇ‹Ç≈Ç…ç≈í·Ç≈Ç‡Åy1ïbä‘Åzë“Ç¬

    void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        StartCoroutine(Ready());
        soundManager.PlayBgmResult();
    }

    public IEnumerator Ready()
    {
        Debug.Log("ÉåÉfÉBäJén");
        vsCpuButton.SetActive(false);
        vsCpuButtonScript.SetAction(ClickButtonVSCpu);
        endGameButton.SetActive(false);
        endGameButtonScript.SetAction(ClickButtonEndGame);
        yield return new WaitForSeconds(LOAD_TIME);
        yield return StartCoroutine(loadingManager.EndLoad());
        StartCoroutine(AnimateRightSlideObject(vsCpuButton));
        StartCoroutine(AnimateRightSlideObject(endGameButton));
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


    public void ClickButtonEndGame()
    {
        StartCoroutine(EndGame());
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(LOAD_TIME);
        Application.Quit();
    }



    public IEnumerator AnimateRightSlideObject(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.DOLocalMoveX(500, SLIDE_SPEED).From();
        yield return null;
    }
}
