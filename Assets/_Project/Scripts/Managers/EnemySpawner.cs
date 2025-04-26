using UnityEngine;
using Game.Managers;
using Game.Enemy;

namespace Game.Managers
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float spawnInterval = 2f;
        [SerializeField] private float spawnRadius = 8f;
        [SerializeField] private float safeRadius = 2f;

        private float timer;
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!GameManager.Instance.isGameActive) return;

            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnEnemy();
                timer = 0f;
            }
        }

        private void SpawnEnemy()
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            Vector2 spawnPos = (Vector2)player.position + randomDir * Random.Range(safeRadius, spawnRadius);

            Vector3 viewportPos = mainCamera.WorldToViewportPoint(spawnPos);
            viewportPos.x = Mathf.Clamp01(viewportPos.x);
            viewportPos.y = Mathf.Clamp01(viewportPos.y);
            spawnPos = mainCamera.ViewportToWorldPoint(viewportPos);

            GameObject enemy = ObjectPoolManager.Instance.SpawnFromPool("Enemy", spawnPos, Quaternion.identity);
            if (enemy != null)
            {
                Debug.Log($"Spawned enemy at {spawnPos}, Layer = {LayerMask.LayerToName(enemy.layer)}");
                if (enemy.TryGetComponent(out EnemyBase enemyBase))
                {
                }
            }
        }
    }
}