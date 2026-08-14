using UnityEngine;

namespace GoldEater
{
    public class DashState : PlayerState
    {
        private float dashTimer;
        private float dashDirection;
        private TrailRenderer trail;

        public DashState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            player.anim.PlayDash();
            dashTimer = 0f;
            dashDirection = player.facingDirection;

            trail = player.GetComponent<TrailRenderer>();
            if (trail != null) trail.emitting = true;

            player.move.SetInvincible(true);
        }

        public override void FixedUpdate()
        {
            //player.rb.linearVelocity = new Vector2(dashDirection * player.move.DashSpeed, 0);
            player.move.SetDashVelocity(dashDirection * player.move.DashSpeed);
        }

        public override void Update()
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= player.move.DashDuration)
            {
                player.stateMachine.ChangeState(
                    player.input.moveX != 0 ? player.MoveState : player.idleState);
            }
        }

        public override void Exit()
        {
            if (trail != null) trail.emitting = false;
            player.move.SetInvincible(false);
        }
    }
}