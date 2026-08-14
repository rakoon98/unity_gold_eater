using UnityEngine;

namespace GoldEater
{
    public enum StatType
    {
        Health,       // 하트 개수 (플레이어) / HP 수치 (몹)
        Attack,       // 몹: 플레이어에게 주는 하트 데미지량 (0.5, 1 등)
        AttackRadius,
        AttackSpeed,

        MoveSpeed,
        JumpSpeed,
        DashSpeed,

        CritChance,
        CritDamage
    }
}