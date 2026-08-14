using UnityEngine;

namespace GoldEater
{
    public class BossHitboxRelay : MonoBehaviour
    {
        private BossAttackHitbox bossAttackHitbox;

        private void Awake()
        {
            // 부모(HitBox)에 붙어있는 BossAttackHitbox를 찾아 가져옵니다.
            bossAttackHitbox = GetComponentInParent<BossAttackHitbox>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (bossAttackHitbox != null)
            {
                bossAttackHitbox.HandleHit(other);
            }
        }
    }
}