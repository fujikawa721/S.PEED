using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] Deck deck;
    [SerializeField] Deck enemyDeck;
    [SerializeField] EnemyUI enemyUI;
    [SerializeField] Player player;
    [SerializeField] Player enemyPlayer;
    [SerializeField] GameObject noactionCurtain;
    [SerializeField] GameObject gameMessage;
    [SerializeField] ResultDisplay resultDisplay;
    [SerializeField] private LoadingManager loadingManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] public CountdownTimer countdownTimer;

    [SerializeField] PlayerHandController playerHandController;
    [SerializeField] PlayerHandController enemyHandController;

    [SerializeField] TextMeshProUGUI playerActionText;
    [SerializeField] TextMeshProUGUI gameMessageText;


    public bool canPlayerAction;
    public bool canEnemyAction;
    public bool canPlayNow;
    public bool endGameFlg;

    private const int GAME_TIMELIMIT = 99;//�Q�[���̐�������
    private const float READY_TIME = 3.0f;//�����J�n����܂ł̑҂�����
    private const float MESSAGE_DISPTIME_MIN = 0.5f;//���b�Z�[�W���\�������ŏ�����


    void Start()
    {
        StartCoroutine(StartGame());
    }

    //0.5�b���ƂɎ��s����B
    void FixedUpdate()
    {
        StartCoroutine(JudgePlaying());
    }

    /// <summary>
    /// �Q�[���J�n���Ɏ��s����鏈���B�R�D�����A��D�����A�Q�[���J�n���x������B
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartGame()
    {
        soundManager.PlayBgmBattle();
        countdownTimer.SetTimer(GAME_TIMELIMIT, LimitTimer);
        yield return StartCoroutine(LoadGame());
        yield return StartCoroutine(loadingManager.EndLoad());

        gameMessage.SetActive(true);
        gameMessageText.text = @$"READY...";
        
        //��D��������
        StartCoroutine(ReadyPlayer());
        StartCoroutine(ReadyEnemy());
        yield return new WaitForSeconds(READY_TIME);

        gameMessageText.text = @$"START!!";
        yield return new WaitForSeconds(MESSAGE_DISPTIME_MIN);

        //��D��������
        StartCoroutine(deck.MakeField(0));
        StartCoroutine(enemyDeck.MakeField(1));
        soundManager.PlaySpeed();
        canPlayNow = true;
        StartCoroutine(countdownTimer.CountDown());
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
    /// �v���C�\���Ɉȉ��𔻒肷��B�u�Q�[���I�������v�u�v���C���[�̍s���ہv�uSP�����\�ہv�u�d�؂蒼�����s�ہv
    /// </summary>
    private IEnumerator JudgePlaying()
    {
        Debug.Log("�W���b�W����");
        if (canPlayNow == true)
        {
            JudgeBothPlayerHp();
            yield return StartCoroutine(CheckPlayerCanAction());

            if (canPlayerAction == false)
            {
                playerActionText.text = @$"�o����J�[�h������܂���B";
            }
            else
            {
                playerActionText.text = @$""; ;
            }

            if (player.canDoSpecial == true)
            {
                playerActionText.text = @$"S.P�Q�[�W�����^���ł��B�R�D�^�b�`��S.P����";
            }

            if (canPlayerAction == false && canEnemyAction == false)
            {
                Debug.Log("�X�s�[�h����");
                canPlayNow = false;
                yield return StartCoroutine(Speed());
            }
        }
    }

    private IEnumerator CheckPlayerCanAction()
    {
        canPlayerAction = playerHandController.CheckCanAction();
        canEnemyAction = enemyHandController.CheckCanAction();
        yield return null;
    }

    /// <summary>
    /// ���v���C���[����D�ɃJ�[�h��u���Ȃ��ꍇ�A���v���C���[�̎R�D�����D��1�����J�[�h��u���B
    /// �Q�[�����ł́w�d�؂蒼���y�X�s�[�h�z�ƌĂԁB�x
    /// </summary>
    public IEnumerator Speed()
    {
        Debug.Log("�X�s�[�h");
        yield return new WaitForSeconds(1.0f);
        soundManager.PlayWhistle();
        PauseGamePlaying();
        gameMessage.SetActive(true);
        gameMessageText.text = @$"�d�؂蒼��";
        playerActionText.text = @$"��D�����Z�b�g���܂��B";
        yield return new WaitForSeconds(1.0f);

        gameMessageText.text = @$"�X�s�[�@ ";
        soundManager.PlayVoiceSPEE();
        yield return new WaitForSeconds(1.0f);

        gameMessageText.text = @$"�X�s�[�h!";
        soundManager.PlayVoiceDO();
        yield return new WaitForSeconds(MESSAGE_DISPTIME_MIN);
        
        soundManager.PlaySpeed();
        StartCoroutine(deck.MakeField(0));
        StartCoroutine(enemyDeck.MakeField(1));
        gameMessage.SetActive(false);
        gameMessageText.text = @$"";
        ReStartGamePlaying();
        player.CheckSpeedTriggerSkill();
        enemyPlayer.CheckSpeedTriggerSkill();
        yield return null;
    }

    /// <summary>
    /// �ǂ��炩�̃v���C���[��HP��0�ȉ��ɂȂ�ƃQ�[�����I������B
    /// </summary>
    public void JudgeBothPlayerHp()
    {
        if(player.nowHp <= 0 || enemyPlayer.nowHp <= 0)
        {
            EndGame();
            if (player.nowHp <= 0)
            {
                string winner = "2P PLAYER";
                StartCoroutine(resultDisplay.Ready(enemyPlayer,winner));
            }else if (enemyPlayer.nowHp <= 0)
            {
                string winner = "1P PLAYER";
                StartCoroutine(resultDisplay.Ready(player,winner));
            }
            
        }
    }

    //�������Ԃ��߂����Ƃ��̏����BHP�������ق������҂ɂȂ�B
    public void LimitTimer()
    {
        EndGame();
        if (player.nowHp >= enemyPlayer.nowHp)
        {
            string winner = "1P PLAYER";
            StartCoroutine(resultDisplay.Ready(player, winner)); 
        }
        else if (player.nowHp < enemyPlayer.nowHp)
        {
            string winner = "2P PLAYER";
            StartCoroutine(resultDisplay.Ready(enemyPlayer, winner));
        }
    }

    private IEnumerator LoadGame()
    {
        yield return StartCoroutine(PassCharacterData());
        yield return StartCoroutine(player.ReadyGame());
        yield return StartCoroutine(enemyPlayer.ReadyGame());
    }

    /// <summary>
    /// �Q�[���I�����Ƀ��U���g��ʂ�\�������鏈���B
    /// </summary>
    private void EndGame()
    {
        canPlayNow = false;
        endGameFlg = true;
        noactionCurtain.SetActive(true);
        gameMessage.SetActive(true);
        gameMessageText.text = @$"GAME SET!!";
        soundManager.PlayWhistle();
    }

    /// <summary>
    /// �I����ʂőI�������L�����N�^�[���v���C���[�I�u�W�F�N�g�ɓn���B�����Ƀf�[�^�������Ă��Ȃ��ꍇ�́A
    /// �\�߃A�^�b�`����Ă����L�����N�^�[�f�[�^�����̂܂ܓǂݍ��ށB
    /// </summary>
    private IEnumerator PassCharacterData()
    {
        if (SelectController.playerCharacterData != null && SelectController.enemyCharacterData != null)
        {
            player.characterData = SelectController.playerCharacterData;
            enemyPlayer.characterData = SelectController.enemyCharacterData;
        }
        yield return null;
    }

}
