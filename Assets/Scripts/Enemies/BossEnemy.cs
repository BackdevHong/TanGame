﻿using Manager;
using UnityEngine;

namespace Enemies
{
    public class BossEnemy : BaseEnemy
    {
        private void Awake()
        {
            _health = Random.Range(10, 12);
            _moveSpeed = 1f; // 보스는 속도가 느림
        }

        protected override void Die()
        {
            base.Die();

            // GameManager에 적 사망 알림
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.OnEnemyKilled();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 총알에 맞았을 때 체력 감소
            if (collision.CompareTag("Bullet"))
            {
                TakeDamage(1); // 총알이 1의 데미지를 준다고 가정
                Destroy(collision.gameObject); // 총알 제거
            }
        }
    }
}