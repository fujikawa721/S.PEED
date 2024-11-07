using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private HPGauge hpGauge;
    [SerializeField] private Player enemyPlayer;


    public int max_hp = 100;
    public int now_hp = 100;
    public int attack_damage = 2;
    public int max_sp_point = 0;
    public string element_mark = "D";

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
        //敵エネミーのダメージ食らう処理
        enemyPlayer.tekes_damage(attack_damage);
    }

    public void tekes_damage(int damage)
    {
        now_hp = now_hp - damage;
        hpGauge.take_damage(damage,max_hp);
    }
}
