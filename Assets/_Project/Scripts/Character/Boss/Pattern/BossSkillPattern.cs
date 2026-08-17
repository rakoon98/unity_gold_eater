using System.Threading;
using Cysharp.Threading.Tasks;

namespace GoldEater
{
    public class BossSkillPattern : IBossPattern
    {
        public string PatternName => "skill1";

        public float Weight => 15f;
        public bool CanExecute(BossContext ctx) => true;


        public async UniTask Execute(BossContext ctx, CancellationToken token)
        {
            ctx.Animator.PlaySkill1();

            await UniTask.Delay(System.TimeSpan.FromSeconds(1.0f));
        }
    }
}