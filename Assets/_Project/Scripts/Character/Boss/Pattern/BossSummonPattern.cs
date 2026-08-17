// BossSummonPattern.cs
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GoldEater
{
    public class BossSummonPattern : IBossPattern
    {
        public string PatternName => "summon";
        public float Weight => 15f;

        private readonly GameObject spiritPrefab;
        private const float Y_OFFSET = 1.3f;   // 기존 2.5f -> 1.3f로 낮춤 (머리 직상단 피함)
        private const float SPACING = 1.8f;    // 기존 1.2f -> 1.8f로 넓힘 (너무 나란히 붙지 않게)

        public BossSummonPattern(GameObject spiritPrefab)
        {
            this.spiritPrefab = spiritPrefab;
        }

        public bool CanExecute(BossContext ctx) => true;

        public async UniTask Execute(BossContext ctx, CancellationToken token)
        {
            if (token.IsCancellationRequested || ctx.Self == null || ctx.Animator == null)
                return;

            ctx.Animator.PlaySummon();

            await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: token);

            if (token.IsCancellationRequested || ctx.Self == null)
                return;

            int count = ctx.IsPhase2 ? 4 : 2;

            for (int i = 0; i < count; i++)
            {
                if (token.IsCancellationRequested || ctx.Self == null)
                    return;

                // 좌우 대칭 오프셋 연산
                float xOffset = (i - (count - 1) / 2.0f) * SPACING;
                Vector3 spawnPos = ctx.Self.position + new Vector3(xOffset, Y_OFFSET, 0f);

                UnityEngine.Object.Instantiate(spiritPrefab, spawnPos, Quaternion.identity);

                await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: token);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
        }
    }
}