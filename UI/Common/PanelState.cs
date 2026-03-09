using UnityEngine;

namespace Code.UI
{
    public abstract class PanelState : MonoBehaviour
    {
        [field: SerializeField] protected PanelView view { get; private set; }
        public string StateName { get; private set; }
        protected PanelControlModel controller;
        public virtual void Init(PanelControlModel controller)
        {
            this.controller = controller;
        }
        public virtual void Enter()
        {
            view.Enter();
        }
        public abstract void FixedUpdate();
        public virtual void Exit()
        {
            view.Exit();
        }
        protected void SetStateName(string name) => StateName = name;
    }
}