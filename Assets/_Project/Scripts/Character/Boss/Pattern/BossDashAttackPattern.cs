using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace GoldEater
{
    public class DashAttackPattern : IBossPattern
    {
        public string PatternName => "DashAttack";
        public float Weight => 40f;

        private const float DashSpeed = 12f;
        private const float StopDistance = 1.2f;

        public bool CanExecute(BossContext ctx)
        {
            return ctx.Player != null;
        }

        public async UniTask Execute(BossContext ctx, CancellationToken token)
        {
            if (ctx.Player == null || ctx.Self == null) return;

            Rigidbody2D rb = ctx.Self.GetComponent<Rigidbody2D>();

            // ===================================================
            // 1. [패턴 시작 시점] 방향과 목표 위치를 '완전히 고정'
            // ===================================================
            float initialPlayerX = ctx.Player.position.x;
            float bossX = ctx.Self.position.x;

            // 시작할 때 바라보는 방향 고정 (-1 또는 1)
            float fixedXDir = Mathf.Sign(initialPlayerX - bossX);
            if (fixedXDir == 0) fixedXDir = 1f; // 예외 처리

            // 시작할 때 정한 방향으로 보스 시선 고정
            if (ctx.Animator != null)
            {
                ctx.Animator.SetFacing(fixedXDir);
            }

            // 돌진 목표 지점 계산 (처음 플레이어 위치 기준)
            Vector2 fixedDirectionVector = new Vector2(fixedXDir, 0f);
            float targetX = initialPlayerX - (fixedXDir * StopDistance);

            // ===================================================
            // 2. 고정된 목표지점까지 돌진 (플레이어 이동 무시)
            // ===================================================
            float timeout = 2f;
            float timer = 0f;

            while (timer < timeout)
            {
                if (token.IsCancellationRequested) return;

                float currentX = ctx.Self.position.x;

                // 처음에 정해둔 targetX에 도달하면 돌진 종료
                if ((fixedXDir > 0 && currentX >= targetX) || (fixedXDir < 0 && currentX <= targetX))
                {
                    break;
                }

                rb.MovePosition(rb.position + fixedDirectionVector * DashSpeed * Time.fixedDeltaTime);

                timer += Time.fixedDeltaTime;
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: token);
            }

            // ===================================================
            // 3. 도착 후 '처음 고정한 방향' 그대로 휘두르기
            // ===================================================
            rb.linearVelocity = Vector2.zero;

            // 공격 직전에 플레이어가 어디 있든 무시하고 처음에 정한 방향으로 다시 강제 고정
            if (ctx.Animator != null)
            {
                ctx.Animator.SetFacing(fixedXDir);
            }

            await BossAttackRoutine.PlayComboAttack(ctx);
        }
    }
}