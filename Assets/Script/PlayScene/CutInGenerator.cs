using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CutInGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cutInObject;

    //カットインのキャラクター画像を読み込み
    [SerializeField] public Sprite cutIn001;
    [SerializeField] public Sprite cutIn002;
    [SerializeField] private Image cutInImg;


    //カットインのアニメーション用のオブジェクト読み込み
    [SerializeField] private GameObject textTopObject;
    [SerializeField] private GameObject textSPNameObject;
    [SerializeField] private GameObject textSPInfoObject;
    [SerializeField] TextMeshProUGUI textTop;
    [SerializeField] TextMeshProUGUI textBottom;
    [SerializeField] TextMeshProUGUI textSpInfo;


    private const float CUTIN_SPEED = 0.5f;//カットインが表示される速度
    private const float CUTIN_TIME = 2.5f;
    private string specialName;

    private Tween tween1;

    

    public void ReadyGame()
    {
        cutInObject.SetActive(false);
    }

    /// <summary>
    /// ゲーム開始時にPlayer.csに呼び出される。
    /// </summary>
    public void CheckCutInImg(int specialId)
    {
        cutInImg.DOFillAmount(0f, 0);
        switch (specialId)
        {
            case 1:
                cutInImg.sprite = cutIn001;
                specialName = "火閃斬";
                break;
            case 2:
                cutInImg.sprite = cutIn002;
                specialName = "いやしの歌";
                break;
            default:
                Debug.Log(@$"スペシャルIDにエラーがあります");
                break;
        }
        textBottom.text = specialName;
    }

    /// <summary>
    /// カットインのアニメーション。SPを使う度呼び出される。
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
