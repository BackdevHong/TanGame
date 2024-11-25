using System;
using Projectiles;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace Enemies
{
    /// <summary>
    /// 적 캐릭터의 기본 동작을 정의하는 클래스
    /// </summary>
    public abstract class BaseEnemy : MonoBehaviour
    {
        [Header("적 상태")]
        [SerializeField] protected int _health; // 적 HP
        [SerializeField] protected float _moveSpeed = 2f; // 이동 속도

        [Header("적 총알 설정")]
        [SerializeField] private GameObject _bulletPrefab; // 적 총알 프리팹
        [SerializeField] private Transform _bulletSpawnPoint; // 총알 생성 위치
        [SerializeField] private float _fireRate = 1f; // 발사 속도
        private float _nextFireTime = 0f;

        private Vector3 _boundaryMin = new Vector3(-7.5f, -4.5f, 0); // 검은 박스 최소 좌표
        private Vector3 _boundaryMax = new Vector3(7.5f, 4.5f, 0); // 검은 박스 최대 좌표
        
        [Header("적 이동 설정")]
        [SerializeField] private float _safeDistance = 2f; // 플레이어와 안전 거리
        [SerializeField] private float _safeDistanceFromOthers = 1.5f; // 다른 적과의 안전 거리
        private Transform _playerTransform; // 플레이어 위치 참조

        protected virtual void Update()
        {
            if (_playerTransform == null)
            {
                FindPlayer();
            }

            if (_playerTransform != null)
            {
                MoveInFrontOfPlayer();
                AvoidPlayerCollision();
            }
            
            AvoidOtherEnemies();
            HandleFire();
        }

        /// <summary>
        /// 플레이어를 찾아 설정
        /// </summary>
        private void FindPlayer()
        {
            Player.Player player = FindFirstObjectByType<Player.Player>();
            if (player != null)
            {
                _playerTransform = player.transform;
            }
        }

        /// <summary>
        /// 플레이어 앞에 머무르며 이동
        /// </summary>
        private void MoveInFrontOfPlayer()
        {
            // 플레이어의 위쪽 방향을 기준으로 이동
            Vector3 targetPosition = new Vector3(
                _playerTransform.position.x,
                Mathf.Max(_playerTransform.position.y + 2f, transform.position.y), // 플레이어 뒤로 이동 방지
                0
            );

            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.Translate(direction * _moveSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 플레이어와의 직접적인 충돌을 회피
        /// </summary>
        private void AvoidPlayerCollision()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

            if (distanceToPlayer < _safeDistance)
            {
                // 플레이어를 기준으로 좌우로 이동
                Vector3 avoidanceDirection = transform.position.x < _playerTransform.position.x
                    ? Vector3.left
                    : Vector3.right;

                transform.Translate(avoidanceDirection * (_moveSpeed * 0.5f) * Time.deltaTime);
            }
        }
        
        /// <summary>
        /// 다른 적들과의 충돌을 회피
        /// </summary>
        private void AvoidOtherEnemies()
        {
            BaseEnemy[] allEnemies = FindObjectsByType<BaseEnemy>(FindObjectsSortMode.None);

            foreach (BaseEnemy otherEnemy in allEnemies)
            {
                if (otherEnemy == this) continue; // 자기 자신은 건너뜀

                float distanceToOther = Vector3.Distance(transform.position, otherEnemy.transform.position);

                if (distanceToOther < _safeDistanceFromOthers)
                {
                    // 가까운 적으로부터 멀어지도록 이동
                    Vector3 avoidanceDirection = (transform.position - otherEnemy.transform.position).normalized;
                    transform.Translate(avoidanceDirection * (_moveSpeed * 0.5f) * Time.deltaTime);
                }
            }
        }
        
        /// <summary>
        /// 적의 총알 발사 처리
        /// </summary>
        protected virtual void HandleFire()
        {
            if (Time.time >= _nextFireTime && IsInFrontOfPlayer())
            {
                GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);

                // 총알 방향 설정
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.SetDirection(Vector3.down); // 아래 방향으로 발사
                }

                _nextFireTime = Time.time + _fireRate;
            }
        }
        
        /// <summary>
        /// 적이 플레이어의 앞에 있는지 확인
        /// </summary>
        private bool IsInFrontOfPlayer()
        {
            return _playerTransform != null && transform.position.y > _playerTransform.position.y;
        }

        /// <summary>
        /// 데미지 처리
        /// </summary>
        public virtual void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// 사망 처리
        /// </summary>
        protected virtual void Die()
        {
            Debug.Log($"{gameObject.name} has died.");
            Destroy(gameObject);
        }
    }
}