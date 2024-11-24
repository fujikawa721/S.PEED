using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPGauge : MonoBehaviour
{

    [SerializeField] private Image healthImage;
    [SerializeField] private Image burnImage;

    private const float DURATION = 0.5f;//ƒQ[ƒW‚ªi‚ŞŠÔ
    private const float STRENGTH = 20f;//—h‚ê‚Ì‹­‚³
    private int VIBRATE = 100;//—h‚ê‹ï‡

    private float nowHpGauge = 1f;
    private Tween shakeTween;

    /// <summary>
    /// UŒ‚‚ğó‚¯‚½‚Æ‚«…F‚ÌƒQ[ƒW‚ğæ‚ÉŒ¸‚ç‚µAy0.5•bŒãz‚ÉÔF‚ÌƒQ[ƒW‚ª’Ç]‚·‚éˆ—B
    /// “¯‚ÉÀs‚³‚ê‚½‚Æ‚«‚ÉˆÓ}‚µ‚È‚¢ˆÊ’u‚ÅƒQ[ƒW‚ª’â~‚·‚é‚Ì‚Åtween‚ÅŠÇ—‚·‚éB
    /// </summary>
    /// <param name="value"></param>
    public void SetGauge(float value)
    {

        healthImage.DOFillAmount(value, DURATION).OnComplete(() =>
            {
                burnImage.DOFillAmount(value, DURATION / 2f).SetDelay(0.5f);
            });
        if (shakeTween == null) {
            shakeTween = transform.DOShakePosition(DURATION / 2f,STRENGTH, VIBRATE).SetAutoKill(false).Pause();
        }
        shakeTween.Restart();
        nowHpGauge = value;
    }



    public void TakeDamage(float takesDamage, float maxHp)
    {
        float decreaseGauge = takesDamage / maxHp;
        SetGauge(nowHpGauge - decreaseGauge);
    }


}
