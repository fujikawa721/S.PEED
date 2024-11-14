using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Deck : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] FieldController field;
    [SerializeField] TextMeshProUGUI restDecksText;
    [SerializeField] Player player;
    [SerializeField] PlayerHandController playerHandController;


    AudioSource audioSource;
    public AudioClip deckMax;


    private const int NUMBER_OF_DECK = 26;
    private const int NUMBER_OF_HAND = 5;
    private const float SPEED_DRAWHAND = 0.1f;
    private const float SPEED_DRAWFIELD = 0.2f;

    private int[] decks = new int[NUMBER_OF_DECK];
    private int restDeck = NUMBER_OF_DECK;

    public void Update()
    {
        restDecksText.text = @$"{restDeck}";
    }


    public IEnumerator ReadyGame()
    {
        audioSource = GetComponent<AudioSource>();
        yield return null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(@$"SP�|�C���g{player.nowSpPoint}");

        if (player.canDoSpecial == true)
        {
            Debug.Log(@$"�X�y�V���������I");
            StartCoroutine(player.DoSpecial());
            transform.position -= Vector3.up * 0.3f;
            transform.localScale = Vector3.one * 1.0f;
        }
        else
        {
            Debug.Log(@$"�X�y�V�����Q�[�W�����܂��Ă܂���");
        }
    }

    public void OnPointerEnter()
    {
        if (player.canDoSpecial == true)
        {
            transform.position += Vector3.up * 0.3f;
            transform.localScale = Vector3.one * 1.1f;
        }
    }

    public void OnPointerExit()
    {
        if (player.canDoSpecial == true)
        {
            transform.position -= Vector3.up * 0.3f;
            transform.localScale = Vector3.one * 1.0f;
        }

    }


    public IEnumerator MakePlayerDeck()
    {
        PlaySeDeckMax();
        restDeck = NUMBER_OF_DECK;
        MakeDeckSerialNumber();
        ShuffleDeck();
        yield return null;
    }

    /// <summary>
    /// �f�b�L�̂��ׂẴJ�[�h���ꂼ��ɘA�Ԃ̐��l������U��B
    /// </summary>
    private void MakeDeckSerialNumber()
    {
        for (int i = 0; i < NUMBER_OF_DECK; i++) {
            decks[i] = i+1;
        }
    }

    /// <summary>
    /// �������g���ăf�b�L���V���b�t������B
    /// </summary>
    private void ShuffleDeck()
    {
        for (var i = restDeck-1; i >= 0; i--)
        {
            var j = Random.Range(0, NUMBER_OF_DECK);
            var tmp = decks[i];
            decks[i] = decks[j];
            decks[j] = tmp;           
        }
    }

    /// <summary>
    /// �Q�[���J�n����GameController����Ăяo������D���������B
    /// </summary>
    public IEnumerator MakePlayerHand()
    {
        for (var i = 0; i < NUMBER_OF_HAND; i++)
        {
            restDeck--;
            playerHandController.DrawHand(decks[restDeck]);
            yield return new WaitForSeconds(SPEED_DRAWHAND);
        }
        yield return null;
        //yield return new WaitForSeconds(2.0f);
    }

    public IEnumerator MakeField(int fieldcardNumber)
    {
        if (restDeck < 1)
        {
            yield return StartCoroutine(MakePlayerDeck());
        }
        restDeck--;
        field.DrawDeck(fieldcardNumber, decks[restDeck]);
        yield return new WaitForSeconds(SPEED_DRAWFIELD);
    }


    public IEnumerator DrawOne()
    {
        if (restDeck < 1)
        {
            
            yield return StartCoroutine(MakePlayerDeck());
        }
        restDeck--;
        playerHandController.DrawHand(decks[restDeck]);
        yield return new WaitForSeconds(SPEED_DRAWHAND);
    
    }
    

    public void PlaySeDeckMax()
    {
        audioSource.clip = deckMax;
        audioSource.Play();
    }


}
