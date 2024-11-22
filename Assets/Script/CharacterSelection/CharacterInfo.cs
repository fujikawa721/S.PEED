using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CharacterInfo : MonoBehaviour
{
    

    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] TextMeshProUGUI characterTypeText;
    [SerializeField] TextMeshProUGUI spNameText;
    [SerializeField] TextMeshProUGUI spInfoText;

    //キャラクターの立ち絵読み込み
    [SerializeField] public GameObject characterObject;
    [SerializeField] private Image characterImage;
    [SerializeField] public Sprite null_img;

    //属性マークの読み込み
    [SerializeField] public GameObject markObject;
    [SerializeField] private Image markImage;
    [SerializeField] public Sprite markClover;
    [SerializeField] public Sprite markDiamond;
    [SerializeField] public Sprite markSpade;
    [SerializeField] public Sprite markHeart;


    public void receive_data(CharacterData characterData)
    {
        characterNameText.text = @$"{characterData.characterName}";
        characterTypeText.text = @$"{characterData.characterType}";
        spNameText.text = @$"{characterData.spName}";
        spInfoText.text = @$"{characterData.spInfo}";
        characterImage = characterObject.GetComponent<Image>();
        characterImage.sprite = characterData.characterImage;
        CheckElementMark(characterData.elementMark);

    }

    public void CheckElementMark(string elementMark)
    {
        markImage = markObject.GetComponent<Image>();


        switch (elementMark)
        {
            case "D":
                markImage.sprite = markDiamond;
                break;
            case "H":
                markImage.sprite = markHeart;
                break;
            case "C":
                markImage.sprite = markClover;
                break;
            case "S":
                markImage.sprite = markSpade;
                break;
            default:
                characterImage.sprite = null_img;
                markImage.sprite = null_img;
                break;

        }

    }
}
