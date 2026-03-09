using Code.Enemy;
using UnityEngine;

public class FlyAnimation : EnemyAnimation
{
    [SerializeField] private FlyMovement flyMovement;

    private void Start()
    {
        if (flyMovement == null)
        {
            flyMovement = GetComponentInParent<Enemy>()?.GetComponentInChildren<FlyMovement>();
        }
    }

    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();

        // 다른 기능 추가시 사용
    }
}
