using GoldEater;
using UnityEngine;

namespace GoldEater
{
    public enum ModifierType
    {
        Flat,        // 그냥 더하기 (+2)
        PercentAdd,  // 퍼센트 가산 (+10%)
        PercentMult  // 퍼센트 곱연산 (x1.1)
    }

    [System.Serializable]
    public class StatModifier
    {
        public StatType statType;
        public ModifierType modType;
        public float value;

        // 이 수정자가 어디서 왔는지 (나중에 제거할 때 이 값으로 찾음)
        // 예: 골드 아이템 오브젝트 자신, 또는 버프 스킬 오브젝트
        public object source;

        public StatModifier(StatType statType, ModifierType modType, float value, object source)
        {
            this.statType = statType;
            this.modType = modType;
            this.value = value;
            this.source = source;
        }
    }
}