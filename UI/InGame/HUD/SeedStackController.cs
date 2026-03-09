using Code.Core.EventSystem;
using Code.Events;
using Code.Farms.Plants;
using System.Collections.Generic;
using UnityEngine;

namespace Code.UI.InGame
{
    public class SeedStackController : MonoBehaviour
    {
        [SerializeField] private SeedStackView view;

        [SerializeField] private GameEventChannelSO uiChannel;

        private void Awake()
        {
            uiChannel.AddListener<InventoryUpdateEvent>(HandleStackUpdated);
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<InventoryUpdateEvent>(HandleStackUpdated);
        }

        public void HandleStackUpdated(InventoryUpdateEvent evt)
        {
            Stack<PlantDataSO> stack = evt.PlantDatas;

            int targetCount = stack.Count;
            int currentCount = view.CurrentCount;

            if (targetCount > currentCount)
            {
                var array = stack.ToArray();

                int addCount = targetCount - currentCount;

                for (int i = addCount - 1; i >= 0; i--)
                {
                    view.AddSeed(array[i].seedColor);
                }
            }
            else if (targetCount < currentCount)
            {
                view.RemoveSeeds(currentCount - targetCount);
            }
        }
    }
}