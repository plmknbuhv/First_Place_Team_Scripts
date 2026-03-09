using UnityEngine;

namespace Code.Turrets.Bullets
{
    [CreateAssetMenu(fileName = "BulletDataSO", menuName = "SO/Bullet/BulletDataSO", order = 0)]
    public class BulletDataSO : ScriptableObject
    {
        public string bulletName;
        public BulletType bulletType;
        public float bulletSpeed;
        public int bulletDamage;
    }
}
