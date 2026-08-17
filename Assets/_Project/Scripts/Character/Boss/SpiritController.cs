using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GoldEater
{
    [RequireComponent(typeof(StatComponent))]
    public class SpiritController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float lifeTime = 6f;
        [SerializeField] private float bobAmplitude = 0.1f;
        [SerializeField] private float bobFrequency = 2f;

        [SerializeField] private SpiritAnimator animator;

        // StatComponent에서 1회만 캐싱하여 사용할 스탯 값
        private float moveSpeed;
        private float contactDamage;

        private Rigidbody2D rb;
        private Collider2D[] allColliders;
        private Transform player;
        private bool isDying;

        private EnemyHealth health;
        private StatComponent statComponent;

        private void Awake()
        {
            animator = GetComponentInChildren<SpiritAnimator>();
            rb = GetComponent<Rigidbody2D>();
            allColliders = GetComponentsInChildren<Collider2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;

            health = GetComponent<EnemyHealth>();
            statComponent = GetComponent<StatComponent>();

            if (health != null)
            {
                health.OnDead += () => SelfDestructAsync().Forget();
            }
        }

        private async void Start()
        {
            // StatComponent.Awake() 이후 시점인 Start에서 1회만 캐싱
            CacheStats();

            animator.PlaySummon();
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f));
            await FindPlayerAsync();
            animator.PlayIdle();
            LifeTimeoutAsync().Forget();
        }

        private void CacheStats()
        {
            if (statComponent != null)
            {
                moveSpeed = statComponent.GetStat(StatType.MoveSpeed);
                contactDamage = statComponent.GetStat(StatType.Attack);
            }
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

            // 미리 캐싱된 moveSpeed 사용
            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            Vector2 nextPos = (Vector2)transform.position + dir * moveSpeed * Time.deltaTime;

            float bob = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
            nextPos.y += bob * Time.deltaTime;

            rb.MovePosition(nextPos);

            if (Mathf.Abs(dir.x) > 0.01f)
                animator.SetFacing(dir.x);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isDying) return;

            if (other.CompareTag("Player"))
            {
                var damageable = other.GetComponentInParent<IDamageable>();

                if (damageable != null && !damageable.isDead)
                {
                    DisableAllColliders();

                    // 미리 캐싱된 contactDamage 적용
                    damageable.TakeDamage(contactDamage);

                    SelfDestructAsync().Forget();
                }
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

            DisableAllColliders();

            animator.PlayDeath();
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f));

            Destroy(gameObject);
        }

        private void DisableAllColliders()
        {
            if (allColliders == null) return;
            foreach (var col in allColliders)
            {
                if (col != null) col.enabled = false;
            }
        }
    }
}