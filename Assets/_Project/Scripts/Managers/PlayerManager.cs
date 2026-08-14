using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace GoldEater
{
    public class PlayerManager : BaseSingletonManager<PlayerManager>
    {

        [SerializeField] GameObject playerPrefab;

        public GameObject player { get; private set; }
        public PlayerController playerController { get; private set; }
        public PlayerController GetPlayerController() => playerController;

        public bool hasPlayer => player != null;

        public void Spawn(Vector3 position)
        {
            if (player != null) return;

            CreatePlayer(position);
        }

        public void ClearPlayer()
        {
            if (player != null)
            {
                Destroy(player);
            }

            player = null;
            playerController = null;
        }

        public void Respawn(Vector3 position)
        {
            ClearPlayer();
            CreatePlayer(position);
            playerController.ResetPlayer(position);
        }

        private void CreatePlayer(Vector3 position)
        {
            player = Instantiate(
                 playerPrefab,
                 position,
                 Quaternion.identity
            );

            playerController = player.GetComponent<PlayerController>();

            var persistentScene = SceneManager.GetSceneByName("PersistentScene");
            SceneManager.MoveGameObjectToScene( player,persistentScene );
            CameraManager.instance.SetTarget( player.transform);
            UIManager.instance.RebindPlayer(      playerController    );

            //player = Instantiate(playerPrefab, position, Quaternion.identity);
            //playerController = player.GetComponent<PlayerController>();

            //var persistentScene = SceneManager.GetSceneByName("PersistentScene");
            //SceneManager.MoveGameObjectToScene(player, persistentScene);

            //CameraManager.instance.SetTarget(player.transform);
            //UIManager.instance.SetSkillDashConnect(playerController);
        }


        public void MoveTo(Vector3 spawnPoint)
        {
            if (player == null)
                return;


            if (player.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.linearVelocity = Vector2.zero;
                rb.position = spawnPoint;
            }
            else
            {
                player.transform.position = spawnPoint;
            }
        }

    }

}