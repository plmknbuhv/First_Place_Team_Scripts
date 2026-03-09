using Code.Core.EventSystem;
using Code.Farms.Plants;
using System.Collections.Generic;

namespace Code.Events
{
    public class GameEvents
    {
        public static readonly UpgradeCallEvent UpgradeCallEvent = new();
    }

    public class UpgradeCallEvent : GameEvent
    {
        public UpgradeCallEvent Init()
        {
            return this;
        }
    }

    public class UIEvents
    {
        public static readonly WaveEvent WaveEvent = new();
        public static readonly CoinUpdateEvent CoinUpdateEvent = new();
        public static readonly AddItemEvent AddItemEvent = new();
        public static readonly InventoryUpdateEvent InventoryUpdateEvent = new();
        public static readonly ShopUIEvent ShopUIEvent = new();
        public static readonly WarningPopupEvent WarningPopupEvent = new();
    }

    public class WaveEvent : GameEvent
    {
        public int Wave { get; private set; }
        public bool IsStart { get; private set; }
        public EnemyWaveDataSO WaveData { get; private set; }
        public WaveEvent Init(int wave,EnemyWaveDataSO waveData,bool isStart)
        {
            IsStart = isStart;
            WaveData = waveData;
            Wave = wave;
            return this;
        }
    }
    public class CoinUpdateEvent : GameEvent
    {
        public int Coin { get; private set; }
        public CoinUpdateEvent Init(int coin)
        {
            Coin = coin;
            return this;
        }
    }

    public class AddItemEvent : GameEvent
    {
        public PlantDataSO Item {get; private set;}
        public AddItemEvent Init(PlantDataSO item)
        {
            Item = item; 
            return this;
        }
    }

    public class InventoryUpdateEvent : GameEvent
    {
        public Stack<PlantDataSO> PlantDatas { get; private set; }
        public InventoryUpdateEvent Init(Stack<PlantDataSO> items)
        {
            PlantDatas = items;
            return this;
        }
    }
    
    public class ShopUIEvent : GameEvent
    {
        public bool Enabled {get; private set;}
        public ShopUIEvent Init(bool value)
        {
            Enabled = value;
            return this;
        }
    }

    public class WarningPopupEvent : GameEvent
    {
        public string Msg {get; private set;}
        public WarningPopupEvent Init(string msg)
        {
            Msg = msg;
            return this;
        }
    }
}
