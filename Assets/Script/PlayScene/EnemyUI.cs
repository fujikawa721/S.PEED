using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] PlayerHandController enemyHandController;
    
    public const float ACTION_SPEED = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// CPUのAI行動。ゲーム終了フラグがtrueになるまで繰り返され、ゲーム中断フラグが経っていると行動を行わない。
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
                    yield return StartCoroutine(enemyHandController.DoEnemyAction());   
                }
            }

        yield return null;
        }
    }


}
