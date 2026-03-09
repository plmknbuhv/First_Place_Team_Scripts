using Code.Entities;
using UnityEngine;

namespace Code.Enemy
{
    public class MoleMovement : EnemyMovement
    {
        private float time;

        [Header("Mole Settings")]
        [SerializeField] private float aboveGroundTime = 2f;
        [SerializeField] private float undergroundTime = 2f;

        public bool isUnderground = false;

        private SpriteRenderer _spriteRenderer;

        protected override void Start()
        {
            base.Start();

            if (enemy != null)
            {
                _spriteRenderer = enemy.GetComponent<SpriteRenderer>();
            }
        }

        protected override void Update()
        {
            if (enemy == null) return;

            base.Update();
        }

        protected override void MoveNormal()
        {
            time += Time.deltaTime;

            float targetTime = isUnderground ? undergroundTime : aboveGroundTime;


            if (time > targetTime)
            {
                time = 0;
                isUnderground = !isUnderground;


                UpdateMoleState();
            }

            if (rigid != null)
            {
                float currentSpeed = isUnderground ? (enemy.data.moveSpeed * 0.5f) : enemy.data.moveSpeed;
                rigid.linearVelocity = Vector2.left * currentSpeed;
            }
        }

        private void UpdateMoleState()
        {
            if (isUnderground)
            {
                if (_spriteRenderer != null) _spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
                if (enemy != null) enemy.gameObject.layer = LayerMask.NameToLayer("Invincible");
            }
            else
            {
                if (_spriteRenderer != null) _spriteRenderer.color = Color.white;
                if (enemy != null) enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
            }
        }
    }
}