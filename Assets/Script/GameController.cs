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
        playerHandController.ready_game();
        enemyHandController.ready_game();
        fieldController.ready_game();

        playerHandController.clear_hand();
        enemyHandController.clear_hand();
        StartCoroutine(deckScript.make_playerdeck());
        StartCoroutine(enemy_deckScript.make_playerdeck());

        yield return new WaitForSeconds(3.0f);//��D����������̎���
        StartCoroutine(deckScript.make_field(0));
        StartCoroutine(enemy_deckScript.make_field(1));
        yield return new WaitForSeconds(3.0f);//�GUI�̋N������
        now_playing_flg = 1;
        StartCoroutine(enemyUI.enemy_action());
    }


    public IEnumerator after_start()
    {
        
    //���҂��v���C����\�Ȏ��ɃQ�[���Ɋւ��Ď����s���B
        if (now_playing_flg == 1)
        {
            judge_bothplayer_hp();
            check_player_canaction();
            if (player_action_flg == 0)
            {
                dialogText.text = @$"�o����J�[�h������܂���B";
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
            Debug.Log(@$"�X�s�[�h����");
            dialogText.text = @$"��D�����Z�b�g���܂��B";
            yield return new WaitForSeconds(2.0f);
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
            dialogText.text = @$"�Q�[���I��";
        }
    }
}
