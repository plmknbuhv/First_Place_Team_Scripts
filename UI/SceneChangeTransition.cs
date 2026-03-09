using Code.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace Code.UI
{
    public class SceneChangeTransition : MonoSingleton<SceneChangeTransition>
    {
        [Header("Transition")]
        [SerializeField] private Image transitionImage;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private Ease ease = Ease.OutCubic;

        private RectTransform rect;
        private bool isTransitioning;

        private void Awake()
        {
            DontDestroyOnLoad(transform.parent);

            rect = transitionImage.rectTransform;
            rect.localScale = Vector3.zero;
            transitionImage.gameObject.SetActive(false);
        }

        public void ChangeScene(string sceneName)
        {
            if (isTransitioning) return;
            isTransitioning = true;

            transitionImage.gameObject.SetActive(true);

            rect.localScale = Vector3.zero;
            rect.DOScale(Vector3.one, duration)
                .SetEase(ease)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    SceneManager.sceneLoaded += OnSceneLoaded;
                    SceneManager.LoadSceneAsync(sceneName);
                });
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            rect.DOScale(Vector3.zero, duration)
                .SetEase(ease)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    transitionImage.gameObject.SetActive(false);
                    isTransitioning = false;
                });
        }
    }
}
