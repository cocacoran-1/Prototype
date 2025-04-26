using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Game.CameraSystem
{
    [RequireComponent(typeof(PixelPerfectCamera))]
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);
        private PixelPerfectCamera pixelCamera;

        private void Awake()
        {
            pixelCamera = GetComponent<PixelPerfectCamera>();
        }

        private void LateUpdate()
        {
            if (target == null) return;

            // �÷��̾� ��ġ + ������
            Vector3 targetPosition = target.position + offset;

            // �ȼ� ���� ����
            float unitsPerPixel = 1f / pixelCamera.assetsPPU;
            float snappedX = Mathf.Round(targetPosition.x / unitsPerPixel) * unitsPerPixel;
            float snappedY = Mathf.Round(targetPosition.y / unitsPerPixel) * unitsPerPixel;

            transform.position = new Vector3(snappedX, snappedY, targetPosition.z);
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}
