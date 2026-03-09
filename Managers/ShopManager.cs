using Code.Core;
using Code.Core.EventSystem;
using Code.Events;
using Code.Farms.Plants;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Managers
{
    public class ShopManager : MonoSingleton<ShopManager>
    {
        [SerializeField] private GameEventChannelSO uiChannel;

        [SerializeField] private List<PlantDataSO> sellGoods;
        public int Coin { get; private set; }
        public bool IsActive { get; private set; }

        private float _sellMultiplier;

        private void Awake()
        {
            uiChannel.AddListener<WaveEvent>(HandleLandTaxEvent);
            uiChannel.AddListener<UpgradeCallEvent>(HandleUpgradeEvent);
        }

        private void Start()
        {
            AddCoin(300);
        }
        private void OnDestroy()
        {
            uiChannel.RemoveListener<WaveEvent>(HandleLandTaxEvent);
            uiChannel.RemoveListener<UpgradeCallEvent>(HandleUpgradeEvent);
        }

        private void HandleUpgradeEvent(UpgradeCallEvent evt)
        {
            _sellMultiplier = UpgradeManager.Instance.SellPriceBonus;
        }

        private void HandleLandTaxEvent(WaveEvent evt)
        {
            if (evt.IsStart)
                return;

            float targetGold = evt.WaveData.Cost + evt.WaveData.Cost * UpgradeManager.Instance.LandTaxDownBonus;
            AddCoin(-(int)targetGold);
        }

        public void OpenShop()
        {
            uiChannel.RaiseEvent(UIEvents.ShopUIEvent.Init(true));
            IsActive = true;
        }

        public void CloseShop()
        {
            uiChannel.RaiseEvent(UIEvents.ShopUIEvent.Init(false));
            IsActive = false;
        }
        public List<PlantDataSO> GetGoods()
        {
            return sellGoods;
        }

        public void PurchaseItem(PlantDataSO data)
        {
            uiChannel.RaiseEvent(UIEvents.AddItemEvent.Init(data));
        }
        public void SellItem(int price)
        {
            price += UpgradeManager.Instance.SellPriceBonus;
            AddCoin(price);
        }
        public void AddCoin(int amount)
        {
            Coin += amount;
            uiChannel.RaiseEvent(UIEvents.CoinUpdateEvent.Init(Coin));
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                AddCoin(100);
            }
        }
#endif
    }
}
