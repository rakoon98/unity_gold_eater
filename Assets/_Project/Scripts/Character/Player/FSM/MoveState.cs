using UnityEngine;

namespace GoldEater
{
    public class MoveState : PlayerState
    {
        private float speed = 5f;

        public MoveState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            player.anim.PlayMove();
        }

        public override void FixedUpdate()
        {
            //player.rb.linearVelocity = new Vector2(player.input.moveX * speed, player.rb.linearVelocity.y);
            player.move.SetHorizontalVelocity(player.input.moveX * player.move.MoveSpeed);
        }

        public override void Update()
        {
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

            if (player.input.moveX == 0)
            {
                player.stateMachine.ChangeState(player.idleState);
            }


            if (player.input.attackPressed || player.input.attackHeld)
            {
                player.stateMachine.ChangeState(player.attackState);
            }
        }
    }

}