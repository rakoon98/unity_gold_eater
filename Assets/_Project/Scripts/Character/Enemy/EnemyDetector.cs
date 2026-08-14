using UnityEngine;

namespace GoldEater
{

    // Vector2.Distance(...) -> Physics2D.OverlapCircle
    public class EnemyDetector : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private float detectionRadius = 6f;

        [Header("Attack")]
        [SerializeField] private float attackRadius = 1.5f;

        [Header("Layer")]
        [SerializeField] private LayerMask playerLayer;

        public Transform target { get; private set; } // 플레이어
        public bool hasTarget => target != null;


        private void Update()
        {
            DetectPlayer();
        }

        private void DetectPlayer()
        {
            Collider2D hit = Physics2D.OverlapCircle(
                transform.position,
                detectionRadius,
                playerLayer);

            target = hit ? hit.transform : null;
        }

        public bool IsInAttackRange()
        {
            if (!hasTarget)
                return false;

            return Vector2.Distance(transform.position, target.position) <= attackRadius;
        }
      
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
#endif
    }
}
