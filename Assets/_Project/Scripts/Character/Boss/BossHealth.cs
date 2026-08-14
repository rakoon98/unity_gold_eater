using GoldEater;
using System;
using UnityEngine;

public class BossHealth : MonoBehaviour, IDamageable
{

    private BossAnimator bossAnimator;

    private StatComponent enemyStat;
    public float maxHp => enemyStat.GetStat(StatType.Health);
    public float currentHp { get; private set; }

    [SerializeField] private float phase2Threshold = 0.5f; // 체력 50%

    private bool isDead;
    private bool phase2Triggered;

    public bool IsDead => isDead;
    public float CurrentHp => currentHp;
    public float MaxHp => maxHp;

    bool IDamageable.isDead => currentHp <= 0;

    // 외부(BossController, UI 등)에서 구독
    public event Action<float, float> OnHealthChanged; // (current, max)
    public event Action OnPhase2Entered;
    public event Action OnDeath;

    public event Action OnDestroyBoss;

    private void Awake()
    {
        if (enemyStat == null)
            enemyStat = GetComponent<StatComponent>();

        bossAnimator = GetComponentInChildren<BossAnimator>();
    }

    private void Start()
    {
        currentHp = maxHp;

        OnHealthChanged?.Invoke(currentHp, maxHp);
    }

    public void TakeDamage(float amount)
    {
        Debug.Log($"보스 데미지 받음!!!amount  {amount}");
        if (isDead) return;

        currentHp -= amount;
        currentHp = Mathf.Max(currentHp, 0f);
        Debug.Log($"보스 데미지 받음!!!currentHp  {currentHp}");

        OnHealthChanged?.Invoke(currentHp, maxHp);

        bossAnimator.PlayHitFlash();

        // 데미지 팝업 연동 (공통 재사용) -> 보스 체력은 최상단에 크게할거라 다르게
        // DamagePopupSpawner.Instance.Spawn(transform.position, amount, false);

        if (!phase2Triggered && currentHp <= maxHp * phase2Threshold)
        {
            phase2Triggered = true;
            OnPhase2Entered?.Invoke();
        }

        if (currentHp <= 0f && !isDead)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }

    private void OnDestroy()
    {
        //OnHealthChanged?.Invoke(0, maxHp);

        //UIManager.instance.bossHealthUI.Reset();

        OnDestroyBoss?.Invoke();

        // 이벤트 정리
        OnHealthChanged = null;
        OnPhase2Entered = null;
        OnDeath = null;
    }
}
