using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private HPGauge hpGauge;
    [SerializeField] private SPGauge spGauge;
    [SerializeField] private Player enemyPlayer;
    [SerializeField] private Deck deck;
    [SerializeField] private CutInGenerator cutInGenerator;
    [SerializeField] public CharacterData characterData;
    [SerializeField] public GameController gameController;
    [SerializeField] TextMeshProUGUI comboText;

    //キャラクターの顔画像読み込み
    [SerializeField] public Sprite faceArgyle;
    [SerializeField] public Sprite faceKokoro;
    [SerializeField] public GameObject faceObject;
    [SerializeField] private Image faceImage;

    //SP関連の効果音の読み込み
    AudioSource audioSource;
    public AudioClip spgaugeMax;
    public AudioClip doSp;
    public AudioClip sword;
    public AudioClip recoverHp;

    //プレイヤーのステータス関連
    public int nowHp = 100;
    public int nowSpPoint = 0;
    private const int PLUS_SPPOINT = 1;


    //攻撃ダメージの計算に使用する変数
    public int combo = 0;
    private float damageRatio = 1.0f;//攻撃倍率
    
    //【3秒以内】に攻撃を行うとコンボが成立する。※例外のキャラクターも存在する。
    private int comboTime = 3;
    private int timerCount;
    private float comboDamageRatio = 0.05f;//コンボダメージ倍率

    //コンボタイマー関連の読み込み
    [SerializeField] private  Image comboTimerImage;
    private const float DURATION = 0.5f;

    public bool canDoSpecial;


    public IEnumerator ReadyGame()
    {
        cutInGenerator.ReadyGame();
        cutInGenerator.CheckCutInImg(characterData.character_id);
        audioSource = GetComponent<AudioSource>();
        CheckPlayerface();
        hpGauge.SetGauge(1f);
        spGauge.SetGauge(0f);
        yield return null;
    }

    /// <summary>
    /// 攻撃を与える処理。コンボが成立する猶予時間は基本は5秒。
    /// </summary>
    public void AttackEnemy()
    {
        timerCount = comboTime;
        combo++;
        damageRatio = 1.0f + comboDamageRatio * (combo - 1);
        float attackDamage = characterData.base_damage * damageRatio;

        if(combo == 1)
        {
            StartCoroutine(ComboCounter());
        }

        if (combo > 1)
        {
            comboText.text = @$"{combo}コンボ!";
        }

        enemyPlayer.TakesDamage((int)attackDamage);
    }


    public void TakesDamage(int damage)
    {
        nowHp = nowHp - damage;
        hpGauge.TakeDamage(damage, characterData.max_hp);
    }

    /// <summary>
    /// HPを回復する処理。HPの上限値を超えて回復しないように制御。
    /// </summary>
    public void RecoverHp(int recoverHp)
    {
        if(nowHp + recoverHp > characterData.max_hp)
        {
            recoverHp = characterData.max_hp - nowHp;
        }

        nowHp = nowHp + recoverHp;
        hpGauge.TakeDamage(-recoverHp, characterData.max_hp);
    }

    /// <summary>
    /// SPゲージを増加させる処理。SPポイントの上限値を超えてチャージされないように制御。
    /// </summary>
    public void PlusSpGauge()
    {
        nowSpPoint += PLUS_SPPOINT;
        
        if (nowSpPoint > characterData.max_sp_point)
        {
            nowSpPoint = characterData.max_sp_point;
        }

        float plusSpPointRate = ((float)nowSpPoint / characterData.max_sp_point);
        spGauge.PlusSpGauge(plusSpPointRate);
        JudgeCanDoSp();
    }

    /// <summary>
    /// SPゲージがMAXになった時の処理。CanDoSpecialをDeck.csがクリック可能か確認するのに使用。
    /// </summary>
    private  void JudgeCanDoSp()
    {     
        if (nowSpPoint >= characterData.max_sp_point)
        {
            playSeSpgaugeMax();
            canDoSpecial = true;
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
        PlaySeDoSp();
        switch (characterData.character_id)
        {
            case 1:
                yield return StartCoroutine(specialKasenzan());
                audioSource.clip = sword;
                break;
            case 2:
                yield return StartCoroutine(specialIyashiNoUta());
                audioSource.clip = recoverHp;
                break;
            default:
                Debug.Log(@$"スペシャルIDにエラーがあります");
                break;
        }
        deck.StopAnimate();
        audioSource.Play();
        gameController.ReStartGamePlaying();
        yield return null;
    }

    
    /// <summary>
    /// S.P『火閃斬』相手のHPに通常の10倍のダメージを与える。
    /// </summary>
    private IEnumerator specialKasenzan()
    {
        yield return StartCoroutine(cutInGenerator.AnimateSpecialCutIn());
        enemyPlayer.TakesDamage(characterData.base_damage * 10);
        nowSpPoint = 0;
        JudgeCanDoSp();
        spGauge.PlusSpGauge(0f);
        yield return null;
    }

    /// <summary>
    /// S.P『癒しの歌』自分のHPを大回復する。
    /// </summary>
    private IEnumerator specialIyashiNoUta()
    {
        yield return StartCoroutine(cutInGenerator.AnimateSpecialCutIn());
        nowSpPoint = 0;
        RecoverHp(50);
        JudgeCanDoSp();
        spGauge.PlusSpGauge(0f);
        yield return null;
    }

    /// <summary>
    /// ステータス部分のプレイヤー顔画像を判定する。
    /// </summary>
    private void CheckPlayerface()
    {
        faceImage = faceObject.GetComponent<Image>(); ;
        switch (characterData.character_id)
        {
            case 1:
                faceImage.sprite = faceArgyle;
                break;
            case 2:
                faceImage.sprite = faceKokoro;
                break;
            default:
                Debug.Log(@$"スペシャルIDにエラーがあります");
                break;
        }
    }
    /// <summary>
    /// コンボ成立を判定する処理。ゲームが中断されている間はカウンターを中断させる。
    /// </summary>
    private IEnumerator ComboCounter()
    {
        while(timerCount > 0)
        {
            while (gameController.canPlayNow == false)
            {
                yield return new WaitForSeconds(0.5f);
                if (gameController.endGameFlg == true)
                {
                    break;
                }
            }
            yield return new WaitForSeconds(1.0f);
            timerCount -= 1;
            float timerFill =1 - (float)timerCount / comboTime;
            comboTimerImage.DOFillAmount(timerFill, DURATION);

            if (timerCount == 0) {
                combo = 0;
                comboText.text = @$"";
            }

        }
    }

    //★効果音再生
    public void playSeSpgaugeMax()
    {
        
        audioSource.clip = spgaugeMax;
        audioSource.Play();
    }

    public void PlaySeDoSp()
    {
        audioSource.clip = doSp;
        audioSource.Play();
    }

}
