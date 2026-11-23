using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    public void Start()
    {
        currentHealth = maxHealth;
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
