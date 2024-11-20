using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHandController : MonoBehaviour
{
    [SerializeField] private Deck deck;
    [SerializeField] private FieldController fieldController;
    [SerializeField] private PlaySeHand playSeHand;
    private string playerElementMark;

    // ��D�ɃJ�[�h��u�����ۂ̃R�[���o�b�N����
    public delegate void CardPut(int handNumber);
    private CardPut cardPutCallBack;

    //�g�����v�̃v���n�u
    public GameObject cardPrefab;
    private Image cardImage;
    private Card cardScript;

    
    public Transform ParentObj;

    private int drawPositionDefault = -620;
    
    //�v���C���[�̎�D�́y5���z
    private const int NUMBER_OF_HAND = 5;

    //�y��D�̃J�[�h���m�̊Ԋu�͉��y310�zpx�z
    private const int SPACE_OF_CARD = 310;
    
    //�h���[�ɂ����鑬�x�́y0.1f�z
    private const float SPEED_DRAWHAND = 0.1f;

    public struct CardData
    {
        public int serialNumber;
        public int cardNumber;
        public string cardMark;
        public GameObject cardObject;
        public Image cardImage;
    }


    public CardData[] playerHands = new CardData[NUMBER_OF_HAND];

    

    public IEnumerator ReadyGame(CardPut putCardAction)
    {
        yield return StartCoroutine(playSeHand.ReadyAudio());
        cardPutCallBack = putCardAction;
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
    /// �Q�[���J�n����GameController����Ăяo������D���������B
    /// </summary>
    public IEnumerator MakePlayerHand()
    {
        for (var i = 0; i < NUMBER_OF_HAND; i++)
        {
            yield return StartCoroutine(DrawHand());
        }
        yield return null;
    }


    /// <summary>
    /// �v���C���[����D���N���b�N�������ɌĂяo�����B�J�[�h��u����ꍇ�͍U�����A�u���Ȃ������ꍇ�͂���t�����������s
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
            //����t���̏ꍇ�R���{�����Z�b�g���鏈��
        }
    }

    /// <summary>
    /// ��D�ɒu�����Ƃ��ł���J�[�h���v���C���[�̎�D�ɑ��݂��邩�m�F����B
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
    /// �G�l�~�[UI���s�����鏈���B��D����D�ɒu�������̌�ASP���g�p�\���m�F����B
    /// </summary>
    public IEnumerator PutCardOfEnemy()
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
        yield return null;
    }


    /// <summary>
    /// Deck.cs����1�`53�̃V���A���i���o�[���󂯎��B0�̓J�[�h���Ȃ���Ԃ�\���B
    /// ��D�̏󋵂��`�F�b�N���A�󂢂Ă�ꏊ�ɃV���A���i���o�[��������B
    /// </summary>
    private IEnumerator DrawHand()
    {
        int deckSerialNumber = deck.DrawOne();
        yield return new WaitForSeconds(SPEED_DRAWHAND);
        int playerHandNumber = 0;
        if (deckSerialNumber > 0)
        {
            for (int i = 0; i < NUMBER_OF_HAND; i++)
            {
                if (playerHands[i].serialNumber == 0)
                {
                    playerHands[i].serialNumber = deckSerialNumber;
                    playerHandNumber = i;
                    break;
                }
            }
            CheckDrawPosition(playerHandNumber);
            ChangeCarddata(playerHandNumber, playerHands[playerHandNumber].serialNumber);
        }
        else
        {
            Debug.Log(@$"PlayerHandController.cs �R�D����ł�");
        }
    }

    /// <summary>
    /// ��D�����D�ɃJ�[�h���o�������B
    /// </summary>
    private void PutCardField(int handNumber)
    {
        cardPutCallBack(handNumber);
        fieldController.PutCardField(playerHands[handNumber].cardNumber, playerHands[handNumber].cardMark, playerHands[handNumber].serialNumber, playerHands[handNumber].cardObject);
        playerHands[handNumber].serialNumber = 0;
        StartCoroutine(DrawHand());
    }

    /// <summary>
    /// ��D�����D�ɏo�����J�[�h�������J�[�h�����肷��B
    /// �L�����N�^�[�̑����}�[�N�Əo�����J�[�h�̃}�[�N������ł����SP�Q�[�W�𑝉�������B
    /// </summary>
    public bool CheckElement(int handNumber,string playerElementMark)
    {
        if (playerHands[handNumber].cardMark == playerElementMark)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// �h���[����ʒu�����肷��B
    /// </summary>
    private void CheckDrawPosition(int playerhandNumber)
    {
        int posx = drawPositionDefault + playerhandNumber * SPACE_OF_CARD;
        playerHands[playerhandNumber].cardObject = Instantiate(cardPrefab, ParentObj, false);
        playerHands[playerhandNumber].cardObject.transform.localPosition = new Vector3(posx, 0, -1);
    }

    /// <summary>
    /// �h���[�����I�u�W�F�N�g�ɃJ�[�h�̏���n���B
    /// </summary>
    private void ChangeCarddata(int playerhandNumber, int serialNumber)
    {
        cardImage = playerHands[playerhandNumber].cardObject.GetComponent<Image>();
        cardScript = playerHands[playerhandNumber].cardObject.GetComponent<Card>();

        playSeHand.PlaySeDraw();
        cardImage.sprite = Resources.Load<Sprite>("CardImages/" + serialNumber.ToString());

        DecomposeCardParameter(playerhandNumber, serialNumber);
        cardScript.CardParameter(playerhandNumber, JudgePlayerAction);
    }


    /// <summary>
    /// �J�[�h�̏ڍ׏������肷��B�󂯎�����J�[�h�i���o�[���g�����v�̐����ƃ}�[�N�ɕ����B
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
            Debug.Log(@$"�i���o�[�G���[:{serialNumber}");
        }
    }


}
