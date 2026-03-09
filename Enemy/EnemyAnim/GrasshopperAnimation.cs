using Code.Enemy;
using Code.Entities;
using UnityEngine;

public class GrasshopperAnimation : EnemyAnimation
{
    [SerializeField] private GrasshopperMovement grasshopperMovement;

    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        if (grasshopperMovement == null)
        {
            grasshopperMovement = entity.GetCompo<GrasshopperMovement>();
        }
    }

    private void Start()
    {
        // Initialize가 안 불렸을 경우를 대비한 안전장치
        if (grasshopperMovement == null)
        {
            grasshopperMovement = GetComponentInParent<Enemy>()?.GetComponentInChildren<GrasshopperMovement>();
        }
    }

    protected override void UpdateAnimation()
    {
        // [중요] 부모의 UpdateAnimation을 호출해야 Escape 및 Move 로직이 실행됩니다.
        base.UpdateAnimation();
    }

    // 애니메이션 이벤트에서 호출되는 함수
    public void OnAnimationJump()
    {
        if (grasshopperMovement != null)
        {
            grasshopperMovement.ExecutePhysicalJump();
        }
    }
}