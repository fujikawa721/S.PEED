using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPGauge : MonoBehaviour
{

    [SerializeField] private Image healthImage;
    [SerializeField] private Image burnImage;

    private const float DURATION = 0.5f;//ゲージが進む時間
    private const float STRENGTH = 20f;//揺れの強さ
    private int VIBRATE = 100;//揺れ具合

    private float nowHpGauge = 1f;

    public void SetGauge(float value)
    {
        healthImage.DOFillAmount(value, DURATION)
            .OnComplete(() =>
            {
                burnImage
                    .DOFillAmount(value, DURATION / 2f)
                    .SetDelay(0.5f);
            });
        transform.DOShakePosition(
            DURATION / 2f,
            STRENGTH, VIBRATE);

        nowHpGauge = value;
    }



    public void TakeDamage(float takesDamage, float maxHp)
    {
        float decreaseGauge = takesDamage / maxHp;
        SetGauge(nowHpGauge - decreaseGauge);
    }


}
