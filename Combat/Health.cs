using Code.Entities;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Combat
{
    public class Health : MonoBehaviour, IEntityComponent
    {
        private Entity _entity;
        private int _maxHealth;

        public int CurrentHealth { get; private set; }
        
        // 첫 번째 인수 : 현재 체력, 두 번째 인수 최대 체력
        public UnityEvent<int, int> OnSetHealthEvent;
        public UnityEvent<int, int> OnDamageEvent;
        public UnityEvent OnDeadEvent;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;   
        }

        public void SetUpHealth(int health) // 시작할떄 쓰는거
        {
            _maxHealth = health;
            CurrentHealth = health;
            OnSetHealthEvent?.Invoke(CurrentHealth, _maxHealth);
        }

        public void ApplyDamage(int damage)
        {
            if (_entity.IsDead) return;
            
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, _maxHealth);
            OnDamageEvent?.Invoke(CurrentHealth, _maxHealth);

            if (CurrentHealth <= 0) // 체력 0이면
            {
                Debug.Log("죽음");
                OnDeadEvent?.Invoke();
                _entity.IsDead = true;
            }
        }

        [ContextMenu("TestDamage")]
        public void TestDamage()
        {
            ApplyDamage(1);
        }
    }
}