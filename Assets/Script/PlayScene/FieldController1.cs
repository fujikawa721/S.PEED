using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FieldController : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip speed;
    public AudioClip whistle;

    //トランプのプレハブ
    public GameObject fieldcard;

    //Prefabオブジェクトの親オブジェクトへの参照を保持する
    public Transform ParentObj;

    //カード1枚目の初期位置
    int posx_start = -190;

    private const int SPACE_OF_CARD = 380;
    private const int NUMBER_OF_FIELD = 2;

    //カードデータ
    public struct CardData
    {
        public int serialNumber;
        public int cardNumber;
        public string cardMark;
        public int cardNumberUnder;
        public int cardNumberOver;
        public GameObject card;
        public Image cardImage;

    }

    public CardData[] fieldcards = new CardData[2];

    public IEnumerator ReadyGame()
    {
        audioSource = GetComponent<AudioSource>();
        yield return null;
    }

    /// <summary>
    /// Deck.csから場札の何枚目か、カード番号（シリアルナンバー）を受けとりその情報をもとにオブジェクトを生成する。
    /// </summary>
    public void DrawDeck(int fieldNumber,int numberFromDeck)
    {
        fieldcards[fieldNumber].serialNumber = numberFromDeck;
        int posx = posx_start + fieldNumber * SPACE_OF_CARD;
        Destroy(fieldcards[fieldNumber].card);
        fieldcards[fieldNumber].card = Instantiate(fieldcard, ParentObj, false);
        fieldcards[fieldNumber].card.transform.localPosition = new Vector3(posx, 0, -1);

        CardImageUpdate(fieldNumber, numberFromDeck);
        CardParameter(fieldNumber, numberFromDeck);

    }

    /// <summary>
    /// カードの詳細情報を決定する。受け取ったカードナンバーをトランプの数字とマークに分解し、
    /// 場札と入れ替えることができる数字１～１３を決定する。
    /// </summary>
    public void CardParameter(int fieldNumber,int serialNumber)
    {
        if (serialNumber >= 1 && serialNumber <= 13)
        {
            fieldcards[fieldNumber].cardNumber = serialNumber;
            fieldcards[fieldNumber].cardMark = "C";
        }
        else if (serialNumber >= 14 && serialNumber <= 26)
        {
            fieldcards[fieldNumber].cardNumber = serialNumber - 13;
            fieldcards[fieldNumber].cardMark = "D";
        }
        else if (serialNumber >= 27 && serialNumber <= 39)
        {
            fieldcards[fieldNumber].cardNumber = serialNumber - 26;
            fieldcards[fieldNumber].cardMark = "S";
        }
        else if (serialNumber >= 40 && serialNumber <= 52)
        {
            fieldcards[fieldNumber].cardNumber = serialNumber - 39;
            fieldcards[fieldNumber].cardMark = "H";
        }
        else if (serialNumber == 53)
        {
            fieldcards[fieldNumber].cardNumber = 0;
            fieldcards[fieldNumber].cardMark = "J";
        }
        else
        {
            Debug.Log(@$"ナンバーエラー:{serialNumber}");
        }

        
        if (fieldcards[fieldNumber].cardNumber >= 2 && fieldcards[fieldNumber].cardNumber <= 12)
        {
            fieldcards[fieldNumber].cardNumberUnder = fieldcards[fieldNumber].cardNumber -1;
            fieldcards[fieldNumber].cardNumberOver = fieldcards[fieldNumber].cardNumber + 1;
        }else if (fieldcards[fieldNumber].cardNumber == 1)
        {
            fieldcards[fieldNumber].cardNumberUnder = 13;
            fieldcards[fieldNumber].cardNumberOver = 2;
        }
        else if (fieldcards[fieldNumber].cardNumber == 13)
        {
            fieldcards[fieldNumber].cardNumberUnder = 12;
            fieldcards[fieldNumber].cardNumberOver = 1;
        }

    }

    public void CardImageUpdate(int fieldNumber, int serialNumber)
    {
        fieldcards[fieldNumber].cardImage = fieldcards[fieldNumber].card.GetComponent<Image>();
        fieldcards[fieldNumber].cardImage.sprite = Resources.Load<Sprite>("CardImages/" + serialNumber.ToString());
    }

    
    /// <summary>
    /// 手札からカード情報を受け取り、場札と入れ替える処理。
    /// </summary>
    public int PutCardField(int handCardNumber,string handCardMark,int handSerialNumber, GameObject handCard)
    {
        for (int i = 0; i < NUMBER_OF_FIELD; i++)
        {
            if (handCardNumber == fieldcards[i].cardNumberUnder || handCardNumber == fieldcards[i].cardNumberOver)
            {
                StartCoroutine(AnimatePutCardField(i,handCard));
                fieldcards[i].serialNumber = handSerialNumber;
                CardImageUpdate(i, fieldcards[i].serialNumber);
                CardParameter(i, fieldcards[i].serialNumber);
                return 1;
            }
        }
        Debug.Log(@$"お手付きです。");
        return 0;
    }

    /// <summary>
    /// 場札に出すカードが手札にあるか確認する処理。
    /// </summary>
    public int JudgeCanPutCard(int handCardNumber)
    {
        for (int i = 0; i < NUMBER_OF_FIELD; i++)
        {
            if (handCardNumber == fieldcards[i].cardNumberUnder || handCardNumber == fieldcards[i].cardNumberOver)
            {
                return 1;
            }
        }
        return 0;
    }


    public IEnumerator AnimatePutCardField(int fieldNumber, GameObject handCard)
    {
        int posx = posx_start + fieldNumber * SPACE_OF_CARD;
        handCard.transform.SetParent(ParentObj);
        handCard.transform.DOLocalMove(new Vector3(posx, 0, 0), 0.1f);
        yield return new WaitForSeconds(0.2f);
        Destroy(handCard);
    }

    public void PlaySeSpeed()
    {
        audioSource.clip = speed;
        audioSource.Play();
    }

    public void PlaySeWhistle()
    {
        audioSource.clip = whistle;
        audioSource.Play();
    }

    

}
