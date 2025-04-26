using UnityEngine;

namespace Game.Weapon
{
    [CreateAssetMenu(fileName = "FirePillarSkillData", menuName = "Skills/FirePillarSkillData")]
    public class FirePillarSkillData : SkillData
    {
        public float baseDamage;
        public int maxTargets = 1;
        public float duration = 0.5f; // Ãß°¡
    }
}