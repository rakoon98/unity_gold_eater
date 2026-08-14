using UnityEngine;

namespace GoldEater
{
    [CreateAssetMenu(fileName = "PlayerStatsData", menuName = "GoldEaterSO/Player Stats")]
    public class PlayerStatsData : ScriptableObject
    {
        [Header("Health")]
        public int maxHp = 100;

        [Header("Move")]
        public float moveSpeed = 5f;

        [Header("Dash")]
        public float dashDistance = 3f;
        public float dashDuration = 0.15f;
        public float dashInvincibleTime = 0.3f;
        public float dashCooldown = 1.2f;

        [Header("Attack")]
        public int[] comboDamage = { 10, 10, 15 }; // 나중에 콤보별로 다르게 조정 가능
        public float attackDuration = 0.4f;
        public float hitTimingRatio = 0.4f;
        public float hitDurationRatio = 0.2f;

        [Header("Hurt")]
        public float hitStunDuration = 0.3f;
    }
}
