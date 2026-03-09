using Code.Core;
using Code.Core.EventSystem;
using Code.Events;
using Code.UI.InGame;
using UnityEngine;
using System.Collections.Generic;
using Code.Players;

namespace Code.Managers
{
    public class UpgradeManager : MonoSingleton<UpgradeManager>
    {
        [SerializeField] private GameEventChannelSO uiChannel;
        [SerializeField] private PlayerDataSO playerData;

        public float PlayerSpeed { get; private set; } = 2.5f;
        public int PlantCount { get; private set; } = 2;
        public int MaxSeedCount { get; private set; } = 3;

        public int SellPriceBonus { get; private set; }
        public float LandTaxDownBonus { get; private set; } = 0f;
        public float StolenGold { get; private set; } = 0f;

        private const float SPEED_STEP = 0.5f;
        private const float SPEED_MAX = 8f;

        private const int PLANT_MAX = 5;
        private const int SEED_MAX = 18;
        private const int BONUS_INT_MAX = 50;

        private const float BONUS_STEP = 0.1f;
        private const float BONUS_MAX = 0.8f;

        private readonly Dictionary<UpgradeType, int> _levels = new();

        private void Awake()
        {
            foreach (UpgradeType type in System.Enum.GetValues(typeof(UpgradeType)))
                _levels[type] = 0;
        }

        public bool IsMax(UpgradeType type)
        {
            return type switch
            {
                UpgradeType.MoveSpeed => PlayerSpeed >= SPEED_MAX,
                UpgradeType.PotAmount => PlantCount >= PLANT_MAX,
                UpgradeType.MaxSeed => MaxSeedCount >= SEED_MAX,
                UpgradeType.SellBonus => SellPriceBonus >= BONUS_INT_MAX,
                UpgradeType.LandTaxDownBonus => LandTaxDownBonus >= BONUS_MAX,
                UpgradeType.GoldDecrease => StolenGold >= BONUS_MAX,
                _ => true
            };
        }

        public int GetUpgradeCost(UpgradeType type)
        {
            int baseCost = type switch
            {
                UpgradeType.MoveSpeed => 20,
                UpgradeType.PotAmount => 100,
                UpgradeType.MaxSeed => 60,
                UpgradeType.SellBonus => 40,
                UpgradeType.LandTaxDownBonus => 100,
                UpgradeType.GoldDecrease => 75,
                _ => 20
            };

            int level = _levels[type];
            return Mathf.RoundToInt(baseCost * Mathf.Pow(1.5f, level));
        }

        public float GetCurrentValue(UpgradeType type)
        {
            return type switch
            {
                UpgradeType.MoveSpeed => PlayerSpeed,
                UpgradeType.PotAmount => PlantCount,
                UpgradeType.MaxSeed => MaxSeedCount,
                UpgradeType.SellBonus => SellPriceBonus,
                UpgradeType.LandTaxDownBonus => LandTaxDownBonus,
                UpgradeType.GoldDecrease => StolenGold,
                _ => 0
            };
        }

        public float GetNextValue(UpgradeType type)
        {
            if (IsMax(type))
                return GetCurrentValue(type);

            return type switch
            {
                UpgradeType.MoveSpeed =>
                    Mathf.Min(PlayerSpeed + SPEED_STEP, SPEED_MAX),

                UpgradeType.PotAmount =>
                    Mathf.Min(PlantCount + 1, PLANT_MAX),

                UpgradeType.MaxSeed =>
                    Mathf.Min(MaxSeedCount + 1, SEED_MAX),

                UpgradeType.SellBonus =>
                    Mathf.Min(SellPriceBonus + 2, BONUS_INT_MAX),

                UpgradeType.LandTaxDownBonus =>
                    Mathf.Min(LandTaxDownBonus + BONUS_STEP, BONUS_MAX),

                UpgradeType.GoldDecrease =>
                    Mathf.Min(StolenGold + BONUS_STEP, BONUS_MAX),

                _ => GetCurrentValue(type)
            };
        }

        public void Upgrade(UpgradeType type)
        {
            if (IsMax(type))
            {
                uiChannel.RaiseEvent(
                    UIEvents.WarningPopupEvent.Init("최대값입니다")
                );
                return;
            }

            _levels[type]++;

            switch (type)
            {
                case UpgradeType.MoveSpeed:
                    PlayerSpeed = Mathf.Min(PlayerSpeed + SPEED_STEP, SPEED_MAX);
                    break;

                case UpgradeType.PotAmount:
                    PlantCount = Mathf.Min(PlantCount + 1, PLANT_MAX);
                    break;

                case UpgradeType.MaxSeed:
                    MaxSeedCount = Mathf.Min(MaxSeedCount + 1, SEED_MAX);
                    break;

                case UpgradeType.SellBonus:
                    SellPriceBonus = Mathf.Min(SellPriceBonus + 2, BONUS_INT_MAX);
                    break;

                case UpgradeType.LandTaxDownBonus:
                    LandTaxDownBonus = Mathf.Min(LandTaxDownBonus + BONUS_STEP, BONUS_MAX);
                    break;

                case UpgradeType.GoldDecrease:
                    StolenGold = Mathf.Min(StolenGold + BONUS_STEP, BONUS_MAX);
                    break;
            }

            uiChannel.RaiseEvent(GameEvents.UpgradeCallEvent.Init());

            playerData.moveSpeed = PlayerSpeed;
            playerData.maxSeedCount = MaxSeedCount;
        }
    }
}
