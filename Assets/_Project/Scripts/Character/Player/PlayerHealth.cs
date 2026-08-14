using System;
using UnityEngine;

namespace GoldEater
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        private const int maxHearts = 3;
        private const float maxHP = maxHearts; // 3.0 (하트 1개 = 1.0, 0.5 단위로 반칸 표현)

        public float currentHP { get; private set; }
        public bool isDead => currentHP <= 0f;

        public event Action OnDamaged;
        public event Action OnDead;

        private void Awake()
        {
            currentHP = maxHP;
        }

        public void ResetForRetry()
        {
            currentHP = maxHP;

            var controller = GetComponent<PlayerController>();
            controller.rb.linearVelocity = Vector2.zero;
            controller.transform.rotation = Quaternion.identity;
            controller.stateMachine.ChangeState(controller.idleState);

            OnDamaged?.Invoke();
        }

        public void TakeDamage(float amount)
        {
            if (isDead) return;

            currentHP -= amount;
            currentHP = Mathf.Max(currentHP, 0f);

            Debug.Log($"Player Hearts: {currentHP}/{maxHP}");

            OnDamaged?.Invoke();

            if (isDead)
                OnDead?.Invoke();
        }

        public void Heal(float amount)
        {
            currentHP = Mathf.Min(currentHP + amount, maxHP);
            OnDamaged?.Invoke(); // UI 갱신용으로 재사용
        }

        /// <summary>인덱스(0~2)에 해당하는 하트 상태를 반환</summary>
        public HeartState GetHeartState(int heartIndex)
        {
            float heartValue = currentHP - heartIndex;

            if (heartValue >= 1f)
                return HeartState.Full;
            else if (heartValue >= 0.5f)
                return HeartState.Half;
            else
                return HeartState.Empty;
        }

        public int MaxHearts => maxHearts;


    }

    public enum HeartState
    {
        Empty,
        Half,
        Full
    }
}