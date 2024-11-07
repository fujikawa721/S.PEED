using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPGauge : MonoBehaviour
{

    [SerializeField] private Image healthImage;
    [SerializeField] private Image burnImage;

    public float duration = 0.5f;//ゲージが進む時間
    public float strength = 20f;//揺れの強さ
    public int vibrate = 100;//揺れ具合

    public float debugDamageRate = 0.1f;

    private float currentRate = 1f;

    // Start is called before the first frame update
    void Start()
    {
        SetGauge(1f);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetGauge(float value)
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



    public void take_damage(float takes_damage, float max_hp)
    {
        float decrease_gauge = takes_damage / max_hp;
        SetGauge(currentRate - decrease_gauge);
    }


}
