using UnityEngine;
using System.Collections;
namespace Game.Weapon
{
    [RequireComponent(typeof(Animator), typeof(Collider2D))]
    public class FireBall : ProjectileBase
    {
        private Animator anim;
        private Collider2D coll;

        protected override void Awake()
        {
            base.Awake();
            anim = GetComponent<Animator>();
            coll = GetComponent<Collider2D>();
            maxLifetime = 5f;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            coll.enabled = true;
            /*if (anim != null)
                anim.SetTrigger("Charge");*/
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        public override void Init(float damage, Vector3 dir, float speed)
        {
            base.Init(damage, dir, speed);
        }

        protected override void FixedUpdate()
        {
            rb.velocity = direction * speed;
            lifetime += Time.fixedDeltaTime;
            if (lifetime >= maxLifetime)
            {
                gameObject.SetActive(false);
            }
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & enemyLayer.value) != 0)
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
                    damageable.OnDamaged(damage, direction);
                }
                rb.velocity = Vector2.zero;
                coll.enabled = false;
                if (anim != null)
                {
                    anim.SetTrigger("Explode");
                    StartCoroutine(DisableAfterAnimation());
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (((1 << other.gameObject.layer) & obstacleLayer) != 0)
            {
                Disable();
            }
        }

        private IEnumerator DisableAfterAnimation()
        {
            // Explode 애니메이션 길이에 맞게 조정 (예: 0.2초)
            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }

        public void OnAnimationEnd()
        {
            Disable();
        }
    }
}