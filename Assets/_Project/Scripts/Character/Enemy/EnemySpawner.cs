using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public void SpawnEnemy(GameObject prefab, Vector3 pos)
    {
        // 지금: 그냥 Instantiate
        Instantiate(prefab, pos, Quaternion.identity);

        // 나중에 풀링 도입 시: 이 한 줄만 ObjectPool.Get()으로 교체
    }
}
