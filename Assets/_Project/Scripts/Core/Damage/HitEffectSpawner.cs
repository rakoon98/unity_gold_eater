using UnityEngine;

namespace GoldEater
{
    public static class HitEffectSpawner
    {
        public static void Spawn(GameObject effectPrefab, Vector3 position, float lifeTime = 1f)
        {
            if (effectPrefab == null) return;

            GameObject fx = Object.Instantiate(effectPrefab, position, Quaternion.identity);
            Object.Destroy(fx, lifeTime);
        }
    }
}