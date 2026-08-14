namespace GoldEater
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.SceneManagement;

    public class SceneLoadManager : BaseSingletonManager<SceneLoadManager>
    {

        public bool IsLoading => _isLoading;
        private bool _isLoading;

        private SceneNames _currentScene;

        private string GetSceneName(SceneNames scene)
        {
            return $"{scene}Scene";
        }

        /// <summary>
        /// Bootstrap 시작 시 호출
        /// </summary>
        public async UniTask Initialize()
        {
            //await SceneManager.LoadSceneAsync(
            //    GetSceneName(SceneNames.Intro),
            //    LoadSceneMode.Additive);

            //_currentScene = SceneNames.Intro;

            Debug.Log("[SceneLoadManager] Initialize 시작");

            await SceneManager.LoadSceneAsync(
                GetSceneName(SceneNames.Intro),
                LoadSceneMode.Additive);

            Debug.Log("[SceneLoadManager] Intro 씬 로드 완료");

            _currentScene = SceneNames.Intro;

            Debug.Log("[SceneLoadManager] Initialize 끝");
        }

        /// <summary>
        /// 씬 전환
        /// </summary>
        public async UniTask ChangeScene(SceneNames nextScene)
        {
            if (_isLoading)
                return;

            _isLoading = true;
            EventSystem.current.SetSelectedGameObject(null);

            await FadeManager.instance.FadeOut();
            //await UniTask.Delay(3000);
            // 이렇게 바꾸어 실행해 보세요.
            //await UniTask.Delay(System.TimeSpan.FromSeconds(3));

            // 다음 씬 로드
            await SceneManager.LoadSceneAsync(GetSceneName(nextScene), LoadSceneMode.Additive);

            // HUD 관리 
            if (nextScene == SceneNames.Intro || nextScene == SceneNames.Title)
                UIManager.instance.HUD.Close();
            else
                UIManager.instance.HUD.Open();

            // 이전 씬 제거
            await SceneManager.UnloadSceneAsync(GetSceneName(_currentScene));


            _currentScene = nextScene;

            await FadeManager.instance.FadeIn();
        
            _isLoading = false;
        }

        //public async UniTask RetryCurrentStage()
        //{
        //    await ChangeScene(_currentScene);
        //    PlayerManager.instance.Respawn();
        //    UIManager.instance.Reset();


        //    //===
        //    PlayerManager.instance.ClearPlayer();
        //    UIManager.instance.Reset();
        //    await ChangeScene(_currentScene);
        //    PlayerManager.instance.Respawn();
        //    UIManager.instance.RebindPlayer(PlayerManager.instance.playerController);
        //}
    }
}