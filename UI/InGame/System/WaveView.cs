using Code.Core.EventSystem;
using Code.Events;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class WaveView : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO uiChannel;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private float highlightHoldTime = 3f;

    private RectTransform textRect;

    private void Awake()
    {
        textRect = waveText.GetComponent<RectTransform>();

        uiChannel.AddListener<WaveEvent>(HandleWaveUpdate);

        //SetWave(1);
    }

    private void OnDestroy()
    {
        uiChannel.RemoveListener<WaveEvent>(HandleWaveUpdate);
    }

    private void HandleWaveUpdate(WaveEvent evt)
    {
        if (evt.IsStart == false)
        {
            SetWaveText("¿Ï·á");
            return;
        }

        SetWaveText($"{evt.Wave}ÀÏ");
    }
    public void SetWaveText(string msg)
    {
        waveText.SetText(msg);
        StartCoroutine(HightlightText());
    }

    private IEnumerator HightlightText()
    {
        textRect.DOScale(2.3f, 1.2f);
        textRect.DOAnchorPosY(-150f, 1.2f);
        yield return new WaitForSeconds(highlightHoldTime);

        textRect.DOScale(1f, 1.2f);
        textRect.DOAnchorPosY(0f, 1.2f);
    }
}
