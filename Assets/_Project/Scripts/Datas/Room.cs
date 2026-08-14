namespace GoldEater
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Room : MonoBehaviour
    {

        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Collider2D cameraBounds;
        public int roomIndex;

        public Transform SpawnPoint => spawnPoint;
        public Collider2D CameraBounds => cameraBounds;


        [Header("몹 스폰 설정")]
        [Tooltip("이 방에서 몹이 소환될 기준 위치들")]
        [SerializeField] private Transform[] enemySpawnPoints;

        [Tooltip("스폰 포인트 기준 랜덤 오프셋 반경")]
        [SerializeField] private float randomOffsetRadius = 0.5f;

        [Tooltip("공용 몹 풀 (모든 Room이 같은 에셋 참조해도 되고, 방마다 다르게 넣어도 됨)")]
        [SerializeField] private EnemyPoolData enemyPool;

        [Tooltip("스폰 포인트당 몇 마리씩 소환할지")]
        [SerializeField] private int enemiesPerPoint = 1;

        private bool hasSpawned = false;
        private readonly List<GameObject> spawnedEnemies = new List<GameObject>();

        /// <summary>이 방의 몹을 전부 처치했을 때 발생 (문 열기, 보상 등에 연결)</summary>
        public System.Action OnRoomCleared;


        /// <summary>
        /// StageManager가 이 방을 활성화(SetActive(true))한 직후 호출해주세요.
        /// 이미 스폰한 방(재입장)은 다시 스폰하지 않습니다.
        /// </summary>
        public void SpawnEnemies()
        {
            if (hasSpawned) return;

            if (enemyPool == null || enemySpawnPoints == null || enemySpawnPoints.Length == 0)
            {
                // 스폰 포인트나 풀이 없는 방(보스룸, 세이프존 등)은 조용히 스킵
                return;
            }

            hasSpawned = true;

            //foreach (Transform point in enemySpawnPoints)
            //{
            //    int count = Random.Range(1, 5);
            //    for (int i = 0; i < enemiesPerPoint; i++)
            //    {
            //        for (int j = 0; j < count; j++) 
            //            SpawnOne(point);
            //    }
            //}
            foreach (Transform point in enemySpawnPoints)
            {
                SpawnOne(point);
            }
        }

        private void SpawnOne(Transform point)
        {
            GameObject prefab = enemyPool.GetRandomEnemyPrefab();
            if (prefab == null) return;

            Vector2 offset = Random.insideUnitCircle * randomOffsetRadius;
            Vector3 spawnPos = point.position + new Vector3(offset.x, offset.y, 0f);

            GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity, transform);
            spawnedEnemies.Add(enemy);

            EnemyHealth health = enemy.GetComponent<EnemyHealth>();
            if (health != null)
            {
                // 클로저로 어떤 몹이 죽었는지 넘김
                health.OnDead += () => HandleEnemyDeath(enemy);
            }
            else Debug.LogWarning($"[Room] {enemy.name}에 EnemyHealth가 없어 방 클리어 감지가 안 됩니다.");
        }

        private void HandleEnemyDeath(GameObject enemy)
        {
            spawnedEnemies.Remove(enemy);

            if (spawnedEnemies.Count == 0 && hasSpawned)            
                OnRoomCleared?.Invoke();            
        }

        private void OnDrawGizmosSelected()
        {
            if (enemySpawnPoints == null) return;

            Gizmos.color = Color.red;
            foreach (Transform point in enemySpawnPoints)
            {
                if (point == null) continue;
                Gizmos.DrawWireSphere(point.position, randomOffsetRadius);
            }
        }
    }
}