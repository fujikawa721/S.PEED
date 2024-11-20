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
    [SerializeField] private Image cutInImg;


    //カットインのアニメーション用のオブジェクト読み込み
    [SerializeField] private GameObject textSPNameObject;
    [SerializeField] private GameObject textSPInfoObject;
    [SerializeField] TextMeshProUGUI spNameText;
    [SerializeField] TextMeshProUGUI spInfoText;


    private const float CUTIN_SPEED = 0.5f;//カットインが表示される速度
    private const float CUTIN_TIME = 2.5f;

    private Tween tween1;

    

    public void ReadyGame(CharacterData characterData)
    {
        cutInObject.SetActive(false);
        cutInImg.DOFillAmount(0f, 0);
        cutInImg.sprite = characterData.specialCutInImage;
        spNameText.text = @$"{characterData.spName}";
        spInfoText.text = @$"{characterData.spInfo}";
    }


    /// <summary>
    /// カットインのアニメーション。SPを使う度呼び出される。
    /// </summary>
    /// <returns></returns>
    public IEnumerator AnimateSpecialCutIn()
    {
        cutInObject.SetActive(true);
        StartCoroutine(AnimateCutInImage());
        StartCoroutine(AnimateSpName());
        StartCoroutine(AnimateSpInfo());
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
    public IEnumerator AnimateSpName()
    {
        textSPNameObject.transform.DOLocalMoveX(-1000, CUTIN_SPEED).From().OnComplete(() =>
        {
            //textSPNameObject.transform.DOLocalMoveX(1000, CUTIN_SPEED).From().SetDelay(2);
        });
        yield return null;
    }

    public IEnumerator AnimateSpInfo()
    {
        textSPInfoObject.transform.DOLocalMoveX(-1000, CUTIN_SPEED).From().OnComplete(() =>
        {
            //textSPInfoObject.transform.DOLocalMoveX(1000, CUTIN_SPEED).From().SetDelay(2);
        });
        yield return null;
    }

}
