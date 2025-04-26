using Game.Managers;
using UnityEngine;

namespace Game.Weapon
{
    public class FireBallSkill : IWeaponSkill
    {
        private FireWeapon weapon;
        private FireBallSkillData skillData;
        private Collider2D[] hitBuffer = new Collider2D[60];

        public FireBallSkill(FireBallSkillData data)
        {
            skillData = data;
        }

        public void Setup(WeaponBase context)
        {
            weapon = context as FireWeapon;
            
            weapon.baseDamage = skillData.baseDamage;
            weapon.skillCooldown = skillData.cooldown;
            weapon.attackRange = skillData.range;
        }

        public void Execute()
        {
            Collider2D closest = WeaponUtils.FindClosestInViewport(
               weapon.transform.position,
               weapon.attackRange,
               hitBuffer,
               LayerMask.GetMask("Enemy")
           );
            if (closest == null) return;

            Vector3 dir = (closest.transform.position - weapon.transform.position).normalized;
            GameObject bullet = ObjectPoolManager.Instance.SpawnFromPool(
                "FireBall", 
                weapon.transform.position, 
                Quaternion.FromToRotation(Vector3.right, dir)
                );
            if (bullet != null && bullet.TryGetComponent(out ProjectileBase proj))
            {
                proj.Init(weapon.baseDamage, dir, skillData.projectileSpeed);
            }

        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (weapon != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(weapon.transform.position, weapon.attackRange);
            }
        }
#endif
    }
}