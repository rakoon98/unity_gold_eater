using UnityEngine;
using UnityEngine.UI;

namespace GoldEater
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private EnemyHealth enemyHealth;
        [SerializeField] private Image fillImage;
        [SerializeField] private GameObject barRoot; // 풀피일 때 숨기고 싶으면 사용 (선택)
        [SerializeField] private bool hideWhenFull = true;

        private void Awake()
        {
            if (enemyHealth == null)
                enemyHealth = GetComponent<EnemyHealth>();
        }

        private void OnEnable()
        {
            Debug.Log($"[HealthBar] OnEnable called, enemyHealth null?: {enemyHealth == null}");
            if (enemyHealth != null)
                enemyHealth.OnDamaged += UpdateBar;
        }

        private void OnDisable()
        {
            if (enemyHealth != null)
                enemyHealth.OnDamaged -= UpdateBar;
        }

        private void Start()
        {
            UpdateBar();
        }

        private void UpdateBar()
        {
            float ratio = enemyHealth.currentHp / enemyHealth.maxHearts;
            fillImage.fillAmount = ratio;
            Debug.Log($"[HealthBar] UpdateBar called, ratio: {ratio}, fillImage null? {fillImage == null}, fillImage name: {fillImage?.gameObject.name}");
            if (hideWhenFull && barRoot != null)
                barRoot.SetActive(ratio < 1f); // 풀피일 땐 숨김 (선택사항)
        }
    }
}