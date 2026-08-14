using UnityEngine;

namespace GoldEater
{
    public class PlayerAttack : MonoBehaviour
    {

        [SerializeField] private Collider2D hitCollider1; // 1타용
        [SerializeField] private Collider2D hitCollider2; // 2타용

        private StatComponent playerStat;
        public float attackDamage => playerStat.GetStat(StatType.Attack);
        public float attackSpeed => playerStat.GetStat(StatType.AttackSpeed);
        public float critDamage => playerStat.GetStat(StatType.CritDamage);
        public float critChance => playerStat.GetStat(StatType.CritChance);

        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private Transform hitEffectTransform;
        [SerializeField] private float effectLifeTime = 1f; 
       

        private void Awake()
        {
            ResetAttack();

            if (playerStat == null) playerStat = GetComponent<StatComponent>();
        }

        public void Activate(int comboIndex)
        {
            if (comboIndex == 0) hitCollider1.enabled = true;
            if(comboIndex == 1) hitCollider2.enabled = true;
        }
        public void Deactive(int comboIndex)
        {
            if (comboIndex == 0) hitCollider1.enabled = false;
            if (comboIndex == 1) hitCollider2.enabled = false;
        }

        // OnTriggerEnter2D 대신 외부에서 호출하는 공개 함수
        public void HandleHit(Collider2D other)
        {
            //if (other.TryGetComponent<IDamageable>(out var damageable) && !damageable.isDead)
            //{
            //    float damage = attackDamage;
            //    bool isCritical = Random.Range(0f, 100f) < critChance;
            //    if (isCritical)               
            //        damage *= playerStat.GetStat(StatType.CritDamage);                     

            //    damageable.TakeDamage(damage);
            //    HitEffectSpawner.Spawn(hitEffectPrefab, hitEffectTransform.position, effectLifeTime);

            //    DamagePopupSpawner.Instance.Spawn(
            //         transform.position + Vector3.up * 0.5f,
            //         damage,
            //         isCritical
            //     );
            //}

            Debug.Log($"[HandleHit] other.tag {other.tag}");
            var damageable = other.GetComponentInParent<IDamageable>();
            var damageable2 = other.GetComponentInChildren<IDamageable>();
            var damageable3 = other.GetComponent<IDamageable>();

            Debug.Log($"[HandleHit] 부딪힌 오브젝트 이름: {other.gameObject.name} damageable {damageable}");
            Debug.Log($"[HandleHit] 부딪힌 오브젝트 이름: {other.gameObject.name} damageable2 {damageable2}");
            Debug.Log($"[HandleHit] 부딪힌 오브젝트 이름: {other.gameObject.name} damageable3 {damageable3}");

            if (damageable != null && !damageable.isDead)
            {
                //float damage = bossStat.GetStat(StatType.Attack);
                //damageable.TakeDamage(damage);

                float damage = attackDamage;
                bool isCritical = Random.Range(0f, 100f) < critChance;
                if (isCritical)
                    damage *= playerStat.GetStat(StatType.CritDamage);

                damageable.TakeDamage(damage);
                HitEffectSpawner.Spawn(hitEffectPrefab, hitEffectTransform.position, effectLifeTime);

                DamagePopupSpawner.Instance.Spawn(
                     transform.position + Vector3.up * 0.5f,
                     damage,
                     isCritical
                 );
            }
        }


        public void ResetAttack()
        {
            if (hitCollider1 != null)
                hitCollider1.enabled = false;

            if (hitCollider2 != null)
                hitCollider2.enabled = false;
        }
    }
}