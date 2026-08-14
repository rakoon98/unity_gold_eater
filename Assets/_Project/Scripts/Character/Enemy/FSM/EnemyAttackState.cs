using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace GoldEater
{
    public class EnemyAttackState : EnemyState
    {
        private const int animationSample = 12;
        private const int attackHitFrame = 10;
        private const int attackEndFrame = 18;
        private const float attackHitTime = attackHitFrame / (float)animationSample;
        private const float attackEndTime = attackEndFrame / (float)animationSample;

        private CancellationTokenSource attackCts;

        public EnemyAttackState(EnemyController controller) : base(controller) { }

        public override void Enter()
        {
            controller.Move.StopMove();
            controller.Animator.PlayAttack();

            CancelAttackTiming();
            attackCts = new CancellationTokenSource();

            // destroyCancellationToken과 현재 State의 Token을 결합하여 안전하게 관리
            var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(
                attackCts.Token,
                controller.destroyCancellationToken
            ).Token;

            AttackAsync(linkedToken).Forget();
        }


        private async UniTaskVoid AttackAsync(CancellationToken token)
        {
            //await UniTask.WaitForSeconds(attackHitTime, cancellationToken: controller.destroyCancellationToken);
            //if (controller.Health.IsDead)
            //    return;

            //Attack.Attack();
            //controller.SetAttackCooldown();

            //await UniTask.WaitForSeconds(attackEndTime - attackHitTime, cancellationToken: controller.destroyCancellationToken);

            //if (controller.Health.IsDead)
            //    return;

            //controller.StateMachine.ChangeState(controller.IdleState);

            // 1. 공격 히트 시점까지 대기 (취소 예외 발생 시 throw 방지)
            bool isCanceled = await UniTask.WaitForSeconds(attackHitTime, cancellationToken: attackCts.Token).SuppressCancellationThrow();
            if (isCanceled || controller.Health.isDead) return;

            // 2. 공격 실행 (controller를 거쳐서 컴포넌트에 접근)
            controller.attack.Attack();
            controller.attack.ApplyCooldown();

            // 3. 공격 후딜레이 대기
            isCanceled = await UniTask.WaitForSeconds(attackEndTime - attackHitTime, cancellationToken: attackCts.Token).SuppressCancellationThrow();
            if (isCanceled || controller.Health.isDead) return;

            // 4. 대기 완료 후 Idle 상태로 전환
            controller.StateMachine.ChangeState(controller.IdleState);
        }


        public override void Exit()
        {
            CancelAttackTiming();
        }

        private void CancelAttackTiming()
        {
            if (attackCts == null) return;

            attackCts.Cancel();
            attackCts.Dispose();
            attackCts = null;
        }
    }
}