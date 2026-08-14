using Cysharp.Threading.Tasks;
using UnityEngine;
using GoldEater;
using UnityEngine.UI;

namespace GoldEater
{

    public class StageManager : MonoBehaviour
    {

        [SerializeField] Room[] rooms; // 각 room 의 정보들

        Room currentRoom;
        int currentIndex;

        private float spawnDelay = 0.5f;

        [SerializeField] private Sprite[] backgrounds;
        [SerializeField] SpriteRenderer background;

        [Header("Boss")]
        [SerializeField] GameObject bossPrefab;
        [SerializeField] Transform bossSpawnPosition;

        private GameObject currentBoss;

        private void Awake()
        {
            currentIndex = -1;
        }

        void Start()
        {
            InjectSelfToWarps();

            if (PlayerManager.instance.player == null)
               PlayerManager.instance.Spawn(rooms[0].SpawnPoint.transform.position);
            else
                PlayerManager.instance.Respawn(rooms[0].SpawnPoint.transform.position);

            EnterNextRoom();
        }

        void Update()
        {

        }


        private void SetBackGround()
        {
            background.sprite = backgrounds[currentIndex];
            if(currentRoom.roomIndex == 2)            
                background.size = new Vector2(43.69067f, 24.576f);            
            else            
                background.size = new Vector2(24.576f, 43.69067f);            
        }

        private void InjectSelfToWarps()
        {
            foreach (var room in rooms)
            {
                Warp[] warpsInRoom = room.GetComponentsInChildren<Warp>(true); // room 기준으로 호출
                foreach (var warp in warpsInRoom)
                {
                    warp.SetStageManager(this);
                }
            }
        }

        // 순서대로 이동
        public void EnterNextRoom()
        {
            if (currentIndex + 1 >= rooms.Length)
                return;

            if (currentRoom != null)
                currentRoom.gameObject.SetActive(false);

            currentIndex++;
            currentRoom = rooms[currentIndex];

            currentRoom.gameObject.SetActive(true);
            Physics2D.SyncTransforms();

            Vector3 oldPosition = PlayerManager.instance.player.transform.position;
            Vector3 newPosition = currentRoom.SpawnPoint.position;
            PlayerManager.instance.MoveTo(newPosition);

            CameraManager.instance.SetBounds(currentRoom.CameraBounds);
            CameraManager.instance.Warp(oldPosition, newPosition);
            SpawnEnemiesWithDelay().Forget();
            SetBackGround();

            if (currentIndex == 2)
                SpawnBossWithDelay().Forget();
        }

        // 지정된 룸으로 이동
        public void EnterRoom(Room room)
        {
            if (currentRoom != null)
                currentRoom.gameObject.SetActive(false); // 추가: 이전 Room 끄기

            currentRoom = room;
            currentIndex = room.roomIndex;
            currentRoom.gameObject.SetActive(true); // 추가: 새 Room 켜기
            Physics2D.SyncTransforms();

            Vector3 oldPosition = PlayerManager.instance.player.transform.position;
            Vector3 newPosition = currentRoom.SpawnPoint.position;
            PlayerManager.instance.MoveTo(newPosition);
            CameraManager.instance.SetBounds(currentRoom.CameraBounds);
            CameraManager.instance.Warp(oldPosition, newPosition);
            SpawnEnemiesWithDelay().Forget();
            SetBackGround();

            if (currentIndex == 2)
                SpawnBossWithDelay().Forget();
        }

        private async UniTaskVoid SpawnBossWithDelay()
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(spawnDelay));
            currentBoss = Instantiate(bossPrefab, bossSpawnPosition.position, Quaternion.identity, bossSpawnPosition);
        }

        public void ClearBoss()
        {
            if (currentBoss != null)
            {
                Destroy(currentBoss);
                currentBoss = null;
            }
        }

        private async UniTaskVoid SpawnEnemiesWithDelay()
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(spawnDelay));
            currentRoom.SpawnEnemies();
        }

    }

}