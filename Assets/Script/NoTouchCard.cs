using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

public class NoTouchCard : Card, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public  void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(@$"このカードは触れません。");
    }
}
