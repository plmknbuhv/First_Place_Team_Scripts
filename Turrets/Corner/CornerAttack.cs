using Code.Turrets.Bullets;
using UnityEngine;

namespace Code.Turrets.Corner
{
    public class CornerAttack : TurretAttack
    {
        
        
        protected override void HandleAttackTrigger()
        {
            if (_turret.IsDead) return;
            if (bulletPrefab == null || _target == null) return;

            for (int i = -2; i < 3; i++)
            {
                Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.Initialize(_target, i * 8.0f);
            }

            _fireCount++;
        }
    }
}