using UnityEngine;

namespace Game.Weapon
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class FireZone : MonoBehaviour
    {
        private float damagePerSecond;
        private float radius;
        private float duration;
        private float timer;
        private float damageInterval = 0.5f; // 0.5초마다 데미지 적용
        private float damageTimer;
        private Collider2D[] hitBuffer = new Collider2D[50];
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private ParticleSystem flameEffect;

        public void Init(float damage, float radius, float duration)
        {
            this.damagePerSecond = damage;
            this.radius = radius;
            this.duration = duration;
            timer = 0f;
            damageTimer = 0f;

            CircleCollider2D coll = GetComponent<CircleCollider2D>();
            coll.radius = radius;
            coll.isTrigger = true;

            if (flameEffect != null)
                flameEffect.Play();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            damageTimer += Time.deltaTime;

            if (timer >= duration)
            {
                if (flameEffect != null)
                    flameEffect.Stop();
                gameObject.SetActive(false);
                return;
            }

            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;
                ApplyDamage();
            }
        }

        private void ApplyDamage()
        {
            if (enemyLayer == 0)
            {
                Debug.LogError("enemyLayer가 설정되어 있지 않습니다.");
                return;
            }
            int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, radius, hitBuffer, enemyLayer);
            for (int i = 0; i < hitCount; i++)
            {
                if (hitBuffer[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.OnDamaged(damagePerSecond * damageInterval, Vector2.zero);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}