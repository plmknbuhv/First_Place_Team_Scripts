using Code.Entities;
using Code.Turrets.Bullets;
using UnityEngine;

namespace Code.Turrets
{
    public class TurretAttack : MonoBehaviour, IEntityComponent
    {
        [SerializeField] protected Bullet bulletPrefab;
        [SerializeField] protected LayerMask whatIsEnemy;

        protected EntityAnimatorTrigger _trigger;
        protected Turret _turret;
        protected Transform _target;
        protected float _fireTimer;
        protected int _fireCount;

        public Vector2 checkBoxPos;
        public Vector2 checkBoxRange;
        
        public void Initialize(Entity entity)
        {
            _turret = entity as Turret;
            _trigger = entity.GetCompo<EntityAnimatorTrigger>();

            _trigger.OnAttackTrigger += HandleAttackTrigger;
        }

        private void OnDestroy()
        {
            _trigger.OnAttackTrigger -= HandleAttackTrigger;
        }

        public void AttackStart()
        {
            _fireTimer = 0;
            _fireCount = 0;
        }
        
        protected virtual void Update()
        {
            if (_turret.IsDead) return;
            
            if (ShouldFindNewTarget())
            {
                FindTarget();
            }

            if (_target != null)
            {
                if (_fireTimer >= _turret.PlantData.attackDelay)
                {
                    _fireTimer = 0;
                    
                    const string attack = "ATTACK";
                    _turret.ChangeAnimation(attack);
                    _trigger.OnAttackEndTrigger += HandleAttackEnd;
                }
            }

            _fireTimer += Time.deltaTime;
        }
        
        protected bool ShouldFindNewTarget()
        {
            if (_target == null) return true;

            Vector2 pos = (Vector2)_turret.transform.position + checkBoxPos;
            Collider2D[] hits = Physics2D.OverlapBoxAll(pos, checkBoxRange, 0f, whatIsEnemy); 

            bool isCanOverlap = false;
            
            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject == _target.gameObject)
                {
                    isCanOverlap = true;
                    break;
                }
            }
            
            if (isCanOverlap == false) return true;

            if (((1 << _target.gameObject.layer) & whatIsEnemy) == 0) return true;

            return false;
        }

        protected void FindTarget()
        {
            Vector2 pos = (Vector2)_turret.transform.position + checkBoxPos;
            Collider2D[] hits = Physics2D.OverlapBoxAll(pos, checkBoxRange, 0f, whatIsEnemy); 

            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (var hit in hits)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = hit.gameObject;
                }
            }

            if (nearestEnemy != null)
                _target = nearestEnemy.transform;
            else
                _target = null;
        }
        
        protected virtual void HandleAttackTrigger()
        {
            if (_turret.IsDead) return;
            if (bulletPrefab == null || _target == null) return;
            
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.Initialize(_target);

            _fireCount++;
        }
        
        protected virtual void HandleAttackEnd()
        {
            if (_fireCount >= _turret.PlantData.attackCount)
            {
                _turret.IsDead = true;
                const string dead = "DEAD";
                _turret.ChangeAnimation(dead);
                _turret.OnDeadEvent?.Invoke();
            }
            else
            {
                Debug.Log("ddd");
                const string idle = "IDLE";
                _turret.ChangeAnimation(idle);
            }
            _trigger.OnAttackEndTrigger -= HandleAttackEnd;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube((Vector2)transform.position + checkBoxPos, checkBoxRange);
        }
    }
}