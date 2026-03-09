using Code.Combat;
using Code.Entities;
using System.Collections;
using Code.Turrets.Bullets;
using UnityEngine;

namespace Code.Enemy
{
    public class Enemy : Entity
    {
        public EnemyDataSO data;
        public Health health;

        public bool attackComplete = false;

        // 보스 전투 상태 확인용 플래그
        public bool isBossAttacking = false;
        public float bossAttackInterval = 1.0f;

        public void Start()
        {
            health = GetCompo<Health>();

            if (health != null)
            {
                health.SetUpHealth(data.maxHp);
                health.OnDeadEvent.AddListener(Die);
            }
        }

        public void HandleHit(int damageAmount)
        {
            if (IsDead) return;

            if (health != null)
            {
                health.ApplyDamage(damageAmount);
                OnHitEvent?.Invoke();
            }
        }

        private void OnDestroy()
        {
            if (EnemyManager.Instance != null)
            {
                EnemyManager.Instance.RemoveEnemy(this);
            }
        }

        private void Die()
        {
            if (IsDead) return;

            IsDead = true;

            StopAllCoroutines();

            OnDeadEvent?.Invoke();

            Destroy(gameObject);
        }

        private int CalculateDamage(Bullet bullet)
        {
            float damage = bullet.data.bulletDamage;

            if (data.weaknessBullet == bullet.data.bulletType)
            {
                damage *= data.weaknessMultiplier;
            }

            return Mathf.RoundToInt(damage);
        }

        // 충돌 감지 로직
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 1. 펜스(Fence) 충돌 처리
            if (collision.gameObject.CompareTag("Fence"))
            {
                // 보스 타입인 경우
                if (data.enemyType == EnemyType.Boss)
                {
                    // 아직 전투 모드가 아니라면 진입
                    if (!isBossAttacking)
                    {
                        Debug.Log("보스: 펜스 도착! 공격 패턴 시작");
                        isBossAttacking = true;

                        // [중요] BossAnimation 컴포넌트를 찾아 공격 시작 명령을 내림
                        BossAnimation bossAnim = GetComponentInChildren<BossAnimation>();
                        if (bossAnim != null)
                        {
                            bossAnim.StartBossBattlePhase();
                        }
                    }
                    return;
                }

                // 일반 몬스터인 경우
                attackComplete = true;
            }

            // 2. 총알(Bullet) 충돌 처리
            else if (collision.gameObject.CompareTag("Bullet"))
            {
                if (gameObject.layer == LayerMask.NameToLayer("Invincible")) return;

                Bullet bullet = collision.gameObject.GetComponent<Bullet>();

                if (bullet != null)
                {
                    int finalDamage = CalculateDamage(bullet);
                    HandleHit(finalDamage);
                }
            }
        }
    }
}