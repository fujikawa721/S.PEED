using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SPGauge : MonoBehaviour
{

    [SerializeField] private Image healthImage;
    [SerializeField] private Image burnImage;

    public float duration = 0.5f;//ゲージが進む時間
    public float strength = 20f;//揺れの強さ
    public int vibrate = 100;//揺れ具合

    public float debugDamageRate = 0.1f;

    private float currentRate = 0f;

    void Start()
    {
        set_gauge(0f);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void set_gauge(float value)
    {
        // DoTweenを連結して動かす
        healthImage.DOFillAmount(value, duration)
            .OnComplete(() =>
            {
                burnImage
                    .DOFillAmount(value, duration / 2f)
                    .SetDelay(0.5f);
            });
        transform.DOShakePosition(
            duration / 2f,
            strength, vibrate);

        currentRate = value;
    }



    public void increase_spgauge(float plus_sp_rate)
    {
        float plus_gauge = plus_sp_rate;
        set_gauge(plus_gauge);
    }
}
