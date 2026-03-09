using Code.Core.EventSystem;
using Code.Events;
using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System;
using Code.Managers;

public class PlayerStatView : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO uiChannel;

    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text coinEffectPrefab;
    [SerializeField] private TMP_Text landTaxText;

    [SerializeField] private int poolSize = 5;
    [SerializeField] private float moveDistance = 40f;
    [SerializeField] private float fadeInTime = 0.2f;
    [SerializeField] private float fadeOutTime = 0.6f;

    private RectTransform _parentTrm;
    private int _lastGold;
    private readonly Queue<TMP_Text> _pool = new();

    private void Awake()
    {
        _parentTrm = coinText.transform.parent.GetComponent<RectTransform>();

        for (int i = 0; i < poolSize; i++)
            CreateEffect().gameObject.SetActive(false);

        uiChannel.AddListener<CoinUpdateEvent>(HandleCashUpdate);
        uiChannel.AddListener<WaveEvent>(HandleWaveTax);
    }

    private void OnDestroy()
    {
        uiChannel.RemoveListener<CoinUpdateEvent>(HandleCashUpdate);
        uiChannel.RemoveListener<WaveEvent>(HandleWaveTax);
    }

    private void HandleWaveTax(WaveEvent evt)
    {
        if (evt.IsStart == false)
            return;

        float targetGold = evt.WaveData.Cost - evt.WaveData.Cost * UpgradeManager.Instance.LandTaxDownBonus;
        landTaxText.SetText($"다음 납부금액:{(int)targetGold}");
    }

    private void HandleCashUpdate(CoinUpdateEvent evt)
    {
        coinText.SetText($"{evt.Coin}G");

        int diff = evt.Coin - _lastGold;
        _lastGold = evt.Coin;

        if (diff != 0)
            PlayEffect(diff);
    }

    private TMP_Text CreateEffect()
    {
        TMP_Text effect = Instantiate(coinEffectPrefab, _parentTrm);
        _pool.Enqueue(effect);
        return effect;
    }

    private TMP_Text GetEffect()
    {
        return _pool.Count > 0 ? _pool.Dequeue() : CreateEffect();
    }

    private void ReturnEffect(TMP_Text effect)
    {
        effect.gameObject.SetActive(false);
        _pool.Enqueue(effect);
    }

    private void PlayEffect(int value)
    {
        TMP_Text effect = GetEffect();
        RectTransform trm = effect.GetComponent<RectTransform>();

        effect.gameObject.SetActive(true);
        effect.SetText($"{value:+#;-#}G");
        effect.color = value > 0 ? Color.green : Color.red;

        Vector2 origin = trm.anchoredPosition;
        float dir = value > 0 ? -1f : 1f;

        trm.anchoredPosition = origin + Vector2.up * moveDistance * dir;
        effect.alpha = 0f;

        Sequence seq = DOTween.Sequence();

        seq.Append(effect.DOFade(1f, fadeInTime));
        seq.Join(trm.DOAnchorPos(origin, fadeInTime));

        seq.AppendInterval(0.1f);

        seq.Append(effect.DOFade(0f, fadeOutTime));
        seq.Join(trm.DOAnchorPos(origin + Vector2.up * moveDistance * -dir, fadeOutTime));

        seq.OnComplete(() => ReturnEffect(effect));
    }
}
