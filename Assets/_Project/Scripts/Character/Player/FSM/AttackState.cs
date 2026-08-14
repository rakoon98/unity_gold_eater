using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace GoldEater
{
    public class AttackState : PlayerState
    {

        [SerializeField] private const float BaseTuningMultiplier = 1.3f;

        // 기준(배율 1.0) 타이밍값 — 애니메이션 클립 실제 프레임 기준으로 잡아둔 값
        private const float BaseAttackDuration = 1.2f;
        private const float BaseFirstHitStart = 0.4f;
        private const float BaseFirstHitEnd = 0.3f;
        private const float BaseGapBetweenHits = 0.1f;
        private const float BaseSecondHitEnd = 0.4f;

        private float attackTimer;
        private bool continueAttack;
        private CancellationTokenSource hitCts;
        private float attackSpeed;


        public AttackState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            attackTimer = 0f;
            continueAttack = false;
            //checkedContinueAttack = false;

            attackSpeed = Mathf.Max(player.playerAttack.attackSpeed, 0.01f);

            if (player.move.isGrounded)            
                player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, 0f);

            player.anim.SetSpeed(attackSpeed);
            player.anim.PlayAttack();
            StartHitTiming();
        }

        public override void Update()
        {
            attackTimer += Time.deltaTime;

            float scaledDuration = BaseAttackDuration / attackSpeed;
            if (attackTimer >= scaledDuration)
                player.stateMachine.ChangeState(player.idleState);
            //if (attackTimer >= BaseAttackDuration)
            //    player.stateMachine.ChangeState(player.idleState);            
        }


        private void StartHitTiming()
        {
            CancelHitTiming();
            hitCts = new CancellationTokenSource();
            HitTimingAsync(hitCts.Token).Forget();
        }

        private void CancelHitTiming()
        {
            if (hitCts == null) return;
            hitCts.Cancel();
            hitCts.Dispose();
            hitCts = null; // 핵심: 정리 후 반드시 null 처리
        }

        private async UniTaskVoid HitTimingAsync(CancellationToken token)
        {
            // 모든 타이밍을 attackSpeed로 나눠서 스케일 (배율 클수록 시간 짧아짐 = 빨라짐)
            float t1 = BaseFirstHitStart / attackSpeed;
            float t2 = BaseFirstHitEnd / attackSpeed;
            float t3 = BaseGapBetweenHits / attackSpeed;
            float t4 = BaseSecondHitEnd / attackSpeed;

            // ---------- 1타 ----------
            await UniTask.WaitForSeconds(t1, cancellationToken: token);
            player.playerAttack.Activate(0);
            await UniTask.WaitForSeconds(t2, cancellationToken: token);
            player.playerAttack.Deactive(0);

            if (player.input.attackPressed || player.input.attackHeld)
            {
                continueAttack = true;
                player.input.ConsumeAttackPressed();
            }
            else
            {
                player.anim.PlayIdle();
                player.stateMachine.ChangeState(player.idleState);
                return;
            }

            // ---------- 2타 ----------
            await UniTask.WaitForSeconds(t3, cancellationToken: token);
            player.playerAttack.Activate(1);
            await UniTask.WaitForSeconds(t4, cancellationToken: token);
            player.playerAttack.Deactive(1);


            //// ---------- 1타 ----------

            //await UniTask.WaitForSeconds(0.4f, cancellationToken: token);
            //player.playerAttack.Activate(0);

            //await UniTask.WaitForSeconds(0.3f, cancellationToken: token);
            //player.playerAttack.Deactive(0);

            //if (player.input.attackPressed || player.input.attackHeld)
            //{
            //    continueAttack = true;
            //    player.input.ConsumeAttackPressed();
            //}
            //else
            //{
            //    player.anim.PlayIdle();
            //    player.stateMachine.ChangeState(player.idleState);
            //    return;
            //}

            //// ---------- 2타 ----------

            //await UniTask.WaitForSeconds(0.1f, cancellationToken: token);
            //player.playerAttack.Activate(1);

            //await UniTask.WaitForSeconds(0.4f, cancellationToken: token);
            //player.playerAttack.Deactive(1);
        }

        public override void Exit()
        {
            CancelHitTiming();
            player.playerAttack.Deactive(0);
            player.playerAttack.Deactive(1);
            player.anim.SetSpeed(1f); // 반드시 원복 (안 하면 Idle/Move도 계속 빨라진 채로 재생됨)
        }
    }
}