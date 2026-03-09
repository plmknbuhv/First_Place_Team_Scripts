using Code.Entities;
using Code.Managers;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyAnimation : MonoBehaviour, IEntityComponent
    {
        protected EnemyMovement enemyMovement;
        public Animator animator;

        // [수정됨] 코인을 훔쳤는지 체크하는 플래그 변수 추가
        private bool hasStolen = false;

        public virtual void Initialize(Entity entity)
        {
            animator = GetComponent<Animator>();
            enemyMovement = entity.GetCompo<EnemyMovement>();

            // 초기화 시 플래그 리셋 (혹시 오브젝트 풀링을 쓸 경우를 대비)
            hasStolen = false;

            Debug.Assert(animator != null, $"{name}: 애니메이터 없음");
            Debug.Assert(enemyMovement != null, $"{name}: 무브먼트 없음");
        }

        private void Update()
        {
            if (enemyMovement == null || animator == null) return;

            UpdateAnimation();
        }

        protected virtual void UpdateAnimation()
        {
            if (enemyMovement.enemy == null) return;

            if (enemyMovement.enemy.data.enemyType != EnemyType.Boss)
            {
                if (enemyMovement.enemy.attackComplete)
                {
                    animator.SetTrigger("Escape");

                    // [수정됨] 아직 훔치지 않았을 때만 실행
                    if (!hasStolen)
                    {
                        print(enemyMovement.enemy.data.stealValue * UpgradeManager.Instance.StolenGold);
                        float stealValue = enemyMovement.enemy.data.stealValue + enemyMovement.enemy.data.stealValue * UpgradeManager.Instance.StolenGold;
                        ShopManager.Instance.AddCoin(-(int)stealValue);
                        hasStolen = true; // 실행 후 true로 바꿔서 다시 들어오지 못하게 함
                    }
                    return;
                }
            }

            if (enemyMovement.Rigid != null)
            {
                if (Mathf.Abs(enemyMovement.Rigid.linearVelocity.x) > 0.1f)
                {
                    animator.SetTrigger("Move");
                }
            }
        }
    }
}