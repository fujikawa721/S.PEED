using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ComboManager : MonoBehaviour
{
    //------------------------------------
    //コンボを管理するクラス
    //コンボ数を増減させ、現在のコンボ数を返す。
    //------------------------------------
    
    
    [SerializeField] public GameController gameController;
    [SerializeField] public SoundManager soundManager;


    [SerializeField] GameObject comboTextObject;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] private Image comboTimerImage;

    public int combo = 0;

    //【5秒以内】に攻撃を行うとコンボが成立する。※例外のキャラクターも存在する。
    private int comboTime = 5;
    private int timerCount;

    //タイマーのアニメーションの動作速度は【0.5f】秒
    private const float DURATION = 0.5f;


    /// <summary>
    /// コンボを加算させる処理。
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
            comboText.text = @$"{combo}コンボ!";
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
    /// コンボ成立を判定する処理。ゲームが中断されている間はカウンターを中断させる。
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
