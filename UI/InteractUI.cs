using Code.Core;
using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class InteractUI : MonoSingleton<InteractUI>
    {
        [SerializeField] private GameObject storeUI;
        [SerializeField] private TextMeshProUGUI storeText;
        
        public void SetActiveStoreUI(bool isActive, string text = "")
        {
            storeText.text = text;
            storeUI.SetActive(isActive);
        }
    }
}
