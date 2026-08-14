namespace GoldEater
{
    using UnityEngine;

    public class SafeManager : MonoBehaviour
    {

        private PlayerController player;
        [SerializeField] private PolygonCollider2D cameraBounds;

        private void Awake()
        {

        }

        void Start()
        {
            Transform focusTarget = GameObject.Find("SpawnPoint")?.transform;
            PlayerManager.instance.Spawn(focusTarget.position);
            CameraManager.instance.SetBounds(cameraBounds);
            CameraManager.instance.SetTarget(PlayerManager.instance.player.transform);
            //CameraManager.instance.Warp();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}