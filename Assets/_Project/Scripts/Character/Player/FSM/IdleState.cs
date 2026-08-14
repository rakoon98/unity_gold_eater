using UnityEngine;

namespace GoldEater
{
    public class IdleState : PlayerState
    {
        public IdleState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            player.anim.PlayIdle();
            player.rb.linearVelocity = new Vector2(0, player.rb.linearVelocity.y);
        }

        public override void Update()
        {
          

            if (player.input.moveX != 0)
            {
                player.stateMachine.ChangeState(player.MoveState);
                return;
            }

            if (player.input.jumpPressed)
            {
                player.stateMachine.ChangeState(player.JumpState);
                return;
            }

            if (player.input.dashPressed)
            {
                player.RequestDash();
                return;
            }

            if (player.input.attackPressed || player.input.attackHeld)
            {
                player.stateMachine.ChangeState(player.attackState);
            }
        }
    }
}