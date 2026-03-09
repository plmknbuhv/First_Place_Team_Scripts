using Code.Entities;
using UnityEngine;

namespace Code.Enemy
{
    public class GrasshopperMovement : EnemyMovement
    {
        [Header("Grasshopper Settings")]
        [SerializeField] private float restTimeAfterLanding = 2.0f;
        [SerializeField] private float jumpPower = 5.0f;
        [SerializeField] private float customGravity = 15f;

        private float _timer;
        private bool _isJumping;
        private bool _isActionInProgress;

        private float _groundY;
        private Animator _animator;

        protected override void Start()
        {
            base.Start();
            _timer = Random.Range(0f, 1f);
            _groundY = transform.position.y;
            _animator = GetComponentInChildren<Animator>();
        }

        protected override void MoveNormal()
        {
            if (rigid == null) return;

            if (_isJumping)
            {
                ApplyCustomGravity();
                CheckLanding();
            }
            else
            {
                HandleIdle();
            }
        }

        private void HandleIdle()
        {
            rigid.linearVelocity = Vector2.zero;

            if (_isActionInProgress) return;

            _timer += Time.deltaTime;

            if (_timer >= restTimeAfterLanding)
            {
                TriggerJumpAnimation();
            }
        }

        private void TriggerJumpAnimation()
        {
            // [중요 수정] 행동 시작 플래그 ON -> 이제 착지할 때까지 타이머 로직이 멈춤
            _isActionInProgress = true;


            if (_animator != null)
            {
                _animator.SetTrigger("Jump");
            }
            else
            {
                ExecutePhysicalJump();
            }
        }

        public void ExecutePhysicalJump()
        {
            if (_isJumping) return;


            _isJumping = true;
            _groundY = transform.position.y;

            Vector2 jumpVelocity = new Vector2(Vector2.left.x * enemy.data.moveSpeed, jumpPower);
            rigid.linearVelocity = jumpVelocity;
        }

        private void ApplyCustomGravity()
        {
            Vector2 currentVelocity = rigid.linearVelocity;
            currentVelocity.y -= customGravity * Time.deltaTime;
            rigid.linearVelocity = currentVelocity;
        }

        private void CheckLanding()
        {
            if (rigid.linearVelocity.y <= 0 && transform.position.y <= _groundY)
            {
                Land();
            }
        }

        private void Land()
        {
            _isJumping = false;

            _isActionInProgress = false;
            _timer = 0f;

            transform.position = new Vector3(transform.position.x, _groundY, transform.position.z);
            rigid.linearVelocity = Vector2.zero;
        }
    }
}