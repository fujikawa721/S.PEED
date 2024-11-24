using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPGauge : MonoBehaviour
{

    [SerializeField] private Image healthImage;
    [SerializeField] private Image burnImage;

    private const float DURATION = 0.5f;//�Q�[�W���i�ގ���
    private const float STRENGTH = 20f;//�h��̋���
    private int VIBRATE = 100;//�h��

    private float nowHpGauge = 1f;
    private Tween shakeTween;

    /// <summary>
    /// �U�����󂯂��Ƃ����F�̃Q�[�W���Ɍ��炵�A�y0.5�b��z�ɐԐF�̃Q�[�W���Ǐ]���鏈���B
    /// �����Ɏ��s���ꂽ�Ƃ��ɈӐ}���Ȃ��ʒu�ŃQ�[�W����~����̂�tween�ŊǗ�����B
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
