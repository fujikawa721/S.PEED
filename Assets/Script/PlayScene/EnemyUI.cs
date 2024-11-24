using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] Player enemy;
    
    public const float ACTION_SPEED = 2.5f;


    /// <summary>
    /// CPUのAI行動。ゲーム終了フラグがtrueになるまで繰り返され、ゲーム中断フラグが経っていると行動を行わない。
    /// プレイ可能フラグとエネミー行動可能フラグが立っている時に行動を行う。
    /// </summary>
    public IEnumerator ActionEnemy()
    {
        Debug.Log(@$"敵アクション開始");
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
