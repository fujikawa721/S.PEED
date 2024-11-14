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

    //�g�����v�̃v���n�u
    public GameObject cardPrefab;
    private Image cardImage;
    private Card cardScript;

    //Prefab�I�u�W�F�N�g�̐e�I�u�W�F�N�g�ւ̎Q�Ƃ�ێ�����
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
    /// Deck.cs����1�`53�̃V���A���i���o�[���󂯎��B0�̓J�[�h���Ȃ���Ԃ�\���B
    /// ��D�̏󋵂��`�F�b�N���A�󂢂Ă�ꏊ�ɃV���A���i���o�[��������B
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
            Debug.Log(@$"PlayerHandController.cs �R�D����ł�");
        }
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
    /// �G�l�~�[UI���s�����鏈���B��D����D�ɒu�������̌�ASP���g�p�\���m�F����B
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
    /// ��D�����D�ɃJ�[�h���o�������B
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
    /// SP�Q�[�W���������B�L�����N�^�[�̑����}�[�N�Əo�����J�[�h�̃}�[�N������ł����SP�Q�[�W�𑝉�������B
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

        PlaySeDraw();
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

    

    //�����ʉ��Đ�
    public void PlaySeDraw()
    {
        audioSource.clip = draw;
        audioSource.Play();
    }

}
