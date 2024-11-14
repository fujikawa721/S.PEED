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
    [SerializeField] private CutInGenerator cutInGenerator;
    [SerializeField] public CharacterData characterData;
    [SerializeField] public GameController gameController;
    [SerializeField] TextMeshProUGUI comboText;

    //キャラクターの顔画像読み込み
    [SerializeField] public Sprite faceArgyle;
    [SerializeField] public Sprite faceKokoro;
    [SerializeField] public GameObject faceObject;
    [SerializeField] private Image faceImage;

    AudioSource audioSource;
    public AudioClip spgaugeMax;

    //プレイヤーのステータス関連
    public int nowHp = 100;
    public int nowSpPoint = 0;
    private const int PLUS_SPPOINT = 1;


    //攻撃ダメージの計算に使用する変数
    public int combo = 0;
    private float damageRatio = 1.0f;//攻撃倍率
    private int comboInterval = 5; //コンボが成立する猶予秒数
    private float comboDamageRatio = 0.05f;//コンボダメージ倍率

    public bool canDoSpecial;


    public IEnumerator ReadyGame()
    {
        cutInGenerator.CheckCutInImg(characterData.character_id);
        cutInGenerator.ReadyGame();
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
        comboInterval = 5;
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
        }
        else
        {
            canDoSpecial = false;
        }

    }

    /// <summary>
    /// プレイヤーのS.P能力を判定した後、SP能力を発動する。
    /// </summary>
    public IEnumerator DoSpecial()
    {
        switch (characterData.character_id)
        {
            case 1:
                StartCoroutine(specialKasenzan());
                break;
            case 2:
                StartCoroutine(specialIyashiNoUta());
                break;
            default:
                Debug.Log(@$"スペシャルIDにエラーがあります");
                break;
        }
        yield return null;
    }

    /// <summary>
    /// S.P『火閃斬』相手のHPに通常の10倍のダメージを与える。
    /// </summary>
    private IEnumerator specialKasenzan()
    {
        StartCoroutine(cutInGenerator.AnimateSpecialCutIn());
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
        StartCoroutine(cutInGenerator.AnimateSpecialCutIn());
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
        while(comboInterval > 0)
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
            comboInterval -= 1;

            if (comboInterval == 0) {
                combo = 0;
                comboText.text = @$"";
            }

        }
    }

    //★効果音再生
    public void playSeSpgaugeMax()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = spgaugeMax;
        audioSource.Play();
    }
}
