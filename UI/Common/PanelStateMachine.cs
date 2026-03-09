using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.UI
{
    public class PanelStateMachine
    {
        public PanelState CurrentUI { get; set; }
        public Dictionary<string, PanelState> panels = new();

        public PanelStateMachine(PanelControlModel controller,List<PanelState> states)
        {
            foreach (var ui in states)
            {
                panels.Add(ui.StateName, ui);
            }

            panels.ToList().ForEach(ui => ui.Value.Init(controller));
        }

        public void ChangeState(string uiName)
        {
            PanelState targetState = panels.GetValueOrDefault(uiName);

            if (targetState == null)
            {
                Debug.LogWarning($"[ {uiName} ] 이라는 state가 존재하지 않음.");
                return;
            }
            else if (CurrentUI == targetState)
            {
                Debug.LogWarning($"이미[ {uiName} ]를 사용하고 있는 중임에도 변경을 요청함");
                return;
            }
            CurrentUI?.Exit();
            CurrentUI = targetState;
            CurrentUI.Enter();
        }
        public void FixedUpdateState()
        {
            CurrentUI.FixedUpdate();
        }
    }
}