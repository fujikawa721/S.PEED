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

    //�L�����N�^�[�̊�摜�ǂݍ���
    [SerializeField] public Sprite faceArgyle;
    [SerializeField] public Sprite faceKokoro;
    [SerializeField] public GameObject faceObject;
    [SerializeField] private Image faceImage;

    AudioSource audioSource;
    public AudioClip spgaugeMax;

    //�v���C���[�̃X�e�[�^�X�֘A
    public int nowHp = 100;
    public int nowSpPoint = 0;
    private const int PLUS_SPPOINT = 1;


    //�U���_���[�W�̌v�Z�Ɏg�p����ϐ�
    public int combo = 0;
    private float damageRatio = 1.0f;//�U���{��
    private int comboInterval = 5; //�R���{����������P�\�b��
    private float comboDamageRatio = 0.05f;//�R���{�_���[�W�{��

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
    /// �U����^���鏈���B�R���{����������P�\���Ԃ͊�{��5�b�B
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
            comboText.text = @$"{combo}�R���{!";
        }

        enemyPlayer.TakesDamage((int)attackDamage);
    }


    public void TakesDamage(int damage)
    {
        nowHp = nowHp - damage;
        hpGauge.TakeDamage(damage, characterData.max_hp);
    }

    /// <summary>
    /// HP���񕜂��鏈���BHP�̏���l�𒴂��ĉ񕜂��Ȃ��悤�ɐ���B
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
    /// SP�Q�[�W�𑝉������鏈���BSP�|�C���g�̏���l�𒴂��ă`���[�W����Ȃ��悤�ɐ���B
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
    /// SP�Q�[�W��MAX�ɂȂ������̏����BCanDoSpecial��Deck.cs���N���b�N�\���m�F����̂Ɏg�p�B
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
    /// �v���C���[��S.P�\�͂𔻒肵����ASP�\�͂𔭓�����B
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
                Debug.Log(@$"�X�y�V����ID�ɃG���[������܂�");
                break;
        }
        yield return null;
    }

    /// <summary>
    /// S.P�w�ΑM�a�x�����HP�ɒʏ��10�{�̃_���[�W��^����B
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
    /// S.P�w�����̉́x������HP���񕜂���B
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
    /// �X�e�[�^�X�����̃v���C���[��摜�𔻒肷��B
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
                Debug.Log(@$"�X�y�V����ID�ɃG���[������܂�");
                break;
        }
    }
    /// <summary>
    /// �R���{�����𔻒肷�鏈���B�Q�[�������f����Ă���Ԃ̓J�E���^�[�𒆒f������B
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

    //�����ʉ��Đ�
    public void playSeSpgaugeMax()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = spgaugeMax;
        audioSource.Play();
    }
}
