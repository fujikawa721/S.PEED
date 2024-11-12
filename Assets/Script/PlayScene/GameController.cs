using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] GameObject noaction_curtain;
    [SerializeField] GameGuidance gameGuidance;

    [SerializeField] PlayerHandController playerHandController;
    [SerializeField] EnemyHandController enemyHandController;

    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] TextMeshProUGUI game_message;


    public bool player_action_flg;
    public bool enemy_action_flg;
    public bool now_playing_flg;
    public bool end_game_flg;




    void Start()
    {
        StartCoroutine(start_game());

    }

    void Update()
    {
        deckScript.update_deck_number();
        enemy_deckScript.update_deck_number();
        StartCoroutine(after_start());

    }


    public IEnumerator start_game()
    {
        pass_character_data();
        game_message.text = @$"READY...";
        yield return StartCoroutine(fieldController.ready_game());
        yield return StartCoroutine(deckScript.ready_game());
        yield return StartCoroutine(enemy_deckScript.ready_game());

        StartCoroutine(ready_player());
        StartCoroutine(ready_enemy());
        yield return new WaitForSeconds(3.0f);//��D����������̎���

        game_message.text = @$"START!!";
        StartCoroutine(deckScript.make_field(0));
        StartCoroutine(enemy_deckScript.make_field(1));
        fieldController.play_se_speed();
        now_playing_flg = true;
        noaction_curtain.SetActive(false);
        StartCoroutine(enemyUI.enemy_action());
        game_message.text = @$"";
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
        
    //���҂��v���C����\�Ȏ��ɃQ�[���Ɋւ��Ď����s���B
        if (now_playing_flg == true)
        {
            judge_bothplayer_hp();
            check_player_canaction();
            
            if (player_action_flg == false)
            {
                dialogText.text = @$"�o����J�[�h������܂���B";
            }
            else
            {
                dialogText.text = @$""; ;
            }
            
            yield return StartCoroutine(speed());

            if (player.can_special == true)
            {
                dialogText.text = @$"S.P�Q�[�W�����^���ł��B�R�D�^�b�`��S.P����";
            }

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
        if (player_action_flg == false && enemy_action_flg == false)
        {
            now_playing_flg = false;
            yield return new WaitForSeconds(1.0f);

            noaction_curtain.SetActive(true);
            fieldController.play_se_whistle();
            gameGuidance.play_se_voice_break();
            game_message.text = @$"�d�؂蒼��";
            dialogText.text = @$"��D�����Z�b�g���܂��B";
            yield return new WaitForSeconds(2.0f);

            game_message.text = @$"�X�s�[�@ ";
            gameGuidance.play_se_voice_spee();
            yield return new WaitForSeconds(2.0f);

            game_message.text = @$"�X�s�[�h!";
            gameGuidance.play_se_voice_do();
            yield return new WaitForSeconds(0.5f);
            fieldController.play_se_speed();
            StartCoroutine(deckScript.make_field(0));
            StartCoroutine(enemy_deckScript.make_field(1));
            noaction_curtain.SetActive(false);
            now_playing_flg = true;
            game_message.text = @$"";

        }
        yield return null;
    }

    public void judge_bothplayer_hp()
    {
        if(player.now_hp <= 0 || enemyplayer.now_hp <= 0)
        {
            now_playing_flg = false;
            end_game_flg = true;
            noaction_curtain.SetActive(true);
            game_message.text = @$"GAME SET!!";
            fieldController.play_se_whistle();
        }
    }

    private void pass_character_data()
    {
        player.charaData = SelectController.player_character_data;
        enemyplayer.charaData = SelectController.enemy_character_data;
        player.ready_game();
        enemyplayer.ready_game();
    }

}