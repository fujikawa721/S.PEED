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

    public IEnumerator ActionEnemy()
    {
        Debug.Log(@$"敵アクション開始");
        while (gameController.endGameFlg == false)
        {
            if (gameController.endGameFlg == true)
            {
                break;
            }

            if (gameController.canEnemyAction == true)
            {
                enemyHandController.DoEnemyAction();
                yield return new WaitForSeconds(ACTION_SPEED);
            }
            else{
                yield return new WaitForSeconds(ACTION_SPEED);
            }

        yield return null;
        }
    }


}
