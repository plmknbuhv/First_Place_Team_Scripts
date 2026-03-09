using Code.Entities;
using Code.Farms;
using UnityEngine;

namespace Code.Players
{
    public class FarmChecker : MonoBehaviour, IEntityComponent
    {
        private Player _player;
        
        public bool IsCanInteract { get; private set; }
        public Cell TargetCell { get; private set; }
        
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
        }
        
        private void Update()
        {
            CheckFarm();
        }

        private void CheckFarm()
        {
            if (FarmManager.Instance.TryGetCell(transform.position, out Cell cell))
            {
                FarmManager.Instance.SetActiveSelectBox(true, cell);
                TargetCell = cell;
                IsCanInteract = true;
            }
            else
            {
                // 만약 찾은 셀이 없고 이전 이전타겠이 남아있다면 해제하고 참조 끊기
                if (TargetCell)
                {
                    IsCanInteract = false;
                    FarmManager.Instance.SetActiveSelectBox(false);
                    TargetCell = null;
                }
            }
        }
    }
}