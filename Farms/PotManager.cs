using System.Collections.Generic;
using Code.Core;
using Code.Effects;
using Code.Farms.Plants;
using Code.Managers;
using UnityEngine;

namespace Code.Farms
{
    public class PotManager : MonoSingleton<PotManager>
    {
        [SerializeField] private PlayGraphVFX plantInstallGraph;
        [SerializeField] private List<Transform> potPoints;
        [SerializeField] private Plant previewPlant;

        private Dictionary<Transform, bool> _canInstallDict = new Dictionary<Transform, bool>();

        private int _plantCount;
        private void Awake()
        {
            foreach (var potTrm in potPoints)
                _canInstallDict.Add(potTrm, true);
        }

        public bool CheckPotPoint(Vector3 pos, out Transform potTrm)
        {
            potTrm = null;
            float minDistance = float.MaxValue;

            if (UpgradeManager.Instance.PlantCount > _plantCount)
            {
                foreach (var trm in potPoints)
                {
                    var distance = Vector3.Distance(pos, trm.position);
                    if (distance >= 1.5f || distance > minDistance) continue; // 가장 가까이 있는거 아니면 스킵

                    if (_canInstallDict[trm] == false) continue;

                    minDistance = distance;
                    potTrm = trm;
                }
            }
            
            return potTrm != null;
        }

        public void ShowPreview(Transform potTrm, PlantDataSO plantData)
        {
            previewPlant.gameObject.SetActive(true);
            previewPlant.transform.position = potTrm.position;
            previewPlant.SetPlant(plantData);
        }

        public void OffPreview()
        {
            previewPlant.gameObject.SetActive(false);
        }

        public void InstallPlant(Transform installTrm)
        {
            _plantCount++;
            ImpulseManager.Instance.Impulse(0.12f);
            plantInstallGraph.PlayVFX(installTrm.position, Quaternion.identity);
            _canInstallDict[installTrm] = false;
        }

        public void UnInstallPlant(Transform installTrm)
        {
            _plantCount--;
            _canInstallDict[installTrm] = true;
        }
    }
}
