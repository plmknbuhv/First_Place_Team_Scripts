using Code.Farms.Plants;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InGame
{
    [RequireComponent(typeof(Button))]
    public class ItemSelectButton : MonoBehaviour
    {
        [SerializeField] private Image buttonImage;
        private Button _button;
        private InformationSetter _infoSetter;
        private PlantDataSO _data;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        public void SetItemInfo(InformationSetter infoSetter, PlantDataSO data)
        {
            _infoSetter = infoSetter;
            _data = data;

            _button.onClick.RemoveListener(HandleButtonClick);
            _button.onClick.AddListener(HandleButtonClick);

            SetButton();
        }


        public void RemoveButtonListener()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }
        private void SetButton()
        {
            buttonImage.sprite = _data.plantSprite;
        }
        private void HandleButtonClick()
        {
            if (_infoSetter == null)
                return;

            _infoSetter.SetItemInfo(_data);
        }
    }
}