using UnityEngine;

namespace Game.Weapon
{
    [CreateAssetMenu(fileName = "FireThrowerSkillData", menuName = "Skills/FireThrowerSkillData")]
    public class FireThrowerSkillData : SkillData
    {
        public float damagePerShot = 3f;
        public float fireInterval = 0.1f;
        public float projectileSpeed = 7f;
        public float coneAngle = 30f; 
        public int projectilesPerShot = 3; 
    }
}
