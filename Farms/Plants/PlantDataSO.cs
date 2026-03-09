using Code.Turrets;
using UnityEngine;

namespace Code.Farms.Plants
{
    [CreateAssetMenu(fileName = "PlantDataSO", menuName = "SO/PlantData", order = 0)]
    public class PlantDataSO : ScriptableObject
    {
        public Turret turretPrefab;
        
        public string plantName;
        [TextArea(2, 4)] public string description;
        
        public int buyPrice;
        public int sellValue;
        public float glowTime;
        
        public int attackCount;
        public int attackDamage;
        public float attackDelay;
        
        public Sprite plantSprite;
        public Color seedColor;
        public AnimationClip plantClip;
    }
}