using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GoldEater
{
    public class DashAttackPattern : IBossPattern
    {
        public string PatternName => "DashAttack";

        public float Weight => 40f;
        private const float DashSpeed = 12f;
        private const float StopDistance = 1.2f; // 이 거리 이내로 붙으면 돌진 멈추고 공격
       

        public bool CanExecute(BossContext ctx)
        {
            if (ctx.Player == null) return false;
            return true; // 항상 실행 가능 (거리 무관하게 쫓아가는 패턴이니까)
        }

        public async UniTask Execute(BossContext ctx)
        {
            Rigidbody2D rb = ctx.Self.GetComponent<Rigidbody2D>();

            // 1. 돌진: 플레이어에게 빠르게 접근
            float timeout = 2f; // 무한 추적 방지 안전장치
            float timer = 0f;

            while (Vector2.Distance(ctx.Self.position, ctx.Player.position) > StopDistance && timer < timeout)
            {
                //Vector2 dir = ((Vector2)ctx.Player.position - (Vector2)ctx.Self.position).normalized;
                float xDir = Mathf.Sign(ctx.Player.position.x - ctx.Self.position.x);
                Vector2 dir = new Vector2(xDir, 0f);
                rb.MovePosition(rb.position + dir * DashSpeed * Time.fixedDeltaTime);

                timer += Time.deltaTime;
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            }

            // 2. 도착 즉시 정지 + 공격
            rb.linearVelocity = Vector2.zero;
            await BossAttackRoutine.PlayComboAttack(ctx);
        }
    }
}