using UnityEngine;
using System;

namespace GoldEater
{
    public class GoldInventory : MonoBehaviour
    {
        public int GoldCount { get; private set; }

        public event Action<int> OnGoldChanged; // HUD Ç¥½Ã¿ë

        public void AddGold(int amount)
        {
            GoldCount += amount;
            OnGoldChanged?.Invoke(GoldCount);
        }

        public bool TryConsumeGold(int amount)
        {
            if (GoldCount < amount) return false;
            GoldCount -= amount;
            OnGoldChanged?.Invoke(GoldCount);

            
            return true;
        }
    }
}