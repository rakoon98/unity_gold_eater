using GoldEater;
using UnityEngine;

/// <summary>
/// [Move] 계층
/// 실제 Rigidbody2D를 다루는 최하위 레이어. 컨트롤러가 내려준 값을 물리엔진에
/// 반영하기만 하고, "왜 이동하는지"에 대한 판단은 절대 하지 않는다.
/// FixedUpdate에서만 물리 값을 갱신해 물리 스텝과 프레임 레이트를 분리한다.
/// </summary>
public class PlayerMove : MonoBehaviour
{

    [Header("스탯")] 
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private StatComponent playerStat;
    public float DashDuration => dashDuration;
    public float MoveSpeed => playerStat.GetStat(StatType.MoveSpeed);
    public float JumpSpeed => playerStat.GetStat(StatType.JumpSpeed);
    public float DashSpeed => playerStat.GetStat(StatType.DashSpeed);

    [Header("공격 중 이동 제한")]
    [SerializeField] public float attackMoveMultiplier = 0.05f;

    [Header("지면 판정")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;

    [Header("2단 점프")]
    private int maxJumpCount = 2;
    private int currentJumpCount = 0;

    public bool isGrounded { get; private set; }
    public bool isInvincible { get; private set; }

    private Rigidbody2D rb;
    private float targetVelocityX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStat = GetComponent<StatComponent>();
    }

    private void Start()
    {
    }

    void FixedUpdate()
    {
        isGrounded = groundCheck != null &&
            Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if(isGrounded)
            ResetJumpCount();

        rb.linearVelocity = new Vector2(targetVelocityX, rb.linearVelocity.y);
        //rb.linearVelocityX = targetVelocityX;
    }

    
    // input.moveX
    public void SetHorizontalVelocity(float velocityX) => targetVelocityX = velocityX;

    public void Jump()
    {
        // 현실적인 물리감에 적합.
        //rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f); // 기존 수직 속도를 지우고 점프해 이중 점프처럼 튀는 현상 방지
        //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JumpSpeed);
    }

    /// <summary>대쉬 등으로 인한 무적 상태를 외부(HealthComponent 등)에 알림</summary>
    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }

    public void OnJumpCountIncrease() => ++currentJumpCount;
    public void ResetJumpCount() => currentJumpCount = 0;
    public bool IsCanDoubleJump()
    {
        return currentJumpCount < maxJumpCount;
    }

    public void SetDashVelocity(float velocityX)
    {
        targetVelocityX = velocityX;
        rb.linearVelocity = new Vector2(velocityX, 0f); // 즉시 반영 + Y를 0으로 고정
    }








    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}