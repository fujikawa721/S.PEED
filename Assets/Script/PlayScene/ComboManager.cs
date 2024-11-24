using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ComboManager : MonoBehaviour
{
    //------------------------------------
    //�R���{���Ǘ�����N���X
    //�R���{���𑝌������A���݂̃R���{����Ԃ��B
    //------------------------------------
    
    
    [SerializeField] public GameController gameController;
    [SerializeField] public SoundManager soundManager;


    [SerializeField] GameObject comboTextObject;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] private Image comboTimerImage;

    public int combo = 0;

    //�y5�b�ȓ��z�ɍU�����s���ƃR���{����������B����O�̃L�����N�^�[�����݂���B
    private int comboTime = 5;
    private int timerCount;

    //�^�C�}�[�̃A�j���[�V�����̓��쑬�x�́y0.5f�z�b
    private const float DURATION = 0.5f;


    /// <summary>
    /// �R���{�����Z�����鏈���B
    /// </summary>
    public int AddCombo()
    {
        timerCount = comboTime;
        combo++;
        if (combo == 1)
        {
            StartCoroutine(ComboCounter());
        }

        if(combo > 1)
        {
            comboText.text = @$"{combo}�R���{!";
            soundManager.PlayCombo();
            AnimateComboText();
        }

        return combo;
    }

    public void ResetCombo()
    {
        combo = 0;
    }

    /// <summary>
    /// �R���{�����𔻒肷�鏈���B�Q�[�������f����Ă���Ԃ̓J�E���^�[�𒆒f������B
    /// </summary>
    private IEnumerator ComboCounter()
    {
        while (timerCount > 0)
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
            float timerFill = 1 - (float)timerCount / comboTime;
            comboTimerImage.DOFillAmount(timerFill, DURATION);

            if (timerCount == 0)
            {
                combo = 0;
                comboText.text = @$"";
            }

        }
    }

    private void AnimateComboText()
    {
        comboTextObject.SetActive(true);
        comboTextObject.transform.DOScale(new Vector3(1.2f, 1.2f, 0f), 1f)
            .OnComplete(() =>
            {
                comboTextObject.transform.DOScale(new Vector3(1, 1f, 0f), 0f);
                comboTextObject.SetActive(false);
            });

    }

}
