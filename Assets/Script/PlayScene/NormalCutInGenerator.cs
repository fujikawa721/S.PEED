using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


public class NormalCutInGenerator : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;

    //イベントカットイン用のオブジェクト
    [SerializeField] private GameObject eventCutInObject;
    [SerializeField] private Image eventCutInImg;
    [SerializeField] TextMeshProUGUI eventText;
    [SerializeField] TextMeshProUGUI effectText;
    private Tween eventCutInFade;
    private Sequence eventCutInSequence;

    //ノーマルカットイン用のオブジェクト
    [SerializeField] private GameObject normalCutInObject;
    [SerializeField] private Image normalCutInBase;
    [SerializeField] private Image normalCutInCharacter;
    private Tween normalCutInFade;
    private Sequence normalCutInSequence;

    private const float CUTIN_SPEED = 0.5f;
    private const float CUTIN_TIME = 2.5f;

    /// <summary>
    /// カットイン用のキャラクター画像をScriptableObjectから取得する
    /// </summary>
    /// <param name="eventCutInImage">イベントカットイン用のキャラ画像（上半身）</param>
    /// <param name="playerCharacter">ノーマルカットイン用のキャラ画像</param>
    public void ReadyGame(Sprite eventCutInImage, Sprite playerCharacter)
    {
        eventCutInObject.SetActive(false);
        eventCutInImg.DOFillAmount(0f, 0);
        eventCutInImg.sprite = eventCutInImage;

        normalCutInObject.SetActive(false);

        normalCutInCharacter.sprite = playerCharacter;
    }

    /// <summary>
    /// キャラの画像＋イベント内容のカットイン
    /// </summary>
    /// <param name="eventName">【TRIGGERSKILL】などイベントの名前</param>
    /// <param name="eventEffect">【HPを回復】などイベントの内容</param>
    /// <returns></returns>
    public IEnumerator ActiveEventCutIn(string eventName,string eventEffect)
    {
        eventText.text = @$"{eventName}";
        effectText.text = @$"{eventEffect}";

        soundManager.PlayEventCutIn();
        eventCutInObject.SetActive(true);
        StartCoroutine(AnimateEventCutIn());
        yield return null;
    }

    /// <summary>
    /// イベントカットインを移動させ設定時間秒後に非表示にする。実行する前に前のカットインの処理を一度Killする。
    /// </summary>
    /// <returns></returns>
    public IEnumerator AnimateEventCutIn()
    {
        if (eventCutInFade != null && eventCutInFade.IsActive())
        {
            eventCutInFade.Kill();
        }

        if (eventCutInSequence == null)
        {
            eventCutInSequence = DOTween.Sequence()
                .Append(eventCutInObject.transform.DOLocalMoveX(-1000, CUTIN_SPEED).From())
                .SetAutoKill(false)
                .Pause();
            eventCutInSequence.Play();
        }
        else
        {
            eventCutInObject.transform.localPosition = new Vector3(0, eventCutInObject.transform.localPosition.y, eventCutInObject.transform.transform.localPosition.z);
        }
        eventCutInSequence.Restart();

        eventCutInFade = DOVirtual.DelayedCall(CUTIN_TIME, () =>
        {
            eventCutInObject.SetActive(false); 
        });
        yield return null;
    }

    public void ActiveNormalCutIn()
    {
        normalCutInObject.SetActive(true);
        StartCoroutine(AnimateNormalCutIn());
    }

    /// <summary>
    /// 連続で呼び出された場合に意図しない位置でオブジェクトが停止してしまうため、前処理が残っていた場合Killする。
    /// 初回のみシーケンスを生成し、2回目以降はポーズしていたシーケンスをリスタートさせる。
    /// </summary>
    /// <returns></returns>
    public IEnumerator AnimateNormalCutIn()
    {
        
        if (normalCutInFade != null && normalCutInFade.IsActive())
        {
            normalCutInFade.Kill();
        }

        if (normalCutInSequence == null)
        {
            normalCutInSequence = DOTween.Sequence()
                .Append(normalCutInBase.transform.DOScaleY(0, CUTIN_SPEED).From())
                .Append(normalCutInCharacter.transform.DOLocalMoveX(-400, CUTIN_SPEED).From())
                .SetAutoKill(false) 
                .Pause();           
            normalCutInSequence.Play();
        }
        else
        {
            normalCutInBase.transform.localPosition = new Vector3(0, normalCutInBase.transform.localPosition.y, normalCutInBase.transform.localPosition.z);
            normalCutInCharacter.transform.localPosition = new Vector3(0, normalCutInCharacter.transform.localPosition.y, normalCutInCharacter.transform.localPosition.z);
        }

        normalCutInSequence.Restart();
        normalCutInFade = DOVirtual.DelayedCall(CUTIN_TIME, () =>
        {
            normalCutInObject.SetActive(false);
        });


        yield return null;
    }

}
