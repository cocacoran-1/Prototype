using UnityEngine;

namespace Game.Weapon
{
    [CreateAssetMenu(fileName = "FireZoneSkillData", menuName = "Skills/FireZoneSkillData")]
    public class FireZoneSkillData : SkillData
    {
        public float damagePerSecond = 5f;
        public float duration = 3f;
        public float radius = 3f;
    }
}
