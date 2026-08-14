using UnityEngine;

public class DamagePopupSpawner : MonoBehaviour
{
    public static DamagePopupSpawner Instance;
    [SerializeField] private DamagePopup popupPrefab;
    [SerializeField] private Canvas targetCanvas;

    private void Awake() => Instance = this;

    public void Spawn(Vector3 worldPos, float damage, bool isCritical)
    {
        //var popup = Instantiate(popupPrefab, worldPos, Quaternion.identity);
        var popup = Instantiate(popupPrefab, targetCanvas.transform);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // 스크린 좌표 → Canvas 로컬 좌표로 정확히 변환
        RectTransform canvasRect = targetCanvas.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out Vector2 localPoint
        );

        popup.GetComponent<RectTransform>().anchoredPosition = localPoint;
        popup.Setup(damage, isCritical);
    }
}