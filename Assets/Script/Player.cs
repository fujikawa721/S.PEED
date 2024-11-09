using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private HPGauge hpGauge;
    [SerializeField] private SPGauge spGauge;
    [SerializeField] private Player enemyPlayer;

    private const int PLUS_SPPOINT = 1;

    public int max_hp = 100;
    public int now_hp = 100;
    public int attack_damage = 5;
    private int max_sp_point = 3;
    public int now_sp_point = 0;
    public string element_mark = "D";
    public int special_id = 1;


    public bool can_special;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void attack_enemy()
    {
        enemyPlayer.tekes_damage(attack_damage);
    }

    public void tekes_damage(int damage)
    {
        now_hp = now_hp - damage;
        hpGauge.take_damage(damage,max_hp);
    }

    public void plus_sp_gauge()
    {
        now_sp_point += PLUS_SPPOINT;
        float plus_sp_rate = ((float)now_sp_point / max_sp_point);
        spGauge.increase_spgauge(plus_sp_rate);
        judge_sp_point();
    }

    public  void judge_sp_point()
    {
        
        if (now_sp_point >= max_sp_point)
        {
            can_special = true;
        }
        else
        {
            can_special = false;
        }
        Debug.Log(@$"現在のSPポイントは{now_sp_point}。MAXSPは{max_sp_point}、スペシャル発動：{can_special}");

    }

    public IEnumerator check_special_type()
    {
        switch (special_id)
        {
            case 1:
                StartCoroutine(special_bakuen());
                break;
            default:
                Debug.Log(@$"スペシャルIDにエラーがあります");
                break;
        }
        yield return null;
    }

    public IEnumerator special_bakuen()
    {
        enemyPlayer.tekes_damage(attack_damage * 10);
        now_sp_point = 0;
        judge_sp_point();
        spGauge.increase_spgauge(0f);
        yield return null;
    }
}
