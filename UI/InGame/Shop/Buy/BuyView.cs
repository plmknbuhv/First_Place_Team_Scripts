using Code.Farms.Plants;
using System.Collections.Generic;
using UnityEngine;

namespace Code.UI.InGame
{
    public class BuyView : PanelView
    {
        [Header("UI")]
        [SerializeField] private ContentsContainer container;
        [SerializeField] private InformationSetter infoSetter;

        [Header("Pool")]
        [SerializeField] private GameObject itemButtonPrefab;
        [SerializeField] private Transform poolParent;
        [SerializeField] private int poolCount = 5;

        private readonly List<ItemSelectButton> itemButtons = new();

        private void Awake()
        {
            InitPool();
        }

        public override void Enter()
        {
            transform.parent.SetAsLastSibling();
        }

        public override void Exit()
        {
        }

        public void SetItemList(List<PlantDataSO> sellGoods)
        {
            if (sellGoods == null)
            {
                ClearButtons();
                return;
            }

            EnsurePoolSize(sellGoods.Count);

            container.SetContainerSize(sellGoods.Count);

            for (int i = 0; i < sellGoods.Count; i++)
            {
                ItemSelectButton button = itemButtons[i];
                button.gameObject.SetActive(true);
                button.SetItemInfo(infoSetter, sellGoods[i]);
            }

            for (int i = sellGoods.Count; i < itemButtons.Count; i++)
            {
                itemButtons[i].gameObject.SetActive(false);
            }
        }

        private void InitPool()
        {
            for (int i = 0; i < poolCount; i++)
            {
                CreateButton();
            }
        }

        private void EnsurePoolSize(int requiredCount)
        {
            while (itemButtons.Count < requiredCount)
            {
                CreateButton();
            }
        }

        private void CreateButton()
        {
            GameObject item = Instantiate(itemButtonPrefab, poolParent);
            item.SetActive(false);

            ItemSelectButton button = item.GetComponent<ItemSelectButton>();
            if (button == null)
            {
                Debug.LogError("ItemSelectButton 컴포넌트가 없음");
                Destroy(item);
                return;
            }

            itemButtons.Add(button);
        }

        private void ClearButtons()
        {
            foreach (var button in itemButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}
