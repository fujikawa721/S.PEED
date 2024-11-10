using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CutInGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cutin_obj;


    [SerializeField] public Sprite cutin_001;
    [SerializeField] public Sprite cutin_002;
    [SerializeField] private Image catin_img;

    [SerializeField] TextMeshProUGUI text_top;
    [SerializeField] TextMeshProUGUI text_bottom;


    private const float CUTIN_SPEED = 0.5f;//カットインが表示される速度
    private string special_name;

    AudioSource audioSource;
    public AudioClip sp_do;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cutin_obj.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator animate_special_cutin()
    {
        play_se_sp_do();
        cutin_obj.SetActive(true);
        text_bottom.text = special_name;
        catin_img.DOFillAmount(1.0f, CUTIN_SPEED).OnComplete(() =>
        {
            catin_img.DOFillAmount(0f, CUTIN_SPEED).SetDelay(5);
        });
        yield return new WaitForSeconds(6.0f);
        cutin_obj.SetActive(false);
    }


    public void check_cutin_img(int special_id)
    {
        catin_img = GetComponent<Image>();
        catin_img.DOFillAmount(0f, 0);
        switch (special_id)
        {
            case 1:
                catin_img.sprite = cutin_001;
                special_name = "火閃斬";
                break;
            case 2:
                catin_img.sprite = cutin_002;
                special_name = "いやしの歌";
                break;
            default:
                Debug.Log(@$"スペシャルIDにエラーがあります");
                break;
        }
    }

    //★効果音再生
    public void play_se_sp_do()
    {
        audioSource.clip = sp_do;
        audioSource.Play();
    }
}
