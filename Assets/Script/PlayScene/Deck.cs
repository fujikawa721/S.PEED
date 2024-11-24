using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

public class Deck : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] SoundManager soundManager;
    [SerializeField] FieldController field;
    [SerializeField] TextMeshProUGUI restDecksText;
    [SerializeField] Player player;
    [SerializeField] private Image deckImg;

    //�R�D�̍ő喇���́y52���z
    private const int NUMBER_OF_DECK = 52;
    
    //�R�D�����D�ɃJ�[�h��u�����ۂɁy0.2f�z�㑱������҂B
    private const float SPEED_DRAWFIELD = 0.2f;

    private int[] decks = new int[NUMBER_OF_DECK];
    private int restDeck = NUMBER_OF_DECK;

    private Tween deckAnimation;


    public void Update()
    {
        restDecksText.text = @$"{restDeck}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(@$"SP�|�C���g{player.nowSpPoint}");
        //transform.position -= Vector3.up * 0.3f;
        transform.localScale = Vector3.one * 1.0f;

        if (player.canDoSpecial == true)
        {
            Debug.Log(@$"�X�y�V���������I");
            StartCoroutine(player.DoSpecial());
            
        }
        else
        {
            Debug.Log(@$"�X�y�V�����Q�[�W�����܂��Ă܂���{player.canDoSpecial}");
        }
    }

    public void OnPointerEnter()
    {
        if (player.canDoSpecial == true)
        {
           // transform.position += Vector3.up * 0.3f;
            transform.localScale = Vector3.one * 1.1f;
        }
    }

    public void OnPointerExit()
    {
        if (player.canDoSpecial == true)
        {
            //transform.position -= Vector3.up * 0.3f;
            transform.localScale = Vector3.one * 1.0f;
        }

    }


    /// <summary>
    /// �f�b�L�������O�ɂȂ������A�V�����R�D����蒼���B�܂��AGameController���Q�[���J�n���ɌĂяo���B
    /// </summary>
    public IEnumerator MakePlayerDeck()
    {
        soundManager.PlayDeckMax();
        restDeck = NUMBER_OF_DECK;
        MakeDeckSerialNumber();
        ShuffleDeck();
        yield return null;
    }

    /// <summary>
    /// �R�D�̐擪�̃J�[�h����D�ɒu���B�Q�[���J�n���A�d�؂蒼������GameController����Ăяo�����B
    /// </summary>
    public IEnumerator MakeField(int fieldcardNumber)
    {
        if (restDeck < 1)
        {
            yield return StartCoroutine(MakePlayerDeck());
        }
        Debug.Log("�R�D�h���[");
        restDeck= restDeck - 1;
        field.DrawDeck(fieldcardNumber, decks[restDeck]);
        yield return new WaitForSeconds(SPEED_DRAWFIELD);
    }

    /// <summary>
    /// �h���[�����B��D�����D�ɏ���n���B�R�D�̖������O�̎��͎R�D����蒼���B
    /// </summary>
    public int DrawOne()
    {
        if (restDeck < 1)
        {
            StartCoroutine(MakePlayerDeck());
        }
        soundManager.PlayDraw();
        restDeck--;
        return decks[restDeck];
    }

    /// <summary>
    /// �f�b�L�̂��ׂẴJ�[�h���ꂼ��ɘA�Ԃ̐��l������U��B
    /// </summary>
    private void MakeDeckSerialNumber()
    {
        for (int i = 0; i < NUMBER_OF_DECK; i++)
        {
            decks[i] = i + 1;
        }
    }

    /// <summary>
    /// �������g���ăf�b�L���V���b�t������B
    /// </summary>
    private void ShuffleDeck()
    {
        for (var i = restDeck - 1; i >= 0; i--)
        {
            var j = Random.Range(0, NUMBER_OF_DECK);
            var tmp = decks[i];
            decks[i] = decks[j];
            decks[j] = tmp;
        }
    }

    /// <summary>
    /// SP�Q�[�W��MAX�ɂȂ������ɎR�D�̉摜��_�ł�����B
    /// </summary>
    public void AnimateDeckFlash()
    {
        deckAnimation.Kill();
        deckImg.transform.DOScale(new Vector3(1, 1f, 0f), 0f);
        deckImg.DOFade(1, 0);
        deckAnimation = DOTween.Sequence()
            .Append(deckImg.transform.DOScale(new Vector3(1.2f, 1.2f, 0f), 1f).SetLoops(-1, LoopType.Restart))
            .Join(deckImg.DOFade(0, 1).SetLoops(-1, LoopType.Restart));
    }

    public void StopAnimate()
    {
        deckAnimation.Kill();
        deckImg.transform.DOScale(new Vector3(1, 1f, 0f), 0f);
        deckImg.DOFade(1, 0);
    }

}
