using Code.Entities;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyMovement : MonoBehaviour, IEntityComponent
    {
        public Enemy enemy;
        protected Rigidbody2D rigid;

        protected Transform targetEscapePoint;
        protected const float ArrivalSqrThreshold = 0.25f;

        public Rigidbody2D Rigid => rigid;

        protected virtual void Start()
        {
            enemy = GetComponentInParent<Enemy>();

            if (enemy != null)
            {
                rigid = enemy.GetComponentInParent<Rigidbody2D>();
            }
        }

        public void Initialize(Entity entity)
        {
            // 초기화 로직
        }

        protected virtual void Update()
        {
            if (enemy == null) return;

            if (!enemy.attackComplete)
            {
                MoveNormal();
            }
            else
            {
                HandleEscape();
            }
        }

        protected virtual void MoveNormal()
        {
            if (rigid != null)
            {
                rigid.linearVelocity = new Vector2(Vector2.left.x * enemy.data.moveSpeed, rigid.linearVelocity.y);
            }
        }

        private void HandleEscape()
        {
            if (targetEscapePoint == null) SetClosestEscapePoint();
            MoveToTarget();
            CheckArrival();
        }

        private void SetClosestEscapePoint()
        {
            if (EnemyManager.Instance == null) return;

            Transform pointA = EnemyManager.Instance.escapePoint[0];
            Transform pointB = EnemyManager.Instance.escapePoint[1];

            float distA = (pointA.position - transform.position).sqrMagnitude;
            float distB = (pointB.position - transform.position).sqrMagnitude;

            targetEscapePoint = (distA < distB) ? pointA : pointB;
        }

        private void MoveToTarget()
        {
            if (targetEscapePoint == null || rigid == null) return;

            Vector2 direction = (targetEscapePoint.position - transform.position).normalized;

            rigid.linearVelocity = direction * enemy.data.escapeSpeed;
        }

        private void CheckArrival()
        {
            if (targetEscapePoint == null) return;
            float sqrDist = (targetEscapePoint.position - transform.position).sqrMagnitude;
            if (sqrDist < ArrivalSqrThreshold) OnEscapeComplete();
        }

        protected virtual void OnEscapeComplete()
        {
            if (rigid != null) rigid.linearVelocity = Vector2.zero;

            if (enemy != null)
            {
                enemy.attackComplete = false;
                Destroy(enemy.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}