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

    [SerializeField] PlayerHandController playerHand;

    //Prefabオブジェクトの親オブジェクトへの参照を保持する
    public Transform ParentObj;

    //カード1枚目の初期位置
    int posx_start = -190;

    private const int SPACE_OF_CARD = 380;

    //カードデータ
    public struct CardData
    {
        public int number_serial;
        public int card_number;
        public string card_mark;
        public int card_number_under;
        public int card_number_over;
        public GameObject card;
        public Image cardImage;

    }

    public CardData[] fieldcards = new CardData[2];

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ready_game()
    {
        audioSource = GetComponent<AudioSource>();
        yield return null;
    }

    //ドロー処理
    public void draw_deck(int fieldcard_number,int number_serial)
    {
        //number_serialは、1～52までの数字。Deck.csから受け取る。
        fieldcards[fieldcard_number].number_serial = number_serial;
        int posx = posx_start + fieldcard_number * SPACE_OF_CARD;
        fieldcards[fieldcard_number].card = Instantiate(fieldcard, ParentObj, false);
        fieldcards[fieldcard_number].card.transform.localPosition = new Vector3(posx, 0, -1);

        CardImageUpdate(fieldcard_number, number_serial);
        CardParameter(fieldcard_number, number_serial);

    }

    public void CardParameter(int fieldcard_number,int number_serial)
    {
        //受け取ったシリアルナンバーを数字とマークに分解。
        if (number_serial >= 1 && number_serial <= 13)
        {
            fieldcards[fieldcard_number].card_number = number_serial;
            fieldcards[fieldcard_number].card_mark = "C";
        }
        else if (number_serial >= 14 && number_serial <= 26)
        {
            fieldcards[fieldcard_number].card_number = number_serial - 13;
            fieldcards[fieldcard_number].card_mark = "D";
        }
        else if (number_serial >= 27 && number_serial <= 39)
        {
            fieldcards[fieldcard_number].card_number = number_serial - 26;
            fieldcards[fieldcard_number].card_mark = "S";
        }
        else if (number_serial >= 40 && number_serial <= 52)
        {
            fieldcards[fieldcard_number].card_number = number_serial - 39;
            fieldcards[fieldcard_number].card_mark = "H";
        }
        else if (number_serial == 53)
        {
            fieldcards[fieldcard_number].card_number = 0;
            fieldcards[fieldcard_number].card_mark = "J";
        }
        else
        {
            Debug.Log(@$"ナンバーエラー:{number_serial}");
        }

        //入れ替える時の数字を決定
        if (fieldcards[fieldcard_number].card_number >= 2 && fieldcards[fieldcard_number].card_number <= 12)
        {
            fieldcards[fieldcard_number].card_number_under = fieldcards[fieldcard_number].card_number -1;
            fieldcards[fieldcard_number].card_number_over = fieldcards[fieldcard_number].card_number + 1;
        }else if (fieldcards[fieldcard_number].card_number == 1)
        {
            fieldcards[fieldcard_number].card_number_under = 13;
            fieldcards[fieldcard_number].card_number_over = 2;
        }
        else if (fieldcards[fieldcard_number].card_number == 13)
        {
            fieldcards[fieldcard_number].card_number_under = 12;
            fieldcards[fieldcard_number].card_number_over = 1;
        }

    }

    public void CardImageUpdate(int fieldcard_number, int number_serial)
    {
        //カードの画像を更新する関数
        fieldcards[fieldcard_number].cardImage = fieldcards[fieldcard_number].card.GetComponent<Image>();
        fieldcards[fieldcard_number].cardImage.sprite = Resources.Load<Sprite>("CardImages/" + number_serial.ToString());
    }

    //手札のカードと入れ替える関数
    public int judge_putcard_center(int hand_card_number,string hand_card_mark,int hand_number_serial, GameObject hand_card)
    {
        for (int i = 0; i < 2; i++)
        {
            if (hand_card_number == fieldcards[i].card_number_under || hand_card_number == fieldcards[i].card_number_over)
            {
                StartCoroutine(animate_putcard_center(i,hand_card));
                fieldcards[i].number_serial = hand_number_serial;
                CardImageUpdate(i, fieldcards[i].number_serial);
                CardParameter(i, fieldcards[i].number_serial);
                
                return 1;
            }
        }
        Debug.Log(@$"お手付きです。");
        return 0;
    }

    public int check_hand_action(int hand_card_number, string hand_card_mark)
    {
        for (int i = 0; i < 2; i++)
        {
            if (hand_card_number == fieldcards[i].card_number_under || hand_card_number == fieldcards[i].card_number_over)
            {
                return 1;
            }
        }
        return 0;
    }


    public IEnumerator animate_putcard_center(int center_number, GameObject hand_card)
    {
        int posx = posx_start + center_number * SPACE_OF_CARD;
        hand_card.transform.SetParent(ParentObj);
        hand_card.transform.DOLocalMove(new Vector3(posx, 0, 0), 0.1f);
        yield return new WaitForSeconds(0.2f);
        Destroy(hand_card);
    }

    public void play_se_speed()
    {
        audioSource.clip = speed;
        audioSource.Play();
    }

    public void play_se_whistle()
    {
        audioSource.clip = whistle;
        audioSource.Play();
        Debug.Log(@$"ホイッスルSE");
    }

    

}
