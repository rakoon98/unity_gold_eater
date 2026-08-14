using UnityEngine;

namespace GoldEater
{
    public class HitState : PlayerState
    {
        private float hitDuration = 0.3f;
        private float hitTimer;

        public HitState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            player.anim.PlayHit();
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, 0f);
            hitTimer = 0f;
        }

        public override void Update()
        {
            hitTimer += Time.deltaTime;
            if (hitTimer >= hitDuration)
            {
                player.stateMachine.ChangeState(new IdleState(player));
            }
        }
    }
}