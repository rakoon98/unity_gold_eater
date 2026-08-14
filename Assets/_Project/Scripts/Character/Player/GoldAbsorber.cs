using System.Collections.Generic;
using UnityEngine;

namespace GoldEater
{
    public class GoldAbsorber : MonoBehaviour
    {
        [System.Serializable]
        public struct FloatRange
        {
            public float min;
            public float max;

            public FloatRange(float min, float max)
            {
                this.min = min;
                this.max = max;
            }

            public float GetRandom() => Random.Range(min, max);
        }

        private GoldInventory inventory;
        private StatComponent playerStat;
        private int goldCostPerAbsorb = 1;

        private static readonly StatType[] candidates =
        {
            StatType.Attack, StatType.MoveSpeed, StatType.JumpSpeed,
            StatType.DashSpeed, StatType.AttackSpeed, StatType.CritChance, StatType.CritDamage
        };

        private static readonly Dictionary<StatType, FloatRange> increaseRanges = new()
        {
            { StatType.Attack,      new FloatRange(2.5f, 5.0f) },
            { StatType.MoveSpeed,   new FloatRange(0.2f, 0.4f) },
            { StatType.JumpSpeed,   new FloatRange(0.1f, 0.8f) },
            { StatType.DashSpeed,   new FloatRange(0.2f, 0.4f) },
            { StatType.AttackSpeed, new FloatRange(0.1f, 0.5f) },
            { StatType.CritChance,  new FloatRange(0.1f, 0.5f) },
            { StatType.CritDamage,  new FloatRange(0.1f, 0.2f) },
        };

        private void Awake()
        {
            if (inventory == null) inventory = GetComponent<GoldInventory>();
            if (playerStat == null) playerStat = GetComponent<StatComponent>();
        }

        public bool TryAbsorb()
        {
            if (!inventory.TryConsumeGold(goldCostPerAbsorb))
            {
                Debug.Log("°ñµå°¡ ºÎÁ·ÇÕ´Ï´Ù.");
                return false;
            }

            StatType picked = candidates[Random.Range(0, candidates.Length)];
            float value = GetIncreaseValue(picked);

            var modifier = new StatModifier(picked, ModifierType.Flat, value, this);
            playerStat.AddModifier(modifier);

            string result = $"{picked} +{value:F2} »ó½Â!";
            Debug.Log($"[Absorb] {result} (³²Àº °ñµå: {inventory.GoldCount})");

            NotificationManager.instance.ShowNotification($"<color=#FFD700>{result}</color>");
            return true;
        }

        private float GetIncreaseValue(StatType stat)
        {
            return increaseRanges.TryGetValue(stat, out FloatRange range) ? range.GetRandom() : 1f;
        }
    }
}