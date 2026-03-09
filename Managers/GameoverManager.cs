using Code.Core;
using Code.Core.EventSystem;
using Code.Events;
using Code.Managers;
using Code.UI;
using DG.Tweening;
using System;
using System.Collections;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;

public class GameoverManager : MonoSingleton<GameoverManager>
{
    [SerializeField] private GameEventChannelSO uiChannel;
    [SerializeField] private TMP_Text minusDayCountText;

    [SerializeField] private int ClearWaveCount;

    private RectTransform countRect;
    private int _minusCount;

    private void Awake()
    {
        countRect = minusDayCountText.GetComponent<RectTransform>();
        uiChannel.AddListener<WaveEvent>(HandleWaveEnd);
    }

    private void OnDestroy()
    {
        uiChannel.RemoveListener<WaveEvent>(HandleWaveEnd);
    }

    private void HandleWaveEnd(WaveEvent evt)
    {
        if (evt.IsStart == true)
            return;

        if (ShopManager.Instance.Coin >= 0)
        {
            _minusCount = 0;
            return;
        }

        _minusCount++;

        SetMinusText(_minusCount);
        if(_minusCount > 3)
        {
            StartCoroutine(GameOver());
            return;
        }

        if (evt.Wave > ClearWaveCount)
        {
            GameClear();
        }
    }

    private IEnumerator GameOver()
    {
        countRect.DOAnchorPos(Vector3.zero, 2f);
        countRect.DOScale(3f, 2f);
        yield return new WaitForSeconds(3f);
        SceneChangeTransition.Instance.ChangeScene("GameOverScene");
    }

    private void GameClear()
    {
        SceneChangeTransition.Instance.ChangeScene("GameClearScene");
    }

    private void SetMinusText(int day)
    {
        if (day <= 0)
        {
            minusDayCountText.SetText(string.Empty);
        }
        else
        {
            minusDayCountText.SetText($"적자:연속{day}일");
        }
    }
}
