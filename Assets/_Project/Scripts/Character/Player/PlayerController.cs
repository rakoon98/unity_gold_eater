using System;
using UnityEngine;

namespace GoldEater
{
    /// <summary>
    /// [Controller] 계층
    /// PlayerInput의 값을 읽어 "무엇을 할지" 판단하고 PlayerMove에 명령만 내린다.
    /// 상태 관리(대쉬 중 여부, 쿨타임, 무적 타이머)와 게임 규칙이 이 레이어에 위치한다.
    /// 실제 물리 처리는 하지 않는다 — 그건 PlayerMove의 책임.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {

        public PlayerAttack playerAttack; // 공격 박스
        public Rigidbody2D rb;
        
        [Header("대쉬 — 상세기획서 '조작 & 프레임 데이터' 기준")]
        private float dashDistance = 3.0f;        // 이동 거리 3m
        private float dashDuration = 0.15f;       // 대쉬 지속시간(초)
        private float dashInvincibleTime = 0.3f;  // 무적 18f = 0.3초
        private float dashCooldown = 1.0f;        // 쿨타임 1.2초

        public PlayerInput input { get; private set; }
        public PlayerMove move { get; private set; }
        public PlayerAnimator anim { get; private set; }
        public PlayerHealth health { get; private set; }
        public GoldAbsorber absorber { get; private set; }

        public PlayerStateMachine stateMachine { get; private set; }
        public IdleState idleState;
        public MoveState MoveState;
        public JumpState JumpState;
        public FallState FallState;
        public DashState dashState;
        public AttackState attackState;
        public DeadState deadState;
        public HitState hitState;


        // 대쉬
        private float dashCooldownTimer;
        public bool CanDash => dashCooldownTimer <= 0f;

        public event Action<float> OnDashCooldownStarted;
        public event Action OnDashReady;

        public float facingDirection { get; private set; }

        private Warp currentWarp; // 워프 

        void Awake()
        {
            //facingDirection = 0;

            input = GetComponent<PlayerInput>();
            move = GetComponent<PlayerMove>();
            anim = GetComponent<PlayerAnimator>();
            health = GetComponent<PlayerHealth>();
            absorber = GetComponent<GoldAbsorber>();
            
            stateMachine = new PlayerStateMachine();

            idleState = new IdleState(this);
            MoveState = new MoveState(this);    
            JumpState = new JumpState(this);
            FallState = new FallState(this);
            dashState = new DashState(this);
            attackState = new AttackState(this);
            deadState = new DeadState(this);
            hitState = new HitState(this);

            health.OnDamaged += HandleDamaged;
            health.OnDead += HandleDead;
        }

        private void Start()
        {
            //anim.spum.OverrideControllerInit();
            stateMachine.ChangeState(idleState);
        }

        void Update()
        {
            if (dashCooldownTimer > 0f)
            {
                dashCooldownTimer -= Time.deltaTime;

                if(dashCooldownTimer <= 0)
                {
                    dashCooldownTimer = 0f;
                    OnDashReady?.Invoke();
                }
            }


            //if (stateMachine.CurrentState != dashState)
            //{
            //    HandleMove();
            //    HandleJump();
            //}
            HandleMove();
            HandleJump();            
            HandleInteract();
            HandleAbsorb();

            stateMachine.Update();
        }

        private void FixedUpdate()
        {
            HandleFacing();
            stateMachine.FixedUpdate();
        }

        private void HandleInteract()
        {
            //Debug.Log($"[HandleInteract] false");
            if (input.interactPressed && currentWarp != null)
            {
                //Debug.Log($"[HandleInteract] true");
                currentWarp.Activate();
            }
        }

        private void HandleFacing()
        {
            if (Mathf.Abs(input.moveX) > 0.01f)
            {
                float newDirection = Mathf.Sign(input.moveX);

                //Debug.Log($"[HandleFacing] moveX={input.moveX}, newDirection={newDirection}, facingDirection={facingDirection}, equal={newDirection == facingDirection}");

                if (newDirection != facingDirection)
                {
                    facingDirection = newDirection;
                    anim.SetFacing(facingDirection);
                    //Debug.Log($"[HandleFacing] SetFacing 호출됨! → {facingDirection}");
                }
            }
        }

        private void HandleMove()
        {
            float speedMultiplier = (stateMachine.CurrentState == attackState)
               ? move.attackMoveMultiplier
               : 1f;

            move.SetHorizontalVelocity(input.moveX * move.MoveSpeed * speedMultiplier);
        }


        private void HandleJump()
        {
            if (input.jumpPressed && move.isGrounded)
            {
                move.Jump();
                input.ConsumeJump();
            }
        }


        private void HandleDamaged()
        {
            if (health.isDead)
                return;

            stateMachine.ChangeState(hitState);
        }

        private void HandleDead()
        {
            stateMachine.ChangeState(deadState);
        }

        private void HandleAbsorb()
        {
            if(input.absorbPressed || input.absorbHeld)
                absorber.TryAbsorb();
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.OnDamaged -= HandleDamaged;
                health.OnDead -= HandleDead;
            }

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Warp>(out var warp))
            {
                currentWarp = warp;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Warp>(out var warp) && warp == currentWarp)
            {
                currentWarp = null;
            }
        }

        public void RequestDash()
        {
            if (!CanDash) 
                return;

            dashCooldownTimer = dashCooldown;
            OnDashCooldownStarted?.Invoke(dashCooldown);

            stateMachine.ChangeState(dashState);
        }


        public void ResetPlayer(Vector3 spawnPosition)
        {
            // 위치 초기화
            transform.position = spawnPosition;


            // 물리 초기화
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }


            // 입력 초기화
            input.ResetInput();


            // 상태 초기화
            stateMachine.ChangeState(idleState);


            // 체력 초기화
            health.ResetForRetry();


            // 대쉬 초기화
            dashCooldownTimer = 0f;
            OnDashReady?.Invoke();


            // 방향 초기화
            facingDirection = 1f;
            anim.SetFacing(facingDirection);


            // 애니메이션 초기화
            anim.ResetAnimation();


            // 워프 상태 제거
            currentWarp = null;


            // 공격 상태 초기화
            if (playerAttack != null)
            {
                playerAttack.ResetAttack();
            }
        }
    }

}