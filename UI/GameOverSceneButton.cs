using Code.UI;
using UnityEngine;

public class GameOverSceneButton : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneChangeTransition.Instance.ChangeScene("TitleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
