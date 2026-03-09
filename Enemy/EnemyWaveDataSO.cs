using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaveDataSO", menuName = "SO/Enemy/EnemyWaveDataSO", order = 1)]
public class EnemyWaveDataSO : ScriptableObject
{
    [Header("General Settings")]
    [Tooltip("이 웨이브가 시작되기 전 대기 시간 (초)")]
    public float initialStartDelay;
    public int Cost;

    [System.Serializable]
    public class SpawnInfo
    {
        [Tooltip("소환할 적 프리팹")]
        public GameObject enemyPrefab;

        [Tooltip("몇 마리 소환할지")]
        public int count;

        [Tooltip("소환 간격")]
        public float interval;

        [Tooltip("시작 딜레이 (다른 몬스터보다 조금 늦게 나오게 하고 싶을 때)")]
        public float startDelay;
    }

    [System.Serializable]
    public class WaveSegment
    {
        [Tooltip("에디터 식별용 이름 (예: 좀비+해골 섞여나옴)")]
        public string segmentName;

        [Tooltip("이 단계에서 동시에 소환될 몬스터 목록")]
        public List<SpawnInfo> spawnList;

        [Tooltip("이 단계의 모든 소환이 끝난 후 다음 단계로 넘어가기 전 대기 시간")]
        public float delayAfterSegment;
    }

    [Header("Wave Configuration")]
    public List<WaveSegment> segments; // 이 웨이브는 여러 단계로 구성됨
}
