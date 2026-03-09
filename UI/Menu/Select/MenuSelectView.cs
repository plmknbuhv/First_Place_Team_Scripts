using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Menu
{
    public class MenuSelectView : PanelView
    {
        [SerializeField] private Animator selectPanelAnimator;

        public Action OnStartPressed;
        public Action OnSettingPressed;
        public Action OnQuitPressed;
        public override void Enter()
        {
            selectPanelAnimator.Play("SelectEnter");
        }

        public override void Exit()
        {
            selectPanelAnimator.Play("SelectExit");
        }

        public void OnStart() => OnStartPressed?.Invoke();
        public void OnSetting() => OnSettingPressed?.Invoke();
        public void OnQuit() => OnQuitPressed?.Invoke();
    }
}