using UnityEngine;

namespace GoldEater
{
    public class BossAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private HitFlashEffect hitFlash;

        private void Awake()
        {
            hitFlash = new HitFlashEffect(transform, Color.red);
        }

        public void SetFacing(float direction)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (direction > 0 ? 1 : -1);
            transform.localScale = scale;
        }

        public void PlayIdle() => animator.Play("idle1_1");
        public void PlayIdle2() => animator.Play("idle2");
        public void PlayAttack() => animator.Play("attack");
        public void PlaySummon() => animator.Play("summon");
        public void PlaySkill1() => animator.Play("skill1");
        public void PlayDeath() => animator.Play("death");

        public void PlayHitFlash() => hitFlash.Play();
    }
}