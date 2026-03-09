using UnityEngine;

namespace Code.Turrets
{
    public class PlacementManager : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject plantPrefab;
        public LayerMask potLayer;

        void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                DetectAndPlace();
            }
        }

        void DetectAndPlace()
        {
            // 1. 카메라 확인
            if (Camera.main == null)
            {
                Debug.LogError("메인 카메라를 찾을 수 없습니다! 카메라 태그를 MainCamera로 설정하세요.");
                return;
            }

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

            Collider2D hitCollider = Physics2D.OverlapPoint(mousePos, potLayer);

            if (hitCollider != null)
            {
                Debug.Log($"감지된 오브젝트: {hitCollider.gameObject.name}"); // 무엇이 감지됐는지 확인

                Pot clickedPot = hitCollider.GetComponent<Pot>();

                if (clickedPot != null)
                {
                    if (!clickedPot.isOccupied)
                    {
                        SpawnPlant(clickedPot);
                    }
                    else
                    {
                        Debug.Log("이미 식물이 심어져 있습니다.");
                    }
                }
                else
                {
                    Debug.LogWarning("감지된 물체에 'Pot' 스크립트가 없습니다!");
                }
            }
            else
            {
                Debug.Log("아무것도 감지되지 않았습니다. (콜라이더나 레이어를 확인하세요)");
            }
        }

        void SpawnPlant(Pot pot)
        {
            GameObject newPlant = Instantiate(plantPrefab, pot.transform.position, Quaternion.identity);
            pot.isOccupied = true;
            pot.currentPlant = newPlant;
            Debug.Log($"{pot.name}에 식물 배치 완료!");
        }
    }
}