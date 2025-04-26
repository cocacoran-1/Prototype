using UnityEngine;
using Game.Weapon;

namespace Game.Enemy
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class EnemyBase : MonoBehaviour, IDamageable
    {
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private float maxHP = 10f;
        protected float currentHP;

        protected Rigidbody2D rb;
        protected SpriteRenderer sprite;
        protected Transform player;

        [Header("Knockback")]
        [SerializeField] private float knockbackForce = 2f;
        [SerializeField] private float knockbackCooldown = 0.2f;
        private float lastKnockbackTime = -10f;
        [SerializeField] private bool isKnockbackImmune = false;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            currentHP = maxHP;
            Debug.Log($"Enemy {name} initialized with HP = {currentHP}, Rigidbody BodyType = {rb.bodyType}, Constraints = {rb.constraints}");
        }

        protected virtual void Start()
        {
            player = GameObject.FindWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError($"Enemy {name} failed to find Player! Ensure a GameObject with tag 'Player' exists in the scene.");
            }
            else
            {
                Debug.Log($"Enemy {name} found Player at {player.position}");
            }
        }

        protected virtual void FixedUpdate()
        {
            if (player == null)
            {
                Debug.LogWarning($"Enemy {name} cannot move: Player is null!");
                return;
            }
            Vector2 dir = (player.position - transform.position).normalized;
            Debug.Log($"Enemy {name} moving: dir = {dir}, moveSpeed = {moveSpeed}, delta = {dir * moveSpeed * Time.fixedDeltaTime}");
            rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
        }

        public virtual void OnDamaged(float amount, Vector2 hitDirection)
        {
            Debug.Log($"Enemy {name} damaged: amount = {amount}, HP before = {currentHP}");
            currentHP -= amount;
            Debug.Log($"Enemy {name} HP after = {currentHP}");
            StartCoroutine(HitFlash());

            if (!isKnockbackImmune && Time.time - lastKnockbackTime >= knockbackCooldown)
            {
                rb.AddForce(hitDirection.normalized * knockbackForce, ForceMode2D.Impulse);
                lastKnockbackTime = Time.time;
                Debug.Log($"Enemy {name} knocked back with force = {knockbackForce}, direction = {hitDirection}");
            }

            if (currentHP <= 0)
            {
                Debug.Log($"Enemy {name} died");
                Die();
            }
        }

        protected virtual void Die()
        {
            gameObject.SetActive(false);
        }

        private System.Collections.IEnumerator HitFlash()
        {
            sprite.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.1f);
            sprite.color = Color.white;
        }
    }
}