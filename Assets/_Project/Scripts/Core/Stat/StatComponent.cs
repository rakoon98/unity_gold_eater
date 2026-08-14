using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GoldEater
{
    public class StatComponent : MonoBehaviour
    {
        [Tooltip("이 캐릭터의 기본 스탯 세트 (에디터에서 드래그로 할당)")]
        [SerializeField] private CharacterStatSO characterStat;

        private Dictionary<StatType, float> baseValues = new();
        private Dictionary<StatType, List<StatModifier>> modifiers = new();

        // HUD 등에서 구독: (어떤 스탯이, 어떤 값으로 바뀌었는지)
        public event Action<StatType, float> OnStatChanged;

        private void Awake()
        {
            foreach (var so in characterStat.stats)
            {
                baseValues[so.statType] = so.baseValue;
                modifiers[so.statType] = new List<StatModifier>();
            }
        }

        public float GetStat(StatType type)
        {
            if (!baseValues.ContainsKey(type))
            {
                Debug.LogWarning($"{gameObject.name}에 {type} 스탯이 정의되어 있지 않습니다.");
                return 0f;
            }

            float baseVal = baseValues[type];
            float flatSum = 0f;
            float percentSum = 0f;

            foreach (var mod in modifiers[type])
            {
                switch (mod.modType)
                {
                    case ModifierType.Flat:
                        flatSum += mod.value;
                        break;
                    case ModifierType.PercentAdd:
                        percentSum += mod.value;
                        break;
                }
            }

            // 전부 덧셈 기반: base + flat + (base * percent합)
            return baseVal + flatSum + (baseVal * percentSum);
        }

        public void AddModifier(StatModifier mod)
        {
            if (!modifiers.ContainsKey(mod.statType))
                modifiers[mod.statType] = new List<StatModifier>();

            modifiers[mod.statType].Add(mod);
            OnStatChanged?.Invoke(mod.statType, GetStat(mod.statType));
        }

        public void RemoveModifiersFromSource(object source)
        {
            foreach (var type in modifiers.Keys.ToList())
            {
                int removed = modifiers[type].RemoveAll(m => m.source == source);
                if (removed > 0)
                    OnStatChanged?.Invoke(type, GetStat(type));
            }
        }

        public void ResetModifiers()
        {
            foreach (var list in modifiers.Values)
            {
                list.Clear();
            }

            // HUD가 이벤트 기반이라면 같이 알려준다.
            foreach (var type in baseValues.Keys)
            {
                OnStatChanged?.Invoke(type, GetStat(type));
            }
        }
    }
}