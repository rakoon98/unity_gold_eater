using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GoldEater
{
    public class BossSummonPattern : IBossPattern
    {
        public string PatternName => "summon";

        public float Weight => 15f;
        
        private readonly GameObject spiritPrefab;

        public BossSummonPattern(GameObject spiritPrefab)
        {
            this.spiritPrefab = spiritPrefab;
        }

        public bool CanExecute(BossContext ctx) => true;

        public async UniTask Execute(BossContext ctx)
        {
            //ctx.Animator.Play(ClipName);
            ctx.Animator.PlaySummon();

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f));

            int count = ctx.IsPhase2 ? 2 : 1;
            for (int i = 0; i < count; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-2f, 2f), 0f, 0f);
                Object.Instantiate(spiritPrefab, ctx.Self.position + offset, Quaternion.identity);
            }

            await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f));
        }
    }
}