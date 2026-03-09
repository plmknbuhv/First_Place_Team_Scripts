using System;
using Code.Entities;
using UnityEngine;

namespace Code.Players
{
    public class PlayerAnimator : MonoBehaviour,  IEntityComponent
    {
        private Animator _animator;
        private PlayerMovement _playerMovement;
        
        private readonly int _isMoveHash = Animator.StringToHash("IsMove"); 
        
        public void Initialize(Entity entity)
        {
            _playerMovement = entity.GetCompo<PlayerMovement>();
            _animator = GetComponent<Animator>();
            
            _playerMovement.OnMoveEvent += HandleMoveEvent;
        }

        private void OnDestroy()
        {
            _playerMovement.OnMoveEvent -= HandleMoveEvent;
        }

        public void HandleMoveEvent(Vector2 moveDir)
        {
            bool isMove = moveDir != Vector2.zero;
            _animator.SetBool(_isMoveHash, isMove);
        }

        public void AnimateTrigger(string triggerName) => _animator.SetTrigger(triggerName);
    }
}