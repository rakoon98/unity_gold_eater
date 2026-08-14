using UnityEngine;
using UnityEngine.Rendering;

namespace GoldEater
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyStateMachine StateMachine { get; private set; }

        public EnemyIdleState IdleState { get; private set; }
        public EnemyWalkState WalkState { get; private set; }
        public EnemyAttackState AttackState { get; private set; }
        public EnemyHurtState HurtState { get; private set; }
        public EnemyDeathState DeathState { get; private set; }

        public EnemyMove Move { get; private set; }
        public EnemyAnimator Animator { get; private set; }
        public EnemyHealth Health { get; private set; }
        public EnemyDetector Detector { get; private set; }
        public EnemyAttack attack { get; private set; }

        private float facingDirection;

        private void Awake()
        {
            Move = GetComponent<EnemyMove>();
            Animator = GetComponent<EnemyAnimator>();
            Health = GetComponent<EnemyHealth>();
            Detector = GetComponent<EnemyDetector>();
            attack = GetComponentInChildren<EnemyAttack>();

            StateMachine = new EnemyStateMachine();

            IdleState = new EnemyIdleState(this);
            WalkState = new EnemyWalkState(this);
            AttackState = new EnemyAttackState(this);
            HurtState = new EnemyHurtState(this);
            DeathState = new EnemyDeathState(this);

            Health.OnDamaged += HandleDamaged;
            Health.OnDead += HandleDead;
        }

        private void Start()
        {
            StateMachine.ChangeState(IdleState);
        }

        private void Update()
        {
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            HandleFacing();
            StateMachine.FixedUpdate();
        }

        private void HandleDamaged()
        {
            if (Health.isDead)
                return;

            StateMachine.ChangeState(HurtState);
        }

        private void HandleDead()
        {
            StateMachine.ChangeState(DeathState);
        }

        private void HandleFacing()
        {
            float dirX = Move.FacingDirection; // moveDirection.x

            if (Mathf.Abs(dirX) > 0.01f)
            {
                float newDirection = Mathf.Sign(dirX);
                if (newDirection != facingDirection)
                {
                    facingDirection = newDirection;
                    Animator.SetFacing(facingDirection);
                }
            }
        }

        private void OnDestroy()
        {
            if (Health != null)
            {
                Health.OnDamaged -= HandleDamaged;
                Health.OnDead -= HandleDead;
            }
        }
    }
}