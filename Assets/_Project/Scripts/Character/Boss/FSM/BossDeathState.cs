using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GoldEater
{
    public class BossDeathState : BossState
    {
        private const string ClipName = "death";

        public BossDeathState(BossController controller) : base(controller) { }

        public override void Enter()
        {
            controller.Animator.PlayDeath();
            DeathRoutineAsync().Forget();


        }

        private async UniTaskVoid DeathRoutineAsync()
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(1.2f)); // death Ŭ�� ���̿� �°� ����

            UIManager.instance.StageClear.OnBossCleared();

            // ���/���� ���, ������ Ŭ���� Ʈ���� ��
            Object.Destroy(controller.gameObject);
        }
    }
}