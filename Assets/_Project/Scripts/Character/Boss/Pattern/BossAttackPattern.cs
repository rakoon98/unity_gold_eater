using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace GoldEater
{
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

        public async UniTask Execute(BossContext ctx, CancellationToken token)
        {
            if (ctx.Player == null || ctx.Self == null) return;

            // ===================================================
            // 1. [패턴 시작 시점] 공격할 방향 완전히 고정
            // ===================================================
            float initialPlayerX = ctx.Player.position.x;
            float bossX = ctx.Self.position.x;

            float fixedXDir = Mathf.Sign(initialPlayerX - bossX);
            if (fixedXDir == 0) fixedXDir = 1f;

            // 시작할 때 정한 방향으로 시선 고정
            if (ctx.Animator != null)
            {
                ctx.Animator.SetFacing(fixedXDir);
            }

            // ===================================================
            // 2. 플레이어 위치 변동 무시하고 고정된 방향으로만 공격
            // ===================================================
            await BossAttackRoutine.PlayComboAttack(ctx);
        }
    }
}