using UnityEngine;

namespace Code.UI.InGame
{
    public class UpgradePanel : PanelState
    {
        private void Awake()
        {
            SetStateName(GetType().Name);
        }
        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void FixedUpdate()
        {
        }
    }
}
