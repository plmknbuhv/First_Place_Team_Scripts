using System;
using Code.Entities;
using Code.Managers;
using Input;
using UnityEngine;

namespace Code.Players
{
    public class PlayerMovement : MonoBehaviour, IEntityComponent
    {
        [Header("�̵� ���ѿ���")]
        [SerializeField] private Vector2 limitLeftBottom;
        [SerializeField] private Vector2 limitRightTop;
        
        private PlayerInputSO playerInput;
        private PlayerDataSO playerData;
        
        private Player _player;
        private bool _isCanMove;
        
        public event Action<Vector2> OnMoveEvent;
        
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            playerInput = _player.PlayerInput;
            playerData = _player.PlayerData;

            _isCanMove = true;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        { 
            if (_isCanMove == false || ShopManager.Instance.IsActive)
            {
                OnMoveEvent?.Invoke(Vector2.zero);
                return;
            }
            
            OnMoveEvent?.Invoke(playerInput.MovementKey);
            
            // �̵� ��ġ ���ϱ�
            Vector3 moveOffset = playerInput.MovementKey * (playerData.moveSpeed * Time.deltaTime);
            Vector3 newPosition = _player.transform.position + moveOffset;

            // Ŭ����
            newPosition.x = Mathf.Clamp(newPosition.x, limitLeftBottom.x, limitRightTop.x);
            newPosition.y = Mathf.Clamp(newPosition.y, limitLeftBottom.y, limitRightTop.y);

            // ������ �̵�
            _player.transform.position = newPosition;
        }

        public void SetCanMove(bool isCanMove)
            => _isCanMove = isCanMove;
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 center = (limitLeftBottom + limitRightTop) / 2;
            Vector3 size = new Vector3(
                Mathf.Abs(limitLeftBottom.x - limitRightTop.x),
                Mathf.Abs(limitLeftBottom.y - limitRightTop.y));
            Gizmos.DrawWireCube(center, size);
            Gizmos.color = Color.white;
        }
#endif
    }
}