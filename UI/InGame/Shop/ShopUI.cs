using Code.Core.EventSystem;
using Code.Events;
using Code.Managers;
using DG.Tweening;
using UnityEngine;

namespace Code.UI.InGame
{
    public class ShopUI : PanelControlModel
    {
        [SerializeField] private GameEventChannelSO uiEvent;

        private RectTransform _rect;
        protected override void Start()
        {
            base.Start();
            ChangeState(states[0].StateName);
            uiEvent.AddListener<ShopUIEvent>(HandleShopUI);

            _rect = GetComponent<RectTransform>();
        }

        private void OnDestroy()
        {
            uiEvent.RemoveListener<ShopUIEvent>(HandleShopUI);
        }

        private void HandleShopUI(ShopUIEvent evt)
        {
            SetShopEnable(evt.Enabled);
        }

        public void SetShopEnable(bool value)
        {
            _rect.DOAnchorPos(value ? Vector3.zero : Vector3.up * 1000, 0.5f).SetUpdate(true);
        }

        public void SetPanel(string name)
        {
            ChangeState(name);
        }

        public void CloseShop()
        {
            ShopManager.Instance.CloseShop();
        }
    }
}