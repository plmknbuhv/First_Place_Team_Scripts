using UnityEngine;
using UnityEngine.UI;

public class GlobalButtonSound : MonoBehaviour
{
    [Header("Audio Clip")]
    [SerializeField] private AudioClip clickSound;

    [Header("Target Buttons")]
    [SerializeField] private Button[] buttons;

    private void Start()
    {
        foreach (var btn in buttons)
        {
            if (btn != null)
            {
                btn.onClick.AddListener(() => PlaySound());
            }
        }
    }

    private void PlaySound()
    {
        SoundManager.PlaySFXStatic(clickSound);
    }
}