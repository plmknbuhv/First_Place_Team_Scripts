using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame
{
    public class SeedStackView : MonoBehaviour
    {
        [SerializeField] private List<Image> seeds;
        [SerializeField] private Sprite seedBag;

        private int currentCount = 0;
        public int CurrentCount => currentCount;

        public void AddSeed(Color seedColor)
        {
            if (currentCount >= seeds.Count)
            {
                Debug.LogWarning("가방이 가득 참");
                return;
            }

            var seed = seeds[currentCount];
            seed.sprite = seedBag;
            seed.color = seedColor;

            if (seed.TryGetComponent(out Animator animator))
                animator.Play("EnableItem");

            currentCount++;
        }

        public void RemoveSeed()
        {
            if (currentCount <= 0)
            {
                Debug.LogWarning("씨앗이 더이상 존재하지 않음");
                return;
            }

            currentCount--;

            var seed = seeds[currentCount];

            if (seed.TryGetComponent(out Animator animator))
                animator.Play("DisableItem");
        }

        public void RemoveSeeds(int count)
        {
            for (int i = 0; i < count; i++)
                RemoveSeed();
        }

        public void Clear()
        {
            currentCount = 0;

            foreach (var seed in seeds)
            {
                seed.sprite = null;
                seed.gameObject.SetActive(false);
            }
        }
    }

}