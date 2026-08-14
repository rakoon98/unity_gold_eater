using Cysharp.Threading.Tasks;
using GoldEater;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimator : MonoBehaviour
{

    private HitFlashEffect hitFlash;

    [SerializeField] private Transform body;
    [SerializeField] private Collider2D collider;

    [SerializeField] private Animator animator;

    [SerializeField] private Transform hitBox1;
    [SerializeField] private Transform hitBox2;

    // 상태 캐싱
    private static readonly int IdleHash = Animator.StringToHash("Idle");
    private static readonly int MoveHash = Animator.StringToHash("Run");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int FallHash = Animator.StringToHash("Fall");
    private static readonly int DashHash = Animator.StringToHash("Dash");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int HurtHash = Animator.StringToHash("Hurt");
    private static readonly int DeathHash = Animator.StringToHash("Death");

    private void Awake()
    {
        hitFlash = new HitFlashEffect(transform, Color.red);
    }

    private void Start()
    {
        //spum.OverrideControllerInit();
    }

    private void OnDestroy() => hitFlash?.Dispose();

    private void LateUpdate()
    {
        // Body가 flip/애니메이션으로 벗어난 만큼 콜라이더 offset을 보정
        Vector2 offset = collider.offset;
        offset.x = body.localPosition.x;
        collider.offset = offset;
    }

    public void SetFacing(float direction)
    {
        Vector3 scale = body.localScale;
        scale.x = direction > 0 ? 1 : -1; //  오른쪽
        //scale.x = direction > 0 ? -1 : 1; // 기본방향이 왼쪽
        body.localScale = scale;


        // 히트박스 로컬 X 위치도 같이 반전
        var p1 = hitBox1.localPosition;
        p1.x = Mathf.Abs(p1.x) * direction;
        hitBox1.localPosition = p1;

        var p2 = hitBox2.localPosition;
        p2.x = Mathf.Abs(p2.x) * direction;
        hitBox2.localPosition = p2;
    }

    public void SetSpeed(float speed) => animator.speed = speed;

    public void PlayIdle() => animator.Play(IdleHash);
    public void PlayMove() => animator.Play(MoveHash);
    public void PlayJump() => animator.Play(JumpHash);
    public void PlayFall() => animator.Play(FallHash);
    public void PlayDash() => animator.Play(DashHash);
    public void PlayAttack() => animator.Play(AttackHash); // 분기 나중에 콤보
    
    public void PlayHit()
    {
        animator.Play(HurtHash);
        PlayHitFlash();
    }
    public void PlayDead() => animator.Play(DeathHash);

    public void PlayHitFlash() => hitFlash.Play();

    public void ResetAnimation()
    {
        animator.Rebind();
        animator.Update(0f);
    }
}
