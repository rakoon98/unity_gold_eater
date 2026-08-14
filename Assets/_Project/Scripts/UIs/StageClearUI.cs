using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoldEater
{
    public class StageClearUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countText;

        private CancellationTokenSource cts;

        bool opened = false;

        public void Toggle()
        {
            opened = !opened;
            if(opened) 
                gameObject.SetActive(opened);    

        }

        private void OnDestroy()
        {
            cts?.Cancel();
            cts?.Dispose();
        }

        public void OnBossCleared()
        {
            cts = new CancellationTokenSource();
            CountdownAndLoadVillage(cts.Token).Forget();
        }

        private async UniTaskVoid CountdownAndLoadVillage(CancellationToken token)
        {
            countText.gameObject.SetActive(true);

            GoldInventory inventory = PlayerManager.instance.player.GetComponentInParent<GoldInventory>();
            if (inventory != null)
            {
                inventory.AddGold(20);
            }

            for (int i = 3; i > 0; i--)
            {
                countText.text = $"보스를 잡았습니다.!!!  {i}초후에 마을로 돌아갑니다.";
                await UniTask.Delay(1000, cancellationToken: token);
            }

            countText.gameObject.SetActive(false);

            await SceneLoadManager.instance.ChangeScene(SceneNames.SafeZone);
            //await SceneManager.LoadSceneAsync(villageSceneName).ToUniTask(cancellationToken: token);
        }
    }
}