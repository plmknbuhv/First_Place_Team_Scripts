using DG.Tweening;
using UnityEngine;

namespace Code.Turrets.Durian
{
    public class DurianAttack : TurretAttack
    {
        [SerializeField] private ParticleSystem particlePrefab;
        
        protected override void Update()
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

                    _turret.transform.DOMove(_target.transform.position - (Vector3.right * 1.5f), 0.45f).OnComplete(() =>
                    {
                        const string attack = "ATTACK";
                        _turret.ChangeAnimation(attack);
                        _trigger.OnAttackEndTrigger += HandleAttackEnd;
                    });
                }
            }

            _fireTimer += Time.deltaTime;
        }
        
        protected override void HandleAttackTrigger()
        {
            Instantiate(particlePrefab, _turret.transform.position, Quaternion.identity).Play();
            _fireCount++;
            
            if (_turret.IsDead) return;
            if (_target == null) return;
            
            Enemy.Enemy enemy = _target.GetComponent<Enemy.Enemy>();
            if (enemy != null)
            {
                enemy.HandleHit(200);
            }
        }
        
        protected override void HandleAttackEnd()
        {
            _turret.IsDead = true;
            _trigger.OnAttackEndTrigger -= HandleAttackEnd;

            _turret.HandleDeadTrigger();
            
            Destroy(_turret.gameObject);
        }
    }
}