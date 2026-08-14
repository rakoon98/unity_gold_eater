using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GoldEater
{
    public class BossIdleState : BossState
    {
        private const float IdleDuration = 3f;

        public BossIdleState(BossController controller) : base(controller) { }

        public override void Enter()
        {
            controller.Animator.PlayIdle();
            WaitThenChooseAsync().Forget();
        }

        private async UniTaskVoid WaitThenChooseAsync()
        {
            var token = controller.GetCancellationTokenOnDestroy();

            await UniTask.Delay(
                System.TimeSpan.FromSeconds(IdleDuration),
                cancellationToken: token);

            if (token.IsCancellationRequested)
                return;

            controller.StateMachine.ChangeState(controller.ChoosePatternState);
        }

        public override void Update() { }
    }
}