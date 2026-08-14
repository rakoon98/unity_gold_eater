using UnityEngine;

namespace GoldEater
{
    public class SpiritAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void SetFacing(float direction)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (direction > 0 ? 1 : -1);
            transform.localScale = scale;
        }

        public void PlayIdle() => animator.Play("idle");
        public void PlaySummon() => animator.Play("summon");
        public void PlayDeath() => animator.Play("death");
    }
}