using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CharacterInfo : MonoBehaviour
{
    

    [SerializeField] TextMeshProUGUI chara_name_text;
    [SerializeField] TextMeshProUGUI chara_type_text;
    [SerializeField] TextMeshProUGUI sp_name_text;
    [SerializeField] TextMeshProUGUI sp_info_text;

    //キャラクターの立ち絵読み込み
    [SerializeField] public GameObject chara_obj;
    [SerializeField] private Image chara_img;
    [SerializeField] public Sprite null_img;
    [SerializeField] public Sprite argyle_img;
    [SerializeField] public Sprite kokoro_img;

    //属性マークの読み込み
    [SerializeField] public GameObject mark_obj;
    [SerializeField] private Image mark_img;
    [SerializeField] public Sprite mark_clover;
    [SerializeField] public Sprite mark_diamond;
    [SerializeField] public Sprite mark_spade;
    [SerializeField] public Sprite mark_heart;


    public void receive_data(CharacterData character_data)
    {
        chara_name_text.text = @$"{character_data.characterName}";
        chara_type_text.text = @$"{character_data.characterType}";
        sp_name_text.text = @$"{character_data.spName}";
        sp_info_text.text = @$"{character_data.spInfo}";
        chara_img = chara_obj.GetComponent<Image>();
        mark_img = mark_obj.GetComponent<Image>();


        switch (character_data.characterId) {
            case 1:
                chara_img.sprite = argyle_img;
                mark_img.sprite = mark_diamond;
                break;
            case 2:
                chara_img.sprite = kokoro_img;
                mark_img.sprite = mark_heart;
                break;
            default:
                chara_img.sprite = null_img;
                mark_img.sprite = null_img;
                break;

        }

    }
}
