using UnityEngine;

namespace GoldEater
{
    public class EnemyAttack : MonoBehaviour
    {

        [Header("스탯")]
        private StatComponent enemyStat;
        float MoveSpeed => enemyStat.GetStat(StatType.MoveSpeed);
        float damage => enemyStat.GetStat(StatType.Attack);

        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRadius = 1f;
        //[SerializeField] private int attackDamage = 15;
        [SerializeField] private LayerMask playerLayer;

        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private float effectLifeTime = 1f;

        [Header("쿨타임 설정")]
        [SerializeField] private float attackCooldown = 3f;
        private float nextAttackTime;

        // 공격 가능 여부 확인
        public bool canAttack => Time.time >= nextAttackTime;

        // 쿨타임 적용
        public void ApplyCooldown()
        {
            nextAttackTime = Time.time + attackCooldown;
        }


        private void Awake()
        {
            if (enemyStat == null)
                enemyStat = GetComponentInParent<StatComponent>();
        }

        public void Attack()
        {
            // OverlapCircleAll -> 추후 단체공격
            Collider2D hit = Physics2D.OverlapCircle(attackPoint.position,attackRadius,playerLayer);
            if (hit == null) return;

            IDamageable damageable = hit.GetComponentInParent<IDamageable>();
            if (damageable == null) return;

            damageable.TakeDamage(damage);
            HitEffectSpawner.Spawn(hitEffectPrefab, attackPoint.position, effectLifeTime);
        }






#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
#endif
    }
}