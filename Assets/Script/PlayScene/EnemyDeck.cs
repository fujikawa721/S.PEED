using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EnemyDeck : Deck, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //���葤�̎R�D���N���b�N���Ă��X�y�V�������������Ȃ��悤�ɏ㏑��
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(@$"�G���̎R�D���N���b�N���ꂽ");

    }
}
