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

    //�g�����v�̃v���n�u
    public GameObject fieldcard;

    //Prefab�I�u�W�F�N�g�̐e�I�u�W�F�N�g�ւ̎Q�Ƃ�ێ�����
    public Transform ParentObj;

    //�J�[�h1���ڂ̏����ʒu
    int posx_start = -190;

    private const int SPACE_OF_CARD = 380;
    private const int NUMBER_OF_FIELD = 2;

    //�J�[�h�f�[�^
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
    /// Deck.cs�����D�̉����ڂ��A�J�[�h�ԍ��i�V���A���i���o�[�j���󂯂Ƃ肻�̏������ƂɃI�u�W�F�N�g�𐶐�����B
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
    /// �J�[�h�̏ڍ׏������肷��B�󂯎�����J�[�h�i���o�[���g�����v�̐����ƃ}�[�N�ɕ������A
    /// ��D�Ɠ���ւ��邱�Ƃ��ł��鐔���P�`�P�R�����肷��B
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
            Debug.Log(@$"�i���o�[�G���[:{serialNumber}");
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
    /// ��D����J�[�h�����󂯎��A��D�Ɠ���ւ��鏈���B
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
        Debug.Log(@$"����t���ł��B");
        return 0;
    }

    /// <summary>
    /// ��D�ɏo���J�[�h����D�ɂ��邩�m�F���鏈���B
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
