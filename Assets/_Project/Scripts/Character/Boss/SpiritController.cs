using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GoldEater
{

    public class SpiritController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float contactDamage = 1f;
        [SerializeField] private float lifeTime = 6f; // 이 시간 지나도 안 닿으면 자동 소멸
        [SerializeField] private float bobAmplitude = 0.1f; // 부유감
        [SerializeField] private float bobFrequency = 2f;


        [SerializeField] private SpiritAnimator animator; // Animator 직접 대신 SpiritAnimator
        private Rigidbody2D rb;
        private Transform player;
        private bool isDying;
        private float baseY;

        EnemyHealth health;

        private void Awake()
        {
            animator = GetComponentInChildren<SpiritAnimator>();
            rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic; // 물리 힘 안 받고 스크립트로만 이동

            health = GetComponent<EnemyHealth>();
            health.OnDead += () => SelfDestructAsync().Forget();
        }

        private async void Start()
        {
            animator.PlaySummon(); // animator.Play("summon") 대신
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f));
            await FindPlayerAsync();
            animator.PlayIdle();
            LifeTimeoutAsync().Forget();
        }

        private async UniTask FindPlayerAsync()
        {
            while (player == null)
            {
                var found = GameObject.FindGameObjectWithTag("Player");
                if (found != null) player = found.transform;
                else await UniTask.Yield();
            }
        }

        private void Update()
        {
            if (isDying || player == null) return;

            // 플레이어 방향으로 서서히 이동
            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            Vector2 nextPos = (Vector2)transform.position + dir * moveSpeed * Time.deltaTime;

            // 위아래 부유감 추가
            float bob = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
            nextPos.y += bob * Time.deltaTime;

            rb.MovePosition(nextPos);

            //// 좌우 반전 (스프라이트가 방향 있는 경우)
            //if (dir.x != 0)
            //{
            //    Vector3 scale = transform.localScale;
            //    scale.x = Mathf.Abs(scale.x) * (dir.x < 0 ? -1 : 1);
            //    transform.localScale = scale;
            //}

            if (Mathf.Abs(dir.x) > 0.01f)
                animator.SetFacing(dir.x);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //if (isDying) return;

            //if (other.TryGetComponent<IDamageable>(out var damageable) && !damageable.isDead)
            //{
            //    damageable.TakeDamage(contactDamage);
            //    SelfDestructAsync().Forget();
            //}

            var damageable = other.GetComponentInParent<IDamageable>();

            if (damageable != null && !damageable.isDead)
            {
                //float damage = bossStat.GetStat(StatType.Attack);
                //damageable.TakeDamage(damage);
                damageable.TakeDamage(contactDamage);
                SelfDestructAsync().Forget();
            }
        }

        private async UniTaskVoid LifeTimeoutAsync()
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(lifeTime));
            if (!isDying)
                SelfDestructAsync().Forget();
        }

        private async UniTaskVoid SelfDestructAsync()
        {
            if (isDying) return;
            isDying = true;

            animator.PlayDeath();
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f)); // death 클립 길이에 맞게 조정

            Destroy(gameObject);
        }

        // 보스 공격(플레이어 무기)에 맞아 죽는 경우도 처리하고 싶다면 IDamageable 구현 추가 가능
    }
}