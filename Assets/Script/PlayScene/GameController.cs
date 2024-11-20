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
        yield return new WaitForSeconds(3.0f);//手札生成処理後の時間

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
    /// プレイ可能中に以下を判定する。「ゲーム終了条件」「プレイヤーの行動可否」「SP発動可能可否」「仕切り直し実行可否」
    /// </summary>
    private IEnumerator JudgePlaying()
    {
        if (canPlayNow == true)
        {
            JudgeBothPlayerHp();
            CheckPlayerCanAction();
            
            if (canPlayerAction == false)
            {
                playerActionText.text = @$"出せるカードがありません。";
            }
            else
            {
                playerActionText.text = @$""; ;
            }

            if (player.canDoSpecial == true)
            {
                playerActionText.text = @$"S.Pゲージが満タンです。山札タッチでS.P発動";
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
    /// 両プレイヤーが場札にカードを置けない場合、両プレイヤーの山札から場札に1枚ずつカードを置く。
    /// ゲーム内では『仕切り直し【スピード】と呼ぶ。』
    /// </summary>
    public IEnumerator Speed()
    {
        yield return new WaitForSeconds(1.0f);
        fieldController.PlaySeWhistle();
        PauseGamePlaying();
        gameMessage.SetActive(true);
        gameMessageText.text = @$"仕切り直し";
        playerActionText.text = @$"場札をリセットします。";
        yield return new WaitForSeconds(1.0f);

        gameMessageText.text = @$"スピー　 ";
        gameGuidance.play_se_voice_spee();
        yield return new WaitForSeconds(1.0f);

        gameMessageText.text = @$"スピード!";
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
    /// どちらかのプレイヤーのHPが0以下になるとゲームを終了する。
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
    /// 選択画面で選択したキャラクターをプレイヤーオブジェクトに渡す。両方にデータが入っていない場合は、
    /// 予めアタッチされていたキャラクターデータをそのまま読み込む。
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
