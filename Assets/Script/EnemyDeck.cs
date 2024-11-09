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

    //相手側の山札をクリックしてもスペシャルが発動しないように上書き
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(@$"敵側の山札がクリックされた");

    }
}
