using UnityEngine;

namespace Game.Weapon
{
    public abstract class SkillData : ScriptableObject
    {
        public float cooldown = 1f;
        public float range = 5f;
    }
}