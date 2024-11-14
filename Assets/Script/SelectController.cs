using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectController : MonoBehaviour
{
    //PlaySceneのGameControllerから呼び出される。
    public static CharacterData player_character_data;
    public static CharacterData enemy_character_data;

    [SerializeField] CharacterInfo playerCharacter;
    [SerializeField] CharacterInfo enemyCharacter;
    [SerializeField] GameObject ready_button;
    [SerializeField] GameObject load_display;

    private const float LOAD_TIME = 3.0f;
    private int selection_status = 1;

    AudioSource audioSource;
    public AudioClip cursor;
    public AudioClip select;
    public AudioClip back;
    public AudioClip gameready;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ready_button.SetActive(false);
        load_display.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator pass_character_data(CharacterData chara_data)
    {
        play_se_cursor();
        switch (selection_status)
        {
            case 1:
                playerCharacter.receive_data(chara_data);
                break;
            case 2:
                enemyCharacter.receive_data(chara_data);
                break;
            case 3:
                break;
            default:
                Debug.Log(@$"selection_statusが正しくありません。");
                break;
        }
        yield return null;
    }

    //キャラクターを選択した場合の処理。
    //カーソルを動かさず同じキャラクターをクリックすると2Pプレイヤーに画像が表示されないため、PointerEnterと同じ処理を一度実行する。
    public IEnumerator select_character(CharacterData chara_data)
    {
        StartCoroutine(pass_character_data(chara_data));
        play_se_select();
        switch (selection_status)
        {
            case 1:
                player_character_data = chara_data;
                selection_status = 2;
                break;
            case 2:
                enemy_character_data = chara_data;
                selection_status = 3;
                ready_button.SetActive(true);
                break;
            case 3:
                break;
            default:
                Debug.Log(@$"selection_statusが正しくありません。");
                break;
        }
        yield return null;
    }

    public IEnumerator back_selection_status()
    {
        play_se_back();
        switch (selection_status)
        {
            case 1:
                break;
            case 2:
                selection_status = 1;
                break;
            case 3:
                selection_status = 2;
                ready_button.SetActive(false);
                break;
            default:
                Debug.Log(@$"selection_statusが正しくありません。");
                break;
        }
        yield return null;
    }

    public IEnumerator goto_gameplay()
    {
        play_se_gameready();
        load_display.SetActive(true);
        yield return new WaitForSeconds(LOAD_TIME);
        SceneManager.LoadScene("Play");
        yield return null;
    }

    //★★★★効果音に関する処理★★★★
    public void play_se_cursor()
    {
        audioSource.clip = cursor;
        audioSource.Play();
    }

    public void play_se_select()
    {
        audioSource.clip = select;
        audioSource.Play();
    }

    public void play_se_back()
    {
        audioSource.clip = back;
        audioSource.Play();
    }

    public void play_se_gameready()
    {
        audioSource.clip = gameready;
        audioSource.Play();
    }

}
