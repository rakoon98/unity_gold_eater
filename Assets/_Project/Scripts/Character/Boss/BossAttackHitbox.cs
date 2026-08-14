using UnityEngine;

namespace GoldEater
{
    public class BossAttackHitbox : MonoBehaviour
    {
        [SerializeField] private Collider2D hitCollider1; // 1타용
        [SerializeField] private Collider2D hitCollider2; // 2타용
        private StatComponent bossStat;

        private void Awake()
        {
            hitCollider1.enabled = false;
            hitCollider2.enabled = false;
            bossStat = GetComponentInParent<StatComponent>();
        }

        public void Activate(int comboIndex)
        {
            if (comboIndex == 0) hitCollider1.enabled = true;
            if (comboIndex == 1) hitCollider2.enabled = true;
        }

        public void Deactivate(int comboIndex)
        {
            if (comboIndex == 0) hitCollider1.enabled = false;
            if (comboIndex == 1) hitCollider2.enabled = false;
        }

        // 기존 private void OnTriggerEnter2D(Collider2D other) 함수 이름을 public으로 변경
        public void HandleHit(Collider2D other)
        {
            Debug.Log($"[BossHit] 트리거 부딪힘: {other.gameObject.name}");

            var damageable = other.GetComponentInParent<IDamageable>();
            if (damageable == null)
            {
                Debug.Log("[BossHit] IDamageable을 찾지 못함");
                return;
            }

            if (bossStat == null)
            {
                Debug.LogError("[BossHit] bossStat이 null입니다!");
                return;
            }

            float damage = bossStat.GetStat(StatType.Attack);
            damageable.TakeDamage(damage);
            Debug.Log($"[BossHit] 데미지 전달 성공: {damage}");
        }
    }
}