using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace GoldEater
{
    public class BossChoosePatternState : BossState
    {
        private List<IBossPattern> patterns;

        public BossChoosePatternState(BossController controller, List<IBossPattern> patterns) : base(controller)
        {
            this.patterns = patterns;
        }

        public override void Enter()
        {
            ChooseAndExecuteAsync().Forget(); // fire-and-forget, 예외는 Forget()이 로그로 잡아줌
        }

        private async UniTaskVoid ChooseAndExecuteAsync()
        {
            
            var token = controller.GetCancellationTokenOnDestroy();

            if (token.IsCancellationRequested)
                return;

            try 
            {
                var ctx = controller.BuildContext();

                var candidates = patterns.FindAll(p => p.CanExecute(ctx));

                if (candidates.Count == 0)
                {
                    if (!token.IsCancellationRequested)
                        controller.StateMachine.ChangeState(controller.IdleState);

                    return;
                }

                var chosen = ChooseByWeight(candidates);

                
                await chosen.Execute(ctx, token);

                if (token.IsCancellationRequested)
                    return;

                controller.StateMachine.ChangeState(controller.IdleState);
            }
           
            catch (OperationCanceledException)
            {
                // Boss Destroy로 인한 정상적인 취소
            }
        }

        private IBossPattern ChooseByWeight(List<IBossPattern> candidates)
        {
            float totalWeight = 0f;
            foreach (var p in candidates) totalWeight += p.Weight;

            float rand = UnityEngine.Random.Range(0f, totalWeight);
            float cumulative = 0f;

            foreach (var p in candidates)
            {
                cumulative += p.Weight;
                if (rand <= cumulative)
                    return p;
            }

            return candidates[candidates.Count - 1]; // 안전장치
        }

        public override void Update() { }
    }
}