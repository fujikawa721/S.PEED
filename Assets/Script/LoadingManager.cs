using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingManager : MonoBehaviour
{
    public GameObject loadingDisplay;
    public Image blackPanel;
    private const float FADE_SPEED = 1.0f;//【3秒】かけて画面を暗く/明るくする。

    //ロードする前に実行させる。だんだん暗くする
    public IEnumerator StartLoad()
    {
        Debug.Log("ロード開始");
        loadingDisplay.SetActive(true);
        blackPanel.DOFade(0, 0);
        blackPanel.DOFade(1, FADE_SPEED);
        yield return null;
    }

    //ロードが終わった後に実行させる。だんだん明るくする
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
