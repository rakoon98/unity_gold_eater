using Cysharp.Threading.Tasks;
using UnityEngine;

public class FadeManager : BaseSingletonManager<FadeManager>
{

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private float duration = 0.5f;

    public async UniTask FadeOut()
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(0, 1, t / duration);

            await UniTask.Yield();
        }

        canvasGroup.alpha = 1;
    }

    public async UniTask FadeIn()
    {
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;

            canvasGroup.alpha = Mathf.Lerp(1, 0, t / duration);

            await UniTask.Yield();
        }

        canvasGroup.alpha = 0;
    }
}