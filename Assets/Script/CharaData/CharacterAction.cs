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

    // �񕜃X�L�����g�p�����ۂ�Player.cs�ւ̃R�[���o�b�N����
    public delegate void RecoverHp(int recoverHp);
    public RecoverHp recoverHpCallBack;

    // �񕜃X�L�����g�p�����ۂ�Player.cs�ւ̃R�[���o�b�N����
    public delegate void GetSpPoint(int getSpPoint);
    public GetSpPoint getSpPointCallBack;

    //�U�������Ŏg���ϐ�
    public float attackDamage;
    public int combo;
    private float damageRatio = 1.0f;//��{�U���{��
    private float comboDamageRatio = 0.05f;//�R���{�_���[�W�{��

    //�X�L���̎g�p�񐔂Ɋւ���ϐ�
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
        Debug.Log($"{combo}�R���{�A{attackDamage}�_���[�W");
        Debug.Log($"{canPassivSkill}�p�b�V�u�X�L��");
    }

    public void ElementAction()
    {
        StartCoroutine(characterSkill.ElementTriggerSkill(player, enemyPlayer, this));
    }

    public IEnumerator Special()
    {
        yield return StartCoroutine(cutInGenerator.AnimateSpecialCutIn());
        StartCoroutine(characterSkill.SpecialSkill(player, enemyPlayer, this));
        //audioSource.clip = ����;
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
    //�P���X�L��
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
            string eventEffect = @$"����ɒǉ��_���[�W";
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
        string eventEffect = @$"������HP�����񕜂���";
        characterAction.RecoverHpAction(1);
        characterAction.activeNormalCutIn(eventName, eventEffect);
        yield return null;
     }
        public IEnumerator StatusTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction) {
            yield return null;
        }
    /// <summary>
    /// S.P�w�����̉́x������HP���񕜂���B
    /// </summary>
    public IEnumerator SpecialSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        characterAction.RecoverHpAction(30);
        yield return null;
    }        
    
}



