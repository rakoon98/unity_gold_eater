using Cysharp.Threading.Tasks;

namespace GoldEater
{
    public static class BossAttackRoutine
    {
        public static async UniTask PlayComboAttack(BossContext ctx)
        {
            ctx.Animator.PlayAttack();

            // 1Å¸
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.17f));
            ctx.Hitbox.Activate(0);
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.13f));
            ctx.Hitbox.Deactivate(0);

            // 2Å¸
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.20f));
            ctx.Hitbox.Activate(1);
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.13f));
            ctx.Hitbox.Deactivate(1);

            // ÈÄµô
            await UniTask.Delay(System.TimeSpan.FromSeconds(0.54f));
        }
    }
}