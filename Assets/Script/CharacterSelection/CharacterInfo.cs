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

    //�L�����N�^�[�̗����G�ǂݍ���
    [SerializeField] public GameObject characterObject;
    [SerializeField] private Image characterImage;
    [SerializeField] public Sprite nullImg;

    //�����}�[�N�̓ǂݍ���
    [SerializeField] public GameObject markObject;
    [SerializeField] private Image markImage;
    [SerializeField] public Sprite markClover;
    [SerializeField] public Sprite markDiamond;
    [SerializeField] public Sprite markSpade;
    [SerializeField] public Sprite markHeart;


    /// <summary>
    /// �L�����N�^�[�{�^������L�����N�^�[�f�[�^���󂯎��e��f�[�^��ݒ肷��B
    /// </summary>
    public void ReceiveData(CharacterData characterData)
    {
        characterNameText.text = @$"{characterData.characterName}";
        characterTypeText.text = @$"{characterData.characterType}";
        spNameText.text = @$"{characterData.spName}";
        spInfoText.text = @$"{characterData.spInfo}";
        characterImage = characterObject.GetComponent<Image>();
        characterImage.sprite = characterData.characterImage;
        CheckElementMark(characterData.elementMark);

    }

    /// <summary>
    /// �L�����N�^�[�̑����L�����m�F���\������}�[�N��ݒ肷��B
    /// </summary>
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
                characterImage.sprite = nullImg;
                markImage.sprite = nullImg;
                break;

        }

    }
}
