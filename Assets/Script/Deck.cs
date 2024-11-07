using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Deck : MonoBehaviour
{
    [SerializeField] PlayerHandController playerHand;
    [SerializeField] FieldController Field;
    [SerializeField] TextMeshProUGUI decks_zan_Text;

    private const int NUMBER_OF_DECK = 52;
    private const int NUMBER_OF_HAND = 5;

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

    public void update_deck_number()
    {
        decks_zan_Text.text = @$"{decks_zan}";
    }

    public IEnumerator make_playerdeck()
    {
        Debug.Log(decks.Length);
        DeckMake();
        Shuffle();
        yield return StartCoroutine(PlayerHandMake());
    }


    void DeckMake()
    {
        for (int i = 0; i < NUMBER_OF_DECK; i++) {
            decks[i] = i+1;
        }
    }

    void Shuffle()
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

    IEnumerator PlayerHandMake()
    {
        for (var i = 0; i < NUMBER_OF_HAND; i++)
        {
            Debug.Log(@$"手札生成処理を開始");
            decks_zan--;
            playerHand.DrawHand(decks[decks_zan]);
            yield return new WaitForSeconds(0.1f);//手札生成速度
        }
        yield return new WaitForSeconds(2.0f);//手札生成処理後の時間
    }

    public IEnumerator make_field(int fieldcard_number)
    {
        //場札の２枚を入れ替えるメソッド
        Debug.Log(@$"場札生成処理を開始");
            decks_zan--;
            Field.draw_deck(fieldcard_number,decks[decks_zan]);
            yield return new WaitForSeconds(0.2f);//場札生成速度
    }


    public IEnumerator Draw_One()
    {        
            Debug.Log(@$"ドロー処理を開始");
            decks_zan--;
            playerHand.DrawHand(decks[decks_zan]);
            yield return new WaitForSeconds(0.1f);//手札生成速度

    }

}
