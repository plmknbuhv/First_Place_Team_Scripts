using Code.Core.EventSystem;
using Code.Events;
using Code.Managers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame
{
    public class UpgradePanelView : MonoBehaviour
    {
        [field: SerializeField] public UpgradeType Type { get; private set; }

        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private TMP_Text upgradeProgress;
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Button upgradeButton;

        private void Awake()
        {
            upgradeButton.onClick.AddListener(OnClickUpgrade);
            uiChannel.AddListener<CoinUpdateEvent>(HandleMoneyRefresh);
        }
        private void OnDestroy()
        {

            uiChannel.AddListener<CoinUpdateEvent>(HandleMoneyRefresh);
        }
        private void HandleMoneyRefresh(CoinUpdateEvent evt)
        {
            SetUpgradeUI();
        }

        public void SetUpgradeUI()
        {
            var upgradeManager = UpgradeManager.Instance;
            var shopManager = ShopManager.Instance;

            float current = upgradeManager.GetCurrentValue(Type);
            float next = upgradeManager.GetNextValue(Type);

            upgradeProgress.text =
                $"{FormatValue(Type, current)} ¡æ {FormatValue(Type, next)}";

            int cost = upgradeManager.GetUpgradeCost(Type);
            priceText.text = $"{cost} G";

            upgradeButton.interactable = shopManager.Coin >= cost;
        }

        private string FormatValue(UpgradeType type, float value)
        {
            switch (type)
            {
                case UpgradeType.LandTaxDownBonus:
                case UpgradeType.GoldDecrease:
                    return $"{value * 100f:0}%";

                default:
                    return value % 1 == 0
                        ? value.ToString("0")
                        : value.ToString("0.0");
            }
        }

        private void OnClickUpgrade()
        {
            var upgradeManager = UpgradeManager.Instance;
            var shopManager = ShopManager.Instance;

            if (upgradeManager.IsMax(Type))
            {
                return;
            }

            int cost = upgradeManager.GetUpgradeCost(Type);

            if (shopManager.Coin < cost)
            {
                return;
            }

            shopManager.AddCoin(-cost);

            upgradeManager.Upgrade(Type);

            SetUpgradeUI();
        }

    }
}
