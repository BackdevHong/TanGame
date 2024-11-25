using UnityEngine;

namespace Projectiles
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f; // 총알 속도
        [SerializeField] private Vector3 _direction = Vector3.up; // 이동 방향 (기본값: 위쪽)

        /// <summary>
        /// 총알 이동 방향 설정
        /// </summary>
        /// <param name="direction">이동 방향</param>
        public void SetDirection(Vector3 direction)
        {
            _direction = direction.normalized;
        }

        private void Update()
        {
            // 총알 이동
            transform.Translate(_direction * _speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Obstacle"))
            {
                Destroy(gameObject);
            }
        }
    }
}