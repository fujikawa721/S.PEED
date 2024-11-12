using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReadyButton : MonoBehaviour, IPointerClickHandler
{ 
    [SerializeField] private SelectController selectController;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(selectController.goto_gameplay());
    }

    public void OnPointerEnter()
    {
        transform.position += Vector3.up * 0.1f;
        transform.localScale = Vector3.one * 1.1f;
    }

    public void OnPointerExit()
    {
        transform.position -= Vector3.up * 0.1f;
        transform.localScale = Vector3.one * 1.0f;
    }
}
