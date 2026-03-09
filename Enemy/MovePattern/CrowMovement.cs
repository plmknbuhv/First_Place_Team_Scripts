using Code.Entities;
using UnityEngine;

namespace Code.Enemy
{
    // [수정됨] 이동 로직이므로 EnemyAnimation이 아니라 EnemyMovement를 상속받습니다.
    public class CrowMovement : EnemyMovement
    {
        [Header("Crow Settings")]
        [SerializeField] private float dashInterval = 3.0f;     // 대쉬 주기
        [SerializeField] private float dashDuration = 0.5f;     // 대쉬 지속 시간
        [SerializeField] private float dashSpeedMultiplier = 3.0f; // 대쉬 속도

        private float _timer;
        private bool _isDashing;

        private Animator _animator;

        protected override void Start()
        {
            base.Start();

            _timer = Random.Range(0f, 1.0f);
        }

        protected override void MoveNormal()
        {
            if (rigid == null) return;

            _timer += Time.deltaTime;

            if (_isDashing)
            {
                HandleDash();
            }
            else
            {
                HandleNormalFly();
            }
        }

        private void HandleNormalFly()
        {
            float moveSpeed = enemy.data.moveSpeed;
            rigid.linearVelocity = new Vector2(Vector2.left.x * moveSpeed, rigid.linearVelocity.y);

            if (_timer >= dashInterval)
            {
                StartDash();
            }
        }

        private void HandleDash()
        {
            float dashSpeed = enemy.data.moveSpeed * dashSpeedMultiplier;
            rigid.linearVelocity = new Vector2(Vector2.left.x * dashSpeed, rigid.linearVelocity.y);

            if (_timer >= dashDuration)
            {
                EndDash();
            }
        }

        private void StartDash()
        {
            _isDashing = true;
            _timer = 0f;

            if (_animator != null)
            {
                _animator.SetBool("IsDashing", true);
            }

            Debug.Log("까마귀 대쉬 시작!");
        }

        private void EndDash()
        {
            _isDashing = false;
            _timer = 0f;

            if (_animator != null)
            {
                _animator.SetBool("IsDashing", false);
            }
        }
    }
}