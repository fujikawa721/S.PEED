using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SelectController selectController;
    [SerializeField] private CharacterData character_data;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(selectController.back_selection_status());
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
