using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GoldEater
{
    // 보스
    public class AttackPattern : IBossPattern
    {

        public string PatternName => "Attack";
        public float Weight => 30f;

        public bool CanExecute(BossContext ctx)
        {
            if (ctx.Player == null) return false;
            float dist = Vector2.Distance(ctx.Self.position, ctx.Player.position);
            return dist < 6f;
        }

        //public async UniTask Execute(BossContext ctx)
        //{
        //    ctx.Animator.PlayAttack(); // Animator.Play("attack") 대신
        //    await UniTask.Delay(System.TimeSpan.FromSeconds(0.9f));
        //}

        public async UniTask Execute(BossContext ctx)
        {
            await BossAttackRoutine.PlayComboAttack(ctx);
        }
    }
}