using UnityEngine;

namespace Code.UI
{
    public struct PopUpData
    {
        public string Name;
        public string Description;
    }
    public interface IPopupable
    {
        public PopUpData GetPopupData();
    }
}