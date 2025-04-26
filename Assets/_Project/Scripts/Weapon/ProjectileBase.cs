using UnityEngine;

namespace Game.Weapon
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class ProjectileBase : MonoBehaviour
    {
        protected Rigidbody2D rb;
        protected float damage;
        protected Vector3 direction;
        protected float speed;
        protected float maxLifetime = 5f; // 최대 생존 시간
        protected float lifetime;

        [SerializeField] protected LayerMask enemyLayer; // 적 레이어 마스크
        [SerializeField] protected LayerMask obstacleLayer; // 벽/경계 레이어 마스크

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void OnEnable()
        {
            lifetime = 0f;
        }

        protected virtual void OnDisable()
        {
            rb.velocity = Vector2.zero; // 속도 초기화
        }

        public virtual void Init(float damage, Vector3 dir, float speed)
        {
            this.damage = damage;
            this.direction = dir.normalized;
            this.speed = speed;
        }

        protected virtual void FixedUpdate()
        {
            rb.velocity = direction * speed;
            lifetime += Time.fixedDeltaTime;
            Debug.Log($"FireBall lifetime = {lifetime}, maxLifetime = {maxLifetime}");
            if (lifetime >= maxLifetime)
                gameObject.SetActive(false);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (enemyLayer == 0)
            {
                Debug.LogError("enemyLayer가 설정되어 있지 않습니다.");
                return;
            }

            if (((1 << other.gameObject.layer) & enemyLayer) != 0)
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
                    damageable.OnDamaged(damage, direction);
                }
                gameObject.SetActive(false);
            }
            else if (((1 << other.gameObject.layer) & obstacleLayer) != 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public interface IDamageable
    {
        void OnDamaged(float damage, Vector2 hitDirection);
    }
}