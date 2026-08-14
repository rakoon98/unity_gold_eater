using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace GoldEater
{
    public class JumpState : PlayerState
    {
        public JumpState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            player.anim.PlayJump(); // Jump 전용 모션 없음, Move로 대체
            //player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, jumpForce);
            player.move.Jump();
            player.move.OnJumpCountIncrease();
            player.input.ConsumeJump(); // 버퍼 소비 필수
        }

        public override void FixedUpdate()
        {
            //player.rb.linearVelocity = new Vector2(player.input.moveX * player.move.MoveSpeed, player.rb.linearVelocity.y);
            player.move.SetHorizontalVelocity(player.input.moveX * player.move.MoveSpeed);
        }

        public override void Update()
        {
            if (player.input.dashPressed)
            {
                player.RequestDash();
                return;
            }

            if (player.input.jumpPressed && player.move.IsCanDoubleJump())
            {
                player.stateMachine.ChangeState(player.JumpState);
                return;
            }

            else if (player.input.attackPressed || player.input.attackHeld)
            {
                player.stateMachine.ChangeState(player.attackState);
            }

            else if (player.rb.linearVelocity.y < 0)
            {
                player.stateMachine.ChangeState(player.FallState);
            }           
        }
    }
}