using UnityEngine;

namespace GoldEater
{
    [System.Serializable]
    public class StatEntry
    {
        public StatType statType;
        public float baseValue;
    }

    [CreateAssetMenu(menuName = "Stats/CharacterStatData", fileName = "New CharacterStatData")]
    public class CharacterStatSO : ScriptableObject
    {
        [Tooltip("이 캐릭터가 가진 스탯 목록 (체력, 공격력 등)")]
        public StatEntry[] stats;

        [ContextMenu("Fill All Stat Types")]
        private void FillAllStatTypes()
        {
            var allTypes = (StatType[])System.Enum.GetValues(typeof(StatType));
            var newStats = new StatEntry[allTypes.Length];

            for (int i = 0; i < allTypes.Length; i++)
            {
                float existingValue = 0f;

                if (stats != null) // ← 이 체크 추가
                {
                    foreach (var entry in stats)
                    {
                        if (entry.statType == allTypes[i])
                        {
                            existingValue = entry.baseValue;
                            break;
                        }
                    }
                }

                newStats[i] = new StatEntry
                {
                    statType = allTypes[i],
                    baseValue = existingValue
                };
            }

            stats = newStats;
        }
    }
}