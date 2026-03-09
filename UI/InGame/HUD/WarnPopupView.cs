using Code.Core.EventSystem;
using Code.Events;
using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Code.UI.InGame
{
    public class WarnPopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text warnText;
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private float fadeTime = 1.2f;

        private readonly Queue<string> msgQ = new();

        private bool _isRotating;
        private string _currentMessage;
        private Tween _fadeTween;

        private void Awake()
        {
            uiChannel.AddListener<WarningPopupEvent>(HandleWarningMessage);
            warnText.alpha = 0f;
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<WarningPopupEvent>(HandleWarningMessage);
            _fadeTween?.Kill();
        }

        private void HandleWarningMessage(WarningPopupEvent evt)
        {
            if (_currentMessage == evt.Msg)
                return;

            if (msgQ.Contains(evt.Msg))
                return;

            msgQ.Enqueue(evt.Msg);

            if (_isRotating == false)
                ShowNext();
        }

        private void ShowNext()
        {
            if (msgQ.Count <= 0)
                return;

            _isRotating = true;
            _currentMessage = msgQ.Dequeue();

            _fadeTween?.Kill();

            warnText.text = _currentMessage;
            warnText.alpha = 1f;

            _fadeTween = warnText
                .DOFade(0f, fadeTime)
                .OnComplete(() =>
                {
                    _currentMessage = string.Empty;
                    _isRotating = false;
                    ShowNext();
                });
        }
    }
}
