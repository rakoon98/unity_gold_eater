using UnityEngine;

namespace GoldEater
{
    public class EnemyWalkState : EnemyState
    {
        public EnemyWalkState(EnemyController controller) : base(controller) { }

        public override void Enter()
        {
            controller.Animator.PlayWalk();
        }

        public override void FixedUpdate()
        {
            if (!controller.Detector.hasTarget)
                return;

            controller.Move.MoveTo(controller.Detector.target.position);
        }

        public override void Update()
        {
            if (!controller.Detector.hasTarget)
            {
                controller.StateMachine.ChangeState(controller.IdleState);
                return;
            }

            if (controller.Detector.IsInAttackRange())
            {
                controller.StateMachine.ChangeState(controller.IdleState);
                return;
            }

        }
    }
}