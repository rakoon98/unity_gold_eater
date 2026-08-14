using UnityEngine;

namespace GoldEater
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMove : MonoBehaviour
    {

        [Header("스탯")]
        [SerializeField] private StatComponent enemyStat;
        public float MoveSpeed => enemyStat.GetStat(StatType.MoveSpeed);

        [SerializeField] private Transform edgeCheck; // 몹 앞쪽 발밑에 배치
        [SerializeField] private float edgeCheckDistance = 1.5f;
        [SerializeField] private LayerMask groundLayer;

        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;

        private Vector2 moveDirection;

        private const float directionDeadzone = 0.05f;
        public float FacingDirection => moveDirection.x;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = rb.GetComponentInChildren<SpriteRenderer>();

            if (enemyStat == null)
                enemyStat = GetComponent<StatComponent>();

            Debug.Log($"groundLayer value: {groundLayer.value}, Ground layer index: {LayerMask.NameToLayer("Ground")}, Ground layer bit: {1 << LayerMask.NameToLayer("Ground")}");
        }

        public void MoveTo(Vector3 targetPosition)
        {
            //if (IsAboutToFallOffEdge())
            //{
            //    Debug.Log($"[{gameObject.name}] 낭떠러지 감지로 Stop 호출됨");
            //    Stop();
            //    return;
            //}

            float dx = targetPosition.x - transform.position.x;
            //Debug.Log($"[{gameObject.name}] dx: {dx}, target: {targetPosition}, my pos: {transform.position}");

            if (Mathf.Abs(dx) < directionDeadzone)
                return; // 너무 가까우면 방향(및 flip) 갱신 안 하고 이전 상태 유지

            Vector3 direction = new Vector3(dx, 0f, 0f);
            moveDirection = direction.normalized;
        }

        private bool IsAboutToFallOffEdge()
        {
            bool noGroundFound = !Physics2D.Raycast(edgeCheck.position, Vector2.down, edgeCheckDistance, groundLayer);
            //Debug.Log($"[{gameObject.name}({GetInstanceID()})] edgeCheck pos: {edgeCheck.position}, noGroundFound: {noGroundFound}");
            return noGroundFound;
        }

        public void StopMove()
        {
            moveDirection = Vector2.zero;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        public void StopAll()
        {
            moveDirection = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
        }

        private void FixedUpdate()
        {
            rb.linearVelocity = new Vector2(moveDirection.x * MoveSpeed, rb.linearVelocity.y);
        }

        private void OnDrawGizmos()
        {
            if (edgeCheck == null) return;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * edgeCheckDistance);
            Gizmos.DrawWireSphere(edgeCheck.position, 0.05f);
        }
    }

}