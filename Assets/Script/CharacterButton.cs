using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SelectController selectController;
    [SerializeField] private CharacterData character_data;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(@$"selection_statusÇ™ê≥ÇµÇ≠Ç†ÇËÇ‹ÇπÇÒÅB");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(selectController.select_character(character_data));
    }

    public void OnPointerEnter()
    {
        transform.position += Vector3.up * 0.1f;
        transform.localScale = Vector3.one * 1.1f;
        StartCoroutine(selectController.pass_character_data(character_data));
    }

    public void OnPointerExit()
    {
        transform.position -= Vector3.up * 0.1f;
        transform.localScale = Vector3.one * 1.0f;
    }
}
