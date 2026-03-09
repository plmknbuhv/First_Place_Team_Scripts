using Code.Farms.Plants;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemButton : MonoBehaviour
{
    [SerializeField] private Image itemImage;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void SetItemData(PlantDataSO data)
    {
        itemImage.sprite = data.plantSprite;
    }
}
