using System.Collections.Generic;
using UnityEngine;

namespace Code.UI
{
    public abstract class PanelControlModel : MonoBehaviour
    {
        [SerializeField] protected List<PanelState> states;
        protected PanelStateMachine menuStateMachine;

        protected virtual void Start()
        {
            menuStateMachine = new(this, states);
        }

        protected virtual void FixedUpdate()
        {
            menuStateMachine.FixedUpdateState();
        }

        public void ChangeState(string newState) => menuStateMachine.ChangeState(newState);
    }
}