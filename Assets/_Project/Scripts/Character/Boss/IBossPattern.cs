using System.Threading;
using Cysharp.Threading.Tasks;
using GoldEater;
using UnityEngine;

public interface IBossPattern
{
    string PatternName { get; }
    float Weight { get; }
    bool CanExecute(BossContext ctx);
    UniTask Execute(BossContext ctx, CancellationToken token);
}

// 패턴들이 공통으로 참조할 컨텍스트 (필요한 참조만 모아둠)
public class BossContext
{
    public Transform Self;
    public Transform Player;
    public BossAnimator Animator;

    public BossAttackHitbox Hitbox;

    public bool IsPhase2;
}