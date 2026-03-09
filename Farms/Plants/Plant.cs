using UnityEngine;

namespace Code.Farms.Plants
{
    public class Plant : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private RuntimeAnimatorController baseController;
        
        public PlantDataSO PlantData { get; private set; }

        public void SetPlant(PlantDataSO plantData)
        {
            spriteRenderer.sprite = plantData.plantSprite;
            PlantData = plantData;
            
            var overrideController =
                new AnimatorOverrideController(baseController);
            overrideController["Idle"] = plantData.plantClip;
            animator.runtimeAnimatorController = overrideController;
        }
    }
}
