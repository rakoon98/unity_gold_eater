using System;
using UnityEngine;

namespace GoldEater
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        private StatComponent enemyStat;

        public float maxHearts => enemyStat.GetStat(StatType.Health);
        public float currentHp { get; private set; }
        public bool isDead => currentHp <= 0;

        public event Action OnDamaged;
        public event Action OnDead;

        private void Awake()
        {
            if (enemyStat == null)
                enemyStat = GetComponent<StatComponent>();

        }

        private void Start()
        {
            currentHp = maxHearts;
        }

        public void TakeDamage(float amount)            
        {
            Debug.Log($"잡몹 데미지 받음!!!currentHp  {amount}");
            if (isDead)
                return;

            currentHp -= amount;
            currentHp = Mathf.Max(currentHp, 0);
            Debug.Log($"잡몹 데미지 받음!!!currentHp  {currentHp}");

            Debug.Log($"Enemy Hearts : {currentHp}/{maxHearts}");

            OnDamaged?.Invoke();
            if (isDead)            
                OnDead?.Invoke();
            
        }



    }

}