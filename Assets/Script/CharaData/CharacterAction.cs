using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterAction : MonoBehaviour
{
    [SerializeField] public CutInGenerator cutInGenerator;
    [SerializeField] public NormalCutInGenerator normalCutInGenerator;
    [SerializeField] public ComboManager comboManager;
    [SerializeField] public Player enemyPlayer;
    [SerializeField] public SoundManager soundManager;


    public CharacterSkill characterSkill;
    public Player player;

    // 回復スキルを使用した際のPlayer.csへのコールバック処理
    public delegate void RecoverHp(int recoverHp);
    public RecoverHp recoverHpCallBack;

    // 回復スキルを使用した際のPlayer.csへのコールバック処理
    public delegate void GetSpPoint(int getSpPoint);
    public GetSpPoint getSpPointCallBack;

    //攻撃処理で使う変数
    public float attackDamage = 0;
    public int combo = 0;
    public float baseDamageRatio = 1.0f;//基本攻撃倍率
    public float comboDamageRatio = 0.05f;//コンボダメージ倍率

    //スキルの使用回数に関する変数
    public bool canPassivSkill;

    public const int RECOVERY_HP_SMALL = 50;//「小回復」で回復する回復量
    public const int RECOVERY_HP_MEDIUM = 100;//「中回復」で回復する回復量
    public const int RECOVERY_HP_BIG = 200;//「大回復」で回復する回復量


    public IEnumerator Ready(CharacterSkill playerSkill, CharacterData characterData,RecoverHp recoverHp, GetSpPoint getSpPoint)
    {
        characterSkill = playerSkill;
        recoverHpCallBack = recoverHp;
        getSpPointCallBack = getSpPoint;
        attackDamage = player.characterData.baseDamage;
        cutInGenerator.ReadyGame(characterData);
        normalCutInGenerator.ReadyGame(characterData.normalCutInImage, characterData.faceImage);

        canPassivSkill =true;
        yield return null;
    }


    public void NormalAttack()
    {
        StartCoroutine(characterSkill.AttackTriggerSkill(player,enemyPlayer,this));
        combo = comboManager.AddCombo();
        var damageRatio = baseDamageRatio + comboDamageRatio * (combo - 1);
        Debug.Log($"{damageRatio}");
        attackDamage = player.characterData.baseDamage * damageRatio;
        enemyPlayer.TakesDamage((int)attackDamage);
        Debug.Log($"{combo}コンボ、{attackDamage}ダメージ");
        Debug.Log($"{canPassivSkill}パッシブスキル");
    }

    /// <summary>
    /// 属性記号カードを場札に置いたときに発動される効果
    /// </summary>
    public void ElementAction()
    {
        StartCoroutine(characterSkill.ElementTriggerSkill(player, enemyPlayer, this));
        switch (player.characterData.elementMark)
        {
            case "D":
                DamageAdditional((int)attackDamage);
                ActiveEventCutIn("ELEMENT SKILL", "相手に追加ダメージ");

                break;
            case "H":
                RecoverHpAction(RECOVERY_HP_SMALL);
                ActiveEventCutIn("ELEMENT SKILL", "HPを小回復");
                break;
            case "C":
                for (int i = 0; i < 3; i++)
                {
                    combo = comboManager.AddCombo();
                }
                ActiveEventCutIn("ELEMENT SKILL", "現在のコンボを+3コンボ");
                break;
            case "S":
                baseDamageRatio = baseDamageRatio * 1.2f;
                ActiveEventCutIn("ELEMENT SKILL", "攻撃力が1.2倍上昇");
                break;
        }

        
    }

    public IEnumerator Special()
    {
        yield return StartCoroutine(cutInGenerator.AnimateSpecialCutIn());
        StartCoroutine(characterSkill.SpecialSkill(player, enemyPlayer, this));
    }

    public IEnumerator CheckTrigger()
    {
        yield return StartCoroutine(characterSkill.TriggerSkill(player, enemyPlayer, this));

    }

    public void ResetCombo()
    {
        comboManager.ResetCombo();
        soundManager.PlayBack();
    }

    public void DamageAdditional(int damage)
    {
        enemyPlayer.TakesDamage(damage);
    }

    public void ActiveEventCutIn(string eventName,string eventEffect)
    {
        StartCoroutine(normalCutInGenerator.ActiveEventCutIn(eventName, eventEffect));
        normalCutInGenerator.ActiveNormalCutIn();
    }

    public void RecoverHpAction(int recoverHp)
    {
        recoverHpCallBack(recoverHp);
    }

    public void GetSpPointAction(int getSpPoint)
    {
        getSpPointCallBack(getSpPoint);
    }


}
public interface CharacterSkill
{
        public IEnumerator AttackTriggerSkill(Player player,Player enemyPlayer,CharacterAction characterAction);
        public IEnumerator ElementTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction);
        public IEnumerator TriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction);
        public IEnumerator SpecialSkill(Player player, Player enemyPlayer, CharacterAction characterAction);
}

//キャラクターID1：アーガイルのスキル
public class ArgyleSkill :  CharacterSkill
{
    public IEnumerator AttackTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        yield return null;
    }


    public IEnumerator ElementTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction){
        yield return null;
    }

    //【条件】HPが30%以下　【効果】攻撃力が2倍に上昇
    public IEnumerator TriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        if(characterAction.canPassivSkill == true)
        {
            if (player.nowHp < player.characterData.maxHp * 0.3)
            {
                characterAction.normalCutInGenerator.ActiveNormalCutIn();
                characterAction.baseDamageRatio = characterAction.baseDamageRatio * 2.0f;
                characterAction.ActiveEventCutIn("TRIGGER SKILL", "攻撃力が2倍上昇");
                characterAction.canPassivSkill = false;
            }
        }
         yield return null;
    }

    public IEnumerator SpecialSkill(Player player, Player enemyPlayer, CharacterAction characterAction) {
        enemyPlayer.TakesDamage(player.characterData.baseDamage * 10);
        characterAction.soundManager.PlaySword();
        yield return null;
    }
}

public class KokoroSkill : CharacterSkill
{
        public IEnumerator AttackTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
        {
            yield return null;
        }

    public IEnumerator ElementTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        yield return null;
     }

    //【条件】仕切り直し時　【効果】HPを小回復する。
    public IEnumerator TriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction) {
        if (characterAction.canPassivSkill == true)
        {
            if (player.nowHp < player.characterData.maxHp * 0.3)
            {
                characterAction.normalCutInGenerator.ActiveNormalCutIn();
                characterAction.baseDamageRatio = characterAction.baseDamageRatio * 2.0f;
                characterAction.ActiveEventCutIn("TRIGGER SKILL", "攻撃力が2倍上昇");
                characterAction.canPassivSkill = false;
            }
        }
        yield return null;
    }

    /// S.P『癒しの歌』自分のHPを【200】回復する。
    public IEnumerator SpecialSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        characterAction.RecoverHpAction(200);
        characterAction.soundManager.PlayRecover();
        yield return null;
    }        
    
}

//キャラクターID3：ヨツバのスキル
public class YotsubaSkill : CharacterSkill
{
    public IEnumerator AttackTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        yield return null;
    }

    public IEnumerator ElementTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        yield return null;
    }

    //【条件】コンボが20以上の時　【効果】コンボによるダメージ倍率を上昇させる。
    public IEnumerator TriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        if (characterAction.comboManager.combo >= 20)
        {
            characterAction.normalCutInGenerator.ActiveNormalCutIn();
            characterAction.comboDamageRatio = 0.1f;
            characterAction.ActiveEventCutIn("TRIGGER SKILL", "コンボダメージ倍率が上昇");
        }
        else
        {
            characterAction.comboDamageRatio = 0.05f;
        }
        yield return null;
    }

    public IEnumerator SpecialSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {

        enemyPlayer.TakesDamage(player.characterData.baseDamage * characterAction.combo);
        Debug.Log($"{player.characterData.baseDamage * characterAction.combo}ダメージ");
        characterAction.soundManager.PlayBigHit();
        yield return null;
    }
}

//キャラクターID4：ジャイアントのスキル
public class GiantSkill : CharacterSkill
{
    public IEnumerator AttackTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        yield return null;
    }

    public IEnumerator ElementTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        yield return null;
    }

    public IEnumerator TriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        if (characterAction.canPassivSkill == true)
        {
            if (characterAction.baseDamageRatio > 4.0f)
            {
                characterAction.normalCutInGenerator.ActiveNormalCutIn();
                characterAction.RecoverHpAction(200);
                characterAction.ActiveEventCutIn("TRIGGER SKILL", "HPが大回復");
                characterAction.canPassivSkill = false;
            }
        }
        yield return null;
    }

    //攻撃力の倍率を2.0倍にする
    public IEnumerator SpecialSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        characterAction.baseDamageRatio = characterAction.baseDamageRatio * 2.0f;
        characterAction.soundManager.PlayPowerUp();
        yield return null;
    }
}


