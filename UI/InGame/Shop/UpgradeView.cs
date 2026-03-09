using System.Collections.Generic;
using UnityEngine;

namespace Code.UI.InGame
{
    public enum UpgradeType
    {
        PotAmount,
        MoveSpeed,
        MaxSeed,
        SellBonus,
        LandTaxDownBonus,
        GoldDecrease
    }

    public class UpgradeView : PanelView
    {
        [SerializeField] private List<UpgradePanelView> panels;
        public override void Enter()
        {
            transform.parent.SetSiblingIndex(2);
        }

        public override void Exit()
        {
        }

        private void SetUpgradePanels()
        {
            foreach (var panel in panels)
            {
                switch (panel.Type)
                {
                    case UpgradeType.PotAmount:
                        break;
                    case UpgradeType.MoveSpeed:
                        break;
                    case UpgradeType.MaxSeed:
                        break;
                    case UpgradeType.SellBonus:
                        break;
                    case UpgradeType.LandTaxDownBonus:
                        break;
                    case UpgradeType.GoldDecrease:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}