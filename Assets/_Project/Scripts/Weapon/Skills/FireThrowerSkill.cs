using Game.Managers;
using UnityEngine;
using Game.Player;

namespace Game.Weapon
{
    public class FireThrowerSkill : IWeaponSkill
    {
        private FireWeapon weapon;
        private FireThrowerSkillData skillData;
        private PlayerController playerController;
        private float fireTimer;

        public FireThrowerSkill(FireThrowerSkillData data)
        {
            skillData = data;
        }

        public void Setup(WeaponBase context)
        {
            weapon = context as FireWeapon;
            if (weapon == null) return;
            weapon.baseDamage = skillData.damagePerShot;
            weapon.skillCooldown = skillData.cooldown;
            weapon.attackRange = skillData.range;
            playerController = weapon.GetComponentInParent<PlayerController>();
            fireTimer = 0f;
        }

        public void Execute()
        {
            fireTimer += Time.deltaTime;
            if (fireTimer < skillData.fireInterval) return;
            fireTimer = 0f;

            Vector3 baseDir = playerController != null && playerController.inputVec != Vector2.zero
                ? playerController.inputVec.normalized
                : weapon.transform.right;

            float halfAngle = skillData.coneAngle / 2f;
            for (int i = 0; i < skillData.projectilesPerShot; i++)
            {
                float angle = Random.Range(-halfAngle, halfAngle);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Vector3 dir = rotation * baseDir;

                GameObject flame = ObjectPoolManager.Instance.SpawnFromPool("FireThrowerFlame", weapon.transform.position, Quaternion.FromToRotation(Vector3.right, dir));
                if (flame != null && flame.TryGetComponent(out ProjectileBase proj))
                {
                    proj.Init(skillData.damagePerShot, dir, skillData.projectileSpeed);
                }
            }
        }
    }
}