using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterAction : MonoBehaviour
{
    [SerializeField] private CutInGenerator cutInGenerator;
    [SerializeField] private NormalCutInGenerator normalCutInGenerator;
    [SerializeField] public ComboManager comboManager;
    [SerializeField] public Player enemyPlayer;

    public CharacterSkill characterSkill;
    public Player player;

    // 回復スキルを使用した際のPlayer.csへのコールバック処理
    public delegate void RecoverHp(int recoverHp);
    public RecoverHp recoverHpCallBack;

    // 回復スキルを使用した際のPlayer.csへのコールバック処理
    public delegate void GetSpPoint(int getSpPoint);
    public GetSpPoint getSpPointCallBack;

    //攻撃処理で使う変数
    public float attackDamage;
    public int combo;
    private float damageRatio = 1.0f;//基本攻撃倍率
    private float comboDamageRatio = 0.05f;//コンボダメージ倍率

    //スキルの使用回数に関する変数
    public bool canPassivSkill;


    public IEnumerator Ready(CharacterSkill playerSkill, CharacterData characterData,RecoverHp recoverHp, GetSpPoint getSpPoint)
    {
        characterSkill = playerSkill;
        recoverHpCallBack = recoverHp;
        getSpPointCallBack = getSpPoint;
        attackDamage = player.characterData.baseDamage;
        cutInGenerator.ReadyGame(characterData);
        normalCutInGenerator.ReadyGame(characterData.normalCutInImage);

        canPassivSkill =true;
        yield return null;
    }


    public void NormalAttack()
    {
        StartCoroutine(characterSkill.AttackTriggerSkill(player,enemyPlayer,this));
        combo = comboManager.AddCombo();
        damageRatio = 1.0f + comboDamageRatio * (combo - 1);
        StartCoroutine(characterSkill.ComboTriggerSkill(player, enemyPlayer, this));
        attackDamage = player.characterData.baseDamage * damageRatio;
        enemyPlayer.TakesDamage((int)attackDamage);
        Debug.Log($"{combo}コンボ、{attackDamage}ダメージ");
        Debug.Log($"{canPassivSkill}パッシブスキル");
    }

    public void ElementAction()
    {
        StartCoroutine(characterSkill.ElementTriggerSkill(player, enemyPlayer, this));
    }

    public IEnumerator Special()
    {
        yield return StartCoroutine(cutInGenerator.AnimateSpecialCutIn());
        StartCoroutine(characterSkill.SpecialSkill(player, enemyPlayer, this));
        //audioSource.clip = ○○;
    }

    public void DamageAdditional(int damage)
    {
        enemyPlayer.TakesDamage(damage);
    }

    public void activeNormalCutIn(string eventName,string eventEffect)
    {
        StartCoroutine(normalCutInGenerator.AnimateNormalCutIn(eventName, eventEffect));
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
        public IEnumerator ComboTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction);
        public IEnumerator ElementTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction);
        public IEnumerator StatusTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction);
        public IEnumerator SpecialSkill(Player player, Player enemyPlayer, CharacterAction characterAction);
}


public class ArgyleSkill :  CharacterSkill
{
    //単発スキル
    public IEnumerator AttackTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        yield return null;
    }

    public IEnumerator ComboTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction){
            yield return null;
    }

    public IEnumerator ElementTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction){
        int attackDamage = (int)characterAction.attackDamage / 2;
        if (characterAction.canPassivSkill == true)
        {
            string eventName = @$"ELEMENT SKILL";
            string eventEffect = @$"相手に追加ダメージ";
            characterAction.DamageAdditional(attackDamage);
            characterAction.activeNormalCutIn(eventName, eventEffect);
        }
        yield return null;
    }

    public IEnumerator StatusTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
         yield return null;
    }

    public IEnumerator SpecialSkill(Player player, Player enemyPlayer, CharacterAction characterAction) {
        enemyPlayer.TakesDamage(player.characterData.baseDamage * 10);
        yield return null;
    }
}

public class KokoroSkill : CharacterSkill
{
        public IEnumerator AttackTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
        {
            yield return null;
        }

        public IEnumerator ComboTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
        {
            yield return null;
        }
    public IEnumerator ElementTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        string eventName = @$"ELEMENT SKILL";
        string eventEffect = @$"自分のHPを少回復する";
        characterAction.RecoverHpAction(1);
        characterAction.activeNormalCutIn(eventName, eventEffect);
        yield return null;
     }
        public IEnumerator StatusTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction) {
            yield return null;
        }
    /// <summary>
    /// S.P『癒しの歌』自分のHPを大回復する。
    /// </summary>
    public IEnumerator SpecialSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        characterAction.RecoverHpAction(30);
        yield return null;
    }        
    
}



