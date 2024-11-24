using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SelectController selectController;
    [SerializeField] private CharacterData characterData;


    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(selectController.SelectCharacter(characterData));
    }

    public void OnPointerEnter()
    {
        transform.position += Vector3.up * 0.1f;
        transform.localScale = Vector3.one * 1.1f;
        StartCoroutine(selectController.PassCharacterData(characterData));
    }

    public void OnPointerExit()
    {
        transform.position -= Vector3.up * 0.1f;
        transform.localScale = Vector3.one * 1.0f;
    }
}
