using UnityEngine;

namespace Game.Weapon
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class FireThrowerFlame : ProjectileBase
    {
        [SerializeField] private ParticleSystem flameTrail;

        protected override void Awake()
        {
            base.Awake();
            maxLifetime = 0.7f; // 범위/속도 = 5/7
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (flameTrail != null)
            {
                var main = flameTrail.main;
                main.maxParticles = 20;
                var emission = flameTrail.emission;
                emission.rateOverTime = 10f;
                flameTrail.Play();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (flameTrail != null)
                flameTrail.Stop();
        }
    }
}