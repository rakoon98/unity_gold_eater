using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;

namespace GoldEater
{

    public class NotificationItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI msgText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform rectTransform;

        [Header("Settings")]
        [SerializeField] private float displayDuration = 2.5f;
        [SerializeField] private float fadeDuration = 0.6f;
        [SerializeField] private float moveDistance = 30f; // 위로 떠오를 거리

        public async UniTask ShowAsync(string message, CancellationToken cancellationToken = default)
        {
            msgText.text = message;
            canvasGroup.alpha = 0f;

            Vector2 startPos = rectTransform.anchoredPosition;
            Vector2 targetPos = startPos + new Vector2(0f, moveDistance);

            // 1. Fade In + Move Up
            float timer = 0f;
            while (timer < fadeDuration)
            {
                cancellationToken.ThrowIfCancellationRequested();

                timer += Time.deltaTime;
                float progress = timer / fadeDuration;

                canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
                rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, progress);

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            canvasGroup.alpha = 1f;
            rectTransform.anchoredPosition = targetPos;

            // 2. Display Wait
            await UniTask.Delay((int)(displayDuration * 1000), cancellationToken: cancellationToken);

            // 3. Fade Out
            timer = 0f;
            while (timer < fadeDuration)
            {
                cancellationToken.ThrowIfCancellationRequested();

                timer += Time.deltaTime;
                float progress = timer / fadeDuration;

                canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            Destroy(gameObject);
        }
    }
}