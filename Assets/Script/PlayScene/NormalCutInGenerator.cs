using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class NormalCutInGenerator : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;

    //�C�x���g�J�b�g�C���p�̃I�u�W�F�N�g
    [SerializeField] private GameObject eventCutInObject;
    [SerializeField] private Image eventCutInImg;
    [SerializeField] TextMeshProUGUI eventText;
    [SerializeField] TextMeshProUGUI effectText;
    private Tween eventCutInFade;
    private Sequence eventCutInSequence;

    //�m�[�}���J�b�g�C���p�̃I�u�W�F�N�g
    [SerializeField] private GameObject normalCutInObject;
    [SerializeField] private Image normalCutInBase;
    [SerializeField] private Image normalCutInCharacter;
    private Tween normalCutInFade;
    private Sequence normalCutInSequence;

    private const float CUTIN_SPEED = 0.5f;
    private const float CUTIN_TIME = 2.5f;

    /// <summary>
    /// �J�b�g�C���p�̃L�����N�^�[�摜��ScriptableObject����擾����
    /// </summary>
    /// <param name="eventCutInImage">�C�x���g�J�b�g�C���p�̃L�����摜�i�㔼�g�j</param>
    /// <param name="playerCharacter">�m�[�}���J�b�g�C���p�̃L�����摜</param>
    public void ReadyGame(Sprite eventCutInImage, Sprite playerCharacter)
    {
        eventCutInObject.SetActive(false);
        eventCutInImg.DOFillAmount(0f, 0);
        eventCutInImg.sprite = eventCutInImage;

        normalCutInObject.SetActive(false);

        normalCutInCharacter.sprite = playerCharacter;
    }

    /// <summary>
    /// �L�����̉摜�{�C�x���g���e�̃J�b�g�C��
    /// </summary>
    /// <param name="eventName">�yTRIGGERSKILL�z�ȂǃC�x���g�̖��O</param>
    /// <param name="eventEffect">�yHP���񕜁z�ȂǃC�x���g�̓��e</param>
    /// <returns></returns>
    public IEnumerator ActiveEventCutIn(string eventName,string eventEffect)
    {
        eventText.text = @$"{eventName}";
        effectText.text = @$"{eventEffect}";

        soundManager.PlayEventCutIn();
        eventCutInObject.SetActive(true);
        StartCoroutine(AnimateEventCutIn());
        yield return null;
    }

    /// <summary>
    /// �C�x���g�J�b�g�C�����ړ������ݒ莞�ԕb��ɔ�\���ɂ���B���s����O�ɑO�̃J�b�g�C���̏�������xKill����B
    /// </summary>
    /// <returns></returns>
    public IEnumerator AnimateEventCutIn()
    {
        if (eventCutInFade != null && eventCutInFade.IsActive())
        {
            eventCutInFade.Kill();
        }

        if (eventCutInSequence == null)
        {
            eventCutInSequence = DOTween.Sequence()
                .Append(eventCutInObject.transform.DOLocalMoveX(-1000, CUTIN_SPEED).From())
                .SetAutoKill(false)
                .Pause();
            eventCutInSequence.Play();
        }
        else
        {
            eventCutInObject.transform.localPosition = new Vector3(0, eventCutInObject.transform.localPosition.y, eventCutInObject.transform.transform.localPosition.z);
        }
        eventCutInSequence.Restart();

        eventCutInFade = DOVirtual.DelayedCall(CUTIN_TIME, () =>
        {
            eventCutInObject.SetActive(false); 
        });
        yield return null;
    }

    public void ActiveNormalCutIn()
    {
        normalCutInObject.SetActive(true);
        StartCoroutine(AnimateNormalCutIn());
    }

    /// <summary>
    /// �A���ŌĂяo���ꂽ�ꍇ�ɈӐ}���Ȃ��ʒu�ŃI�u�W�F�N�g����~���Ă��܂����߁A�O�������c���Ă����ꍇKill����B
    /// ����̂݃V�[�P���X�𐶐����A2��ڈȍ~�̓|�[�Y���Ă����V�[�P���X�����X�^�[�g������B
    /// </summary>
    /// <returns></returns>
    public IEnumerator AnimateNormalCutIn()
    {
        
        if (normalCutInFade != null && normalCutInFade.IsActive())
        {
            normalCutInFade.Kill();
        }

        if (normalCutInSequence == null)
        {
            normalCutInSequence = DOTween.Sequence()
                .Append(normalCutInBase.transform.DOScaleY(0, CUTIN_SPEED).From())
                .Append(normalCutInCharacter.transform.DOLocalMoveX(-400, CUTIN_SPEED).From())
                .SetAutoKill(false) 
                .Pause();           
            normalCutInSequence.Play();
        }
        else
        {
            normalCutInBase.transform.localPosition = new Vector3(0, normalCutInBase.transform.localPosition.y, normalCutInBase.transform.localPosition.z);
            normalCutInCharacter.transform.localPosition = new Vector3(0, normalCutInCharacter.transform.localPosition.y, normalCutInCharacter.transform.localPosition.z);
        }

        normalCutInSequence.Restart();
        normalCutInFade = DOVirtual.DelayedCall(CUTIN_TIME, () =>
        {
            normalCutInObject.SetActive(false);
        });


        yield return null;
    }

}
