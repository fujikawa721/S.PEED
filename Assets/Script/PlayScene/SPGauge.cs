using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SPGauge : MonoBehaviour
{

    [SerializeField] private Image spPointImage;
    [SerializeField] private Image burnImage;

    private const float DURATION = 0.5f;//ゲージが進む時間
    private const float STRENGTH = 20f;//揺れの強さ
    private int VIBRATE = 100;//揺れ具合

    private float nowSpGauge = 0f;


    public void SetGauge(float value)
    {
        // DoTweenを連結して動かす
        spPointImage.DOFillAmount(value, DURATION)
            .OnComplete(() =>
            {
                burnImage
                    .DOFillAmount(value, DURATION / 2f)
                    .SetDelay(0.5f);
            });
        //transform.DOShakePosition(
        //    DURATION / 2f,
        //    STRENGTH, VIBRATE);

        nowSpGauge = value;
    }



    public void PlusSpGauge(float plusRate)
    {
        float plusGauge = plusRate;
        SetGauge(plusGauge);
    }
}
