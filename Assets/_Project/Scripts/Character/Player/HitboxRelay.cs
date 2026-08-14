using UnityEngine;

namespace GoldEater
{
    public class HitboxRelay : MonoBehaviour
    {
        [SerializeField] private PlayerAttack playerAttack;

        private void OnTriggerEnter2D(Collider2D other)
        {
            playerAttack.HandleHit(other);
        }
    }
}