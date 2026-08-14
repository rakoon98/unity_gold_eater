using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text; // World Space TMP (3D 오브젝트용)

    [Header("Normal")]
    private Color normalColor = Color.white;
    private float normalFontSize = 32f;

    [Header("Critical")]
    private Color criticalColor = new Color(1f, 0.3f, 0.1f); // 주황/빨강
    private float criticalFontSize = 64f;

    [Header("Animation")]
    [SerializeField] private float moveDistance = 1.5f;
    [SerializeField] private float lifeTime = 1.2f;
    [SerializeField] private float fadeStartRatio = 0.6f;
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 감속 커브

    private float timer;
    private Vector3 startPos;
    private Vector3 moveDir = Vector3.up;

    public void Setup(float damage, bool isCritical)
    {
        if (isCritical)
        {
            text.text = damage.ToString() + "!";
            text.color = criticalColor;
            text.fontSize = criticalFontSize;
            text.fontStyle = FontStyles.Bold;
        }
        else
        {
            text.text = damage.ToString();
            text.color = normalColor;
            text.fontSize = normalFontSize;
            text.fontStyle = FontStyles.Normal;
        }

        // 랜덤 좌우 흔들림 (겹치는 것 방지)
        moveDir = new Vector3(Random.Range(-0.3f, 0.3f), 1f, 0f).normalized;
        Debug.Log($"Setup 호출됨: damage={damage}, isCritical={isCritical}");
        startPos = transform.position;
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float t = timer / lifeTime;

        //transform.position += moveDir * moveSpeed * Time.deltaTime;
        //transform.localScale = Vector3.one * scaleCurve.Evaluate(t);

        float moveT = moveCurve.Evaluate(Mathf.Clamp01(t));
        transform.position = startPos + moveDir * moveDistance * moveT;

        // fadeStartRatio 시점부터 알파만 서서히 감소
        Color c = text.color;
        c.a = Mathf.Lerp(1f, 0f, Mathf.Clamp01((t - fadeStartRatio) / (1f - fadeStartRatio)));
        text.color = c;

        if (timer >= lifeTime)
            Destroy(gameObject);
    }
}