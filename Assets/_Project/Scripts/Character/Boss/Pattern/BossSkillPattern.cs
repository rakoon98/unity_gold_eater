using Cysharp.Threading.Tasks;

namespace GoldEater
{
    public class BossSkillPattern : IBossPattern
    {
        public string PatternName => "skill1";

        public float Weight => 15f;
        public bool CanExecute(BossContext ctx) => true;

        public async UniTask Execute(BossContext ctx)
        {
            // 예고(윈드업) 구간 = 패링 가능 창
            //ctx.Animator.Play(ClipName);
            ctx.Animator.PlaySkill1();

            await UniTask.Delay(System.TimeSpan.FromSeconds(1.0f));
        }
    }
}