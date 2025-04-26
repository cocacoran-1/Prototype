using Game.Managers;
using UnityEngine;

namespace Game.Weapon
{
    public class FireZoneSkill : IWeaponSkill
    {
        private FireWeapon weapon;
        private FireZoneSkillData skillData;
        private Collider2D[] hitBuffer = new Collider2D[50];

        public FireZoneSkill(FireZoneSkillData data)
        {
            skillData = data;
        }

        public void Setup(WeaponBase context)
        {
            weapon = context as FireWeapon;
            if (weapon == null) return;
            weapon.baseDamage = skillData.damagePerSecond;
            weapon.skillCooldown = skillData.cooldown;
            weapon.attackRange = skillData.radius;
        }

        public void Execute()
        {
            Vector3 spawnPos = weapon.transform.position;
            GameObject zone = ObjectPoolManager.Instance.SpawnFromPool("FireZone", spawnPos, Quaternion.identity);
            if (zone != null && zone.TryGetComponent(out FireZone zoneComponent))
            {
                zoneComponent.Init(skillData.damagePerSecond, skillData.radius, skillData.duration);
            }
        }
    }
}