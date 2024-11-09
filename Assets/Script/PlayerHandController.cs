using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHandController : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip draw;

    [SerializeField] Deck deckscript;
    [SerializeField] FieldController fieldscript;
    [SerializeField] Player player_script;

    //�g�����v�̃v���n�u
    public GameObject playercard;//�m�[�}���ۗ�
    private Image cardImage;
    private Card cardscript;

    //Prefab�I�u�W�F�N�g�̐e�I�u�W�F�N�g�ւ̎Q�Ƃ�ێ�����
    public Transform ParentObj;

    int field_start_posx = -620;
    private const int NUMBER_OF_HAND = 5;
    private const int SPACE_OF_CARD = 310;

    int playerhand_number = 0;
    //�J�[�h�f�[�^
    public struct CardData
    {
        public int number_serial;
        public int card_number;
        public string card_mark;
        public GameObject card_obj;
        public Image cardImage;
    }

    public CardData[] playerhands = new CardData[NUMBER_OF_HAND];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //��D���Z�b�g����
    public void clear_hand()
    {
        for (int i = 0; i < NUMBER_OF_HAND; i++)
        {
                playerhands[i].number_serial = 0;
        }
    }



    //�h���[����
    public void DrawHand(int deck_number_serial)
    {
        //number_serial�́A1�`53�܂ł̐����BDeck.cs����󂯎��B0�̓J�[�h���Ȃ���Ԃ�\���B
        //��D�̏󋵂��`�F�b�N���A�󂢂Ă�ꏊ�ɃV���A���i���o�[��������B

        if (deck_number_serial > 0)
        {
            for (int i = 0; i < NUMBER_OF_HAND; i++)
            {
                if (playerhands[i].number_serial == 0)
                {
                    Debug.Log(@$"PlayerHandController.cs {i}�Ԗڂ̃n���h����");
                    playerhands[i].number_serial = deck_number_serial;
                    playerhand_number = i;
                    break;
                }
            }
            check_draw_position(playerhand_number);
            change_carddata(playerhand_number, playerhands[playerhand_number].number_serial);
        }
        else
        {
            Debug.Log(@$"PlayerHandController.cs �R�D����ł�");
        }
    }

    void check_draw_position(int playerhand_number)
    {
        int posx = field_start_posx + playerhand_number * SPACE_OF_CARD;
        playerhands[playerhand_number].card_obj = Instantiate(playercard, ParentObj, false);
        playerhands[playerhand_number].card_obj.transform.localPosition = new Vector3(posx, 0, -1);
        Debug.Log(@$"PlayerHandController.cs check_draw_position {posx}�J�[�h�����ʒu�̐ݒ�");
    }


    void change_carddata(int playerhand_number,int number_serial)
    {
        cardImage = playerhands[playerhand_number].card_obj.GetComponent<Image>();
        cardscript = playerhands[playerhand_number].card_obj.GetComponent<Card>();//Card.cs��ǂݍ���

        play_se_draw();
        //�ʂ��ԍ��ɉ����ăJ�[�h�̉摜��ύX
        cardImage.sprite = Resources.Load<Sprite>("CardImages/" + number_serial.ToString());

        //�J�[�h�ɏ���n��
        decompose_card_parameter(playerhand_number, number_serial);
        cardscript.CardParameter(playerhand_number);
        Debug.Log(@$"PlayerHandController.cs change_carddata {playerhand_number}�Ԗڂ̃n���h�ɃJ�[�h��ǉ����܂��B");
    }

    //�J�[�h�̏ڍ׏���ݒ�
    public void decompose_card_parameter(int playerhand_number, int number_serial)
    {
        //�󂯎�����V���A���i���o�[�𐔎��ƃ}�[�N�ɕ����B
        if (number_serial >= 1 && number_serial <= 13)
        {
            playerhands[playerhand_number].card_number = number_serial;
            playerhands[playerhand_number].card_mark = "C";
        }
        else if (number_serial >= 14 && number_serial <= 26)
        {
            playerhands[playerhand_number].card_number = number_serial - 13;
            playerhands[playerhand_number].card_mark = "D";
        }
        else if (number_serial >= 27 && number_serial <= 39)
        {
            playerhands[playerhand_number].card_number = number_serial - 26;
            playerhands[playerhand_number].card_mark = "S";
        }
        else if (number_serial >= 40 && number_serial <= 52)
        {
            playerhands[playerhand_number].card_number = number_serial - 39;
            playerhands[playerhand_number].card_mark = "H";
        }
        else if (number_serial == 53)
        {
            playerhands[playerhand_number].card_number = 0;
            playerhands[playerhand_number].card_mark = "J";
        }
        else
        {
            Debug.Log(@$"�i���o�[�G���[:{number_serial}");
        }
    }


    //��D�̃J�[�h�Ɠ���ւ���֐�
    public void put_card_field(int hand_number)
    {
        
        int correct_judge = fieldscript.judge_putcard_center(playerhands[hand_number].card_number, playerhands[hand_number].card_mark, playerhands[hand_number].number_serial, playerhands[hand_number].card_obj);
        if (correct_judge == 1)
        {
            judge_sp_gauge(hand_number);
            player_script.attack_enemy();
            Debug.Log(@$"��ɏo���܂��� correct_judge��{correct_judge},{hand_number}���ڂɃh���[���܂��B");
            playerhands[hand_number].number_serial = 0;
            StartCoroutine(deckscript.Draw_One());
        }
        
    }

    //�J�[�h���o���邩�`�F�b�N�B�o����Ȃ�1��Ԃ��B
    public bool check_can_action()
    {
        int stuck_checker = 0;
        for (int i = 0; i < NUMBER_OF_HAND; i++){
            int correct_judge = fieldscript.check_hand_action(playerhands[i].card_number, playerhands[i].card_mark);
            if(correct_judge == 1)
            {
                stuck_checker = 0;
                break;
            }
            else
            {
                stuck_checker++;
            }
        }

        if(stuck_checker == NUMBER_OF_HAND)
        {
            return false;
        }
        return true; 
    }


    //SP�Q�[�W���������B�L�����N�^�[�̑����}�[�N�Əo�����J�[�h�̃}�[�N������ł����SP�Q�[�W�𑝉�������B
    private void judge_sp_gauge(int hand_number)
    {
        if (playerhands[hand_number].card_mark == player_script.element_mark)
        {
            player_script.plus_sp_gauge();
        }
    }


    //�G�l�~�[UI�����s����B
    public void put_handcard_center()
    {
        for (int i = 0; i < NUMBER_OF_HAND; i++)
        {
            int correct_judge = fieldscript.check_hand_action(playerhands[i].card_number, playerhands[i].card_mark);
            if (correct_judge == 1)
            {
                judge_sp_gauge(i);
                player_script.attack_enemy();
                fieldscript.judge_putcard_center(playerhands[i].card_number, playerhands[i].card_mark, playerhands[i].number_serial, playerhands[i].card_obj);
                playerhands[i].number_serial = 0;
                StartCoroutine(deckscript.Draw_One());
                break;
            }
        }
    }

    public void ready_game()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //�����ʉ��Đ�
    public void play_se_draw()
    {
        audioSource.clip = draw;
        audioSource.Play();
    }

}
