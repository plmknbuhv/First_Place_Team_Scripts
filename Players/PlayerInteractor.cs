using System;
using Code.Entities;
using Code.Farms;
using Code.Farms.Plants;
using Code.Managers;
using Code.Turrets;
using DG.Tweening;
using UnityEngine;

namespace Code.Players
{
    public class PlayerInteractor : MonoBehaviour, IEntityComponent
    {
        private Player _player;
        private StoreChecker _storeChecker;
        private FarmChecker _farmChecker;
        private PlayerInventory _inventory;
        private PlayerMovement _movement;
        private PlayerAnimator _animator;
        private EntityVFX _entityVfx;
        
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            _storeChecker = entity.GetCompo<StoreChecker>();
            _farmChecker = entity.GetCompo<FarmChecker>();
            _inventory = entity.GetCompo<PlayerInventory>();
            _animator = entity.GetCompo<PlayerAnimator>();
            _movement = entity.GetCompo<PlayerMovement>();
            _entityVfx = entity.GetCompo<EntityVFX>();
            
            _player.PlayerInput.OnInteractPressed += HandleInteract;
        }

        private void Update()
        {
            if (_inventory.HeldPlant)
            {
                if (PotManager.Instance.CheckPotPoint(
                        _player.transform.position, out var trm))
                {
                    PotManager.Instance.ShowPreview(trm, _inventory.HeldPlant.PlantData);
                }
                else
                    PotManager.Instance.OffPreview();
            }
        }

        private void OnDestroy()
        {
            _player.PlayerInput.OnInteractPressed -= HandleInteract;
        }

        private void HandleInteract()
        {
            if (_storeChecker.IsCanInteract) // 상점이 인터랙션 가능 할 경우
            {
                if (_inventory.HeldPlant)
                {
                    _entityVfx.PlayVFX("CoinVFX", transform.position, Quaternion.identity);
                    ShopManager.Instance.SellItem(_inventory.HeldPlant.PlantData.sellValue);
                    _inventory.RemovePlant();
                }
                else // 상점 열기
                    ShopManager.Instance.OpenShop();
            }
            else if (_farmChecker.IsCanInteract) // 밭이 인터랙션 가능 할 경우
            {
                if (_inventory.HeldPlant) return; // 식물 들고있으면 안됨
                
                if (_farmChecker.TargetCell.FarmType == FarmType.None)
                    PlantFarm();
                else if (_farmChecker.TargetCell.FarmType == FarmType.Grown)
                    HarvestFarm();
            }
            else if (_inventory.HeldPlant)
            {
                if (PotManager.Instance.CheckPotPoint(
                        _player.transform.position, out var trm))
                {
                    PotManager.Instance.InstallPlant(trm);
                    PotManager.Instance.OffPreview();
                    Turret turret = Instantiate(_inventory.HeldPlant.PlantData.turretPrefab);
                    turret.SetupTurret(_inventory.HeldPlant.PlantData, trm);
                    turret.transform.position = trm.position;
                    
                    _inventory.RemovePlant();
                }
            }
        }

        private void PlantFarm()
        {
            PlantDataSO plantData = _inventory.PopSeed();
            if (plantData != null) // 지금 심을 수 있는 씨앗이 있으면
            {
                const string plant = "Plant";
                
                _animator.AnimateTrigger(plant);
                _farmChecker.TargetCell.PlacePlant(plantData);
                _movement.SetCanMove(false);
                
                DOVirtual.DelayedCall(0.3f, () =>
                {
                    _movement.SetCanMove(true);
                });
            }
        }
        
        private void HarvestFarm()
        {
            Plant plant = _farmChecker.TargetCell.Harvest();
            _inventory.TakePlant(plant);
        }
    }
}