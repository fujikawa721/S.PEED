using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public delegate void ClickCard(int handNumber);
    private ClickCard clickCardCallBack;
    private int handNumber;


    //PlayerHandConrtollerから呼び出される。
    //何番目の手札か、カードをクリックした時実行するメソッド（コールバック）を受け取る。
    public void CardParameter(int playerHandNumber, ClickCard clickCard)
    {
        handNumber = playerHandNumber;
        clickCardCallBack = clickCard;
    }


    public void OnPointerClick(PointerEventData eventData) {
        clickCardCallBack(handNumber);
    }

    public void OnPointerEnter()
    {
        transform.position += Vector3.up * 0.3f;
        transform.localScale = Vector3.one * 3.3f;
    }

    public void OnPointerExit()
    {
        transform.position -= Vector3.up * 0.3f;
        transform.localScale = Vector3.one * 3.0f;
    }



}
