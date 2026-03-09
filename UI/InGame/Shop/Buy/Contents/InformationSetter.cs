using Code.Farms.Plants;
using Code.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame
{
    public class InformationSetter : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameTMP;
        [SerializeField] private TMP_Text costTMP;
        [SerializeField] private TMP_Text explainTMP;

        [SerializeField] private Button purchaseButton;

        private PlantDataSO _currentPlant;

        public void SetItemInfo(PlantDataSO data)
        {
            _currentPlant = data;
            SetInfoUI();

            purchaseButton.onClick.RemoveAllListeners();
            purchaseButton.onClick.AddListener(HandlePurchaseClicked);

        }
        private void HandlePurchaseClicked()
        {
            ShopManager.Instance.PurchaseItem(_currentPlant);
            
                SetInfoUI();//변경되는걸 AddListener
            
        }

        private void SetInfoUI()
        {
            nameTMP.SetText(_currentPlant.plantName);
            costTMP.SetText($"{_currentPlant.buyPrice}G");
            explainTMP.SetText(_currentPlant.description);
        }
    }
}