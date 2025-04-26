using Game.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapon
{
    public class FirePillarSkill : IWeaponSkill
    {
        private FireWeapon weapon;
        private FirePillarSkillData skillData;
        private Collider2D[] hitBuffer = new Collider2D[60];

        public FirePillarSkill(FirePillarSkillData data)
        {
            skillData = data;
            Debug.Log($"FirePillarSkill created with data: Cooldown = {data.cooldown}, Range = {data.range}, Damage = {data.baseDamage}, MaxTargets = {data.maxTargets}, Duration = {data.duration}");
        }

        public void Setup(WeaponBase context)
        {
            weapon = context as FireWeapon;
            if (weapon == null) return;
            weapon.baseDamage = skillData.baseDamage; 
            weapon.skillCooldown = skillData.cooldown;
            weapon.attackRange = skillData.range; 
            Debug.Log($"FirePillarSkill Setup: baseDamage = {weapon.baseDamage}, skillCooldown = {weapon.skillCooldown}, attackRange = {weapon.attackRange}");
        }

        public void Execute()
        {
            Debug.Log("FirePillarSkill.Execute called");
            Collider2D[] buffer = new Collider2D[10];
            int enemyLayerIndex = LayerMask.NameToLayer("Enemy");
            if (enemyLayerIndex == -1)
            {
                Debug.LogError("Enemy layer not found! Cannot execute FirePillarSkill.");
                return;
            }
            LayerMask enemyMask = 1 << enemyLayerIndex;
            List<Collider2D> visibleEnemies = WeaponUtils.FindEnemiesInViewport(weapon.transform.position, weapon.attackRange, buffer, enemyMask);
            if (visibleEnemies.Count == 0)
            {
                Debug.LogWarning("No visible enemies found for FirePillar!");
                return;
            }

            int targetCount = Mathf.Min(skillData.maxTargets, visibleEnemies.Count);
            List<Collider2D> selectedTargets = new List<Collider2D>();
            for (int i = 0; i < targetCount; i++)
            {
                if (visibleEnemies.Count == 0) break;
                int randomIndex = Random.Range(0, visibleEnemies.Count);
                selectedTargets.Add(visibleEnemies[randomIndex]);
                visibleEnemies.RemoveAt(randomIndex);
            }

            foreach (var target in selectedTargets)
            {
                Vector3 position = target.transform.position;
                GameObject pillar = ObjectPoolManager.Instance.SpawnFromPool("FirePillar", position, Quaternion.identity);
                if (pillar != null && pillar.TryGetComponent(out FirePillar firePillar))
                {
                    Debug.Log($"Spawning FirePillar at {position}, target = {target.gameObject.name}");
                    // Init 매개변수 수정 예정
                    firePillar.Init(weapon.baseDamage, skillData.duration);
                }
            }
        }
    }
}