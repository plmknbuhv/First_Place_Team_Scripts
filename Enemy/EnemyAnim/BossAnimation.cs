using Code.Enemy;
using Code.Entities;
using Code.Managers;
using System.Collections;
using UnityEngine;

public class BossAnimation : EnemyAnimation
{
    [Header("Boss Attack Settings")]
    [Tooltip("공격 애니메이션이 끝난 후, 다음 공격까지 대기할 시간 (초)")]
    [SerializeField] private float delayAfterAttack = 0.5f;

    private int _attackCounter = 0;
    private bool _isBattleStarted = false;

    // 초기화
    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
    }

    private void Start()
    {
        // 시작 시 트리거 초기화 (안전장치)
        if (animator != null)
        {
            animator.ResetTrigger("NormalAttack");
            animator.ResetTrigger("SpecialAttack");
        }
    }

    // 매 프레임 호출: 보스의 상태에 따라 애니메이터의 'Move' 파라미터를 제어
    protected override void UpdateAnimation()
    {
        if (enemyMovement == null || animator == null) return;
        if (enemyMovement.enemy == null) return;

        // Enemy.cs에서 펜스 충돌 시 isBossAttacking = true가 됨
        // 전투 중(true) -> Move = false (-> Animator는 Idle 상태로 전환됨)
        // 이동 중(false) -> Move = true (-> Animator는 Move 상태 유지)
        bool isMoving = !enemyMovement.enemy.isBossAttacking;

        animator.SetBool("Move", isMoving);
    }

    // Enemy.cs에서 호출
    public void StartBossBattlePhase()
    {
        if (_isBattleStarted) return;

        Debug.Log("보스: 전투 모드 시작 (Move=false로 전환됨, 공격 루프 시작)");
        _isBattleStarted = true;

        StartCoroutine(CoAttackPattern());
    }

    private IEnumerator CoAttackPattern()
    {
        yield return new WaitForSeconds(1.0f);

        while (enemyMovement.enemy != null && !enemyMovement.enemy.IsDead)
        {
            PerformAttack();

            yield return new WaitForSeconds(0.2f);

            if (animator != null)
            {
                animator.ResetTrigger("NormalAttack");
                animator.ResetTrigger("SpecialAttack");
            }

            float currentAnimLength = animator.GetCurrentAnimatorStateInfo(0).length;

            if (currentAnimLength < 0.2f) currentAnimLength = 1.0f;

            yield return new WaitForSeconds(currentAnimLength + delayAfterAttack);
        }
    }

    private void PerformAttack()
    {
        if (animator == null) return;

        animator.ResetTrigger("NormalAttack");
        animator.ResetTrigger("SpecialAttack");

        if (_attackCounter >= 4)
        {
            animator.SetTrigger("SpecialAttack");
            _attackCounter = 0;
        }
        else
        {
            animator.SetTrigger("NormalAttack");
            _attackCounter++;
        }
    }

    // --- Animation Events (애니메이션 클립에서 호출) ---
    public void OnAnimNormalAttack()
    {
        ShopManager.Instance.AddCoin(-40);
    }

    public void OnAnimSpecialAttack()
    {
        ShopManager.Instance.AddCoin(-60);
    }
} 