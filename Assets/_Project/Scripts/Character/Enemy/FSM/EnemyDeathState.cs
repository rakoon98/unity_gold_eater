using UnityEngine;

namespace GoldEater
{
    public class EnemyDeathState : EnemyState
    {
        public EnemyDeathState(EnemyController controller) : base(controller) { }

        public override void Enter()
        {
            controller.Move.StopAll();
            controller.Animator.PlayDeath();

            //controller.StartCoroutine(controller.Health.DestroyRoutine());
            Object.Destroy(controller.gameObject, 1f);


        }

    }
}

