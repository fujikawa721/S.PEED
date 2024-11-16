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
    /// CPU��AI�s���B�Q�[���I���t���O��true�ɂȂ�܂ŌJ��Ԃ���A�Q�[�����f�t���O���o���Ă���ƍs�����s��Ȃ��B
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
                    yield return StartCoroutine(enemyHandController.DoEnemyAction());   
                }
            }

        yield return null;
        }
    }


}
