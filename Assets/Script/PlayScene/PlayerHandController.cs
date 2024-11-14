using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHandController : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip draw;

    [SerializeField] private Deck deck;
    [SerializeField] private FieldController fieldController;
    [SerializeField] private Player player;

    //トランプのプレハブ
    public GameObject cardPrefab;
    private Image cardImage;
    private Card cardScript;

    //Prefabオブジェクトの親オブジェクトへの参照を保持する
    public Transform ParentObj;

    private int drawPositionDefault = -620;
    private const int NUMBER_OF_HAND = 5;
    private const int SPACE_OF_CARD = 310;
    

    public struct CardData
    {
        public int serialNumber;
        public int cardNumber;
        public string cardMark;
        public GameObject cardObject;
        public Image cardImage;
    }

    public CardData[] playerHands = new CardData[NUMBER_OF_HAND];

    

    public IEnumerator ReadyGame()
    {
        audioSource = GetComponent<AudioSource>();
        ClearHand();
        yield return null;

    }

    
    public void ClearHand()
    {
        for (int i = 0; i < NUMBER_OF_HAND; i++)
        {
            playerHands[i].serialNumber = 0;
        }
    }

    /// <summary>
    /// Deck.csから1～53のシリアルナンバーを受け取る。0はカードがない状態を表す。
    /// 手札の状況をチェックし、空いてる場所にシリアルナンバーを代入する。
    /// </summary>
    public void DrawHand(int deckNumberSerial)
    {
        int playerHandNumber = 0;
        if (deckNumberSerial > 0)
        {
            for (int i = 0; i < NUMBER_OF_HAND; i++)
            {
                if (playerHands[i].serialNumber == 0)
                {
                    playerHands[i].serialNumber = deckNumberSerial;
                    playerHandNumber = i;
                    break;
                }
            }
            CheckDrawPosition(playerHandNumber);
            ChangeCarddata(playerHandNumber, playerHands[playerHandNumber].serialNumber);
        }
        else
        {
            Debug.Log(@$"PlayerHandController.cs 山札が空です");
        }
    }


    /// <summary>
    /// プレイヤーが手札をクリックした時に呼び出される。カードを置ける場合は攻撃を、置けなかった場合はお手付き処理を実行
    /// </summary>
    /// <param name="handNumber"></param>
    public void JudgePlayerAction(int handNumber)
    {
        int judgeResult = fieldController.JudgeCanPutCard(playerHands[handNumber].cardNumber);
        if (judgeResult == 1)
        {
            PutCardField(handNumber);
        }
        else
        {
            //お手付きの場合コンボをリセットする処理
        }
    }

    /// <summary>
    /// エネミーUIが行動する処理。手札を場札に置く処理の後、SPが使用可能か確認する。
    /// </summary>
    public void DoEnemyAction()
    {
        for (int i = 0; i < NUMBER_OF_HAND; i++)
        {
            int judgeResult = fieldController.JudgeCanPutCard(playerHands[i].cardNumber);
            if (judgeResult == 1)
            {
                PutCardField(i);
                break;
            }
        }
        DoSpecial();
    }


    /// <summary>
    /// 場札に置くことができるカードがプレイヤーの手札に存在するか確認する。
    /// </summary>
    public bool CheckCanAction()
    {
        int stuckChecker = 0;
        for (int i = 0; i < NUMBER_OF_HAND; i++){
            int correct_judge = fieldController.JudgeCanPutCard(playerHands[i].cardNumber);
            if(correct_judge == 1)
            {
                stuckChecker = 0;
                break;
            }
            else
            {
                stuckChecker++;
            }
        }

        if(stuckChecker == NUMBER_OF_HAND)
        {
            return false;
        }
        return true; 
    }

    /// <summary>
    /// 手札から場札にカードを出す処理。
    /// </summary>
    private void PutCardField(int handNumber)
    {
        JudgePlusSpPoint(handNumber);
        player.AttackEnemy();
        fieldController.PutCardField(playerHands[handNumber].cardNumber, playerHands[handNumber].cardMark, playerHands[handNumber].serialNumber, playerHands[handNumber].cardObject);
        playerHands[handNumber].serialNumber = 0;
        StartCoroutine(deck.DrawOne());
    }

    /// <summary>
    /// SPゲージ増加処理。キャラクターの属性マークと出したカードのマークが同一であればSPゲージを増加させる。
    /// </summary>
    private void JudgePlusSpPoint(int handNumber)
    {
        if (playerHands[handNumber].cardMark == player.characterData.element_mark)
        {
            player.PlusSpGauge();
        }
    }

    private  void DoSpecial()
    {
        if (player.canDoSpecial == true)
        {
            StartCoroutine(player.DoSpecial());
        }
    }

    /// <summary>
    /// ドローする位置を決定する。
    /// </summary>
    private void CheckDrawPosition(int playerhandNumber)
    {
        int posx = drawPositionDefault + playerhandNumber * SPACE_OF_CARD;
        playerHands[playerhandNumber].cardObject = Instantiate(cardPrefab, ParentObj, false);
        playerHands[playerhandNumber].cardObject.transform.localPosition = new Vector3(posx, 0, -1);
    }

    /// <summary>
    /// ドローしたオブジェクトにカードの情報を渡す。
    /// </summary>
    private void ChangeCarddata(int playerhandNumber, int serialNumber)
    {
        cardImage = playerHands[playerhandNumber].cardObject.GetComponent<Image>();
        cardScript = playerHands[playerhandNumber].cardObject.GetComponent<Card>();

        PlaySeDraw();
        cardImage.sprite = Resources.Load<Sprite>("CardImages/" + serialNumber.ToString());

        DecomposeCardParameter(playerhandNumber, serialNumber);
        cardScript.CardParameter(playerhandNumber, JudgePlayerAction);
    }


    /// <summary>
    /// カードの詳細情報を決定する。受け取ったカードナンバーをトランプの数字とマークに分解。
    /// </summary>
    private void DecomposeCardParameter(int playerhandNumber, int serialNumber)
    {
        if (serialNumber >= 1 && serialNumber <= 13)
        {
            playerHands[playerhandNumber].cardNumber = serialNumber;
            playerHands[playerhandNumber].cardMark = "C";
        }
        else if (serialNumber >= 14 && serialNumber <= 26)
        {
            playerHands[playerhandNumber].cardNumber = serialNumber - 13;
            playerHands[playerhandNumber].cardMark = "D";
        }
        else if (serialNumber >= 27 && serialNumber <= 39)
        {
            playerHands[playerhandNumber].cardNumber = serialNumber - 26;
            playerHands[playerhandNumber].cardMark = "S";
        }
        else if (serialNumber >= 40 && serialNumber <= 52)
        {
            playerHands[playerhandNumber].cardNumber = serialNumber - 39;
            playerHands[playerhandNumber].cardMark = "H";
        }
        else if (serialNumber == 53)
        {
            playerHands[playerhandNumber].cardNumber = 0;
            playerHands[playerhandNumber].cardMark = "J";
        }
        else
        {
            Debug.Log(@$"ナンバーエラー:{serialNumber}");
        }
    }

    

    //★効果音再生
    public void PlaySeDraw()
    {
        audioSource.clip = draw;
        audioSource.Play();
    }

}
