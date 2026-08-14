using UnityEngine;

namespace GoldEater
{
    public class EnemyHurtState : EnemyState
    {
        private float timer;

        public EnemyHurtState(EnemyController controller) : base(controller) { }

        public override void Enter()
        {
            timer = 0f;

            controller.Move.StopMove();
            controller.Animator.PlayHurt();
            controller.Animator.PlayHitFlash();
        }

        public override void Update()
        {
            timer += Time.deltaTime;

            if (controller.Health.isDead)
            {
                controller.StateMachine.ChangeState(controller.DeathState);
                return;
            }

            if (timer >= 0.3f)
            {
                controller.StateMachine.ChangeState(controller.IdleState);
            }
        }
    }
}