using Code.Entities;
using Code.ETC;
using UnityEngine;

namespace Code.Players
{
    public class PlayerRenderer : SortingRenderer, IEntityComponent
    {
        private Player _player;
        private PlayerMovement _playerMovement;
        
        [field: SerializeField] public float FacingDirection { get; private set; } = 1f;
        
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            _playerMovement = entity.GetCompo<PlayerMovement>();
            
            _playerMovement.OnMoveEvent += HandleFlipEvent;
        }

        private void OnDestroy()
        {
            _playerMovement.OnMoveEvent -= HandleFlipEvent;
        }

        public void HandleFlipEvent(Vector2 moveDir)
        {
            float xVelocity = moveDir.x;
            float xMove = Mathf.Approximately(xVelocity, 0) ? 0 : Mathf.Sign(xVelocity);
            if (Mathf.Abs(xMove + FacingDirection) < 0.5f) //바라보는 방향과 진행방향이 다르다면 플립
            {
                Flip();
            }
        }
        
        private void Flip()
        {
            FacingDirection *= -1;
            transform.Rotate(0, 180f, 0);
        }
    }
}