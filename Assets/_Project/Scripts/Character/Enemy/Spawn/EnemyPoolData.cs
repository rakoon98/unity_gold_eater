using UnityEngine;

namespace GoldEater
{
    [CreateAssetMenu(fileName = "EnemyPoolData", menuName = "Data/EnemyPoolData")]
    public class EnemyPoolData : ScriptableObject
    {
        [Tooltip("랜덤으로 뽑힐 잡몹 프리팹 목록")]
        public GameObject[] enemyPrefabs;

        /// <summary>
        /// 풀에서 랜덤 프리팹 하나를 반환합니다.
        /// </summary>
        public GameObject GetRandomEnemyPrefab()
        {
            if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            {
                Debug.LogWarning("[EnemyPoolData] enemyPrefabs가 비어있습니다.");
                return null;
            }

            int index = Random.Range(0, enemyPrefabs.Length);
            return enemyPrefabs[index];
        }
    }

}