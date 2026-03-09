using Code.Entities;
using Code.Farms;
using Code.Farms.Plants;
using UnityEngine;

namespace Code.Turrets
{
    public class Turret : Entity
    {
        private TurretAttack _turretAttack;
        private EntityAnimator _entityAnimator;
        private EntityAnimatorTrigger _animatorTrigger;
        private string _prevAnimName;
        private Transform _potTrm;
        
        public PlantDataSO PlantData { get; private set; }
        
        #region 임시 테스트 용
        [SerializeField] private bool isTest;
        [SerializeField] private PlantDataSO testData;
        
        private void Start()
        {
            if (isTest)
                SetupTurret(testData, null);
        }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            _turretAttack = GetCompo<TurretAttack>();
            _entityAnimator = GetCompo<EntityAnimator>();
            _animatorTrigger = GetCompo<EntityAnimatorTrigger>();
        }

        public void SetupTurret(PlantDataSO plantData, Transform potTrm)
        {
            PlantData =  plantData;
            _potTrm = potTrm;
            IsDead = false;

            _turretAttack.AttackStart();
            const string idle = "IDLE";
            ChangeAnimation(idle);

            _animatorTrigger.OnDeadTrigger += HandleDeadTrigger;
        }

        private void OnDestroy()
        {
            _animatorTrigger.OnDeadTrigger -= HandleDeadTrigger;
        }

        public void HandleDeadTrigger()
        {
            Destroy(gameObject);
            PotManager.Instance.UnInstallPlant(_potTrm);
        }

        public void ChangeAnimation(string animName)
        {
            Debug.Log(animName);
            _entityAnimator.SetParam(Animator.StringToHash(_prevAnimName), false);
            _prevAnimName = animName;
            _entityAnimator.SetParam(Animator.StringToHash(animName), true);
        }
    }
}