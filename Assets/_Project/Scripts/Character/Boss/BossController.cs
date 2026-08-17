using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

namespace GoldEater
{
    public class BossController : MonoBehaviour
    {
        private Transform player;
        [SerializeField] private GameObject spiritPrefab;

        public BossAnimator Animator { get; private set; }
        public BossHealth Health { get; private set; }
        public BossStateMachine StateMachine { get; private set; }
        public MonoBehaviour Runner => this;

        public BossIdleState IdleState { get; private set; }
        public BossChoosePatternState ChoosePatternState { get; private set; }
        public BossDeathState DeathState { get; private set; }

        public DashAttackPattern dashAttackPattern;
        public AttackPattern attackPattern;
        public BossSummonPattern summonPattern;
        public BossSkillPattern skillPattern;

        private bool isPhase2;
        public BossAttackHitbox Hitbox { get; private set; }


        private void Awake()
        {
            dashAttackPattern = new DashAttackPattern();
            attackPattern = new AttackPattern();
            summonPattern = new BossSummonPattern(spiritPrefab);
            skillPattern = new BossSkillPattern();

            var patterns = new List<IBossPattern> { dashAttackPattern, attackPattern, summonPattern, skillPattern };

            IdleState = new BossIdleState(this);
            ChoosePatternState = new BossChoosePatternState(this, patterns);
            DeathState = new BossDeathState(this);

            Animator = GetComponentInChildren<BossAnimator>();
            Health = GetComponent<BossHealth>();
            StateMachine = new BossStateMachine();

            Hitbox = GetComponentInChildren<BossAttackHitbox>();

            Health.OnPhase2Entered += () => isPhase2 = true;
            Health.OnDeath += () => StateMachine.ChangeState(DeathState);
        }

        private void Start()
        {
            FindAndStart().Forget();

            UIManager.instance.bossHealthUI.Bind(Health);
        }

        private void Update()
        {
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            UpdateFacing();
        }

        private async UniTaskVoid FindAndStart()
        {
            // ★ 추가
            var token = this.GetCancellationTokenOnDestroy();

            try
            {
                // ★ 변경
                await FindPlayerAsync(token);

                // ★ 추가
                if (token.IsCancellationRequested)
                    return;

                StateMachine.ChangeState(IdleState);
            }
            // ★ 추가
            catch (OperationCanceledException)
            {
                // Boss가 Destroy되면서 정상적으로 취소됨
            }
        }

        private void UpdateFacing()
        {
            if (player == null) return;

            if (StateMachine.CurrentState == ChoosePatternState) return;

            float dir = player.position.x - transform.position.x;
            if (Mathf.Abs(dir) < 0.01f) return;
            Animator.SetFacing(dir); // 직접 transform 안 건드리고 BossAnimator에 위임
        }

        public BossContext BuildContext()
        {
            // 참조가 끊겼으면(파괴됐으면) 다시 찾기
            if (player == null)
            {
                var found = GameObject.FindGameObjectWithTag("Player");
                if (found != null) player = found.transform;
            }

            return new BossContext
            {
                Self = transform,
                Player = player,
                Animator = Animator,
                Hitbox = Hitbox,
                IsPhase2 = isPhase2
            };
        }

        private async UniTask FindPlayerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var found = GameObject.FindGameObjectWithTag("Player");

                if (found != null)
                {
                    player = found.transform;
                    return;
                }

                await UniTask.Delay(
                    TimeSpan.FromSeconds(0.2f),
                    cancellationToken: token);
            }
        }
    }
}