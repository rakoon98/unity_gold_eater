using UnityEngine;

namespace GoldEater
{
    [CreateAssetMenu(fileName = "EnemyStatsData", menuName = "GoldEaterSO/EnemyStatsData")]
    public class EnemyStatsData : ScriptableObject
    {
        [Header("Health")]
        public int maxHp = 100;

        [Header("Attack")]
        public int attackDamage = 15;
        public float attackRadius = 1f;
        public float attackCooldown = 3f;

        [Header("Attack Animation Timing")]
        public int animationSample = 12;
        public int attackHitFrame = 10;
        public int attackEndFrame = 18;

        [Header("Hurt")]
        public float hurtDuration = 0.3f;
    }

}