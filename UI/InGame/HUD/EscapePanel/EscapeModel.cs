using Input;
using UnityEngine;

namespace Code.UI.InGame
{
    public class EscapeModel : MonoBehaviour
    {
        [SerializeField] private EscapeView view;
        [SerializeField] private PlayerInputSO input;

        private bool _enabled;
        private void Awake()
        {
            input.OnEscapePressed += HandleEscape;

            view.OnContinuePressed += HandleContinue;
            view.OnExitPressed += HandleExit;
        }

        private void OnDestroy()
        {
            input.OnEscapePressed -= HandleEscape;
            view.OnContinuePressed -= HandleContinue;
            view.OnExitPressed -= HandleExit;
        }

        private void HandleEscape()
        {
            if(_enabled)
                view.Close();
            else
                view.Open();

            _enabled = !_enabled;
        }

        private void HandleContinue()
        {
            view.Close();
            _enabled = false;
        }
        private void HandleExit()
        {
            print("exit");
            SceneChangeTransition.Instance.ChangeScene("TitleScene");
        }
    }
}
