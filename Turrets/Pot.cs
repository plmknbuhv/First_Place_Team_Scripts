using UnityEngine;

namespace Code.Turrets
{
    public class Pot : MonoBehaviour
    {
        // 현재 이 화분에 식물이 심어져 있는지 확인하는 변수
        public bool isOccupied = false;

        // (선택사항) 심어진 식물을 삭제하거나 할 때 참조하기 위해 저장
        public GameObject currentPlant;
    }
}