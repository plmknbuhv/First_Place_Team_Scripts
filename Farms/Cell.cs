using Code.Effects;
using Code.Farms.Plants;
using Code.Managers;
using UnityEngine;

namespace Code.Farms
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Plant plantPrefab;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite noneSprite;
        [SerializeField] private Sprite growingSprite;
        [SerializeField] private PlayGraphVFX seedInstallGraph;
        [SerializeField] private PlayGraphVFX growPlantGraph;

        [SerializeField] private float impulseForce = 0.1f;
        
        private PlantDataSO _plantData;
        private Plant _grownPlant;
        private float _timer;
        
        public FarmType FarmType { get; private set; }

        public void PlacePlant(PlantDataSO plantData)
        {
            if (FarmType != FarmType.None) return;
            
            ImpulseManager.Instance.Impulse(impulseForce);
            seedInstallGraph.PlayVFX(transform.position, Quaternion.identity);
            
            _plantData = plantData;
            _timer = 0;
            FarmType = FarmType.Grow;
            spriteRenderer.sprite = growingSprite;
        }

        private void Update()
        {
            if (FarmType != FarmType.Grow) return;
            
            _timer += Time.deltaTime;

            if (_timer >= _plantData.glowTime)
            {
                GrowOver();
            }
        }

        private void GrowOver()
        {
            growPlantGraph.PlayVFX(transform.position, Quaternion.identity);
            ImpulseManager.Instance.Impulse(impulseForce);
            
            FarmType = FarmType.Grown;
            spriteRenderer.sprite = noneSprite;
            _grownPlant = Instantiate(plantPrefab, transform.position, Quaternion.identity);
            _grownPlant.SetPlant(_plantData);
        }

        public Plant Harvest()
        {
            seedInstallGraph.PlayVFX(transform.position, Quaternion.identity);
            ImpulseManager.Instance.Impulse(impulseForce * 1.5f);
            
            FarmType = FarmType.None;
            var result = _grownPlant;
            _grownPlant = null;
            return result;
        }
    }
}
