using GoldEater;
using UnityEngine;

namespace GoldEater
{
    public class EnemyIdleState : EnemyState
    {
        public EnemyIdleState(EnemyController controller) : base(controller) { }

        public override void Enter()
        {
            controller.Animator.PlayIdle();
            controller.Move.StopMove();
        }

        public override void Update()
        {
            if (!controller.Detector.hasTarget)
                return;

            if (controller.Detector.IsInAttackRange())
            {
                if (controller.attack.canAttack)
                {
                    controller.StateMachine.ChangeState(controller.AttackState);
                }

                return;
            }

            controller.StateMachine.ChangeState(controller.WalkState);
        }
    }
}