namespace GoldEater
{

    using Cysharp.Threading.Tasks;
    using UnityEngine;

    public class Warp : MonoBehaviour
    {

        public enum WarpType
        {
            RoomToRoom,
            SceneChange
        }

        [SerializeField] private WarpType warpType;

        [Header("Room 변경 시")]
        [SerializeField] private Room targetRoom;

        [Header("Scene 이동 시")]
        [SerializeField] private SceneNames targetSceneName;
        [SerializeField] private Vector2 targetSpawnPosition;

        private StageManager stageManager;

        public void SetStageManager(StageManager manager) => this.stageManager = manager;

        public void Activate()
        {
            //SceneManager.UnloadSceneAsync(nameof(currentSceneName));
            //SceneManager.LoadSceneAsync(nameof(targetSceneName), LoadSceneMode.Additive);

            switch (warpType)
            {
                case WarpType.RoomToRoom:
                    stageManager.EnterRoom(targetRoom);
                    break;
                case WarpType.SceneChange:
                    SceneLoadManager.instance.ChangeScene(targetSceneName).Forget();
                    break;
            }
        }



    }
}