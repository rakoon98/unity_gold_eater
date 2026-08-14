namespace GoldEater
{
    using Unity.Cinemachine;
    using UnityEngine;

    public class CameraManager : BaseSingletonManager<CameraManager>
    {
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private CinemachineConfiner2D confiner;

        public void SetTarget(Transform target)
        {
            cinemachineCamera.Follow = target;
        }

        public void SetBounds(Collider2D bounds)
        {
            confiner.BoundingShape2D = bounds;
            // Bounding Shape(콜라이더) 자체가 바뀌었을 때
            confiner.InvalidateBoundingShapeCache();
        }

        public void Warp(Vector3 oldPosition, Vector3 newPosition)
        {
            if (cinemachineCamera.Follow == null)
                return;

            // Follow 대상이 순간이동(텔레포트)했을 때 — 카메라가 부드럽게 따라가지 않고 즉시 그 위치로 점프하게 함
            Vector3 delta = newPosition - oldPosition;
            cinemachineCamera.OnTargetObjectWarped(cinemachineCamera.Follow, delta);
        }
    }

}