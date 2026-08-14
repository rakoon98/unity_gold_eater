// HitFlashEffect.cs (»õ ÆÄÀÏ)
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace GoldEater
{
    public class HitFlashEffect
    {
        private readonly SpriteRenderer[] spriteRenderers;
        private readonly Color[] originalColors;
        private readonly Color hitColor;
        private readonly float flashDuration;
        private readonly int flashCount;

        private CancellationTokenSource cts;

        public HitFlashEffect(Transform root, Color hitColor, float flashDuration = 0.1f, int flashCount = 2)
        {
            spriteRenderers = root.GetComponentsInChildren<SpriteRenderer>();
            originalColors = new Color[spriteRenderers.Length];
            for (int i = 0; i < spriteRenderers.Length; i++)
                originalColors[i] = spriteRenderers[i].color;

            this.hitColor = hitColor;
            this.flashDuration = flashDuration;
            this.flashCount = flashCount;
        }

        public void Play()
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = new CancellationTokenSource();

            Routine(cts.Token).Forget();
        }

        private async UniTaskVoid Routine(CancellationToken token)
        {
            try
            {
                for (int i = 0; i < flashCount; i++)
                {
                    SetColor(hitColor);
                    await UniTask.Delay(System.TimeSpan.FromSeconds(flashDuration), cancellationToken: token);

                    RestoreOriginalColors();
                    await UniTask.Delay(System.TimeSpan.FromSeconds(flashDuration), cancellationToken: token);
                }
            }
            catch (System.OperationCanceledException)
            {
                RestoreOriginalColors();
            }
        }

        private void SetColor(Color color)
        {
            foreach (var sr in spriteRenderers)
                sr.color = color;
        }

        private void RestoreOriginalColors()
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
                spriteRenderers[i].color = originalColors[i];
        }

        public void Dispose()
        {
            cts?.Cancel();
            cts?.Dispose();
        }
    }
}