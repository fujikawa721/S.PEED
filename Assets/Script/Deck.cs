using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Deck : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] PlayerHandController playerHand;
    [SerializeField] FieldController Field;
    [SerializeField] TextMeshProUGUI decks_zan_Text;
    [SerializeField] Player player_script;

    AudioSource audioSource;
    public AudioClip deck_max;


    private const int NUMBER_OF_DECK = 26;
    private const int NUMBER_OF_HAND = 5;
    private const float SPEED_DRAWHAND = 0.1f;
    private const float SPEED_DRAWFIELD = 0.2f;

    private int[] decks = new int[NUMBER_OF_DECK];
    private int decks_zan = NUMBER_OF_DECK;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(@$"�R�D���N���b�N���ꂽ");
        Debug.Log(@$"SP�|�C���g{player_script.now_sp_point}");

        if (player_script.can_special == true)
        {
            Debug.Log(@$"�X�y�V���������I");
            StartCoroutine(player_script.check_special_type());
        }
        else
        {
            Debug.Log(@$"�X�y�V�����Q�[�W�����܂��Ă܂���");
        }
    }



    public void update_deck_number()
    {
        decks_zan_Text.text = @$"{decks_zan}";
    }

    public IEnumerator make_playerdeck()
    {
        play_se_deck_max();
        decks_zan = NUMBER_OF_DECK;
        make_deck_serial_number();
        shuffle_deck();
        yield return null;
    }


    void make_deck_serial_number()
    {
        for (int i = 0; i < NUMBER_OF_DECK; i++) {
            decks[i] = i+1;
        }
    }

    void shuffle_deck()
    {
        Debug.Log(@$"�f�b�L������{decks_zan}�A�V���b�t���������J�n");
        for (var i = decks_zan-1; i >= 0; i--)
        {
            var j = Random.Range(0, NUMBER_OF_DECK);
            var tmp = decks[i];
            decks[i] = decks[j];
            decks[j] = tmp;           
        }

    }

    //�Q�[���J�n���̎�D��������
    public IEnumerator make_playerhand()
    {
        for (var i = 0; i < NUMBER_OF_HAND; i++)
        {
            Debug.Log(@$"��D�����������J�n");
            decks_zan--;
            playerHand.DrawHand(decks[decks_zan]);
            yield return new WaitForSeconds(SPEED_DRAWHAND);
        }
        yield return new WaitForSeconds(2.0f);//��D����������̎���
    }

    public IEnumerator make_field(int fieldcard_number)
    {
        if (decks_zan < 1)
        {
            yield return StartCoroutine(make_playerdeck());
        }
        Debug.Log(@$"��D�����������J�n");
            decks_zan--;
            Field.draw_deck(fieldcard_number, decks[decks_zan]);
            yield return new WaitForSeconds(SPEED_DRAWFIELD);
    }


    public IEnumerator Draw_One()
    {
        if (decks_zan < 1)
        {
            
            yield return StartCoroutine(make_playerdeck());
        }

        Debug.Log(@$"�h���[�������J�n");
        decks_zan--;
        playerHand.DrawHand(decks[decks_zan]);
        yield return new WaitForSeconds(SPEED_DRAWHAND);
    
    }

    //�������ʉ�����
    public IEnumerator ready_game()
    {
        audioSource = GetComponent<AudioSource>();
        yield return null;
    }

    public void play_se_deck_max()
    {
        audioSource.clip = deck_max;
        audioSource.Play();
    }


}
