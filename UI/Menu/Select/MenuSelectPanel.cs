using UnityEngine;

namespace Code.UI.Menu
{
    public class MenuSelectPanel : PanelState
    {
        [SerializeField] private AudioClip clickSFX;
        private void Awake()
        {
            SetStateName(GetType().Name);
        }
        public override void Enter()
        {
            base.Enter();
            AddListeners(view as MenuSelectView);
        }

        public override void Exit()
        {
            base.Exit();
            RemoveListeners(view as MenuSelectView);
        }

        public override void FixedUpdate()
        {
        }

        private void AddListeners(MenuSelectView view)
        {
            view.OnStartPressed += StartGame;
            view.OnSettingPressed += SettingPanel;
            view.OnQuitPressed += QuitGame;
        }

        private void RemoveListeners(MenuSelectView view)
        {
            view.OnStartPressed -= StartGame;
            view.OnSettingPressed -= SettingPanel;
            view.OnQuitPressed -= QuitGame;
        }

        private void StartGame()
        {
            PlayClickSound();
            SceneChangeTransition.Instance.ChangeScene("GameScene");
        }
        private void SettingPanel()
        {
            PlayClickSound();
            controller.ChangeState("MenuSettingPanel");
        }
        private void QuitGame()
        {
            PlayClickSound();
            Application.Quit();
        }

        private void PlayClickSound()
        {
            SoundManager.Instance.PlayUISFX(clickSFX);
        }
    }
}
