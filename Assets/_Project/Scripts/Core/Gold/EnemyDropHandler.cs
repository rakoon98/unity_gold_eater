using UnityEngine;

namespace GoldEater
{
    public class EnemyDropHandler : MonoBehaviour
    {
        //[SerializeField]
        private EnemyHealth enemyHealth;
        [SerializeField] private GameObject goldPrefab;
        [Range(0f, 1f)]
        [SerializeField] private float dropChance = 0.5f; // 50% È®·ü

        private void Awake()
        {
            if (enemyHealth == null)
                enemyHealth = GetComponent<EnemyHealth>();
        }

        private void OnEnable()
        {
            if (enemyHealth != null)
                enemyHealth.OnDead += HandleDeath;
        }

        private void OnDisable()
        {
            if (enemyHealth != null)
                enemyHealth.OnDead -= HandleDeath;
        }

        private void HandleDeath()
        {
            if (Random.value <= dropChance)
            {
                Instantiate(goldPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}