using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] public GameController gameController;
    public int countdownSeconds;

    // 制限時間が超えたときのコールバック処理
    public delegate void LimitTimer();
    private LimitTimer limitTimerCallBack;


    public void SetTimer(int Second,LimitTimer limitTimer)
    {
        countdownSeconds = Second;
        limitTimerCallBack = limitTimer;
    }

    public IEnumerator CountDown()
    {
        while (countdownSeconds > 0)
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
            countdownSeconds -= 1;
            timeText.text = $"{countdownSeconds}";

            if (countdownSeconds == 0)
            {
                limitTimerCallBack();
            }
        }
    }



}
