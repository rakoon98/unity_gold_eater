using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace GoldEater
{
    public class DeadState : PlayerState
    {

        private const float DeadAnimationTime = 1.1f;

        public DeadState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            player.anim.PlayDead();
            player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, 0f);

            GameOver().Forget();
        }

        async UniTaskVoid GameOver()
        {
            await UniTask.WaitForSeconds(DeadAnimationTime);
            UIManager.instance.GameOver.Open();
        }
    }
}