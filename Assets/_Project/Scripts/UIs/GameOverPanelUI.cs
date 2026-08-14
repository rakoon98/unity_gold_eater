using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GoldEater
{
    public class GameOverPanelUI : BasePanel
    {

        [SerializeField] Button retryBtn;
        [SerializeField] Button exitBtn;

        private void Start()
        {
            retryBtn.onClick.AddListener(() =>
            {
                Retry();
            });
            exitBtn.onClick.AddListener(() =>
            {
                GoTitle();
            });
        }

        public override void Open()
        {
            base.Open();

            Time.timeScale = 0f;
            //retryBtn.Select();
        }

        public override void Close()
        {
            Time.timeScale = 1f;

            base.Close();
        }


        // 재시작, 타이틀로가기 누르면 플레이어 삭제.
        public void Retry()
        {
            //Time.timeScale = 1f;

            //Close();
            //PlayerManager.instance.Respawn();
            //SceneLoadManager.instance.ChangeScene(SceneNames.Stage1).Forget();

            SceneLoadManager.instance.ChangeScene(SceneNames.Stage1).Forget();
            Close();
            //SceneLoadManager.instance.RetryCurrentStage().Forget();
            PlayerManager.instance.ClearPlayer();
        }

        public void GoTitle()
        {
            //Time.timeScale = 1f;
            SceneLoadManager.instance.ChangeScene(SceneNames.Title).Forget();
            Close();
            PlayerManager.instance.ClearPlayer();
            //PlayerManager.instance.Respawn();
        }
    }
}