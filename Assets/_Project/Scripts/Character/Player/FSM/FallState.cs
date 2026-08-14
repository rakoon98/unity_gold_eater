using UnityEngine;

namespace GoldEater
{
    public class FallState : PlayerState
    {

        public FallState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            player.anim.PlayFall();
        }

        public override void FixedUpdate()
        {
            player.move.SetHorizontalVelocity(player.input.moveX * player.move.MoveSpeed);
            //player.rb.linearVelocity = new Vector2(player.input.moveX * player.move.JumpSpeed, player.rb.linearVelocity.y);
        }

        public override void Update()
        {
            if (player.input.dashPressed)
            {
                player.RequestDash();
                return;
            }

            if (player.input.attackPressed || player.input.attackHeld)
            {
                player.stateMachine.ChangeState(player.attackState);
            }

            if (player.move.isGrounded)
            {
                if (player.input.jumpPressed)
                {
                    player.stateMachine.ChangeState(player.JumpState);
                    return;
                }

                player.stateMachine.ChangeState(player.input.moveX != 0 ? player.MoveState : player.idleState);
            }

            if (player.input.jumpPressed && player.move.IsCanDoubleJump())
            {
                player.stateMachine.ChangeState(player.JumpState);
            }
        }
    }
}