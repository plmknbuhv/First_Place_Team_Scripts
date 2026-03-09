using System;
using UnityEngine;

namespace Code.UI.InGame
{
    public class EscapeView : MonoBehaviour
    {
        [SerializeField] private Animator viewAnimator;

        public Action OnContinuePressed;
        public Action OnExitPressed;

        public void Open()
        {
            viewAnimator.Play("EnableEscape");  
        }
        public void Close()
        {
            viewAnimator.Play("DisableEscape");
        }

        public void Continue() => OnContinuePressed?.Invoke();
        public void Exit() => OnExitPressed?.Invoke();
    }
}