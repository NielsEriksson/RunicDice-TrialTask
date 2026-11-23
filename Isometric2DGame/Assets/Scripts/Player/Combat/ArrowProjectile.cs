
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed;

    private Vector2 direction;

    public void Launch(Vector2 shootDirection)
    {
        direction = shootDirection.normalized;
        Destroy(gameObject,10f);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
