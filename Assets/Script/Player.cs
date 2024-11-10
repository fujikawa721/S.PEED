using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] private HPGauge hpGauge;
    [SerializeField] private SPGauge spGauge;
    [SerializeField] private Player enemyPlayer;
    [SerializeField] private CutInGenerator cutInGenerator;
    [SerializeField] public CharacterData charaData;

    //キャラクターの顔画像読み込み
    [SerializeField] public Sprite face_001;
    [SerializeField] public Sprite face_002;
    [SerializeField] public GameObject face_obj;
    [SerializeField] private Image face_img;


    AudioSource audioSource;
    public AudioClip spgauge_max;

    private const int PLUS_SPPOINT = 1;



    public int now_hp = 100;
    public int now_sp_point = 0;

    public bool can_special;


    // Start is called before the first frame update
    void Start()
    {
        ready_game();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void attack_enemy()
    {
        enemyPlayer.tekes_damage(charaData.attack_damage);
    }

    public void tekes_damage(int damage)
    {
        now_hp = now_hp - damage;
        hpGauge.take_damage(damage, charaData.max_hp);
    }

    public void plus_sp_gauge()
    {
        now_sp_point += PLUS_SPPOINT;
        float plus_sp_rate = ((float)now_sp_point / charaData.max_sp_point);
        spGauge.increase_spgauge(plus_sp_rate);
        judge_sp_point();
    }

    public  void judge_sp_point()
    {     
        if (now_sp_point >= charaData.max_sp_point)
        {
            play_se_spgauge_max();
            can_special = true;
        }
        else
        {
            can_special = false;
        }
        Debug.Log(@$"現在のSPポイントは{now_sp_point}。MAXSPは{charaData.max_sp_point}、スペシャル発動：{can_special}");

    }

    public IEnumerator check_special_type()
    {
        switch (charaData.special_id)
        {
            case 1:
                StartCoroutine(special_bakuen());
                break;
            case 2:
                StartCoroutine(special_iyashi_no_uta());
                break;
            default:
                Debug.Log(@$"スペシャルIDにエラーがあります");
                break;
        }
        yield return null;
    }


    private void ready_game()
    {
        audioSource = GetComponent<AudioSource>();
        cutInGenerator.check_cutin_img(charaData.special_id);
        check_playerface();
    }

    public IEnumerator special_bakuen()
    {
        StartCoroutine(cutInGenerator.animate_special_cutin());
        enemyPlayer.tekes_damage(charaData.attack_damage * 10);
        now_sp_point = 0;
        judge_sp_point();
        spGauge.increase_spgauge(0f);
        yield return null;
    }

    public IEnumerator special_iyashi_no_uta()
    {
        StartCoroutine(cutInGenerator.animate_special_cutin());
        now_sp_point = 0;
        tekes_damage(-50);
        judge_sp_point();
        spGauge.increase_spgauge(0f);
        yield return null;
    }

    private void check_playerface()
    {
        face_img = face_obj.GetComponent<Image>(); ;
        switch (charaData.special_id)
        {
            case 1:
                face_img.sprite = face_001;
                break;
            case 2:
                face_img.sprite = face_002;
                break;
            default:
                Debug.Log(@$"スペシャルIDにエラーがあります");
                break;
        }
    }

    //★効果音再生
    public void play_se_spgauge_max()
    {
        audioSource.clip = spgauge_max;
        audioSource.Play();
    }
}
