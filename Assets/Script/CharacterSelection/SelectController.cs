using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectController : MonoBehaviour
{
    //PlayScene��GameController����Ăяo�����B
    public static CharacterData player_character_data;
    public static CharacterData enemy_character_data;

    [SerializeField] private LoadingManager loadingManager;
    [SerializeField] private SoundManager soundManager;

    [SerializeField] CharacterInfo playerCharacter;
    [SerializeField] CharacterInfo enemyCharacter;
    [SerializeField] GameObject load_display;
    [SerializeField] GameObject readyPanel;
    [SerializeField] MenuButton backButton;
    [SerializeField] MenuButton okButton;

    [SerializeField] private CharacterData nullCharacterData;

    private const float LOAD_TIME = 3.0f;
    private int selection_status = 1;

    void Start()
    {
        load_display.SetActive(false);
        readyPanel.SetActive(false);
        backButton.SetAction(ClickBackButton);
        okButton.SetAction(ClickOKButton);


        StartCoroutine(loadingManager.EndLoad());
        soundManager.PlayBgmCharacterSelection();
    }

    /// <summary>
    /// �L�����N�^�[�̊�摜�ɃJ�[�\�������킹���Ƃ��ɃL�����N�^�[�̏���\��������B
    /// </summary>
    /// <param name="chara_data">�L�����t�F�C�X�ɃA�^�b�`����Ă�L�����N�^�[�f�[�^</param>
    /// <returns></returns>
    public IEnumerator pass_character_data(CharacterData chara_data)
    {
        soundManager.PlayCursor();
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
                Debug.Log(@$"selection_status������������܂���B");
                break;
        }
        yield return null;
    }


    /// <summary>
    /// �L�����N�^�[��I�������ꍇ�̏����B�J�[�\���𓮂����������L�����N�^�[���N���b�N�����2P�v���C���[�ɉ摜���\������Ȃ����߁APointerEnter�Ɠ�����������x���s����B
    /// </summary>
    /// <param name="chara_data"></param>
    /// <returns></returns>
    public IEnumerator select_character(CharacterData chara_data)
    {
        StartCoroutine(pass_character_data(chara_data));
        soundManager.PlaySelect();
        switch (selection_status)
        {
            case 1:
                player_character_data = chara_data;
                selection_status = 2;
                break;
            case 2:
                enemy_character_data = chara_data;
                selection_status = 3;
                readyPanel.SetActive(true);
                break;
            case 3:
                break;
            default:
                Debug.Log(@$"selection_status������������܂���B");
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
        switch (selection_status)
        {
            case 1:
                break;
            case 2:
                selection_status = 1;
                playerCharacter.receive_data(nullCharacterData);
                break;
            case 3:
                selection_status = 2;
                enemyCharacter.receive_data(nullCharacterData);
                readyPanel.SetActive(false);
                break;
            default:
                Debug.Log(@$"selection_status������������܂���B");
                break;
        }
        yield return null;
    }

    public IEnumerator GoGameplayScene()
    {
        soundManager.PlayGameready();
        load_display.SetActive(true);
        yield return new WaitForSeconds(LOAD_TIME);
        SceneManager.LoadScene("Play");
        yield return null;
    }

}
