using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    /// <summary>
    /// 플레이어 캐릭터를 제어하는 클래스
    /// </summary>
    public class Player : MonoBehaviour
    {
        [Header("플레이어 이동 설정")]
        [SerializeField] private float _moveSpeed = 5f; // 이동 속도

        [Header("플레이어 상태")]
        [SerializeField] private int _health = 1; // 플레이어 HP

        [Header("플레이어 총알 설정")]
        [SerializeField] private GameObject _bulletPrefab; // 플레이어 총알 프리팹
        [SerializeField] private Transform _bulletSpawnPoint; // 총알 생성 위치
        [SerializeField] private float _fireRate = 0.5f; // 발사 속도
        private float _nextFireTime = 0f; // 다음 발사 가능 시간

        private Vector3 _boundaryMin = new Vector3(-7.5f, -4.5f, 0); // 검은 박스 최소 좌표
        private Vector3 _boundaryMax = new Vector3(7.5f, 4.5f, 0); // 검은 박스 최대 좌표
        
        private void Update()
        {
            HandleMovement();
            HandleFire();
        }
        
        // Getter
        public float GetFireRate()
        {
            return _fireRate;
        }

        // Setter
        public void SetFireRate(float newRate)
        {
            _fireRate = newRate;
        }

        public int GetHealth()
        {
            return _health;
        }
        
        /// <summary>
        /// 플레이어 이동 처리
        /// </summary>
        private void HandleMovement()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontal, vertical, 0);
            transform.Translate(direction.normalized * _moveSpeed * Time.deltaTime);

            // 검은 박스 경계 내에서만 이동
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, _boundaryMin.x, _boundaryMax.x),
                Mathf.Clamp(transform.position.y, _boundaryMin.y, _boundaryMax.y),
                0
            );
        }
        
        /// <summary>
        /// 플레이어 총알 발사 처리
        /// </summary>
        private void HandleFire()
        {
            if (Time.time >= _nextFireTime)
            {
                Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
                _nextFireTime = Time.time + _fireRate;
            }
        }
        
        /// <summary>
        /// 데미지 처리
        /// </summary>
        public void TakeDamage()
        {
            _health--;
            
            if (_health <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// 플레이어 사망 처리
        /// </summary>
        private void Die()
        {
            Debug.Log("플레이어 죽음!");
            Destroy(gameObject);
            SceneManager.LoadScene("MainMenuScene");
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 적이나 적의 총알에 맞았을 때 체력 감소
            if (collision.CompareTag("Enemy") || collision.CompareTag("EnemyBullet"))
            {
                TakeDamage();
            }
        }
    }
}