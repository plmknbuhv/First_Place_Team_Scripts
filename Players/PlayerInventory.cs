using Code.Core.EventSystem;
using Code.Entities;
using Code.Events;
using Code.Farms.Plants;
using Code.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Players
{
    public class PlayerInventory : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        
        private Player _player;
        private PlayerRenderer _playerRenderer;
        private Stack<PlantDataSO> _seedStack;
        private int _maxSeedCount => _player.PlayerData.maxSeedCount;

        public Plant HeldPlant { get; private set; }

        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            _playerRenderer = entity.GetCompo<PlayerRenderer>();
                
            _seedStack = new Stack<PlantDataSO>();

            uiChannel.AddListener<AddItemEvent>(HandleItemAdd);
        }

        private void OnDestroy()
        {
            uiChannel.RemoveListener<AddItemEvent>(HandleItemAdd);
        }
        private void HandleItemAdd(AddItemEvent evt)
        {
            PlantDataSO data = evt.Item;
            if (/*ShopManager.Instance.Coin < data.buyPrice || */data == null)
            {
                uiChannel.RaiseEvent(UIEvents.WarningPopupEvent.Init("오류가 발생했습니다"));
                return;
            }
            if (PushSeed(data) == false)
                return;

            ShopManager.Instance.AddCoin(-evt.Item.buyPrice);
        }

        private void LateUpdate()
        {
            FollowPlant();
        }

        private void FollowPlant()
        {
            if (HeldPlant != null)
            {
                // 이동 부분
                Vector3 offset = new Vector3(
                    0.26f * _playerRenderer.FacingDirection,
                    0.4f);
                
                HeldPlant.transform.position = _player.transform.position + offset;
                
                // 회전 부분
                HeldPlant.transform.localScale = new Vector3(_playerRenderer.FacingDirection, 1, 1);
                HeldPlant.transform.eulerAngles = new Vector3(0, 0, 23 * _playerRenderer.FacingDirection);
            }
        }

        private void SeedUpdated()
        {
            print($"currentSeedCount:{_seedStack.Count}");
            uiChannel.RaiseEvent(UIEvents.InventoryUpdateEvent.Init(_seedStack));
        }

        public bool PushSeed(PlantDataSO plantData)
        {
            if (_maxSeedCount > _seedStack.Count)
            {
                _seedStack.Push(plantData);
                SeedUpdated();
                return true;
            }
            uiChannel.RaiseEvent(UIEvents.WarningPopupEvent.Init("가방이 가득 찼습니다!"));
            return false;
        }

        public PlantDataSO PopSeed()
        {
            if (_seedStack.Count > 0)
            {
                PlantDataSO data = _seedStack.Pop();
                SeedUpdated();
                return data;
            }
            return null;
        }

        public void TakePlant(Plant plant)
        {
            HeldPlant = plant;
        }
        
        public void RemovePlant()
        {
            Destroy(HeldPlant.gameObject);
            HeldPlant = null;
        }

#if UNITY_EDITOR
        [SerializeField] private PlantDataSO testData;

        [ContextMenu("PushSeed")]
        public void TestPush()
        {
            PushSeed(testData);
        }
#endif
    }
}