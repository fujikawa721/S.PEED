using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class NormalCutInGenerator : MonoBehaviour
{
    [SerializeField] private GameObject normalCutInObject;

    [SerializeField] private Image normalCutInImg;

    [SerializeField] TextMeshProUGUI eventText;
    [SerializeField] TextMeshProUGUI effectText;

    private const float CUTIN_SPEED = 0.5f;
    private const float CUTIN_TIME = 2.5f;

    public void ReadyGame(Sprite normalCutInImage)
    {
        normalCutInObject.SetActive(false);
        normalCutInImg.DOFillAmount(0f, 0);
        normalCutInImg.sprite = normalCutInImage;
    }

    public IEnumerator AnimateNormalCutIn(string eventName,string eventEffect)
    {
        eventText.text = @$"{eventName}";
        effectText.text = @$"{eventEffect}";
        normalCutInObject.SetActive(true);
        StartCoroutine(AnimateTextTop());
        yield return new WaitForSeconds(CUTIN_TIME);
        normalCutInObject.SetActive(false);
    }

    public IEnumerator AnimateTextTop()
    {
        normalCutInObject.transform.DOLocalMoveX(-1000, CUTIN_SPEED).From().OnComplete(() =>
        {
            //textTopObject.transform.DOLocalMoveX(650, CUTIN_SPEED).From().SetRelative(true).SetDelay(2);
        });
        yield return null;
    }

}
