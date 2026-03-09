using System;
using UnityEngine;

namespace Code.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public Action OnAnimationEndTrigger; // 안씀
        public Action OnDeadTrigger;
        public Action OnAttackTrigger;
        public Action OnAttackEndTrigger;

        
        private Entity _entity;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void AnimationEnd() => OnAnimationEndTrigger?.Invoke(); // 안씀
        private void DeadTrigger() => OnDeadTrigger?.Invoke();
        private void AttackTrigger() => OnAttackTrigger?.Invoke();
        private void AttackEndTrigger() => OnAttackEndTrigger?.Invoke();
    }
}