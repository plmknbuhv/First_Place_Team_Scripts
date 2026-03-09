using Code.Core;
using Code.Core.EventSystem;
using Code.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyManager : MonoSingleton<EnemyManager>
    {
        [SerializeField] private GameEventChannelSO uiChannel;

        [Header("Global Settings")]
        [Tooltip("게임 시작 후 첫 웨이브가 시작되기 전까지의 전역 대기 시간 (최초 1회만 적용)")]
        public float gameStartWaitTime = 2.0f;

        [Header("Wave Settings")]
        public List<EnemyWaveDataSO> waves;
        public Transform[] escapePoint;

        private int _currentWaveIndex = 0;
        private List<Enemy> _activeEnemies = new List<Enemy>();
        private bool _isSpawningFinished = false;

        [Header("SpawnRange")]
        private const float SPAWN_X_MIN = 7.0f;
        private const float SPAWN_X_MAX = 10.0f;
        private const float SPAWN_Y_RANGE = 2.0f; // -2 ~ 2

        // void Start()를 IEnumerator Start()로 변경하여 시작 지연을 처리
        IEnumerator Start()
        {
            // 1. 게임 시작 대기 시간 (Game Start Delay)
            if (gameStartWaitTime > 0)
            {
                Debug.Log($"게임 시작! {gameStartWaitTime}초 뒤에 첫 웨이브가 시작됩니다.");
                yield return new WaitForSeconds(gameStartWaitTime);
            }

            // 2. 웨이브 프로세스 시작
            if (waves != null && waves.Count > 0)
            {
                StartCoroutine(ProcessWave());
            }
        }

        // 웨이브 처리 코루틴
        private IEnumerator ProcessWave()
        {
            _isSpawningFinished = false;

            // 인덱스 초과 방지
            if (_currentWaveIndex >= waves.Count)
            {
                Debug.Log("All Waves Completed!");
                yield break;
            }

            EnemyWaveDataSO currentWaveData = waves[_currentWaveIndex];

            // SO에 설정된 웨이브 별 초기 딜레이 (Wave Start Delay)
            // 필요하다면 유지하고, 위에서 만든 gameStartWaitTime만 쓰고 싶다면 이 부분은 0으로 설정하면 됩니다.
            if (currentWaveData.initialStartDelay > 0)
            {
                Debug.Log($"Wave {_currentWaveIndex + 1} Starting in {currentWaveData.initialStartDelay} seconds...");
                yield return new WaitForSeconds(currentWaveData.initialStartDelay);
            }

            Debug.Log($"Wave {_currentWaveIndex + 1} Started!");
            uiChannel.RaiseEvent(UIEvents.WaveEvent.Init((_currentWaveIndex + 1), waves[_currentWaveIndex], true));

            // 웨이브 내의 각 세그먼트(단계) 실행
            foreach (var segment in currentWaveData.segments)
            {
                List<Coroutine> runningSpawns = new List<Coroutine>();

                foreach (var info in segment.spawnList)
                {
                    Coroutine co = StartCoroutine(SpawnRoutine(info));
                    runningSpawns.Add(co);
                }

                // 현재 세그먼트 소환 완료 대기
                foreach (var co in runningSpawns)
                {
                    yield return co;
                }

                // 세그먼트 간 딜레이
                if (segment.delayAfterSegment > 0)
                {
                    yield return new WaitForSeconds(segment.delayAfterSegment);
                }
            }

            _isSpawningFinished = true;
            CheckWaveClear();
        }

        // ... (이하 SpawnRoutine, SpawnEnemy, GetValidSpawnPosition 등 기존 코드와 동일) ...

        private IEnumerator SpawnRoutine(EnemyWaveDataSO.SpawnInfo info)
        {
            if (info.startDelay > 0)
                yield return new WaitForSeconds(info.startDelay);

            for (int i = 0; i < info.count; i++)
            {
                SpawnEnemy(info.enemyPrefab);
                yield return new WaitForSeconds(info.interval);
            }
        }

        private void SpawnEnemy(GameObject prefab)
        {
            if (prefab == null) return;

            Enemy enemyComponent = prefab.GetComponent<Enemy>();
            float requiredDistance = (enemyComponent != null && enemyComponent.data != null)
                                    ? enemyComponent.data.spawnInterval
                                    : 1.0f;

            Vector3 spawnPos = GetValidSpawnPosition(requiredDistance);

            GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
            Enemy enemy = obj.GetComponent<Enemy>();

            if (enemy != null)
            {
                _activeEnemies.Add(enemy);
            }
        }

        private Vector3 GetValidSpawnPosition(float minDistance)
        {
            int maxAttempts = 30;
            Vector3 bestPosition = new Vector3(SPAWN_X_MIN, 0, 0);
            float maxDistFound = -1f;

            for (int i = 0; i < maxAttempts; i++)
            {
                float randomX = Random.Range(SPAWN_X_MIN, SPAWN_X_MAX);
                float randomY = Random.Range(-SPAWN_Y_RANGE, SPAWN_Y_RANGE);
                Vector3 candidatePos = new Vector3(randomX, randomY, 0);

                float closestDist = float.MaxValue;
                bool isTooClose = false;

                for (int j = _activeEnemies.Count - 1; j >= 0; j--)
                {
                    Enemy activeEnemy = _activeEnemies[j];
                    if (activeEnemy == null || activeEnemy.gameObject == null)
                    {
                        _activeEnemies.RemoveAt(j);
                        continue;
                    }
                    if (activeEnemy.transform.position.x < 5f) continue;

                    float dist = Vector3.Distance(candidatePos, activeEnemy.transform.position);
                    if (dist < closestDist) closestDist = dist;
                    if (dist < minDistance) isTooClose = true;
                }

                if (!isTooClose) return candidatePos;

                if (closestDist > maxDistFound)
                {
                    maxDistFound = closestDist;
                    bestPosition = candidatePos;
                }
            }
            return bestPosition;
        }

        public void RemoveEnemy(Enemy enemy)
        {
            if (_activeEnemies.Contains(enemy))
            {
                _activeEnemies.Remove(enemy);
            }
            CheckWaveClear();
        }

        private void CheckWaveClear()
        {
            _activeEnemies.RemoveAll(x => x == null);

            if (_isSpawningFinished && _activeEnemies.Count == 0)
            {
                uiChannel.RaiseEvent(UIEvents.WaveEvent.Init((_currentWaveIndex + 1), waves[_currentWaveIndex], false));
                Debug.Log($"Wave {_currentWaveIndex + 1} Clear!");
                StartNextWave();
            }
        }

        private void StartNextWave()
        {
            _currentWaveIndex++;

            if (_currentWaveIndex < waves.Count)
            {
                StartCoroutine(ProcessWave());
            }
            else
            {
                Debug.Log("Game Clear: All waves finished.");
            }
        }
    }
}