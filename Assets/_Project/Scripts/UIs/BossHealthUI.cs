using UnityEngine;
using UnityEngine.UI;

namespace GoldEater
{
    public class BossHealthUI : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        private BossHealth bossHealth;

        public void Bind(BossHealth health)
        {
            Reset();

            bossHealth = health;

            bossHealth.OnHealthChanged += UpdateHealth;
            bossHealth.OnDeath += Close;
            bossHealth.OnDestroyBoss += Reset;

            UpdateHealth(
                bossHealth.CurrentHp,
                bossHealth.MaxHp
            );

            Open();
        }

        //public void Bind(BossHealth health)
        //{
        //    // 이전 보스 이벤트 해제
        //    if (bossHealth != null)
        //    {
        //        bossHealth.OnHealthChanged -= UpdateHealth;
        //        bossHealth.OnDeath -= Close;
        //    }

        //    bossHealth = health;

        //    if (bossHealth == null)
        //    {
        //        Close();
        //        return;
        //    }

        //    // 새 보스 이벤트 등록
        //    bossHealth.OnHealthChanged += UpdateHealth;
        //    bossHealth.OnDeath += Close;

        //    // 초기 UI 갱신
        //    UpdateHealth(bossHealth.CurrentHp, bossHealth.MaxHp);

        //    Open();
        //}

        private void UpdateHealth(float current, float max)
        {
            fillImage.fillAmount = current / max;
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (bossHealth != null)
            {
                bossHealth.OnHealthChanged -= UpdateHealth;
                bossHealth.OnDeath -= Close;
            }
        }

        public void Reset()
        {
            if (bossHealth != null)
            {
                bossHealth.OnHealthChanged -= UpdateHealth;
                bossHealth.OnDeath -= Close;
            }

            bossHealth = null;

            fillImage.fillAmount = 0;

            Close();
        }
    }
}