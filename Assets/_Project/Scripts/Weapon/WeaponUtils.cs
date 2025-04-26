using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapon
{
    public static class WeaponUtils
    {
        public static Collider2D FindClosestTarget(Vector3 position, float range, Collider2D[] buffer, LayerMask layer)
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(position, range, buffer, layer);
            Collider2D closest = null;
            float minDist = float.MaxValue;

            for (int i = 0; i < hitCount; i++)
            {
                float dist = Vector3.Distance(buffer[i].transform.position, position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = buffer[i];
                }
            }

            return closest;
        }

        public static void ApplyDamageToTargets(Vector3 position, float radius, float damage, Collider2D[] buffer, LayerMask layer)
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(position, radius, buffer, layer);
            for (int i = 0; i < hitCount; i++)
            {
                if (buffer[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.OnDamaged(damage, Vector2.zero);
                }
            }
        }

        public static List<Collider2D> FindEnemiesInViewport(Vector3 position, float range, Collider2D[] buffer, LayerMask enemyLayerMask)
        {
            if (enemyLayerMask == 0)
            {
                return new List<Collider2D>();
            }

            int hitCount = Physics2D.OverlapCircleNonAlloc(position, range, buffer, enemyLayerMask);
            List<Collider2D> visibleEnemies = new List<Collider2D>();
            Camera cam = Camera.main;

            for (int i = 0; i < hitCount; i++)
            {
                Vector3 viewportPos = cam.WorldToViewportPoint(buffer[i].transform.position);
                if (viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1 && viewportPos.z > 0)
                {
                    visibleEnemies.Add(buffer[i]);
                }
            }

            return visibleEnemies;
        }

        public static Collider2D FindClosestInViewport(Vector3 position, float range, Collider2D[] buffer, LayerMask layerMask)
        {
            var enemies = FindEnemiesInViewport(position, range, buffer, layerMask);
            if (enemies.Count == 0) return null;

            Collider2D closest = enemies[0];
            float minDist = Vector3.Distance(position, closest.transform.position);

            for (int i = 1; i < enemies.Count; i++)
            {
                float dist = Vector3.Distance(position, enemies[i].transform.position);
                if (dist < minDist)
                {
                    closest = enemies[i];
                    minDist = dist;
                }
            }

            return closest;
        }

    }
}
