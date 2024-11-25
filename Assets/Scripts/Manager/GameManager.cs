using System;
using Enemies;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        [Header("레벨 설정")]
        [SerializeField] private int _currentLevel = 1; // 현재 레벨
        [SerializeField] private int _maxLevel = 10; // 최대 레벨 (필요시 설정)

        [Header("필수 매니저")]
        [SerializeField] private SpawnManager _spawnManager; // SpawnManager 참조
        [SerializeField] private UIManager _uiManager; // UIManager 참조
        
        private Player.Player _player;
        
        private void Start()
        {
            _player = FindFirstObjectByType<Player.Player>();
            
            if (_spawnManager == null)
            {
                Debug.LogError("SpawnManager가 설정되지 않았습니다!");
                return;
            }
            
            // UIManager 연결 확인
            if (_uiManager == null)
            {
                Debug.LogError("UIManager가 설정되지 않았습니다!");
                return;
            }

            // 초기 레벨 시작
            StartLevel();
        }

        private void Update()
        {
            _uiManager.UpdateHP(_player.GetHealth());
        }

        /// <summary>
        /// 레벨을 시작합니다.
        /// </summary>
        private void StartLevel()
        {
            Debug.Log($"레벨 {_currentLevel} 시작!");
            _spawnManager.SpawnEnemies();
        }

        /// <summary>
        /// 적이 사망했을 때 호출됩니다.
        /// </summary>
        public void OnEnemyKilled()
        {
            Debug.Log($"남은 적의 수: {CountEnemies()}");

            if (CountEnemies() <= 1)
            {
                // 다음 레벨로 이동
                MoveToNextLevel();
            }
        }

        /// <summary>
        /// 다음 레벨로 이동합니다.
        /// </summary>
        private void MoveToNextLevel()
        {
            if (_currentLevel < _maxLevel)
            {
                _currentLevel++;
                _spawnManager.UpdateLevel();
                StartLevel();
                _uiManager.UpdateLevel(_currentLevel);
            }
            else
            {
                Debug.Log("모든 레벨 완료! 게임 종료!");
                EndGame();
            }
        }
        
        /// 현재 씬에서 유효한 적의 수를 세는 함수
        /// </summary>
        /// <returns>남아있는 적의 수</returns>
        private static int CountEnemies()
        {
            BaseEnemy[] enemies = FindObjectsByType<BaseEnemy>(FindObjectsSortMode.InstanceID); // 씬에서 모든 BaseEnemy 검색
            int validEnemyCount = 0;

            foreach (BaseEnemy enemy in enemies)
            {
                // 오브젝트가 파괴되지 않았으면 카운트
                if (enemy != null && enemy.gameObject.activeInHierarchy)
                {
                    validEnemyCount++;
                }
            }

            return validEnemyCount;
        }

        /// <summary>
        /// 게임 종료 로직
        /// </summary>
        private static void EndGame()
        {
            Debug.Log("축하합니다! 게임을 클리어했습니다!");
            // 게임 종료 화면 또는 메인 메뉴로 이동 구현 가능
        }
    }
}