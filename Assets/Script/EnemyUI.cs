using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] EnemyHandController enemyHandController;
    
    public const float ACTION_SPEED = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator enemy_action()
    {
        Debug.Log(@$"敵UIを起動");
        for (int i = 0; i < 999; i++)
        {
            if (gameController.enemy_action_flg == 1)
            {
                Debug.Log(@$"敵プレイヤーが手札を場札に置きます");
                enemyHandController.put_handcard_center();
                yield return new WaitForSeconds(ACTION_SPEED);
            }
            else{
                Debug.Log(@$"敵プレイヤーは置けるカードがありません");
                yield return new WaitForSeconds(ACTION_SPEED);
            }

        yield return null;
        }
    }
}
