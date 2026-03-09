using Code.Entities;
using Code.UI;
using UnityEngine;

namespace Code.Players
{
    public class StoreChecker : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private LayerMask whatIsStore;
        [SerializeField] private float detectRadius = 2.0f;
        
        private PlayerInventory _inventory;

        public bool IsCanInteract { get; private set; }
        
        public void Initialize(Entity entity)
        {
            _inventory = entity.GetCompo<PlayerInventory>();
        }
        
        private void Update()
        {
            CheckStore();
        }

        private void CheckStore()
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position, detectRadius, whatIsStore);

            if (col != null)
            {
                InteractUI.Instance.SetActiveStoreUI(true, _inventory.HeldPlant ? "식물판매" : "상점열기");
                IsCanInteract = true;
            }
            else
            {
                InteractUI.Instance.SetActiveStoreUI(false);
                IsCanInteract = false;
            }
        }
    }
}