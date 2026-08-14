using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GoldEater
{
    public class InteractionTooltip : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform tooltip;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI label;   // 텍스트 표시용
        [SerializeField] private Image icon;               // 아이콘 표시용

        [Header("Position")]
        [SerializeField] private Vector3 offset = new(0f, 1.2f, 0f);

        [Header("Animation")]
        [SerializeField] private float duration = 0.15f;

        [Header("Default Content")]
        [SerializeField] private string defaultText = "상호작용";
        [SerializeField] private Sprite defaultIcon;

        private bool isShowing;
        private System.Threading.CancellationToken destroyToken;

        private void Awake()
        {
            tooltip.localScale = new Vector3(0f, 1f, 1f);
            canvasGroup.alpha = 0f;
            destroyToken = this.GetCancellationTokenOnDestroy();

            SetContent(defaultText, defaultIcon);
        }

        private void LateUpdate()
        {
            tooltip.position = transform.position + offset;
        }

        /// <summary>
        /// 텍스트와 아이콘을 외부에서 지정할 때 사용 (Warp, NPC, 상자 등 재사용 시 호출)
        /// </summary>
        public void SetContent(string text, Sprite iconSprite = null)
        {
            if (label != null)
                label.text = text;

            if (icon != null)
            {
                if (iconSprite != null)
                {
                    icon.sprite = iconSprite;
                    icon.enabled = true;
                }
                else
                {
                    icon.enabled = false; // 아이콘 없으면 숨김
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;
            Show().Forget();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;
            Hide().Forget();
        }

        private async UniTaskVoid Show()
        {
            if (isShowing)
                return;
            isShowing = true;
            float time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                float t = Mathf.Clamp01(time / duration);
                tooltip.localScale = new Vector3(t, 1, 1);
                canvasGroup.alpha = t;
                await UniTask.Yield(destroyToken);
            }
            tooltip.localScale = Vector3.one;
            canvasGroup.alpha = 1;
        }

        private async UniTaskVoid Hide()
        {
            if (!isShowing)
                return;
            isShowing = false;
            float time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                float t = 1 - Mathf.Clamp01(time / duration);
                tooltip.localScale = new Vector3(t, 1, 1);
                canvasGroup.alpha = t;
                await UniTask.Yield(destroyToken);
            }
            tooltip.localScale = new Vector3(0, 1, 1);
            canvasGroup.alpha = 0;
        }
    }
}