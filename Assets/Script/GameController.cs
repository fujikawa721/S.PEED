using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{

    //[SerializeField] PlayerHand playerHand;
    //[SerializeField] PlayerDeck playerDeck;
    
    
    [SerializeField] Deck deckScript;
    [SerializeField] EnemyDeck enemy_deckScript;
    [SerializeField] EnemyUI enemyUI;
    [SerializeField] FieldController fieldController;
    [SerializeField] Player player;
    [SerializeField] Player enemyplayer;

    [SerializeField] PlayerHandController playerHandController;
    [SerializeField] EnemyHandController enemyHandController;

    [SerializeField] TextMeshProUGUI dialogText;


    public int player_action_flg;
    public int enemy_action_flg;
    public int now_playing_flg = 0;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(start_game());
        


    }

    // Update is called once per frame
    void Update()
    {
        deckScript.update_deck_number();
        enemy_deckScript.update_deck_number();
        StartCoroutine(after_start());

    }


    public IEnumerator start_game()
    {
        yield return StartCoroutine(fieldController.ready_game());
        yield return StartCoroutine(deckScript.ready_game());
        yield return StartCoroutine(enemy_deckScript.ready_game());

        StartCoroutine(ready_player());
        StartCoroutine(ready_enemy());
        yield return new WaitForSeconds(3.0f);//手札生成処理後の時間

        StartCoroutine(deckScript.make_field(0));
        StartCoroutine(enemy_deckScript.make_field(1));
        fieldController.play_se_speed();
        now_playing_flg = 1;
        StartCoroutine(enemyUI.enemy_action());
    }

    public IEnumerator ready_player()
    {
        enemyHandController.ready_game();
        enemyHandController.clear_hand();
        yield return StartCoroutine(enemy_deckScript.make_playerdeck());
        yield return StartCoroutine(enemy_deckScript.make_playerhand());
    }

    public IEnumerator ready_enemy()
    {
        playerHandController.ready_game();
        playerHandController.clear_hand();
        yield return StartCoroutine(deckScript.make_playerdeck());
        yield return StartCoroutine(deckScript.make_playerhand());
    }


    public IEnumerator after_start()
    {
        
    //両者がプレイ操作可能な時にゲームに関わる監視を行う。
        if (now_playing_flg == 1)
        {
            judge_bothplayer_hp();
            check_player_canaction();
            if (player_action_flg == 0)
            {
                dialogText.text = @$"出せるカードがありません。";
            }
            else
            {
                dialogText.text = @$""; ;
            }
            
            yield return StartCoroutine(speed());

        }
        yield return null;
    }

    public void check_player_canaction()
    {
        player_action_flg = playerHandController.check_can_action();
        enemy_action_flg = enemyHandController.check_can_action();
    }

    public IEnumerator speed()
    {
        if (player_action_flg == 0 && enemy_action_flg == 0)
        {
            
            now_playing_flg = 0;
            yield return new WaitForSeconds(1.0f);
            Debug.Log(@$"スピード成立");
            dialogText.text = @$"場札をリセットします。";
            yield return new WaitForSeconds(2.0f);
            fieldController.play_se_speed();
            StartCoroutine(deckScript.make_field(0));
            StartCoroutine(enemy_deckScript.make_field(1));
            now_playing_flg = 1;
        }
        yield return null;
    }

    public void judge_bothplayer_hp()
    {
        if(player.now_hp <= 0 || enemyplayer.now_hp <= 0)
        {
            now_playing_flg = 0;
            dialogText.text = @$"ゲーム終了";
        }
    }
}
