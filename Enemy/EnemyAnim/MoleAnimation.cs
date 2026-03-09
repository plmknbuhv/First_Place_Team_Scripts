using Code.Enemy;
using Code.Entities;
using UnityEngine;

public class MoleAnimation : EnemyAnimation
{
    [SerializeField] private MoleMovement moleMovement;

    private void Start()
    {
        moleMovement = GetComponentInParent<Enemy>().gameObject.GetComponentInChildren<MoleMovement>();
    }
    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();

        animator.SetBool("IsUnder", moleMovement.isUnderground);
    }
}
