using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Player : MonoBehaviour
{
    /// -------------------------------------------------------------------------
    /// Player.cs
    /// 手札から場札に出した後のプレイヤーの動きを主に制御するクラス
    /// 新規キャラクターを追加した場合、CheckCharacterID関数に記載を追加する。
    /// -------------------------------------------------------------------------

    [SerializeField] private HPGauge hpGauge;
    [SerializeField] private SPGauge spGauge;
    [SerializeField] private Player enemyPlayer;
    [SerializeField] private Deck deck;
    [SerializeField] private PlayerHandController playerHandController;

    [SerializeField] public CharacterData characterData;
    [SerializeField] public CharacterAction characterAction;

    [SerializeField] public GameController gameController;
    [SerializeField] private SoundManager soundManager;

    //画面上部に表示されるキャラクターの顔画像関連
    [SerializeField] public GameObject faceObject;
    [SerializeField] private Image faceImage;

    //プレイヤーのステータス関連
    public int nowHp;
    public int nowSpPoint = 0;
    private const int PLUS_SPPOINT = 1;

    //攻撃ダメージの計算に使用する変数
    public int combo = 0;
    public float attackDamage;

    
    private const float DURATION = 0.5f;

    public bool canDoSpecial;


    public IEnumerator ReadyGame()
    {
        yield return (playerHandController.ReadyGame(PutCardAction,PutCardMiss));
        yield return StartCoroutine(characterAction.Ready(characterAction.characterSkill, characterData,RecoverHp, PlusSpGauge));
        CheckCharacterID();
        hpGauge.SetGauge(1f);
        spGauge.SetGauge(0f);
        yield return null;
    }

    /// <summary>
    /// 手札から場札を置いた際に呼び出される処理。属性カードか判定し、攻撃処理を行う。
    /// </summary>
    public void PutCardAction(int handNumber)
    {
        bool isElementCard = playerHandController.CheckElement(handNumber,characterData.elementMark);
        if (isElementCard == true) {
            PlusSpGauge(1);
            characterAction.ElementAction();
        } 
        AttackEnemy();
    }

    /// <summary>
    /// お手付きした際に呼び出される処理。現在のコンボを0に戻す
    /// </summary>
    public void PutCardMiss()
    {
        characterAction.ResetCombo();
        CheckTriggerSkill();
    }


    /// <summary>
    /// 攻撃を与える処理。コンボが成立する猶予時間は基本は5秒。
    /// </summary>
    public void AttackEnemy()
    {
        characterAction.NormalAttack();
    }

    public void TakesDamage(int damage)
    {
        nowHp = nowHp - damage;
        hpGauge.TakeDamage(damage, characterData.maxHp);
        CheckTriggerSkill();
    }

    /// <summary>
    /// HPを回復する処理。HPの上限値を超えて回復しないように制御。
    /// </summary>
    public void RecoverHp(int recoverHp)
    {
        if(nowHp + recoverHp > characterData.maxHp)
        {
            recoverHp = characterData.maxHp - nowHp;
        }

        nowHp = nowHp + recoverHp;
        hpGauge.TakeDamage(-recoverHp, characterData.maxHp);
        soundManager.PlayRecover();
        CheckTriggerSkill();
    }

    /// <summary>
    /// SPゲージを増加させる処理。SPポイントの上限値を超えてチャージされないように制御。
    /// </summary>
    public void PlusSpGauge(int getSpPoint)
    {
        soundManager.PlaySPPointGet();
        nowSpPoint += PLUS_SPPOINT;
       
        if (nowSpPoint > characterData.maxSpPoint)
        {
            nowSpPoint = characterData.maxSpPoint;
        }
        float plusSpPointRate = ((float)nowSpPoint / characterData.maxSpPoint);
        spGauge.PlusSpGauge(plusSpPointRate);
        CheckTriggerSkill();
        JudgeCanDoSp();
    }

    /// <summary>
    /// SPゲージがMAXになった時の処理。CanDoSpecialはDeck.csがクリック可能か確認するのに使用。
    /// </summary>
    private  void JudgeCanDoSp()
    {     
        if (nowSpPoint >= characterData.maxSpPoint)
        {
            soundManager.PlaySPGaugeMax();
            canDoSpecial = true;
            CheckTriggerSkill();
            deck.AnimateDeckFlash();
        }
        else
        {
            canDoSpecial = false;
            deck.StopAnimate();
        }

    }

    /// <summary>
    /// プレイヤーの操作を中断させ、プレイヤーのS.P能力を判定した後、固有能力を発動する。
    /// </summary>
    public IEnumerator DoSpecial()
    {
        gameController.PauseGamePlaying();
        soundManager.PlayDoSP();
        yield return StartCoroutine(characterAction.Special());
        
        nowSpPoint = 0;
        JudgeCanDoSp();
        spGauge.PlusSpGauge(0f);
        deck.StopAnimate();
        gameController.ReStartGamePlaying();
        CheckTriggerSkill();
        yield return null;
    }

    public void CheckTriggerSkill()
    {
        StartCoroutine(characterAction.CheckTrigger());
    }

    /// <summary>
    /// キャラクターIDを判別し顔画像・スキル内容などを決定する。
    /// キャラクター追加の際にはここに記載を追加する。
    /// </summary>
    private void CheckCharacterID()
    {
        faceImage = faceObject.GetComponent<Image>();
        faceImage.sprite = characterData.faceImage;
        nowHp = characterData.maxHp;
        switch (characterData.characterId)
        {
            case 1:
                characterAction.characterSkill = new ArgyleSkill();
                break;
            case 2:
                characterAction.characterSkill = new KokoroSkill();
                break;
            case 3:
                characterAction.characterSkill = new YotsubaSkill();
                break;
            case 4:
                characterAction.characterSkill = new GiantSkill();
                break;
            default:
                Debug.Log(@$"スペシャルIDにエラーがあります");
                break;
        }
    }
    

    /// <summary>
    /// エネミーの自動行動用の処理。EnemyUIから呼び出される。
    /// </summary>
    public IEnumerator DoEnemyAction()
    {
        yield return StartCoroutine(playerHandController.PutCardOfEnemy());
        if (canDoSpecial == true)
        {
            yield return StartCoroutine(DoSpecial());
        }
    }

}
