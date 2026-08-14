using UnityEngine;

namespace GoldEater
{
    public class GoldPickup : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Transform visualTransform; // 회전을 담당할 자식 Transform

        [Header("Drop Effect")]
        [SerializeField] private float upDistance = 1.5f;
        [SerializeField] private float dropDuration = 0.6f;
        [SerializeField] private float rotateSpeed = 720 * 8f; // 시계방향 회전 속도

        private int goldValue;
        private Collider2D col;

        private Vector3 startPosition;
        private float timer;
        private bool dropFinished;

        private void Awake()
        {
            goldValue = Random.Range(1, 6);

            col = GetComponentInChildren<Collider2D>();
            col.enabled = false;
        }

        private void OnEnable()
        {
            startPosition = transform.position;
            timer = 0f;
            dropFinished = false;
        }

        private void Update()
        {
            if (dropFinished)
                return;

            timer += Time.deltaTime;
            float progress = timer / dropDuration;

            // 1. 자식(visualTransform)만 Z축 마이너스(-) 연산으로 시계방향 제자리 자전
            if (visualTransform != null)
            {
                float angle = -timer * rotateSpeed;
                visualTransform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            // 2. 부모는 보정값 없이 pure Sine 곡선 점프만 수행
            float height = Mathf.Sin(progress * Mathf.PI) * upDistance;

            transform.position = new Vector3(
                startPosition.x,
                startPosition.y + height,
                startPosition.z
            );

            // 3. 착지 완료 시 정지 및 정렬
            if (timer >= dropDuration)
            {
                transform.position = startPosition;

                if (visualTransform != null)
                {
                    visualTransform.rotation = Quaternion.identity; // 자식 회전 0도로 정렬
                }

                dropFinished = true;
                col.enabled = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & playerLayer) == 0)
                return;

            GoldInventory inventory = other.GetComponentInParent<GoldInventory>();

            if (inventory == null)
                return;

            inventory.AddGold(goldValue);
            NotificationManager.instance.ShowNotification("골드 획득!");

            Destroy(gameObject);
        }
    }
}