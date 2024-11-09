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
        Debug.Log(@$"山札がクリックされた");
        Debug.Log(@$"SPポイント{player_script.now_sp_point}");

        if (player_script.can_special == true)
        {
            Debug.Log(@$"スペシャル発動！");
            StartCoroutine(player_script.check_special_type());
        }
        else
        {
            Debug.Log(@$"スペシャルゲージが溜まってません");
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
        Debug.Log(@$"デッキ枚数は{decks_zan}、シャッフル処理を開始");
        for (var i = decks_zan-1; i >= 0; i--)
        {
            var j = Random.Range(0, NUMBER_OF_DECK);
            var tmp = decks[i];
            decks[i] = decks[j];
            decks[j] = tmp;           
        }

    }

    //ゲーム開始時の手札生成処理
    public IEnumerator make_playerhand()
    {
        for (var i = 0; i < NUMBER_OF_HAND; i++)
        {
            Debug.Log(@$"手札生成処理を開始");
            decks_zan--;
            playerHand.DrawHand(decks[decks_zan]);
            yield return new WaitForSeconds(SPEED_DRAWHAND);
        }
        yield return new WaitForSeconds(2.0f);//手札生成処理後の時間
    }

    public IEnumerator make_field(int fieldcard_number)
    {
        if (decks_zan < 1)
        {
            yield return StartCoroutine(make_playerdeck());
        }
        Debug.Log(@$"場札生成処理を開始");
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

        Debug.Log(@$"ドロー処理を開始");
        decks_zan--;
        playerHand.DrawHand(decks[decks_zan]);
        yield return new WaitForSeconds(SPEED_DRAWHAND);
    
    }

    //★★効果音★★
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
