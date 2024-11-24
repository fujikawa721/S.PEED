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

    //山札の最大枚数は【52枚】
    private const int NUMBER_OF_DECK = 52;
    
    //山札から場札にカードを置いた際に【0.2f】後続処理を待つ。
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
        Debug.Log(@$"SPポイント{player.nowSpPoint}");
        //transform.position -= Vector3.up * 0.3f;
        transform.localScale = Vector3.one * 1.0f;

        if (player.canDoSpecial == true)
        {
            Debug.Log(@$"スペシャル発動！");
            StartCoroutine(player.DoSpecial());
            
        }
        else
        {
            Debug.Log(@$"スペシャルゲージが溜まってません{player.canDoSpecial}");
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
    /// デッキ枚数が０になった時、新しく山札を作り直す。また、GameControllerがゲーム開始時に呼び出す。
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
    /// 山札の先頭のカードを場札に置く。ゲーム開始時、仕切り直し時にGameControllerから呼び出される。
    /// </summary>
    public IEnumerator MakeField(int fieldcardNumber)
    {
        if (restDeck < 1)
        {
            yield return StartCoroutine(MakePlayerDeck());
        }
        Debug.Log("山札ドロー");
        restDeck= restDeck - 1;
        field.DrawDeck(fieldcardNumber, decks[restDeck]);
        yield return new WaitForSeconds(SPEED_DRAWFIELD);
    }

    /// <summary>
    /// ドロー処理。場札から手札に情報を渡す。山札の枚数が０の時は山札を作り直す。
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
    /// デッキのすべてのカードそれぞれに連番の数値を割り振る。
    /// </summary>
    private void MakeDeckSerialNumber()
    {
        for (int i = 0; i < NUMBER_OF_DECK; i++)
        {
            decks[i] = i + 1;
        }
    }

    /// <summary>
    /// 乱数を使ってデッキをシャッフルする。
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
    /// SPゲージがMAXになった時に山札の画像を点滅させる。
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
