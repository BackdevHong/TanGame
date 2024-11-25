using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager
{
    /// <summary>
    /// 씬이 시작될 때 플레이어와 적들을 생성하는 클래스
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
        [Header("플레이어 설정")]
        [SerializeField] private GameObject _playerPrefab; // 플레이어 프리팹
        [SerializeField] private Transform _playerSpawnPoint; // 플레이어 생성 위치

        [Header("적 생성 설정")]
        [SerializeField] private GameObject _normalEnemyPrefab;
        [SerializeField] private GameObject _rareEnemyPrefab;
        [SerializeField] private GameObject _bossEnemyPrefab;

        [SerializeField] private Transform[] _enemySpawnPoints; // 적 생성 위치 배열

        [Header("레벨 설정")]
        [SerializeField] private int _currentLevel = 1; // 현재 레벨

        private void Start()
        {
            SpawnPlayer();
        }

        /// <summary>
        /// 플레이어를 생성합니다.
        /// </summary>
        private void SpawnPlayer()
        {
            if (_playerPrefab != null && _playerSpawnPoint != null)
            {
                Instantiate(_playerPrefab, _playerSpawnPoint.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("플레이어 프리팹 또는 스폰 위치가 설정되지 않았습니다!");
            }
        }

        /// <summary>
        /// 적들을 생성합니다.
        /// </summary>
        public void SpawnEnemies()
        {
            Debug.Log($"현재 레벨: {_currentLevel}"); // 디버깅용 로그
            
            if (_currentLevel % 5 == 0)
            {
                SpawnBossEnemy();
            }
            else
            {
                SpawnNormalAndRareEnemies();
            }
        }
        /// <summary>
        /// 일반 몹과 레어 몹을 생성합니다.
        /// </summary>
        private void SpawnNormalAndRareEnemies()
        {
            int normalEnemyCount = Random.Range(1, 7); // 1~6개 랜덤 생성
            int rareEnemyCount = Random.Range(1, 4); // 1~3개 랜덤 생성

            SpawnEnemy(_normalEnemyPrefab, normalEnemyCount);
            SpawnEnemy(_rareEnemyPrefab, rareEnemyCount);
        }

        /// <summary>
        /// 보스 몹을 생성합니다.
        /// </summary>
        private void SpawnBossEnemy()
        {
            if (_bossEnemyPrefab != null && _enemySpawnPoints.Length > 0)
            {
                Transform spawnPoint = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Length)];
                Instantiate(_bossEnemyPrefab, spawnPoint.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("보스 몹 프리팹 또는 적 스폰 위치가 설정되지 않았습니다!");
            }
        }

        /// <summary>
        /// 특정 적을 지정된 개수만큼 생성합니다.
        /// </summary>
        private void SpawnEnemy(GameObject enemyPrefab, int count)
        {
            if (enemyPrefab != null && _enemySpawnPoints.Length > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Transform spawnPoint = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Length)];
                    Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                }
            }
            else
            {
                Debug.LogError("적 프리팹 또는 스폰 위치가 설정되지 않았습니다!");
            }
        }

        public void UpdateLevel()
        {
            _currentLevel++;
        }
    }
}