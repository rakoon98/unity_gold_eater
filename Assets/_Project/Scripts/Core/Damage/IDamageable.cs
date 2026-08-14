using UnityEngine;

namespace GoldEater
{
    public interface IDamageable
    {
        void TakeDamage(float amount);
        bool isDead { get; }
    }
}