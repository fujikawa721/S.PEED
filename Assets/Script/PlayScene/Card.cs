using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler
{

    public int hand_number;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CardParameter(int playerhand_number)
    {
        hand_number = playerhand_number;

    }


    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log(@$"{hand_number}–Ú‚ªƒNƒŠƒbƒN‚³‚ê‚½");
        GameObject PlayerHand = GameObject.Find("PlayerHand");
        PlayerHand.GetComponent<PlayerHandController>().put_card_field(hand_number);
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
