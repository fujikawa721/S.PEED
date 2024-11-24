using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectController : MonoBehaviour
{
    //PlaySceneのGameControllerから呼び出される。
    public static CharacterData playerCharacterData;
    public static CharacterData enemyCharacterData;

    [SerializeField] private LoadingManager loadingManager;
    [SerializeField] private SoundManager soundManager;

    [SerializeField] CharacterInfo playerCharacter;
    [SerializeField] CharacterInfo enemyCharacter;
    [SerializeField] GameObject loadDisplay;
    [SerializeField] GameObject readyPanel;
    [SerializeField] MenuButton backButton;
    [SerializeField] MenuButton okButton;

    [SerializeField] private CharacterData nullCharacterData;

    private const float LOAD_TIME = 3.0f;
    private int selectionStatus = 1;

    void Start()
    {
        loadDisplay.SetActive(false);
        readyPanel.SetActive(false);
        backButton.SetAction(ClickBackButton);
        okButton.SetAction(ClickOKButton);

        StartCoroutine(loadingManager.EndLoad());
        soundManager.PlayBgmCharacterSelection();
    }

    /// <summary>
    /// キャラクターの顔画像にカーソルを合わせたときにキャラクターの情報を表示させる。
    /// </summary>
    /// <param name="characterData">キャラフェイスにアタッチされてるキャラクターデータ</param>
    /// <returns></returns>
    public IEnumerator PassCharacterData(CharacterData characterData)
    {
        soundManager.SetCharacterVoice(characterData);
        soundManager.PlayCursor();
        switch (selectionStatus)
        {
            case 1:
                playerCharacter.ReceiveData(characterData);
                break;
            case 2:
                enemyCharacter.ReceiveData(characterData);
                break;
            case 3:
                break;
            default:
                Debug.Log(@$"selectionStatusが正しくありません。");
                break;
        }
        yield return null;
    }


    /// <summary>
    /// キャラクターを選択した場合の処理。カーソルを動かさず同じキャラクターをクリックすると2Pプレイヤーに画像が表示されないため、PointerEnterと同じ処理を一度実行する。
    /// </summary>
    /// <param name="chara_data"></param>
    /// <returns></returns>
    public IEnumerator SelectCharacter(CharacterData characterData)
    {
        StartCoroutine(PassCharacterData(characterData));
        soundManager.PlaySelect();
        switch (selectionStatus)
        {
            case 1:
                soundManager.PlayCVGameStart();
                playerCharacterData = characterData;
                selectionStatus = 2;
                break;
            case 2:
                soundManager.PlayCVGameStart();
                enemyCharacterData = characterData;
                selectionStatus = 3;
                readyPanel.SetActive(true);
                break;
            case 3:
                break;
            default:
                Debug.Log(@$"selectionStatusが正しくありません。");
                break;
        }
        yield return null;
    }

    public void ClickBackButton()
    {
        StartCoroutine(BackSelectionStatus());
    }

    public void ClickOKButton()
    {
        StartCoroutine(GoGameplayScene());
    }



    public IEnumerator BackSelectionStatus()
    {
        soundManager.PlayBack();
        switch (selectionStatus)
        {
            case 1:
                break;
            case 2:
                selectionStatus = 1;
                playerCharacter.ReceiveData(nullCharacterData);
                break;
            case 3:
                selectionStatus = 2;
                enemyCharacter.ReceiveData(nullCharacterData);
                readyPanel.SetActive(false);
                break;
            default:
                Debug.Log(@$"selection_statusが正しくありません。");
                break;
        }
        yield return null;
    }

    public IEnumerator GoGameplayScene()
    {
        soundManager.PlayGameready();
        loadDisplay.SetActive(true);
        yield return new WaitForSeconds(LOAD_TIME);
        SceneManager.LoadScene("Play");
        yield return null;
    }

}
