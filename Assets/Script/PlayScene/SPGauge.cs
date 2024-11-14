using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SPGauge : MonoBehaviour
{

    [SerializeField] private Image spPointImage;
    [SerializeField] private Image burnImage;

    private const float DURATION = 0.5f;//ƒQ[ƒW‚ªi‚ÞŽžŠÔ
    private const float STRENGTH = 20f;//—h‚ê‚Ì‹­‚³
    private int VIBRATE = 100;//—h‚ê‹ï‡

    private float nowSpGauge = 0f;


    public void SetGauge(float value)
    {
        // DoTween‚ð˜AŒ‹‚µ‚Ä“®‚©‚·
        spPointImage.DOFillAmount(value, DURATION)
            .OnComplete(() =>
            {
                burnImage
                    .DOFillAmount(value, DURATION / 2f)
                    .SetDelay(0.5f);
            });
        transform.DOShakePosition(
            DURATION / 2f,
            STRENGTH, VIBRATE);

        nowSpGauge = value;
    }



    public void PlusSpGauge(float plusRate)
    {
        float plusGauge = plusRate;
        SetGauge(plusGauge);
    }
}
