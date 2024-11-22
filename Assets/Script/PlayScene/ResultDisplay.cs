using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class ResultDisplay : MonoBehaviour
{
    [SerializeField] private GameObject resultDisplay;
    [SerializeField] private LoadingManager loadingManager;
    [SerializeField] private SoundManager soundManager;

    [SerializeField] private GameObject cutInObject;
    [SerializeField] private Image cutInImg;
    [SerializeField] private GameObject winImage;

    [SerializeField] private GameObject winnerData;
    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] TextMeshProUGUI winPlayerName;

    
    [SerializeField] private GameObject nextButton;
    [SerializeField] private MenuButton nextButtonScript;
    [SerializeField] private GameObject characterSelectionButton;
    [SerializeField] private MenuButton characterSelectionScript;

    private Tween winImageAnimation;

    private const float CUTIN_SPEED = 0.5f;//�J�b�g�C�����\������鑬�x
    private const float LOAD_TIME = 1.0f;//��ʑJ�ڂ���܂łɍŒ�ł��y1�b�ԁz�҂�

    void Start()
    {
        resultDisplay.SetActive(false);
        nextButton.SetActive(false);
        characterSelectionButton.SetActive(false);
    }

    public IEnumerator Ready(Player player,string winner)
    {
        
        winPlayerName.text = @$"{winner}";
        characterName.text = @$"{player.characterData.characterName}"; 
        cutInImg.sprite = player.characterData.specialCutInImage;
        nextButtonScript.SetAction(ClickButtonNext);
        characterSelectionScript.SetAction(ClickButtonCharacterSelection);
        yield return new WaitForSeconds(2.0f);
        soundManager.PlayBgmResult();
        resultDisplay.SetActive(true);
        StartCoroutine(AnimateResultDisplay());
        yield return null;
    }

    /// <summary>
    /// �u���ɐi�ށv�{�^���������ꂽ���̏����B�Q�[���I����ʂɂȂ���B
    /// </summary>
    private void ClickButtonNext()
    {
        StartCoroutine(MoveEndGameScene());
    }

    private IEnumerator MoveEndGameScene()
    {
        yield return StartCoroutine(loadingManager.StartLoad());
        yield return new WaitForSeconds(LOAD_TIME);
        SceneManager.LoadScene("EndScene");
    }

    /// <summary>
    /// �u�L�����N�^�[�I���v�{�^���������ꂽ���̏����B�L�����N�^�[�I����ʂɂȂ���B
    /// </summary>
    private void ClickButtonCharacterSelection()
    {
        StartCoroutine(MoveCharacterSelection());
    }

    private IEnumerator MoveCharacterSelection()
    {
        yield return StartCoroutine(loadingManager.StartLoad());
        yield return new WaitForSeconds(LOAD_TIME);
        SceneManager.LoadScene("CharacterSelection");
    }



    public IEnumerator AnimateResultDisplay()
    {
        Debug.Log("�A�j���[�V�����J�n");
        StartCoroutine(AnimateLeftSlideObject(cutInObject));
        StartCoroutine(AnimateRightSlideObject(winnerData));
        StartCoroutine(AnimateRightSlideObject(winImage));
        StartCoroutine(AnimateBigSmallObject(winImage));

        yield return new WaitForSeconds(2.0f);
        StartCoroutine(AnimateRightSlideObject(nextButton));
        StartCoroutine(AnimateRightSlideObject(characterSelectionButton));
        yield return null;
    }

    public IEnumerator AnimateRightSlideObject(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.DOLocalMoveX(500, CUTIN_SPEED).From();
        yield return null;
    }

    public IEnumerator AnimateLeftSlideObject(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.DOLocalMoveX(-1000, CUTIN_SPEED).From();
        yield return null;
    }

    public IEnumerator AnimateBigSmallObject(GameObject obj)
    {
        winImageAnimation = DOTween.Sequence().SetLoops(-1, LoopType.Restart)
            .Append(obj.transform.DOScale(new Vector3(1.2f, 1.2f, 0f), 1f))
            .Append(obj.transform.DOScale(new Vector3(1.0f, 1.0f, 0f), 1f));

        yield return null;
    }


}
