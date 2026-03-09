using System.Collections.Generic;
using UnityEngine;

namespace Code.UI.Menu
{
    public class MenuUI : PanelControlModel
    {
        protected override void Start()
        {
            base.Start();
            menuStateMachine.ChangeState(states[0].StateName);
        }
    }
}