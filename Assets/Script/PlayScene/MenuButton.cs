using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButton : MonoBehaviour, IPointerClickHandler
{
    public Image buttonImage;
    [SerializeField] private SoundManager soundManager;

    public delegate void ClickButton();//クリックしたときの処理呼び出し元が宣言する。
    private ClickButton clickButtonCallBack;

    public void SetAction(ClickButton clickButton)
    {
        clickButtonCallBack = clickButton;
        buttonImage = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        soundManager.PlaySelect();
        buttonImage.DOColor(Color.black, 0);
        clickButtonCallBack();
        
    }

    public void OnPointerEnter()
    {
        buttonImage.DOColor(Color.yellow,0);
        soundManager.PlayCursor();
    }

    public void OnPointerExit()
    {
        //transform.localScale = Vector3.one * 1.0f;
        buttonImage.DOColor(Color.black, 0);
    }
}
