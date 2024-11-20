using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{

    //[SerializeField] PlayerHand playerHand;
    //[SerializeField] PlayerDeck playerDeck;
    
    
    [SerializeField] Deck deck;
    [SerializeField] Deck enemyDeck;
    [SerializeField] EnemyUI enemyUI;
    [SerializeField] FieldController fieldController;
    [SerializeField] Player player;
    [SerializeField] Player enemyPlayer;
    [SerializeField] GameObject noactionCurtain;
    [SerializeField] GameObject gameMessage;
    [SerializeField] GameGuidance gameGuidance;

    [SerializeField] PlayerHandController playerHandController;
    [SerializeField] PlayerHandController enemyHandController;

    [SerializeField] TextMeshProUGUI playerActionText;
    [SerializeField] TextMeshProUGUI gameMessageText;


    public bool canPlayerAction;
    public bool canEnemyAction;
    public bool canPlayNow;
    public bool endGameFlg;




    void Start()
    {
        StartCoroutine(StartGame());

    }

    void Update()
    {
        StartCoroutine(JudgePlaying());
    }


    public IEnumerator StartGame()
    {
        yield return StartCoroutine(LoadGame());
        gameMessage.SetActive(true);
        gameMessageText.text = @$"READY...";
        
        StartCoroutine(ReadyPlayer());
        StartCoroutine(ReadyEnemy());
        yield return new WaitForSeconds(3.0f);//��D����������̎���

        gameMessageText.text = @$"START!!";
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(deck.MakeField(0));
        StartCoroutine(enemyDeck.MakeField(1));
        fieldController.PlaySeSpeed();
        canPlayNow = true;
        noactionCurtain.SetActive(false);
        gameMessage.SetActive(false);
        StartCoroutine(enemyUI.ActionEnemy());
        gameMessageText.text = @$"";
    }

    public void PauseGamePlaying()
    {
        canPlayNow = false;
        noactionCurtain.SetActive(true);
    }

    public void ReStartGamePlaying()
    {
        canPlayNow = true;
        noactionCurtain.SetActive(false);
    }


    private IEnumerator ReadyPlayer()
    {
        yield return StartCoroutine(deck.MakePlayerDeck());
        yield return StartCoroutine(playerHandController.MakePlayerHand());  
    }

    private IEnumerator ReadyEnemy()
    {
        yield return StartCoroutine(enemyDeck.MakePlayerDeck());
        yield return StartCoroutine(enemyHandController.MakePlayerHand());
    }


    /// <summary>
    /// �v���C�\���Ɉȉ��𔻒肷��B�u�Q�[���I�������v�u�v���C���[�̍s���ہv�uSP�����\�ہv�u�d�؂蒼�����s�ہv
    /// </summary>
    private IEnumerator JudgePlaying()
    {
        if (canPlayNow == true)
        {
            JudgeBothPlayerHp();
            CheckPlayerCanAction();
            
            if (canPlayerAction == false)
            {
                playerActionText.text = @$"�o����J�[�h������܂���B";
            }
            else
            {
                playerActionText.text = @$""; ;
            }

            if (player.canDoSpecial == true)
            {
                playerActionText.text = @$"S.P�Q�[�W�����^���ł��B�R�D�^�b�`��S.P����";
            }

            if (canPlayerAction == false && canEnemyAction == false)
            {
                canPlayNow = false;
                yield return StartCoroutine(Speed());
            }
        }
        yield return null;
    }

    private void CheckPlayerCanAction()
    {
        canPlayerAction = playerHandController.CheckCanAction();
        canEnemyAction = enemyHandController.CheckCanAction();
    }

    /// <summary>
    /// ���v���C���[����D�ɃJ�[�h��u���Ȃ��ꍇ�A���v���C���[�̎R�D�����D��1�����J�[�h��u���B
    /// �Q�[�����ł́w�d�؂蒼���y�X�s�[�h�z�ƌĂԁB�x
    /// </summary>
    public IEnumerator Speed()
    {
        yield return new WaitForSeconds(1.0f);
        fieldController.PlaySeWhistle();
        PauseGamePlaying();
        gameMessage.SetActive(true);
        gameMessageText.text = @$"�d�؂蒼��";
        playerActionText.text = @$"��D�����Z�b�g���܂��B";
        yield return new WaitForSeconds(1.0f);

        gameMessageText.text = @$"�X�s�[�@ ";
        gameGuidance.play_se_voice_spee();
        yield return new WaitForSeconds(1.0f);

        gameMessageText.text = @$"�X�s�[�h!";
        gameGuidance.play_se_voice_do();
        yield return new WaitForSeconds(0.5f);
        
        fieldController.PlaySeSpeed();
        StartCoroutine(deck.MakeField(0));
        StartCoroutine(enemyDeck.MakeField(1));
        gameMessage.SetActive(false);
        gameMessageText.text = @$"";
        ReStartGamePlaying();

        yield return null;
    }

    /// <summary>
    /// �ǂ��炩�̃v���C���[��HP��0�ȉ��ɂȂ�ƃQ�[�����I������B
    /// </summary>
    public void JudgeBothPlayerHp()
    {
        if(player.nowHp <= 0 || enemyPlayer.nowHp <= 0)
        {
            canPlayNow = false;
            endGameFlg = true;
            noactionCurtain.SetActive(true);
            gameMessage.SetActive(true);
            gameMessageText.text = @$"GAME SET!!";
            fieldController.PlaySeWhistle();
        }
    }

    private IEnumerator LoadGame()
    {
        yield return StartCoroutine(PassCharacterData());
        yield return StartCoroutine(player.ReadyGame());
        yield return StartCoroutine(enemyPlayer.ReadyGame());
        yield return StartCoroutine(fieldController.ReadyGame());
        yield return StartCoroutine(deck.ReadyGame());
        yield return StartCoroutine(enemyDeck.ReadyGame());
        

    }

    /// <summary>
    /// �I����ʂőI�������L�����N�^�[���v���C���[�I�u�W�F�N�g�ɓn���B�����Ƀf�[�^�������Ă��Ȃ��ꍇ�́A
    /// �\�߃A�^�b�`����Ă����L�����N�^�[�f�[�^�����̂܂ܓǂݍ��ށB
    /// </summary>
    private IEnumerator PassCharacterData()
    {
        if (SelectController.player_character_data != null && SelectController.enemy_character_data != null)
        {
            player.characterData = SelectController.player_character_data;
            enemyPlayer.characterData = SelectController.enemy_character_data;
        }
        yield return null;
    }

}
