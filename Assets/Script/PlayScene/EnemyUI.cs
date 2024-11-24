using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] Player enemy;
    
    public const float ACTION_SPEED = 2.5f;


    /// <summary>
    /// CPU��AI�s���B�Q�[���I���t���O��true�ɂȂ�܂ŌJ��Ԃ���A�Q�[�����f�t���O���o���Ă���ƍs�����s��Ȃ��B
    /// �v���C�\�t���O�ƃG�l�~�[�s���\�t���O�������Ă��鎞�ɍs�����s���B
    /// </summary>
    public IEnumerator ActionEnemy()
    {
        Debug.Log(@$"�G�A�N�V�����J�n");
        while (gameController.endGameFlg == false)
        {
            yield return new WaitForSeconds(ACTION_SPEED);
            if (gameController.endGameFlg == true)
            {
                break;
            }

            if (gameController.canPlayNow == true)
            {
                if (gameController.canEnemyAction == true)
                {
                    yield return StartCoroutine(enemy.DoEnemyAction());
                }
            }

        yield return null;
        }
    }


}
