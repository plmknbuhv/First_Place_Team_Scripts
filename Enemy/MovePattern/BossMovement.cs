using Code.Entities;
using UnityEngine;

namespace Code.Enemy
{
    public class BossMovement : EnemyMovement
    {
        protected override void MoveNormal()
        {
            // Enemy 스크립트에서 펜스와 충돌하면 isBossAttacking을 true로 만듭니다.
            // 그때 이동을 멈춥니다.
            if (enemy.isBossAttacking)
            {
                if (rigid != null)
                {
                    rigid.linearVelocity = Vector2.zero;
                }
            }
            else
            {
                // 전투 상태가 아니라면(아직 펜스 도착 전) 부모의 기본 이동 로직 수행
                base.MoveNormal();
            }
        }
    }
}