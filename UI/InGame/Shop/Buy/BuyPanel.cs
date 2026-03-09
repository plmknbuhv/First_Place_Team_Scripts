using Code.Farms.Plants;
using Code.Managers;
using System.Collections.Generic;

namespace Code.UI.InGame
{
    public class BuyPanel : PanelState
    {
        private BuyView _view;
        private void Awake()
        {
            _view = view as BuyView;
            SetStateName(GetType().Name);
        }
        public override void Enter()
        {
            base.Enter();
            SetViewList(ShopManager.Instance.GetGoods());
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void FixedUpdate()
        {
        }

        public void SetViewList(List<PlantDataSO> sellGoods)
        {
            _view.SetItemList(sellGoods);
        }
    }
}
