using UnityEngine;
using System.Collections;
using Game.Weapon;

public class FirePillar : MonoBehaviour
{
    private float damage;
    private float duration;
    private CircleCollider2D coll;

    private void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        if (coll == null)
        {
            Debug.LogError($"FirePillar {name} is missing CircleCollider2D!");
        }
    }

    public void Init(float damage, float duration)
    {
        this.damage = damage;
        this.duration = duration;
        Debug.Log($"FirePillar Init: damage = {damage}, duration = {duration}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        int enemyLayerIndex = LayerMask.NameToLayer("Enemy");
        if (enemyLayerIndex == -1)
        {
            Debug.LogError("Enemy layer not found! Please define 'Enemy' layer in Tags and Layers settings.");
            return;
        }

        if (other.gameObject.layer == enemyLayerIndex)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                damageable.OnDamaged(damage, Vector2.up);
                Debug.Log($"FirePillar hit enemy at {other.transform.position}, damage = {damage}");
            }
        }
    }

    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}