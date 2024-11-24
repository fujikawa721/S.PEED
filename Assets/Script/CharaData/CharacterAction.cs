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

    // �񕜃X�L�����g�p�����ۂ�Player.cs�ւ̃R�[���o�b�N����
    public delegate void RecoverHp(int recoverHp);
    public RecoverHp recoverHpCallBack;

    // �񕜃X�L�����g�p�����ۂ�Player.cs�ւ̃R�[���o�b�N����
    public delegate void GetSpPoint(int getSpPoint);
    public GetSpPoint getSpPointCallBack;

    //�U�������Ŏg���ϐ�
    public float attackDamage = 0;
    public int combo = 0;
    public float baseDamageRatio = 1.0f;//��{�U���{��
    public float comboDamageRatio = 0.05f;//�R���{�_���[�W�{��

    //�X�L���̎g�p�񐔂Ɋւ���ϐ�
    public bool canPassivSkill;

    public const int RECOVERY_HP_SMALL = 50;//�u���񕜁v�ŉ񕜂���񕜗�
    public const int RECOVERY_HP_MEDIUM = 100;//�u���񕜁v�ŉ񕜂���񕜗�
    public const int RECOVERY_HP_BIG = 200;//�u��񕜁v�ŉ񕜂���񕜗�


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
        Debug.Log($"{combo}�R���{�A{attackDamage}�_���[�W");
        Debug.Log($"{canPassivSkill}�p�b�V�u�X�L��");
    }

    /// <summary>
    /// �����L���J�[�h����D�ɒu�����Ƃ��ɔ�����������
    /// </summary>
    public void ElementAction()
    {
        StartCoroutine(characterSkill.ElementTriggerSkill(player, enemyPlayer, this));
        switch (player.characterData.elementMark)
        {
            case "D":
                DamageAdditional((int)attackDamage);
                ActiveEventCutIn("ELEMENT SKILL", "����ɒǉ��_���[�W");

                break;
            case "H":
                RecoverHpAction(RECOVERY_HP_SMALL);
                ActiveEventCutIn("ELEMENT SKILL", "HP������");
                break;
            case "C":
                for (int i = 0; i < 3; i++)
                {
                    combo = comboManager.AddCombo();
                }
                ActiveEventCutIn("ELEMENT SKILL", "���݂̃R���{��+3�R���{");
                break;
            case "S":
                baseDamageRatio = baseDamageRatio * 1.2f;
                ActiveEventCutIn("ELEMENT SKILL", "�U���͂�1.2�{�㏸");
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

//�L�����N�^�[ID1�F�A�[�K�C���̃X�L��
public class ArgyleSkill :  CharacterSkill
{
    public IEnumerator AttackTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        yield return null;
    }


    public IEnumerator ElementTriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction){
        yield return null;
    }

    //�y�����zHP��30%�ȉ��@�y���ʁz�U���͂�2�{�ɏ㏸
    public IEnumerator TriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        if(characterAction.canPassivSkill == true)
        {
            if (player.nowHp < player.characterData.maxHp * 0.3)
            {
                characterAction.normalCutInGenerator.ActiveNormalCutIn();
                characterAction.baseDamageRatio = characterAction.baseDamageRatio * 2.0f;
                characterAction.ActiveEventCutIn("TRIGGER SKILL", "�U���͂�2�{�㏸");
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

    //�y�����z�d�؂蒼�����@�y���ʁzHP�����񕜂���B
    public IEnumerator TriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction) {
        if (characterAction.canPassivSkill == true)
        {
            if (player.nowHp < player.characterData.maxHp * 0.3)
            {
                characterAction.normalCutInGenerator.ActiveNormalCutIn();
                characterAction.baseDamageRatio = characterAction.baseDamageRatio * 2.0f;
                characterAction.ActiveEventCutIn("TRIGGER SKILL", "�U���͂�2�{�㏸");
                characterAction.canPassivSkill = false;
            }
        }
        yield return null;
    }

    /// S.P�w�����̉́x������HP���y200�z�񕜂���B
    public IEnumerator SpecialSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        characterAction.RecoverHpAction(200);
        characterAction.soundManager.PlayRecover();
        yield return null;
    }        
    
}

//�L�����N�^�[ID3�F���c�o�̃X�L��
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

    //�y�����z�R���{��20�ȏ�̎��@�y���ʁz�R���{�ɂ��_���[�W�{�����㏸������B
    public IEnumerator TriggerSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        if (characterAction.comboManager.combo >= 20)
        {
            characterAction.normalCutInGenerator.ActiveNormalCutIn();
            characterAction.comboDamageRatio = 0.1f;
            characterAction.ActiveEventCutIn("TRIGGER SKILL", "�R���{�_���[�W�{�����㏸");
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
        Debug.Log($"{player.characterData.baseDamage * characterAction.combo}�_���[�W");
        characterAction.soundManager.PlayBigHit();
        yield return null;
    }
}

//�L�����N�^�[ID4�F�W���C�A���g�̃X�L��
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
                characterAction.ActiveEventCutIn("TRIGGER SKILL", "HP�����");
                characterAction.canPassivSkill = false;
            }
        }
        yield return null;
    }

    //�U���͂̔{����2.0�{�ɂ���
    public IEnumerator SpecialSkill(Player player, Player enemyPlayer, CharacterAction characterAction)
    {
        characterAction.baseDamageRatio = characterAction.baseDamageRatio * 2.0f;
        characterAction.soundManager.PlayPowerUp();
        yield return null;
    }
}


