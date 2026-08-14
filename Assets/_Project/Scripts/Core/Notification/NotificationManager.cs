using Cysharp.Threading.Tasks;
using GoldEater;
using UnityEngine;

namespace GoldEater
{
    public class NotificationManager : BaseSingletonManager<NotificationManager>
    {       

        [SerializeField] private NotificationItem notificationPrefab;
        [SerializeField] private Transform container; // Vertical Layout Group이 부착된 패널

        public void ShowNotification(string message)
        {
            NotificationItem item = Instantiate(notificationPrefab, container);

            // UniTask 실행 (Forget으로 Fire-and-forget 처리)
            item.ShowAsync(message, this.GetCancellationTokenOnDestroy()).Forget();
        }
    }
}