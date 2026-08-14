using GoldEater;
using UnityEngine;
using UnityEngine.UI;

public class HealthCountUI : MonoBehaviour
{
    private PlayerHealth playerHealth;

    [SerializeField] private Image[] heartImages;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite emptyHeart;

    public void Bind(PlayerHealth health)
    {
        // 기존 플레이어 이벤트 해제
        if (playerHealth != null)
        {
            playerHealth.OnDamaged -= UpdateHearts;
        }

        // 새 플레이어 연결
        playerHealth = health;

        if (playerHealth != null)
        {
            playerHealth.OnDamaged += UpdateHearts;
            UpdateHearts();
        }
    }

    public void Refresh()
    {
        if (playerHealth == null)
            return;

        UpdateHearts();
    }

    private void UpdateHearts()
    {
        if (playerHealth == null)
            return;

        for (int i = 0; i < heartImages.Length; i++)
        {
            HeartState state = playerHealth.GetHeartState(i);

            heartImages[i].sprite = state switch
            {
                HeartState.Full => fullHeart,
                HeartState.Half => halfHeart,
                _ => emptyHeart
            };
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDamaged -= UpdateHearts;
        }
    }
}