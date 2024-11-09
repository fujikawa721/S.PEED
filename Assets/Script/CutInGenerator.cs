using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CutInGenerator : MonoBehaviour
{

    [SerializeField] public Sprite cutin_001;
    [SerializeField] public Sprite cutin_002;
    [SerializeField] private Image catin_img;

    [SerializeField] private GameObject cutin_obj;

    private const float CUTIN_SPEED = 0.5f;//カットインが表示される速度

    AudioSource audioSource;
    public AudioClip sp_do;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        catin_img = GetComponent<Image>();

        cutin_obj.SetActive(false);
        catin_img.DOFillAmount(0f, CUTIN_SPEED);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator animate_special_cutin()
    {
        play_se_sp_do();
        cutin_obj.SetActive(true);
        catin_img.DOFillAmount(1.0f, CUTIN_SPEED).OnComplete(() =>
        {
            catin_img.DOFillAmount(0f, CUTIN_SPEED).SetDelay(5);
        });
        yield return null;
    }


    public void check_cutin_img(int special_id)
    {
        switch (special_id)
        {
            case 1:
                catin_img.sprite = cutin_001;
                break;
            case 2:
                catin_img.sprite = cutin_002;
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
