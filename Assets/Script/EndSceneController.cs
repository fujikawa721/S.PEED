using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

public class EndSceneController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private LoadingManager loadingManager;
    [SerializeField] private Image background;
    [SerializeField] TextMeshProUGUI EndMessage;

    private const float LOADING_TIME = 1.0f;//最低【1秒間】ロード画面が続く

    void Start()
    {
        StartCoroutine(Ready());
    }

    public IEnumerator Ready()
    {
        Debug.Log("レディ開始");
        yield return new WaitForSeconds(LOADING_TIME);
        StartCoroutine(loadingManager.EndLoad());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(loadingManager.StartLoad());
        SceneManager.LoadScene("StartMenu");
    }

}
