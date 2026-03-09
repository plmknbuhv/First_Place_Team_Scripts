using System;
using UnityEngine;

namespace Code.UI.Menu
{
    public class MenuSettingView : PanelView
    {
        [SerializeField] private Animator selectPanelAnimator;

        public Action OnExitPressed;
        public override void Enter()
        {
            selectPanelAnimator.Play("SettingEnter");
        }

        public override void Exit()
        {
            selectPanelAnimator.Play("SettingExit");
        }

        public void OnExit() => OnExitPressed?.Invoke();
    }
}