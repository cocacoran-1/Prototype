using UnityEngine;

namespace Game.Weapon
{
    [CreateAssetMenu(fileName = "FireBallSkillData", menuName = "Skills/FireBallSkillData")]
    public class FireBallSkillData : SkillData
    {
        public float baseDamage = 10f;
        public float projectileSpeed = 5f;
    }
}