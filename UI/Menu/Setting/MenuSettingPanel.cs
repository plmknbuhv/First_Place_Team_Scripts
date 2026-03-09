using UnityEngine;

namespace Code.UI.Menu
{
    public class MenuSettingPanel : PanelState
    {
        [SerializeField] private AudioClip clickSFX;
        private void Awake()
        {
            SetStateName(GetType().Name);
        }
        public override void Enter()
        {
            base.Enter();
            AddListeners(view as MenuSettingView);
        }

        public override void Exit()
        {
            base.Exit();
            RemoveListeners(view as MenuSettingView);
        }

        public override void FixedUpdate()
        {
        }

        private void AddListeners(MenuSettingView view)
        {
            view.OnExitPressed += ExitSetting;
        }

        private void RemoveListeners(MenuSettingView view)
        {
            view.OnExitPressed -= ExitSetting;
        }

        private void ExitSetting()
        {
            SoundManager.Instance.PlayUISFX(clickSFX);
            controller.ChangeState("MenuSelectPanel");
        }
    }
}