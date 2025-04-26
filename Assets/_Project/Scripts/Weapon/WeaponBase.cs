using UnityEngine;
using Game.Player;

namespace Game.Weapon
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [SerializeField] protected PlayerController playerController;
        public float baseDamage;
        public float skillCooldown;
        public float attackRange;
        protected float timer;

        public virtual void Init()
        {
            if (playerController == null)
            {
                Debug.LogWarning("PlayerController is not assigned in WeaponBase!");
            }
        }

        public abstract void FireSkill();

        public virtual void LevelUp(int damageDelta, int countDelta)
        {
            baseDamage += damageDelta;
        }
    }
}