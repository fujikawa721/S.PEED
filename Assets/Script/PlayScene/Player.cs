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
    /// ��D�����D�ɏo������̃v���C���[�̓�������ɐ��䂷��N���X
    /// �V�K�L�����N�^�[��ǉ������ꍇ�ACheckCharacterID�֐��ɋL�ڂ�ǉ�����B
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

    //��ʏ㕔�ɕ\�������L�����N�^�[�̊�摜�֘A
    [SerializeField] public GameObject faceObject;
    [SerializeField] private Image faceImage;

    //�v���C���[�̃X�e�[�^�X�֘A
    public int nowHp;
    public int nowSpPoint = 0;
    private const int PLUS_SPPOINT = 1;

    //�U���_���[�W�̌v�Z�Ɏg�p����ϐ�
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
    /// ��D�����D��u�����ۂɌĂяo����鏈���B�����J�[�h�����肵�A�U���������s���B
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
    /// ����t�������ۂɌĂяo����鏈���B���݂̃R���{��0�ɖ߂�
    /// </summary>
    public void PutCardMiss()
    {
        characterAction.ResetCombo();
        CheckTriggerSkill();
    }


    /// <summary>
    /// �U����^���鏈���B�R���{����������P�\���Ԃ͊�{��5�b�B
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
    /// HP���񕜂��鏈���BHP�̏���l�𒴂��ĉ񕜂��Ȃ��悤�ɐ���B
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
    /// SP�Q�[�W�𑝉������鏈���BSP�|�C���g�̏���l�𒴂��ă`���[�W����Ȃ��悤�ɐ���B
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
    /// SP�Q�[�W��MAX�ɂȂ������̏����BCanDoSpecial��Deck.cs���N���b�N�\���m�F����̂Ɏg�p�B
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
    /// �v���C���[�̑���𒆒f�����A�v���C���[��S.P�\�͂𔻒肵����A�ŗL�\�͂𔭓�����B
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
    /// �L�����N�^�[ID�𔻕ʂ���摜�E�X�L�����e�Ȃǂ����肷��B
    /// �L�����N�^�[�ǉ��̍ۂɂ͂����ɋL�ڂ�ǉ�����B
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
                Debug.Log(@$"�X�y�V����ID�ɃG���[������܂�");
                break;
        }
    }
    

    /// <summary>
    /// �G�l�~�[�̎����s���p�̏����BEnemyUI����Ăяo�����B
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
